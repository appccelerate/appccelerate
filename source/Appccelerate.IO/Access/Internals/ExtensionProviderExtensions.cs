//-------------------------------------------------------------------------------
// <copyright file="ExtensionProviderExtensions.cs" company="Appccelerate">
//   Copyright (c) 2008-2013
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace Appccelerate.IO.Access.Internals
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// The extension provider extensions greatly simplify the way how calls are intercepted with extensions.
    /// By using a simple set of conventions the correct extension points are automatically found on the extensions.
    /// The conventions are the following:
    /// <![CDATA[
    /// 1) Methods which return void
    /// i.e. File.Delete(string path) the extension must provide the following three methods:
    /// - BeginDelete(string path)
    /// - EndDelete(string path)
    /// - FailDelete(ref Exception exception)
    /// 2) Methods which return a result
    /// i.e. File.Exists(string path) the extension must provide the following three methods:
    /// - BeginExists(string path)
    /// - EndExists(bool result, string path)
    /// - FailExists(ref Exception exception)]]>
    /// </summary>
    public static class ExtensionProviderExtensions
    {
        internal const string BeginMethodPrefix = "Begin";

        internal const string EndMethodPrefix = "End";

        internal const string FailMethodPrefix = "Fail";

        private const string UnableToDetermineBeginEndOrFailMethodMessage = "Unable to determine Begin-, End- or Fail Method! Please check that you are using proper conventions.";

        private static readonly ConcurrentDictionary<Key, Item> ReflectionCache = new ConcurrentDictionary<Key, Item>();

        /// <summary>
        /// Surrounds the specified function expression with extension methods following a certain convention.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="provider">The provider.</param>
        /// <param name="function">The function expression.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The result of the function expression.</returns>
        public static TResult SurroundWithExtension<TExtension, TResult>(this IExtensionProvider<TExtension> provider, Expression<Func<TResult>> function, params object[] args)
            where TExtension : class
        {
            return SurroundWithExtensionInternal<TExtension, TResult>(provider, function, args);
        }

        /// <summary>
        /// Surrounds the specified action expression with extension methods following a certain convention.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <param name="provider">The provider.</param>
        /// <param name="action">The action expression.</param>
        /// <param name="args">The arguments.</param>
        public static void SurroundWithExtension<TExtension>(this IExtensionProvider<TExtension> provider, Expression<Action> action, params object[] args)
            where TExtension : class
        {
            SurroundWithExtensionInternal<TExtension, Missing>(provider, action, args);
        }

        /// <summary>
        /// Clears the reflection cache.
        /// </summary>
        public static void ClearCache()
        {
            ReflectionCache.Clear();
        }

        private static TReturn SurroundWithExtensionInternal<TExtension, TReturn>(this IExtensionProvider<TExtension> provider, LambdaExpression expression, params object[] args)
             where TExtension : class
        {
            var callExpression = (MethodCallExpression)expression.Body;
            MethodInfo callMethodInfo = callExpression.Method;
            bool hasReturn = typeof(TReturn) != typeof(Missing);
            string methodName = callMethodInfo.Name;

            var key = new Key(callMethodInfo.MethodHandle, callMethodInfo.Name, callMethodInfo.GetParameterTypes().ToArray());

            Item item = !ReflectionCache.ContainsKey(key) ? CacheItem<TExtension>(methodName, key, hasReturn, typeof(TReturn)) : ReflectionCache[key];

            object result;

            try
            {
                InvokeBeginExtensions(item, provider, args);

                result = expression.Compile().DynamicInvoke();

                object[] arguments = GetArguments(hasReturn, args, result);

                InvokeEndExtensions(item, provider, arguments);
            }
            catch (TargetInvocationException exception)
            {
                Exception innerException = exception.InnerException;
                innerException.PreserveStackTrace();

                InvokeFailExtensions(item, provider, ref innerException);

                throw innerException;
            }

            return (TReturn)result;
        }

        private static object[] GetArguments(bool hasReturn, object[] args, object result)
        {
            var arguments = args;
            if (hasReturn)
            {
                arguments = new List<object>(args)
                    .Prepend(result)
                    .ToArray();
            }

            return arguments;
        }

        private static Item CacheItem<TExtension>(string methodName, Key key, bool hasReturn, Type returnType)
        {
            string beginMethodName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", BeginMethodPrefix, methodName);
            string endMethodName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", EndMethodPrefix, methodName);
            string failMethodName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", FailMethodPrefix, methodName);

            IEnumerable<MethodInfo> candidates = GetMethodCandidates<TExtension>(beginMethodName, endMethodName, failMethodName);

            IEnumerable<Type> endMethodParameterTypes = GetEndMethodParameterTypes(key, hasReturn, returnType);

            MethodInfo beginMethodInfo, endMethodInfo, failMethodInfo;
            try
            {
                beginMethodInfo = candidates.Single(c => c.Name.Equals(beginMethodName, StringComparison.Ordinal) && c.GetParameterTypes().SequenceEqual(key.TypeArguments));
                endMethodInfo = candidates.Single(c => c.Name.Equals(endMethodName, StringComparison.Ordinal) && c.GetParameterTypes().SequenceEqual(endMethodParameterTypes));
                failMethodInfo = candidates.Single(c => c.Name.Equals(failMethodName, StringComparison.Ordinal));
            }
            catch (ArgumentException exception)
            {
                throw new InvalidOperationException(UnableToDetermineBeginEndOrFailMethodMessage, exception);
            }
            catch (InvalidOperationException exception)
            {
                throw new InvalidOperationException(UnableToDetermineBeginEndOrFailMethodMessage, exception);
            }

            Item item = new Item(beginMethodInfo.MethodHandle, endMethodInfo.MethodHandle, failMethodInfo.MethodHandle);

            ReflectionCache.AddOrUpdate(key, item, (k, i) => item);
            return item;
        }

        private static IEnumerable<Type> GetEndMethodParameterTypes(Key key, bool hasReturn, Type returnType)
        {
            IEnumerable<Type> endMethodParameterTypes = key.TypeArguments;

            if (hasReturn)
            {
                endMethodParameterTypes = endMethodParameterTypes.Prepend(returnType);
            }

            return endMethodParameterTypes;
        }

        private static IEnumerable<MethodInfo> GetMethodCandidates<TExtension>(string beginMethodName, string endMethodName, string failMethodName)
        {
            return from method in typeof(TExtension).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                   let n = method.Name
                   where n.Equals(beginMethodName, StringComparison.Ordinal) ||
                         n.Equals(endMethodName, StringComparison.Ordinal) ||
                         n.Equals(failMethodName, StringComparison.Ordinal)
                   select method;
        }

        private static void InvokeFailExtensions<TExtension>(Item item, IExtensionProvider<TExtension> provider, ref Exception exception)
                         where TExtension : class
        {
            var args = new[] { exception };

            foreach (TExtension extension in provider.Extensions)
            {
                item.Fail.Invoke(extension, !exception.Equals(args.Single()) ? new[] { exception } : args);
            }

            exception = args.Single();
        }

        private static void InvokeEndExtensions<TExtension>(Item item, IExtensionProvider<TExtension> provider, object[] args)
                         where TExtension : class
        {
            foreach (TExtension extension in provider.Extensions)
            {
                item.End.Invoke(extension, args);
            }
        }

        private static void InvokeBeginExtensions<TExtension>(Item item, IExtensionProvider<TExtension> provider, object[] args)
                         where TExtension : class
        {
            foreach (TExtension extension in provider.Extensions)
            {
                item.Begin.Invoke(extension, args);
            }
        }

        private static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
        {
            yield return item;

            foreach (T i in source)
            {
                yield return i;
            }
        }

        private static IEnumerable<Type> GetParameterTypes(this MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Select(p => p.ParameterType);
        }

        private class Item
        {
            private readonly RuntimeMethodHandle begin;

            private readonly RuntimeMethodHandle end;

            private readonly RuntimeMethodHandle fail;

            public Item(RuntimeMethodHandle begin, RuntimeMethodHandle end, RuntimeMethodHandle fail)
            {
                this.begin = begin;
                this.end = end;
                this.fail = fail;
            }

            public MethodBase Begin
            {
                get
                {
                    return MethodBase.GetMethodFromHandle(this.begin);
                }
            }

            public MethodBase End
            {
                get
                {
                    return MethodBase.GetMethodFromHandle(this.end);
                }
            }

            public MethodBase Fail
            {
                get
                {
                    return MethodBase.GetMethodFromHandle(this.fail);
                }
            }
        }

        private class Key
        {
            public Key(RuntimeMethodHandle th, string methodName, Type[] args)
            {
                this.InstanceHandle = th;
                this.MethodName = methodName;
                this.TypeArguments = args;
            }

            public Type[] TypeArguments { get; private set; }

            public string MethodName { get; private set; }

            public RuntimeMethodHandle InstanceHandle { get; private set; }

            public bool Equals(Key other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return object.Equals(other.MethodName, this.MethodName) && other.InstanceHandle.Equals(this.InstanceHandle);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != typeof(Key))
                {
                    return false;
                }

                return this.Equals((Key)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int result = this.MethodName.GetHashCode();
                    result = (result * 397) ^ this.InstanceHandle.GetHashCode();
                    return result;
                }
            }
        }
    }
}
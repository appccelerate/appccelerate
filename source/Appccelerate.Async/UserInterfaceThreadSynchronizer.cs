//-------------------------------------------------------------------------------
// <copyright file="UserInterfaceThreadSynchronizer.cs" company="Appccelerate">
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

namespace Appccelerate.Async
{
    using System;
    using System.Threading;

    /// <summary>
    /// Executes operations on the user interface thread.
    /// </summary>
    public class UserInterfaceThreadSynchronizer : IUserInterfaceThreadSynchronizer
    {
        /// <summary>
        /// The synchronization context used to switch threads.
        /// </summary>
        private readonly SynchronizationContext synchronizationContext;

        private readonly IUserInterfaceThreadSynchronizerLogExtension logExtension;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceThreadSynchronizer"/> class.
        /// </summary>
        /// <param name="synchronizationContext">The synchronization context used for synchronization.</param>
        /// <param name="logExtension">The log extension used to log synchronization events.</param>
        public UserInterfaceThreadSynchronizer(SynchronizationContext synchronizationContext, IUserInterfaceThreadSynchronizerLogExtension logExtension)
        {
            this.synchronizationContext = synchronizationContext;
            this.logExtension = logExtension;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceThreadSynchronizer"/> class
        /// with no logging.
        /// </summary>
        /// <param name="synchronizationContext">The synchronization context used for synchronization.</param>
        public UserInterfaceThreadSynchronizer(SynchronizationContext synchronizationContext)
            : this(synchronizationContext, new EmptyLogExtension())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceThreadSynchronizer"/> class
        /// with synchronization context of current thread and no logging.
        /// </summary>
        public UserInterfaceThreadSynchronizer()
            : this(SynchronizationContext.Current)
        {
        }

        /// <summary>
        /// Executes the specified action on the user interface thread.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Execute(Action action)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Send(
                state =>
                    {
                        action();

                        this.logExtension.LogSynchronous(action, threadId, threadName);
                    },
                null);
        }

        /// <summary>
        /// Executes the specified action on the user interface thread.
        /// </summary>
        /// <typeparam name="T">Type of the method parameter.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value">The value passed to the action as method parameter.</param>
        public void Execute<T>(Action<T> action, T value)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Send(
                state =>
                    {
                        action(value);

                        this.logExtension.LogSynchronous(action, threadId, threadName);
                    }, 
                null);
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <typeparam name="TParameter1">The type of the 1.</typeparam>
        /// <typeparam name="TParameter2">The type of the 2.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        public void Execute<TParameter1, TParameter2>(Action<TParameter1, TParameter2> action, TParameter1 value1, TParameter2 value2)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Send(
                state =>
                    {
                        action(value1, value2);

                        this.logExtension.LogSynchronous(action, threadId, threadName);
                    }, 
                null);
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <typeparam name="TParameter1">The type of the 1.</typeparam>
        /// <typeparam name="TParameter2">The type of the 2.</typeparam>
        /// <typeparam name="TParameter3">The type of the 3.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <param name="value3">The value3.</param>
        public void Execute<TParameter1, TParameter2, TParameter3>(Action<TParameter1, TParameter2, TParameter3> action, TParameter1 value1, TParameter2 value2, TParameter3 value3)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Send(
                state =>
                    {
                        action(value1, value2, value3);

                        this.logExtension.LogSynchronous(action, threadId, threadName);
                    }, 
                null);
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <typeparam name="TParameter1">The type of the 1.</typeparam>
        /// <typeparam name="TParameter2">The type of the 2.</typeparam>
        /// <typeparam name="TParameter3">The type of the 3.</typeparam>
        /// <typeparam name="TParameter4">The type of the 4.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <param name="value3">The value3.</param>
        /// <param name="value4">The value4.</param>
        public void Execute<TParameter1, TParameter2, TParameter3, TParameter4>(Action<TParameter1, TParameter2, TParameter3, TParameter4> action, TParameter1 value1, TParameter2 value2, TParameter3 value3, TParameter4 value4)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Send(
                state =>
                    {
                        action(value1, value2, value3, value4);

                        this.logExtension.LogSynchronous(action, threadId, threadName);
                    }, 
                null);
        }

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <param name="action">The action.</param>
        public void ExecuteAsync(Action action)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Post(
                state =>
                    {
                        action();

                        this.logExtension.LogAsynchronous(action, threadId, threadName);
                    }, 
                null);
        }

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the method parameter.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value">The value passed to the action as method parameter.</param>
        public void ExecuteAsync<T>(Action<T> action, T value)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Post(
                state =>
                    {
                        action(value);

                        this.logExtension.LogAsynchronous(action, threadId, threadName);
                    }, 
                null);
        }

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <typeparam name="TParameter2">Type of the second method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <param name="value2">The second method parameter.</param>
        public void ExecuteAsync<TParameter1, TParameter2>(Action<TParameter1, TParameter2> action, TParameter1 value1, TParameter2 value2)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Post(
                state =>
                    {
                        action(value1, value2);

                        this.logExtension.LogAsynchronous(action, threadId, threadName);
                    }, 
                null);
        }

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <typeparam name="TParameter2">Type of the second method parameter</typeparam>
        /// <typeparam name="TParameter3">Type of the third method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <param name="value2">The second method parameter.</param>
        /// <param name="value3">The third method parameter.</param>
        public void ExecuteAsync<TParameter1, TParameter2, TParameter3>(Action<TParameter1, TParameter2, TParameter3> action, TParameter1 value1, TParameter2 value2, TParameter3 value3)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Post(
                state =>
                    {
                        action(value1, value2, value3);

                        this.logExtension.LogAsynchronous(action, threadId, threadName);
                    }, 
                null);
        }

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <typeparam name="TParameter2">Type of the second method parameter</typeparam>
        /// <typeparam name="TParameter3">Type of the third method parameter</typeparam>
        /// <typeparam name="TParameter4">Type of the fourth method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <param name="value2">The second method parameter.</param>
        /// <param name="value3">The third method parameter.</param>
        /// <param name="value4">The fourth method parameter.</param>
        public void ExecuteAsync<TParameter1, TParameter2, TParameter3, TParameter4>(Action<TParameter1, TParameter2, TParameter3, TParameter4> action, TParameter1 value1, TParameter2 value2, TParameter3 value3, TParameter4 value4)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            this.synchronizationContext.Post(
                state =>
                    {
                        action(value1, value2, value3, value4);

                        this.logExtension.LogAsynchronous(action, threadId, threadName);
                    }, 
                null);
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The result of the action.</returns>
        public TResult Execute<TResult>(Func<TResult> action)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            TResult result = default(TResult);
            this.synchronizationContext.Send(
                state =>
                    {
                        result = action();

                        this.logExtension.LogSynchronous(action, threadId, threadName, result);
                    }, 
                null);

            return result;
        }

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <returns>The result of the action.</returns>
        public TResult Execute<TResult, TParameter1>(Func<TParameter1, TResult> action, TParameter1 value1)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            TResult result = default(TResult);
            this.synchronizationContext.Send(
                state =>
                    {
                        result = action(value1);

                        this.logExtension.LogSynchronous(action, threadId, threadName, result);
                    }, 
                null);

            return result;
        }

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <typeparam name="TParameter2">Type of the second method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <param name="value2">The second method parameter.</param>
        /// <returns>The result of the action.</returns>
        public TResult Execute<TResult, TParameter1, TParameter2>(Func<TParameter1, TParameter2, TResult> action, TParameter1 value1, TParameter2 value2)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            TResult result = default(TResult);
            this.synchronizationContext.Send(
                state =>
                    {
                        result = action(value1, value2);

                        this.logExtension.LogSynchronous(action, threadId, threadName, result);
                    }, 
                null);

            return result;
        }

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <typeparam name="TParameter2">Type of the second method parameter</typeparam>
        /// <typeparam name="TParameter3">Type of the third method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <param name="value2">The second method parameter.</param>
        /// <param name="value3">The third method parameter.</param>
        /// <returns>The result of the action.</returns>
        public TResult Execute<TResult, TParameter1, TParameter2, TParameter3>(Func<TParameter1, TParameter2, TParameter3, TResult> action, TParameter1 value1, TParameter2 value2, TParameter3 value3)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            TResult result = default(TResult);
            this.synchronizationContext.Send(
                state =>
                    {
                        result = action(value1, value2, value3);

                        this.logExtension.LogSynchronous(action, threadId, threadName, result);
                    }, 
                null);

            return result;
        }

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <typeparam name="TParameter2">Type of the second method parameter</typeparam>
        /// <typeparam name="TParameter3">Type of the third method parameter</typeparam>
        /// <typeparam name="TParameter4">Type of the fourth method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <param name="value2">The second method parameter.</param>
        /// <param name="value3">The third method parameter.</param>
        /// <param name="value4">The fourth method parameter.</param>
        /// <returns>The result of the action.</returns>
        public TResult Execute<TResult, TParameter1, TParameter2, TParameter3, TParameter4>(Func<TParameter1, TParameter2, TParameter3, TParameter4, TResult> action, TParameter1 value1, TParameter2 value2, TParameter3 value3, TParameter4 value4)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string threadName = Thread.CurrentThread.Name;

            TResult result = default(TResult);
            this.synchronizationContext.Send(
                state =>
                    {
                        result = action(value1, value2, value3, value4);

                        this.logExtension.LogSynchronous(action, threadId, threadName, result);
                    }, 
                null);

            return result;
        }

        private class EmptyLogExtension : IUserInterfaceThreadSynchronizerLogExtension
        {
            public void LogSynchronous(Delegate action, int threadId, string threadName)
            {
            }

            public void LogSynchronous<TResult>(Delegate action, int threadId, string threadName, TResult result)
            {
            }

            public void LogAsynchronous(Delegate action, int threadId, string threadName)
            {
            }
        }
    }
}
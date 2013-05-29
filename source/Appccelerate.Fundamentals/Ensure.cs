//-------------------------------------------------------------------------------
// <copyright file="Ensure.cs" company="Appccelerate">
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

namespace Appccelerate
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Provides several methods for validating arguments and throwing appropriate exceptions.
    /// </summary>
    [DebuggerStepThrough]
    public static class Ensure
    {
        private const string ArgumentMustBeAssignable = "The provided argument must be assignable to the type {0}.";
        private const string NumberMustNotBeNegative = "The provided number argument must not be negative.";
        private const string StringMustNotBeEmpty = "The provided string argument must not be empty.";
        private const string TypeMustBeAssignable = "The provided type {0} must be assignable to the type {1}.";

        /// <summary>
        /// Verifies the <paramref name="argumentValue"/> is not <c>null</c> and throws an <see cref="ArgumentNullException"/> if it is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="argumentValue"/> to verify.</typeparam>
        /// <param name="argumentValue">The value to verify.</param>
        /// <param name="argumentName">The name of the <paramref name="argumentValue"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="argumentValue"/> parameter is <c>null</c>.</exception>
        public static void ArgumentNotNull<T>([ValidatedNotNull] T argumentValue, string argumentName)
            where T : class
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Verifies the <paramref name="argumentValue"/> is not <c>null</c> or an empty string and throws an <see cref="ArgumentNullException"/> if
        /// it is <c>null</c> or an <see cref="ArgumentException"/> if it is an empty string.
        /// </summary>
        /// <param name="argumentValue">The value to verify.</param>
        /// <param name="argumentName">The name of the <paramref name="argumentValue"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="argumentValue"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="argumentValue"/> parameter is an empty string.</exception>
        public static void ArgumentNotNullOrEmpty([ValidatedNotNull] string argumentValue, string argumentName)
        {
            Ensure.ArgumentNotNull(argumentValue, argumentName);
            if (argumentValue.Length == 0)
            {
                throw new ArgumentException(Ensure.StringMustNotBeEmpty, argumentName);
            }
        }

        /// <summary>
        /// Verifies the <paramref name="argumentValue"/> is not a negative number and throws an <see cref="ArgumentOutOfRangeException"/> if it is
        /// a negative number.
        /// </summary>
        /// <param name="argumentValue">The value to verify.</param>
        /// <param name="argumentName">The name of the <paramref name="argumentValue"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="argumentValue"/> parameter is a negative number.</exception>
        public static void ArgumentNotNegative(int argumentValue, string argumentName)
        {
            if (argumentValue < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName, argumentValue, Ensure.NumberMustNotBeNegative);
            }
        }

        /// <summary>
        /// Verifies the type of <paramref name="argumentValue"/> is assignable to the <paramref name="targetType"/> (meaning interfaces are
        /// implemented, or classes exist in the base class hierarchy) and throws an <see cref="ArgumentException"/> if it is not assignable or
        /// a <see cref="ArgumentNullException"/> if the <paramref name="targetType"/> is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="argumentValue"/> to verify.</typeparam>
        /// <param name="targetType">The target type that will be assigned to.</param>
        /// <param name="argumentValue">The value of the argument being assigned.</param>
        /// <param name="argumentName">The name of the <paramref name="argumentValue"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="targetType"/> parameter is <c>null</c>.
        /// - or - The <paramref name="argumentValue"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The type of <paramref name="argumentValue"/> parameter is not assignable to the <paramref name="targetType"/>.</exception>
        public static void ArgumentAssignableFrom<T>(Type targetType, [ValidatedNotNull] T argumentValue, string argumentName)
            where T : class
        {
            Ensure.ArgumentNotNull(targetType, "targetType");
            Ensure.ArgumentNotNull(argumentValue, argumentName);

            if (!targetType.GetTypeInfo().IsAssignableFrom(argumentValue.GetType().GetTypeInfo()))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Ensure.ArgumentMustBeAssignable, targetType), argumentName);
            }
        }

        /// <summary>
        /// Verifies the <paramref name="argumentValue"/> is assignable to the <paramref name="targetType"/> (meaning interfaces are implemented, 
        /// or classes exist in the base class hierarchy) and throws an <see cref="ArgumentException"/> if it is not assignable or
        /// a <see cref="ArgumentNullException"/> if the <paramref name="targetType"/> is <c>null</c>.
        /// </summary>
        /// <param name="targetType">The target type that will be assigned to.</param>
        /// <param name="argumentValue">The type of the argument being assigned.</param>
        /// <param name="argumentName">The name of the <paramref name="argumentValue"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="targetType"/> parameter is <c>null</c>.
        /// - or - The <paramref name="argumentValue"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="argumentValue"/> is not assignable to the <paramref name="targetType"/>.</exception>
        public static void ArgumentTypeAssignableFrom(Type targetType, [ValidatedNotNull] Type argumentValue, string argumentName)
        {
            Ensure.ArgumentNotNull(targetType, "targetType");
            Ensure.ArgumentNotNull(argumentValue, argumentName);

            if (!targetType.GetTypeInfo().IsAssignableFrom(argumentValue.GetTypeInfo()))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Ensure.TypeMustBeAssignable, argumentValue, targetType), argumentName);
            }
        }

        /// <summary>
        /// Verifies the <paramref name="condition"/> evaluates to <c>true</c> and throws an <see cref="ArgumentOutOfRangeException"/> if the <paramref name="condition"/>
        /// evaluates to <c>false</c>.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">The message used for the <see cref="ArgumentOutOfRangeException"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="message"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="message"/> parameter is an empty string.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="condition"/> evaluates to <c>false</c>.</exception>
        public static void ArgumentInRange(bool condition, string message)
        {
            Ensure.ArgumentNotNullOrEmpty(message, "message");

            if (!condition)
            {
                throw new ArgumentOutOfRangeException(string.Empty, message);
            }
        }

        /// <summary>
        /// Verifies the <paramref name="condition"/> evaluates to <c>true</c> for the <paramref name="argumentValue"/> and throws an <see cref="ArgumentOutOfRangeException"/>
        /// if the <paramref name="condition"/> evaluates to <c>false</c>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="argumentValue"/> to verify.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="argumentValue">The value to verify.</param>
        /// <param name="argumentName">The name of the <paramref name="argumentValue"/>.</param>
        /// <param name="message">The message used for the <see cref="ArgumentOutOfRangeException"/>. The message is being formatted using
        /// the <see cref="CultureInfo.InvariantCulture"/>. The <paramref name="argumentValue"/> is passed as first parameter and
        /// the <paramref name="argumentName"/> as second parameter to the formatting method.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="message"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="message"/> parameter is an empty string.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="condition"/> evaluates to <c>false</c>.</exception>
        public static void ArgumentInRange<T>(bool condition, T argumentValue, string argumentName, string message)
        {
            Ensure.ArgumentNotNullOrEmpty(message, "message");

            if (!condition)
            {
                throw new ArgumentOutOfRangeException(argumentName, argumentValue, string.Format(CultureInfo.InvariantCulture, message, argumentValue, argumentName));
            }
        }

        /// <summary>
        /// Verifies the <paramref name="condition"/> evaluates to <c>true</c> and throws an <see cref="ArgumentException"/> if the <paramref name="condition"/>
        /// evaluates to <c>false</c>.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">The message used for the <see cref="ArgumentException"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="message"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="message"/> parameter is an empty string.
        /// - or - The <paramref name="condition"/> evaluates to <c>false</c>.</exception>
        public static void ArgumentMatches(bool condition, string message)
        {
            Ensure.ArgumentNotNullOrEmpty(message, "message");

            if (!condition)
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Verifies the <paramref name="condition"/> evaluates to <c>true</c> for the <paramref name="argumentValue"/> and throws an <see cref="ArgumentException"/>
        /// if the <paramref name="condition"/> evaluates to <c>false</c>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="argumentValue"/> to verify.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="argumentValue">The value to verify.</param>
        /// <param name="argumentName">The name of the <paramref name="argumentValue"/>.</param>
        /// <param name="message">The message used for the <see cref="ArgumentException"/>. The message is being formatted using
        /// the <see cref="CultureInfo.InvariantCulture"/>. The <paramref name="argumentValue"/> is passed as first parameter and
        /// the <paramref name="argumentName"/> as second parameter to the formatting method.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="message"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="message"/> parameter is an empty string.
        /// - or - The <paramref name="condition"/> evaluates to <c>false</c>.</exception>
        public static void ArgumentMatches<T>(bool condition, T argumentValue, string argumentName, string message)
        {
            Ensure.ArgumentNotNullOrEmpty(message, "message");

            if (!condition)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, message, argumentValue, argumentName), argumentName);
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message used for the <see cref="InvalidOperationException"/>.</param>
        /// <param name="arguments">The arguments used to format the <paramref name="message"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="message"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="message"/> parameter is an empty string.</exception>
        /// <exception cref="InvalidOperationException">This exception is always thrown.</exception>
        public static void OperationNotValid(string message, params object[] arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(message, "message");
            Ensure.OperationValid(false, message, arguments);
        }

        /// <summary>
        /// Verifies the <paramref name="condition"/> evaluates to <c>true</c> and throws an <see cref="InvalidOperationException"/> if the <paramref name="condition"/>
        /// evaluates to <c>false</c>.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">The message used for the <see cref="InvalidOperationException"/>.</param>
        /// <param name="arguments">The arguments used to format the <paramref name="message"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="message"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="message"/> parameter is an empty string.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="condition"/> evaluates to <c>false</c>.</exception>
        public static void OperationValid(bool condition, string message, params object[] arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(message, "message");

            if (!condition)
            {
                // Format message if required
                if ((arguments != null) && (arguments.Length > 0))
                {
                    message = string.Format(CultureInfo.InvariantCulture, message, arguments);
                }

                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Throws an <see cref="NotSupportedException"/> with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message used for the <see cref="NotSupportedException"/>.</param>
        /// <param name="arguments">The arguments used to format the <paramref name="message"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="message"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="message"/> parameter is an empty string.</exception>
        /// <exception cref="NotSupportedException">This exception is always thrown.</exception>
        public static void OperationNotSupported(string message, params object[] arguments)
        {
            Ensure.OperationSupported(false, message, arguments);
        }

        /// <summary>
        /// Verifies the <paramref name="condition"/> evaluates to <c>true</c> and throws an <see cref="NotSupportedException"/> if the <paramref name="condition"/>
        /// evaluates to <c>false</c>.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">The message used for the <see cref="NotSupportedException"/>.</param>
        /// <param name="arguments">The arguments used to format the <paramref name="message"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="message"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="message"/> parameter is an empty string.</exception>
        /// <exception cref="NotSupportedException">The <paramref name="condition"/> evaluates to <c>false</c>.</exception>
        public static void OperationSupported(bool condition, string message, params object[] arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(message, "message");

            if (!condition)
            {
                // Format message if required
                if ((arguments != null) && (arguments.Length > 0))
                {
                    message = string.Format(CultureInfo.InvariantCulture, message, arguments);
                }

                throw new NotSupportedException(message);
            }
        }
    }
}
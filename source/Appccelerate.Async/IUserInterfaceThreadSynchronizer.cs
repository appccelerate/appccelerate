//-------------------------------------------------------------------------------
// <copyright file="IUserInterfaceThreadSynchronizer.cs" company="Appccelerate">
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

    /// <summary>
    /// Provides functionality to execute operations on the user interface thread.
    /// </summary>
    public interface IUserInterfaceThreadSynchronizer
    {
        /// <summary>
        /// Executes the specified action on the user interface thread.
        /// </summary>
        /// <param name="action">The action.</param>
        void Execute(Action action);

        /// <summary>
        /// Executes the specified action on the user interface thread.
        /// </summary>
        /// <typeparam name="T">Type of the method parameter.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value">The value passed to the action as method parameter.</param>
        void Execute<T>(Action<T> action, T value);

        /// <summary>
        /// Executes the specified action on the user interface thread.
        /// </summary>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <typeparam name="TParameter2">Type of the second method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <param name="value2">The second method parameter.</param>
        void Execute<TParameter1, TParameter2>(Action<TParameter1, TParameter2> action, TParameter1 value1, TParameter2 value2);

        /// <summary>
        /// Executes the specified action on the user interface thread.
        /// </summary>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <typeparam name="TParameter2">Type of the second method parameter</typeparam>
        /// <typeparam name="TParameter3">Type of the third method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <param name="value2">The second method parameter.</param>
        /// <param name="value3">The third method parameter.</param>
        void Execute<TParameter1, TParameter2, TParameter3>(Action<TParameter1, TParameter2, TParameter3> action, TParameter1 value1, TParameter2 value2, TParameter3 value3);

        /// <summary>
        /// Executes the specified action on the user interface thread.
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
        void Execute<TParameter1, TParameter2, TParameter3, TParameter4>(Action<TParameter1, TParameter2, TParameter3, TParameter4> action, TParameter1 value1, TParameter2 value2, TParameter3 value3, TParameter4 value4);

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <param name="action">The action.</param>
        void ExecuteAsync(Action action);

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the method parameter.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value">The value passed to the action as method parameter.</param>
        void ExecuteAsync<T>(Action<T> action, T value);

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <typeparam name="TParameter2">Type of the second method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <param name="value2">The second method parameter.</param>
        void ExecuteAsync<TParameter1, TParameter2>(Action<TParameter1, TParameter2> action, TParameter1 value1, TParameter2 value2);

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
        void ExecuteAsync<TParameter1, TParameter2, TParameter3>(Action<TParameter1, TParameter2, TParameter3> action, TParameter1 value1, TParameter2 value2, TParameter3 value3);

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
        void ExecuteAsync<TParameter1, TParameter2, TParameter3, TParameter4>(Action<TParameter1, TParameter2, TParameter3, TParameter4> action, TParameter1 value1, TParameter2 value2, TParameter3 value3, TParameter4 value4);

        /// <summary>
        /// Executes the specified action on the user interface thread.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The result of the action.</returns>
        TResult Execute<TResult>(Func<TResult> action);

        /// <summary>
        /// Executes the specified action on the user interface thread asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TParameter1">Type of the first method parameter</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="value1">The first method parameter.</param>
        /// <returns>The result of the action.</returns>
        TResult Execute<TResult, TParameter1>(Func<TParameter1, TResult> action, TParameter1 value1);

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
        TResult Execute<TResult, TParameter1, TParameter2>(Func<TParameter1, TParameter2, TResult> action, TParameter1 value1, TParameter2 value2);

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
        TResult Execute<TResult, TParameter1, TParameter2, TParameter3>(Func<TParameter1, TParameter2, TParameter3, TResult> action, TParameter1 value1, TParameter2 value2, TParameter3 value3);

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
        TResult Execute<TResult, TParameter1, TParameter2, TParameter3, TParameter4>(Func<TParameter1, TParameter2, TParameter3, TParameter4, TResult> action, TParameter1 value1, TParameter2 value2, TParameter3 value3, TParameter4 value4);
    }
}
//-------------------------------------------------------------------------------
// <copyright file="IUserInterfaceThreadSynchronizerLogExtension.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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
    /// Logs synchronization events in user interface thread synchronizer.
    /// </summary>
    public interface IUserInterfaceThreadSynchronizerLogExtension
    {
        /// <summary>
        /// Logs a synchronous operation.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="threadId">The source thread id.</param>
        /// <param name="threadName">Name of the source thread.</param>
        void LogSynchronous(Delegate action, int threadId, string threadName);

        /// <summary>
        /// Logs the synchronous operation with result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="threadId">The source thread id.</param>
        /// <param name="threadName">Name of the source thread.</param>
        /// <param name="result">The result.</param>
        void LogSynchronous<TResult>(Delegate action, int threadId, string threadName, TResult result);

        /// <summary>
        /// Logs the asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="threadId">The source thread id.</param>
        /// <param name="threadName">Name of the source thread.</param>
        void LogAsynchronous(Delegate action, int threadId, string threadName);
    }
}
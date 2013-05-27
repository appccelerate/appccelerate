//-------------------------------------------------------------------------------
// <copyright file="UserInterfaceThreadSynchronizerLogExtension.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;

    using Appccelerate.Async;

    using log4net;

    /// <summary>
    /// Log extension for <see cref="UserInterfaceThreadSynchronizer"/> that uses log4net.
    /// </summary>
    public class UserInterfaceThreadSynchronizerLogExtension : IUserInterfaceThreadSynchronizerLogExtension
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceThreadSynchronizerLogExtension"/> class.
        /// </summary>
        public UserInterfaceThreadSynchronizerLogExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceThreadSynchronizerLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UserInterfaceThreadSynchronizerLogExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceThreadSynchronizerLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UserInterfaceThreadSynchronizerLogExtension(ILog logger)
        {
            this.log = logger;
        }

        /// <summary>
        /// Logs a synchronous operation.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="threadId">The source thread id.</param>
        /// <param name="threadName">Name of the source thread.</param>
        public void LogSynchronous(Delegate action, int threadId, string threadName)
        {
            this.Log(
                "{0} executed synchronous thread switch from thread {1}:{2} to {3}:{4}. Operation {5}.{6}",
                threadId,
                threadName,
                action);
        }

        /// <summary>
        /// Logs the synchronous operation with result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="threadId">The source thread id.</param>
        /// <param name="threadName">Name of the source thread.</param>
        /// <param name="result">The result.</param>
        public void LogSynchronous<TResult>(Delegate action, int threadId, string threadName, TResult result)
        {
            this.log.DebugFormat(
                CultureInfo.InvariantCulture,
                "{0} executed synchronous thread switch from thread {1}:{2} to {3}:{4}. Operation {5}.{6} with result {7}",
                this,
                threadId,
                threadName,
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.Name,
                action.Method.DeclaringType.FullName,
                action.Method.Name,
                result);
        }

        /// <summary>
        /// Logs the asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="threadId">The source thread id.</param>
        /// <param name="threadName">Name of the source thread.</param>
        public void LogAsynchronous(Delegate action, int threadId, string threadName)
        {
            this.Log(
                "{0} executed asynchronous thread switch from thread {1}:{2} to {3}:{4}. Operation {5}.{6}",
                threadId,
                threadName,
                action);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="threadId">The source thread id.</param>
        /// <param name="threadName">Name of the source thread.</param>
        /// <param name="action">The action.</param>
        private void Log(string message, int threadId, string threadName, Delegate action)
        {
            this.log.DebugFormat(
                CultureInfo.InvariantCulture,
                message,
                this,
                threadId,
                threadName,
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.Name,
                action.Method.DeclaringType.FullName,
                action.Method.Name);
        }
    }
}
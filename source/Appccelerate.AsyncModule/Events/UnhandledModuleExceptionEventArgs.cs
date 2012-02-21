//-------------------------------------------------------------------------------
// <copyright file="UnhandledModuleExceptionEventArgs.cs" company="Appccelerate">
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

namespace Appccelerate.AsyncModule.Events
{
    using System;

    /// <summary>
    /// The event arguments of the UnhandledExceptionOccurred event 
    /// of the module coordinator.
    /// </summary>
    public class UnhandledModuleExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledModuleExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="module">The module, where the unhandled exception occurred.</param>
        /// <param name="message">The message causing the exception. Null if not a message is causing the exception.</param>
        /// <param name="unhandledException">The unhandled exception.</param>
        public UnhandledModuleExceptionEventArgs(object module, object message, Exception unhandledException)
        {
            this.Module = module;
            this.UnhandledException = unhandledException;
            this.Message = message;
        }

        /// <summary>
        /// Gets the module, where the unhandled exception occurred.
        /// </summary>
        public object Module
        {
            get; private set;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception UnhandledException
        {
            get; private set;
        }

        /// <summary>
        /// Gets the message causing the exception. Null if not a message is causing the exception.
        /// </summary>
        public object Message
        {
            get; private set;
        }
    }
}

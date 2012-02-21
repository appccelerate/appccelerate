//-------------------------------------------------------------------------------
// <copyright file="ConsumeMessageExceptionEventArgs.cs" company="Appccelerate">
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
    /// The event arguments of the module controller event
    /// ConsumeMessageExceptionOccurred.
    /// </summary>
    public class ConsumeMessageExceptionEventArgs : ConsumeMessageEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumeMessageExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="message">The message causing the exception.</param>
        /// <param name="exception">The exception.</param>
        public ConsumeMessageExceptionEventArgs(object module, object message, Exception exception) : base(module, message)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// Gets the exception, which occured during the call to the 
        /// message consumer.
        /// </summary>
        public Exception Exception
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the extension 
        /// has handled the exception and no further action of the 
        /// module controller is necessary.
        /// </summary>
        public bool ExceptionHandled
        {
            get; set;
        }
    }
}

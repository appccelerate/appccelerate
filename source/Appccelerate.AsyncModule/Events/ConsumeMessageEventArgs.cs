//-------------------------------------------------------------------------------
// <copyright file="ConsumeMessageEventArgs.cs" company="Appccelerate">
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
    /// The event arguments of the module controller events: 
    /// BeforeConsumeMessage and AfterConsumeMessage.
    /// </summary>
    public class ConsumeMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumeMessageEventArgs"/> class.
        /// </summary>
        /// <param name="module">The module, which consumes the message.</param>
        /// <param name="message">The message to be consumed.</param>
        public ConsumeMessageEventArgs(object module, object message)
        {
            this.Module = module;
            this.Message = message;
        }

        /// <summary>
        /// Gets the module, which consumes the message.
        /// </summary>
        public object Module
        {
            get; private set;
        }

        /// <summary>
        /// Gets the message to be consumed.
        /// </summary>
        public object Message
        {
            get; private set;
        }
    }
}

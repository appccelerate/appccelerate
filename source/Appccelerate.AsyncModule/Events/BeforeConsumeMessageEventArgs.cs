//-------------------------------------------------------------------------------
// <copyright file="BeforeConsumeMessageEventArgs.cs" company="Appccelerate">
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
    /// <summary>
    /// Event args for the <see cref="IModuleController.BeforeConsumeMessage"/> event.
    /// </summary>
    public class BeforeConsumeMessageEventArgs : ConsumeMessageEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeConsumeMessageEventArgs"/> class.
        /// </summary>
        /// <param name="module">The module, which consumes the message.</param>
        /// <param name="message">The message to be consumed.</param>
        public BeforeConsumeMessageEventArgs(object module, object message) : base(module, message)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BeforeConsumeMessageEventArgs"/> is cancel (the message will not be passed to its handler).
        /// </summary>
        /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
        public bool Cancel
        { 
            get; set;
        }
    }
}
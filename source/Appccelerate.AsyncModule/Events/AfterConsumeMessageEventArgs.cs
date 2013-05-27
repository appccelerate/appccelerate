//-------------------------------------------------------------------------------
// <copyright file="AfterConsumeMessageEventArgs.cs" company="Appccelerate">
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

namespace Appccelerate.AsyncModule.Events
{
    /// <summary>
    /// Event arguments for the AfterMessageConsume event.
    /// </summary>
    public class AfterConsumeMessageEventArgs : ConsumeMessageEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AfterConsumeMessageEventArgs"/> class.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="message">The message.</param>
        /// <param name="notSkipped">True if the message was consumed, otherwise false (message was skipped).</param>
        public AfterConsumeMessageEventArgs(object module, object message, bool notSkipped) : base(module, message)
        {
            this.NotSkipped = notSkipped;
        }

        /// <summary>
        /// Gets a value indicating whether the message was actually consumed and not skipped.
        /// </summary>
        /// <value><c>true</c> if message was not skipped otherwise, <c>false</c>.</value>
        public bool NotSkipped
        {
            get; private set;
        }
    }
}
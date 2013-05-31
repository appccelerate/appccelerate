//-------------------------------------------------------------------------------
// <copyright file="RepeatedPublicationException.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals.Exceptions
{
    using Appccelerate.EventBroker.Exceptions;

    /// <summary>
    /// An <see cref="EventBrokerException"/> thrown when a single publication event defines the same topic several times.
    /// </summary>
    public class RepeatedPublicationException : EventBrokerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatedPublicationException"/> class.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventName">Name of the event.</param>
        public RepeatedPublicationException(object publisher, string eventName)
            : base("Cannot add more than one instance of the same publisher to one topic: '{0}.{1}'.", publisher.GetType().FullName, eventName)
        {
        }
    }
}

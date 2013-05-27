//-------------------------------------------------------------------------------
// <copyright file="PublisherEventNotFoundException.cs" company="Appccelerate">
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
    using System;

    using Appccelerate.EventBroker.Exceptions;

    /// <summary>
    /// An <see cref="EventBrokerException"/> thrown when a published event can not be found while registering a publisher.
    /// </summary>
    public class PublisherEventNotFoundException : EventBrokerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherEventNotFoundException"/> class.
        /// </summary>
        /// <param name="publisherType">Type of the publisher.</param>
        /// <param name="eventName">Name of the event.</param>
        public PublisherEventNotFoundException(Type publisherType, string eventName)
            : base("Publication event not found: '{0}.{1}'", publisherType.FullName, eventName)
        {
        }
    }
}

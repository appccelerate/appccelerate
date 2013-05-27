//-------------------------------------------------------------------------------
// <copyright file="MissingMappingContext.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker.Internals
{
    using System;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals;

    /// <summary>
    /// The missing mapping context provides detailed information about a missing mapping.
    /// </summary>
    public class MissingMappingContext : IMissingMappingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingMappingContext"/> class.
        /// </summary>
        /// <param name="eventTopic">The source event topic.</param>
        /// <param name="destinationTopic">The destination topic URI.</param>
        /// <param name="publication">The publication which triggered the event.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="exception">The exception which contains information why the mapping was not possible.</param>
        public MissingMappingContext(IEventTopicInfo eventTopic, string destinationTopic, IPublication publication, object sender, EventArgs eventArgs, Exception exception)
        {
            this.Exception = exception;
            this.EventArgs = eventArgs;
            this.Sender = sender;
            this.Publication = publication;
            this.DestinationTopic = destinationTopic;
            this.EventTopic = eventTopic;
        }

        /// <summary>
        /// Gets the source event topic information.
        /// </summary>
        public IEventTopicInfo EventTopic { get; private set; }

        /// <summary>
        /// Gets the destination topic as URI.
        /// </summary>
        public string DestinationTopic { get; private set; }

        /// <summary>
        /// Gets the publication.
        /// </summary>
        public IPublication Publication { get; private set; }

        /// <summary>
        /// Gets the sender of the event
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Gets the event arguments which are part of the event.
        /// </summary>
        public EventArgs EventArgs { get; private set; }

        /// <summary>
        /// Gets the exception which contains information why the mapping was not possible.
        /// </summary>
        public Exception Exception { get; private set; }
    }
}
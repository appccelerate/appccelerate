//-------------------------------------------------------------------------------
// <copyright file="EventInformation.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine
{
    using System;

    /// <summary>
    /// Provides information about an event: event-id and arguments.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public class EventInformation<TEvent>
        where TEvent : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventInformation&lt;TEvent&gt;"/> class.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventArgument">The event argument.</param>
        public EventInformation(TEvent eventId, object eventArgument)
        {
            this.EventId = eventId;
            this.EventArgument = eventArgument;
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        public TEvent EventId { get; private set; }

        /// <summary>
        /// Gets the event argument.
        /// </summary>
        /// <value>The event argument.</value>
        public object EventArgument { get; private set; }
    }
}
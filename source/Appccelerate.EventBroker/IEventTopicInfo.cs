//-------------------------------------------------------------------------------
// <copyright file="IEventTopicInfo.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents a point of information on a certain topic between the topic publishers and the topic subscribers.
    /// </summary>
    public interface IEventTopicInfo
    {
        /// <summary>
        /// Gets the URI for the event topic. This URI is the unique identifier for this event topic.
        /// </summary>
        /// <value>The URI of this event topic.</value>
        string Uri { get; }

        /// <summary>
        /// Gets the publications for the event topic.
        /// </summary>
        /// <remarks>The publications are frequently cleaned internally when
        /// necessary. Therefore the publications are only valid at the time
        /// when they are requested and should not be cached or referenced longer
        /// then necessary.</remarks>
        IEnumerable<IPublication> Publications { get; }

        /// <summary>
        /// Gets the subscriptions for the event topic.
        /// </summary>
        /// <remarks>The subscriptions are frequently cleaned internally when
        /// necessary. Therefore the subscriptions are only valid at the time
        /// when they are requested and should not be cached or referenced longer
        /// then necessary.</remarks>
        IEnumerable<ISubscription> Subscriptions { get; }

        /// <summary>
        /// Describes this event topic:
        /// publications, subscriptions, names, thread options, scopes, event arguments.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void DescribeTo(TextWriter writer);
    }
}
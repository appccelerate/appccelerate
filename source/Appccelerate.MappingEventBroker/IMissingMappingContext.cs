//-------------------------------------------------------------------------------
// <copyright file="IMissingMappingContext.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker
{
    using System;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals;

    /// <summary>
    /// The missing mapping context provides detailed information about a missing mapping.
    /// </summary>
    public interface IMissingMappingContext
    {
        /// <summary>
        /// Gets the source event topic information.
        /// </summary>
        IEventTopicInfo EventTopic { get; }

        /// <summary>
        /// Gets the destination topic as URI.
        /// </summary>
        string DestinationTopic { get; }

        /// <summary>
        /// Gets the publication.
        /// </summary>
        IPublication Publication { get; }

        /// <summary>
        /// Gets the sender of the event
        /// </summary>
        object Sender { get; }

        /// <summary>
        /// Gets the event arguments which are part of the event.
        /// </summary>
        EventArgs EventArgs { get; }

        /// <summary>
        /// Gets the exception which contains information why the mapping was not possible.
        /// </summary>
        Exception Exception { get; }
    }
}
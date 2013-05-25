//-------------------------------------------------------------------------------
// <copyright file="IEventFired.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.Messages
{
    using EventBroker;

    /// <summary>
    /// Defines the interface for the standard message.
    /// </summary>
    public interface IEventFired
    {
        /// <summary>
        /// Gets or sets the distributed event broker identification. 
        /// The distributed event broker identification is the only
        /// identification which must be known to bound logically 
        /// together several event broker instances. 
        /// </summary>
        string DistributedEventBrokerIdentification { get; set; }

        /// <summary>
        /// Gets or sets the identification of the event broker which fired the event.
        /// </summary>
        string EventBrokerIdentification { get; set; }

        /// <summary>
        /// Gets or sets the topic of the event which was fired.
        /// </summary>
        string Topic { get; set; }

        /// <summary>
        /// Gets or sets the subscriber handler restriction.
        /// </summary>
        /// <value>The subscriber handler restriction.</value>
        HandlerRestriction HandlerRestriction { get; set; }

        /// <summary>
        /// Gets or sets the type of the event args.
        /// </summary>
        /// <value>The type of the event args.</value>
        string EventArgsType { get; set; }

        /// <summary>
        /// Gets or sets the event arguments of the event which was fired.
        /// </summary>
        string EventArgs { get; set; }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="EventFiredHandlerBase.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.Handlers
{
    using System.Globalization;
    using EventBroker;
    using Events;
    using Messages;

    /// <summary>
    /// Base implementation for all handlers.
    /// </summary>
    public abstract class EventFiredHandlerBase
    {
        private readonly IEventBroker eventBroker;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiredHandlerBase"/> class.
        /// </summary>
        protected EventFiredHandlerBase()
            : this(InternalEventBrokerHolder.InternalEventBroker)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiredHandlerBase"/> class.
        /// </summary>
        /// <param name="eventBroker">The event broker.</param>
        protected EventFiredHandlerBase(IEventBroker eventBroker)
        {
            this.eventBroker = eventBroker;
            this.Restriction = HandlerRestriction.Asynchronous;
        }

        /// <summary>
        /// Gets the event broker.
        /// </summary>
        /// <value>The event broker.</value>
        protected IEventBroker EventBroker
        {
            get { return this.eventBroker; }
        }

        protected HandlerRestriction Restriction { get; set; }

        /// <summary>
        /// Fires the message event on the internal event broker.
        /// </summary>
        /// <param name="message">The message.</param>
        protected virtual void DoHandle(IEventFired message)
        {
            string topic = this.CreateTopic(message);

            this.EventBroker.Fire(topic, this, this.Restriction, this, new EventArgs<IEventFired>(message));
        }

        /// <summary>
        /// Creates the dynamic topic according to the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The topic uri.</returns>
        protected virtual string CreateTopic(IEventFired message)
        {
            Ensure.ArgumentNotNull(message, "message");

            return string.Format(CultureInfo.InvariantCulture, Constants.InternalTopicFormat, message.DistributedEventBrokerIdentification);
        }
    }
}
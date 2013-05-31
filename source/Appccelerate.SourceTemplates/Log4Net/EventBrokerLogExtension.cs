//-------------------------------------------------------------------------------
// <copyright file="EventBrokerLogExtension.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Exceptions;
    using Appccelerate.EventBroker.Extensions;
    using Appccelerate.EventBroker.Matchers;

    using log4net;
    
    /// <summary>
    /// Event broker extension that logs event broker activity.
    /// </summary>
    public class EventBrokerLogExtension : EventBrokerExtensionBase
    {
        /// <summary>
        /// Logger of this instance.
        /// </summary>
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBrokerLogExtension"/> class.
        /// </summary>
        public EventBrokerLogExtension()
        {
            this.log = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBrokerLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger to be used for log messages.</param>
        public EventBrokerLogExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Called when the event was fired (processing completed).
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public override void FiredEvent(IEventTopicInfo eventTopic, IPublication publication, object sender, EventArgs e)
        {
            INamedItem namedItem = publication.Publisher as INamedItem;
            if (namedItem != null)
            {
                this.log.DebugFormat(
                    CultureInfo.InvariantCulture,
                    "Fired event '{0}'. Invoked by publisher '{1}' with name '{2}' with sender '{3}' and EventArgs '{4}'.",
                    eventTopic.Uri,
                    publication.Publisher,
                    namedItem.EventBrokerItemName,
                    sender,
                    e);
            }
            else
            {
                this.log.DebugFormat(
                    CultureInfo.InvariantCulture,
                    "Fired event '{0}'. Invoked by publisher '{1}' with sender '{2}' and EventArgs '{3}'.",
                    eventTopic.Uri,
                    publication.Publisher,
                    sender,
                    e);
            }
        }

        /// <summary>
        /// Called when an item was registered.
        /// </summary>
        /// <param name="item">The item that was registered.</param>
        public override void RegisteredItem(object item)
        {
            INamedItem namedItem = item as INamedItem;
            if (namedItem != null)
            {
                this.log.DebugFormat("Registered item '{0}' with name '{1}'.", item, namedItem.EventBrokerItemName);
            }
            else
            {
                this.log.DebugFormat("Registered item '{0}'.", item);
            }
        }

        /// <summary>
        /// Called when an item was unregistered.
        /// </summary>
        /// <param name="item">The item that was unregistered.</param>
        public override void UnregisteredItem(object item)
        {
            INamedItem namedItem = item as INamedItem;
            if (namedItem != null)
            {
                this.log.DebugFormat("Unregistered item '{0}' with name '{1}'.", item, namedItem.EventBrokerItemName);
            }
            else
            {
                this.log.DebugFormat("Unregistered item '{0}'.", item);
            }
        }

        /// <summary>
        /// Called after an event topic was created.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        public override void CreatedTopic(IEventTopicInfo eventTopic)
        {
            this.log.DebugFormat("Topic created: '{0}'.", eventTopic.Uri);
        }

        /// <summary>
        /// Called after a publication was created.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        public override void CreatedPublication(IEventTopicInfo eventTopic, IPublication publication)
        {
        }

        /// <summary>
        /// Called after a subscription was created.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="subscription">The subscription.</param>
        public override void CreatedSubscription(IEventTopicInfo eventTopic, ISubscription subscription)
        {
        }

        /// <summary>
        /// Called when an event is fired.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public override void FiringEvent(IEventTopicInfo eventTopic, IPublication publication, object sender, EventArgs e)
        {
            this.log.DebugFormat(
                "Firing event '{0}'. Invoked by publisher '{1}' with EventArgs '{2}'.",
                eventTopic.Uri,
                sender,
                e);
        }

        /// <summary>
        /// Called after a publication was added to an event topic.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        public override void AddedPublication(IEventTopicInfo eventTopic, IPublication publication)
        {
            using (TextWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                foreach (IPublicationMatcher publicationMatcher in publication.PublicationMatchers)
                {
                    publicationMatcher.DescribeTo(writer);
                    writer.Write(", ");
                }

                this.log.DebugFormat(
                    CultureInfo.InvariantCulture,
                    "Added publication '{0}.{1}' to topic '{2}' with matchers '{3}'.",
                    publication.Publisher != null ? publication.Publisher.GetType().FullName : "-",
                    publication.EventName,
                    eventTopic.Uri,
                    writer);
            }
        }

        /// <summary>
        /// Called after a publication was removed from an event topic.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        public override void RemovedPublication(IEventTopicInfo eventTopic, IPublication publication)
        {
            this.log.DebugFormat(
                    "Removed publication '{0}.{1}' from topic '{2}'.",
                    publication.Publisher != null ? publication.Publisher.GetType().FullName : "-",
                    publication.EventName,
                    eventTopic.Uri);
        }

        /// <summary>
        /// Called after a subscription was added to an event topic.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="subscription">The subscription.</param>
        public override void AddedSubscription(IEventTopicInfo eventTopic, ISubscription subscription)
        {
            using (TextWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                foreach (ISubscriptionMatcher subscriptionMatcher in subscription.SubscriptionMatchers)
                {
                    subscriptionMatcher.DescribeTo(writer);
                    writer.Write(", ");
                }

                this.log.DebugFormat(
                    CultureInfo.InvariantCulture,
                    "Added subscription '{0}.{1}' to topic '{2}' with matchers '{3}'.",
                    subscription.Subscriber != null ? subscription.Subscriber.GetType().FullName : "-",
                    subscription.HandlerMethodName,
                    eventTopic.Uri,
                    writer);
            }
        }

        /// <summary>
        /// Called after a subscription was removed from an event topic.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="subscription">The subscription.</param>
        public override void RemovedSubscription(IEventTopicInfo eventTopic, ISubscription subscription)
        {
            this.log.DebugFormat(
                    "Removed subscription '{0}.{1}' from topic '{2}'.",
                    subscription.Subscriber != null ? subscription.Subscriber.GetType().FullName : "-",
                    subscription.HandlerMethodName,
                    eventTopic.Uri);
        }

        /// <summary>
        /// Called before an event is relayed from the publication to the subscribers.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public override void RelayingEvent(IEventTopicInfo eventTopic, IPublication publication, ISubscription subscription, IHandler handler, object sender, EventArgs e)
        {
            this.log.DebugFormat(
                CultureInfo.InvariantCulture,
                "Relaying event '{6}' from publisher '{0}' [{1}] to subscriber '{2}' [{3}] with EventArgs '{4}' with handler '{5}'.",
                publication.Publisher,
                publication.Publisher is INamedItem ? ((INamedItem)publication.Publisher).EventBrokerItemName : string.Empty,
                subscription.Subscriber,
                subscription.Subscriber is INamedItem ? ((INamedItem)subscription.Subscriber).EventBrokerItemName : string.Empty,
                e,
                handler,
                eventTopic.Uri);
        }

        /// <summary>
        /// Called after the event was relayed from the publication to the subscribers.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public override void RelayedEvent(IEventTopicInfo eventTopic, IPublication publication, ISubscription subscription, IHandler handler, object sender, EventArgs e)
        {
            this.log.DebugFormat(
                CultureInfo.InvariantCulture,
                "Relayed event '{6}' from publisher '{0}' [{1}] to subscriber '{2}' [{3}] with EventArgs '{4}' with handler '{5}'.",
                publication.Publisher,
                publication.Publisher is INamedItem ? ((INamedItem)publication.Publisher).EventBrokerItemName : string.Empty,
                subscription.Subscriber,
                subscription.Subscriber is INamedItem ? ((INamedItem)subscription.Subscriber).EventBrokerItemName : string.Empty,
                e,
                handler,
                eventTopic.Uri);
        }

        /// <summary>
        /// Called when a publication or subscription matcher did not match and the event was not relayed to a subscription.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public override void SkippedEvent(IEventTopicInfo eventTopic, IPublication publication, ISubscription subscription, object sender, EventArgs e)
        {
            List<IMatcher> matchers = new List<IMatcher>();

            var publicationMatchers = from matcher in publication.PublicationMatchers where !matcher.Match(publication, subscription, e) select matcher;
            var subscriptionMatchers = from matcher in subscription.SubscriptionMatchers where !matcher.Match(publication, subscription, e) select matcher;
            matchers.AddRange(publicationMatchers);
            matchers.AddRange(subscriptionMatchers);

            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                foreach (IMatcher matcher in matchers)
                {
                    matcher.DescribeTo(writer);
                    writer.Write(", ");
                }
            }

            if (sb.Length > 0)
            {
                sb.Length -= 2;
            }

            this.log.DebugFormat(
                CultureInfo.InvariantCulture,
                "Skipped event '{0}' from publisher '{1}' [{2}] to subscriber '{3}' [{4}] with EventArgs '{5}' because the matchers '{6}' did not match.",
                eventTopic.Uri,
                publication.Publisher,
                publication.Publisher is INamedItem ? ((INamedItem)publication.Publisher).EventBrokerItemName : string.Empty,
                subscription.Subscriber,
                subscription.Subscriber is INamedItem ? ((INamedItem)subscription.Subscriber).EventBrokerItemName : string.Empty,
                e,
                sb);
        }

        /// <summary>
        /// Called when an exception occurred during event handling by a subscriber.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The context providing information whether the exception is handled by an extension or is re-thrown.</param>
        public override void SubscriberExceptionOccurred(IEventTopicInfo eventTopic, Exception exception, ExceptionHandlingContext context)
        {
            this.log.Error(
                string.Format(CultureInfo.InvariantCulture, "An exception was thrown during handling the topic '{0}'", eventTopic.Uri),
                exception);
        }
    }
}
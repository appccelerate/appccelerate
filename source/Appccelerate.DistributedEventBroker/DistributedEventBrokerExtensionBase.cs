//-------------------------------------------------------------------------------
// <copyright file="DistributedEventBrokerExtensionBase.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Appccelerate.Events;
    using EventBroker;
    using EventBroker.Extensions;
    using EventBroker.Handlers;

    using Factories;
    using Messages;

    /// <summary>
    /// The base class for all distributed event broker extensions.
    /// </summary>
    public class DistributedEventBrokerExtensionBase : EventBrokerExtensionBase, IDistributedEventBrokerExtension
    {
        private readonly object locker = new object();

        private readonly List<string> topics;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedEventBrokerExtensionBase"/> class.
        /// </summary>
        /// <param name="distributedEventBrokerIdentification">The distributed event broker identification.</param>
        /// <param name="eventBrokerBus">The event broker bus.</param>
        protected DistributedEventBrokerExtensionBase(string distributedEventBrokerIdentification, IEventBrokerBus eventBrokerBus)
            : this(distributedEventBrokerIdentification, eventBrokerBus, new DefaultDistributedFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedEventBrokerExtensionBase"/> class.
        /// </summary>
        /// <param name="distributedEventBrokerIdentification">The distributed event broker identification.</param>
        /// <param name="eventBrokerBus">The event broker bus.</param>
        /// <param name="factory">The factory.</param>
        protected DistributedEventBrokerExtensionBase(string distributedEventBrokerIdentification, IEventBrokerBus eventBrokerBus, IDistributedFactory factory)
        {
            this.Factory = factory;
            this.Serializer = this.Factory.CreateEventArgsSerializer();
            this.DistributedEventBrokerIdentification = distributedEventBrokerIdentification;
            this.MessageFactory = this.Factory.CreateMessageFactory();
            this.SelectionStrategy = this.Factory.CreateTopicSelectionStrategy();

            this.EventBrokerBus = eventBrokerBus;

            this.topics = new List<string>();
        }

        /// <summary>
        /// Gets the internal event registerer which is the <see cref="InternalEventBrokerHolder.InternalEventBroker"/> casted to <see cref="IEventRegistrar"/>.
        /// </summary>
        /// <value>The internal event registerer.</value>
        protected virtual IEventRegistrar EventRegistrar
        {
            get { return (IEventRegistrar)InternalEventBrokerHolder.InternalEventBroker.SpecialCasesRegistrar; }
        }

        /// <summary>
        /// Gets the hosted event broker.
        /// </summary>
        /// <value>The hosted event broker.</value>
        protected IEventBroker HostedEventBroker
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the topics which are controlled by the <see cref="HostedEventBroker"/> and accepted by the <see cref="SelectionStrategy"/>.
        /// </summary>
        /// <value>The topics.</value>
        protected IEnumerable<string> Topics
        {
            get { return this.topics; }
        }

        /// <summary>
        /// Gets the event broker bus.
        /// </summary>
        /// <value>The event broker bus.</value>
        protected IEventBrokerBus EventBrokerBus
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the factory which is responsible for creating the types
        /// necessary to operate the extension.
        /// </summary>
        /// <value>The factory.</value>
        protected IDistributedFactory Factory
        {
            get; private set;
        }

        /// <summary>
        /// Gets the message factory.
        /// </summary>
        /// <value>The message factory.</value>
        protected IEventMessageFactory MessageFactory
        {
            get; private set;
        }

        /// <summary>
        /// Gets the serializer which is responsible for event argument serialization.
        /// </summary>
        protected IEventArgsSerializer Serializer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the topic selection strategy.
        /// </summary>
        /// <value>The topic selection strategy.</value>
        protected ITopicSelectionStrategy SelectionStrategy
        {
            get; private set;
        }

        /// <summary>
        /// Gets the distributed event broker identification.
        /// </summary>
        /// <value>The distributed event broker identification.</value>
        protected string DistributedEventBrokerIdentification
        {
            get; private set;
        }

        /// <summary>
        /// Gets the hosted event broker identification.
        /// </summary>
        /// <value>The hosted event broker identification.</value>
        protected string HostedEventBrokerIdentification
        {
            get;
            private set;
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
            base.FiringEvent(eventTopic, publication, sender, e);

            if (this.Equals(sender))
            {
                return;
            }

            lock (this.locker)
            {
                if (!this.Topics.Contains(eventTopic.Uri))
                {
                    return;
                }
            }

            var message = this.MessageFactory.CreateEventFiredMessage(m =>
                                                            {
                                                                m.Topic = eventTopic.Uri;
                                                                m.HandlerRestriction = publication.HandlerRestriction;
                                                                m.EventArgsType = e.GetType().AssemblyQualifiedName;
                                                                m.EventArgs = this.Serializer.Serialize(e);
                                                                m.EventBrokerIdentification = this.HostedEventBrokerIdentification.ToString(CultureInfo.InvariantCulture);
                                                                m.DistributedEventBrokerIdentification =
                                                                    this.DistributedEventBrokerIdentification;
                                                            });

            this.EventBrokerBus.Publish(message);
        }

        /// <summary>
        /// Called after an event topic was created.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        public override void CreatedTopic(IEventTopicInfo eventTopic)
        {
            base.CreatedTopic(eventTopic);

            Ensure.ArgumentNotNull(eventTopic, "eventTopic");

            if (!this.SelectionStrategy.SelectTopic(eventTopic))
            {
                return;
            }

            var publicationsWithNotAllowedRestrictions = eventTopic.Publications.Where(p => p.HandlerRestriction != HandlerRestriction.Asynchronous).ToList();

            if (publicationsWithNotAllowedRestrictions.Any())
            {
                string message = BuildPublicationsWithNotAllowedHandlerRestrictionsMessage(publicationsWithNotAllowedRestrictions);

                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Only asynchronous handling of events is allowed! The wrong publications are: {0}", message));
            }

            lock (this.locker)
            {
                this.topics.Add(eventTopic.Uri);
            }
        }

        /// <summary>
        /// Called after an event topic was disposed.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        public override void Disposed(IEventTopicInfo eventTopic)
        {
            base.Disposed(eventTopic);

            Ensure.ArgumentNotNull(eventTopic, "eventTopic");

            lock (this.locker)
            {
                string uri = eventTopic.Uri;

                if (this.topics.Contains(uri))
                {
                    this.topics.Remove(uri);
                }
            }
        }

        /// <summary>
        /// Manages the specified event broker.
        /// </summary>
        /// <param name="eventBroker">The event broker.</param>
        public void Manage(IEventBroker eventBroker)
        {
            this.DoManage(eventBroker, null);
        }

        /// <summary>
        /// Manages the specified event broker.
        /// </summary>
        /// <param name="eventBroker">The event broker.</param>
        /// <param name="eventBrokerIdentification">The event broker identification.</param>
        public void Manage(IEventBroker eventBroker, string eventBrokerIdentification)
        {
            this.DoManage(eventBroker, eventBrokerIdentification);
        }

        /// <summary>
        /// Concrete implementation for managing a single event broker.
        /// </summary>
        /// <param name="eventBroker">The event broker.</param>
        /// <param name="eventBrokerIdentification">The event broker identification.</param>
        protected virtual void DoManage(IEventBroker eventBroker, string eventBrokerIdentification)
        {
            if (this.HostedEventBroker != null)
            {
                throw new InvalidOperationException("Cannot manage more than one event broker.");
            }

            this.HostedEventBrokerIdentification = this.CreateHostedEventBrokerIdentification(eventBrokerIdentification);
            this.HostedEventBroker = eventBroker;

            var topic = this.CreateDynamicSubscriptionTopic();

            this.EventRegistrar.AddSubscription<EventArgs<IEventFired>>(topic, this, this.HandleEvent, this.CreateHandler());
        }

        /// <summary>
        /// Message handler which can be called by extensions or internal event brokers.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventFired">The event fired.</param>
        protected virtual void HandleMessage(object sender, IEventFired eventFired)
        {
            Ensure.ArgumentNotNull(eventFired, "eventFired");

            if (this.HostedEventBrokerIdentification.Equals(eventFired.EventBrokerIdentification))
            {
                return;
            }

            var eventArgsType = Type.GetType(eventFired.EventArgsType, true);

            var eventArgs = this.Serializer.Deserialize(eventArgsType, eventFired.EventArgs);

            this.HostedEventBroker.Fire(eventFired.Topic, this, eventFired.HandlerRestriction, this, eventArgs);
        }

        /// <summary>
        /// Creates the hosted event broker identification.
        /// </summary>
        /// <param name="eventBrokerIdentification">The event broker identification.</param>
        /// <returns>An identification for the hosted event broker.</returns>
        protected virtual string CreateHostedEventBrokerIdentification(string eventBrokerIdentification)
        {
            return string.IsNullOrEmpty(eventBrokerIdentification) ? Guid.NewGuid().ToString() : eventBrokerIdentification;
        }

        /// <summary>
        /// Creates the dynamic subscription topic.
        /// </summary>
        /// <returns>A dynamic subscription topic.</returns>
        protected virtual string CreateDynamicSubscriptionTopic()
        {
            return string.Format(CultureInfo.InvariantCulture, Constants.InternalTopicFormat, this.DistributedEventBrokerIdentification);
        }

        /// <summary>
        /// Creates the handler which is the default handler type for internal topics.
        /// </summary>
        /// <returns>The default handler type for internal topics</returns>
        protected virtual IHandler CreateHandler()
        {
            return new OnBackground();
        }

        /// <summary>
        /// Builds up a message text describing the publications which do not have correct handler restrictions.
        /// </summary>
        /// <param name="publicationsWithNotAllowedRestriction">The publications which do not have correct handler restrictions.</param>
        /// <returns>The message.</returns>
        private static string BuildPublicationsWithNotAllowedHandlerRestrictionsMessage(IEnumerable<IPublication> publicationsWithNotAllowedRestriction)
        {
            StringBuilder sb = new StringBuilder();
            using (TextWriter stringWriter = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                publicationsWithNotAllowedRestriction.ToList().ForEach(p => p.DescribeTo(stringWriter));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Called from the internal event broker when an event is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Appccelerate.Events.EventArgs{IEventFired}"/> instance containing the event data.</param>
        private void HandleEvent(object sender, EventArgs<IEventFired> e)
        {
            this.HandleMessage(sender, e.Value);
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="MappingEventBrokerExtension.cs" company="Appccelerate">
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
    using Appccelerate.EventBroker.Extensions;
    using Appccelerate.EventBroker.Internals;
    using Appccelerate.MappingEventBroker.Conventions;
    using Appccelerate.MappingEventBroker.Internals;

    /// <summary>
    /// This extension allows to dynamically remap topics based on a convention
    /// from one event argument type to another event argument type using 
    /// <see cref="IMapper"/>.
    /// <code>
    ///    public class Publisher
    ///    {
    ///        [EventPublication(@"topic://Original")]
    ///        public event EventHandler Event;
    ///        private void InvokeEvent(EventArgs e)
    ///        {
    ///            EventHandler handler = Event;
    ///            if (handler != null) handler(this, e);
    ///        }
    ///        public void Publish()
    ///        {
    ///            this.InvokeEvent(EventArgs.Empty);
    ///        }
    ///    }
    ///    public class SubscriberOriginal
    ///    {
    ///        [EventSubscription(@"topic://Original", typeof(Appccelerate.EventBroker.Handlers.OnPublisher))]
    ///        public void HandleOriginal(object sender, EventArgs e)
    ///        {
    ///        }
    ///    }
    ///    public class SubscriberMapped
    ///    {
    ///        [EventSubscription(@"mapped://Original", typeof(Appccelerate.EventBroker.Handlers.OnPublisher))]
    ///        public void HandleOriginal(object sender, CancelEventArgs e)
    ///        {
    ///        }
    ///    }
    /// </code>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Currently I have no better naming!")]
    public abstract class MappingEventBrokerExtension : EventBrokerExtensionBase, IMappingEventBrokerExtension
    {
        private readonly object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEventBrokerExtension"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="typeProvider">The destination event argument type provider which overrides the default.</param>
        protected MappingEventBrokerExtension(IMapper mapper, IDestinationEventArgsTypeProvider typeProvider)
            : this(mapper, new DefaultTopicConvention(), typeProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEventBrokerExtension"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="topicConvention">The topic convention which overrides the default behavior.</param>
        /// <param name="typeProvider">The destination event argument type provider which overrides the default.</param>
        protected MappingEventBrokerExtension(IMapper mapper, ITopicConvention topicConvention, IDestinationEventArgsTypeProvider typeProvider)
        {
            this.TopicConvention = topicConvention;
            this.Topics = new EventTopicCollection();
            this.Action = ctx => { };
            this.Mapper = mapper;
            this.TypeProvider = typeProvider;
        }

        /// <summary>
        /// Gets the topics which are controlled by the event broker and accepted by the <see cref="TopicConvention"/>.
        /// </summary>
        /// <value>The topics.</value>
        protected EventTopicCollection Topics { get; private set; }

        /// <summary>
        /// Gets the topic convention.
        /// </summary>
        /// <value>The topic convention.</value>
        protected ITopicConvention TopicConvention { get; private set; }

        /// <summary>
        /// Gets the missing mapping action.
        /// </summary>
        /// <value>The missing mapping action.</value>
        protected Action<IMissingMappingContext> Action { get; private set; }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Gets the destination event argument type provider.
        /// </summary>
        protected IDestinationEventArgsTypeProvider TypeProvider { get; private set; }

        /// <summary>
        /// Gets the hosted event broker.
        /// </summary>
        protected IEventBroker HostedEventBroker { get; private set; }

        /// <summary>
        /// Called after an event topic was created.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        public override void CreatedTopic(IEventTopicInfo eventTopic)
        {
            base.CreatedTopic(eventTopic);

            this.AssertEventBrokerManaged();

            if (!this.TopicConvention.IsCandidate(eventTopic))
            {
                return;
            }

            lock (this.locker)
            {
                this.Topics.Add(eventTopic);
            }
        }

        /// <inheritdoc />
        public void SetMissingMappingAction(Action<IMissingMappingContext> action)
        {
            this.Action = action;
        }

        /// <summary>
        /// Called when an event is fired.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        public override void FiringEvent(IEventTopicInfo eventTopic, IPublication publication, object sender, EventArgs e)
        {
            base.FiringEvent(eventTopic, publication, sender, e);

            Ensure.ArgumentNotNull(eventTopic, "eventTopic");
            Ensure.ArgumentNotNull(publication, "publication");

            this.AssertEventBrokerManaged();

            string sourceTopicUri = eventTopic.Uri;
            string destinationTopicUri = this.TopicConvention.MapTopic(sourceTopicUri);

            if (this.MustProceed(sourceTopicUri, destinationTopicUri))
            {
                Type sourceEventArgsType = publication.EventArgsType;

                Type destinationEventArgsType = this.TypeProvider.GetDestinationEventArgsType(destinationTopicUri, sourceEventArgsType);

                try
                {
                    if (destinationEventArgsType == null)
                    {
                        throw new ArgumentException("Destination event argument type provider was unable to determine destination event argument type.");
                    }

                    var mappedEventArgs = this.Mapper.Map(sourceEventArgsType, destinationEventArgsType, e);

                    this.HostedEventBroker.Fire(destinationTopicUri, publication.Publisher, publication.HandlerRestriction, sender, mappedEventArgs);
                }
                catch (Exception mappingException)
                {
                    this.Action(new MissingMappingContext(eventTopic, destinationTopicUri, publication, sender, e, mappingException));
                }
            }
        }

        /// <summary>
        /// Called after an event topic was disposed.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        public override void Disposed(IEventTopicInfo eventTopic)
        {
            base.Disposed(eventTopic);

            if (!this.Topics.Contains(eventTopic))
            {
                return;
            }

            lock (this.locker)
            {
                this.Topics.Remove(eventTopic);
            }
        }

        /// <summary>
        /// Manages the specified event broker.
        /// </summary>
        /// <param name="eventBroker">The event broker.</param>
        public void Manage(IEventBroker eventBroker)
        {
            if (this.HostedEventBroker != null)
            {
                throw new InvalidOperationException("Cannot manage more than one event broker.");
            }

            this.HostedEventBroker = eventBroker;
        }

        /// <summary>
        /// Determines whether the the topic must be mapped and fired again.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="mappedTopic">The mapped topic.</param>
        /// <returns><see langword="true"/> if the topic must be mapped and
        /// fired again.</returns>
        protected virtual bool MustProceed(string topic, string mappedTopic)
        {
            Ensure.ArgumentNotNullOrEmpty(topic, "topic");
            Ensure.ArgumentNotNullOrEmpty(topic, "mappedTopic");

            return !topic.Equals(mappedTopic, StringComparison.OrdinalIgnoreCase)
                && this.Topics.Contains(topic);
        }

        /// <summary>
        /// Asserts that an event broker is managed by this instance.
        /// </summary>
        private void AssertEventBrokerManaged()
        {
            if (this.HostedEventBroker == null)
            {
                throw new InvalidOperationException("The extension does not manage an event broker. Please call Manage() before executing any action on the extension!");
            }
        }
    }
}

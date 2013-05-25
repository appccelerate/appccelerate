//-------------------------------------------------------------------------------
// <copyright file="ExtensionSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Appccelerate.EventBroker.Exceptions;
    using Appccelerate.EventBroker.Internals.Inspection;
    using Appccelerate.EventBroker.Matchers;

    using FluentAssertions;

    using Machine.Specifications;

    public class When_registering_objects_and_an_extension_was_added_on_the_event_broker : ExtensionSpecification
    {
        Establish context = () => 
            eventBroker.AddExtension(extension);

        Because of = () =>
            {
                eventBroker.Register(publisher);
                eventBroker.Register(subscriber);
            };

        It should_call_extension_when_event_topic_was_created = () =>
            extension.Log.Should().Contain("CreatedTopic");

        It should_call_extension_when_publication_was_created = () =>
            extension.Log.Should().Contain("CreatedPublication");
        
        It should_call_extension_when_subscription_was_created = () =>
            extension.Log.Should().Contain("CreatedSubscription");

        It should_call_extension_when_publication_was_added = () =>
            extension.Log.Should().Contain("AddedPublication");
        
        It should_call_extension_when_subscription_was_added = () =>
            extension.Log.Should().Contain("AddedSubscription");

        It should_call_extension_when_item_was_scanned = () =>
            extension.Log.Should().Contain("Scanned");

        It should_call_extension_when_item_was_registered = () =>
            extension.Log.Should().Contain("RegisteredItem");
    }

    public class When_unregistering_objects_and_an_extension_was_added_on_the_event_broker : ExtensionSpecification
    {
        Establish context = () =>
            {
                eventBroker.Register(publisher);
                eventBroker.Register(subscriber);
                eventBroker.AddExtension(extension);
            };

        Because of = () =>
            {
                eventBroker.Unregister(publisher);
                eventBroker.Unregister(subscriber);
            };

        It should_call_extension_when_publication_was_removed = () =>
            extension.Log.Should().Contain("RemovedPublication");
        
        It should_call_extension_when_subscription_was_removed = () =>
            extension.Log.Should().Contain("RemovedSubscription");
        
        It should_call_extension_when_item_was_scanned = () =>
            extension.Log.Should().Contain("Scanned");

        It should_call_extension_when_item_was_unregistered = () =>
            extension.Log.Should().Contain("UnregisteredItem");
    }

    public class When_firing_an_event_and_an_extension_was_added_on_the_event_broker : ExtensionSpecification
    {
        Establish context = () =>
            {
                eventBroker.Register(publisher);
                eventBroker.Register(subscriber);

                eventBroker.AddExtension(extension);
            };

        Because of = () => 
            publisher.FireEvent(sentEventArgs);

        It should_call_extension_when_event_is_fired = () =>
            extension.Log.Should().Contain("FiringEvent");

        It should_call_extension_when_event_is_relayed = () =>
            extension.Log.Should().Contain("RelayingEvent");

        It should_call_extension_when_event_was_relayed = () =>
            extension.Log.Should().Contain("RelayedEvent");

        It should_call_extension_when_event_was_fired = () =>
            extension.Log.Should().Contain("FiredEvent");
    }

    public class When_disposing_the_event_broker_and_an_extension_was_added_on_the_event_broker : ExtensionSpecification
    {
        Establish context = () =>
        {
            eventBroker.Register(new SimpleEvent.EventPublisher());
            eventBroker.AddExtension(extension);
        };

        Because of = () =>
            eventBroker.Dispose();

        It should_call_extension_when_event_topic_was_disposed = () =>
            extension.Log.Should().Contain("Disposed");
    }

    public class When_events_are_skipped_and_an_extension_was_added_on_the_event_broker : ExtensionSpecification
    {
        Establish context = () =>
        {
            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);

            eventBroker.AddGlobalMatcher(new Matcher());

            eventBroker.AddExtension(extension);
        };

        Because of = () =>
            publisher.FireEvent(EventArgs.Empty);

        It should_call_extension_when_event_was_skipped = () =>
            extension.Log.Should().Contain("SkippedEvent");

        public class Matcher : IMatcher
        {
            public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
            {
                return false;
            }

            public void DescribeTo(TextWriter writer)
            {
            }
        }
    }

    public class When_subscribers_throw_exceptions_and_an_extension_was_added_on_the_event_broker : ExtensionSpecification
    {
        static ExceptionSubscriber exceptionSubscriber;
 
        Establish context = () =>
        {
            exceptionSubscriber = new ExceptionSubscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(exceptionSubscriber);

            eventBroker.AddExtension(extension);
        };

        Because of = () =>
            Catch.Exception(() => publisher.FireEvent(EventArgs.Empty));

        It should_call_extension_when_subscriber_throws_exception = () =>
            extension.Log.Should().Contain("SubscriberExceptionOccured");

        public class ExceptionSubscriber
        {
            [EventSubscription(SimpleEvent.EventTopic, typeof(Handlers.OnPublisher))]
            public void Handle(object sender, EventArgs eventArgs)
            {
                throw new Exception("test");
            }
        }
    }

    [Subject(Subjects.Extensions)]
    public class ExtensionSpecification
    {
        protected static EventBroker eventBroker;
        protected static SimpleEvent.EventPublisher publisher;
        protected static SimpleEvent.EventSubscriber subscriber;
        protected static EventArgs sentEventArgs;
        protected static Extension extension;

        Establish context = () =>
            {
                extension = new Extension();
                eventBroker = new EventBroker();
            
                publisher = new SimpleEvent.EventPublisher();
                subscriber = new SimpleEvent.EventSubscriber();

                sentEventArgs = new EventArgs();
            };

        public class Extension : IEventBrokerExtension
        {
            private readonly StringBuilder log = new StringBuilder();

            public string Log
            {
                get { return this.log.ToString(); }
            }

            public void FiringEvent(IEventTopicInfo eventTopic, IPublication publication, object sender, EventArgs e)
            {
                this.log.AppendLine("FiringEvent");
            }

            public void FiredEvent(IEventTopicInfo eventTopic, IPublication publication, object sender, EventArgs e)
            {
                this.log.AppendLine("FiredEvent");
            }

            public void RegisteredItem(object item)
            {
                this.log.AppendLine("RegisteredItem");
            }

            public void UnregisteredItem(object item)
            {
                this.log.AppendLine("UnregisteredItem");
            }

            public void ScannedInstanceForPublicationsAndSubscriptions(object publisher, IEnumerable<PropertyPublicationScanResult> foundPublications, IEnumerable<PropertySubscriptionScanResult> foundSubscriptions)
            {
                this.log.AppendLine("Scanned");
            }

            public void ProcessedSubscriber(object subscriber, bool register, IEventTopicHost eventTopicHost)
            {
                this.log.AppendLine("ProcessedSubscriber");
            }

            public void CreatedTopic(IEventTopicInfo eventTopic)
            {
                this.log.AppendLine("CreatedTopic");
            }

            public void CreatedPublication(IEventTopicInfo eventTopic, IPublication publication)
            {
                this.log.AppendLine("CreatedPublication");
            }

            public void CreatedSubscription(IEventTopicInfo eventTopic, ISubscription subscription)
            {
                this.log.AppendLine("CreatedSubscription");
            }

            public void AddedPublication(IEventTopicInfo eventTopic, IPublication publication)
            {
                this.log.AppendLine("AddedPublication");
            }

            public void RemovedPublication(IEventTopicInfo eventTopic, IPublication publication)
            {
                this.log.AppendLine("RemovedPublication");
            }

            public void AddedSubscription(IEventTopicInfo eventTopic, ISubscription subscription)
            {
                this.log.AppendLine("AddedSubscription");
            }

            public void RemovedSubscription(IEventTopicInfo eventTopic, ISubscription subscription)
            {
                this.log.AppendLine("RemovedSubscription");
            }

            public void Disposed(IEventTopicInfo eventTopic)
            {
                this.log.AppendLine("Disposed");
            }

            public void SubscriberExceptionOccurred(IEventTopicInfo eventTopic, Exception exception, ExceptionHandlingContext context)
            {
                this.log.AppendLine("SubscriberExceptionOccured");
            }

            public void RelayingEvent(IEventTopicInfo eventTopic, IPublication publication, ISubscription subscription, IHandler handler, object sender, EventArgs e)
            {
                this.log.AppendLine("RelayingEvent");
            }

            public void RelayedEvent(IEventTopicInfo eventTopic, IPublication publication, ISubscription subscription, IHandler handler, object sender, EventArgs e)
            {
                this.log.AppendLine("RelayedEvent");
            }

            public void SkippedEvent(IEventTopicInfo eventTopic, IPublication publication, ISubscription subscription, object sender, EventArgs e)
            {
                this.log.AppendLine("SkippedEvent");
            }
        }
    }
}
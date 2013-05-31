//-------------------------------------------------------------------------------
// <copyright file="EventBrokerCleanupSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.WeakReferences
{
    using System;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.Cleanup)]
    public class When_subscribers_are_not_reference_anymore_by_client_code
    {
        static EventBroker eventBroker = new EventBroker();
        static SimpleEvent.EventPublisher publisher;
        static SimpleEvent.EventSubscriber subscriber;
        static SimpleEvent.EventSubscriber subscriberThatIsCollected;
        static SimpleEvent.RegisterableEventSubscriber registerableSubscriberThatIsCollected;
        static WeakReference referenceOnSubscriberThatIsCollected;
        static WeakReference referenceOnRegisterableSubscriberThatIsCollected;
        
        Establish context = () =>
            {
                eventBroker = new EventBroker();

                publisher = new SimpleEvent.EventPublisher();
                subscriber = new SimpleEvent.EventSubscriber();
                subscriberThatIsCollected = new SimpleEvent.EventSubscriber();
                registerableSubscriberThatIsCollected = new SimpleEvent.RegisterableEventSubscriber();

                eventBroker.Register(publisher);
                eventBroker.Register(subscriber);
                eventBroker.Register(subscriberThatIsCollected);
                eventBroker.Register(registerableSubscriberThatIsCollected);

                referenceOnSubscriberThatIsCollected = new WeakReference(subscriberThatIsCollected);
                referenceOnRegisterableSubscriberThatIsCollected = new WeakReference(registerableSubscriberThatIsCollected);
            };

        Because of = () =>
            {
                subscriberThatIsCollected = null;
                registerableSubscriberThatIsCollected = null;
                GC.Collect();
            };

        It should_garbage_collect_subscribers_registered_by_attribute = () =>
            referenceOnSubscriberThatIsCollected.IsAlive
                .Should().BeFalse("subscriber should be collected");

        It should_garbage_collect_subscribers_registered_by_registrar = () =>
            referenceOnRegisterableSubscriberThatIsCollected.IsAlive
                .Should().BeFalse("subscriber should be collected");
    }

    [Subject(Subjects.Cleanup)]
    public class When_publishers_are_not_reference_anymore_by_client_code
    {
        static EventBroker eventBroker = new EventBroker();
        static SimpleEvent.EventPublisher publisher;
        static SimpleEvent.RegisterableEventPublisher registerablePublisher;
        static SimpleEvent.EventSubscriber subscriber;
        static WeakReference weakReferenceOnPublisher;
        static WeakReference weakReferenceOnRegisterablePublisher;

        Establish context = () =>
        {
            eventBroker = new EventBroker();

            publisher = new SimpleEvent.EventPublisher();
            registerablePublisher = new SimpleEvent.RegisterableEventPublisher();
            subscriber = new SimpleEvent.EventSubscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);

            weakReferenceOnPublisher = new WeakReference(publisher);
            weakReferenceOnRegisterablePublisher = new WeakReference(registerablePublisher);
        };

        Because of = () =>
        {
            publisher = null;
            registerablePublisher = null;
            GC.Collect();
        };

        It should_garbage_collect_publisher_registered_by_property = () =>
            weakReferenceOnPublisher.IsAlive
                .Should().BeFalse("publisher should be collected");

        It should_garbage_collect_subscribers_registered_by_registrar = () =>
            weakReferenceOnRegisterablePublisher.IsAlive
                .Should().BeFalse("publisher should be collected");
    }

    [Subject(Subjects.Cleanup)]
    public class When_the_event_broker_is_disposed
    {
        static EventBroker eventBroker;
        static WeakReference publisher;
        static WeakReference registerablePublisher;
        static WeakReference subscriber;
        static WeakReference registerableSubscriber;

        Establish context = () =>
            {
                eventBroker = new EventBroker();

                var p = new SimpleEvent.EventPublisher();
                var rp = new SimpleEvent.RegisterableEventPublisher();
                var s = new SimpleEvent.EventSubscriber();
                var rs = new SimpleEvent.RegisterableEventSubscriber();

                publisher = new WeakReference(p);
                registerablePublisher = new WeakReference(rp);
                subscriber = new WeakReference(s);
                registerableSubscriber = new WeakReference(rs);

                eventBroker.Register(p);
                eventBroker.Register(rp);
                eventBroker.Register(s);
                eventBroker.Register(rs);
            };

        Because of = () =>
            {
                eventBroker.Dispose();
                
                GC.Collect();
            };

        It should_unregister_publishers_registered_by_attribute = () =>
            publisher.IsAlive
                .Should().BeFalse("publisher should not be referenced anymore");

        It should_unregister_publishers_registered_by_registrar = () =>
            registerablePublisher.IsAlive
                .Should().BeFalse("publisher should not be referenced anymore");

        It should_unregister_subscribers_registered_by_attribtue = () =>
            subscriber.IsAlive
                .Should().BeFalse("subscriber should not be referenced anymore");

        It should_unregister_subscribers_registered_by_registrar = () =>
            registerableSubscriber.IsAlive
                .Should().BeFalse("subscriber should not be referenced anymore");
    }
}
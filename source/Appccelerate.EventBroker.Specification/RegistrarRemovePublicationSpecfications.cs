//-------------------------------------------------------------------------------
// <copyright file="RegistrarRemovePublicationSpecfications.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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
    using System;

    using Appccelerate.Events;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.EventRegistrar)]
    public class When_unregistering_a_registerable_event_publisher_on_the_event_breoker
    {
        static EventBroker eventBroker;
        static SimpleEvent.RegisterableEventPublisher publisher;
        static SimpleEvent.EventSubscriber subscriber;

        Establish context = () =>
            {
                eventBroker = new EventBroker();
                publisher = new SimpleEvent.RegisterableEventPublisher();
                subscriber = new SimpleEvent.EventSubscriber();

                eventBroker.Register(publisher);
                eventBroker.Register(subscriber);

                publisher.Event += delegate { };
            };

        Because of = () =>
            {
                eventBroker.Unregister(publisher);

                publisher.FireEvent(EventArgs.Empty);
            };

        It should_not_relay_fired_events_anymore_to_registered_subscribers = () =>
            subscriber.HandledEvent
                .Should().BeFalse("event should not be relayed to subscribers anymore.");
    }

    [Subject(Subjects.EventRegistrar)]
    public class When_unregistering_a_registerable_event_witch_custom_event_args_publisher_on_the_event_broker
    {
        static EventBroker eventBroker;
        static CustomEvent.RegisterableEventPublisher publisher;
        static CustomEvent.EventSubscriber subscriber;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
            publisher = new CustomEvent.RegisterableEventPublisher();
            subscriber = new CustomEvent.EventSubscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);
        };

        Because of = () =>
        {
            eventBroker.Unregister(publisher);

            publisher.FireEvent(new EventArgs<string>(string.Empty));
        };

        It should_not_relay_fired_events_anymore_to_registered_subscribers = () =>
            subscriber.HandledEvent
                .Should().BeFalse("event should not be relayed to subscribers anymore");
    }
}
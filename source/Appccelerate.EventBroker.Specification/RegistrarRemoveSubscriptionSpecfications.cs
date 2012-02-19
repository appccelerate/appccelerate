//-------------------------------------------------------------------------------
// <copyright file="RegistrarRemoveSubscriptionSpecfications.cs" company="Appccelerate">
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
    public class When_removing_an_registerable_event_subscriber_from_the_event_broker
    {
        static EventBroker eventBroker;
        static SimpleEvent.EventPublisher publisher;
        static SimpleEvent.RegisterableEventSubscriber subscriber;

        Establish context = () =>
            {
                eventBroker = new EventBroker();
                publisher = new SimpleEvent.EventPublisher();
                subscriber = new SimpleEvent.RegisterableEventSubscriber();

                eventBroker.Register(publisher);
                eventBroker.Register(subscriber);
            };

        Because of = () =>
            {
                eventBroker.Unregister(subscriber);

                publisher.FireEvent(EventArgs.Empty);
            };

        It should_not_relay_fired_events_anymore_to_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeFalse("event should not be relayed to subscriber anymore.");
    }

    [Subject(Subjects.EventRegistrar)]
    public class When_removing_an_registerable_event_with_custom_event_args_subscriber_from_the_event_broker
    {
        static EventBroker eventBroker;
        static CustomEvent.EventPublisher publisher;
        static CustomEvent.RegisterableEventSubscriber subscriber;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
            publisher = new CustomEvent.EventPublisher();
            subscriber = new CustomEvent.RegisterableEventSubscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);
        };

        Because of = () =>
        {
            eventBroker.Unregister(subscriber);

            publisher.FireEvent(new EventArgs<string>(string.Empty));
        };

        It should_not_relay_fired_events_anymore_to_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeFalse("event should not be relayed to subscriber anymore.");
    }
}
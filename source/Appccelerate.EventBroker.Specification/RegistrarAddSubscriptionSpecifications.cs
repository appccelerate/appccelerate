//-------------------------------------------------------------------------------
// <copyright file="RegistrarAddSubscriptionSpecifications.cs" company="Appccelerate">
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
    public class When_registering_an_registerable_event_subscription_on_the_event_broker
    {
        static readonly EventArgs SentEventArgs = new EventArgs();

        static EventBroker eventBroker;

        static SimpleEvent.EventPublisher publisher;

        static SimpleEvent.RegisterableEventSubscriber subscriber;

        Establish context = () =>
            {
                eventBroker = new EventBroker();

                publisher = new SimpleEvent.EventPublisher();
                eventBroker.Register(publisher);
            };

        Because of = () =>
            {
                subscriber = new SimpleEvent.RegisterableEventSubscriber();
                eventBroker.Register(subscriber);

                publisher.FireEvent(SentEventArgs);
            };

        It should_relay_fired_events_to_added_subscription = () =>
            subscriber.HandledEvent
                .Should().BeTrue("registered subscribers should be called");

        It should_pass_event_args_to_registered_subscribers = () =>
            subscriber.ReceivedEventArgs
                .Should().BeSameAs(SentEventArgs, "event args should be passed to subscriber");
    }

    [Subject(Subjects.EventRegistrar)]
    public class When_registering_an_registerable_event_with_custom_event_args_subscriber_on_the_event_broker
    {
        static readonly EventArgs<string> SentEventArgs = new EventArgs<string>("custom");

        static EventBroker eventBroker;

        static CustomEvent.EventPublisher publisher;
        static CustomEvent.RegisterableEventSubscriber subscriber;

        Establish context = () =>
        {
            eventBroker = new EventBroker();

            publisher = new CustomEvent.EventPublisher();
            eventBroker.Register(publisher);
        };

        Because of = () =>
        {
            subscriber = new CustomEvent.RegisterableEventSubscriber();
            eventBroker.Register(subscriber);

            publisher.FireEvent(SentEventArgs);
        };

        It should_relay_fired_events_to_added_subscription = () =>
            subscriber.HandledEvent
                .Should().BeTrue("registered subscribers should be called");

        It should_pass_event_args_to_registered_subscribers = () =>
            subscriber.ReceivedEventArgs
                .Should().BeSameAs(SentEventArgs, "event args should be passed to subscriber");
    }
}
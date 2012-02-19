//-------------------------------------------------------------------------------
// <copyright file="RegistrarAddPublicationSpecifications.cs" company="Appccelerate">
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
    public class When_registering_a_registerable_event_publication_on_the_event_broker
    {
        static readonly EventArgs sentEventArgs = new EventArgs();

        static EventBroker eventBroker;

        static SimpleEvent.EventSubscriber subscriber;

        Establish context = () =>
            {
                eventBroker = new EventBroker();

                subscriber = new SimpleEvent.EventSubscriber();
                eventBroker.Register(subscriber);
            };

        Because of = () =>
            {
                var publisher = new SimpleEvent.RegisterableEventPublisher();
                eventBroker.Register(publisher);

                publisher.FireEvent(sentEventArgs);
            };

        It should_relay_fired_events_to_registered_subscribers = () =>
            subscriber.HandledEvent
                .Should().BeTrue("registered subscribers should be called");

        It should_pass_event_args_to_registered_subscribers = () =>
            subscriber.ReceivedEventArgs
                .Should().BeSameAs(sentEventArgs, "event args should be passed to subscriber");
    }

    [Subject(Subjects.EventRegistrar)]
    public class When_registering_an_event_with_custom_event_args_registerable_publication_on_the_event_broker
    {
        static readonly EventArgs<string> sentEventArgs = new EventArgs<string>("custom");

        static EventBroker eventBroker;

        static CustomEvent.EventSubscriber subscriber;

        Establish context = () =>
        {
            eventBroker = new EventBroker();

            subscriber = new CustomEvent.EventSubscriber();
            eventBroker.Register(subscriber);
        };

        Because of = () =>
        {
            var publisher = new CustomEvent.RegisterableEventPublisher();
            eventBroker.Register(publisher);

            publisher.FireEvent(sentEventArgs);
        };

        It should_relay_fired_events_to_registered_subscribers = () =>
            subscriber.HandledEvent
                .Should().BeTrue("registered subscribers should be called");

        It should_pass_event_args_to_registered_subscribers = () =>
            subscriber.ReceivedEventArgs
                .Should().BeSameAs(sentEventArgs, "event args should be passed to subscriber");
    }
}
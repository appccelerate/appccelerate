//-------------------------------------------------------------------------------
// <copyright file="SubscribersWithSenderAndEventArgsSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Registration.Subscribers
{
    using System;
    using System.Collections.Generic;
    using Appccelerate.EventBroker.Handlers;
    using FluentAssertions;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_with_sender_and_event_argument_using_registration_by_attribute
    {
        static readonly EventArgs EventArgs = new EventArgs(); 

        static EventBroker eventBroker;
        static SimpleEvent.EventPublisher publisher;
        static SubscriberWithSenderAndEventArgs subscriber;

        Establish context = () =>
            {
                publisher = new SimpleEvent.EventPublisher();
                subscriber = new SubscriberWithSenderAndEventArgs();

                eventBroker = new EventBroker();

                eventBroker.Register(publisher);
            };

        Because of = () =>
            {
                eventBroker.Register(subscriber);

                publisher.FireEvent(EventArgs);

                eventBroker.Unregister(subscriber);

                publisher.FireEvent(EventArgs);
            };

        It should_call_handler_method_on_subscriber_with_value_of_generic_event_arguments_from_publisher = () =>
            subscriber.ReceivedEventArgValues.Should().Contain(EventArgs);

        It should_call_handler_method_only_as_long_as_subscriber_is_registered = () =>
            subscriber.ReceivedEventArgValues.Should().HaveCount(1, "event should not be routed anymore after subscriber is unregistered.");

        public class SubscriberWithSenderAndEventArgs : SubscriberWithSenderAndEventArgsBase
        {
            [EventSubscription(SimpleEvent.EventTopic, typeof(OnPublisher)), UsedImplicitly]
            public void Handle(object sender, EventArgs eventArgs)
            {
                this.ReceivedEventArgValues.Add(eventArgs);
            }
        }
    }

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_with_sender_and_event_argument_using_registration_by_registrar
    {
        static readonly EventArgs EventArgs = new EventArgs(); 

        static EventBroker eventBroker;
        static SimpleEvent.EventPublisher publisher;
        static SubscriberWithSenderAndEventArgs subscriber;

        Establish context = () =>
        {
            publisher = new SimpleEvent.EventPublisher();
            subscriber = new SubscriberWithSenderAndEventArgs();

            eventBroker = new EventBroker();

            eventBroker.Register(publisher);
        };

        Because of = () =>
        {
            eventBroker.SpecialCasesRegistrar.AddSubscription(SimpleEvent.EventTopic, subscriber, subscriber.Handle, new OnPublisher());

            publisher.FireEvent(EventArgs);

            eventBroker.SpecialCasesRegistrar.RemoveSubscription(SimpleEvent.EventTopic, subscriber, subscriber.Handle);

            publisher.FireEvent(EventArgs);
        };

        It should_call_handler_method_on_subscriber_with_value_of_generic_event_arguments_from_publisher = () =>
            subscriber.ReceivedEventArgValues.Should().Contain(EventArgs);

        It should_call_handler_method_only_as_long_as_subscriber_is_registered = () =>
            subscriber.ReceivedEventArgValues.Should().HaveCount(1, "event should not be routed anymore after subscriber is unregistered.");

        public class SubscriberWithSenderAndEventArgs : SubscriberWithSenderAndEventArgsBase
        {
            public void Handle(object sender, EventArgs eventArgs)
            {
                this.ReceivedEventArgValues.Add(eventArgs);
            }
        }
    }

    public class SubscriberWithSenderAndEventArgsBase
    {
        protected SubscriberWithSenderAndEventArgsBase()
        {
            this.ReceivedEventArgValues = new List<EventArgs>();
        }

        public List<EventArgs> ReceivedEventArgValues { get; private set; }
    }
}
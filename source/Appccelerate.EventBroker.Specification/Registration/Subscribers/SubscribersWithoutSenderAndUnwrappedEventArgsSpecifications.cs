//-------------------------------------------------------------------------------
// <copyright file="SubscribersWithoutSenderAndUnwrappedEventArgsSpecifications.cs" company="Appccelerate">
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
    using Appccelerate.Events;
    using FluentAssertions;
    using Machine.Specifications;

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_with_no_sender_and_unwrapped_generic_event_argument_using_registration_by_attribute
    {
        const string Value = "value";

        static EventBroker eventBroker;
        static Publisher publisher;
        static SubscriberWithoutSenderAndUnwrappedEventArgs subscriber;

        Establish context = () =>
            {
                publisher = new Publisher();
                subscriber = new SubscriberWithoutSenderAndUnwrappedEventArgs();

                eventBroker = new EventBroker();

                eventBroker.Register(publisher);
            };

        Because of = () =>
            {
                eventBroker.Register(subscriber);

                publisher.FireEvent(Value);

                eventBroker.Unregister(subscriber);

                publisher.FireEvent(Value);
            };

        It should_call_handler_method_on_subscriber_with_value_of_generic_event_arguments_from_publisher = () =>
            subscriber.ReceivedEventArgValues.Should().Contain(Value);

        It should_call_handler_method_only_as_long_as_subscriber_is_registered = () =>
            subscriber.ReceivedEventArgValues.Should().HaveCount(1, "event should not be routed anymore after subscriber is unregistered.");

        public class SubscriberWithoutSenderAndUnwrappedEventArgs : SubscriberWithoutSenderAndUnwrappedEventArgsBase
        {
            [EventSubscription(SimpleEvent.EventTopic, typeof(OnPublisher))]
            public void Handle(string value)
            {
                this.ReceivedEventArgValues.Add(value);
            }
        }
    }

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_with_no_sender_and_unwrapped_generic_event_argument_using_registration_by_registrar
    {
        const string Value = "value";

        static EventBroker eventBroker;
        static Publisher publisher;
        static SubscriberWithoutSenderAndUnwrappedEventArgs subscriber;

        Establish context = () =>
        {
            publisher = new Publisher();
            subscriber = new SubscriberWithoutSenderAndUnwrappedEventArgs();

            eventBroker = new EventBroker();

            eventBroker.Register(publisher);
        };

        Because of = () =>
        {
            eventBroker.SpecialCasesRegistrar.AddSubscription<string>(SimpleEvent.EventTopic, subscriber, subscriber.Handle, new OnPublisher());

            publisher.FireEvent(Value);

            eventBroker.SpecialCasesRegistrar.RemoveSubscription<string>(SimpleEvent.EventTopic, subscriber, subscriber.Handle);

            publisher.FireEvent(Value);
        };

        It should_call_handler_method_on_subscriber_with_value_of_generic_event_arguments_from_publisher = () =>
            subscriber.ReceivedEventArgValues.Should().Contain(Value);

        It should_call_handler_method_only_as_long_as_subscriber_is_registered = () =>
            subscriber.ReceivedEventArgValues.Should().HaveCount(1, "event should not be routed anymore after subscriber is unregistered.");

        public class SubscriberWithoutSenderAndUnwrappedEventArgs : SubscriberWithoutSenderAndUnwrappedEventArgsBase
        {
            public void Handle(string value)
            {
                this.ReceivedEventArgValues.Add(value);
            }
        }
    }

    public class Publisher
    {
        [EventPublication(SimpleEvent.EventTopic)]
        public event EventHandler<EventArgs<string>> Event;

        public void FireEvent(string value)
        {
            this.Event(this, new EventArgs<string>(value));
        }
    }

    public class SubscriberWithoutSenderAndUnwrappedEventArgsBase
    {
        public SubscriberWithoutSenderAndUnwrappedEventArgsBase()
        {
            this.ReceivedEventArgValues = new List<object>();
        }

        public List<object> ReceivedEventArgValues { get; private set; }
    }
}
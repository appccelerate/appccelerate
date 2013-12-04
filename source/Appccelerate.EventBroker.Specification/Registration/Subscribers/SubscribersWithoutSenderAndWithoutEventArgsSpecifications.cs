//-------------------------------------------------------------------------------
// <copyright file="SubscribersWithoutSenderAndWithoutEventArgsSpecifications.cs" company="Appccelerate">
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
    using Appccelerate.EventBroker.Handlers;
    using FluentAssertions;
    using Machine.Specifications;

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_with_no_sender_and_no_event_argument_using_registration_by_attribute
    {
        const string Value = "value";

        static EventBroker eventBroker;
        static Publisher publisher;
        static SubscriberWithoutSenderAndWithoutEventArgs subscriber;

        Establish context = () =>
            {
                publisher = new Publisher();
                subscriber = new SubscriberWithoutSenderAndWithoutEventArgs();

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

        It should_call_handler_method = () =>
            subscriber.CallCount.Should().BePositive();

        It should_call_handler_method_only_as_long_as_subscriber_is_registered = () =>
            subscriber.CallCount.Should().Be(1, "event should not be routed anymore after subscriber is unregistered.");

        public class SubscriberWithoutSenderAndWithoutEventArgs : SubscriberWithoutSenderAndWithoutEventArgsBase
        {
            [EventSubscription(SimpleEvent.EventTopic, typeof(OnPublisher))]
            public void Handle()
            {
                this.CallCount++;
            }
        }
    }

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_with_no_sender_and_no_event_argument_using_registration_by_registrar
    {
        const string Value = "value";

        static EventBroker eventBroker;
        static Publisher publisher;
        static SubscriberWithoutSenderAndWithoutEventArgs subscriber;

        Establish context = () =>
        {
            publisher = new Publisher();
            subscriber = new SubscriberWithoutSenderAndWithoutEventArgs();

            eventBroker = new EventBroker();

            eventBroker.Register(publisher);
        };

        Because of = () =>
        {
            eventBroker.SpecialCasesRegistrar.AddSubscription(SimpleEvent.EventTopic, subscriber, subscriber.Handle, new OnPublisher());

            publisher.FireEvent(Value);

            eventBroker.SpecialCasesRegistrar.RemoveSubscription(SimpleEvent.EventTopic, subscriber, subscriber.Handle);

            publisher.FireEvent(Value);
        };

        It should_call_handler_method = () =>
            subscriber.CallCount.Should().BePositive();

        It should_call_handler_method_only_as_long_as_subscriber_is_registered = () =>
            subscriber.CallCount.Should().Be(1, "event should not be routed anymore after subscriber is unregistered.");

        public class SubscriberWithoutSenderAndWithoutEventArgs : SubscriberWithoutSenderAndWithoutEventArgsBase
        {
            public void Handle()
            {
                this.CallCount++;
            }
        }
    }

    public class SubscriberWithoutSenderAndWithoutEventArgsBase
    {
        public SubscriberWithoutSenderAndWithoutEventArgsBase()
        {
            this.CallCount = 0;
        }

        public int CallCount { get; protected set; }
    }
}
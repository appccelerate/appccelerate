//-------------------------------------------------------------------------------
// <copyright file="SubscribersWithoutSenderSpecifications.cs" company="Appccelerate">
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

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_without_sender_using_registration_by_attribute
    {
        static readonly EventArgs EventArgs = new EventArgs();

        static EventBroker eventBroker;
        static SimpleEvent.EventPublisher publisher;
        static SubscriberWithoutSender subscriber;

        Establish context = () =>
            {
                publisher = new SimpleEvent.EventPublisher();
                subscriber = new SubscriberWithoutSender();

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

        It should_call_handler_method_on_subscriber_with_event_arguments_from_publisher = () =>
            subscriber.ReceivedEventArgs.Should().Contain(EventArgs);

        It should_call_handler_method_only_as_long_as_subscriber_is_registered = () =>
            subscriber.ReceivedEventArgs.Should().HaveCount(1, "event should not be routed anymore after subscriber is unregistered.");

        public class SubscriberWithoutSender : SubscriberWithoutSenderBase
        {
            [EventSubscription(SimpleEvent.EventTopic, typeof(OnPublisher))]
            public void Handle(EventArgs e)
            {
                this.ReceivedEventArgs.Add(e);
            }
        }
    }

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_without_sender_using_registration_by_registrar
    {
        static readonly EventArgs EventArgs = new EventArgs();

        static EventBroker eventBroker;
        static SimpleEvent.EventPublisher publisher;
        static SubscriberWithoutSender subscriber;

        Establish context = () =>
        {
            publisher = new SimpleEvent.EventPublisher();
            subscriber = new SubscriberWithoutSender();

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

        It should_call_handler_method_on_subscriber_with_event_arguments_from_publisher = () =>
            subscriber.ReceivedEventArgs.Should().Contain(EventArgs);

        It should_call_handler_method_only_as_long_as_subscriber_is_registered = () =>
            subscriber.ReceivedEventArgs.Should().HaveCount(1, "event should not be routed anymore after subscriber is unregistered.");

        public class SubscriberWithoutSender : SubscriberWithoutSenderBase
        {
            public void Handle(EventArgs e)
            {
                this.ReceivedEventArgs.Add(e);
            }
        }
    }

    public class SubscriberWithoutSenderBase
    {
        public SubscriberWithoutSenderBase()
        {
            this.ReceivedEventArgs = new List<EventArgs>();
        }

        public List<EventArgs> ReceivedEventArgs { get; private set; }
    }
}
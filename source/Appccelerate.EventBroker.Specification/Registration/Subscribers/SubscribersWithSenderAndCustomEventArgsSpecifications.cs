//-------------------------------------------------------------------------------
// <copyright file="SubscribersWithSenderAndCustomEventArgsSpecifications.cs" company="Appccelerate">
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
    using Machine.Specifications.Annotations;

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_with_sender_and_custom_event_argument_using_registration_by_attribute
    {
        static readonly EventArgs<string> EventArgs = new EventArgs<string>("test"); 

        static EventBroker eventBroker;
        static CustomEvent.EventPublisher publisher;
        static SubscriberWithSenderAndCustomEventArgs subscriber;

        Establish context = () =>
            {
                publisher = new CustomEvent.EventPublisher();
                subscriber = new SubscriberWithSenderAndCustomEventArgs();

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

        public class SubscriberWithSenderAndCustomEventArgs : SubscriberWithSenderAndCustomEventArgsBase
        {
            [EventSubscription(CustomEvent.EventTopic, typeof(OnPublisher)), UsedImplicitly]
            public void Handle(object sender, EventArgs<string> eventArgs)
            {
                this.ReceivedEventArgValues.Add(eventArgs);
            }
        }
    }

    [Subject(Subscribers.RegisteringHandlerMethods)]
    public class When_defining_a_handler_method_with_sender_and_custom_event_argument_using_registration_by_registrar
    {
        static readonly EventArgs<string> EventArgs = new EventArgs<string>("test"); 

        static EventBroker eventBroker;
        static CustomEvent.EventPublisher publisher;
        static SubscriberWithSenderAndCustomEventArgs subscriber;

        Establish context = () =>
        {
            publisher = new CustomEvent.EventPublisher();
            subscriber = new SubscriberWithSenderAndCustomEventArgs();

            eventBroker = new EventBroker();

            eventBroker.Register(publisher);
        };

        Because of = () =>
        {
            eventBroker.SpecialCasesRegistrar.AddSubscription<EventArgs<string>>(CustomEvent.EventTopic, subscriber, subscriber.Handle, new OnPublisher());

            publisher.FireEvent(EventArgs);

            eventBroker.SpecialCasesRegistrar.RemoveSubscription<EventArgs<string>>(CustomEvent.EventTopic, subscriber, subscriber.Handle);

            publisher.FireEvent(EventArgs);
        };

        It should_call_handler_method_on_subscriber_with_value_of_generic_event_arguments_from_publisher = () =>
            subscriber.ReceivedEventArgValues.Should().Contain(EventArgs);

        It should_call_handler_method_only_as_long_as_subscriber_is_registered = () =>
            subscriber.ReceivedEventArgValues.Should().HaveCount(1, "event should not be routed anymore after subscriber is unregistered.");

        public class SubscriberWithSenderAndCustomEventArgs : SubscriberWithSenderAndCustomEventArgsBase
        {
            public void Handle(object sender, EventArgs<string> eventArgs)
            {
                this.ReceivedEventArgValues.Add(eventArgs);
            }
        }
    }

    public class SubscriberWithSenderAndCustomEventArgsBase
    {
        protected SubscriberWithSenderAndCustomEventArgsBase()
        {
            this.ReceivedEventArgValues = new List<EventArgs>();
        }

        public List<EventArgs> ReceivedEventArgValues { get; private set; }
    }
}
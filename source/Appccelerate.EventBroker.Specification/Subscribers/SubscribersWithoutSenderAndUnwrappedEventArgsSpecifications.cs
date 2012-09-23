//-------------------------------------------------------------------------------
// <copyright file="SubscribersWithoutSenderAndUnwrappedEventArgsSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Subscribers
{
    using System;

    using Appccelerate.EventBroker.Handlers;
    using Appccelerate.Events;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject("Special subscribers")]
    public class When_registering_a_handler_method_with_no_sender_and_unwrapped_generic_event_argument
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
                eventBroker.Register(subscriber);
            };

        Because of = () => publisher.FireEvent(Value);

        It should_call_handler_method_on_subscriber_with_value_of_generic_event_arguments_from_publisher = () =>
            subscriber.ReceivedValue.Should().Be(Value);

        public class Publisher
        {
            [EventPublication(SimpleEvent.EventTopic)]
            public event EventHandler<EventArgs<string>> Event;

            public void FireEvent(string value)
            {
                this.Event(this, new EventArgs<string>(value));
            }
        }

        public class SubscriberWithoutSenderAndUnwrappedEventArgs
        {
            public string ReceivedValue { get; private set; }

            [EventSubscription(SimpleEvent.EventTopic, typeof(OnPublisher))]
            public void Handle(string value)
            {
                this.ReceivedValue = value;
            }
        }
    }
}
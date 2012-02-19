//-------------------------------------------------------------------------------
// <copyright file="RegisterHandlerMethodsDirectlyOnEventBrokerSpecifications.cs" company="Appccelerate">
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

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.RegisterDirectlyOnEventBroker)]
    public class When_firing_an_event_for_which_a_subscriber_was_registered_directly_on_the_event_broker
    {
        static EventBroker eventBroker;
        static SimpleEvent.EventPublisher publisher;
        static Subscriber subscriber;
        static EventArgs sentEventArgs;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
            publisher = new SimpleEvent.EventPublisher();           

            eventBroker.Register(publisher);

            sentEventArgs = new EventArgs();

            subscriber = new Subscriber();
        };

        Because of = () =>
            {
                eventBroker.RegisterHandlerMethod(
                    SimpleEvent.EventTopic,
                    subscriber,
                    subscriber.Handle,
                    new Handlers.Publisher());

                publisher.FireEvent(sentEventArgs);
            };

        It should_call_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeTrue();

        public class Subscriber
        {
            public bool HandledEvent { get; private set; }

            public void Handle(object sender, EventArgs eventArgs)
            {
                this.HandledEvent = true;    
            }
        }
    }

    [Subject(Subjects.RegisterDirectlyOnEventBroker)]
    public class When_firing_an_event_for_which_the_publisher_was_registered_directly_on_the_event_broker
    {
        static EventBroker eventBroker;
        static Publisher publisher;
        static SimpleEvent.EventSubscriber subscriber;
        static EventArgs sentEventArgs;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
            subscriber = new SimpleEvent.EventSubscriber();

            eventBroker.Register(subscriber);

            sentEventArgs = new EventArgs();

            publisher = new Publisher();
        };

        Because of = () =>
        {
            eventBroker.RegisterEvent(
                SimpleEvent.EventTopic,
                publisher,
                "Event",
                HandlerRestriction.None);

            publisher.FireEvent();
        };

        It should_call_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeTrue();

        public class Publisher
        {
            public event EventHandler Event;

            public void FireEvent()
            {
                this.Event(this, EventArgs.Empty);
            }
        }
    }
}
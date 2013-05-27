//-------------------------------------------------------------------------------
// <copyright file="RoutingSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Routing
{
    using System;
    using System.ComponentModel;

    using Appccelerate.Events;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.Events)]
    public class When_firing_an_event_on_a_publisher
    {
        static EventBroker eventBroker;
        static SimpleEvent.EventPublisher publisher;
        static SimpleEvent.EventSubscriber subscriber;
        static EventArgs sentEventArgs; 

        Establish context = () =>
            {
                eventBroker = new EventBroker();
                publisher = new SimpleEvent.EventPublisher();
                subscriber = new SimpleEvent.EventSubscriber();

                eventBroker.Register(publisher);
                eventBroker.Register(subscriber);

                sentEventArgs = new EventArgs(); 
            };

        Because of = () => 
            publisher.FireEvent(sentEventArgs);

        It should_call_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeTrue("event should be handled by subscriber");

        It should_pass_event_args_to_subscriber = () =>
            subscriber.ReceivedEventArgs
                .Should().BeSameAs(sentEventArgs);
    }

    [Subject(Subjects.Events)]
    public class When_firing_an_event_with_custom_event_args_on_a_publisher
    {
        static EventBroker eventBroker;
        static CustomEvent.EventPublisher publisher;
        static CustomEvent.EventSubscriber subscriber;
        static EventArgs<string> sentEventArgs;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
            publisher = new CustomEvent.EventPublisher();
            subscriber = new CustomEvent.EventSubscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);

            sentEventArgs = new EventArgs<string>("test"); 
        };

        Because of = () =>
            publisher.FireEvent(sentEventArgs);

        It should_call_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeTrue("event should be handled by subscriber");

        It should_pass_event_args_to_subscriber = () =>
            subscriber.ReceivedEventArgs
                .Should().BeSameAs(sentEventArgs);
    }

    [Subject(Subjects.Events)]
    public class When_firing_an_event_and_subscriber_returns_result_in_event_args
    {
        const string EventTopic = "topic://topic";

        static EventBroker eventBroker;
        static Publisher publisher;
        static Subscriber subscriber;
        static CancelEventArgs cancelEventArgs;

        Establish context = () =>
            {
                eventBroker = new EventBroker();
                publisher = new Publisher();
                subscriber = new Subscriber();

                eventBroker.Register(publisher);
                eventBroker.Register(subscriber);

                cancelEventArgs = new CancelEventArgs(false);
            };

        Because of = () =>
            {
                publisher.FireEvent(cancelEventArgs);
            };

        It should_return_result_in_event_args = () =>
            cancelEventArgs.Cancel.Should().BeTrue("result value should be returned to caller");

        public class Publisher
        {
            [EventPublication(EventTopic)]
            public event EventHandler<CancelEventArgs> Event;

            public void FireEvent(CancelEventArgs cancelEventArgs)
            {
                this.Event(this, cancelEventArgs);
            }
        }

        public class Subscriber
        {
            [EventSubscription(EventTopic, typeof(Handlers.OnPublisher))]
            public void Handle(object sender, CancelEventArgs cancelEventArgs)
            {
                cancelEventArgs.Cancel = true;
            }
        }
    }

    [Subject(Subjects.Events)]
    public class When_firing_an_event_with_several_subscribers
    {
        static EventBroker eventBroker;
        static SimpleEvent.EventPublisher publisher;
        static SimpleEvent.EventSubscriber subscriber1;
        static SimpleEvent.EventSubscriber subscriber2;
        
        Establish context = () =>
            {
                eventBroker = new EventBroker();
                
                publisher = new SimpleEvent.EventPublisher();
                subscriber1 = new SimpleEvent.EventSubscriber();
                subscriber2 = new SimpleEvent.EventSubscriber();

                eventBroker.Register(publisher);
                eventBroker.Register(subscriber1);
                eventBroker.Register(subscriber2);
            };

        Because of = () =>
            {
                publisher.FireEvent(EventArgs.Empty);
            };

        It should_call_all_subscribers = () =>
            {
                subscriber1.HandledEvent.Should().BeTrue("subscriber1 should be called");
                subscriber2.HandledEvent.Should().BeTrue("subscriber2 should be called");
            };
    }

    [Subject(Subjects.Events)]
    public class When_firing_several_publishers_with_same_event_topic
    {
        static EventBroker eventBroker;
        static SimpleEvent.EventPublisher publisher1;
        static SimpleEvent.EventPublisher publisher2;
        static SimpleEvent.EventSubscriber subscriber;
        
        Establish context = () =>
        {
            eventBroker = new EventBroker();

            publisher1 = new SimpleEvent.EventPublisher();
            publisher2 = new SimpleEvent.EventPublisher();
            subscriber = new SimpleEvent.EventSubscriber();
            
            eventBroker.Register(publisher1);
            eventBroker.Register(publisher2);
            eventBroker.Register(subscriber);
        };

        Because of = () =>
        {
            publisher1.FireEvent(EventArgs.Empty);
            publisher2.FireEvent(EventArgs.Empty);
        };

        It should_call_subscriber_every_time = () =>
        {
            subscriber.CallCount.Should().Be(2, "subscriber should be called for every fire");
        };
    }

    [Subject(Subjects.Events)]
    public class When_firing_an_event_belonging_to_several_event_topics
    {
        const string EventTopic1 = "topic://topic1";
        const string EventTopic2 = "topic://topic2";

        static EventBroker eventBroker;
        static Publisher publisher;
        static Subscriber subscriber;

        Establish context = () =>
        {
            eventBroker = new EventBroker();

            publisher = new Publisher();
            subscriber = new Subscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);
        };

        Because of = () =>
        {
            publisher.FireEvent();
        };

        It should_fire_all_event_topics = () =>
            {
                subscriber.HandledEventTopic1.Should().BeTrue("EventTopic1 should be handled");
                subscriber.HandledEventTopic2.Should().BeTrue("EventTopic2 should be handled");
            };

        public class Publisher
        {
            [EventPublication(EventTopic1)]
            [EventPublication(EventTopic2)]
            public event EventHandler Event;

            public void FireEvent()
            {
                this.Event(this, EventArgs.Empty);
            }
        }

        public class Subscriber
        {
            public bool HandledEventTopic1 { get; private set; }

            public bool HandledEventTopic2 { get; private set; }

            [EventSubscription(EventTopic1, typeof(Handlers.OnPublisher))]
            public void HandleEventTopic1(object sender, EventArgs eventArgs)
            {
                this.HandledEventTopic1 = true;
            }

            [EventSubscription(EventTopic2, typeof(Handlers.OnPublisher))]
            public void HandleEventTopic2(object sender, EventArgs eventArgs)
            {
                this.HandledEventTopic2 = true;
            }
        }
    }

    [Subject(Subjects.Events)]
    public class When_registering_a_subscriber_handler_method_belonging_to_several_event_topics
    {
        const string EventTopic1 = "topic://topic1";
        const string EventTopic2 = "topic://topic2";

        static EventBroker eventBroker;
        static Publisher publisher;
        static Subscriber subscriber;

        Establish context = () =>
        {
            eventBroker = new EventBroker();

            publisher = new Publisher();
            subscriber = new Subscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);
        };

        Because of = () =>
        {
            publisher.FireEvent1();
            publisher.FireEvent2();
        };

        It should_handle_all_registered_event_topics = () =>
        {
            subscriber.CallCount.Should().Be(2);
        };

        public class Publisher
        {
            [EventPublication(EventTopic1)]
            public event EventHandler Event1;

            [EventPublication(EventTopic2)]
            public event EventHandler Event2;

            public void FireEvent1()
            {
                this.Event1(this, EventArgs.Empty);
            }

            public void FireEvent2()
            {
                this.Event2(this, EventArgs.Empty);
            }
        }

        public class Subscriber
        {
            public int CallCount { get; private set; }

            [EventSubscription(EventTopic1, typeof(Handlers.OnPublisher))]
            [EventSubscription(EventTopic2, typeof(Handlers.OnPublisher))]
            public void HandleEventTopic1(object sender, EventArgs eventArgs)
            {
                this.CallCount++;
            }
        }
    }
}
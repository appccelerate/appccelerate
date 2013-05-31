//-------------------------------------------------------------------------------
// <copyright file="DescribeToSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Description
{
    using System;
    using System.Globalization;
    using System.IO;

    using Appccelerate.EventBroker.Matchers.Scope;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.Description)]
    public class When_describing_an_event_broker
    {
        const string SimpleEventTopic = "topic://SimpleEvent";
        const string CustomEventTopic = "topic://CustomEvent";

        static EventBroker eventBroker;
        static StringWriter writer;
        static string description;

        Establish context = () =>
            {
                eventBroker = new EventBroker();

                writer = new StringWriter();

                var publisher = new Publisher();
                var namedPublisher = new NamedPublisher();
                var subscriber = new Subscriber();
                var namedSubscriber = new NamedSubscriber();

                eventBroker.Register(publisher);
                eventBroker.Register(namedPublisher);
                eventBroker.Register(subscriber);
                eventBroker.Register(namedSubscriber);
            };

        Because of = () =>
            {
                eventBroker.DescribeTo(writer);
                description = writer.ToString();
            };

        It should_list_all_event_topics = () =>
            description
                .Should().Contain(SimpleEventTopic)
                .And.Contain(CustomEventTopic);

        It should_list_all_publishers_per_event_topic = () =>
            description
                .Should().Match("*" + SimpleEventTopic + "*" + typeof(Publisher) + "*" + CustomEventTopic + "*")
                .And.Match("*" + SimpleEventTopic + "*" + typeof(NamedPublisher) + "*" + CustomEventTopic + "*")
                .And.Match("*" + CustomEventTopic + "*" + typeof(Publisher) + "*");

        It should_list_all_subscribers_per_event_topic = () =>
            description
                .Should().Match("*" + SimpleEventTopic + "*" + typeof(Subscriber) + "*" + CustomEventTopic + "*")
                .And.Match("*" + CustomEventTopic + "*" + typeof(NamedSubscriber) + "*");

        It should_list_event_name_per_publisher = () =>
            description
                .Should().Match(
                    "*" + 
                    typeof(Publisher) + 
                    "*Event = SimpleEvent*" +
                    typeof(NamedPublisher) + 
                    "*Event = SimpleEvent*" +
                    typeof(Publisher) + 
                    "*Event = CustomEvent*");

        It should_list_event_handler_type_per_publisher = () =>
            description
                .Should().Match(
                    "*" +
                    typeof(Publisher) +
                    "*EventHandler type = System.EventHandler*" +
                    typeof(NamedPublisher) +
                    "*EventHandler type = System.EventHandler*" +
                    typeof(Publisher) +
                    "*EventHandler type = System.EventHandler<Appccelerate.EventBroker.Description.When_describing_an_event_broker+CustomEventArgs>*");

        It should_list_matchers_per_publisher_and_subscriber = () =>
            description
                .Should().Match(
                    "*" +
                    typeof(NamedPublisher) +
                    "*matchers = subscriber name starts with publisher name*" +
                    typeof(Subscriber) +
                    "*matchers = always*");

        It should_list_name_of_named_publishers_and_subscribers = () =>
            description
                .Should().Match(
                    "*" +
                    typeof(NamedPublisher) +
                    "*Name = NamedCustomEventPublisherName*" +
                    typeof(NamedSubscriber) +
                    "*Name = NamedSubscriberName*");

        It should_list_handler_method_per_subscriber = () =>
            description
                .Should().Match(
                    "*" +
                    typeof(Subscriber) +
                    "*Handler method = HandleSimpleEvent*" +
                    typeof(NamedSubscriber) +
                    "*Handler method = HandleCustomEvent*");

        It should_list_handler_type_per_subscriber = () =>
            description
                .Should().Match(
                    "*" +
                    typeof(Subscriber) +
                    "*Handler = Appccelerate.EventBroker.Handlers.OnPublisher*" +
                    typeof(NamedSubscriber) +
                    "*Handler = Appccelerate.EventBroker.Handlers.OnPublisher*");

        It should_list_event_args_type_per_subscriber = () =>
            description
                .Should().Match(
                    "*" +
                    typeof(Subscriber) +
                    "*EventArgs type = System.EventArgs*" +
                    typeof(NamedSubscriber) +
                    "*EventArgs type = Appccelerate.EventBroker.Description.When_describing_an_event_broker+CustomEventArgs, *");
        
        Cleanup stuff = () =>
            writer.Dispose();

        public class Publisher
        {
            [EventPublication(SimpleEventTopic)]
            public event EventHandler SimpleEvent;

            [EventPublication(CustomEventTopic)]
            public event EventHandler<CustomEventArgs> CustomEvent;

            public void FireSimpleEvent()
            {
                this.SimpleEvent(this, EventArgs.Empty);
            }

            public void FireCustomEvent()
            {
                this.CustomEvent(this, null);
            }
        }

        public class NamedPublisher : INamedItem
        {
            [EventPublication("topic://SimpleEvent", typeof(PublishToChildren))]
            public event EventHandler SimpleEvent;

            public string EventBrokerItemName
            {
                get { return "NamedCustomEventPublisherName"; }
            }

            public void FireSimpleEvent()
            {
                this.SimpleEvent(this, EventArgs.Empty);
            }
        }

        public class Subscriber
        {
            [EventSubscription("topic://SimpleEvent", typeof(Handlers.OnPublisher), typeof(SubscribeGlobal))]
            public void HandleSimpleEvent(object sender, EventArgs e)
            {
            }
        }

        public class NamedSubscriber : INamedItem
        {
            public string EventBrokerItemName
            {
                get { return "NamedSubscriberName"; }
            }

            [EventSubscription("topic://CustomEvent", typeof(Handlers.OnPublisher))]
            public void HandleCustomEvent(object sender, CustomEventArgs e)
            {
            }
        }

        public class CustomEventArgs : EventArgs
        {
            public int I { get; set; }

            public override string ToString()
            {
                return this.I.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
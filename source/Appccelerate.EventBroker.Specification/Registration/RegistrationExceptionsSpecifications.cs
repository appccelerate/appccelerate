//-------------------------------------------------------------------------------
// <copyright file="RegistrationExceptionsSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Registration
{
    using System;

    using Appccelerate.EventBroker.Handlers;
    using Appccelerate.EventBroker.Internals.Exceptions;
    using Appccelerate.Events;

    using FluentAssertions;

    using Machine.Specifications;

    public class When_registering_a_publisher_with_multiple_publications_on_same_event_topic : ExceptionsSpecification
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() => 
                eventBroker.Register(new InvalidPublisherWithRepeatedEventPublication()));

        It should_throw_exception = () =>
            exception
                .Should().NotBeNull()
                .And.BeOfType<RepeatedPublicationException>();

        public class InvalidPublisherWithRepeatedEventPublication
        {
            [EventPublication(SimpleEvent.EventTopic)]
            [EventPublication(SimpleEvent.EventTopic)]
            public event EventHandler Event;
        }
    }
    
    public class When_registering_a_publisher_with_invalid_event_signature : ExceptionsSpecification
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
                eventBroker.Register(new InvalidPublisherWithWrongEventSignature()));

        It should_throw_exception = () =>
            exception
                .Should().NotBeNull()
                .And.BeOfType<InvalidPublicationSignatureException>();

        public class InvalidPublisherWithWrongEventSignature
        {
            public delegate int MyEventHandler(string name);

            [EventPublication("topic")]
            public event MyEventHandler SimpleEvent;
        }
    }
    
    public class When_registering_a_subscriber_with_invalid_handler_method_signature : ExceptionsSpecification
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
                eventBroker.Register(new InvalidSubscriberWithWrongSignature()));

        It should_throw_exception = () =>
            exception
                .Should().NotBeNull()
                .And.BeOfType<InvalidSubscriptionSignatureException>();

        public class InvalidSubscriberWithWrongSignature
        {
            [EventSubscription("topic", typeof(Handlers.OnPublisher))]
            public void SimpleEvent(int i, EventArgs e)
            {
            }
        }
    }

    public class When_registering_a_publisher_with_a_static_publication_event : ExceptionsSpecification
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
                eventBroker.Register(new InvalidPublisherWithStaticEvent()));

        It should_throw_exception = () =>
            exception
                .Should().NotBeNull()
                .And.BeOfType<StaticPublisherEventException>();

        public class InvalidPublisherWithStaticEvent
        {
            [EventPublication("topic")]
            public static event EventHandler SimpleEvent;
        }
    }
    
    public class When_registering_a_subscriber_with_a_static_subscription_handler_method : ExceptionsSpecification
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
                eventBroker.Register(new InvalidSubscriberStaticHandler()));

        It should_throw_exception = () =>
            exception
                .Should().NotBeNull()
                .And.BeOfType<StaticSubscriberHandlerException>();

        public class InvalidSubscriberStaticHandler
        {
            [EventSubscription("topic", typeof(Handlers.OnPublisher))]
            public static void SimpleEvent(object sender, EventArgs e)
            {
            }
        }
    }
    
    public class When_registering_a_subscriber_with_handler_type_user_interface_not_on_the_ui_thread : ExceptionsSpecification
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
                eventBroker.Register(new UserInterfaceSubscriber()));

        It should_throw_exception = () =>
            exception
                .Should().NotBeNull()
                .And.BeOfType<NotUserInterfaceThreadException>();

        public class UserInterfaceSubscriber
        {
            [EventSubscription("topic", typeof(Handlers.OnUserInterface))]
            public void SimpleEvent(object sender, EventArgs e)
            {
            }
        }
    }
    
    public class When_registering_a_subscriber_with_event_args_type_not_matching_existing_publication : ExceptionsSpecification
    {
        static Exception exception;
        static SimpleEvent.EventPublisher publisher;
        
        Establish context = () =>
            {
                publisher = new SimpleEvent.EventPublisher();

                eventBroker.Register(publisher);
            };

        Because of = () =>
            exception = Catch.Exception(() =>
                eventBroker.Register(new SubscriberWithWrongEventArgsType()));

        It should_throw_exception = () =>
            exception
                .Should().NotBeNull()
                .And.BeOfType<EventTopicException>();

        public class SubscriberWithWrongEventArgsType
        {
            [EventSubscription(SimpleEvent.EventTopic, typeof(Handlers.OnPublisher))]
            public void Handle(object sender, EventArgs<string> e)
            {
            }
        }
    }

    public class When_registering_a_publisher_with_event_args_type_not_matching_existing_subscription : ExceptionsSpecification
    {
        static Exception exception;
        static CustomEvent.EventSubscriber subscriber;

        Establish context = () =>
        {
            subscriber = new CustomEvent.EventSubscriber();

            eventBroker.Register(subscriber);
        };

        Because of = () =>
            exception = Catch.Exception(() =>
                eventBroker.Register(new PublisherWithWrongEventArgsType()));

        It should_throw_exception = () =>
            exception
                .Should().NotBeNull()
                .And.BeOfType<EventTopicException>();

        public class PublisherWithWrongEventArgsType
        {
            [EventPublication(CustomEvent.EventTopic)]
            public event EventHandler<EventArgs<int>> Event;
        }
    }
    
    public class When_registering_a_publisher_with_private_publication_event : ExceptionsSpecification
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
                eventBroker.Register(new InvalidPublisherPrivateEvent()));

        [Ignore("this is not yet supported.")]
        It should_throw_exception = () =>
            true.Should().BeFalse();

        public class InvalidPublisherPrivateEvent
        {
            [EventPublication("topic")]
            private event EventHandler Event;
        }
    }
    
    [Subject(Subjects.Exceptions)]
    public class ExceptionsSpecification
    {
        protected static EventBroker eventBroker;
        
        Establish context = () =>
            {
                eventBroker = new EventBroker();
            };
    }
}
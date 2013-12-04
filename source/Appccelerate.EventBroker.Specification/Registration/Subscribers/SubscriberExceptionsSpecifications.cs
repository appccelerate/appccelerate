//-------------------------------------------------------------------------------
// <copyright file="SubscriberExceptionsSpecifications.cs" company="Appccelerate">
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
    using Appccelerate.EventBroker.Exceptions;
    using Appccelerate.EventBroker.Extensions;
    using FluentAssertions;
    using Machine.Specifications;

    public class When_a_subscriber_throws_an_exception : SubscriberExceptionsSpecification
    {
        static Exception exception;
        static SimpleEvent.EventPublisher publisher;
        static ThrowingSubscriber subscriber;

        Establish context = () =>
        {
            publisher = new SimpleEvent.EventPublisher();
            subscriber = new ThrowingSubscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);
        };

        Because of = () =>
            exception = Catch.Exception(() => publisher.FireEvent(EventArgs.Empty));

        It should_pass_exception_to_publisher = () =>
            exception
                .Should().NotBeNull()
                .And.Subject.As<Exception>().Message
                    .Should().Be("test");
    }

    public class When_several_subscribers_throw_an_exception : SubscriberExceptionsSpecification
    {
        static Exception exception;
        static SimpleEvent.EventPublisher publisher;
        static ThrowingSubscriber subscriber1;
        static ThrowingSubscriber subscriber2;

        Establish context = () =>
        {
            publisher = new SimpleEvent.EventPublisher();
            subscriber1 = new ThrowingSubscriber();
            subscriber2 = new ThrowingSubscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber1);
            eventBroker.Register(subscriber2);
        };

        Because of = () =>
            exception = Catch.Exception(() => publisher.FireEvent(EventArgs.Empty));

        It should_pass_exception_to_publisher = () =>
            exception
                .Should().NotBeNull()
                .And.Subject.As<Exception>().Message
                    .Should().Be("test");

        It should_stop_executing_subscribers_after_first_exception = () =>
            subscriber2.Handled.Should().BeFalse("second subscriber should not be clled anymore");
    }

    public class When_a_subscriber_throws_an_exception_and_an_extension_handles_it : SubscriberExceptionsSpecification
    {
        static Exception exception;
        static SimpleEvent.EventPublisher publisher;
        static ThrowingSubscriber subscriber;

        Establish context = () =>
        {
            publisher = new SimpleEvent.EventPublisher();
            subscriber = new ThrowingSubscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);

            eventBroker.AddExtension(new ExceptionHandlingExtension());
        };

        Because of = () =>
            exception = Catch.Exception(() => publisher.FireEvent(EventArgs.Empty));

        It should_not_pass_exception_to_publisher = () =>
            exception
                .Should().BeNull();

        public class ExceptionHandlingExtension : EventBrokerExtensionBase
        {
            public override void SubscriberExceptionOccurred(IEventTopicInfo eventTopic, Exception exception, ExceptionHandlingContext context)
            {
                context.SetHandled();
            }
        }
    }

    [Subject(Subjects.Exceptions)]
    public class SubscriberExceptionsSpecification
    {
        protected static EventBroker eventBroker;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
        };

        public class ThrowingSubscriber
        {
            public bool Handled { get; private set; }

            [EventSubscription(SimpleEvent.EventTopic, typeof(Handlers.OnPublisher))]
            public void Handle(object sender, EventArgs eventArgs)
            {
                this.Handled = true;
                throw new Exception("test");
            }
        }
    }
}
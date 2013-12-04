//-------------------------------------------------------------------------------
// <copyright file="InterfaceRegistrationSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.InterfaceRegistration
{
    using System;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.InterfaceRegistration)]
    public class When_registering_objects_with_publications_or_subscriptions_on_interface
    {
        const string EventTopic = "topic://topic";

        static EventBroker eventBroker;
        static Publisher publisher;
        static Subscriber subscriber;
        static EventArgs sentEventArgs;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
            publisher = new Publisher();
            subscriber = new Subscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);

            sentEventArgs = new EventArgs();
        };

        Because of = () =>
            publisher.FireEvent();

        It should_register_the_implementing_event_and_method = () =>
            subscriber.Handled
                .Should().BeTrue("event should be handled by subscriber");

        public interface IPublisher
        {
            [EventPublication(EventTopic)]
            event EventHandler Event;
        }

        public interface ISubscriber
        {
            [EventSubscription(EventTopic, typeof(Handlers.OnPublisher))]
            void Handle(object sender, EventArgs eventArgs);
        }

        public class Publisher : IPublisher
        {
            public event EventHandler Event;

            public void FireEvent()
            {
                this.Event(this, EventArgs.Empty);
            }
        }

        public class Subscriber : ISubscriber
        {
            public bool Handled { get; private set; }

            public void Handle(object sender, EventArgs eventArgs)
            {
                this.Handled = true;
            }
        }
    }
}
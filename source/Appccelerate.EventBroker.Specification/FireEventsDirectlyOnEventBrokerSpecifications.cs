//-------------------------------------------------------------------------------
// <copyright file="FireEventsDirectlyOnEventBrokerSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker
{
    using System;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.Events)]
    public class When_firing_an_event_directly_on_the_event_broker
    {
        const string EventTopic = "topic://topic";
        
        static EventBroker eventBroker;
        static Subscriber subscriber;

        Establish context = () =>
            {
                eventBroker = new EventBroker();

                subscriber = new Subscriber();

                eventBroker.Register(subscriber);
            };

        Because of = () =>
            {
                eventBroker.Fire(EventTopic, new object(), HandlerRestriction.None, new object(), EventArgs.Empty);
            };

        It should_call_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeTrue();

        public class Subscriber
        {
            public bool HandledEvent { get; private set; }

            [EventSubscription(EventTopic, typeof(Handlers.OnPublisher))]
            public void Handle(object sender, EventArgs eventArgs)
            {
                this.HandledEvent = true;
            }
        }
    }
}
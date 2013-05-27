//-------------------------------------------------------------------------------
// <copyright file="RegisterHandlerMethodsDirectlyOnEventBrokerSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Registration.Publishers
{
    using System;
    using FluentAssertions;
    using Machine.Specifications;

    [Subject(Subjects.RegisterDirectlyOnEventBroker)]
    public class When_defining_an_event_publication_using_registration_by_registrar
    {
        static EventBroker eventBroker;
        static Publisher publisher;
        static SimpleEvent.EventSubscriber subscriber;
        static EventArgs sentEventArguments;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
            subscriber = new SimpleEvent.EventSubscriber();

            eventBroker.Register(subscriber);

            sentEventArguments = new EventArgs();

            publisher = new Publisher();
        };

        Because of = () =>
        {
            eventBroker.SpecialCasesRegistrar.AddPublication(
                SimpleEvent.EventTopic,
                publisher,
                "Event",
                HandlerRestriction.None);

            publisher.FireEvent();

            eventBroker.SpecialCasesRegistrar.RemovePublication(
                SimpleEvent.EventTopic,
                publisher,
                "Event");

            publisher.FireEvent();
        };

        It should_call_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeTrue();

        It should_call_subscriber_only_as_long_as_publisher_is_registered = () =>
            subscriber.CallCount.Should().Be(1, "event should not be relayed after unregister");

        public class Publisher
        {
            public event EventHandler Event = delegate { };

            public void FireEvent()
            {
                this.Event(this, EventArgs.Empty);
            }
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="EventBrokerRegisterableSpecifications.cs" company="Appccelerate">
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
    using FluentAssertions;
    using Machine.Specifications;

    [Subject(Subscribers.RegisteringEventBrokerRegisterables)]
    public class When_defining_an_subscriber_as_an_event_broker_registerable
    {
        static EventBroker eventBroker;
        static EventBrokerRegisterableSubscriber subscriber;

        static bool registerWasCalled;
        static bool unregisterWasCalled;

        Establish context = () =>
            {
                registerWasCalled = false;
                unregisterWasCalled = false;

                eventBroker = new EventBroker();
                subscriber = new EventBrokerRegisterableSubscriber();        
            };

        Because of = () =>
            {
                eventBroker.Register(subscriber);

                registerWasCalled = subscriber.RegisterWasCalled;

                eventBroker.Unregister(subscriber);

                unregisterWasCalled = subscriber.UnregisterWasCalled;
            };

        It should_call_register_on_subscriber_when_it_is_registered_on_event_broker = () =>
            registerWasCalled.Should().BeTrue();

        It should_call_unregister_on_subscriber_when_it_is_unregistered_from_event_broker = () =>
            unregisterWasCalled.Should().BeTrue();

        public class EventBrokerRegisterableSubscriber : IEventBrokerRegisterable
        {
            public bool RegisterWasCalled { get; private set; }

            public bool UnregisterWasCalled { get; private set; }

            public void Register(IEventRegistrar eventRegistrar)
            {
                this.RegisterWasCalled = true;
            }

            public void Unregister(IEventRegistrar eventRegistrar)
            {
                this.UnregisterWasCalled = true;
            }
        }
    }
}
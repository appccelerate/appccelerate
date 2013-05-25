//-------------------------------------------------------------------------------
// <copyright file="EventBrokerRegisterablesSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.EventBrokerRegisterables
{
    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.EventRegistrar)]
    public class When_registering_an_event_broker_registerable_on_the_event_broker
    {
        private static EventBroker eventBroker;

        private static Registerable registerable;

        Establish context = () =>
            {
                eventBroker = new EventBroker();
                registerable = new Registerable();
            };

        Because of = () =>
            {
                eventBroker.Register(registerable);
                eventBroker.Unregister(registerable);
            };

        It should_call_register_on_registerable = () =>
            registerable.WasRegistered
                .Should().BeTrue("register should be called");

        private class Registerable : IEventBrokerRegisterable
        {
            public bool WasRegistered { get; private set; }

            public void Register(IEventRegistrar eventRegistrar)
            {
                this.WasRegistered = true;
            }

            public void Unregister(IEventRegistrar eventRegistrar)
            {
            }
        }
    }

    [Subject(Subjects.EventRegistrar)]
    public class When_unregistering_an_event_broker_registerable_on_the_event_broker
    {
        private static EventBroker eventBroker;

        private static Registerable registerable;

        private Establish context = () =>
            {
                eventBroker = new EventBroker();
                registerable = new Registerable();
            };

        private Because of = () =>
            eventBroker.Unregister(registerable);

        private It should_call_unregister_on_registerable = () => 
            registerable.WasUnregistered.Should().BeTrue("register should be called");

        private class Registerable : IEventBrokerRegisterable
        {
            public bool WasUnregistered { get; private set; }

            public void Register(IEventRegistrar eventRegistrar)
            {
            }

            public void Unregister(IEventRegistrar eventRegistrar)
            {
                this.WasUnregistered = true;
            }
        }
    }
}
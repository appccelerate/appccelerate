//-------------------------------------------------------------------------------
// <copyright file="EventArgsSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.Fundamentals.Specification.Events
{
    using System;

    using Appccelerate.Events;

    using FluentAssertions;

    using Machine.Specifications;

    public class When_firing_generic_event_args
    {
        const int Value = 42;

        static Publisher publisher;
        static Subscriber subscriber;

        Establish context = () =>
            { 
                publisher = new Publisher();
                subscriber = new Subscriber();

                subscriber.RegisterEvent(publisher);
            };

        Because of = () => 
            publisher.FireEvent(Value);

        It should_pass_value_to_event_handler = () =>
            subscriber.ReceivedValue
                .Should().Be(Value);

        private class Publisher
        {
            public event EventHandler<EventArgs<int>> AnEvent = delegate { };

            public void FireEvent(int i)
            {
                this.AnEvent(this, new EventArgs<int>(i));
            }
        }

        private class Subscriber
        {
            public int ReceivedValue { get; private set; }

            public void RegisterEvent(Publisher publisher)
            {
                publisher.AnEvent += (sender, eventArgs) => this.ReceivedValue = eventArgs.Value;
            }
        }
    }
}
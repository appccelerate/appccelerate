//-------------------------------------------------------------------------------
// <copyright file="SimpleEvent.cs" company="Appccelerate">
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

    using Appccelerate.EventBroker.Handlers;

    public class SimpleEvent
    {
        public const string EventTopic = "topic://simple.event";

        public class EventPublisher
        {
            [EventPublication(EventTopic)]
            public event EventHandler Event;

            public void FireEvent(EventArgs eventArgs)
            {
                this.Event(this, eventArgs);
            }
        }

        public class RegisterableEventPublisher : IEventBrokerRegisterable
        {
            public event EventHandler Event;

            public void FireEvent(EventArgs eventArgs)
            {
                this.Event(this, eventArgs);
            }

            public void Register(IEventRegistrar eventRegistrar)
            {
                eventRegistrar.AddPublication(EventTopic, this, ref this.Event);
            }

            public void Unregister(IEventRegistrar eventRegistrar)
            {
                eventRegistrar.RemovePublication(EventTopic, this, ref this.Event);
            }
        }

        public class EventSubscriber
        {
            public bool HandledEvent { get; private set; }

            public int CallCount { get; private set; }

            public EventArgs ReceivedEventArgs { get; private set; }

            [EventSubscription(EventTopic, typeof(OnPublisher))]
            public void HandleEvent(object sender, EventArgs eventArgs)
            {
                this.HandledEvent = true;
                this.CallCount++;
                this.ReceivedEventArgs = eventArgs;
            }
        }

        public class RegisterableEventSubscriber : IEventBrokerRegisterable
        {
            public bool HandledEvent { get; private set; }

            public EventArgs ReceivedEventArgs { get; private set; }

            public void HandleEvent(object sender, EventArgs eventArgs)
            {
                this.HandledEvent = true;
                this.ReceivedEventArgs = eventArgs;
            }

            public void Register(IEventRegistrar eventRegistrar)
            {
                eventRegistrar.AddSubscription(EventTopic, this, this.HandleEvent, new OnPublisher());
            }

            public void Unregister(IEventRegistrar eventRegistrar)
            {
                eventRegistrar.RemoveSubscription(EventTopic, this, this.HandleEvent);
            }
        }
    }
}
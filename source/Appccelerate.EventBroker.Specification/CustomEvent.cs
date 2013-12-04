//-------------------------------------------------------------------------------
// <copyright file="CustomEvent.cs" company="Appccelerate">
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

    using Appccelerate.Events;

    public static class CustomEvent
    {
        public const string EventTopic = "topic://custom.event";

        public class EventPublisher
        {
            [EventPublication(EventTopic)]
            public event EventHandler<EventArgs<string>> Event = delegate { };

            public void FireEvent(EventArgs<string> eventArgs)
            {
                this.Event(this, eventArgs);
            }
        }

        public class RegisterableEventPublisher : IEventBrokerRegisterable
        {
            public event EventHandler<EventArgs<string>> Event = delegate { };

            public void FireEvent(EventArgs<string> eventArgs)
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

            public EventArgs<string> ReceivedEventArgs { get; private set; }

            [EventSubscription(EventTopic, typeof(Handlers.OnPublisher))]
            public void HandleEvent(object sender, EventArgs<string> eventArgs)
            {
                this.HandledEvent = true;
                this.ReceivedEventArgs = eventArgs;
            }
        }

        public class RegisterableEventSubscriber : IEventBrokerRegisterable
        {
            public bool HandledEvent { get; private set; }

            public EventArgs<string> ReceivedEventArgs { get; private set; }

            public void HandleEvent(object sender, EventArgs<string> eventArgs)
            {
                this.HandledEvent = true;
                this.ReceivedEventArgs = eventArgs;
            }

            public void Register(IEventRegistrar eventRegistrar)
            {
                eventRegistrar.AddSubscription<EventArgs<string>>(EventTopic, this, this.HandleEvent, new Handlers.OnPublisher());
            }

            public void Unregister(IEventRegistrar eventRegistrar)
            {
                eventRegistrar.RemoveSubscription<EventArgs<string>>(EventTopic, this, this.HandleEvent);
            }
        }
    }
}
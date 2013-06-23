//-------------------------------------------------------------------------------
// <copyright file="DistributedEventBrokerSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker
{
    using System;

    using Appccelerate.DistributedEventBroker.Handlers;
    using Appccelerate.DistributedEventBroker.Messages;
    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Factories;
    using Appccelerate.EventBroker.Handlers;

    using FluentAssertions;

    using Machine.Specifications;

    public class DistributedEventBrokerSpecifications
    {
        protected static EventBroker localEventBroker;

        private static AppDomain remoteAppDomain;

        private static EventFiredHandlerDecorator localEventFiredHandler;

        Establish ctx = () =>
            { 
                remoteAppDomain = AppDomain.CreateDomain("Remote Distributed Event Broker App Domain", AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);

                var initializer = (Initializer)remoteAppDomain.CreateInstanceAndUnwrap(
                    typeof(Initializer).Assembly.FullName, typeof(Initializer).FullName);

                localEventFiredHandler = new EventFiredHandlerDecorator();

                var remoteEventFiredHandler = initializer.Initialize(localEventFiredHandler);

                // With the unit test factory everything becomes synchronous
                localEventBroker = new EventBroker(new UnitTestFactory());
                localEventBroker.AddDistributedExtension(new FakeDistributedEventBrokerExtension(new FakeEventBrokerBus(remoteEventFiredHandler)));
            };

        Cleanup cleanup = () =>
            {
                InternalEventBrokerHolder.InternalEventBroker = null;
                AppDomain.Unload(remoteAppDomain);
            };

        private class Initializer : MarshalByRefObject
        {
            private EventBroker remoteEventBroker;

            private EventFiredHandlerDecorator decorator;

            private RemoteSubscriberAndPublisher remoteSubscriberAndPublisher;

            public EventFiredHandlerDecorator Initialize(EventFiredHandlerDecorator localHandler)
            {
                this.decorator = new EventFiredHandlerDecorator();

                // With the unit test factory everything becomes synchronous
                this.remoteEventBroker = new EventBroker(new UnitTestFactory());
                this.remoteEventBroker.AddDistributedExtension(new FakeDistributedEventBrokerExtension(new FakeEventBrokerBus(localHandler)));

                this.remoteSubscriberAndPublisher = new RemoteSubscriberAndPublisher();
                this.remoteEventBroker.Register(this.remoteSubscriberAndPublisher);

                return this.decorator;
            }
        }

        private class FakeDistributedEventBrokerExtension : DistributedEventBrokerExtensionBase
        {
            public FakeDistributedEventBrokerExtension(IEventBrokerBus eventBrokerBus)
                : base("Specification Distributed Event Broker", eventBrokerBus)
            {
            }

            protected override IHandler CreateHandler()
            {
                return new OnPublisher();
            }
        }

        private class FakeEventBrokerBus : IEventBrokerBus
        {
            private readonly EventFiredHandlerDecorator eventFiredHandler;

            public FakeEventBrokerBus(EventFiredHandlerDecorator eventFiredHandler)
            {
                this.eventFiredHandler = eventFiredHandler;
            }

            public void Publish(IEventFired message)
            {
                this.eventFiredHandler.Handle(message);
            }
        }

        private class EventFiredHandlerDecorator : MarshalByRefObject
        {
            private readonly FakeEventFiredHandler fakeEventFiredHandler;

            public EventFiredHandlerDecorator()
            {
                this.fakeEventFiredHandler = new FakeEventFiredHandler();
            }

            public void Handle(IEventFired message)
            {
                this.fakeEventFiredHandler.Handle(message);
            }
        }

        private class FakeEventFiredHandler : EventFiredHandlerBase
        {
            public FakeEventFiredHandler()
            {
                // We fake here the restriction away in order to do everything asynchronously
                this.Restriction = HandlerRestriction.Synchronous;
            }

            public void Handle(IEventFired message)
            {
                this.DoHandle(message);
            }
        }

        private class RemoteSubscriberAndPublisher
        {
            [EventPublication("topic://TopicFiredByRemoteAndReceivedByLocal")]
            public event EventHandler SomeEvent = delegate { };

            [EventSubscription("topic://TopicFiredByLocalAndReceivedByRemote", typeof(OnPublisher))]
            public void Handle(object sender, EventArgs args)
            {
                this.SomeEvent(this, EventArgs.Empty);
            }
        }
    }

    public class when_event_published : DistributedEventBrokerSpecifications
    {
        private static LocalSubscriberAndPublisher localSubscriberAndPublisher;

        Establish ctx = () =>
            {
                localSubscriberAndPublisher = new LocalSubscriberAndPublisher();

                localEventBroker.Register(localSubscriberAndPublisher);
            };

        Because of = () => localSubscriberAndPublisher.PublishEventWhichIsSubscribedByRemote();

        It should_send_and_receive_events_from_remote = () => localSubscriberAndPublisher.RemoteEventReceived.Should().BeTrue();

        private class LocalSubscriberAndPublisher
        {
            [EventPublication("topic://TopicFiredByLocalAndReceivedByRemote")]
            public event EventHandler SomeEvent = delegate { };

            public bool RemoteEventReceived { get; private set; }

            [EventSubscription("topic://TopicFiredByRemoteAndReceivedByLocal", typeof(OnPublisher))]
            public void Handle(object sender, EventArgs args)
            {
                this.RemoteEventReceived = true;
            }

            public void PublishEventWhichIsSubscribedByRemote()
            {
                this.SomeEvent(this, EventArgs.Empty);
            }
        }
    }
}

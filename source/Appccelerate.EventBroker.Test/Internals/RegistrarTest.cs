//-------------------------------------------------------------------------------
// <copyright file="RegistrarTest.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Appccelerate.EventBroker.Internals.Inspection;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class RegistrarTest
    {
        private const string EventTopic = "topic";

        private const string EventName = "Event";

        private Registrar testee;
        private IFactory factory;
        private IEventTopicHost eventTopicHost;
        private IEventInspector eventInspector;
        private IExtensionHost extensionsHost;

        public RegistrarTest()
        {
            this.factory = A.Fake<IFactory>();
            this.eventTopicHost = A.Fake<IEventTopicHost>();
            this.eventInspector = A.Fake<IEventInspector>();
            this.extensionsHost = A.Fake<IExtensionHost>();

            this.testee = new Registrar(this.factory, this.eventTopicHost, this.eventInspector, this.extensionsHost);
        }

        [Fact]
        public void DoesNothing_WhenUnregisteringANotRegisteredObject()
        {
            var publisher = new Publisher();

            var eventTopic = A.Fake<IEventTopic>();

            EventInfo eventInfo = typeof(Publisher).GetEvent(EventName);
            var publication = new PropertyPublicationScanResult(EventTopic, eventInfo, HandlerRestriction.None, null);
            A.CallTo(() => this.eventInspector.Scan(publisher)).Returns(new ScanResult(new[] { publication }, Enumerable.Empty<PropertySubscriptionScanResult>()));
            A.CallTo(() => this.eventTopicHost.GetEventTopic(EventTopic)).Returns(eventTopic);
            A.CallTo(() => eventTopic.RemovePublication(publisher, EventName)).Returns(null);

            this.testee.Invoking(x => x.Unregister(publisher))
                .ShouldNotThrow();
        }

        [Fact]
        public void DoesNothing_WhenRemovingANotAddedPublication()
        {
            var publisher = new Publisher();

            var eventTopic = A.Fake<IEventTopic>();
            A.CallTo(() => this.eventTopicHost.GetEventTopic(EventTopic)).Returns(eventTopic);
            A.CallTo(() => eventTopic.RemovePublication(publisher, EventName)).Returns(null);

            this.testee.Invoking(x => x.RemovePublication(EventTopic, publisher, EventName))
                .ShouldNotThrow();
        }

        public class Publisher
        {
            private const string Topic = "Topic";

            [EventPublication(Topic)]
            public event EventHandler Event;

            public void Fire()
            {
                this.Event(this, EventArgs.Empty);
            }
        }
    }
}
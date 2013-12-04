//-------------------------------------------------------------------------------
// <copyright file="NServiceBusEventBrokerBusTest.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.NServiceBus
{
    using System;

    using Appccelerate.DistributedEventBroker.Messages;
    using Appccelerate.DistributedEventBroker.NServiceBus.Messages;

    using Moq;

    using global::NServiceBus;

    using Xunit;

    public class NServiceBusEventBrokerBusTest
    {
        private readonly Mock<IBus> serviceBus;
        private readonly NServiceBusEventBrokerBus testee;

        public NServiceBusEventBrokerBusTest()
        {
            this.serviceBus = new Mock<IBus>();

            this.testee = new NServiceBusEventBrokerBus(this.serviceBus.Object);
        }

        [Fact]
        public void Publish_MustPublishMessageOnBus()
        {
            var eventFired = new NServiceBusEventFired();

            this.testee.Publish(eventFired);

            this.serviceBus.Verify(bus => bus.Publish(It.Is<INServiceBusEventFired>(msg => msg.Equals(eventFired))));
        }

        [Fact]
        public void Publish_WhenProvidedMessageIsNotINServiceBusEventFired_ArgumentExceptionMustBeThrown()
        {
            var eventFired = new EventFired();

            Assert.Throws<ArgumentException>(() => this.testee.Publish(eventFired));
        }
    }
}

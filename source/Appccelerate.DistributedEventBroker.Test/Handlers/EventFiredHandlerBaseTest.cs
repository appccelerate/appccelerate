//-------------------------------------------------------------------------------
// <copyright file="EventFiredHandlerBaseTest.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.Handlers
{
    using System;
    using EventBroker;
    using Events;
    using Messages;
    using Moq;
    using Xunit;

    public class EventFiredHandlerBaseTest : IDisposable
    {
        private readonly Mock<IEventBroker> internalEventBroker;

        private readonly TestableEventFiredHandlerBase testee;

        public EventFiredHandlerBaseTest()
        {
            this.internalEventBroker = new Mock<IEventBroker>();

            DistributedEventBrokerExtensionBase.InternalEventBroker = this.internalEventBroker.Object;

            this.testee = new TestableEventFiredHandlerBase();
        }

        [Fact]
        public void Handle_MustFireOnEventBroker()
        {
            Mock<IEventFired> message = GetMessage();

            this.testee.TestDoHandle(message.Object);

            this.internalEventBroker.Verify(broker => broker.Fire("topic://Appccelerate.DistributedEventBroker/DISTRIBUTED", this.testee, HandlerRestriction.Asynchronous, this.testee, It.IsAny<EventArgs<IEventFired>>()));
        }

        [Fact]
        public void Handle_MustPassMessageToEventArgs()
        {
            EventArgs<IEventFired> collectedArgs = null;
            Mock<IEventFired> message = GetMessage();

            this.internalEventBroker.Setup(
                broker =>
                broker.Fire(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<HandlerRestriction>(),
                    It.IsAny<object>(),
                    It.IsAny<EventArgs>())).Callback<string, object, HandlerRestriction, object, EventArgs>(
                        (topic, publisher, restriction, sender, args) => collectedArgs = (EventArgs<IEventFired>)args);

            this.testee.TestDoHandle(message.Object);

            Assert.NotNull(collectedArgs);
            Assert.Same(collectedArgs.Value, message.Object);
        }

        public void Dispose()
        {
            DistributedEventBrokerExtensionBase.InternalEventBroker = null;
        }

        private static Mock<IEventFired> GetMessage()
        {
            var message = new Mock<IEventFired>();
            message.SetupGet(m => m.DistributedEventBrokerIdentification).Returns("DISTRIBUTED");
            message.SetupGet(m => m.EventArgs).Returns("SomeData");
            message.SetupGet(m => m.Topic).Returns("topic://SomeTopic");
            return message;
        }

        private class TestableEventFiredHandlerBase : EventFiredHandlerBase
        {
            public void TestDoHandle(IEventFired message)
            {
                this.DoHandle(message);
            }
        }
    }
}
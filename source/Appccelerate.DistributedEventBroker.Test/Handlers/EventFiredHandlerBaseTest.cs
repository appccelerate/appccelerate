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

    using Appccelerate.DistributedEventBroker.Messages;
    using Appccelerate.EventBroker;
    using Appccelerate.Events;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class EventFiredHandlerBaseTest
    {
        private readonly IEventBroker eventBroker;

        private readonly TestableEventFiredHandlerBase testee;

        public EventFiredHandlerBaseTest()
        {
            this.eventBroker = A.Fake<IEventBroker>();

            this.testee = new TestableEventFiredHandlerBase(this.eventBroker);
        }

        [Fact]
        public void FiresOnEventBroker()
        {
            IEventFired message = GetMessage();

            this.testee.TestDoHandle(message);

            A.CallTo(() => this.eventBroker.Fire("topic://Appccelerate.DistributedEventBroker/DISTRIBUTED", this.testee, HandlerRestriction.Asynchronous, this.testee, A<EventArgs<IEventFired>>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public void PassesMessageToEventArgs()
        {
            EventArgs<IEventFired> collectedArgs = null;
            IEventFired message = GetMessage();

            A.CallTo(
                () =>
                this.eventBroker.Fire(
                    A<string>.Ignored,
                    A<object>.Ignored,
                    A<HandlerRestriction>.Ignored,
                    A<object>.Ignored,
                    A<EventArgs>.Ignored)).Invokes(fake => collectedArgs = fake.Arguments.Get<EventArgs<IEventFired>>(4));

            this.testee.TestDoHandle(message);

            collectedArgs.Should().NotBeNull();
            collectedArgs.Value.Should().BeSameAs(message);
        }

        private static IEventFired GetMessage()
        {
            var message = A.Fake<IEventFired>();
            message.DistributedEventBrokerIdentification = "DISTRIBUTED";
            message.EventArgs = "SomeData";
            message.Topic = "topic://SomeTopic";
            return message;
        }

        private class TestableEventFiredHandlerBase : EventFiredHandlerBase
        {
            public TestableEventFiredHandlerBase(IEventBroker eventBroker)
                : base(eventBroker)
            {
            }

            public void TestDoHandle(IEventFired message)
            {
                this.DoHandle(message);
            }
        }
    }
}
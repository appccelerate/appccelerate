//-------------------------------------------------------------------------------
// <copyright file="MassTransitEventFiredHandlerTest.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.MassTransit.Handlers
{
    using Appccelerate.DistributedEventBroker.Messages;

    using Moq;

    using Xunit;

    public class MassTransitEventFiredHandlerTest
    {
        private readonly TestableMassTransitEventFiredHandler testee;

        public MassTransitEventFiredHandlerTest()
        {
            this.testee = new TestableMassTransitEventFiredHandler();
        }

        [Fact]
        public void Consume_MustCallDoHandle()
        {
            this.testee.Consume(new Mock<IEventFired>().Object);

            Assert.True(this.testee.DoHandleWasCalled);
        }

        private class TestableMassTransitEventFiredHandler : MassTransitEventFiredHandler
        {
            public bool DoHandleWasCalled
            {
                get;
                private set;
            }

            protected override void DoHandle(IEventFired message)
            {
                this.DoHandleWasCalled = true;
            }
        }
    }
}
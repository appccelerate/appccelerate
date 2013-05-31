//-------------------------------------------------------------------------------
// <copyright file="NServiceBusEventFiredHandlerTest.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.NServiceBus.Handlers
{
    using Appccelerate.DistributedEventBroker.NServiceBus.Messages;
    using Moq;
    using Xunit;

    public class NServiceBusEventFiredHandlerTest
    {
        private readonly TestableNServiceBusEventFiredHandler testee;

        public NServiceBusEventFiredHandlerTest()
        {
            this.testee = new TestableNServiceBusEventFiredHandler();
        }

        [Fact]
        public void Handle_MustCallDoHandle()
        {
            this.testee.Handle(new Mock<INServiceBusEventFired>().Object);

            Assert.True(this.testee.DoHandleWasCalled);
        }

        private class TestableNServiceBusEventFiredHandler : NServiceBusEventFiredHandler
        {
            public bool DoHandleWasCalled
            {
                get;
                private set;
            }

            protected override void DoHandle(DistributedEventBroker.Messages.IEventFired message)
            {
                this.DoHandleWasCalled = true;
            }
        }
    }
}
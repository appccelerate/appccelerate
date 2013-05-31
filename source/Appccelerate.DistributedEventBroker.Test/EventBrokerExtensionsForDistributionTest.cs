//-------------------------------------------------------------------------------
// <copyright file="EventBrokerExtensionsForDistributionTest.cs" company="Appccelerate">
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
    using Appccelerate.EventBroker;

    using Moq;

    using Xunit;

    public class EventBrokerExtensionsForDistributionTest
    {
        private readonly Mock<IEventBroker> eventBroker;

        private readonly Mock<IDistributedEventBrokerExtension> extension;

        public EventBrokerExtensionsForDistributionTest()
        {
            this.eventBroker = new Mock<IEventBroker>();

            this.extension = new Mock<IDistributedEventBrokerExtension>();
        }

        [Fact]
        public void AddDistributedExtension_MustAddExtension()
        {
            this.eventBroker.Object.AddDistributedExtension(this.extension.Object);

            this.eventBroker.Verify(eb => eb.AddExtension(this.extension.Object));
        }

        [Fact]
        public void AddDistributedExtension_MustManageEventBroker()
        {
            this.eventBroker.Object.AddDistributedExtension(this.extension.Object);

            this.extension.Verify(e => e.Manage(this.eventBroker.Object));
        }

        [Fact]
        public void AddDistributedExtension_WithEventBrokerIdentification_MustManageEventBroker()
        {
            const string EventBrokerIdentification = "ID";

            this.eventBroker.Object.AddDistributedExtension(this.extension.Object, EventBrokerIdentification);

            this.extension.Verify(e => e.Manage(this.eventBroker.Object, EventBrokerIdentification));
        }

        [Fact]
        public void AddDistributedExtension_WithEventBrokerIdentification_MustAddExtension()
        {
            this.eventBroker.Object.AddDistributedExtension(this.extension.Object);

            this.eventBroker.Verify(eb => eb.AddExtension(this.extension.Object));
        }
    }
}
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

    using FakeItEasy;

    using Xunit;

    public class EventBrokerExtensionsForDistributionTest
    {
        private readonly IEventBroker eventBroker;

        private readonly IDistributedEventBrokerExtension extension;

        public EventBrokerExtensionsForDistributionTest()
        {
            this.eventBroker = A.Fake<IEventBroker>();

            this.extension = A.Fake<IDistributedEventBrokerExtension>();
        }

        [Fact]
        public void AddsExtension()
        {
            this.eventBroker.AddDistributedExtension(this.extension);

            A.CallTo(() => this.eventBroker.AddExtension(this.extension)).MustHaveHappened();
        }

        [Fact]
        public void ManagesTheEventBroker()
        {
            this.eventBroker.AddDistributedExtension(this.extension);

            A.CallTo(() => this.extension.Manage(this.eventBroker)).MustHaveHappened();
        }

        [Fact]
        public void WhenEventBrokerIdentification_ManagesTheEventBroker()
        {
            const string EventBrokerIdentification = "ID";

            this.eventBroker.AddDistributedExtension(this.extension, EventBrokerIdentification);

            A.CallTo(() => this.extension.Manage(this.eventBroker, EventBrokerIdentification));
        }

        [Fact]
        public void WhenEventBrokerIdentification_AddsExtension()
        {
            this.eventBroker.AddDistributedExtension(this.extension);

            A.CallTo(() => this.eventBroker.AddExtension(this.extension));
        }
    }
}
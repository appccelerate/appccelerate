//-------------------------------------------------------------------------------
// <copyright file="EventBrokerExtensionsForMappingTest.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker
{
    using Appccelerate.EventBroker;

    using Moq;

    using Xunit;

    public class EventBrokerExtensionsForMappingTest
    {
        private readonly Mock<IEventBroker> eventBroker;

        private readonly Mock<IMappingEventBrokerExtension> extension;

        public EventBrokerExtensionsForMappingTest()
        {
            this.eventBroker = new Mock<IEventBroker>();

            this.extension = new Mock<IMappingEventBrokerExtension>();
        }

        [Fact]
        public void AddMappingExtension_MustAddExtension()
        {
            this.eventBroker.Object.AddMappingExtension(this.extension.Object);

            this.eventBroker.Verify(eb => eb.AddExtension(this.extension.Object));
        }

        [Fact]
        public void AddMappingExtension_MustManageEventBroker()
        {
            this.eventBroker.Object.AddMappingExtension(this.extension.Object);

            this.extension.Verify(e => e.Manage(this.eventBroker.Object));
        }
    }
}
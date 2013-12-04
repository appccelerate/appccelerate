//-------------------------------------------------------------------------------
// <copyright file="DefaultTopicSelectionStrategyTest.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.Strategies
{
    using Appccelerate.EventBroker;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class DefaultTopicSelectionStrategyTest
    {
        private readonly DefaultTopicSelectionStrategy testee;

        public DefaultTopicSelectionStrategyTest()
        {
            this.testee = new DefaultTopicSelectionStrategy();
        }

        [Fact]
        public void AnyTopicIsSelected()
        {
            var topic = A.Fake<IEventTopic>();

            var shouldBeSelected = this.testee.SelectTopic(topic);

            shouldBeSelected.Should().BeTrue();
        }

        [Fact]
        public void TopicIsNotUsedInAnyWayByStrategy()
        {
            var topic = A.Fake<IEventTopic>(builder => builder.Strict());

            this.testee.SelectTopic(topic);
        }
    }
}
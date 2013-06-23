//-------------------------------------------------------------------------------
// <copyright file="DefaultDistributedFactoryTest.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.Factories
{
    using Appccelerate.DistributedEventBroker.Serializer;
    using Appccelerate.DistributedEventBroker.Strategies;

    using FluentAssertions;

    using Xunit;

    public class DefaultDistributedFactoryTest
    {
        private readonly DefaultDistributedFactory testee;

        public DefaultDistributedFactoryTest()
        {
            this.testee = new DefaultDistributedFactory();
        }

        [Fact]
        public void CreatesDefaultMessageFactory()
        {
            var factory = this.testee.CreateMessageFactory();

            factory.Should().BeOfType<DefaultEventMessageFactory>();
        }

        [Fact]
        public void CreatesDefaultSerializer()
        {
            var serializer = this.testee.CreateEventArgsSerializer();

            serializer.Should().BeOfType<BinaryEventArgsSerializer>();
        }

        [Fact]
        public void CreatesDefaulTopicSelectionStrategy()
        {
            var strategy = this.testee.CreateTopicSelectionStrategy();

            strategy.Should().BeOfType<DefaultTopicSelectionStrategy>();
        }
    }
}
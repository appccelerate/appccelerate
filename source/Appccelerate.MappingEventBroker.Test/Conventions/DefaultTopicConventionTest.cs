//-------------------------------------------------------------------------------
// <copyright file="DefaultTopicConventionTest.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker.Conventions
{
    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals;

    using Moq;

    using Xunit;

    public class DefaultTopicConventionTest
    {
        private readonly DefaultTopicConvention testee;

        public DefaultTopicConventionTest()
        {
            this.testee = new DefaultTopicConvention();
        }

        [Fact]
        public void IsCandidate_WhenTopicEqualsDefaultInput_MustReturnTrue()
        {
            var eventTopic = new Mock<IEventTopic>();
            eventTopic.SetupGet(t => t.Uri).Returns(DefaultTopicConvention.EventTopicUriInput);

            Assert.True(this.testee.IsCandidate(eventTopic.Object));
        }

        [Fact]
        public void IsCandidate_WhenTopicEqualsDefaultOutput_MustReturnFalse()
        {
            var eventTopic = new Mock<IEventTopicInfo>();
            eventTopic.SetupGet(t => t.Uri).Returns(DefaultTopicConvention.EventTopicUriOutput);

            Assert.False(this.testee.IsCandidate(eventTopic.Object));
        }

        [Fact]
        public void MapTopic_MustRewriteTopic()
        {
            const string Uri = @"topic://Appccelerate.AutoMapperEventBrokerExtension.Conventions/DefaultTopicConventionTest";
            const string Expected = @"mapped://Appccelerate.AutoMapperEventBrokerExtension.Conventions/DefaultTopicConventionTest";

            var result = this.testee.MapTopic(Uri);

            Assert.Equal(Expected, result);
        }
    }
}
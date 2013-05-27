//-------------------------------------------------------------------------------
// <copyright file="FuncTopicConventionTest.cs" company="Appccelerate">
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
    using System;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals;

    using Moq;

    using Xunit;
    using Xunit.Extensions;

    public class FuncTopicConventionTest
    {
        private readonly Mock<IEventTopicInfo> eventTopicInfo;

        public FuncTopicConventionTest()
        {
            this.eventTopicInfo = new Mock<IEventTopicInfo>();
        }

        [Fact]
        public void IsCandidate_WhenTopicMapperOnly_MustReturnTrue()
        {
            var testee = CreateTesteeWithTopicMapperOnly(from => string.Empty);

            Assert.True(testee.IsCandidate(this.eventTopicInfo.Object));
        }

        [Fact]
        public void IsCandidate_MustExecuteCandidateSelector()
        {
            bool wasCalled = false;
            IEventTopicInfo topicResult = null;

            var testee = CreateTestee(topic => { wasCalled = true; topicResult = topic; return true; }, from => string.Empty);
            
            testee.IsCandidate(this.eventTopicInfo.Object);

            Assert.True(wasCalled, "CandidateSelector was not called!");
            Assert.Same(this.eventTopicInfo.Object, topicResult);
        }

        [Theory]
        [InlineData(true),
        InlineData(false)]
        public void IsCandidate_MustReturnResultOfCandidateSelector(bool candidateSelectorResult)
        {
            var testee = CreateTesteeWithCandidateSelectorOnly(topic => candidateSelectorResult);

            var result = testee.IsCandidate(this.eventTopicInfo.Object);

            Assert.Equal(candidateSelectorResult, result);
        }

        [Fact]
        public void MapTopic_MustExecutedTopicMapper()
        {
            bool wasCalled = false;
            string interceptedInput = string.Empty;
            const string Input = "Input";

            var testee = CreateTesteeWithTopicMapperOnly(from => { wasCalled = true; interceptedInput = from; return string.Empty; });

            testee.MapTopic(Input);

            Assert.True(wasCalled, "TopicSelector was not called!");
            Assert.Equal(Input, interceptedInput);
        }

        [Theory]
        [InlineData("FirstResult"),
        InlineData("SecondResult")]
        public void IsCandidate_MustReturnResultOfTopicMapper(string topicMapperResult)
        {
            var testee = CreateTesteeWithTopicMapperOnly(from => topicMapperResult);

            var result = testee.MapTopic(string.Empty);

            Assert.Equal(topicMapperResult, result);
        }

        private static FuncTopicConvention CreateTesteeWithCandidateSelectorOnly(Func<IEventTopicInfo, bool> candidateSelector)
        {
            return new FuncTopicConvention(candidateSelector, from => string.Empty);
        }

        private static FuncTopicConvention CreateTesteeWithTopicMapperOnly(Func<string, string> topicMapper)
        {
            return new FuncTopicConvention(topicMapper);
        }

        private static FuncTopicConvention CreateTestee(Func<IEventTopicInfo, bool> candidateSelector, Func<string, string> topicMapper)
        {
            return new FuncTopicConvention(candidateSelector, topicMapper);
        }
    }
}
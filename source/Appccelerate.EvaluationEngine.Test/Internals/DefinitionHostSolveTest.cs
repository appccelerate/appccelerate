//-------------------------------------------------------------------------------
// <copyright file="DefinitionHostSolveTest.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Internals
{
    using System;
    using System.Reflection;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class DefinitionHostSolveTest
    {
        private readonly DefinitionHost testee;

        public DefinitionHostSolveTest()
        {
            this.testee = new DefinitionHost();
        }

        [Fact]
        public void ReturnsNull_WhenNoMatchingDefinitionWasAdded()
        {
            this.AddSomeNonMatchingDefinitions();

            var definition = this.testee.FindDefinition<string>(typeof(TestQuestion));

            definition.Should().BeNull();
        }

        [Fact]
        public void ReturnsDefinition_WhenMatchingDefinitionWasAdded()
        {
            this.AddSomeNonMatchingDefinitions();
            var definition = AddDefinition<TestQuestion>();

            var returnedDefinition = this.testee.FindDefinition<string>(typeof(TestQuestion));

            returnedDefinition.Should().BeSameAs(definition);
        }

        [Fact]
        public void ThrowsException_WhenAddingADefinitionWithAQuestionTypeThatIsAlreadyPresent()
        {
            var definition1 = new Definition<TestQuestion, string, Missing, string>();
            var definition2 = new Definition<TestQuestion, string, Missing, string>();

            this.testee.AddDefinition(definition1);

            Action action = () => this.testee.AddDefinition(definition2);

            action.ShouldThrow<InvalidOperationException>();
        }

        private Definition<TQuestion, string, Missing, string> AddDefinition<TQuestion>() where TQuestion : IQuestion<string>
        {
            var definition = new Definition<TQuestion, string, Missing, string>
                {
                    Strategy = new Mock<IStrategy<string, Missing>>().Object,
                    Aggregator = new Mock<IAggregator<string, string, Missing>>().Object
                };

            this.testee.AddDefinition(definition);
            
            return definition;
        }

        private void AddSomeNonMatchingDefinitions()
        {
            this.AddDefinition<AnotherQuestion>();
            this.AddDefinition<YetAnotherQuestion>();
        }

        private class TestQuestion : Question<string>
        {
        }

        private class AnotherQuestion : Question<string>
        {
        }

        private class YetAnotherQuestion : Question<string>
        {
        }
    }
}
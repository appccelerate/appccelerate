//-------------------------------------------------------------------------------
// <copyright file="EngineAnswerTest.cs" company="Appccelerate">
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

    using Appccelerate.EvaluationEngine.Syntax;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class EngineAnswerTest
    {
        private readonly Engine testee;

        private readonly Mock<IDefinitionHost> definitionHostMock;
        private readonly Mock<IDefinitionSyntaxFactory> syntaxFactoryMock;
        private readonly Mock<IDefinitionFactory> definitionFactoryMock;

        public EngineAnswerTest()
        {
            this.definitionHostMock = new Mock<IDefinitionHost>();
            this.syntaxFactoryMock = new Mock<IDefinitionSyntaxFactory>();
            this.definitionFactoryMock = new Mock<IDefinitionFactory>();
            
            this.testee = new Engine(this.definitionHostMock.Object, this.syntaxFactoryMock.Object, this.definitionFactoryMock.Object);
        }

        [Fact]
        public void AnswersQuestionsByExecutingTheStategyReturnedByTheDefinitionReturnedByTheDefinitionHostForTheQuestion()
        {
            const string Answer = "42";
            const string Parameter = "test";

            var question = new TestQuestion();
            var definitionMock = new Mock<IDefinition>();
            var strategyMock = new Mock<IStrategy<string, string>>();

            this.definitionHostMock.Setup(host => host.FindInHierarchyAndCloneDefinition(question)).Returns(definitionMock.Object);
            definitionMock.Setup(definition => definition.GetStrategy<string, string>()).Returns(strategyMock.Object);
            strategyMock.Setup(strategy => strategy.Execute(question, Parameter, definitionMock.Object, It.IsAny<Context>())).Returns(Answer);

            var answer = this.testee.Answer(question, Parameter);

            answer.Should().Be(Answer);
        }

        [Fact]
        public void ThrowsExcpetion_WhenNoDefinitionExists()
        {
            var question = new TestQuestion();

            Action action = () => this.testee.Answer(question, string.Empty);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ThrowsException_WhenNoStrategyExists()
        {
            var question = new TestQuestion();
            var definitionMock = new Mock<IDefinition>();
            
            this.definitionHostMock.Setup(host => host.FindDefinition<string>(typeof(TestQuestion))).Returns(definitionMock.Object);

            Action action = () => this.testee.Answer(question, string.Empty);

            action.ShouldThrow<InvalidOperationException>();
        }

        private class TestQuestion : Question<string, string>
        {
        }
    }
}
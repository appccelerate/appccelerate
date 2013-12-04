//-------------------------------------------------------------------------------
// <copyright file="DefinitionTest.cs" company="Appccelerate">
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

    using Appccelerate.EvaluationEngine.ExpressionProviders;
    using Appccelerate.EvaluationEngine.Expressions;

    using FluentAssertions;

    using Moq;

    using Xunit;

    using Answer = System.String;
    using ExpressionResult = System.Int32;
    using Parameter = System.Double;

    public class DefinitionTest
    {
        private readonly Mock<IStrategy<Answer, Parameter>> strategyMock;
        private readonly Mock<IAggregator<ExpressionResult, Answer, Parameter>> aggregatorMock;
        private readonly Definition<TestQuestion, Answer, Parameter, ExpressionResult> testee;

        public DefinitionTest()
        {
            this.strategyMock = new Mock<IStrategy<Answer, Parameter>>();
            this.aggregatorMock = new Mock<IAggregator<ExpressionResult, Answer, Parameter>>();

            this.testee = new Definition<TestQuestion, Answer, Parameter, ExpressionResult>
                {
                    Strategy = this.strategyMock.Object,
                    Aggregator = this.aggregatorMock.Object,
                };
        }

        [Fact]
        public void ReturnsTypeOfQuestion()
        {
            var questionType = this.testee.QuestionType;

            questionType.Should().Be<TestQuestion>();
        }

        [Fact]
        public void ReturnsStrategy()
        {
            var strategy = this.testee.GetStrategy<Answer, Parameter>();

            strategy.Should().BeSameAs(this.strategyMock.Object);
        }

        [Fact]
        public void ReturnsExpressionProvidersOfExpressionProviderSetsWhichConditionIsMetByTheQuestion()
        {
            var provider1InMatchingSet = new SingleExpressionProvider<TestQuestion, Answer, Parameter, ExpressionResult>(q => new TestExpression());
            var provider2InMatchingSet = new SingleExpressionProvider<TestQuestion, Answer, Parameter, ExpressionResult>(q => new TestExpression());

            this.testee.AddExpressionProviderSet(CreateSet(q => true, provider1InMatchingSet, provider2InMatchingSet));
            this.testee.AddExpressionProviderSet(CreateSet(q => false, new SingleExpressionProvider<TestQuestion, Answer, Parameter, ExpressionResult>(q => new TestExpression())));

            var question = new TestQuestion();

            var providers = this.testee.GetExpressionProviders(question);

            providers
                .Should().HaveCount(2)
                .And.Equal(provider1InMatchingSet, provider2InMatchingSet);
        }

        [Fact]
        public void CopiesStrategyToClone()
        {
            var clone = this.testee.Clone();

            clone.As<Definition<TestQuestion, Answer, Parameter, ExpressionResult>>().Strategy
                .Should().BeSameAs(this.strategyMock.Object);
        }

        [Fact]
        public void CopiesAggregatorToClone()
        {
            var clone = this.testee.Clone();

            clone.As<Definition<TestQuestion, Answer, Parameter, ExpressionResult>>().Aggregator
                .Should().BeSameAs(this.aggregatorMock.Object);
        }

        [Fact]
        public void CopiesExpressionProviderSetsToClone()
        {
            var question = new TestQuestion();

            this.testee.AddExpressionProviderSet(
                CreateSet(
                    q => true,
                    new SingleExpressionProvider<TestQuestion, Answer, Parameter, ExpressionResult>(q => new TestExpression()),
                    new SingleExpressionProvider<TestQuestion, Answer, Parameter, ExpressionResult>(q => new TestExpression())));
            this.testee.AddExpressionProviderSet(
                CreateSet(
                    q => true,
                    new SingleExpressionProvider<TestQuestion, Answer, Parameter, ExpressionResult>(q => new TestExpression())));

            var clone = this.testee.Clone();

            var providers = clone.As<Definition<TestQuestion, Answer, Parameter, ExpressionResult>>().GetExpressionProviders(question);

            providers.Should().HaveCount(3);
        }

        private static ExpressionProviderSet<TestQuestion, string, double, int> CreateSet(Func<TestQuestion, bool> condition, params IExpressionProvider<TestQuestion, string, double, int>[] providers)
        {
            var matchingSet = new ExpressionProviderSet<TestQuestion, string, double, int>
                {
                    Condition = condition
                };

            foreach (var provider in providers)
            {
                matchingSet.ExpressionProviders.Add(provider);
            }

            return matchingSet;
        }

        private class TestQuestion : Question<string, double>
        {
        }

        private class TestExpression : EvaluationExpression<ExpressionResult, Parameter>
        {
            public override int Evaluate(double parameter)
            {
                return (int)parameter;
            }
        }
    }
}
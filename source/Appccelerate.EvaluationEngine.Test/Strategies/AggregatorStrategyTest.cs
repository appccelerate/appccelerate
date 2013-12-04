//-------------------------------------------------------------------------------
// <copyright file="AggregatorStrategyTest.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Strategies
{
    using System;
    using System.Collections.Generic;

    using Appccelerate.EvaluationEngine.ExpressionProviders;
    using Appccelerate.EvaluationEngine.Expressions;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class AggregatorStrategyTest
    {
        [Fact]
        public void ExecutesAggregator()
        {
            var testee = new AggregatorStrategy<TestQuestion, string, int>();

            var context = new Context();
            var aggregatorMock = new Mock<IAggregator<string, string, int>>();

            var expressionProviderMock = new Mock<IExpressionProvider<TestQuestion, string, int, string>>();
            var question = new TestQuestion();
            var expression = new TestExpression<string>();
            expressionProviderMock.Setup(provider => provider.GetExpressions(question)).Returns(new[] { expression });
            
            var definition = new TestableDefinition<string>
                {
                    Aggregator = aggregatorMock.Object,
                    ExpressionProviders = new[] { expressionProviderMock.Object }
                };

            const int Parameter = 7;

            testee.Execute(question, Parameter, definition, context);

            AssertThatAggregatorIsCalledWithExpressionsFromDefinition(aggregatorMock, new[] { expression }, Parameter);
        }

        [Fact]
        public void ExecutesAggregator_WhenStrategyWithMappingIsUsed()
        {
            var testee = new AggregatorStrategy<TestQuestion, string, int, int>();

            var context = new Context();
            var aggregatorMock = new Mock<IAggregator<int, string, int>>();

            var expressionProviderMock = new Mock<IExpressionProvider<TestQuestion, string, int, int>>();
            var question = new TestQuestion();
            var expression = new TestExpression<int>();
            expressionProviderMock.Setup(provider => provider.GetExpressions(question)).Returns(new[] { expression });
            var definition = new TestableDefinition<int>
            {
                Aggregator = aggregatorMock.Object,
                ExpressionProviders = new[] { expressionProviderMock.Object }
            };
            
            const int Parameter = 7;
            
            testee.Execute(question, Parameter, definition, context);

            AssertThatAggregatorIsCalledWithExpressionsFromDefinition(aggregatorMock, new[] { expression }, Parameter);
        }

        [Fact]
        public void Describe()
        {
            var testee = new AggregatorStrategy<TestQuestion, string, int>();

            var description = testee.Describe();

            description.Should().Be("aggregator strategy");
        }

        private static void AssertThatAggregatorIsCalledWithExpressionsFromDefinition<TExpressionResult>(
            Mock<IAggregator<TExpressionResult, string, int>> aggregatorMock, 
            IEnumerable<IExpression<TExpressionResult, int>> expressions, 
            int expectedParameter)
        {
            aggregatorMock.Verify(aggregator => aggregator.Aggregate(expressions, expectedParameter, It.IsAny<Context>()));
        }

        public class TestQuestion : Question<string, int>
        {
        }

        public class TestExpression<TExpressionResult> : EvaluationExpression<TExpressionResult, int>
        {
            public override TExpressionResult Evaluate(int parameter)
            {
                throw new NotImplementedException();
            }
        }

        private class TestableDefinition<TExpressionResult> : IDefinition<TestQuestion, string, int, TExpressionResult>
        {
            public Type QuestionType { get; set; }

            public IStrategy<string, int> Strategy { get; set; }

            public IAggregator<TExpressionResult, string, int> Aggregator { get; set; }

            public IEnumerable<IExpressionProvider<TestQuestion, string, int, TExpressionResult>> ExpressionProviders { get; set; }

            public IStrategy<TAnswer, TParameter> GetStrategy<TAnswer, TParameter>()
            {
                throw new NotImplementedException();
            }

            public IDefinition Clone()
            {
                throw new NotImplementedException();
            }

            public void Merge(IDefinition definition)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IExpressionProvider<TestQuestion, string, int, TExpressionResult>> GetExpressionProviders(IQuestion<string, int> question)
            {
                return this.ExpressionProviders;
            }

            public void AddExpressionProviderSet(IExpressionProviderSet<TestQuestion, string, int, TExpressionResult> expressionProviderSet)
            {
                throw new NotImplementedException();
            }
        }
    }
}
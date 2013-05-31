//-------------------------------------------------------------------------------
// <copyright file="SingleExpressionAggregatorTest.cs" company="Appccelerate">
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
//
// </copyright>
//-------------------------------------------------------------------------------

namespace Appccelerate.EvaluationEngine.Aggregators
{
    using System;
    using System.Collections.Generic;

    using Appccelerate.EvaluationEngine.Expressions;

    using FluentAssertions;

    using Moq;
    using Xunit;

    public class SingleExpressionAggregatorTest
    {
        private readonly SingleExpressionAggregator<string, int> testee;

        public SingleExpressionAggregatorTest()
        {
            this.testee = new SingleExpressionAggregator<string, int>();
        }

        [Fact]
        public void AggregatesByReturningTheValueReturnedByTheExpression()
        {
            const string Result = "Result";
            const int Parameter = 7;

            var expressionMock = new Mock<IExpression<string, int>>();
            expressionMock.Setup(expression => expression.Evaluate(Parameter)).Returns(Result);
            
            string result = this.testee.Aggregate(new[] { expressionMock.Object }, Parameter, null);

            result.Should().Be(Result);
        }

        // TODO: add information about context to error message (maybe provide an own exception type for this)
        [Fact]
        public void ThrowsException_WhenAggregatingNoExpression()
        {
            var expressions = new List<IExpression<string, int>>();
            const int Parameter = 7;
            var context = new Context();

            Action action = () => this.testee.Aggregate(expressions, Parameter, context);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ThrowsException_WhenAggregatingSeveralExpressions()
        {
            var expressions = new List<IExpression<string, int>>
                {
                    new Mock<IExpression<string, int>>().Object, 
                    new Mock<IExpression<string, int>>().Object
                };
            const int Parameter = 7;
            var context = new Context();

            Action action = () => this.testee.Aggregate(expressions, Parameter, context);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void DescribesItself()
        {
            string description = this.testee.Describe();

            description.Should().Be("single expression aggregator", description);
        }
    }
}
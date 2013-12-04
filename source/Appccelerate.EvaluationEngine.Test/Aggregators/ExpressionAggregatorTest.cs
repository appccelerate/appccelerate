//-------------------------------------------------------------------------------
// <copyright file="ExpressionAggregatortest.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Aggregators
{
    using System;
    using System.Linq.Expressions;
    
    using Appccelerate.EvaluationEngine.Expressions;

    using FluentAssertions;

    using Xunit;

    public class ExpressionAggregatorTest
    {
        [Fact]
        public void AggregatesResultsOfEvaluatedExpressionsWithSpecifiedAggregationFunction()
        {
            const string Seed = "";

            var expressions = new IExpression<int, int>[]
                {
                    new TestExpression { Value = 1 },
                    new TestExpression { Value = 2 }
                };

            var testee = new ExpressionAggregator<int, string, int>(Seed, (sum, expressionResult) => sum + expressionResult);
            
            var answer = testee.Aggregate(expressions, 5, new Context());

            answer.Should().Be("67");
        }

        [Fact]
        public void DescribesItselfWithSeedAndAggregationFunction()
        {
            const string Seed = "seed";
            Expression<Func<string, string, string>> aggregateFunc = (aggregate, value) => aggregate + value;

            var testee = new ExpressionAggregator<string, string>(Seed, aggregateFunc);

            string description = testee.Describe();

            description.Should().Be("expression aggregator with seed 'seed' and aggregate function (aggregate, value) => (aggregate + value)");
        }

        private class TestExpression : EvaluationExpression<int, int>
        {
            public int Value { get; set; }

            public override int Evaluate(int parameter)
            {
                return this.Value + parameter;
            }
        }
    }
}
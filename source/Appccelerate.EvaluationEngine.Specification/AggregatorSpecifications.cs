//-------------------------------------------------------------------------------
// <copyright file="AggregatorSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Appccelerate.EvaluationEngine.Expressions;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern.Aggregator)]
    public class When_defining_a_own_aggregator
    {
        private static EvaluationEngine engine;

        private static string answer;

        Establish context = () =>
            {
                engine = new EvaluationEngine();

                engine.Solve<MyQuestion, string>()
                    .AggregateWith(new MyAggregator())
                    .ByEvaluating((q, p) => "hello")
                    .ByEvaluating((q, p) => "world");
            };

        Because of = () =>
            answer = engine.Answer(new MyQuestion());

        It should_use_own_aggregator_to_aggregate_expression_results = () =>
            answer.Should().Be(" hello world");

        public class MyQuestion : IQuestion<string>
        {
            public string Describe()
            {
                return "my question";
            }
        }

        public class MyAggregator : IAggregator<string, string, Missing>
        {
            public string Aggregate(IEnumerable<IExpression<string, Missing>> expressions, Missing parameter, Context context)
            {
                return expressions.Aggregate(string.Empty, (aggregate, expression) => aggregate + " " + expression.Evaluate(Missing.Value));
            }

            public string Describe()
            {
                return "my aggregator";
            }
        }
    }
}
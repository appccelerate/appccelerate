//-------------------------------------------------------------------------------
// <copyright file="LoggingSpecification.cs" company="Appccelerate">
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
    using System;
    using System.Linq;

    using Appccelerate.EvaluationEngine.Extensions;
    using Appccelerate.EvaluationEngine.Internals;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern.Logging)]
    public class When_an_answer_is_calculated
    {
        private static IEvaluationEngine engine;

        private static Logger logExtension;

        Establish context = () =>
            {
                logExtension = new Logger();

                engine = new EvaluationEngine();
                engine.SetLogExtension(logExtension);

                engine.Solve<HowManyFruitsAreThereStartingWith, int, char>()
                    .AggregateWithExpressionAggregator(0, (aggregate, value) => aggregate + value)
                    .ByEvaluating(q => new ParameterizedFruitExpression { Kind = "Apple", Count = 1 })
                    .When(q => true)    
                        .ByEvaluating(q => new[] { new ParameterizedFruitExpression { Kind = "Ananas", Count = 2 }, new ParameterizedFruitExpression { Kind = "Banana", Count = 4 } })
                        .ByEvaluating((q, p) => p == 'B' ? 8 : 0)
                    .When(q => false)
                        .ByEvaluating(q => new ParameterizedFruitExpression { Kind = "Unknown" });
            };

        Because of = () =>
            {
                engine.Answer(new HowManyFruitsAreThereStartingWith(), 'A');
            };

        It should_log_how_answer_was_derived = () =>
            {
                Console.WriteLine(logExtension.FoundAnswerLog);
                logExtension.FoundAnswerLog
                    .Should().Contain(HowManyFruitsAreThereStartingWith.Description, "answered question")
                    .And.Contain("aggregator strategy", "used strategy")
                    .And.Contain("expression aggregator with seed '0' and aggregate function (aggregate, value) => (aggregate + value)", "used aggregator")
                    .And.Contain("Parameter = A", "provided parameter")
                    .And.Contain("Answer = 3", "calculated answer")
                    .And.Contain("1 Apple returned 1", "used expression with result")
                    .And.Contain("2 Ananas returned 2", "used expression with result")
                    .And.Contain("4 Banana returned 0", "used expression with result")
                    .And.Contain("inline expression = (q, p) => IIF((Convert(p) == 66), 8, 0) returned 0", "used expression with result");

                logExtension.FoundAnswerLog.IndexOf("Unknown").Should().Be(-1, "unknwon expression should not be evaluted due to unfulfilled constraint");
            };

        private class Logger : ILogExtension
        {
            public string FoundAnswerLog { get; private set; }

            public void FoundAnswer(Context context)
            {
                var expressions = from expression in context.Expressions
                                  select new { Expression = expression.Expression.Describe(), expression.ExpressionResult };

                this.FoundAnswerLog = string.Format(
                    "Question = {1}{0}Strategy = {2}{0}Aggregator = {3}{0}Parameter = {4}{0}Answer = {5}{0}Expressions = {6}",
                    Environment.NewLine,
                    context.Question.Describe(),
                    context.Strategy.Describe(),
                    context.Aggregator.Describe(),
                    context.Parameter,
                    context.Answer,
                    expressions.Aggregate(string.Empty, (aggregate, value) => aggregate + Environment.NewLine + "    " + value.Expression + " returned " + value.ExpressionResult)); 
            }
        }
    }
}
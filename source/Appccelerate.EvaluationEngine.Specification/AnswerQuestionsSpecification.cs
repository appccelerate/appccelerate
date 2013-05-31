//-------------------------------------------------------------------------------
// <copyright file="AnswerQuestionsSpecification.cs" company="Appccelerate">
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
    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern.Answer)]
    public class When_calling_answer_on_evaluation_engine_with_several_defined_expressions_and_an_expression_aggregator
    {
        private const int NumberOfApples = 3;
        private const int NumberOfBananas = 2;
        
        private static IEvaluationEngine evaluationEngine;
        private static int answer;

        Establish context = () =>
            {
                evaluationEngine = new EvaluationEngine();
            };

        Because of = () =>
            {
                evaluationEngine.Solve<HowManyFruitsAreThere, int>()
                    .AggregateWithExpressionAggregator(0, (aggregate, value) => aggregate + value)
                    .ByEvaluating(q => new FruitCountExpression { Kind = "Apples", NumberOfFruits = NumberOfApples })
                    .ByEvaluating(q => new FruitCountExpression { Kind = "Bananas", NumberOfFruits = NumberOfBananas });

                answer = evaluationEngine.Answer(new HowManyFruitsAreThere());
            };

        It should_aggregate_the_result_of_all_expressions_into_a_single_result = () =>
            {
                answer.Should().Be(NumberOfApples + NumberOfBananas);
            };
    }
}

// ReSharper restore UnusedMember.Global
// ReSharper restore InconsistentNaming
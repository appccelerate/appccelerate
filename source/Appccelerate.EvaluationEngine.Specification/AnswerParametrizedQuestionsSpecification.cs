//-------------------------------------------------------------------------------
// <copyright file="AnswerParametrizedQuestionsSpecification.cs" company="Appccelerate">
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
    public class When_calling_answer_on_evaluation_engine_with_a_parameter
    {
        private const string Parameter = "hello world of questions and answers.";

        private static IEvaluationEngine evaluationEngine;

        private static WordCountExpression expression;

        Establish context = () =>
        {
            evaluationEngine = new EvaluationEngine();
            expression = new WordCountExpression();

            evaluationEngine.Solve<HowManyWordsDoesThisTextHave, int, string>()
                .AggregateWithSingleExpressionAggregator()
                .ByEvaluating(q => expression);
        };

        Because of = () =>
        {
            evaluationEngine.Answer(new HowManyWordsDoesThisTextHave(), Parameter);
        };

        It should_pass_parameter_to_the_evaluation_expressions = () =>
        {
            expression.ReceivedParameter.Should().Be(Parameter);
        };
    }
}

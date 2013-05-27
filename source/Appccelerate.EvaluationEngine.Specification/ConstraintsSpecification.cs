//-------------------------------------------------------------------------------
// <copyright file="ConstraintsSpecification.cs" company="Appccelerate">
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

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
#pragma warning disable 169

namespace Appccelerate.EvaluationEngine
{
    using Machine.Specifications;

    [Subject(Concern.Answer)]
    public class When_calling_answer_with_expressions_with_constraints
    {
        private const string NoConstraint = "N";
        private const string WithTrueConstraint = "T";
        private const string WithFalseConstraint = "F";

        private static IEvaluationEngine engine;

        private static string answer;

        Establish context = () =>
            {
                engine = new EvaluationEngine();

                engine.Solve<WhatIsTheText, string>()
                    .AggregateWithExpressionAggregator(string.Empty, (aggregate, value) => aggregate + value)
                    .ByEvaluating((q, p) => NoConstraint)
                    .When(q => false)
                        .ByEvaluating((q, p) => WithFalseConstraint)
                    .When(q => true)
                        .ByEvaluating((q, p) => WithTrueConstraint);
            };

        Because of = () =>
            {
                answer = engine.Answer(new WhatIsTheText());
            };

        It should_evaluate_expressions_without_constraints = () =>
            {
                answer.ShouldContain(NoConstraint);
            };

        It should_evaluate_expressions_with_fulfilled_constraints = () =>
            {
                answer.ShouldContain(WithTrueConstraint);
            };

        It should_ignore_expressions_with_constraints_that_are_not_fulfilled = () =>
            {
                answer.ShouldNotContain(WithFalseConstraint);
            };
    }
}
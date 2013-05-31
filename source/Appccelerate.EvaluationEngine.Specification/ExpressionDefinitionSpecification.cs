//-------------------------------------------------------------------------------
// <copyright file="ExpressionDefinitionSpecification.cs" company="Appccelerate">
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

    [Subject(Concern.Solve)]
    public class When_defining_several_expressions_with_individual_calls_to_solve : HowManyFruitsAreThereContext
    {
        Because of = () =>
            {
                Engine.Solve<HowManyFruitsAreThere, int>()
                    .ByEvaluating(q => new FruitCountExpression { NumberOfFruits = 2 });

                Engine.Solve<HowManyFruitsAreThere, int>()
                    .ByEvaluating(q => new FruitCountExpression { NumberOfFruits = 3 });

                Answer = Engine.Answer(new HowManyFruitsAreThere());
            };

        It should_evaluate_all_expressions_for_the_question = () =>
            {
                Answer.Should().Be(5);
            };
    }

    [Subject(Concern.Solve)]
    public class When_defining_several_expressions_with_a_single_call_to_solve : HowManyFruitsAreThereContext
    {
        Because of = () =>
        {
            Engine.Solve<HowManyFruitsAreThere, int>()
                .ByEvaluating(q => new FruitCountExpression { NumberOfFruits = 2 })
                .ByEvaluating(q => new FruitCountExpression { NumberOfFruits = 3 });

            Answer = Engine.Answer(new HowManyFruitsAreThere());
        };

        It should_evaluate_all_expressions_for_the_question = () =>
        {
            Answer.Should().Be(5);
        };
    }

    [Subject(Concern.Solve)]
    public class When_defining_several_expressions_with_a_single_call_to_by_evaluating : HowManyFruitsAreThereContext
    {
        Because of = () =>
        {
            Engine.Solve<HowManyFruitsAreThere, int>()
                .ByEvaluating(q => new[] { new FruitCountExpression { NumberOfFruits = 2 }, new FruitCountExpression { NumberOfFruits = 3 } });

            Answer = Engine.Answer(new HowManyFruitsAreThere());
        };

        It should_evaluate_all_expressions_for_the_question = () =>
        {
            Answer.Should().Be(5);
        };
    }

    [Subject(Concern.Solve)]
    public class When_defining_inline_expressions_in_the_call_to_by_evaluating : HowManyFruitsAreThereContext
    {
        Because of = () =>
        {
            Engine.Solve<HowManyFruitsAreThere, int>()
                .ByEvaluating((q, p) => 2)
                .ByEvaluating((q, p) => 3);

            Answer = Engine.Answer(new HowManyFruitsAreThere());
        };

        It should_evaluate_all_expressions_for_the_question = () =>
        {
            Answer.Should().Be(5);
        };
    }

    public class HowManyFruitsAreThereContext
    {
        Establish context = () =>
            {
                Engine = new EvaluationEngine();
                Engine.Solve<HowManyFruitsAreThere, int>()
                    .AggregateWithExpressionAggregator(0, (aggregate, value) => aggregate + value);
            };

        protected static IEvaluationEngine Engine { get; private set; }

        protected static int Answer { get; set; }
    }
}
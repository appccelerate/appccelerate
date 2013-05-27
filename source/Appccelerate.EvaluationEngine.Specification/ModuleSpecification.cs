//-------------------------------------------------------------------------------
// <copyright file="ModuleSpecification.cs" company="Appccelerate">
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

    [Subject(Concern.Modules)]
    public class When_loading_a_module_into_an_evaluation_engine
    {
        private const int NumberOfAnanas = 3;
        private const int NumberOfApples = 2;

        private static IEvaluationEngineModule module;
        private static IEvaluationEngine testee;

        Establish context = () =>
            {
                testee = new EvaluationEngine();
                testee.Solve<HowManyFruitsAreThere, int>()
                    .AggregateWithExpressionAggregator(0, (aggregate, value) => aggregate + value)
                    .ByEvaluating(q => new FruitCountExpression { Kind = "Apples", NumberOfFruits = NumberOfApples });

                module = new FruitModule();
            };

        Because of = () =>
            {
                testee.Load(module);
            };

        It should_use_definitions_from_module_to_answer_questions_too = () =>
            {
                var answer = testee.Answer(new HowManyFruitsAreThere());

                answer.Should().Be(NumberOfApples + NumberOfAnanas);
            };

        private class FruitModule : EvaluationEngineModule
        {
            protected override void Load()
            {
                this.Solve<HowManyFruitsAreThere, int>()
                    .ByEvaluating(q => new FruitCountExpression { Kind = "Ananas", NumberOfFruits = NumberOfAnanas });
            }
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="StrategySpecification.cs" company="Appccelerate">
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
    using System.Reflection;

    using Appccelerate.EvaluationEngine.Internals;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern.Strategy)]
    public class When_defining_an_own_strategy
    {
        private const int TheAnswer = 42;

        private static IEvaluationEngine engine;

        private static int answer;

        Establish context = () =>
            {
                engine = new EvaluationEngine();
            };

        Because of = () =>
            {
                engine.Solve<HowManyFruitsAreThere, int>()
                    .With(new SpecialStrategy());

                answer = engine.Answer(new HowManyFruitsAreThere());
            };

        It should_use_own_strategy_instead_of_default_strategy_to_answer_the_question = () =>
            {
                answer.Should().Be(TheAnswer);
            };

        private class SpecialStrategy : IStrategy<int>
        {
            public int Execute(IQuestion<int, Missing> question, Missing parameter, IDefinition definition, Context context)
            {
                return TheAnswer;
            }

            public string Describe()
            {
                return "my own special strategy";
            }
        }
    }
}
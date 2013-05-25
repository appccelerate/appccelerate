//-------------------------------------------------------------------------------
// <copyright file="NoAggregatorSpecification.cs" company="Appccelerate">
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

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern.Answer)]
    public class When_calling_answer_on_evaluation_engine_and_no_aggregator_is_specified
    {
        private static IEvaluationEngine engine;

        private static Exception exception;

        Establish context = () =>
            {
                engine = new EvaluationEngine();

                engine.Solve<HowManyFruitsAreThere, int>();
            };

        Because of = () =>
            {
                try
                {
                    engine.Answer(new HowManyFruitsAreThere());
                }
                catch (Exception e)
                {
                    exception = e;
                }
            };

        It should_throw_invalid_operation_exception = () =>
            {
                exception.Should().BeOfType<InvalidOperationException>();
            };
    }
}
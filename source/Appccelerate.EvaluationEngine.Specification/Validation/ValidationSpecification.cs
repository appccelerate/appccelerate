//-------------------------------------------------------------------------------
// <copyright file="ValidationSpecification.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Validation
{
    using System.Linq;

    using Appccelerate.EvaluationEngine.Expressions;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern.Validation)]
    public class When_validating_valid_data : DataValidationContext
    {
        private static IValidationResult answer;

        Because of = () =>
            {
                answer = Engine.Answer(new IsDataValid(), new Data { Name = "Tester", Description = "A tester" });
            };

        It should_return_valid_validation_result = () =>
            {
                answer.Valid.ShouldBeTrue();
            };

        It should_return_validation_result_without_violations = () =>
            {
                answer.Violations.ShouldBeEmpty();
            };
    }

    [Subject(Concern.Validation)]
    public class When_validating_invalid_data : DataValidationContext
    {
        private static IValidationResult answer;

        Because of = () =>
        {
            answer = Engine.Answer(new IsDataValid(), new Data { Name = null, Description = "A tester" });
        };

        It should_return_invalid_validation_result = () =>
            {
                answer.Valid.ShouldBeFalse();
            };

        It should_return_validation_result_with_violations = () =>
            {
                answer.Violations.Should().HaveCount(1);
            };

        It should_return_violations_with_reason_set_by_failing_rule = () =>
        {
            answer.Violations.Single().Reason.Should().Be(ViolationReason);
        };
    }

    public class DataValidationContext
    {
        protected const string ViolationReason = "Name is empty";

        Establish context = () =>
            {
                Engine = new EvaluationEngine();

                Engine.Solve<IsDataValid, IValidationResult, Data>()
                    .AggregateWithValidationAggregator()
                    .ByEvaluating(q => new NameSetRule())
                    .ByEvaluating(q => new DescriptionSetRule());
            };

        protected static EvaluationEngine Engine { get; private set; }

        protected class IsDataValid : IQuestion<IValidationResult, Data>
        {
            public string Describe()
            {
                return "Is data valid?";
            }
        }

        protected class Data
        {
            public string Name { get; set; }

            public string Description { get; set; }
        }

        protected class NameSetRule : EvaluationExpression<IValidationResult, Data>
        {
            public override IValidationResult Evaluate(Data data)
            {
                var result = new ValidationResult();

                if (string.IsNullOrEmpty(data.Name))
                {
                    result.Valid = false;
                    result.AddViolation(new ValidationViolation { Reason = ViolationReason });
                }

                return result;
            }
        }

        protected class DescriptionSetRule : EvaluationExpression<IValidationResult, Data>
        {
            public override IValidationResult Evaluate(Data data)
            {
                return new ValidationResult();
            }
        }
    }
}
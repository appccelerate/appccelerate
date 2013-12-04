//-------------------------------------------------------------------------------
// <copyright file="ValidationExtensibilitySpecification.cs" company="Appccelerate">
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

    [Subject(Concern.ValidationExtensibility)]
    public class When_validating_valid_data_with_extended_validation_result : ValidationExtensibilityContext
    {
        private static IMyValidationResult answer;

        Because of = () =>
            {
                answer = Engine.Answer(new IsDataValid(), new Data { Name = "Tester" });
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

    [Subject(Concern.ValidationExtensibility)]
    public class When_validating_invalid_data_with_extended_validation_result : ValidationExtensibilityContext
    {
        private static IMyValidationResult answer;

        Because of = () =>
        {
            answer = Engine.Answer(new IsDataValid(), new Data { Name = null });
        };

        It should_return_invalid_validation_result = () =>
            {
                answer.Valid.ShouldBeFalse();
            };

        It should_return_validation_result_with_violations = () =>
            {
                answer.Violations.Should().HaveCount(1);

                answer.Violations.Single().Reason.Should().Be(NameIsEmptyReason);
                answer.Violations.Single().ViolationHint.Should().Be(Hint);
            };

        It should_return_violations_with_reason_set_by_failing_rule = () =>
            {
                answer.Violations.Single().Reason.Should().Be(NameIsEmptyReason);
            };

        It should_return_violations_with_extended_data = () =>
        {
            answer.Violations.Single().ViolationHint.Should().Be(Hint);
        };
    }

    public class ValidationExtensibilityContext
    {
        protected const string NameIsEmptyReason = "Name is empty";

        protected const string Hint = "A hint";

        private Establish context = () =>
            {
                Engine = new EvaluationEngine();

                Engine.Solve<IsDataValid, IMyValidationResult, Data>()
                    .AggregateWithValidationAggregator(new MyValidationResultFactory())
                    .ByEvaluating(q => new NameSetRule());
            };

        /// <summary>
        /// Validation result with additional data.
        /// </summary>
        protected interface IMyValidationResult : IValidationResult<IMyValidationViolation>
        {
        }

        /// <summary>
        /// Validation violation with additional data.
        /// </summary>
        protected interface IMyValidationViolation : IValidationViolation
        {
            string ViolationHint { get; set; }
        }

        protected static EvaluationEngine Engine { get; private set; }

        protected class Data
        {
            public string Name { get; set; }
        }

        protected class IsDataValid : IQuestion<IMyValidationResult, Data>
        {
            public string Describe()
            {
                return "Is data valid?";
            }
        }

        protected class NameSetRule : MyValidationExpression<Data>
        {
            public override IMyValidationResult Evaluate(Data data)
            {
                var result = this.Factory.CreateValidationResult();

                if (string.IsNullOrEmpty(data.Name))
                {
                    result.Valid = false;

                    var validationViolation = this.Factory.CreateValidationViolation();
                    validationViolation.Reason = ValidationExtensibilityContext.NameIsEmptyReason;
                    validationViolation.ViolationHint = ValidationExtensibilityContext.Hint;
                    result.AddViolation(validationViolation);
                }

                return result;
            }
        }

        protected abstract class MyValidationExpression<TParameter> : EvaluationExpression<IMyValidationResult, TParameter>
        {
            protected MyValidationExpression()
            {
                this.Factory = new MyValidationResultFactory();
            }

            protected IValidationResultFactory<IMyValidationResult, IMyValidationViolation> Factory { get; private set; }
        }

        protected class MyValidationResult : ValidationResult<IMyValidationViolation>, IMyValidationResult
        {
        }

        private class MyValidationResultFactory : IValidationResultFactory<IMyValidationResult, IMyValidationViolation>
        {
            public IMyValidationResult CreateValidationResult()
            {
                return new MyValidationResult();
            }

            public IMyValidationViolation CreateValidationViolation()
            {
                return new MyValidationViolation();
            }
        }

        private class MyValidationViolation : ValidationViolation, IMyValidationViolation
        {
            public string ViolationHint
            {
                get;
                set;
            }
        }
    }
}
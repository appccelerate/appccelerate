//-------------------------------------------------------------------------------
// <copyright file="ValidationAggregator{TValidationResult,TValidationViolation,TParameter}.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Validation.Aggregators
{
    using System.Collections.Generic;

    using Appccelerate.EvaluationEngine.Expressions;

    /// <summary>
    /// Aggregator for validation results.
    /// </summary>
    /// <typeparam name="TValidationResult">The type of the validation result.</typeparam>
    /// <typeparam name="TValidationViolation">The type of the validation violation.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public class ValidationAggregator<TValidationResult, TValidationViolation, TParameter> : IAggregator<TValidationResult, TValidationResult, TParameter>
        where TValidationResult : class, IValidationResult<TValidationViolation>
        where TValidationViolation : IValidationViolation
    {
        private readonly IValidationResultFactory<TValidationResult, TValidationViolation> factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAggregator{TValidationResult, TValidationViolation, TParameter}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public ValidationAggregator(IValidationResultFactory<TValidationResult, TValidationViolation> factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Aggregates the specified expressions.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="context">The context.</param>
        /// <returns>Aggregated validation result.</returns>
        public TValidationResult Aggregate(IEnumerable<IExpression<TValidationResult, TParameter>> expressions, TParameter parameter, Context context)
        {
            Ensure.ArgumentNotNull(expressions, "expressions");

            var result = this.factory.CreateValidationResult();

            foreach (var expression in expressions)
            {
                var expressionResult = expression.Evaluate(parameter);

                if (expressionResult != null)
                {
                    if (!expressionResult.Valid)
                    {
                        result.Valid = false;
                    }

                    foreach (var violation in expressionResult.Violations)
                    {
                        result.AddViolation(violation);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Describes this instance.
        /// </summary>
        /// <returns>Description of this instance.</returns>
        public string Describe()
        {
            return "validation aggregator";
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="AggregatorExtensionMethods.cs" company="Appccelerate">
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
    using System.Linq.Expressions;

    using Appccelerate.EvaluationEngine.Syntax;
    using Appccelerate.EvaluationEngine.Validation;
    using Appccelerate.EvaluationEngine.Validation.Aggregators;

    /// <summary>
    /// Extension methods for simpler aggregator syntax definition.
    /// </summary>
    public static class AggregatorExtensionMethods
    {
        /// <summary>
        /// Defines that an expression aggregator is used to aggregate expressions.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <param name="syntax">The syntax.</param>
        /// <param name="seed">The seed used in the aggregation.</param>
        /// <param name="aggregateFunc">The aggregate function.</param>
        /// <returns>Expression syntax.</returns>
        public static IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> AggregateWithExpressionAggregator<TQuestion, TAnswer, TParameter, TExpressionResult>(
            this IAggregatorSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> syntax, TAnswer seed, Expression<Func<TAnswer, TExpressionResult, TAnswer>> aggregateFunc)
             where TQuestion : IQuestion<TAnswer, TParameter>
        {
            Ensure.ArgumentNotNull(syntax, "syntax");

            return syntax.AggregateWith(new Aggregators.ExpressionAggregator<TExpressionResult, TAnswer, TParameter>(seed, aggregateFunc));
        }

        /// <summary>
        /// Defines that a single expression aggregator is used. Therefore only one single expression may take part in the answer finding.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="syntax">The syntax.</param>
        /// <returns>Expression syntax.</returns>
        public static IConstraintSyntax<TQuestion, TAnswer, TParameter, TAnswer> AggregateWithSingleExpressionAggregator<TQuestion, TAnswer, TParameter>(
            this IAggregatorSyntax<TQuestion, TAnswer, TParameter, TAnswer> syntax)
             where TQuestion : IQuestion<TAnswer, TParameter>
        {
            Ensure.ArgumentNotNull(syntax, "syntax");

            return syntax.AggregateWith(new Aggregators.SingleExpressionAggregator<TAnswer, TParameter>());
        }

        /// <summary>
        /// Defines that a validation aggregator is used.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TValidationResult">The type of the validation result.</typeparam>
        /// <typeparam name="TValidationViolation">The type of the validation violation.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="syntax">The syntax.</param>
        /// <param name="validationResultFactory">The validation result factory.</param>
        /// <returns>Expression syntax</returns>
        public static IConstraintSyntax<TQuestion, TValidationResult, TParameter, TValidationResult> AggregateWithValidationAggregator<TQuestion, TValidationResult, TValidationViolation, TParameter>(
                this IAggregatorSyntax<TQuestion, TValidationResult, TParameter, TValidationResult> syntax,
                IValidationResultFactory<TValidationResult, TValidationViolation> validationResultFactory)
            where TQuestion : IQuestion<TValidationResult, TParameter>
            where TValidationResult : class, IValidationResult<TValidationViolation>
            where TValidationViolation : IValidationViolation
        {
            Ensure.ArgumentNotNull(syntax, "syntax");

            return syntax.AggregateWith(new ValidationAggregator<TValidationResult, TValidationViolation, TParameter>(validationResultFactory));
        }

        /// <summary>
        /// Defines that a validation aggregator is used.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="syntax">The syntax.</param>
        /// <returns>Expression syntax</returns>
        public static IConstraintSyntax<TQuestion, IValidationResult, TParameter, IValidationResult> AggregateWithValidationAggregator<TQuestion, TParameter>(
            this IAggregatorSyntax<TQuestion, IValidationResult, TParameter, IValidationResult> syntax)
             where TQuestion : IQuestion<IValidationResult, TParameter>
        {
            Ensure.ArgumentNotNull(syntax, "syntax");

            return syntax.AggregateWith(new ValidationAggregator<TParameter>());
        }
    }
}
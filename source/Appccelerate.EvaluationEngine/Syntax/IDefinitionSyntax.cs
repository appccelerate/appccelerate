//-------------------------------------------------------------------------------
// <copyright file="IDefinitionSyntax.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Syntax
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Appccelerate.EvaluationEngine.Expressions;

    /// <summary>
    /// The syntax to define how a question can be answered.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public interface IDefinitionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> :
        IStrategySyntax<TQuestion, TAnswer, TParameter, TExpressionResult>,
        IAggregatorSyntax<TQuestion, TAnswer, TParameter, TExpressionResult>,
        IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult>
        where TQuestion : IQuestion<TAnswer, TParameter> 
    { 
    }

    /// <summary>
    /// Syntax used to define a strategy.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public interface IStrategySyntax<TQuestion, TAnswer, TParameter, TExpressionResult> where TQuestion : IQuestion<TAnswer, TParameter>
    {
        /// <summary>
        /// Defines the strategy that is used to answer the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        /// <returns>Aggregator syntax.</returns>
        IAggregatorSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> With(IStrategy<TAnswer, TParameter> strategy);
    }

    /// <summary>
    /// Syntax used to define an aggregator.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public interface IAggregatorSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> where TQuestion : IQuestion<TAnswer, TParameter>
    {
        /// <summary>
        /// Defines the aggregator that is used to aggregate expressions defined for the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="aggregator">The aggregator.</param>
        /// <returns>Expression syntax.</returns>
        IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> AggregateWith(IAggregator<TExpressionResult, TAnswer, TParameter> aggregator);
    }

    /// <summary>
    /// Syntax used to define a constraint.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public interface IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> 
        : IExpressionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult>
        where TQuestion : IQuestion<TAnswer, TParameter>
    {
        /// <summary>
        /// Defines a condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>Expression syntax.</returns>
        IExpressionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> When(Func<TQuestion, bool> condition);
    }

    /// <summary>
    /// Syntax to define expressions used to answer a question.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public interface IExpressionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> where TQuestion : IQuestion<TAnswer, TParameter>
    {
        /// <summary>
        /// Defines an expression that has to be evaluated to answer the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Expression syntax.</returns>
        IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> ByEvaluating(Func<TQuestion, IExpression<TExpressionResult, TParameter>> expressionProvider);

        /// <summary>
        /// Defines expressions that have to be evaluated to answer the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Constraint syntax.</returns>
        IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> ByEvaluating(Func<TQuestion, IEnumerable<IExpression<TExpressionResult, TParameter>>> expressionProvider);

        /// <summary>
        /// Defines an inline expression that has to be evaluated to answer the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Constraint syntax</returns>
        IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> ByEvaluating(Expression<Func<TQuestion, TParameter, TExpressionResult>> expressionProvider);
    }
}
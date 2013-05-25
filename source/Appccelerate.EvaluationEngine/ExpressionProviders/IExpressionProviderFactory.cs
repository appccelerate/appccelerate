//-------------------------------------------------------------------------------
// <copyright file="IExpressionProviderFactory.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.ExpressionProviders
{
    using System;
    using System.Collections.Generic;

    using Appccelerate.EvaluationEngine.Expressions;

    /// <summary>
    /// Factory creating expression providers.
    /// </summary>
    public interface IExpressionProviderFactory
    {
        /// <summary>
        /// Creates a single expression provider.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Expression provider</returns>
        IExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult> CreateSingleExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult>(
            Func<TQuestion, IExpression<TExpressionResult, TParameter>> expressionProvider)
            where TQuestion : IQuestion<TAnswer, TParameter>;

        /// <summary>
        /// Creates a multiple expressions provider.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Expression provider</returns>
        IExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult> CreateMultipleExpressionsProvider<TQuestion, TAnswer, TParameter, TExpressionResult>(
            Func<TQuestion, IEnumerable<IExpression<TExpressionResult, TParameter>>> expressionProvider)
            where TQuestion : IQuestion<TAnswer, TParameter>;
    }
}
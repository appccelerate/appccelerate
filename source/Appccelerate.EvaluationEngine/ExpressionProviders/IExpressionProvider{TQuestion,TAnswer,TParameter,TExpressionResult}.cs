//-------------------------------------------------------------------------------
// <copyright file="IExpressionProvider{TQuestion,TAnswer,TParameter,TExpressionResult}.cs" company="Appccelerate">
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
    using System.Collections.Generic;

    using Appccelerate.EvaluationEngine.Expressions;

    /// <summary>
    /// Provides expressions for a question.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public interface IExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult>
        where TQuestion : IQuestion<TAnswer, TParameter>
    {
        /// <summary>
        /// Gets the expressions.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns>Expressions for the question.</returns>
        IEnumerable<IExpression<TExpressionResult, TParameter>> GetExpressions(TQuestion question);
    }
}
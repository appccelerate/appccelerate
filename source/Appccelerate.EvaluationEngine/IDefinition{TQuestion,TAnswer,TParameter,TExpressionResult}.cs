//-------------------------------------------------------------------------------
// <copyright file="IDefinition{TQuestion,TAnswer,TParameter,TExpressionResult}.cs" company="Appccelerate">
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
    using System.Collections.Generic;

    using Appccelerate.EvaluationEngine.ExpressionProviders;

    /// <summary>
    /// Provides information about how a question can be solved.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public interface IDefinition<TQuestion, TAnswer, TParameter, TExpressionResult> : IDefinition
        where TQuestion : IQuestion<TAnswer, TParameter>
    {
        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        IStrategy<TAnswer, TParameter> Strategy { get; set; }

        /// <summary>
        /// Gets or sets the aggregator.
        /// </summary>
        /// <value>The aggregator.</value>
        IAggregator<TExpressionResult, TAnswer, TParameter> Aggregator { get; set; }

        /// <summary>
        /// Gets the expression providers.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns>A list of expression providers.</returns>
        IEnumerable<IExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult>> GetExpressionProviders(IQuestion<TAnswer, TParameter> question);

        /// <summary>
        /// Adds the expression provider set to this instance.
        /// </summary>
        /// <param name="expressionProviderSet">The set of expression providers.</param>
        void AddExpressionProviderSet(IExpressionProviderSet<TQuestion, TAnswer, TParameter, TExpressionResult> expressionProviderSet);
    }
}
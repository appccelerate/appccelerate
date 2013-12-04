//-------------------------------------------------------------------------------
// <copyright file="IAggregator.cs" company="Appccelerate">
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

    using Appccelerate.EvaluationEngine.Expressions;

    /// <summary>
    /// An aggregator combines the results of individual expressions to a single answer.
    /// </summary>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public interface IAggregator<TExpressionResult, TAnswer, TParameter> : IDescriptionProvider
    {
        /// <summary>
        /// Aggregates the specified expressions to a single answer.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="context">The context.</param>
        /// <returns>The answer.</returns>
        TAnswer Aggregate(IEnumerable<IExpression<TExpressionResult, TParameter>> expressions, TParameter parameter, Context context);
    }
}
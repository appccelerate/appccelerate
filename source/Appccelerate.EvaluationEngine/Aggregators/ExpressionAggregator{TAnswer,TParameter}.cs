//-------------------------------------------------------------------------------
// <copyright file="ExpressionAggregator{TAnswer,TParameter}.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Aggregators
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Aggregates the result of all passed expressions into a single result using an aggregate lambda expression.
    /// Use this class if the result type returned by the expressions is the same as the type of the overall result.
    /// </summary>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public class ExpressionAggregator<TAnswer, TParameter> : ExpressionAggregator<TAnswer, TAnswer, TParameter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionAggregator{TAnswer, TParameter}"/> class.
        /// </summary>
        /// <param name="seed">The seed used in the aggregation.</param>
        /// <param name="aggregateFunc">The aggregate function.</param>
        public ExpressionAggregator(TAnswer seed, Expression<Func<TAnswer, TAnswer, TAnswer>> aggregateFunc)
            : base(seed, aggregateFunc)
        {
        }
    }
}
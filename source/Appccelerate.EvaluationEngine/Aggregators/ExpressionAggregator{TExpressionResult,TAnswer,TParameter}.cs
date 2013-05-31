//-------------------------------------------------------------------------------
// <copyright file="ExpressionAggregator{TExpressionResult,TAnswer,TParameter}.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    using Appccelerate.EvaluationEngine.Expressions;
    
    /// <summary>
    /// Aggregates the result of all passed expressions into a single result using an aggregate lambda expression.
    /// Use this class if the result type returned by the expressions is different from the type of the overall result.
    /// </summary>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public class ExpressionAggregator<TExpressionResult, TAnswer, TParameter> : IAggregator<TExpressionResult, TAnswer, TParameter>
    {
        private readonly TAnswer seed;

        private readonly Expression<Func<TAnswer, TExpressionResult, TAnswer>> aggregateFunc;
        private readonly Func<TAnswer, TExpressionResult, TAnswer> f;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionAggregator{TExpressionResult, TAnswer, TParameter}"/> class.
        /// </summary>
        /// <param name="seed">The seed used in the aggregation.</param>
        /// <param name="aggregateFunc">The aggregate function.</param>
        public ExpressionAggregator(TAnswer seed, Expression<Func<TAnswer, TExpressionResult, TAnswer>> aggregateFunc)
        {
            Ensure.ArgumentNotNull(aggregateFunc, "aggregateFunc");

            this.aggregateFunc = aggregateFunc;
            this.f = aggregateFunc.Compile();
            this.seed = seed;
        }

        /// <summary>
        /// Aggregates the specified expressions.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="context">The context.</param>
        /// <returns>Aggregated result</returns>
        public TAnswer Aggregate(IEnumerable<IExpression<TExpressionResult, TParameter>> expressions, TParameter parameter, Context context)
        {
            Ensure.ArgumentNotNull(context, "context");

            var expressionResults = (from expression in expressions select new { expression, ExpressionResult = expression.Evaluate(parameter) }).ToList();

            foreach (var expressionResult in expressionResults)
            {
                context.Expressions.Add(new Context.ExpressionInfo { Expression = expressionResult.expression, ExpressionResult = expressionResult.ExpressionResult });
            }

            var answer = expressionResults.Select(result => result.ExpressionResult).Aggregate(this.seed, this.f);

            return answer;
        }

        /// <summary>
        /// Describes this instance.
        /// </summary>
        /// <returns>Description of this instance.</returns>
        public string Describe()
        {
            return string.Format(CultureInfo.InvariantCulture, "expression aggregator with seed '{0}' and aggregate function {1}", this.seed, this.aggregateFunc);
        }
    }
}
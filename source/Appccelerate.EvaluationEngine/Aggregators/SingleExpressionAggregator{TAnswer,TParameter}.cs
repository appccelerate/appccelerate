//-------------------------------------------------------------------------------
// <copyright file="SingleExpressionAggregator{TAnswer,TParameter}.cs" company="Appccelerate">
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
    using System.Linq;

    using Appccelerate.EvaluationEngine.Expressions;

    /// <summary>
    /// Aggregator that checks that exactly one expression is defined for the question.
    /// </summary>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public class SingleExpressionAggregator<TAnswer, TParameter> : IAggregator<TAnswer, TAnswer, TParameter>
    {
        /// <summary>
        /// Aggregates the specified expressions.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="context">The context.</param>
        /// <returns>The answer.</returns>
        public TAnswer Aggregate(IEnumerable<IExpression<TAnswer, TParameter>> expressions, TParameter parameter, Context context)
        {
            CheckSingleExpression(expressions);

            return expressions.ElementAt(0).Evaluate(parameter);
        }

        /// <summary>
        /// Describes this instance.
        /// </summary>
        /// <returns>Description of this instance.</returns>
        public string Describe()
        {
            return "single expression aggregator";
        }

        private static void CheckSingleExpression(IEnumerable<IExpression<TAnswer, TParameter>> expressions)
        {
            if (expressions.Count() != 1)
            {
                throw new InvalidOperationException("SingleExpressionAggregator can only handle a single expression.");
            }
        }
    }
}
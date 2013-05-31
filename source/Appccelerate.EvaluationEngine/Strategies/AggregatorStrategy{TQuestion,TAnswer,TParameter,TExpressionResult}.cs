//-------------------------------------------------------------------------------
// <copyright file="AggregatorStrategy{TQuestion,TAnswer,TParameter,TExpressionResult}.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Strategies
{
    using System;
    using System.Linq;

    /// <summary>
    /// Strategy using an aggregator to combine the results of expressions into a single result.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public class AggregatorStrategy<TQuestion, TAnswer, TParameter, TExpressionResult> : IStrategy<TAnswer, TParameter>
            where TQuestion : IQuestion<TAnswer, TParameter>
    {
        /// <summary>
        /// Executes the aggregator with all expressions found in the definition.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="context">The context.</param>
        /// <returns>The answer.</returns>
        public TAnswer Execute(IQuestion<TAnswer, TParameter> question, TParameter parameter, IDefinition definition, Context context)
        {
            Ensure.ArgumentNotNull(context, "context");

            var d = (IDefinition<TQuestion, TAnswer, TParameter, TExpressionResult>)definition;
            var q = (TQuestion)question;

            context.Aggregator = d.Aggregator;

            CheckAggregatorNotNull(d.Aggregator);

            var expressions = from expressionProvider in d.GetExpressionProviders(question)
                              from expression in expressionProvider.GetExpressions(q)
                              select expression;

            var answer = d.Aggregator.Aggregate(expressions, parameter, context);

            return answer;
        }

        /// <summary>
        /// Describes this instance.
        /// </summary>
        /// <returns>Description of this instance.</returns>
        public string Describe()
        {
            return "aggregator strategy";
        }

        private static void CheckAggregatorNotNull(IAggregator<TExpressionResult, TAnswer, TParameter> aggregator)
        {
            if (aggregator == null)
            {
                throw new InvalidOperationException("aggregator must not be null. Set it with a call to Solve on evaluation engine.");
            }
        }
    }
}
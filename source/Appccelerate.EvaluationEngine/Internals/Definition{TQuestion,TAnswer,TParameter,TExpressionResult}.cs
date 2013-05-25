//-------------------------------------------------------------------------------
// <copyright file="Definition{TQuestion,TAnswer,TParameter,TExpressionResult}.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Appccelerate.EvaluationEngine.ExpressionProviders;

    /// <summary>
    /// Holds the strategy, aggregator and expressions to answer a question.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public class Definition<TQuestion, TAnswer, TParameter, TExpressionResult> : IDefinition<TQuestion, TAnswer, TParameter, TExpressionResult>
        where TQuestion : IQuestion<TAnswer, TParameter>
    {
        private readonly List<IExpressionProviderSet<TQuestion, TAnswer, TParameter, TExpressionResult>> expressionProvidersSets;

        /// <summary>
        /// Initializes a new instance of the <see cref="Definition{TQuestion, TAnswer, TParameter, TExpressionResult}"/> class.
        /// </summary>
        public Definition()
        {
            this.expressionProvidersSets = new List<IExpressionProviderSet<TQuestion, TAnswer, TParameter, TExpressionResult>>();
        }

        /// <summary>
        /// Gets the type of the question.
        /// </summary>
        /// <value>The type of the question.</value>
        public Type QuestionType
        {
            get
            {
                return typeof(TQuestion);
            }
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public IStrategy<TAnswer, TParameter> Strategy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the aggregator.
        /// </summary>
        /// <value>The aggregator.</value>
        public IAggregator<TExpressionResult, TAnswer, TParameter> Aggregator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the expression providers.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns>A list of expression providers.</returns>
        /// <value>The expressions.</value>
        public IEnumerable<IExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult>> GetExpressionProviders(IQuestion<TAnswer, TParameter> question)
        {
            TQuestion q = (TQuestion)question;

            var r = from set in this.expressionProvidersSets
                    from expressionProvider in set.ExpressionProviders
                    where set.Condition(q)
                    select expressionProvider;

            return r;
        }

        /// <summary>
        /// Adds the expression provider set.
        /// </summary>
        /// <param name="expressionProviderSet">The set of expression providers.</param>
        public void AddExpressionProviderSet(IExpressionProviderSet<TQuestion, TAnswer, TParameter, TExpressionResult> expressionProviderSet)
        {
            this.expressionProvidersSets.Add(expressionProviderSet);
        }

        /// <summary>
        /// Gets the strategy.
        /// </summary>
        /// <typeparam name="T">The type of the answer. Has to match the type provided by this class."/&gt;</typeparam>
        /// <typeparam name="TP">The type of the parameter.</typeparam>
        /// <returns>The strategy.</returns>
        public IStrategy<T, TP> GetStrategy<T, TP>()
        {
            return (IStrategy<T, TP>)this.Strategy;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public IDefinition Clone()
        {
            var clone = new Definition<TQuestion, TAnswer, TParameter, TExpressionResult>();

            clone.Merge(this);

            return clone;
        }

        /// <summary>
        /// Merges the specified definition into this instance.
        /// If specified, the strategy/aggregator of this instance is overridden.
        /// Expression provider sets are added.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public void Merge(IDefinition definition)
        {
            var def = definition as Definition<TQuestion, TAnswer, TParameter, TExpressionResult>;

            this.Aggregator = def.Aggregator ?? this.Aggregator;

            this.Strategy = def.Strategy ?? this.Strategy;
            
            foreach (var expressionProvider in def.expressionProvidersSets)
            {
                this.expressionProvidersSets.Add(expressionProvider);
            }
        }
    }
}
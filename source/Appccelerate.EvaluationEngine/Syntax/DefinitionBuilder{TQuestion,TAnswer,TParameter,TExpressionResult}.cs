//-------------------------------------------------------------------------------
// <copyright file="DefinitionBuilder{TQuestion,TAnswer,TParameter,TExpressionResult}.cs" company="Appccelerate">
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

    using Appccelerate.EvaluationEngine.ExpressionProviders;
    using Appccelerate.EvaluationEngine.Expressions;

    /// <summary>
    /// Builds definitions.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public class DefinitionBuilder<TQuestion, TAnswer, TParameter, TExpressionResult> : IDefinitionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult>
        where TQuestion : IQuestion<TAnswer, TParameter>
    {
        private readonly IExpressionProviderFactory factory;

        private IExpressionProviderSet<TQuestion, TAnswer, TParameter, TExpressionResult> currentSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionBuilder{TQuestion, TAnswer, TParameter, TExpressionResult}"/> class.
        /// </summary>
        /// <param name="definition">The definition to build.</param>
        /// <param name="factory">The factory for creating expression providers.</param>
        public DefinitionBuilder(IDefinition<TQuestion, TAnswer, TParameter, TExpressionResult> definition, IExpressionProviderFactory factory)
        {
            this.Definition = definition;
            this.factory = factory;

            this.currentSet = new ExpressionProviderSet<TQuestion, TAnswer, TParameter, TExpressionResult>();
            this.Definition.AddExpressionProviderSet(this.currentSet);
        }

        /// <summary>
        /// Gets the definition that is built.
        /// </summary>
        /// <value>The definition.</value>
        public IDefinition<TQuestion, TAnswer, TParameter, TExpressionResult> Definition { get; private set; }

        /// <summary>
        /// Defines the strategy that is used to answer the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        /// <returns>Aggregator syntax.</returns>
        public IAggregatorSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> With(IStrategy<TAnswer, TParameter> strategy)
        {
            this.Definition.Strategy = strategy;

            return this;
        }

        /// <summary>
        /// Defines the aggregator that is used to aggregate expressions defined for the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="aggregator">The aggregator.</param>
        /// <returns>Expression syntax.</returns>
        public IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> AggregateWith(IAggregator<TExpressionResult, TAnswer, TParameter> aggregator)
        {
            this.Definition.Aggregator = aggregator;

            return this;
        }

        /// <summary>
        /// Whens the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>Expression syntax.</returns>
        public IExpressionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> When(Func<TQuestion, bool> condition)
        {
            this.currentSet = new ExpressionProviderSet<TQuestion, TAnswer, TParameter, TExpressionResult>
                { 
                    Condition = condition
                };

            this.Definition.AddExpressionProviderSet(this.currentSet);

            return this;
        }

        /// <summary>
        /// Defines an expression that has to be evaluated to answer the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Expression syntax.</returns>
        public IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> ByEvaluating(Func<TQuestion, IExpression<TExpressionResult, TParameter>> expressionProvider)
        {
            var provider = this.factory.CreateSingleExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult>(expressionProvider);

            this.currentSet.ExpressionProviders.Add(provider);

            return this;
        }

        /// <summary>
        /// Defines expressions that have to be evaluated to answer the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Constraint syntax.</returns>
        public IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> ByEvaluating(Func<TQuestion, IEnumerable<IExpression<TExpressionResult, TParameter>>> expressionProvider)
        {
            var provider = this.factory.CreateMultipleExpressionsProvider<TQuestion, TAnswer, TParameter, TExpressionResult>(expressionProvider);

            this.currentSet.ExpressionProviders.Add(provider);

            return this;
        }

        /// <summary>
        /// Defines expressions that have to be evaluated to answer the question <typeparamref name="TQuestion"/>.
        /// </summary>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Constraint syntax.</returns>
        public IConstraintSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> ByEvaluating(Expression<Func<TQuestion, TParameter, TExpressionResult>> expressionProvider)
        {
            var provider = new InlineExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult>(expressionProvider);

            this.currentSet.ExpressionProviders.Add(provider);

            return this;
        }
    }
}
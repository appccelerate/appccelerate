//-------------------------------------------------------------------------------
// <copyright file="DefaultFactory.cs" company="Appccelerate">
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

    using Appccelerate.EvaluationEngine.ExpressionProviders;
    using Appccelerate.EvaluationEngine.Expressions;
    using Appccelerate.EvaluationEngine.Syntax;

    /// <summary>
    /// Default implementation of the factory used to create internally used instances.
    /// </summary>
    public class DefaultFactory : IDefinitionSyntaxFactory, IExpressionProviderFactory, IDefinitionFactory
    {
        /// <summary>
        /// Creates a definition syntax.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <param name="definition">The definition.</param>
        /// <returns>Definition syntax</returns>
        public IDefinitionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> CreateDefinitionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult>(IDefinition<TQuestion, TAnswer, TParameter, TExpressionResult> definition) 
            where TQuestion : IQuestion<TAnswer, TParameter>
        {
            return new DefinitionBuilder<TQuestion, TAnswer, TParameter, TExpressionResult>(definition, this);
        }

        /// <summary>
        /// Creates a single expression provider.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Expression provider</returns>
        public IExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult> CreateSingleExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult>(Func<TQuestion, IExpression<TExpressionResult, TParameter>> expressionProvider) 
            where TQuestion : IQuestion<TAnswer, TParameter>
        {
            return new SingleExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult>(expressionProvider);
        }

        /// <summary>
        /// Creates a multiple expressions provider.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <param name="expressionProvider">The expression provider.</param>
        /// <returns>Expression provider</returns>
        public IExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult> CreateMultipleExpressionsProvider<TQuestion, TAnswer, TParameter, TExpressionResult>(Func<TQuestion, IEnumerable<IExpression<TExpressionResult, TParameter>>> expressionProvider) 
            where TQuestion : IQuestion<TAnswer, TParameter>
        {
            return new MultipleExpressionsProvider<TQuestion, TAnswer, TParameter, TExpressionResult>(expressionProvider);
        }

        /// <summary>
        /// Creates a definition.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <returns>A definition.</returns>
        public IDefinition<TQuestion, TAnswer, TParameter, TExpressionResult> CreateDefinition<TQuestion, TAnswer, TParameter, TExpressionResult>() where TQuestion : IQuestion<TAnswer, TParameter>
        {
            return new Definition<TQuestion, TAnswer, TParameter, TExpressionResult> { Strategy = new Strategies.AggregatorStrategy<TQuestion, TAnswer, TParameter, TExpressionResult>() };
        }
    }
}
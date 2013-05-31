//-------------------------------------------------------------------------------
// <copyright file="Engine.cs" company="Appccelerate">
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
    using System.Globalization;
    using System.Reflection;

    using Appccelerate.EvaluationEngine.Extensions;
    using Appccelerate.EvaluationEngine.Syntax;

    /// <summary>
    /// The engine is used to answer questions and defining solution strategies for them.
    /// </summary>
    public class Engine : IEngine, IDefinitionHostProvider
    {
        private readonly IDefinitionHost definitionHost;

        private readonly IDefinitionSyntaxFactory definitionSyntaxFactory;

        private readonly IDefinitionFactory definitionFactory;

        private ILogExtension log;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="definitionHost">The definition host.</param>
        /// <param name="definitionSyntaxFactory">The definition syntax factory.</param>
        /// <param name="definitionFactory">The definition factory.</param>
        public Engine(IDefinitionHost definitionHost, IDefinitionSyntaxFactory definitionSyntaxFactory, IDefinitionFactory definitionFactory)
        {
            this.definitionHost = definitionHost;
            this.definitionSyntaxFactory = definitionSyntaxFactory;
            this.definitionFactory = definitionFactory;
            this.log = new Extensions.EmptyLogExtension();
        }

        /// <summary>
        /// Gets the definition host.
        /// </summary>
        /// <value>The definition host.</value>
        public IDefinitionHost DefinitionHost
        {
            get
            {
                return this.definitionHost;
            }
        }

        /// <summary>
        /// Adds a solution definition for the specified question type.
        /// Use this overload in cases when the result returned by expressions is the same as the type of the answer.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question for which to add a solution definition.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <returns>Definition syntax</returns>
        public IDefinitionSyntax<TQuestion, TAnswer, TParameter, TAnswer> Solve<TQuestion, TAnswer, TParameter>() where TQuestion : IQuestion<TAnswer, TParameter>
        {
            return this.Solve<TQuestion, TAnswer, TParameter, TAnswer>();
        }

        /// <summary>
        /// Adds a solution definition for the specified question type.
        /// use this overload in cases when the result returned by expressions is different from the type of the answer of the question.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question for which to add a solution definition.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <returns>Definition syntax</returns>
        public IDefinitionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> Solve<TQuestion, TAnswer, TParameter, TExpressionResult>() where TQuestion : IQuestion<TAnswer, TParameter>
        {
            var existingDefinition = this.definitionHost.FindDefinition<TAnswer>(typeof(TQuestion));

            IDefinition<TQuestion, TAnswer, TParameter, TExpressionResult> definition;

            if (existingDefinition == null)
            {
                definition = this.definitionFactory.CreateDefinition<TQuestion, TAnswer, TParameter, TExpressionResult>();
                this.definitionHost.AddDefinition(definition);    
            }
            else
            {
                definition = (IDefinition<TQuestion, TAnswer, TParameter, TExpressionResult>)existingDefinition;
            }
            
            var builder = this.definitionSyntaxFactory.CreateDefinitionSyntax(definition);

            return builder;
        }

        /// <summary>
        /// Answers the specified question.
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <param name="question">The question.</param>
        /// <returns>The answer</returns>
        public TAnswer Answer<TAnswer>(IQuestion<TAnswer> question)
        {
            return this.Answer(question, Missing.Value);
        }

        /// <summary>
        /// Answers the specified question by searching for matching modules and executing the strategy provided by the question.
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="question">The question.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The answer</returns>
        public TAnswer Answer<TAnswer, TParameter>(IQuestion<TAnswer, TParameter> question, TParameter parameter)
        {
            var context = new Context { Question = question, Parameter = parameter };

            var definition = this.definitionHost.FindInHierarchyAndCloneDefinition(question);

            CheckValueNotNull(definition, string.Format(CultureInfo.InvariantCulture, "No definition found. Set definition with Solve method first: question = {0}", question));

            var strategy = definition.GetStrategy<TAnswer, TParameter>();

            context.Strategy = strategy;

            CheckValueNotNull(strategy, string.Format(CultureInfo.InvariantCulture, "No strategy found. Set strategy with Solve method first: question = {0}", question));

            var answer = strategy.Execute(question, parameter, definition, context);

            context.Answer = answer;

            this.log.FoundAnswer(context);

            return answer;
        }

        /// <summary>
        /// Sets the log extension.
        /// </summary>
        /// <param name="logExtension">The log extension.</param>
        public void SetLogExtension(ILogExtension logExtension)
        {
            this.log = logExtension;
        }

        private static void CheckValueNotNull(object value, string message)
        {
            if (value == null)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
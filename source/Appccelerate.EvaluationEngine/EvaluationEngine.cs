//-------------------------------------------------------------------------------
// <copyright file="EvaluationEngine.cs" company="Appccelerate">
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
    using System.Reflection;

    using Appccelerate.EvaluationEngine.Extensions;
    using Appccelerate.EvaluationEngine.Internals;
    using Appccelerate.EvaluationEngine.Syntax;

    /// <summary>
    /// The evaluation engine is the central component for answering questions.
    /// </summary>
    public class EvaluationEngine : IEvaluationEngine
    {
        private readonly Engine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationEngine"/> class.
        /// </summary>
        public EvaluationEngine()
        {
            var factory = new DefaultFactory();
            this.engine = new Engine(new DefinitionHost(), factory, factory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationEngine"/> class.
        /// </summary>
        /// <param name="parentEngine">The parent engine. Pass the parent engine to build up hierarchical engines.</param>
        public EvaluationEngine(IDefinitionHostProvider parentEngine)
        {
            Ensure.ArgumentNotNull(parentEngine, "parentEngine");

            var factory = new DefaultFactory();
            this.engine = new Engine(new DefinitionHost(parentEngine.DefinitionHost), factory, factory);
        }

        /// <summary>
        /// Gets the definition host.
        /// </summary>
        /// <value>The definition host.</value>
        public IDefinitionHost DefinitionHost
        {
            get
            {
                return this.engine.DefinitionHost;
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
            return this.engine.Solve<TQuestion, TAnswer, TParameter>();
        }

        /// <summary>
        /// Adds a solution definition for the specified question type.
        /// Use this overload in cases when your do not have a parameter passed along with the question and the result returned by expressions is the same as the type of the answer.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question for which to add a solution definition.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <returns>Definition syntax</returns>
        public IDefinitionSyntax<TQuestion, TAnswer, Missing, TAnswer> Solve<TQuestion, TAnswer>() where TQuestion : IQuestion<TAnswer>
        {
            return this.engine.Solve<TQuestion, TAnswer, Missing>();
        }

        /// <summary>
        /// Adds a solution definition for the specified question type.
        /// Use this overload in cases when the result returned by expressions is different from the type of the answer of the question.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question for which to add a solution definition.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <returns>Definition syntax</returns>
        public IDefinitionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> SolveWithResultMapping<TQuestion, TAnswer, TParameter, TExpressionResult>() 
            where TQuestion : IQuestion<TAnswer, TParameter>
        {
            return this.engine.Solve<TQuestion, TAnswer, TParameter, TExpressionResult>();
        }

        /// <summary>
        /// Adds a solution definition for the specified question type.
        /// Use this overload in cases when you do not have a parameter passed along with the question and the result returned by expressions is different from the type of the answer of the question.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question for which to add a solution definition.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <returns>Definition syntax</returns>
        public IDefinitionSyntax<TQuestion, TAnswer, Missing, TExpressionResult> SolveWithResultMapping<TQuestion, TAnswer, TExpressionResult>() 
            where TQuestion : IQuestion<TAnswer>
        {
            return this.engine.Solve<TQuestion, TAnswer, Missing, TExpressionResult>();
        }

        /// <summary>
        /// Answers the specified question.
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <param name="question">The question.</param>
        /// <returns>The answer</returns>
        public TAnswer Answer<TAnswer>(IQuestion<TAnswer> question)
        {
            return this.engine.Answer(question, Missing.Value);
        }

        /// <summary>
        /// Answers the specified question.
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="question">The question.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The answer</returns>
        public TAnswer Answer<TAnswer, TParameter>(IQuestion<TAnswer, TParameter> question, TParameter parameter)
        {
            return this.engine.Answer(question, parameter);
        }

        /// <summary>
        /// Loads the specified evaluation engine module.
        /// </summary>
        /// <param name="evaluationEngineModule">The evaluation engine module.</param>
        public void Load(IEvaluationEngineModule evaluationEngineModule)
        {
            Ensure.ArgumentNotNull(evaluationEngineModule, "evaluationEngineModule");

            evaluationEngineModule.Load(this);
        }

        /// <summary>
        /// Sets the log extension.
        /// </summary>
        /// <param name="logExtension">The log extension.</param>
        public void SetLogExtension(ILogExtension logExtension)
        {
            this.engine.SetLogExtension(logExtension);
        }
    }
}
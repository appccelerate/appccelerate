//-------------------------------------------------------------------------------
// <copyright file="EvaluationEngineModule.cs" company="Appccelerate">
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

    using Appccelerate.EvaluationEngine.Syntax;

    /// <summary>
    /// Base class for <see cref="IEvaluationEngineModule"/>s.
    /// </summary>
    public abstract class EvaluationEngineModule : IEvaluationEngineModule
    {
        private ISolutionDefinitionHost SolutionDefinitionHost { get; set; }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <param name="solutionDefinitionHost">The solution definition host on which the solution definition is to be added.</param>
        public void Load(ISolutionDefinitionHost solutionDefinitionHost)
        {
            this.SolutionDefinitionHost = solutionDefinitionHost;

            this.Load();
        }

        /// <summary>
        /// Loads this instance.
        /// Derived classes add their solution definition.
        /// </summary>
        protected abstract void Load();

        /// <summary>
        /// Adds a solution definition for the specified question type.
        /// Use this overload in cases when the result returned by expressions is the same as the type of the answer.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question for which to add a solution definition.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <returns>Definition syntax</returns>
        protected IDefinitionSyntax<TQuestion, TAnswer, Missing, TAnswer> Solve<TQuestion, TAnswer>() where TQuestion : IQuestion<TAnswer>
        {
            return this.SolutionDefinitionHost.Solve<TQuestion, TAnswer>();
        }

        /// <summary>
        /// Adds a solution definition for the specified question type.
        /// Use this overload in cases when the result returned by expressions is the same as the type of the answer.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question for which to add a solution definition.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <returns>Definition syntax</returns>
        protected IDefinitionSyntax<TQuestion, TAnswer, TParameter, TAnswer> Solve<TQuestion, TAnswer, TParameter>() where TQuestion : IQuestion<TAnswer, TParameter>
        {
            return this.SolutionDefinitionHost.Solve<TQuestion, TAnswer, TParameter>();
        }

        /// <summary>
        /// Adds a solution definition for the specified question type.
        /// Use this overload in cases when the result returned by expressions is different from the type of the answer of the question.
        /// </summary>
        /// <typeparam name="TQuestion">The type of the question for which to add a solution definition.</typeparam>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
        /// <returns>Definition syntax</returns>
        protected IDefinitionSyntax<TQuestion, TAnswer, Missing, TExpressionResult> SolveWithResultMapping<TQuestion, TAnswer, TExpressionResult>() where TQuestion : IQuestion<TAnswer>
        {
            return this.SolutionDefinitionHost.SolveWithResultMapping<TQuestion, TAnswer, TExpressionResult>();
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
        protected IDefinitionSyntax<TQuestion, TAnswer, TParameter, TExpressionResult> SolveWithResultMapping<TQuestion, TAnswer, TParameter, TExpressionResult>() where TQuestion : IQuestion<TAnswer, TParameter>
        {
            return this.SolutionDefinitionHost.SolveWithResultMapping<TQuestion, TAnswer, TParameter, TExpressionResult>();
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="InlineExpression.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Expressions
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Encapsulates an inline expression (a lambda expression) as an <see cref="IExpression{TExpressionResult, TParameter}"/>.
    /// </summary>
    /// <typeparam name="TQuestion">The type of the question.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public class InlineExpression<TQuestion, TParameter, TExpressionResult> : IExpression<TExpressionResult, TParameter>
    {
        private readonly Expression<Func<TQuestion, TParameter, TExpressionResult>> inlineExpression;
        private readonly Func<TQuestion, TParameter, TExpressionResult> compiledInlineExpression;

        private readonly TQuestion question;

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineExpression{TQuestion, TParameter, TExpressionResult}"/> class.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="inlineExpression">The inline expression.</param>
        public InlineExpression(TQuestion question, Expression<Func<TQuestion, TParameter, TExpressionResult>> inlineExpression)
        {
            Ensure.ArgumentNotNull(inlineExpression, "inlineExpression");

            this.question = question;
            this.inlineExpression = inlineExpression;
            this.compiledInlineExpression = inlineExpression.Compile();
        }

        /// <summary>
        /// Evaluates the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Expression result</returns>
        public TExpressionResult Evaluate(TParameter parameter)
        {
            return this.compiledInlineExpression.Invoke(this.question, parameter);
        }

        /// <summary>
        /// Describes this instance.
        /// </summary>
        /// <returns>The description of this instance.</returns>
        public string Describe()
        {
            return "inline expression = " + this.inlineExpression;
        }
    }
}
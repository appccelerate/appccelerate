//-------------------------------------------------------------------------------
// <copyright file="IExpression.cs" company="Appccelerate">
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
    using System.Reflection;

    /// <summary>
    /// An expression provides a result.
    /// </summary>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    public interface IExpression<TExpressionResult> : IExpression<TExpressionResult, Missing>
    {    
    }

    /// <summary>
    /// An expression provides a result.
    /// </summary>
    /// <typeparam name="TExpressionResult">The type of the expression result.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public interface IExpression<TExpressionResult, TParameter> : IDescriptionProvider
    {
        /// <summary>
        /// Evaluates this instance and provides an result.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The result of the evaluation.</returns>
        TExpressionResult Evaluate(TParameter parameter);
    }
}
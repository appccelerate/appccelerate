//-------------------------------------------------------------------------------
// <copyright file="IStrategy.cs" company="Appccelerate">
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

    /// <summary>
    /// The strategy defines how a question can be answered.
    /// </summary>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    public interface IStrategy<TAnswer> : IStrategy<TAnswer, Missing>
    {
    }

    /// <summary>
    /// The strategy defines how a question can be answered.
    /// </summary>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public interface IStrategy<TAnswer, TParameter> : IDescriptionProvider
    {
        /// <summary>
        /// Executes the strategy and answers the specified question.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="context">The context.</param>
        /// <returns>The answer.</returns>
        TAnswer Execute(IQuestion<TAnswer, TParameter> question, TParameter parameter, IDefinition definition, Context context);
    }
}
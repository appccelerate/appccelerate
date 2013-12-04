//-------------------------------------------------------------------------------
// <copyright file="IDefinition.cs" company="Appccelerate">
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
    using System;

    /// <summary>
    /// A definition defines the strategy used to answer a question.
    /// </summary>
    public interface IDefinition
    {
        /// <summary>
        /// Gets the type of the question.
        /// </summary>
        /// <value>The type of the question.</value>
        Type QuestionType { get; }

        /// <summary>
        /// Gets the strategy used to answer the question of type <see cref="QuestionType"/>.
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <returns>The strategy.</returns>
        IStrategy<TAnswer, TParameter> GetStrategy<TAnswer, TParameter>();

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        IDefinition Clone();

        /// <summary>
        /// Merges the specified definition into this instance.
        /// If specified, the strategy/aggregator of this instance is overridden.
        /// Expression provider sets are added.
        /// </summary>
        /// <param name="definition">The definition.</param>
        void Merge(IDefinition definition);
    }
}
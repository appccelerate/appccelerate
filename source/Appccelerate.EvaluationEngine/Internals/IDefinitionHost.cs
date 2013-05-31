//-------------------------------------------------------------------------------
// <copyright file="IDefinitionHost.cs" company="Appccelerate">
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

    /// <summary>
    /// Host for definitions.
    /// </summary>
    public interface IDefinitionHost
    {
        /// <summary>
        /// Adds a definition.
        /// </summary>
        /// <param name="definition">The definition.</param>
        void AddDefinition(IDefinition definition);

        /// <summary>
        /// Finds the definition for the specified question type.
        /// Only definitions of this instance are considered.
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <param name="questionType">Type of the question.</param>
        /// <returns>The matching definition or null.</returns>
        IDefinition FindDefinition<TAnswer>(Type questionType);

        /// <summary>
        /// Finds the definition for the specified question. All matching definitions in the upward-hierarchy are considered, too.
        /// Returns a merged and cloned definition.
        /// Cannot be used to modify definitions because a clone is returned.
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="question">The question.</param>
        /// <returns>A cloned definition containing the merged data of the whole upward-hierarchy of definition hosts.</returns>
        IDefinition FindInHierarchyAndCloneDefinition<TAnswer, TParameter>(IQuestion<TAnswer, TParameter> question);
    }
}
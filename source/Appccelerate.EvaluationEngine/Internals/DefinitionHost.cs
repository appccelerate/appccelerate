//-------------------------------------------------------------------------------
// <copyright file="DefinitionHost.cs" company="Appccelerate">
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
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Host for definitions.
    /// </summary>
    public class DefinitionHost : IDefinitionHost
    {
        private readonly List<IDefinition> definitions = new List<IDefinition>();

        private readonly IDefinitionHost parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionHost"/> class.
        /// </summary>
        public DefinitionHost()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionHost"/> class.
        /// </summary>
        /// <param name="parent">The parent definition host. Used for hierarchical evaluation engines.</param>
        public DefinitionHost(IDefinitionHost parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Adds a definition.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public void AddDefinition(IDefinition definition)
        {
            this.CheckThatNoDefinitionWithSameQuestionTypeAlreadyExists(definition);

            this.definitions.Add(definition);
        }

        /// <summary>
        /// Finds the definition for the specified question. All matching definitions in the upward-hierarchy are considered, too.
        /// Returns a merged and cloned definition.
        /// Cannot be used to modify definitions because a clone is returned.
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="question">The question.</param>
        /// <returns>
        /// A cloned definition containing the merged data of the whole upward-hierarchy of definition hosts.
        /// </returns>
        public IDefinition FindInHierarchyAndCloneDefinition<TAnswer, TParameter>(IQuestion<TAnswer, TParameter> question)
        {
            var def = this.definitions.SingleOrDefault(d => d.QuestionType.GetTypeInfo().IsAssignableFrom(question.GetType().GetTypeInfo()));

            if (this.parent == null)
            {
                return def != null ? def.Clone() : null;
            }
            
            var parentDefinition = this.parent.FindDefinition<TAnswer>(question.GetType());

            if (parentDefinition != null)
            {
                parentDefinition = parentDefinition.Clone();

                if (def != null)
                {
                    parentDefinition.Merge(def);

                    return parentDefinition;
                }
                
                return parentDefinition.Clone();
            }

            return def != null ? def.Clone() : null;
        }

        /// <summary>
        /// Finds the definition for the specified question type.
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer.</typeparam>
        /// <param name="questionType">Type of the question.</param>
        /// <returns>The matching definition or null.</returns>
        public IDefinition FindDefinition<TAnswer>(Type questionType)
        {
            return this.definitions.SingleOrDefault(d => d.QuestionType.GetTypeInfo().IsAssignableFrom(questionType.GetTypeInfo()));
        }

        private void CheckThatNoDefinitionWithSameQuestionTypeAlreadyExists(IDefinition definition)
        {
            if (this.definitions.Any(d => d.QuestionType.GetTypeInfo().IsAssignableFrom(definition.QuestionType.GetTypeInfo())))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "A definition for question {0} was already added.", definition.QuestionType));
            }
        }
    }
}
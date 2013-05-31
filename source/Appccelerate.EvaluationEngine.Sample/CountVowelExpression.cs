//-------------------------------------------------------------------------------
// <copyright file="CountVowelExpression.cs" company="Appccelerate">
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
    using System.Linq;

    using Appccelerate.EvaluationEngine.Expressions;

    /// <summary>
    /// Expression for counting vowels in a string.
    /// </summary>
    public class CountVowelExpression : IExpression<int, string>
    {
        /// <summary>
        /// Gets or sets the vowel to count.
        /// </summary>
        /// <value>The vowel to count.</value>
        public char Vowel { get; set; }

        /// <summary>
        /// Evaluates the number of <see cref="Vowel"/> in the passed parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The number of occurrences.</returns>
        public int Evaluate(string parameter)
        {
            return parameter.Count(c => c == this.Vowel);
        }

        /// <summary>
        /// Describes this instance.
        /// </summary>
        /// <returns>The description of this instance.</returns>
        public string Describe()
        {
            return "counts the number of " + this.Vowel;
        }
    }
}
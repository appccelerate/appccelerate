//-------------------------------------------------------------------------------
// <copyright file="HowManyVowelsAreInThisText.cs" company="Appccelerate">
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
    /// <summary>
    /// Question for counting vowels in a text.
    /// </summary>
    public class HowManyVowelsAreInThisText : IQuestion<int, string>
    {
        /// <summary>
        /// Capital or small letters.
        /// </summary>
        public enum Letters
        {
            /// <summary>
            /// Capital letters.
            /// </summary>
            Capital,

            /// <summary>
            /// Small letters.
            /// </summary>
            Small
        }

        /// <summary>
        /// Gets or sets the whether capitol or small letters should be counted.
        /// </summary>
        /// <value>The letters to count.</value>
        public Letters CountLetters { get; set; }

        /// <summary>
        /// Describes this instance.
        /// </summary>
        /// <returns>The description of this instance.</returns>
        public string Describe()
        {
            return "how many " + this.CountLetters + " vowels are in this text?";
        }
    }
}
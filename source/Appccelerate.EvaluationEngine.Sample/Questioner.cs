//-------------------------------------------------------------------------------
// <copyright file="Questioner.cs" company="Appccelerate">
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
    using System.Globalization;

    /// <summary>
    /// Asks questions without knowing how they are answered.
    /// </summary>
    public class Questioner
    {
        private readonly IEvaluationEngine evaluationEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Questioner"/> class.
        /// </summary>
        /// <param name="evaluationEngine">The evaluation engine.</param>
        public Questioner(IEvaluationEngine evaluationEngine)
        {
            this.evaluationEngine = evaluationEngine;
        }

        /// <summary>
        /// Asks questions on the evaluation engine without knowledge how they are solved.
        /// </summary>
        public void Ask()
        {
            const string SampleText = "sample Text";

            Console.WriteLine();
            Console.WriteLine("log messages from answering how cool it is:");

            string coolness = this.evaluationEngine.Answer(new HowCoolIsTheEvaluationEngine());

            Console.WriteLine();
            Console.WriteLine("log messages from answering how many vowels there are:");

            int vowels = this.evaluationEngine.Answer(
                new HowManyVowelsAreInThisText { CountLetters = HowManyVowelsAreInThisText.Letters.Small }, 
                SampleText);

            Console.WriteLine();
            Console.WriteLine();
            string message = string.Format(
                CultureInfo.InvariantCulture,
                "knowing that {0} has {1} vowels is {2} cool!",
                SampleText,
                vowels,
                coolness);
            Console.WriteLine(message);
        }
    }
}
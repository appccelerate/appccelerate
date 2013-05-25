//-------------------------------------------------------------------------------
// <copyright file="Answerer.cs" company="Appccelerate">
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
    /// Knows how questions can be answered and defines the solution strategy on the evaluation engine.
    /// </summary>
    public class Answerer
    {
        private readonly IEvaluationEngine evaluationEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Answerer"/> class.
        /// </summary>
        /// <param name="evaluationEngine">The evaluation engine.</param>
        public Answerer(IEvaluationEngine evaluationEngine)
        {
            this.evaluationEngine = evaluationEngine;
        }

        /// <summary>
        /// Prepares the answers by defining the solution strategy on the evaluation engine.
        /// </summary>
        public void PrepareAnswers()
        {
            this.evaluationEngine.Solve<HowCoolIsTheEvaluationEngine, string>()
                .AggregateWithExpressionAggregator(string.Empty, (aggregate, value) => aggregate + " " + value)
                .ByEvaluating((q, p) => "extremely")
                .ByEvaluating((q, p) => "super")
                .ByEvaluating((q, p) => "fantastic");

            this.evaluationEngine.Solve<HowManyVowelsAreInThisText, int, string>()
                .AggregateWithExpressionAggregator(0, (aggregate, value) => aggregate + value)
                .When(q => q.CountLetters == HowManyVowelsAreInThisText.Letters.Small)
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'a' })
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'e' })
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'i' })
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'o' })
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'u' })
                .When(q => q.CountLetters == HowManyVowelsAreInThisText.Letters.Capital)
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'A' })
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'E' })
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'I' })
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'O' })
                    .ByEvaluating(q => new CountVowelExpression { Vowel = 'U' });
        }
    }
}
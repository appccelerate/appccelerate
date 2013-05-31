//-------------------------------------------------------------------------------
// <copyright file="Question{TAnswer,TParameter}.cs" company="Appccelerate">
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
    /// Base class for questions. Only use this class (instead of implementing <see cref="IQuestion{TAnswer, TParameter}"/>) if you do not want to provide a description of your own in <see cref="Describe"/>.
    /// </summary>
    /// <typeparam name="TAnswer">The type of the answer.</typeparam>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public class Question<TAnswer, TParameter> : IQuestion<TAnswer, TParameter>
    {
        /// <summary>
        /// Describes this instance.
        /// </summary>
        /// <returns>Description of this instance.</returns>
        public string Describe()
        {
            return this.ToString();
        }
    }
}
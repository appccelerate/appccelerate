//-------------------------------------------------------------------------------
// <copyright file="FruitCountExpression.cs" company="Appccelerate">
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

    using Appccelerate.EvaluationEngine.Expressions;

    public class FruitCountExpression : IExpression<int>
    {
        public string Kind { get; set; }

        public int NumberOfFruits { get; set; }

        public int Evaluate(Missing parameter)
        {
            return this.NumberOfFruits;
        }

        public string Describe()
        {
            return "number of " + this.Kind;
        }
    }
}
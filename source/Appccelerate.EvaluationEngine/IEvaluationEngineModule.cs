//-------------------------------------------------------------------------------
// <copyright file="IEvaluationEngineModule.cs" company="Appccelerate">
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
    /// An evaluation engine module is used to encapsulate parts of the solution definition in an own class.
    /// Use these modules for example for plug-ins or simply to structure the solution definition.
    /// </summary>
    public interface IEvaluationEngineModule
    {
        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <param name="solutionDefinitionHost">The solution definition host on which the solution definition is to be added.</param>
        void Load(ISolutionDefinitionHost solutionDefinitionHost);        
    }
}
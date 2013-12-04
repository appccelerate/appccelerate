//-------------------------------------------------------------------------------
// <copyright file="IValidationResult.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Validation
{
    /// <summary>
    /// The result of a validation.
    /// Use this interface if the standard <see cref="IValidationViolation"/> is sufficient; otherwise use <see cref="IValidationResult{TValidationViolation}"/>.
    /// </summary>
    public interface IValidationResult : IValidationResult<IValidationViolation>
    {
    }
}
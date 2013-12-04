//-------------------------------------------------------------------------------
// <copyright file="IValidationResult{TValidationViolation}.cs" company="Appccelerate">
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
    using System.Collections.Generic;

    /// <summary>
    /// The result of a validation.
    /// Use this interface to extend the type of the violations. If you do not need to extend the violations then use <see cref="IValidationResult"/>.
    /// </summary>
    /// <typeparam name="TValidationViolation">The type of the validation violation.</typeparam>
    public interface IValidationResult<TValidationViolation> where TValidationViolation : IValidationViolation
    {
        /// <summary>
        /// Gets or sets a value indicating whether the validation ended valid.
        /// </summary>
        /// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
        bool Valid { get; set; }

        /// <summary>
        /// Gets the violations of the validation.
        /// </summary>
        /// <value>The violations.</value>
        IEnumerable<TValidationViolation> Violations { get; }

        /// <summary>
        /// Adds a violation.
        /// </summary>
        /// <param name="violation">The violation.</param>
        void AddViolation(TValidationViolation violation);
    }
}
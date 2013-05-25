//-------------------------------------------------------------------------------
// <copyright file="ValidationResult{TValidationViolation}.cs" company="Appccelerate">
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
    /// Extensible validation result.
    /// </summary>
    /// <typeparam name="TValidationViolation">The type of the validation violation.</typeparam>
    public class ValidationResult<TValidationViolation> : IValidationResult<TValidationViolation>
            where TValidationViolation : IValidationViolation
    {
        private readonly List<TValidationViolation> violations;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult{TValidationViolation}"/> class.
        /// <see cref="Valid"/> is initially set to <c>true</c>.
        /// </summary>
        public ValidationResult()
        {
            this.Valid = true;
            this.violations = new List<TValidationViolation>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ValidationResult{TValidationViolation}"/> is valid.
        /// </summary>
        /// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets the violations of the validation.
        /// </summary>
        /// <value>The violations.</value>
        public IEnumerable<TValidationViolation> Violations
        {
            get
            {
                return this.violations;
            }
        }

        /// <summary>
        /// Adds a violation.
        /// </summary>
        /// <param name="violation">The violation.</param>
        public void AddViolation(TValidationViolation violation)
        {
            this.violations.Add(violation);
        }
    }
}
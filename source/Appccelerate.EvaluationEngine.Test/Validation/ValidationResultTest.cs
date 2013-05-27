//-------------------------------------------------------------------------------
// <copyright file="ValidationResultTest.cs" company="Appccelerate">
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
    using System.Linq;

    using Moq;

    using Xunit;

    public class ValidationResultTest
    {
        private readonly ValidationResult testee;

        public ValidationResultTest()
        {
            this.testee = new ValidationResult();
        }

        [Fact]
        public void ValidIsInitiallyTrue()
        {
            bool valid = this.testee.Valid;

            Assert.True(valid);
        }

        [Fact]
        public void ValidCanBeSet()
        {
            this.testee.Valid = false;

            bool valid = this.testee.Valid;

            Assert.False(valid);
        }

        [Fact]
        public void ReturnsNoViolations_WhenInitialized()
        {
            int count = this.testee.Violations.Count();

            Assert.Equal(0, count);
        }

        [Fact]
        public void ReturnsAddedViolations()
        {
            var violationMock = new Mock<IValidationViolation>();

            this.testee.AddViolation(violationMock.Object);

            var violations = this.testee.Violations.ToList();

            Assert.Equal(1, violations.Count());
            Assert.Same(violationMock.Object, violations.ElementAt(0));
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="ExecutionContextTest.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Reporting
{
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ExecutionContextTest
    {
        private readonly Mock<IDescribable> describable;

        public ExecutionContextTest()
        {
            this.describable = new Mock<IDescribable>();
        }

        [Fact]
        public void Constructor_ShouldDescribe()
        {
            const string ExpectedName = "Name";
            const string ExpectedDescription = "TestDescription";

            this.describable.Setup(d => d.Name).Returns(ExpectedName);
            this.describable.Setup(d => d.Describe()).Returns(ExpectedDescription);

            ExecutionContext testee = CreateTestee(this.describable.Object);

            testee.Name.Should().Be(ExpectedName);
            testee.Description.Should().Be(ExpectedDescription);
            this.describable.Verify(d => d.Describe());
        }

        [Fact]
        public void Constructor_Executables_ShouldBeEmpty()
        {
            ExecutionContext testee = CreateTestee(Mock.Of<IDescribable>());

            testee.Executables.Should().BeEmpty();
        }

        [Fact]
        public void CreateExecutableContext_ShouldCreateExecutableContext()
        {
            ExecutionContext testee = CreateTestee(Mock.Of<IDescribable>());

            var executableContext = testee.CreateExecutableContext(Mock.Of<IDescribable>());

            testee.Executables.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.Contain(executableContext);
        }

        private static ExecutionContext CreateTestee(IDescribable describable)
        {
            return new ExecutionContext(describable);
        }
    }
}
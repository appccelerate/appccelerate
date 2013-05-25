//-------------------------------------------------------------------------------
// <copyright file="ReportingContextTest.cs" company="Appccelerate">
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

    public class ReportingContextTest
    {
        private readonly ReportingContext testee;

        public ReportingContextTest()
        {
            this.testee = new ReportingContext();
        }

        [Fact]
        public void Constructor_Run_ShouldBeNull()
        {
            this.testee.Run.Should().BeNull();
        }

        [Fact]
        public void Constructor_Extensions_ShouldBeEmpty()
        {
            this.testee.Extensions.Should().BeEmpty();
        }

        [Fact]
        public void CreateRunExecutionContext_ShouldCreateExecutionContext()
        {
            var runExecutionContext = this.testee.CreateRunExecutionContext(Mock.Of<IDescribable>());

            this.testee.Run.Should().NotBeNull()
                .And.Be(runExecutionContext);
        }

        [Fact]
        public void CreateShutdownExecutionContext_ShouldCreateExecutionContext()
        {
            var shutdownExecutionContext = this.testee.CreateShutdownExecutionContext(Mock.Of<IDescribable>());

            this.testee.Shutdown.Should().NotBeNull()
                .And.Be(shutdownExecutionContext);
        }

        [Fact]
        public void CreateExtensionContext_ShouldCreateExtensionContext()
        {
            var extensionContext = this.testee.CreateExtensionContext(Mock.Of<IDescribable>());

            this.testee.Extensions.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.Contain(extensionContext);
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="AbstractStrategyTest.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper
{
    using System;

    using Appccelerate.Bootstrapper.Execution;
    using Appccelerate.Bootstrapper.Extension;
    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Bootstrapper.Syntax;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class AbstractStrategyTest
    {
        private readonly Mock<ISyntaxBuilder<IExtension>> runSyntaxBuilder;

        private readonly Mock<ISyntaxBuilder<IExtension>> shutdownSyntaxBuilder;

        private readonly TestableAbstractStrategy testee;

        public AbstractStrategyTest()
        {
            this.runSyntaxBuilder = new Mock<ISyntaxBuilder<IExtension>>();
            this.shutdownSyntaxBuilder = new Mock<ISyntaxBuilder<IExtension>>();

            this.testee = new TestableAbstractStrategy(this.runSyntaxBuilder.Object, this.shutdownSyntaxBuilder.Object);
        }

        [Fact]
        public void BuildRunSyntax_WhenCalledMultipleTimes_ShouldThrowException()
        {
            this.testee.BuildRunSyntax();

            this.testee.Invoking(x => x.BuildRunSyntax()).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BuildRunSyntax_ShouldReturnDefinedRunSyntax()
        {
            var syntax = this.testee.BuildRunSyntax();

            syntax.Equals(this.runSyntaxBuilder.Object).Should().BeTrue();
            this.testee.RunSyntaxBuilder.Equals(this.runSyntaxBuilder.Object).Should().BeTrue();
        }

        [Fact]
        public void BuildShutdownSyntax_WhenCalledMultipleTimes_ShouldThrowException()
        {
            this.testee.BuildShutdownSyntax();

            this.testee.Invoking(x => x.BuildShutdownSyntax()).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BuildShutdownSyntax_ShouldReturnDefinedRunSyntax()
        {
            var syntax = this.testee.BuildShutdownSyntax();

            syntax.Equals(this.shutdownSyntaxBuilder.Object).Should().BeTrue();
            this.testee.ShutdownSyntaxBuilder.Equals(this.shutdownSyntaxBuilder.Object).Should().BeTrue();
        }

        [Fact]
        public void CreateRunExecutor_ShouldCreateSynchronousExecutor()
        {
            var runExecutor = this.testee.CreateRunExecutor();

            runExecutor.Should().BeOfType<SynchronousExecutor<IExtension>>();
        }

        [Fact]
        public void CreateShutdownExecutor_ShouldCreateSynchronousReverseExecutor()
        {
            var runExecutor = this.testee.CreateShutdownExecutor();

            runExecutor.Should().BeOfType<SynchronousReverseExecutor<IExtension>>();
        }

        [Fact]
        public void CreateExtensionResolver_ShouldCreateNullExtensionResolver()
        {
            var extensionResolver = this.testee.CreateExtensionResolver();

            extensionResolver.Should().BeOfType<NullExtensionResolver<IExtension>>();
        }

        [Fact]
        public void CreateReportingContext_ShouldCreateReportingContext()
        {
            var reportingContext = this.testee.CreateReportingContext();

            reportingContext.Should().BeOfType<ReportingContext>();
        }

        [Fact]
        public void Dispose_MultipleTimes_ShouldNotThrow()
        {
            this.testee.Dispose();

            this.testee.Invoking(t => t.Dispose()).ShouldNotThrow();
        }

        private class TestableAbstractStrategy : AbstractStrategy<IExtension>
        {
            public TestableAbstractStrategy(ISyntaxBuilder<IExtension> runSyntaxBuilder, ISyntaxBuilder<IExtension> shutdownSyntaxBuilder)
                : base(runSyntaxBuilder, shutdownSyntaxBuilder)
            {
            }

            public ISyntaxBuilder<IExtension> RunSyntaxBuilder { get; private set; }

            public ISyntaxBuilder<IExtension> ShutdownSyntaxBuilder { get; private set; }

            protected override void DefineRunSyntax(ISyntaxBuilder<IExtension> builder)
            {
                this.RunSyntaxBuilder = builder;
            }

            protected override void DefineShutdownSyntax(ISyntaxBuilder<IExtension> builder)
            {
                this.ShutdownSyntaxBuilder = builder;
            }
        }
    }
}
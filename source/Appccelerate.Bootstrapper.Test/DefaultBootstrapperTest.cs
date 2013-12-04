//-------------------------------------------------------------------------------
// <copyright file="DefaultBootstrapperTest.cs" company="Appccelerate">
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
    using System.Collections.Generic;

    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Bootstrapper.Syntax;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class DefaultBootstrapperTest
    {
        private readonly Mock<IExtensionHost<IExtension>> extensionHost;

        private readonly Mock<IReporter> reporter;

        private readonly Mock<IExecutor<IExtension>> runExecutor;

        private readonly Mock<IExecutor<IExtension>> shutdownExecutor;

        private readonly Mock<IReportingContext> reportingContext;

        private readonly Mock<IStrategy<IExtension>> strategy;

        private readonly Mock<IExtensionResolver<IExtension>> extensionResolver;

        private readonly DefaultBootstrapper<IExtension> testee;

        public DefaultBootstrapperTest()
        {
            this.extensionHost = new Mock<IExtensionHost<IExtension>>();
            this.reporter = new Mock<IReporter>();
            this.strategy = new Mock<IStrategy<IExtension>>();
            this.runExecutor = new Mock<IExecutor<IExtension>>();
            this.shutdownExecutor = new Mock<IExecutor<IExtension>>();
            this.reportingContext = new Mock<IReportingContext> { DefaultValue = DefaultValue.Mock };
            this.extensionResolver = new Mock<IExtensionResolver<IExtension>>();

            this.testee = new DefaultBootstrapper<IExtension>(this.extensionHost.Object, this.reporter.Object);
        }

        [Fact]
        public void Initialize_MultipleTimes_ShouldThrowException()
        {
            this.SetupStrategyReturnsBuilderAndContextAndResolver();

            this.testee.Initialize(this.strategy.Object);

            this.testee.Invoking(x => x.Initialize(Mock.Of<IStrategy<IExtension>>())).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Initialize_ShouldCreateReportingContext()
        {
            this.SetupStrategyReturnsBuilderAndContextAndResolver();

            this.testee.Initialize(this.strategy.Object);

            this.strategy.Verify(s => s.CreateReportingContext());
        }

        [Fact]
        public void AddExtension_WhenNotInitialized_ShouldThrowInvalidOperationException()
        {
            this.testee.Invoking(x => x.AddExtension(Mock.Of<IExtension>())).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void AddExtension_ShouldTrackExtension()
        {
            this.InitializeTestee();

            var extension = Mock.Of<IExtension>();

            this.testee.AddExtension(extension);

            this.extensionHost.Verify(x => x.AddExtension(extension));
        }

        [Fact]
        public void AddExtension_ShouldCreateExtensionContext()
        {
            this.InitializeTestee();

            var extension = Mock.Of<IExtension>();

            this.testee.AddExtension(extension);

            this.reportingContext.Verify(c => c.CreateExtensionContext(extension));
        }

        [Fact]
        public void Run_ShouldBuildRunSyntax()
        {
            this.InitializeTestee();

            this.testee.Run();

            this.strategy.Verify(s => s.BuildRunSyntax());
        }

        [Fact]
        public void Run_ShouldCreateExtensionResolver()
        {
            this.InitializeTestee();

            this.testee.Run();

            this.strategy.Verify(s => s.CreateExtensionResolver());
        }

        [Fact]
        public void Run_ShouldPassItselfToExtensionResolver()
        {
            this.InitializeTestee();

            this.testee.Run();

            this.extensionResolver.Verify(er => er.Resolve(this.testee));
        }

        [Fact]
        public void Run_ShouldExecuteSyntaxAndExtensionsOnRunExecutor()
        {
            var runSyntax = new Mock<ISyntax<IExtension>>();
            this.strategy.Setup(s => s.BuildRunSyntax()).Returns(runSyntax.Object);

            var extensions = new List<IExtension> { Mock.Of<IExtension>(), };
            this.extensionHost.Setup(e => e.Extensions).Returns(extensions);

            this.InitializeTestee();

            this.testee.Run();

            this.runExecutor.Verify(r => r.Execute(runSyntax.Object, extensions, It.IsAny<IExecutionContext>()));
        }

        [Fact]
        public void Run_ShouldThrowExceptionWhenNotInitialized()
        {
            this.testee.Invoking(t => t.Run())
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Run_ShouldCreateRunExecutionContextWithRunExecutor()
        {
            this.InitializeTestee();

            this.testee.Run();

            this.reportingContext.Verify(c => c.CreateRunExecutionContext(this.runExecutor.Object));
        }

        [Fact]
        public void Run_ShouldProvideRunExecutionContextForRunExecutor()
        {
            var runExecutionContext = Mock.Of<IExecutionContext>();

            this.reportingContext.Setup(c => c.CreateRunExecutionContext(It.IsAny<IDescribable>()))
                .Returns(runExecutionContext);

            this.InitializeTestee();

            this.testee.Run();

            this.runExecutor.Verify(r => r.Execute(It.IsAny<ISyntax<IExtension>>(), It.IsAny<IEnumerable<IExtension>>(), runExecutionContext));
        }

        [Fact]
        public void Shutdown_ShouldBuildShutdownSyntax()
        {
            this.ShouldBuildShutdownSyntax(() => this.testee.Shutdown());
        }

        [Fact]
        public void Shutdown_ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor()
        {
            this.ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor(() => this.testee.Shutdown());
        }

        [Fact]
        public void Shutdown_ShouldThrowExceptionWhenNotInitialized()
        {
            this.testee.Invoking(t => t.Shutdown())
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Shutdown_ShouldCreateShutdownExecutionContextWithShutdownExecutor()
        {
            this.ShouldCreateShutdownExecutionContextWithShutdownExecutor(() => this.testee.Shutdown());
        }

        [Fact]
        public void Shutdown_ShouldProvideShutdownExecutionContextForShutdownExecutor()
        {
            this.ShouldProvideShutdownExecutionContextForShutdownExecutor(() => this.testee.Shutdown());
        }

        [Fact]
        public void Shutdown_ShouldReport()
        {
            this.ShouldReportBeforeStrategyDisposal(() => this.testee.Shutdown());
        }

        [Fact]
        public void Dispose_ShouldBuildShutdownSyntax()
        {
            this.ShouldBuildShutdownSyntax(() => this.testee.Dispose());
        }

        [Fact]
        public void Dispose_ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor()
        {
            this.ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor(() => this.testee.Dispose());
        }

        [Fact]
        public void Dispose_ShouldCreateShutdownExecutionContextWithShutdownExecutor()
        {
            this.ShouldCreateShutdownExecutionContextWithShutdownExecutor(() => this.testee.Dispose());
        }

        [Fact]
        public void Dispose_ShouldProvideShutdownExecutionContextForShutdownExecutor()
        {
            this.ShouldProvideShutdownExecutionContextForShutdownExecutor(() => this.testee.Dispose());
        }

        [Fact]
        public void Dispose_ShouldReport()
        {
            this.ShouldReportBeforeStrategyDisposal(() => this.testee.Dispose());
        }

        [Fact]
        public void Dispose_ShouldDisposeStrategy()
        {
            this.InitializeTestee();

            this.testee.Dispose();

            this.strategy.Verify(s => s.Dispose());
        }

        private void ShouldCreateShutdownExecutionContextWithShutdownExecutor(Action executionAction)
        {
            this.InitializeTestee();

            executionAction();

            this.reportingContext.Verify(c => c.CreateShutdownExecutionContext(this.shutdownExecutor.Object));
        }

        private void ShouldProvideShutdownExecutionContextForShutdownExecutor(Action executionAction)
        {
            var shutdownExecutionContext = Mock.Of<IExecutionContext>();

            this.reportingContext.Setup(c => c.CreateShutdownExecutionContext(It.IsAny<IDescribable>()))
                .Returns(shutdownExecutionContext);

            this.InitializeTestee();

            executionAction();

            this.shutdownExecutor.Verify(r => r.Execute(It.IsAny<ISyntax<IExtension>>(), It.IsAny<IEnumerable<IExtension>>(), shutdownExecutionContext));
        }

        private void ShouldBuildShutdownSyntax(Action executionAction)
        {
            this.InitializeTestee();

            executionAction();

            this.strategy.Verify(s => s.BuildShutdownSyntax());
        }

        private void ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor(Action executionAction)
        {
            var shutdownSyntax = new Mock<ISyntax<IExtension>>();
            this.strategy.Setup(s => s.BuildShutdownSyntax()).Returns(shutdownSyntax.Object);

            var extensions = new List<IExtension> { Mock.Of<IExtension>(), };
            this.extensionHost.Setup(e => e.Extensions).Returns(extensions);

            this.InitializeTestee();

            executionAction();

            this.shutdownExecutor.Verify(r => r.Execute(shutdownSyntax.Object, extensions, It.IsAny<IExecutionContext>()));
        }

        private void ShouldReportBeforeStrategyDisposal(Action executionAction)
        {
            var queue = new Queue<string>();

            this.strategy.Setup(s => s.Dispose()).Callback(() => queue.Enqueue("Dispose"));
            this.reporter.Setup(s => s.Report(It.IsAny<IReportingContext>())).Callback<IReportingContext>(ctx => queue.Enqueue("Report"));

            this.InitializeTestee();

            executionAction();

            queue.Should().ContainInOrder(new List<string> { "Report", "Dispose" });
        }

        private void SetupStrategyReturnsBuilderAndContextAndResolver()
        {
            this.strategy.Setup(s => s.CreateReportingContext()).Returns(this.reportingContext.Object);
            this.strategy.Setup(s => s.CreateExtensionResolver()).Returns(this.extensionResolver.Object);
            this.strategy.Setup(s => s.CreateRunExecutor()).Returns(this.runExecutor.Object);
            this.strategy.Setup(s => s.CreateShutdownExecutor()).Returns(this.shutdownExecutor.Object);
        }

        private void InitializeTestee()
        {
            this.SetupStrategyReturnsBuilderAndContextAndResolver();

            this.testee.Initialize(this.strategy.Object);
        }
    }
}
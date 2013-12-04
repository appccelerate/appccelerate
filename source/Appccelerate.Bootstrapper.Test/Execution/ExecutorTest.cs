//-------------------------------------------------------------------------------
// <copyright file="ExecutorTest.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Execution
{
    using System.Collections.Generic;
    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Bootstrapper.Syntax;
    using Appccelerate.Formatters;
    using FluentAssertions;
    using Moq;
    using Xunit.Extensions;

    public class ExecutorTest
    {
        private readonly Mock<IExecutionContext> executionContext;

        private readonly Mock<IExecutable<IExtension>> firstExecutable;

        private readonly Mock<IExecutable<IExtension>> secondExecutable;

        private readonly List<IExecutable<IExtension>> executables;

        private readonly Mock<ISyntax<IExtension>> syntax;

        private readonly List<IExtension> extensions;

        public ExecutorTest()
        {
            this.firstExecutable = new Mock<IExecutable<IExtension>>();
            this.secondExecutable = new Mock<IExecutable<IExtension>>();

            this.executionContext = new Mock<IExecutionContext> { DefaultValue = DefaultValue.Mock };
            this.executables = new List<IExecutable<IExtension>> { this.firstExecutable.Object, this.secondExecutable.Object };
            this.syntax = new Mock<ISyntax<IExtension>>();

            this.extensions = new List<IExtension> { Mock.Of<IExtension>(), };
        }

        public static IEnumerable<object[]> Testees
        {
            get
            {
                yield return new object[] { new SynchronousExecutor<IExtension>() };
                yield return new object[] { new SynchronousReverseExecutor<IExtension>() };
            }
        }

        [Theory]
        [PropertyData("Testees")]
        public void Execute_ShouldExecuteSyntaxWithExtensions(IExecutor<IExtension> testee)
        {
            this.SetupSyntaxReturnsExecutables();

            testee.Execute(this.syntax.Object, this.extensions, this.executionContext.Object);

            this.firstExecutable.Verify(e => e.Execute(this.extensions, It.IsAny<IExecutableContext>()));
            this.secondExecutable.Verify(e => e.Execute(this.extensions, It.IsAny<IExecutableContext>()));
        }

        [Theory]
        [PropertyData("Testees")]
        public void Execute_ShouldCreateExecutableContextForExecutables(IExecutor<IExtension> testee)
        {
            this.SetupSyntaxReturnsExecutables();

            testee.Execute(this.syntax.Object, this.extensions, this.executionContext.Object);

            this.executionContext.Verify(e => e.CreateExecutableContext(this.firstExecutable.Object));
            this.executionContext.Verify(e => e.CreateExecutableContext(this.secondExecutable.Object));
        }

        [Theory]
        [PropertyData("Testees")]
        public void Execute_ShouldProvideExecutableContextForExecutables(IExecutor<IExtension> testee)
        {
            this.SetupSyntaxReturnsExecutables();

            var firstExecutableContext = Mock.Of<IExecutableContext>();
            var secondExecutableContext = Mock.Of<IExecutableContext>();

            this.executionContext.Setup(e => e.CreateExecutableContext(this.firstExecutable.Object))
                .Returns(firstExecutableContext);
            this.executionContext.Setup(e => e.CreateExecutableContext(this.secondExecutable.Object))
                .Returns(secondExecutableContext);

            testee.Execute(this.syntax.Object, this.extensions, this.executionContext.Object);

            this.firstExecutable.Verify(e => e.Execute(It.IsAny<IEnumerable<IExtension>>(), firstExecutableContext));
            this.secondExecutable.Verify(e => e.Execute(It.IsAny<IEnumerable<IExtension>>(), secondExecutableContext));
        }

        [Theory]
        [PropertyData("Testees")]
        public void Name_ShouldReturnTypeName(IExecutor<IExtension> testee)
        {
            string expectedName = testee.GetType().FullNameToString();

            testee.Name.Should().Be(expectedName);
        }

        private void SetupSyntaxReturnsExecutables()
        {
            this.syntax.Setup(s => s.GetEnumerator()).Returns(this.executables.GetEnumerator());
        }
    }
}
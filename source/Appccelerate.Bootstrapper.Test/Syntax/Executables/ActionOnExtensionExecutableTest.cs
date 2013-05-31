//-------------------------------------------------------------------------------
// <copyright file="ActionOnExtensionExecutableTest.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Syntax.Executables
{
    using System.Collections.Generic;
    using Appccelerate.Bootstrapper.Dummies;
    using Appccelerate.Bootstrapper.Reporting;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ActionOnExtensionExecutableTest
    {
        private readonly Mock<IExecutableContext> executableContext;

        private readonly ActionOnExtensionExecutable<ICustomExtension> testee;

        public ActionOnExtensionExecutableTest()
        {
            this.executableContext = new Mock<IExecutableContext>();

            this.testee = new ActionOnExtensionExecutable<ICustomExtension>(x => x.Dispose());
        }

        [Fact]
        public void Execute_ShouldExecuteActionOnExtensions()
        {
            var firstExtension = new Mock<ICustomExtension>();
            var secondExtension = new Mock<ICustomExtension>();

            this.testee.Execute(new List<ICustomExtension> { firstExtension.Object, secondExtension.Object }, this.executableContext.Object);

            firstExtension.Verify(x => x.Dispose());
            secondExtension.Verify(x => x.Dispose());
        }

        [Fact]
        public void ShoulDescribeItself()
        {
            this.testee.Describe().Should().Be("Executes \"x => x.Dispose()\" on each extension during bootstrapping.");
        }
    }
}
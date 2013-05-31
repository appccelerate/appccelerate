//-------------------------------------------------------------------------------
// <copyright file="ActionOnExtensionWithInitializerExecutableTest.cs" company="Appccelerate">
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

    public class ActionOnExtensionWithInitializerExecutableTest
    {
        private readonly object context;

        private readonly ActionOnExtensionWithInitializerExecutable<object, ICustomExtension> testee;

        private readonly Mock<IExecutableContext> executableContext;

        private int contextAccessCounter;

        private IBehaviorAware<IExtension> interceptedBehaviorAware;

        public ActionOnExtensionWithInitializerExecutableTest()
        {
            this.executableContext = new Mock<IExecutableContext>();

            this.context = new object();

            this.testee = new ActionOnExtensionWithInitializerExecutable<object, ICustomExtension>(
                () => this.CountAccessToContext(),
                (x, i) => x.SomeMethod(i),
                (aware, ctx) => { this.interceptedBehaviorAware = aware; });
        }

        [Fact]
        public void Execute_ShouldCallInitializerOnce()
        {
            this.testee.Execute(new List<ICustomExtension> { Mock.Of<ICustomExtension>(), Mock.Of<ICustomExtension>() }, this.executableContext.Object);

            this.contextAccessCounter.Should().Be(1);
        }

        [Fact]
        public void Execute_ShouldPassItselfToInitializer()
        {
            this.testee.Execute(new List<ICustomExtension> { Mock.Of<ICustomExtension>(), Mock.Of<ICustomExtension>() }, this.executableContext.Object);

            this.interceptedBehaviorAware.Should().Be(this.testee);
        }

        [Fact]
        public void Execute_ShouldExecuteActionOnExtensions()
        {
            var firstExtension = new Mock<ICustomExtension>();
            var secondExtension = new Mock<ICustomExtension>();

            this.testee.Execute(new List<ICustomExtension> { firstExtension.Object, secondExtension.Object }, this.executableContext.Object);

            firstExtension.Verify(x => x.SomeMethod(It.IsAny<object>()));
            secondExtension.Verify(x => x.SomeMethod(It.IsAny<object>()));
        }

        [Fact]
        public void Execute_ShouldPassContextToExtensions()
        {
            var firstExtension = new Mock<ICustomExtension>();
            var secondExtension = new Mock<ICustomExtension>();

            this.testee.Execute(new List<ICustomExtension> { firstExtension.Object, secondExtension.Object }, this.executableContext.Object);

            firstExtension.Verify(x => x.SomeMethod(this.context));
            secondExtension.Verify(x => x.SomeMethod(this.context));
        }

        [Fact]
        public void ShouldDescribeItself()
        {
            this.testee.Describe().Should().Be("Initializes the context once with \"() => value(Appccelerate.Bootstrapper.Syntax.Executables.ActionOnExtensionWithInitializerExecutableTest).CountAccessToContext()\" and executes \"(x, i) => x.SomeMethod(i)\" on each extension during bootstrapping.");
        }

        private object CountAccessToContext()
        {
            this.contextAccessCounter++;
            return this.context;
        }
    }
}
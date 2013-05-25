//-------------------------------------------------------------------------------
// <copyright file="ExecutableFactoryTest.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Syntax
{
    using Appccelerate.Bootstrapper.Syntax.Executables;
    using FluentAssertions;
    using Xunit;

    public class ExecutableFactoryTest
    {
        private readonly ExecutableFactory<IExtension> testee;

        public ExecutableFactoryTest()
        {
            this.testee = new ExecutableFactory<IExtension>();
        }

        [Fact]
        public void CreateExecutable_WithActionOnExtension_ShouldCreateActionOnExtensionExecutable()
        {
            IExecutable<IExtension> executable = this.testee.CreateExecutable(e => this.ActionOnExtension(e));

            executable.Should().BeOfType<ActionOnExtensionExecutable<IExtension>>();
        }

        [Fact]
        public void CreateExecutable_WithAction_ShouldCreateActionExecutable()
        {
            IExecutable<IExtension> executable = this.testee.CreateExecutable(() => this.Action());

            executable.Should().BeOfType<ActionExecutable<IExtension>>();
        }

        [Fact]
        public void CreateExecutable_WithActionOnExtensionAndInitializer_ShouldCreateActionOnExtensionWithInitializerExecutable()
        {
            IExecutable<IExtension> executable = this.testee.CreateExecutable(() => "AnyContext", (e, ctx) => this.ActionOnExtensionWithContext(e, ctx), (aware, ctx) => { });

            executable.Should().BeOfType<ActionOnExtensionWithInitializerExecutable<string, IExtension>>();
        }

        private void ActionOnExtensionWithContext(IExtension extension, string context)
        {
        }

        private void ActionOnExtension(IExtension extension)
        {
        }

        private void Action()
        {
        }
    }
}
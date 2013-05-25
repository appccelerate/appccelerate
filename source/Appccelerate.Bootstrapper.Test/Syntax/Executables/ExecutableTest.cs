//-------------------------------------------------------------------------------
// <copyright file="ExecutableTest.cs" company="Appccelerate">
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
    using System.Linq;
    using Appccelerate.Bootstrapper.Dummies;
    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Formatters;
    using FluentAssertions;
    using Moq;
    using Xunit.Extensions;

    public class ExecutableTest
    {
        private readonly Mock<IExecutableContext> executableContext;

        public ExecutableTest()
        {
            this.executableContext = new Mock<IExecutableContext>();
        }

        public static IEnumerable<object[]> Testees
        {
            get
            {
                yield return new object[] { new ActionExecutable<ICustomExtension>(() => ActionHelper()) };
                yield return new object[] { new ActionOnExtensionExecutable<ICustomExtension>(x => x.Dispose()) };
                yield return
                    new object[]
                        {
                            new ActionOnExtensionWithInitializerExecutable<object, ICustomExtension>(
                                () => FunctionHelper(), (x, i) => x.SomeMethod(i), (aware, ctx) => { })
                        };
            }
        }

        [Theory]
        [PropertyData("Testees")]
        public void ShouldReturnTypeName(IExecutable<ICustomExtension> testee)
        {
            string expectedName = testee.GetType().FullNameToString();

            testee.Name.Should().Be(expectedName);
        }

        [Theory]
        [PropertyData("Testees")]
        public void Execute_ShouldExecuteBehavior(IExecutable<ICustomExtension> testee)
        {
            var first = new Mock<IBehavior<ICustomExtension>>();
            var second = new Mock<IBehavior<ICustomExtension>>();
            var extensions = Enumerable.Empty<ICustomExtension>();

            testee.Add(first.Object);
            testee.Add(second.Object);

            testee.Execute(extensions, this.executableContext.Object);

            first.Verify(b => b.Behave(extensions));
            second.Verify(b => b.Behave(extensions));
        }

        [Theory]
        [PropertyData("Testees")]
        public void Execute_ShouldCreateBehaviorContextForBehaviors(IExecutable<ICustomExtension> testee)
        {
            var first = new Mock<IBehavior<ICustomExtension>>();
            var second = new Mock<IBehavior<ICustomExtension>>();

            testee.Add(first.Object);
            testee.Add(second.Object);

            testee.Execute(Enumerable.Empty<ICustomExtension>(), this.executableContext.Object);

            this.executableContext.Verify(e => e.CreateBehaviorContext(first.Object));
            this.executableContext.Verify(e => e.CreateBehaviorContext(second.Object));
        }

        private static void ActionHelper()
        {
        }

        private static object FunctionHelper()
        {
            return null;
        }
    }
}
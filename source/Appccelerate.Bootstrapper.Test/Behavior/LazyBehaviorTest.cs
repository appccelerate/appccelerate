//-------------------------------------------------------------------------------
// <copyright file="LazyBehaviorTest.cs" company="Appccelerate">
//   Copyright (c) 2008-2013
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace Appccelerate.Bootstrapper.Behavior
{
    using System.Linq;
    using Appccelerate.Bootstrapper.Dummies;
    using Appccelerate.Formatters;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class LazyBehaviorTest
    {
        private readonly Mock<IBehavior<ICustomExtension>> lazyBehavior;

        private readonly LazyBehavior<ICustomExtension> testee;

        private int accessCounter;

        public LazyBehaviorTest()
        {
            this.lazyBehavior = new Mock<IBehavior<ICustomExtension>>();

            this.testee = new LazyBehavior<ICustomExtension>(() => this.DelayCreation());
        }

        [Fact]
        public void Constructor_ShouldNotCreateBehavior()
        {
            this.accessCounter.Should().Be(default(int));
        }

        [Fact]
        public void Behave_ShouldCreateBehavior()
        {
            const int AccessedOnce = 1;

            this.testee.Behave(Enumerable.Empty<ICustomExtension>());

            this.accessCounter.Should().Be(AccessedOnce);
        }

        [Fact]
        public void Behave_ShouldBehaveOnLazyBehavior()
        {
            var expectedExtensions = Enumerable.Empty<ICustomExtension>();

            this.testee.Behave(expectedExtensions);

            this.lazyBehavior.Verify(b => b.Behave(expectedExtensions));
        }

        [Fact]
        public void ShouldReturnTypeName()
        {
            string expectedName = this.testee.GetType().FullNameToString();

            this.testee.Name.Should().Be(expectedName);
        }

        [Fact]
        public void ShouldDescribeItself()
        {
            this.testee.Describe().Should().Be("Creates the behavior with () => value(Appccelerate.Bootstrapper.Behavior.LazyBehaviorTest).DelayCreation() and executes behave on the lazy initialized behavior.");
        }

        private IBehavior<ICustomExtension> DelayCreation()
        {
            this.accessCounter++;
            return this.lazyBehavior.Object;
        }
    }
}
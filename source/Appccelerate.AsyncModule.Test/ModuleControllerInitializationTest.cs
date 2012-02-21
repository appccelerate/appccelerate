//-------------------------------------------------------------------------------
// <copyright file="ModuleControllerInitializationTest.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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

namespace Appccelerate.AsyncModule
{
    using System;

    using FluentAssertions;

    using Xunit;

    public class ModuleControllerInitializationTest
    {
        [Fact]
        public void InitWithNullModuleThrowsArgumentNullException()
        {
            var testee = new ModuleController();
            testee.Invoking(t => t.Initialize(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void InitWithNegativeThreadNumberThrowsArgumentException()
        {
            var testee = new ModuleController();
            testee.Invoking(t => t.Initialize(new TestModule(), -1))
                .ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void InitWithZeroThreadNumberThrowsArgumentException()
        {
            var testee = new ModuleController();
            testee.Invoking(t => t.Initialize(new TestModule(), 0))
                .ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void InitWithModuleWithWrongMessageCOnsumerMethodSignature()
        {
            var testee = new ModuleController();
            testee.Invoking(t => t.Initialize(new ModuleWithWrongMessageConsumerMethodSignature()))
                .ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void InitWithModuleWithModuleWithNoMessageConsumerMethod()
        {
            var testee = new ModuleController();
            testee.Invoking(t => t.Initialize(new ModuleWithNoMessageConsumerMethod()))
                .ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void StartWithForgroundThreads()
        {
            ModuleController testee = new ModuleController();
            testee.Initialize(new TestModule());
            testee.Start();

            testee.IsAlive.Should().BeTrue();
        }

        [Fact]
        public void StartWithBackgroundThreads()
        {
            ModuleController testee = new ModuleController();
            testee.Initialize(new TestModule(), true);
            testee.Start();

            testee.IsAlive.Should().BeTrue();
        }

        public class ModuleWithWrongMessageConsumerMethodSignature
        {
            [MessageConsumer]
            public void Consume(int i, string s)
            {
            }
        }

        public class ModuleWithNoMessageConsumerMethod
        {
            public void Consume(int i)
            {
            }
        }
    }
}
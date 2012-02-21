//-------------------------------------------------------------------------------
// <copyright file="SkipIfDuplicateInQueueModuleExtensionTest.cs" company="Appccelerate">
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

namespace Appccelerate.AsyncModule.Extensions
{
    using Appccelerate.AsyncModule.Events;
    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class SkipIfDuplicateInQueueModuleExtensionTest
    {
        private readonly IModuleController controller;

        private readonly SkipIfDuplicateInQueueModuleExtension testee;

        public SkipIfDuplicateInQueueModuleExtensionTest()
        {
            this.controller = A.Fake<IModuleController>();

            this.testee = new SkipIfDuplicateInQueueModuleExtension
                {
                    ModuleController = this.controller
                };

            this.testee.Attach();
        }

        [Fact]
        public void MessagesAreConsumedIfNoMessageInQueue()
        {
            A.CallTo(() => this.controller.Messages).Returns(new object[] { });

            BeforeConsumeMessageEventArgs e = new BeforeConsumeMessageEventArgs(this, "test");

            this.controller.BeforeConsumeMessage += Raise.With(e).Now;

           e.Cancel.Should().BeFalse();
        }

        [Fact]
        public void MessagesAreConsumedIfNoDuplicateInQueue()
        {
            A.CallTo(() => this.controller.Messages).Returns(new object[] { "hello", "world" });

            BeforeConsumeMessageEventArgs e = new BeforeConsumeMessageEventArgs(this, "test");

            this.controller.BeforeConsumeMessage += Raise.With(e).Now;

            e.Cancel.Should().BeFalse();
        }

        [Fact]
        public void MessagesAreNotConsumedIfDuplicateInQueue()
        {
            A.CallTo(() => this.controller.Messages).Returns(new object[] { "hello", "test", "world" });

            BeforeConsumeMessageEventArgs e = new BeforeConsumeMessageEventArgs(this, "test");

            this.controller.BeforeConsumeMessage += Raise.With(e).Now;

            e.Cancel.Should().BeTrue();
        }
    }
}
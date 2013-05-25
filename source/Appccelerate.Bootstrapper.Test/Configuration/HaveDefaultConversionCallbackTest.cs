//-------------------------------------------------------------------------------
// <copyright file="HaveDefaultConversionCallbackTest.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Configuration
{
    using Appccelerate.Bootstrapper.Configuration.Internals;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class HaveDefaultConversionCallbackTest
    {
        [Fact]
        public void DefaultConversionCallback_ExtensionNotIHaveDefaultConversionCallback_ShouldUseDefault()
        {
            var extension = new Mock<IExtension>();

            var testee = new HaveDefaultConversionCallback(extension.Object);

            testee.DefaultConversionCallback.Should().BeOfType<DefaultConversionCallback>();
        }

        [Fact]
        public void DefaultConversionCallback_ExtensionIsIHaveDefaultConversionCallback_ShouldAcquireDefaultCallbackFromExtension()
        {
            var extension = new Mock<IExtension>();
            var consumer = extension.As<IHaveDefaultConversionCallback>();
            var expected = Mock.Of<IConversionCallback>();

            consumer.Setup(n => n.DefaultConversionCallback).Returns(expected);

            var testee = new HaveDefaultConversionCallback(extension.Object);
            testee.DefaultConversionCallback.Should().Be(expected);
        }
    }
}
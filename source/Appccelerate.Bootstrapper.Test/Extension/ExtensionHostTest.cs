//-------------------------------------------------------------------------------
// <copyright file="ExtensionHostTest.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Extension
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class ExtensionHostTest
    {
        private readonly ExtensionHost<IExtension> testee;

        public ExtensionHostTest()
        {
            this.testee = new ExtensionHost<IExtension>();
        }

        [Fact]
        public void ShouldTrackExtension()
        {
            var extension = Mock.Of<IExtension>();

            this.testee.AddExtension(extension);

            this.testee.Extensions.Should().Contain(extension).And.HaveCount(1);
        }

        [Fact]
        public void ShouldTrackExtensionInOrder()
        {
            var firstExtension = Mock.Of<IExtension>();
            var secondExtension = Mock.Of<IExtension>();

            this.testee.AddExtension(firstExtension);
            this.testee.AddExtension(secondExtension);

            this.testee.Extensions.Should().ContainInOrder(new List<IExtension> { firstExtension, secondExtension }).And.HaveCount(2);
        }
    }
}
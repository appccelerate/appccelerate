//-------------------------------------------------------------------------------
// <copyright file="ConsumeConfigurationSectionTest.cs" company="Appccelerate">
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

    using Moq;

    using Xunit;

    public class ConsumeConfigurationSectionTest
    {
        [Fact]
        public void Apply_WhenExtensionIConsumeConfigurationSection_ShouldApplySection()
        {
            var extension = new Mock<IExtension>();
            var consumer = extension.As<IConsumeConfigurationSection>();

            var testee = new ConsumeConfigurationSection(extension.Object);
            testee.Apply(null);

            consumer.Verify(c => c.Apply(null));
        }

        [Fact]
        public void Apply_WhenExtensionNotIConsumeConfigurationSection_ShouldNotApplySection()
        {
            var extension = new Mock<IExtension>(MockBehavior.Strict);
            var testee = new ConsumeConfigurationSection(extension.Object);
            testee.Apply(null);

            extension.VerifyAll();
        }
    }
}
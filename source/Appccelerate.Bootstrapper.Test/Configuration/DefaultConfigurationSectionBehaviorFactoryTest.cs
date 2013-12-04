//-------------------------------------------------------------------------------
// <copyright file="DefaultConfigurationSectionBehaviorFactoryTest.cs" company="Appccelerate">
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

    public class DefaultConfigurationSectionBehaviorFactoryTest
    {
        private readonly DefaultConfigurationSectionBehaviorFactory testee;

        public DefaultConfigurationSectionBehaviorFactoryTest()
        {
            this.testee = new DefaultConfigurationSectionBehaviorFactory();
        }

        [Fact]
        public void CreateHaveConfigurationSectionName_ShouldCreateDefaultHaveConfigurationSectionName()
        {
            var sectionNameProvider = this.testee.CreateHaveConfigurationSectionName(Mock.Of<IExtension>());

            sectionNameProvider.Should().BeOfType<HaveConfigurationSectionName>();
        }

        [Fact]
        public void CreateLoadConfigurationSection_ShouldCreateDefaultLoadConfigurationSection()
        {
            var sectionProvider = this.testee.CreateLoadConfigurationSection(Mock.Of<IExtension>());

            sectionProvider.Should().BeOfType<LoadConfigurationSection>();
        }

        [Fact]
        public void CreateConsumeConfigurationSection_ShouldCreateDefaultConsumeConfigurationSection()
        {
            var consumer = this.testee.CreateConsumeConfigurationSection(Mock.Of<IExtension>());

            consumer.Should().BeOfType<ConsumeConfigurationSection>();
        }
    }
}
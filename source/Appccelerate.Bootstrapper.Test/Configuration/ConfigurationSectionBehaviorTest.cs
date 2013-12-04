//-------------------------------------------------------------------------------
// <copyright file="ConfigurationSectionBehaviorTest.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Configuration;
    using Appccelerate.Formatters;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ConfigurationSectionBehaviorTest
    {
        private readonly Mock<IConsumeConfigurationSection> consumer;

        private readonly Mock<IHaveConfigurationSectionName> sectionNameProvider;

        private readonly Mock<ILoadConfigurationSection> sectionProvider;

        private readonly Mock<IConfigurationSectionBehaviorFactory> factory;

        private readonly List<IExtension> extensions;

        private readonly ConfigurationSectionBehavior testee;

        public ConfigurationSectionBehaviorTest()
        {
            this.consumer = new Mock<IConsumeConfigurationSection>();
            this.sectionNameProvider = new Mock<IHaveConfigurationSectionName>();
            this.sectionProvider = new Mock<ILoadConfigurationSection>();

            this.factory = new Mock<IConfigurationSectionBehaviorFactory>();
            this.AutoStubFactory();

            this.extensions = new List<IExtension> { Mock.Of<IExtension>(), Mock.Of<IExtension>(), };

            this.testee = new ConfigurationSectionBehavior(this.factory.Object);
        }

        [Fact]
        public void Behave_ShouldApply()
        {
            this.testee.Behave(this.extensions);

            this.consumer.Verify(c => c.Apply(It.IsAny<ConfigurationSection>()));
        }

        [Fact]
        public void Behave_ShouldApplySectionFromProvider()
        {
            var configurationSection = Mock.Of<ConfigurationSection>();
            this.sectionProvider.Setup(p => p.GetSection(It.IsAny<string>())).Returns(configurationSection);

            this.testee.Behave(this.extensions);

            this.consumer.Verify(c => c.Apply(configurationSection));
        }

        [Fact]
        public void Behave_ShouldAcquireSectionName()
        {
            this.testee.Behave(this.extensions);

            this.sectionNameProvider.Verify(p => p.SectionName);
        }

        [Fact]
        public void Behave_ShouldAcquireSectionByName()
        {
            const string AnySectionName = "SectionName";

            this.sectionNameProvider.Setup(p => p.SectionName).Returns(AnySectionName);

            this.testee.Behave(this.extensions);

            this.sectionProvider.Verify(p => p.GetSection(AnySectionName));
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
            this.testee.Describe().Should().Be("Automatically provides configuration sections for all extensions.");
        }

        private void AutoStubFactory()
        {
            this.factory.Setup(f => f.CreateConsumeConfigurationSection(It.IsAny<IExtension>())).Returns(
                this.consumer.Object);
            this.factory.Setup(f => f.CreateHaveConfigurationSectionName(It.IsAny<IExtension>())).Returns(
                this.sectionNameProvider.Object);
            this.factory.Setup(f => f.CreateLoadConfigurationSection(It.IsAny<IExtension>())).Returns(
                this.sectionProvider.Object);
        }
    }
}
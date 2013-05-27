//-------------------------------------------------------------------------------
// <copyright file="ExtensionConfigurationSectionBehaviorTest.cs" company="Appccelerate">
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
    using Appccelerate.Formatters;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ExtensionConfigurationSectionBehaviorTest
    {
        private readonly Mock<IExtensionConfigurationSectionBehaviorFactory> factory;

        private readonly Mock<IHaveConversionCallbacks> conversionCallbacksProvider;

        private readonly Mock<ILoadConfigurationSection> sectionProvider;

        private readonly Mock<IConsumeConfiguration> consumer;

        private readonly Mock<IHaveConfigurationSectionName> sectionNameProvider;

        private readonly Mock<IReflectExtensionProperties> extensionPropertyReflector;

        private readonly List<IExtension> extensions;

        private readonly Mock<IAssignExtensionProperties> assigner;

        private readonly Mock<IHaveDefaultConversionCallback> defaultConversionCallbackProvider;

        private readonly ExtensionConfigurationSectionBehavior testee;

        public ExtensionConfigurationSectionBehaviorTest()
        {
            this.consumer = new Mock<IConsumeConfiguration>();
            this.extensionPropertyReflector = new Mock<IReflectExtensionProperties>();
            this.sectionNameProvider = new Mock<IHaveConfigurationSectionName>();
            this.sectionProvider = new Mock<ILoadConfigurationSection>();
            this.conversionCallbacksProvider = new Mock<IHaveConversionCallbacks>();
            this.defaultConversionCallbackProvider = new Mock<IHaveDefaultConversionCallback>();
            this.assigner = new Mock<IAssignExtensionProperties>();

            this.factory = new Mock<IExtensionConfigurationSectionBehaviorFactory>();
            this.SetupAutoStubFactory();

            this.extensions = new List<IExtension> { Mock.Of<IExtension>(), };

            this.testee = new ExtensionConfigurationSectionBehavior(this.factory.Object);
        }

        [Fact]
        public void Behave_ShouldConsumeSectionFromProvider()
        {
            var expectedConfiguration = new KeyValuePair<string, string>("AnyKey", "AnyValue");
            var configuration = new Dictionary<string, string>();

            var configurationSection = ExtensionConfigurationSectionHelper.CreateSection(expectedConfiguration);

            this.sectionProvider.Setup(p => p.GetSection(It.IsAny<string>())).Returns(configurationSection);
            this.consumer.Setup(c => c.Configuration).Returns(configuration);

            this.testee.Behave(this.extensions);

            configuration.Should().Contain(expectedConfiguration);
        }

        [Fact]
        public void Behave_ShouldAcquireSectionName()
        {
            this.SetupEmptyConsumerConfiguration();

            this.testee.Behave(this.extensions);

            this.sectionNameProvider.Verify(p => p.SectionName);
        }

        [Fact]
        public void Behave_ShouldAcquireSectionByName()
        {
            this.SetupEmptyConsumerConfiguration();

            const string AnySectionName = "SectionName";

            this.sectionNameProvider.Setup(p => p.SectionName).Returns(AnySectionName);

            this.testee.Behave(this.extensions);

            this.sectionProvider.Verify(p => p.GetSection(AnySectionName));
        }

        [Fact]
        public void Behave_ShouldAssign()
        {
            this.SetupEmptyConsumerConfiguration();
            this.SetupExtensionConfigurationSectionWithEntries();

            this.testee.Behave(this.extensions);

            this.assigner.Verify(a => a.Assign(this.extensionPropertyReflector.Object, It.IsAny<IExtension>(), this.consumer.Object, this.conversionCallbacksProvider.Object, this.defaultConversionCallbackProvider.Object));
        }

        [Fact]
        public void Behave_ShouldNotProceedWhenNoConfigurationAvailable()
        {
            var configurationSection = ExtensionConfigurationSectionHelper.CreateSection(new Dictionary<string, string>());

            this.sectionProvider.Setup(p => p.GetSection(It.IsAny<string>())).Returns(configurationSection);

            this.testee.Behave(this.extensions);

            this.consumer.Verify(c => c.Configuration, Times.Never());
            this.assigner.Verify(a => a.Assign(It.IsAny<IReflectExtensionProperties>(), It.IsAny<IExtension>(), It.IsAny<IConsumeConfiguration>(), It.IsAny<IHaveConversionCallbacks>(), It.IsAny<IHaveDefaultConversionCallback>()), Times.Never());
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
            this.testee.Describe().Should().Be("Automatically propagates properties of all extensions with configuration values when a matching ExtensionConfigurationSection is found.");
        }

        private void SetupExtensionConfigurationSectionWithEntries()
        {
            var configurationSection = ExtensionConfigurationSectionHelper.CreateSection(new Dictionary<string, string> { { "AnyKey", "AnyValue" } });
            this.sectionProvider.Setup(p => p.GetSection(It.IsAny<string>())).Returns(configurationSection);
        }

        private void SetupEmptyConsumerConfiguration()
        {
            this.consumer.Setup(x => x.Configuration).Returns(new Dictionary<string, string>());
        }

        private void SetupAutoStubFactory()
        {
            this.factory.Setup(x => x.CreateConsumeConfiguration(It.IsAny<IExtension>())).Returns(this.consumer.Object);
            this.factory.Setup(x => x.CreateReflectExtensionProperties()).Returns(
                this.extensionPropertyReflector.Object);
            this.factory.Setup(x => x.CreateAssignExtensionProperties()).Returns(this.assigner.Object);
            this.factory.Setup(x => x.CreateHaveConfigurationSectionName(It.IsAny<IExtension>())).Returns(
                this.sectionNameProvider.Object);
            this.factory.Setup(x => x.CreateHaveConversionCallbacks(It.IsAny<IExtension>())).Returns(
                this.conversionCallbacksProvider.Object);
            this.factory.Setup(x => x.CreateHaveDefaultConversionCallback(It.IsAny<IExtension>())).Returns(
                this.defaultConversionCallbackProvider.Object);
            this.factory.Setup(x => x.CreateLoadConfigurationSection(It.IsAny<IExtension>())).Returns(
                this.sectionProvider.Object);
        }
    }
}
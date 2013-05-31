//-------------------------------------------------------------------------------
// <copyright file="DefaultExtensionConfigurationSectionBehaviorFactoryTest.cs" company="Appccelerate">
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

    public class DefaultExtensionConfigurationSectionBehaviorFactoryTest
    {
        private readonly DefaultExtensionConfigurationSectionBehaviorFactory testee;

        public DefaultExtensionConfigurationSectionBehaviorFactoryTest()
        {
            this.testee = new DefaultExtensionConfigurationSectionBehaviorFactory();
        }

        [Fact]
        public void CreateConsumeConfiguration_ShouldDefaultCreateConsumeConfiguration()
        {
            var consumer = this.testee.CreateConsumeConfiguration(Mock.Of<IExtension>());

            consumer.Should().BeOfType<ConsumeConfiguration>();
        }

        [Fact]
        public void CreateReflectExtensionProperties_ShouldCreateDefaultReflectExtensionProperties()
        {
            var reflector = this.testee.CreateReflectExtensionProperties();

            reflector.Should().BeOfType<ReflectExtensionPublicProperties>();
        }

        [Fact]
        public void CreateAssignExtensionProperties_ShouldCreateDefaultAssingExtensionProperties()
        {
            var assigner = this.testee.CreateAssignExtensionProperties();

            assigner.Should().BeOfType<AssignExtensionProperties>();
        }

        [Fact]
        public void CreateHaveConfigurationSectionName_ShouldCreateDefaultHaveConfigurationSectionName()
        {
            var sectionNameProvider = this.testee.CreateHaveConfigurationSectionName(Mock.Of<IExtension>());

            sectionNameProvider.Should().BeOfType<HaveConfigurationSectionName>();
        }

        [Fact]
        public void CreateHaveConversionCallbacks_ShouldCreateDefaultHaveConversionCallbacks()
        {
            var conversionCallbacks = this.testee.CreateHaveConversionCallbacks(Mock.Of<IExtension>());

            conversionCallbacks.Should().BeOfType<HaveConversionCallbacks>();
        }

        [Fact]
        public void CreateHaveDefaultConversionCallback_ShouldCreateDefaultHaveDefaultConversionCallback()
        {
            var defaultConversionCallback = this.testee.CreateHaveDefaultConversionCallback(Mock.Of<IExtension>());

            defaultConversionCallback.Should().BeOfType<HaveDefaultConversionCallback>();
        }

        [Fact]
        public void CreateLoadConfigurationSection_ShouldCreateDefaultLoadConfigurationSection()
        {
            var sectionProvider = this.testee.CreateLoadConfigurationSection(Mock.Of<IExtension>());

            sectionProvider.Should().BeOfType<LoadConfigurationSection>();
        }
    }
}
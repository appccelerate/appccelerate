//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapper_is_run_with_extension_configuration_section_behavior_and_extension_with_customized_loading.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Specification
{
    using Appccelerate.Bootstrapper.Specification.Dummies;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern)]
    public class when_the_bootstrapper_is_run_with_extension_configuration_section_behavior_and_extension_with_customized_loading : BootstrapperWithExtensionConfigurationSectionBehaviorSpecification
    {
        protected static CustomExtensionWithExtensionConfigurationWhichHasCallbacks WithCallbacksExtension;

        protected static CustomExtensionWithExtensionConfigurationWhichConsumesConfiguration WhichConsumesConfigurationExtension;
        
        Establish context = () =>
            {
                WithCallbacksExtension = new CustomExtensionWithExtensionConfigurationWhichHasCallbacks();
                WhichConsumesConfigurationExtension = new CustomExtensionWithExtensionConfigurationWhichConsumesConfiguration();

                Bootstrapper.Initialize(Strategy);

                Bootstrapper.AddExtension(WithCallbacksExtension);
                Bootstrapper.AddExtension(WhichConsumesConfigurationExtension);
            };

        Because of = () =>
            {
                Bootstrapper.Run();
            };

        It should_use_default_conversion_callback = () =>
        {
            WithCallbacksExtension.SomeStringWithDefault.Should().Be("SomeStringWithDefault. Modified by Default!");
        };

        It should_use_conversion_callbacks = () =>
        {
            WithCallbacksExtension.SomeString.Should().Be("SomeString. Modified by Callback!");
            WithCallbacksExtension.SomeInt.Should().Be(1);
        };

        It should_ignore_not_configured_properties = () =>
        {
            WithCallbacksExtension.SomeStringWhichIsIgnored.Should().BeNull();
        };

        It should_propagate_configuration = () =>
            {
                WhichConsumesConfigurationExtension.Configuration.Should()
                    .HaveCount(3)
                    .And.Contain("SomeInt", "1")
                    .And.Contain("SomeString", "SomeString")
                    .And.Contain("SomeStringWithDefault", "SomeStringWithDefault");
            };

        It should_acquire_section = () =>
            {
                WithCallbacksExtension.SectionAcquired.Should().Be("CustomExtensionWithExtensionConfigurationWhichHasCallbacks");
                WhichConsumesConfigurationExtension.SectionAcquired.Should().Be("CustomExtensionWithExtensionConfigurationWhichConsumesConfiguration");
            };
    }
}
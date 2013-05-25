//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapper_is_run_with_configuration_section_behavior_and_extension_with_customized_loading.cs" company="Appccelerate">
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
    public class when_the_bootstrapper_is_run_with_configuration_section_behavior_and_extension_with_customized_loading : BootstrapperWithConfigurationSectionBehaviorSpecification
    {
        protected static CustomExtensionWithConfigurationWhichKnowsNameAndWhereToLoadFrom NameAndWhereToLoadFromExtension;
        protected static CustomExtensionWithConfigurationWhichKnowsWhereToLoadFrom WhereToLoadFromExtension;

        Establish context = () =>
        {
            NameAndWhereToLoadFromExtension = new CustomExtensionWithConfigurationWhichKnowsNameAndWhereToLoadFrom();
            WhereToLoadFromExtension = new CustomExtensionWithConfigurationWhichKnowsWhereToLoadFrom();
 
            Bootstrapper.Initialize(Strategy);

            Bootstrapper.AddExtension(NameAndWhereToLoadFromExtension);
            Bootstrapper.AddExtension(WhereToLoadFromExtension);
        };

        Because of = () =>
        {
            Bootstrapper.Run();
        };

        It should_apply_configuration_section = () =>
            {
                NameAndWhereToLoadFromExtension.AppliedSection.Should().NotBeNull();
                NameAndWhereToLoadFromExtension.AppliedSection.Context.Should().Be("KnowsName|KnowsLoading");

                WhereToLoadFromExtension.AppliedSection.Should().NotBeNull();
                WhereToLoadFromExtension.AppliedSection.Context.Should().Be("KnowsLoading");
            };

        It should_acquire_section_name = () =>
            {
                NameAndWhereToLoadFromExtension.SectionNameAcquired.Should().BeTrue();
            };

        It should_acquire_section = () =>
        {
            NameAndWhereToLoadFromExtension.SectionAcquired.Should().Be("FakeConfigurationSection");
            WhereToLoadFromExtension.SectionAcquired.Should().Be("CustomExtensionWithConfigurationWhichKnowsWhereToLoadFrom");
        };
    }
}
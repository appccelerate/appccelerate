//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapper_is_run_with_behavior_attached.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Linq;

    using Appccelerate.Bootstrapper.Specification.Dummies;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern)]
    public class when_the_bootstrapper_is_run_with_behavior_attached : BootstrapperWithBehaviorSpecification
    {
        Establish context = () =>
        {
            Bootstrapper.Initialize(Strategy);
            Bootstrapper.AddExtension(First);
            Bootstrapper.AddExtension(Second);
        };

        Because of = () =>
        {
            Bootstrapper.Run();
        };

        It should_only_initialize_contexts_once_for_all_extensions = () =>
        {
            Strategy.RunConfigurationInitializerAccessCounter.Should().Be(1);
        };

        It should_pass_the_initialized_values_from_the_contexts_to_the_extensions = () =>
        {
            var expected = new Dictionary<string, string>
                {
                    { "RunTest", "RunTestValue" },
                    { "RunFirstValue", "RunTestValue" },
                    { "RunSecondValue", "RunTestValue" },
                };

            First.RunConfiguration.Should().Equal(expected);
            Second.RunConfiguration.Should().Equal(expected);

            First.Registered.Should().Be("RunTest");
            Second.Registered.Should().Be("RunTest");
        };

        It should_execute_the_extensions_with_its_extension_points_and_the_behaviors_according_to_the_strategy_defined_order = () =>
        {
            var sequence = CustomExtensionBase.Sequence;

            sequence.Should().HaveCount(33, sequence.Flatten());
            sequence.ElementAt(0).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at run first beginning.");
            sequence.ElementAt(1).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at run first beginning.");
            sequence.ElementAt(2).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at run second beginning.");
            sequence.ElementAt(3).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at run second beginning.");

            sequence.ElementAt(4).Should().BeEquivalentTo("Action: CustomRun");

            sequence.ElementAt(5).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at run first start.");
            sequence.ElementAt(6).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at run first start.");
            sequence.ElementAt(7).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at run second start.");
            sequence.ElementAt(8).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at run second start.");
            sequence.ElementAt(9).Should().BeEquivalentTo("FirstExtension: Start");
            sequence.ElementAt(10).Should().BeEquivalentTo("SecondExtension: Start");

            sequence.ElementAt(11).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at configuration modification with RunFirstValue = RunTestValue.");
            sequence.ElementAt(12).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at configuration modification with RunFirstValue = RunTestValue.");
            sequence.ElementAt(13).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at configuration modification with RunSecondValue = RunTestValue.");
            sequence.ElementAt(14).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at configuration modification with RunSecondValue = RunTestValue.");
            sequence.ElementAt(15).Should().BeEquivalentTo("FirstExtension: Configure");
            sequence.ElementAt(16).Should().BeEquivalentTo("SecondExtension: Configure");

            sequence.ElementAt(17).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at run first initialize.");
            sequence.ElementAt(18).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at run first initialize.");
            sequence.ElementAt(19).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at run second initialize.");
            sequence.ElementAt(20).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at run second initialize.");
            sequence.ElementAt(21).Should().BeEquivalentTo("FirstExtension: Initialize");
            sequence.ElementAt(22).Should().BeEquivalentTo("SecondExtension: Initialize");

            sequence.ElementAt(23).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at input modification with RunTestValueFirst.");
            sequence.ElementAt(24).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at input modification with RunTestValueFirst.");
            sequence.ElementAt(25).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at input modification with RunTestValueSecond.");
            sequence.ElementAt(26).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at input modification with RunTestValueSecond.");
            sequence.ElementAt(27).Should().BeEquivalentTo("FirstExtension: Register");
            sequence.ElementAt(28).Should().BeEquivalentTo("SecondExtension: Register");

            sequence.ElementAt(29).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at run first end.");
            sequence.ElementAt(30).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at run first end.");
            sequence.ElementAt(31).Should().BeEquivalentTo("FirstExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension at run second end.");
            sequence.ElementAt(32).Should().BeEquivalentTo("SecondExtension: Behaving on Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension at run second end.");
        };
    }
}
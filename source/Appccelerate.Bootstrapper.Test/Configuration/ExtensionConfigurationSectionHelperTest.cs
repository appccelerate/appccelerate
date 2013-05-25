//-------------------------------------------------------------------------------
// <copyright file="ExtensionConfigurationSectionHelperTest.cs" company="Appccelerate">
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

    using FluentAssertions;

    using Xunit;

    public class ExtensionConfigurationSectionHelperTest
    {
        [Fact]
        public void CreateSection_MustBuildUpCorrectConfigurationSection()
        {
            var section = ExtensionConfigurationSectionHelper.CreateSection(new Dictionary<string, string> { { "A", "B" }, { "C", "D" }, });

            section.Configuration["A"].Value.Should().Be("B");
            section.Configuration["C"].Value.Should().Be("D");
        }

        [Fact]
        public void CreateSection_WithParams_MustBuildUpCorrectConfigurationSection()
        {
            var section = ExtensionConfigurationSectionHelper.CreateSection(new KeyValuePair<string, string>("A", "B"));

            section.Configuration["A"].Value.Should().Be("B");
        }
    }
}
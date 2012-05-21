//-------------------------------------------------------------------------------
// <copyright file="EnumerableExtensionMethodsTest.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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

namespace Appccelerate
{
    using System;
    using System.Globalization;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public class EnumerableExtensionMethodsTest
    {
        [Fact]
        public void AggregateInto_WithDefaultSeperator_ShouldAggregate()
        {
            Enumerable.Range(0, 4).AggregateInto().Should().Be("0; 1; 2; 3");
        }

        [Fact]
        public void AggregateInto_WithOwnSeperator_ShouldAggregate()
        {
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;

            string separator = string.Format(invariantCulture, ";{0}", Environment.NewLine);
            string expected = string.Format(invariantCulture, "0{0}1{0}2{0}3", separator);

            Enumerable.Range(0, 4).AggregateInto(separator).Should().Be(expected);
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="AbsoluteFolderPathTest.cs" company="Appccelerate">
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
namespace Appccelerate.IO
{
    using System;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public class AbsoluteFolderPathTest
    {
        private const string Path = @"c:\folder\";

        [Theory]
        [InlineData(@"c:\folder\")]
        [InlineData(@"c:\folder")]
        [InlineData(@"c:\fol.der")]
        public void RepresentsAnAbsoluteFolderPath(string path)
        {
            var absoluteFolderPath = new AbsoluteFolderPath(path);

            absoluteFolderPath.Value.Should().Be(path);
        }

        [Fact]
        public void IsAssignableFromString()
        {
            AbsoluteFolderPath absoluteFolderPath = Path;

            absoluteFolderPath.Value.Should().Be(Path);
        }

        [Fact]
        public void IsAssignableToString()
        {
            AbsoluteFolderPath absolutePath = new AbsoluteFolderPath(Path);

            string path = absolutePath;

            path.Should().Be(Path);
        }

        [Fact]
        public void ThrowsException_WhenPathIsNotAbsolute()
        {
            Action action = () => new AbsolutePath(@"..\folder\");

            action.ShouldThrow<ArgumentException>();
        }

        [Theory]
        [InlineData(@"c:\folder\file.ext", @"c:\folder\file.ext", true)]
        [InlineData(@"c:\folder\file.ext", @"c:\folder\other.ext", false)]
        [InlineData(@"c:\folder\file.ext", @"c:\other\file.ext", false)]
        public void SupportsEqualityOperator(string aa, string bb, bool expected)
        {
            AbsoluteFolderPath a = aa;
            AbsoluteFolderPath b = bb;

            bool result = a == b;

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(@"c:\folder\file.ext", @"c:\folder\file.ext", false)]
        [InlineData(@"c:\folder\file.ext", @"c:\folder\other.ext", true)]
        [InlineData(@"c:\folder\file.ext", @"c:\other\file.ext", true)]
        public void SupportsInequalityOperator(string aa, string bb, bool expected)
        {
            AbsoluteFolderPath a = aa;
            AbsoluteFolderPath b = bb;

            bool result = a != b;

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(@"c:\folder\file.ext", @"c:\folder\file.ext", true)]
        [InlineData(@"c:\folder\file.ext", @"c:\folder\other.ext", false)]
        [InlineData(@"c:\folder\file.ext", @"c:\other\file.ext", false)]
        public void SupportsEquals(string aa, string bb, bool expected)
        {
            AbsoluteFolderPath a = aa;
            AbsoluteFolderPath b = bb;

            bool result = a == b;

            result.Should().Be(expected);
        }
    }
}
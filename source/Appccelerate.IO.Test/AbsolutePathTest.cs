//-------------------------------------------------------------------------------
// <copyright file="AbsolutePathTest.cs" company="Appccelerate">
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

    public class AbsolutePathTest
    {
        private const string Path = @"c:\folder\file.extension";

        [Fact]
        public void RepresentsAnAbsolutePath()
        {
            var absolutePath = new AbsolutePath(Path);

            absolutePath.Value.Should().Be(Path);
        }

        [Fact]
        public void ReturnsAbsoluteFilePath()
        {
            var absolutePath = new AbsolutePath(Path);

            absolutePath.AsAbsoluteFilePath.Value.Should().Be(Path);
        }

        [Fact]
        public void ReturnsAbsoluteFolderPath()
        {
            var absolutePath = new AbsolutePath(Path);

            absolutePath.AsAbsoluteFolderPath.Value.Should().Be(Path);
        }

        [Fact]
        public void IsAssignableFromString()
        {
            AbsolutePath absolutePath = Path;

            absolutePath.Value.Should().Be(Path);
        }

        [Fact]
        public void IsAssignableToString()
        {
            AbsolutePath absolutePath = new AbsolutePath(Path);

            string path = absolutePath;

            path.Should().Be(Path);
        }

        [Fact]
        public void ThrowsException_WhenPathIsNotAbsolute()
        {
            Action action = () => new AbsolutePath("..\folder\file.ext");

            action.ShouldThrow<ArgumentException>();
        }
    }
}
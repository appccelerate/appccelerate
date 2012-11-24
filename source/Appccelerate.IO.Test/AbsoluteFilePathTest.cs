//-------------------------------------------------------------------------------
// <copyright file="AbsoluteFilePathTest.cs" company="Appccelerate">
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
namespace Appccelerate.IO
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class AbsoluteFilePathTest
    {
        private const string Path = @"c:\folder\file.extension";
        private const string FolderPath = @"c:\folder\";

        [Fact]
        public void RepresentsAnAbsoluteFilePath()
        {
            var absoluteFilePath = new AbsoluteFilePath(Path);

            absoluteFilePath.Value.Should().Be(Path);
        }

        [Fact]
        public void IsAssignableFromString()
        {
            AbsoluteFilePath absoluteFilePath = Path;

            absoluteFilePath.Value.Should().Be(Path);
        }

        [Fact]
        public void IsAssignableToString()
        {
            AbsoluteFilePath absolutePath = new AbsoluteFilePath(Path);

            string path = absolutePath;

            path.Should().Be(Path);
        }

        [Fact]
        public void ThrowsException_WhenPathIsAFolder()
        {
            Action action = () => new AbsoluteFilePath(FolderPath);

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ThrowsException_WhenPathIsNotAbsolute()
        {
            Action action = () => new AbsoluteFilePath(@".\file");

            action.ShouldThrow<ArgumentException>();
        }
    }
}
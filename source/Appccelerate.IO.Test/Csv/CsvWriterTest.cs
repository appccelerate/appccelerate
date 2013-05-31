//-------------------------------------------------------------------------------
// <copyright file="CsvWriterTest.cs" company="Appccelerate">
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

namespace Appccelerate.IO.Csv
{
    using System.Globalization;

    using FluentAssertions;

    using Xunit;
    using Xunit.Extensions;

    public class CsvWriterTest
    {
        private readonly CsvWriter testee;

        public CsvWriterTest()
        {
            this.testee = new CsvWriter();
        }

        [Theory]
        [InlineData("simple values", new[] { "1", "2", "hello", "world" }, "1,2,hello,world\r\n")]
        [InlineData("trailing white space", new[] { " 1 ", "\t2 ", "hello\t", "\tworld\t" }, "\" 1 \",\"\t2 \",\"hello\t\",\"\tworld\t\"\r\n")]
        [InlineData("embedded commas", new[] { "hello, world", "The answer is, 42" }, "\"hello, world\",\"The answer is, 42\"\r\n")]
        [InlineData("embedded double quote", new[] { "hello \"world\"", "The answer is \"42\"" }, "\"hello \"world\"\",\"The answer is \"42\"\"\r\n")]
        [InlineData("embedded line break", new[] { "hello\r\nworld", "The answer is\r\n42" }, "\"hello\r\nworld\",\"The answer is\r\n42\"\r\n")]
        public void WritesCsv(string testName, string[] values, string expected)
        {
            var result = this.testee.Write(values, ",");

            result.Should().Be(expected, testName);
        }

        [Fact]
        public void SystemListSeparatorIsUsedAsDefaultSeparator()
        {
            string[] values = new[] { "hello", "world" };
            string expected = "hello" + CultureInfo.CurrentCulture.TextInfo.ListSeparator + "world\r\n";

            var result = this.testee.Write(values);

            result.Should().Be(expected);
        }
    }
}
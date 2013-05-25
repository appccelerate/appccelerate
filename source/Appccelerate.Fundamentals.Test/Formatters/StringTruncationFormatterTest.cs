//-------------------------------------------------------------------------------
// <copyright file="StringTruncationFormatterTest.cs" company="Appccelerate">
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

namespace Appccelerate.Formatters
{
    using System;

    using FluentAssertions;

    using Xunit;

    public class StringTruncationFormatterTest
    {
        [Fact]
        public void WithoutLength()
        {
            string.Format(new StringTruncationFormatter(), "{0,-10}", "12345678")
                .Should().Be("12345678  ");
        }

        [Fact]
        public void WithLengthStringShortEnough()
        {
            string.Format(new StringTruncationFormatter(), "{0,-10:L10}", "12345678")
                .Should().Be("12345678  ");
        }

        [Fact]
        public void WithLengthStringTooLong()
        {
            string.Format(new StringTruncationFormatter(), "{0,-10:L10}", "1234567890123")
                .Should().Be("1234567890");
        }

        [Fact]
        public void MultipleArguments()
        {
            string.Format(new StringTruncationFormatter(), "{0,-10:L10}{1,9:###.##}", "1234567890123", 235.25)
                .Should().Be("1234567890   235.25");
        }

        [Fact]
        public void NotIFormattableArgument()
        {
            string.Format(new StringTruncationFormatter(), "{0:L3}", new TestClassNotFormattable())
                .Should().Be("Tes");
        }

        [Fact]
        public void NotIFormattableArgumentNormalFormat()
        {
            string.Format(new StringTruncationFormatter(), "{0}", new TestClassNotFormattable())
                .Should().Be("Test");
        }

        [Fact]
        public void Formattable()
        {
            string.Format(new StringTruncationFormatter(), "{0:L4}", new TestClassFormattable())
                .Should().Be("form");
        }

        [Fact]
        public void NullValue()
        {
            Assert.Throws<ArgumentNullException>(
                () => string.Format(new StringTruncationFormatter(), "{0:L10}", null));
        }

        public class TestClassNotFormattable
        {
            public override string ToString()
            {
                return "Test";
            }
        }

        public class TestClassFormattable : IFormattable
        {
            public string ToString(string format, IFormatProvider formatProvider)
            {
                return "formatted";
            }
        }
    }
}

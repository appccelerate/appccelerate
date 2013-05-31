//-------------------------------------------------------------------------------
// <copyright file="FormatHelperTest.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Globalization;

    using FluentAssertions;

    using Xunit;

    public class FormatHelperTest
    {
        [Fact]
        public void SecureFormat_ReturnsFormattedString_WhenTooManyParameters()
        {
            const string Format = "{0} {1} {2}";
            const string Correct = "1 1.25 string";
            const int I = 1;
            const decimal D = 1.25m;
            const string S = "string";

            string result = FormatHelper.SecureFormat(CultureInfo.InvariantCulture, Format, I, D, S, I);
            result.Should().Be(Correct, "too much parameters.");
        }
        
        [Fact]
        public void SecureFormat_ReturnsErrorString_WhenTooFewParameters()
        {
            const string Format = "{0} {1} {2}";
            const string Expected = "!!! FORMAT ERROR !!!! " + Format + ": ";
            const int I = 1;
            
            string result = FormatHelper.SecureFormat(CultureInfo.InvariantCulture, Format, I);

            result.Should().Be(Expected + I + ", ", "too few parameters.");
        }

        [Fact]
        public void SecureFormat_ReturnFormattedString_WhenCorrectNumberOfParameters()
        {
            const string Format = "{0} {1} {2}";
            const string Correct = "1 1.25 string";
            const int I = 1;
            const decimal D = 1.25m;
            const string S = "string";

            string result = FormatHelper.SecureFormat(CultureInfo.InvariantCulture, Format, I, D, S);
            result.Should().Be(Correct, "Correct number of parameters.");
        }

        [Fact]
        public void SecureFormat_ReturnsEmptyString_WhenFormatIsNull()
        {
            FormatHelper.SecureFormat(CultureInfo.InvariantCulture, null)
                .Should().Be(string.Empty, "null should result in empty string.");
        }
        
        [Fact]
        public void SecureFormat_ReturnsFormatString_WhenNoFormatParameters()
        {
            FormatHelper.SecureFormat(CultureInfo.InvariantCulture, "format", null)
                .Should().Be("format", "no args should result in format string.");
        }

        [Fact]
        public void ConvertCollectionToString()
        {
            string s = FormatHelper.ConvertToString(new object[] { 3, "hello", new Exception("exception") }, ", ");
            s.Should().Be("3, hello, System.Exception: exception");
        }

        [Fact]
        public void ConvertDictionaryToString()
        {
            IDictionary<object, object> dictionary = new Dictionary<object, object>();
            dictionary.Add("bla", "haha");
            dictionary.Add(1, 25);
            dictionary.Add("exception", new Exception("exception"));

            string s = FormatHelper.ConvertToString(dictionary, "; ");

            s.Should().Be("bla=haha; 1=25; exception=System.Exception: exception");
        }
    }
}

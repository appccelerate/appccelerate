//-------------------------------------------------------------------------------
// <copyright file="CsvParserTest.cs" company="Appccelerate">
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
    using System;

    using FluentAssertions;

    using Xunit;

    public class CsvParserTest
    {
        private readonly CsvParser parser;

        public CsvParserTest()
        {
            this.parser = new CsvParser();
        }

        [Fact]
        public void ParseWithComma()
        {
            const string Line = "\"00501\",\"ABC\",Test \"XYZ,,,,,\"9, Strasse\",\"\",,\"8200\",\"Ort\",\"CH\"";

            string[] values = this.parser.Parse(Line, ',');

            values.Should().ContainInOrder(new[]
                {
                    "00501",
                    "ABC",
                    "Test \"XYZ",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "9, Strasse",
                    string.Empty,
                    string.Empty,
                    "8200",
                    "Ort",
                    "CH"
                });
        }

        [Fact]
        public void ParseWithSemicolon()
        {
            const string Line = "\"00501\";\"ABC\";\"Test XYZ\";\"\";\"\";\"\";\"\";\"9, Strasse\";\"\";\"\";\"8200\";\"Ort\";\"CH\"";

            string[] values = this.parser.Parse(Line, ';');

            values.Should().ContainInOrder(new[]
                {
                    "00501",
                    "ABC",
                    "Test XYZ",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "9, Strasse",
                    string.Empty,
                    string.Empty,
                    "8200",
                    "Ort",
                    "CH"
                });
        }

        [Fact]
        public void ParseDoubleQuote()
        {
            const string Line = "\"00501\"\"ABC\",\"Test XYZ\",\"\"\"\"\"\"\"\",\"9, Strasse\",\"\"\"\",\"8200\"\"Ort\"\"CH\"";

            string[] values = this.parser.Parse(Line);

            values.Should().ContainInOrder(new[]
                {
                    "00501\"ABC",
                    "Test XYZ",
                    "\"\"\"",
                    "9, Strasse",
                    "\"",
                    "8200\"Ort\"CH"
                });
        }

        [Fact]
        public void ParseSpaceAndEmptyString()
        {
            const string Line = "\"00501\", \"ABC\" ,\"Test XYZ\",";

            string[] values = this.parser.Parse(Line);

            values.Should().ContainInOrder(new[]
                {
                    "00501",
                    " \"ABC\" ", 
                    "Test XYZ",
                    string.Empty
                });
        }

        [Fact]
        public void ParseUnvalidValuesNoQuote()
        {
            Action action = () => this.parser.Parse("\"00501, ABC");

            action.ShouldThrow<CsvParseException>();
        }

        [Fact]
        public void ParseUnvalidValuesSpaceAfterQuote()
        {
            Action action = () => this.parser.Parse("\"00501\" , \"ABC\"");

            action.ShouldThrow<CsvParseException>();
        }

        [Fact]
        public void ParseWithoutQuotes()
        {
            const string Line = "personal_id;nachname;vorname;";

            string[] values = this.parser.Parse(Line, ';');

            values.Should().ContainInOrder(new[]
                {
                    "personal_id",
                    "nachname",
                    "vorname"
                });
        }
    }
}

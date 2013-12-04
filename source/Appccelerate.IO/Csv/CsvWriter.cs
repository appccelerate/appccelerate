//-------------------------------------------------------------------------------
// <copyright file="CsvWriter.cs" company="Appccelerate">
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
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Writer for csv files.
    /// Takes care of quoting values if needed.
    /// </summary>
    public class CsvWriter
    {
        /// <summary>
        /// List of special characters that need the value to be quoted.
        /// </summary>
        private readonly Regex specialChars = new Regex(@"[\t| |\r|\n]", RegexOptions.Compiled);

        /// <summary>
        /// Returns a line containing the specified values formatted as csv.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>A line containing the specified values formatted as csv.</returns>
        public string Write(string[] values)
        {
            return this.Write(values, CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Returns a line containing the specified values formatted as csv.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="delimiter">The delimiter used to separate the values.</param>
        /// <returns>
        /// A line containing the specified values formatted as csv.
        /// </returns>
        public string Write(string[] values, string delimiter)
        {
            Ensure.ArgumentNotNull(values, "values");

            StringBuilder line = new StringBuilder();
            foreach (string value in values)
            {
                this.WriteValue(line, value);
                line.Append(delimiter);
            }

            if (line.Length > 0)
            {
                line.Length -= 1; // cut away last delimiter
            }

            line.Append(Environment.NewLine);

            return line.ToString();
        }

        /// <summary>
        /// Writes a value to a line. Encloses the value into " " if necessary.
        /// </summary>
        /// <param name="line">The line to write the value to.</param>
        /// <param name="value">The value.</param>
        private void WriteValue(StringBuilder line, string value)
        {
            if (this.specialChars.Match(value).Success)
            {
                line.Append("\"");
                line.Append(value);
                line.Append("\"");
            }
            else
            {
                line.Append(value);
            }
        }
    }
}
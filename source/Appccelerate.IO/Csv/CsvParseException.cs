//-------------------------------------------------------------------------------
// <copyright file="CsvParseException.cs" company="Appccelerate">
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

    /// <summary>
    /// Exception thrown when a parse exception occurs within the <see cref="CsvParser"/>. 
    /// </summary>
    public class CsvParseException : Exception
    {
        /// <summary>
        /// Where the exception occurred.
        /// </summary>
        private readonly int position;

        /// <summary>
        /// The line that was parsed.
        /// </summary>
        private readonly string line;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        /// <param name="line">The line that was parsed.</param>
        /// <param name="position">The position on which the exception occurred.</param>
        /// <param name="message">The exception message.</param>
        public CsvParseException(string line, int position, string message)
            : base(message + string.Format(CultureInfo.InvariantCulture, " Line: '{0}' Position: '{1}'", line, position))
        {
            this.line = line;
            this.position = position;
        }

        /// <summary>
        /// Gets the line that was parsed.
        /// </summary>
        /// <value>The line parsed.</value>
        public string Line
        {
            get { return this.line; }
        }

        /// <summary>
        /// Gets the position where the exception occurred.
        /// </summary>
        /// <value>The position.</value>
        public int Position
        {
            get { return this.position; }
        }
    }
}

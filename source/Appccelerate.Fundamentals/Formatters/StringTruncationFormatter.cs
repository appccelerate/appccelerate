//-------------------------------------------------------------------------------
// <copyright file="StringTruncationFormatter.cs" company="Appccelerate">
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
    using System.Globalization;

    /// <summary>
    /// Allows to define the maximal length of an string. A string longer than the given value will be truncated.
    /// Use: Format("{0,-5:L5}", "123456")         -> "12345"
    /// Use: Format("{0,5:L5}", "123")             -> "  123"
    /// Use: Format("{0,-5:L10}", "1234567890123") -> "1234567890"
    /// </summary>
    public class StringTruncationFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// String.Format calls this method to get an instance of an ICustomFormatter to handle the formatting.
        /// </summary>
        /// <param name="formatType">The requested formatType.</param>
        /// <returns>An ICustomFormatter.</returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        /// <summary>
        /// After String.Format gets the ICustomFormatter, it calls this format method on each argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg">The arguments for the format string.</param>
        /// <param name="formatProvider">The formatProvider.</param>
        /// <returns>Formatted string.</returns>
        public virtual string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null || !format.StartsWith("L", StringComparison.Ordinal))
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}", arg);
            }

            string s;
            var formattable = arg as IFormattable;
            if (formattable != null)
            {
                s = formattable.ToString(format, formatProvider);
            }
            else if (arg != null)
            {
                s = arg.ToString();
            }
            else
            {
                return null;
            }

            // Uses the format string to
            // form the output string.
            int length = Convert.ToInt32(format.Substring(1), formatProvider);
            if (s.Length > length)
            {
                s = s.Substring(0, length);
            }

            return s;
        }
    }
}

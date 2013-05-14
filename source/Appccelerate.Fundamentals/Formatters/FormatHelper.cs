//-------------------------------------------------------------------------------
// <copyright file="FormatHelper.cs" company="Appccelerate">
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

namespace Appccelerate.Formatters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provides functionality for formatting strings.
    /// </summary>
    public static class FormatHelper
    {
        /// <summary>
        /// Replacement for the String.Format method, that throws an exception
        /// when the count of arguments does not match the count of placeholders.
        /// <para>
        /// If format and/or arguments are null then still a string is returned.
        /// </para>
        /// </summary>
        /// <param name="formatProvider">
        /// The format Provider.
        /// </param>
        /// <param name="format">
        /// The format string.
        /// </param>
        /// <param name="args">
        /// The arguments to the format string..
        /// </param>
        /// <returns>
        /// Returns the fully formatted string, if successful In case of
        /// an error, the format string and all parameters added in a list
        /// will be returned.
        /// </returns>
        /// <remarks>
        /// Tries to format with String.Format. In case of an Exception the
        /// original format string and all parameters added in a list will
        /// be returned.
        /// </remarks>
        public static string SecureFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (format == null) 
            {
                // no format string
                return string.Empty;
            }

            // no arguments, just use format
            if (args == null) 
            {
                return format;
            }
            
            try
            {
                return string.Format(formatProvider, format, args);
            }
            catch (FormatException)
            {
                string result = "!!! FORMAT ERROR !!!! " + format + ": ";

                return args.Aggregate(result, (current, arg) => current + (arg + ", "));
            }
        }

        /// <summary>
        /// Converts a collection of objects to a string representation.
        /// </summary>
        /// <param name="collection">Collection of objects.</param>
        /// <param name="separator">Separator to separate objects.</param>
        /// <returns>String representation of the collection.</returns>
        public static string ConvertToString(IEnumerable collection, string separator)
        {
            Ensure.ArgumentNotNull(separator, "seperator");

            StringBuilder sb = new StringBuilder();
            if (collection != null)
            {
                foreach (object o in collection)
                {
                    sb.Append(o);
                    sb.Append(separator);
                }

                // cut away last separator
                if (sb.Length > 0)
                {
                    sb.Length -= separator.Length;
                }
                else
                {
                    sb.Append("none");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a dictionary of object, object to a string representation in the form of Key=Value<paramref name="separator"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="separator">The separator to separate key value pairs.</param>
        /// <returns>String representation of the dictionary</returns>
        public static string ConvertToString(IDictionary<object, object> dictionary, string separator)
        {
            Ensure.ArgumentNotNull(separator, "seperator");

            StringBuilder sb = new StringBuilder();
            if (dictionary != null)
            {
                foreach (KeyValuePair<object, object> valuePair in dictionary)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}{2}", valuePair.Key, valuePair.Value, separator);
                }

                // cut away last separator
                if (sb.Length > 0)
                {
                    sb.Length -= separator.Length;
                }
                else
                {
                    sb.Append("none");
                }
            }

            return sb.ToString();
        }
    }
}

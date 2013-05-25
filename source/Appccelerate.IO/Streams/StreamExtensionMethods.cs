//-------------------------------------------------------------------------------
// <copyright file="StreamExtensionMethods.cs" company="Appccelerate">
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

namespace Appccelerate.IO.Streams
{
    using System;
    using System.IO;

    /// <summary>
    /// Class to help with stream handling that is not covered by .NET.
    /// </summary>
    public static class StreamExtensionMethods
    {
        /// <summary>
        /// Copies the input stream to the output stream.
        /// </summary>
        /// <param name="input">The input stream.</param><param name="output">The output stream.</param><exception cref="ArgumentNullException"><paramref name="input" /> or <paramref name="output" /> are null.
        /// </exception><exception cref="ArgumentException"><paramref name="input" />is not readable or <paramref name="output" /> is
        /// not writable.</exception>
        public static void CopyTo(this Stream input, Stream output)
        {
            // assert these are the right kind of streams
            if (input == null)
            {
                throw new ArgumentNullException("input", "Input stream was null");
            }

            if (output == null)
            {
                throw new ArgumentNullException("output", "Output stream was null");
            }

            if (!input.CanRead)
            {
                throw new ArgumentException("Input stream must support CanRead");
            }

            if (!output.CanWrite)
            {
                throw new ArgumentException("Output stream must support CanWrite");
            }

            // skip if the input stream is empty (if seeking is supported)
            if (input.CanSeek)
            {
                if (input.Length == 0)
                {
                    return;
                }
            }

            // copy it
            const int Size = 4096;
            byte[] bytes = new byte[Size];
            int numBytes;
            while ((numBytes = input.Read(bytes, 0, Size)) > 0)
            {
                output.Write(bytes, 0, numBytes);
            }
        }

        /// <summary>
        /// Compares the contents of the streams given.
        /// </summary>
        /// <param name="actual">
        /// The actual.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <returns>
        /// True if the stream contents are equal, else false.
        /// </returns>
        public static bool CompareStreamContentsTo(this Stream actual, Stream expected)
        {
            Ensure.ArgumentNotNull(actual, "actual");
            Ensure.ArgumentNotNull(expected, "expected");

            if (!expected.CanRead)
            {
                throw new ArgumentException("Expected stream is not readable");
            }

            if (!actual.CanRead)
            {
                throw new ArgumentException("Actual stream is not readable");
            }

            int i = 0;
            int j = 0;
            while (i == j && i != -1)
            {
                i = expected.ReadByte();
                j = actual.ReadByte();
            }

            return i == j;
        }
    }
}
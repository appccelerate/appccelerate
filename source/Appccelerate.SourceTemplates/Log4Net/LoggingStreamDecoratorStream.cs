//-------------------------------------------------------------------------------
// <copyright file="LoggingStreamDecoratorStream.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Globalization;
    using System.IO;

    using Appccelerate.IO.Streams;

    using log4net;

    /// <summary>
    /// Logs the data that is written to and read from a stream.
    /// </summary>
    public class LoggingStreamDecoratorStream : StreamDecoratorStream
    {
        /// <summary>
        /// The number of bytes that are written per line in the log.
        /// </summary>
        private readonly int bytesPerLine;

        /// <summary>
        /// The number of bytes that are written as a block. After a block a space is written.
        /// </summary>
        private readonly int blockLength;

        /// <summary>
        /// The logger that is used for writing the log output.
        /// </summary>
        private readonly ILog log;

        public LoggingStreamDecoratorStream(Stream stream, ILog logger, int bytesPerLine, int blockLength)
            : base(stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.log = logger;
            this.bytesPerLine = bytesPerLine;
            this.blockLength = blockLength;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = base.Read(buffer, offset, count);
            LogHelper.Debug(this.log, "Received: \r\n", buffer, offset, result, this.bytesPerLine, this.blockLength);
            return result;
        }

        public override int ReadByte()
        {
            int result = base.ReadByte();
            this.log.DebugFormat("Received: {0:X2}", result);
            return result;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            LogHelper.Debug(this.log, "Transmitting: \r\n", buffer, offset, count, this.bytesPerLine, this.blockLength);
            base.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            this.log.DebugFormat("Transmitting: {0:X2}", value);
            base.WriteByte(value);
        }

        #region LogHelper class

        /// <summary>
        /// Helper class for logging data with log4net
        /// </summary>
        private static class LogHelper
        {
            /// <summary>
            /// This delegate is internally used to pass the log method of the <see cref="ILog"/> to the shared
            /// <see cref="LogBuffer"/> method 
            /// </summary>
            /// <param name="message">The message that is logged.</param>
            private delegate void LogMethod(string message);

            /// <summary> 
            /// Logs a byte array for debugging
            /// </summary>
            /// <param name="log">The logger to which the data is logged.</param>
            /// <param name="msg">A message that is displayed before the data.</param>
            /// <param name="buffer">The byte array that is logged</param>
            /// <param name="offset">The index of the first byte that is logged.</param>
            /// <param name="count">The number of bytes that are logged.</param>
            /// <param name="lineLength">Number of bytes that are displayedPerLine</param>
            /// <param name="blockLength">Defines how many bytes are in a block. A space is inserted after each block
            /// </param>
            public static void Debug(
                ILog log,
                string msg,
                byte[] buffer,
                long offset,
                long count,
                int lineLength,
                int blockLength)
            {
                if (log.IsDebugEnabled)
                {
                    LogBuffer(log.Debug, msg, buffer, offset, count, lineLength, blockLength);
                }
            }

            /// <summary>
            /// Logs a byte array
            /// </summary>
            /// <param name="logMethod">The method that is used for logging.</param>
            /// <param name="msg">A message that is displayed before the data.</param>
            /// <param name="buffer">The byte array that is logged</param>
            /// <param name="offset">The index of the first byte that is logged.</param>
            /// <param name="count">The number of bytes that are logged.</param>
            /// <param name="lineLength">Number of bytes that are displayedPerLine</param>
            /// <param name="blockLength">Defines how many bytes are in a block. A space is inserted after each block
            /// </param>
            private static void LogBuffer(
                LogMethod logMethod,
                string msg,
                byte[] buffer,
                long offset,
                long count,
                int lineLength,
                int blockLength)
            {
                // Create the string builder and add the message 
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append(msg);

                // For all bytes to be logged
                int linePos = 0;
                for (int i = 0; i < count; i++, linePos++)
                {
                    // Check if a new line needs to be added.
                    if (i % lineLength == 0)
                    {
                        if (i != 0)
                        {
                            stringBuilder.Append("\r\n");
                            linePos = 0;
                        }
                    }
                    else
                    {
                        // Check if a block separation character needs to be added
                        if (linePos % blockLength == 0)
                        {
                            stringBuilder.Append(" ");
                        }
                    }

                    // Add the character to the string builder
                    stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "[{0:X2}]", buffer[offset + i]));
                }

                // Use the logMethod to log the complete message.
                logMethod(stringBuilder.ToString());
            }
        }

        #endregion
    }
}
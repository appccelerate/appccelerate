//-------------------------------------------------------------------------------
// <copyright file="LoggingStreamDecoratorStream.cs" company="Appccelerate">
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

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingStreamDecoratorStream"/> class.
        /// </summary>
        /// <param name="stream">The stream that is logged.</param>
        /// <param name="logger">The logger that is used for logging.</param>
        /// <param name="bytesPerLine">The number of bytes that are written per line in the log.</param>
        /// <param name="blockLength">The number of bytes that are written as a block. After a block a
        /// space is written.</param>
        /// <exception cref="ArgumentNullException">stream or logger is null.</exception>
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

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the
        /// position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte
        /// array with the values between offset and (offset + count - 1) replaced by the bytes read from the current
        /// source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the
        /// current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that
        /// many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length.
        /// </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="ArgumentNullException">buffer is null. </exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = base.Read(buffer, offset, count);
            LogHelper.Debug(this.log, "Received: \r\n", buffer, offset, result, this.bytesPerLine, this.blockLength);
            return result;
        }

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at 
        /// the end of the stream.
        /// </summary>
        /// <returns>
        /// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
        /// </returns>
        /// <exception cref="NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. 
        /// </exception>
        public override int ReadByte()
        {
            int result = base.ReadByte();
            this.log.DebugFormat("Received: {0:X2}", result);
            return result;
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the 
        /// current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.
        /// </param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current 
        /// stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">The stream does not support writing. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. 
        /// </exception>
        /// <exception cref="ArgumentNullException">buffer is null. </exception>
        /// <exception cref="ArgumentException">The sum of offset and count is greater than the buffer length.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            LogHelper.Debug(this.log, "Transmitting: \r\n", buffer, offset, count, this.bytesPerLine, this.blockLength);
            base.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte.
        /// </summary>
        /// <param name="value">The byte to write to the stream.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="NotSupportedException">The stream does not support writing, or the stream is already 
        /// closed. </exception>
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
//-------------------------------------------------------------------------------
// <copyright file="StreamDecoratorStream.cs" company="Appccelerate">
//   Copyright (c) 2008-2013
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
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
    /// Abstract decorator class for a Stream
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is used to implement stream decorator classes.
    /// For creating a new stream decorator derive a new class from this one and override
    /// the methods that you want to decorate. All methods that are not overwritten
    /// are passed to the decorated stream.
    /// </para>
    /// <para>
    /// All methods throw a NoStreamException when no stream is assigned to this class
    /// at the time a method is called.
    /// </para>
    /// </remarks>
    public abstract class StreamDecoratorStream : Stream
    {
        /// <summary>
        /// The decorated stream
        /// </summary>
        private Stream decoratedStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDecoratorStream"/> class.
        /// </summary>
        /// <param name="decoratedStream">The decorated stream.</param>
        protected StreamDecoratorStream(Stream decoratedStream)
        {
            this.SetStream(decoratedStream);
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports reading; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override bool CanRead
        {
            get
            {
                this.AssertStreamNotNull();
                return this.decoratedStream.CanRead;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports seeking; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override bool CanSeek
        {
            get
            {
                this.AssertStreamNotNull();
                return this.decoratedStream.CanSeek;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override bool CanWrite
        {
            get
            {
                this.AssertStreamNotNull();
                return this.decoratedStream.CanWrite;
            }
        }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        /// <value></value>
        /// <returns>A long value representing the length of the stream in bytes.</returns>
        /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
        /// <exception cref="ObjectDisposedException">Property was called after the stream was closed.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override long Length
        {
            get
            {
                this.AssertStreamNotNull();
                return this.decoratedStream.Length;
            }
        }

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        /// <value></value>
        /// <returns>The current position within the stream.</returns>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="NotSupportedException">The stream does not support seeking. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override long Position
        {
            get
            {
                this.AssertStreamNotNull();
                return this.decoratedStream.Position;
            }

            set
            {
                this.AssertStreamNotNull();
                this.decoratedStream.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines how long the stream will attempt to read before timing out.
        /// </summary>
        /// <value></value>
        /// <returns>A value that determines how long the stream will attempt to read before timing out.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override int ReadTimeout
        {
            get
            {
                this.AssertStreamNotNull();
                return this.decoratedStream.ReadTimeout;
            }

            set
            {
                this.AssertStreamNotNull();
                this.decoratedStream.ReadTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines how long the stream will attempt to write before timing out.
        /// </summary>
        /// <value></value>
        /// <returns>A value that determines how long the stream will attempt to write before timing out.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override int WriteTimeout
        {
            get
            {
                this.AssertStreamNotNull();
                return this.decoratedStream.WriteTimeout;
            }

            set
            {
                this.AssertStreamNotNull();
                this.decoratedStream.WriteTimeout = value;
            }
        }

        /// <summary>
        /// Begins an asynchronous read operation.
        /// </summary>
        /// <param name="buffer">The buffer to read the data into.</param>
        /// <param name="offset">The byte offset in buffer at which to begin writing data read from the stream.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="callback">An optional asynchronous callback, to be called when the read is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request 
        /// from other requests.</param>
        /// <returns>
        /// An <see cref="IAsyncResult"></see> that represents the asynchronous read, which could still be 
        /// pending.
        /// </returns>
        /// <exception cref="IOException">Attempted an asynchronous read past the end of the stream, or a disk error 
        /// occurs. </exception>
        /// <exception cref="NotSupportedException">The current Stream implementation does not support the read 
        /// operation. </exception>
        /// <exception cref="ArgumentException">One or more of the arguments is invalid. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override IAsyncResult BeginRead(
            byte[] buffer,
            int offset,
            int count,
            AsyncCallback callback,
            object state)
        {
            this.AssertStreamNotNull();
            return this.decoratedStream.BeginRead(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Waits for the pending asynchronous read to complete.
        /// </summary>
        /// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
        /// <returns>
        /// The number of bytes read from the stream, between zero (0) and the number of bytes you requested. 
        /// Streams return zero (0) only at the end of the stream, otherwise, they should block until at least one 
        /// byte is available.
        /// </returns>
        /// <exception cref="ArgumentException">asyncResult did not originate from a 
        /// <see cref="BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)">
        /// </see> method on the current stream.</exception>
        /// <exception cref="ArgumentNullException">asyncResult is null. </exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override int EndRead(IAsyncResult asyncResult)
        {
            this.AssertStreamNotNull();
            return this.decoratedStream.EndRead(asyncResult);
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the 
        /// position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte 
        /// array with the values between offset and (offset + count - 1) replaced by the bytes read from the current 
        /// source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from 
        /// the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if 
        /// that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length.
        /// </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="ArgumentNullException">buffer is null. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count is negative. </exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            this.AssertStreamNotNull();
            return this.decoratedStream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            this.AssertStreamNotNull();
            return this.decoratedStream.ReadByte();
        }

        /// <summary>
        /// Begins an asynchronous write operation.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">The byte offset in buffer from which to begin writing.</param>
        /// <param name="count">The maximum number of bytes to write.</param>
        /// <param name="callback">An optional asynchronous callback, to be called when the write is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular asynchronous write request 
        /// from other requests.</param>
        /// <returns> An IAsyncResult that represents the asynchronous write, which could still be pending.</returns>
        /// <exception cref="NotSupportedException">The current Stream implementation does not support the write 
        /// operation. </exception>
        /// <exception cref="IOException">Attempted an asynchronous write past the end of the stream, 
        /// or a disk error occurs. </exception>
        /// <exception cref="ArgumentException">One or more of the arguments is invalid. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override IAsyncResult BeginWrite(
            byte[] buffer,
            int offset,
            int count,
            AsyncCallback callback,
            object state)
        {
            this.AssertStreamNotNull();
            return this.decoratedStream.BeginWrite(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Ends an asynchronous write operation.
        /// </summary>
        /// <param name="asyncResult">A reference to the outstanding asynchronous I/O request.</param>
        /// <exception cref="T:System.ArgumentNullException">asyncResult is null. </exception>
        /// <exception cref="T:System.ArgumentException">asyncResult did not originate from a 
        /// <see cref="BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)"/>
        /// method on the current stream. </exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            this.AssertStreamNotNull();
            this.decoratedStream.EndWrite(asyncResult);
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
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <exception cref="ArgumentNullException">buffer is null. </exception>
        /// <exception cref="ArgumentException">The sum of offset and count is greater than the buffer length.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count is negative. </exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.AssertStreamNotNull();
            this.decoratedStream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte.
        /// </summary>
        /// <param name="value">The byte to write to the stream.</param>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <exception cref="NotSupportedException">The stream does not support writing, or the stream is already 
        /// closed. </exception>
        public override void WriteByte(byte value)
        {
            this.AssertStreamNotNull();
            this.decoratedStream.WriteByte(value);
        }

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with 
        /// the current stream.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override void Close()
        {
            this.AssertStreamNotNull();
            this.decoratedStream.Close();
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to 
        /// be written to the underlying device.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override void Flush()
        {
            this.AssertStreamNotNull();
            this.decoratedStream.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference 
        /// point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream 
        /// is constructed from a pipe or console output. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. 
        /// </exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            this.AssertStreamNotNull();
            return this.decoratedStream.Seek(offset, origin);
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="NotSupportedException">The stream does not support both writing and seeking, 
        /// such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override void SetLength(long value)
        {
            this.AssertStreamNotNull();
            this.decoratedStream.SetLength(value);
        }

        /// <summary>
        /// Returns a <see cref="String"></see> that represents the current <see cref="Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"></see> that represents the current <see cref="Object"></see>.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown when no stream is assigned as decorated stream.
        /// </exception>
        public override string ToString()
        {
            this.AssertStreamNotNull();
            return this.decoratedStream.ToString();
        }

        /// <summary>
        /// Derived classes can override this method to handle cases where a method is called while the decorated 
        /// device is not assigned.
        /// </summary>
        protected virtual void ThrowNoStreamException()
        {
            throw new InvalidOperationException("The decorated stream must not be null.");
        }

        /// <summary>
        /// Sets the stream that shall be decorated.
        /// </summary>
        /// <param name="stream">The stream that shall be decorated.</param>
        protected void SetStream(Stream stream)
        {
            this.decoratedStream = stream;
        }

        /// <summary>
        /// Asserts the stream is not null.
        /// </summary>
        private void AssertStreamNotNull()
        {
            if (this.decoratedStream == null)
            {
                this.ThrowNoStreamException();
            }
        }
    }
}

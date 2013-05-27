//-------------------------------------------------------------------------------
// <copyright file="StreamDecoratorStreamPropertiesTest.cs" company="Appccelerate">
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
    
    using FluentAssertions;

    using Xunit;
    using Xunit.Extensions;

    public class StreamDecoratorStreamPropertiesTest
    {
        private const int Timeout = 123;

        private readonly StreamDecoratorTestStream streamDecorator;

        private readonly TestStream testStream;

        public StreamDecoratorStreamPropertiesTest()
        {
            this.testStream = new TestStream();
            this.streamDecorator = new StreamDecoratorTestStream(this.testStream);
        }

        [Fact]
        public void GetsReadTimeoutFromDecoratedStream()
        {
            this.testStream.ReadTimeout = Timeout;
            
            var timeout = this.streamDecorator.ReadTimeout;
            
            timeout.Should().Be(Timeout);
        }

        [Fact]
        public void SetsReadTimeoutGetInformationOnDecoratedStream()
        {
            this.streamDecorator.ReadTimeout = Timeout;

            this.testStream.ReadTimeout.Should().Be(Timeout);
        }

        [Fact]
        public void GetsWriteTimeoutFromDecoratedStream()
        {
            this.testStream.WriteTimeout = Timeout;
            
            var timeout = this.streamDecorator.WriteTimeout;
                
            timeout.Should().Be(Timeout);
        }
        
        [Fact]
        public void SetsWriteTimeoutOnDecoratedStream()
        {
            this.streamDecorator.WriteTimeout = Timeout;
            
            this.testStream.WriteTimeout.Should().Be(Timeout);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetsCanSeekFromDecoratedStream(bool expected)
        {
            this.testStream.SetCanSeek(expected);

            var canSeek = this.streamDecorator.CanSeek;

            canSeek.Should().Be(expected);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetsCanReadFromDecoratedStream(bool expected)
        {
            this.testStream.SetCanRead(expected);

            var canRead = this.streamDecorator.CanRead;

            canRead.Should().Be(expected);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetsCanWriteFromDecoratedStream(bool expected)
        {
            this.testStream.SetCanWrite(expected);

            var canWrite = this.streamDecorator.CanWrite;

            canWrite.Should().Be(expected);
        }

        [Fact]
        public void ReturnsToStringFromDecoratedStream()
        {
            var tostring = this.streamDecorator.ToString();
                
            tostring.Should().Be(this.testStream.ToString());
        }

        [Fact]
        public void SetsLengthOnDecoratedStream()
        {
            const int Length = 100;

            this.streamDecorator.SetLength(Length);

            this.testStream.Length.Should().Be(Length);
        }

        [Fact]
        public void ForwardsSeekToDecoratedStream()
        {
            this.streamDecorator.Seek(999, SeekOrigin.End);

            this.testStream.SeekWasCalled.Should().BeTrue();
            this.testStream.SeekOffset.Should().Be(999);
            this.testStream.SeekOrigin.Should().Be(SeekOrigin.End);
        }

        [Fact]
        public void ForwardsFlushToDecoratedStream()
        {
            this.streamDecorator.Flush();
            
            this.testStream.FlushWasCalled.Should().BeTrue();
        }

        [Fact]
        public void ForwardsCloseToDecoratedStream()
        {
            this.streamDecorator.Close();

            this.testStream.CloseWasCalled.Should().BeTrue();
        }

        private class TestStream : Stream
        {
            private bool canRead;
            private bool canSeek;
            private bool canWrite;
            private long length;

            public override bool CanRead 
            { 
                get
                {
                    return this.canRead;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return this.canSeek;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return this.canWrite;
                }
            }

            public override long Length
            {
                get
                {
                    return this.length;
                }
            }

            public override int ReadTimeout { get; set; }

            public override int WriteTimeout { get; set; }

            public override bool CanTimeout
            {
                get
                {
                    return true;
                }
            }

            public override long Position { get; set; }

            public bool SeekWasCalled { get; private set; }

            public long SeekOffset { get; private set; }

            public SeekOrigin SeekOrigin { get; private set; }

            public bool FlushWasCalled { get; private set; }

            public bool CloseWasCalled { get; private set; }

            public void SetCanRead(bool value)
            {
                this.canRead = value;
            }

            public void SetCanSeek(bool value)
            {
                this.canSeek = value;
            }

            public void SetCanWrite(bool value)
            {
                this.canWrite = value;
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override void Flush()
            {
                this.FlushWasCalled = true;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                this.SeekWasCalled = true;
                this.SeekOffset = offset;
                this.SeekOrigin = origin;
                return 1010;
            }

            public override void Close()
            {
                this.CloseWasCalled = true;
            }

            public override void SetLength(long value)
            {
                this.length = value;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return "TestOutput";
            }
        }
    }
}
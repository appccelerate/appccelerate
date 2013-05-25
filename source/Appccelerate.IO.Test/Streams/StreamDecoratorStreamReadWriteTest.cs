//-------------------------------------------------------------------------------
// <copyright file="StreamDecoratorStreamReadWriteTest.cs" company="Appccelerate">
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

    public class StreamDecoratorStreamReadWriteTest
    {
        private static readonly Random Random = new Random(15865);

        private readonly StreamDecoratorTestStream streamDecorator;

        private readonly MemoryStream memoryStream;

        public StreamDecoratorStreamReadWriteTest()
        {
            this.memoryStream = new MemoryStream();
            this.streamDecorator = new StreamDecoratorTestStream(this.memoryStream);
        }

        [Fact]
        public void ForwardsDataToDecoratedStream()
        {
            byte[] data = CreateByteArray(6);
            
            this.streamDecorator.Write(data, 1, 4);

            this.memoryStream.ToArray().Should().Be(data, 1, 4);
        }

        [Fact]
        public void ForwardsByteDataToDecoratedStream()
        {
            byte[] data = CreateByteArray(6);
            
            for (int i = 1; i < 5; i++)
            {
                this.streamDecorator.WriteByte(data[i]);
            }

            this.memoryStream.ToArray().Should().Be(data, 1, 4);
        }

        [Fact]
        public void ForwardsDataToDecoratedStream_WhenAsynchronous()
        {
            byte[] data = CreateByteArray(6);

            IAsyncResult asyncResult = this.streamDecorator.BeginWrite(data, 1, 4, null, null);
            this.streamDecorator.EndWrite(asyncResult);

            this.memoryStream.ToArray().Should().Be(data, 1, 4);
        }

        [Fact]
        public void ReadsDataFromDecoratedStream()
        {
            byte[] data = CreateByteArray(6);

            this.memoryStream.Write(data, 0, 6);
            
            this.streamDecorator.Position = 0;
            byte[] readData = new byte[8];
            this.streamDecorator.Read(readData, 1, 6);

            data.Should().Be(readData, 1, 6);
        }
        
        [Fact]
        public void ReadsByteDataFromDecoratedStream()
        {
            byte[] data = CreateByteArray(6);
            this.memoryStream.Write(data, 0, 6);
            this.streamDecorator.Position = 0;
            
            byte[] readData = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                readData[i] = (byte)this.streamDecorator.ReadByte();
            }

            readData.Should().Be(data, 0, 6);
        }

        [Fact]
        public void ReadsDataFromDecoratedStream_WhenAsynchronous()
        {
            byte[] data = CreateByteArray(6);
            this.memoryStream.Write(data, 0, 6);
            this.memoryStream.Position = 0;

            byte[] readData = new byte[8];
            IAsyncResult asyncResult = this.streamDecorator.BeginRead(readData, 1, 6, null, null);
            this.streamDecorator.EndRead(asyncResult);
            
            data.Should().Be(readData, 1, 6);
        }

        private static byte[] CreateByteArray(int length)
        {
            byte[] result = new byte[length];
            Random.NextBytes(result);
            return result;
        }
    }
}
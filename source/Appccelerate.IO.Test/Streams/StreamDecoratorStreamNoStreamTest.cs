//-------------------------------------------------------------------------------
// <copyright file="StreamDecoratorStreamNoStreamTest.cs" company="Appccelerate">
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

    using FluentAssertions;

    using Xunit;

    public class StreamDecoratorStreamNoStreamTest
    {
        [Fact]
        public void ThrowsInvalidOperationExceptin_WhenNoStreamIsAssigned()
        {
            var streamDecorator = new StreamDecoratorTestStream(null);
            
            streamDecorator.Invoking(s => s.Position = 10)
                .ShouldThrow<InvalidOperationException>();
        }
    }
}
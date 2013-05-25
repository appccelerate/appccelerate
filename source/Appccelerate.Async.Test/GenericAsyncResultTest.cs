//-------------------------------------------------------------------------------
// <copyright file="GenericAsyncResultTest.cs" company="Appccelerate">
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

namespace Appccelerate.Async
{
    using FluentAssertions;

    using Xunit;

    /// <summary>
    /// Tests the implementation of <see cref="AsyncResult{TResult}"/>
    /// </summary>
    public class GenericAsyncResultTest
    {
        /// <summary>
        /// Tests the result is returned by <see cref="AsyncResult{TResult}.EndInvoke()"/>
        /// </summary>
        [Fact]
        public void ResultReturnedByEndInvoke()
        {
            AsyncResult<bool> asyncResult = new AsyncResult<bool>(null, null);
            
            asyncResult.IsCompleted.Should().BeFalse();
            asyncResult.CompletedSynchronously.Should().BeFalse();
            asyncResult.AsyncState.Should().BeNull();

            asyncResult.SetAsCompleted(true, false);
            
            asyncResult.EndInvoke().Should().BeTrue();
        }
    }
}

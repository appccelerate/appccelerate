//-------------------------------------------------------------------------------
// <copyright file="AsyncResult{TResult}.cs" company="Appccelerate">
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
    using System;    
    
    /// <summary>
    /// Result for asynchronous tasks with a return value.
    /// </summary>    
    /// <typeparam name="TResult">The result type</typeparam>
    public class AsyncResult<TResult> : AsyncResult
    {
        /// <summary>
        /// The result of the asynchronous operation. It is set when the operation is completed.
        /// </summary>
        private TResult resultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncResult{TResult}"/> class.
        /// </summary>
        /// <param name="asyncCallback">The callback function that is called after the task has completed.</param>
        /// <param name="state">A user-defined object that qualifies or contains information about an asynchronous
        /// operation.</param>
        public AsyncResult(AsyncCallback asyncCallback, object state)
            : base(asyncCallback, state)
        {
        }

        /// <summary>
        /// Sets the task to completed with a result.
        /// </summary>
        /// <param name="result">The return value of the task.</param>
        /// <param name="completedSynchronously">true if the task run synchronously.</param>
        public void SetAsCompleted(
            TResult result,
            bool completedSynchronously)
        {
            // Save the asynchronous operation's result
            this.resultValue = result;

            // Tell the base class that the operation completed 
            // successfully (no exception)
            this.SetAsCompleted(null, completedSynchronously);
        }

        /// <summary>
        /// Waits until the task has completed.
        /// </summary>
        /// <returns>The return value of the task.</returns>
        public new TResult EndInvoke()
        {
            base.EndInvoke(); // Wait until operation has completed 
            return this.resultValue;  // Return the result (if above didn't throw)
        }
    }
}

//-------------------------------------------------------------------------------
// <copyright file="IAsyncWorkerExtension.cs" company="Appccelerate">
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
    using System.ComponentModel;

    /// <summary>
    /// Provides extension points for <see cref="AsyncWorker"/>.
    /// </summary>
    public interface IAsyncWorkerExtension
    {
        /// <summary>
        /// Called when an operation is started.
        /// </summary>
        /// <param name="asyncWorker">The asynchronous worker.</param>
        /// <param name="worker">The worker.</param>
        /// <param name="argument">The argument.</param>
        void StartedExecution(AsyncWorker asyncWorker, DoWorkEventHandler worker, object argument);

        /// <summary>
        /// Called when an operation is cancelled.
        /// </summary>
        /// <param name="asyncWorker">The asynchronous worker.</param>
        /// <param name="worker">The worker.</param>
        void CancellingExecution(AsyncWorker asyncWorker, DoWorkEventHandler worker);

        /// <summary>
        /// Called when an operation reports progress.
        /// </summary>
        /// <param name="asyncWorker">The asynchronous worker.</param>
        /// <param name="worker">The worker.</param>
        /// <param name="progress">The progress.</param>
        /// <param name="userState">State of the user.</param>
        void ReportProgress(AsyncWorker asyncWorker, DoWorkEventHandler worker, ProgressChangedEventHandler progress, object userState);

        /// <summary>
        /// Called when an operation was completed.
        /// </summary>
        /// <param name="asyncWorker">The async worker.</param>
        /// <param name="worker">The worker.</param>
        /// <param name="completed">The completed handler.</param>
        /// <param name="runWorkerCompletedEventArgs">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        void CompletedExecution(AsyncWorker asyncWorker, DoWorkEventHandler worker, RunWorkerCompletedEventHandler completed, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs);
    }
}
//-------------------------------------------------------------------------------
// <copyright file="AsyncWorkerTest.cs" company="Appccelerate">
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

namespace Appccelerate.Async
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    using FluentAssertions;

    using Xunit;

    public class AsyncWorkerTest
    {
        private const int TimeOut = 1000;

        /// <summary>Event that signals that an unhandled exception was caught globally.</summary>
        private readonly AutoResetEvent caughtExceptionSignal;

        /// <summary>Exception that was caught on global exception handler.</summary>
        private Exception caughtException;

        public AsyncWorkerTest()
        {
            this.caughtException = null;
            this.caughtExceptionSignal = new AutoResetEvent(false);
        }

        [Fact]
        public void ExecutesAsyncOperation()
        {
            AutoResetEvent go = new AutoResetEvent(false);
            AutoResetEvent workerExecuted = new AutoResetEvent(false);

            DoWorkEventHandler worker = delegate
            {
                go.WaitOne();
                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker);

            testee.RunWorkerAsync();
            go.Set();

            var finished = workerExecuted.WaitOne(TimeOut);
                
            finished.Should().BeTrue("worker should execute.");
        }

        [Fact]
        public void ExecutesAsyncOperationWithArguments()
        {
            AutoResetEvent go = new AutoResetEvent(false);
            AutoResetEvent workerExecuted = new AutoResetEvent(false);

            const string Argument = "test";
            string receivedArgument = null;

            DoWorkEventHandler worker = delegate(object sender, DoWorkEventArgs e)
            {
                receivedArgument = (string)e.Argument;
                go.WaitOne();
                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker);

            testee.RunWorkerAsync(Argument);
            go.Set();

            workerExecuted.WaitOne(TimeOut);

            receivedArgument.Should().Be(Argument);
        }

        [Fact]
        public void ExecutesCompletedHandler()
        {
            AutoResetEvent workerExecuted = new AutoResetEvent(false);

            const int Result = 3;
            Exception receivedException = new Exception();
            int receivedResult = 0;

            DoWorkEventHandler worker = delegate(object sender, DoWorkEventArgs e)
            {
                e.Result = Result;
            };

            RunWorkerCompletedEventHandler completed = delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                receivedException = e.Error;
                receivedResult = (int)e.Result;

                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker, completed);

            testee.RunWorkerAsync();

            workerExecuted.WaitOne(TimeOut);

            receivedException.Should().BeNull();
            receivedResult.Should().Be(Result);
        }

        [Fact(Skip = "Cannot be executed by xUnit console because it'd crash the process.")]
        public void PassesExceptionThrownByWorkerMethodToGlobalExceptionHandler_WhenNoCompletedHandlerIsSpecified()
        {
            AppDomain.CurrentDomain.UnhandledException += this.UnhandledException;

            DoWorkEventHandler worker = delegate
            {
                throw new InvalidOperationException("test");
            };

            AsyncWorker testee = new AsyncWorker(worker);

            testee.RunWorkerAsync();

            var receivedException = this.caughtExceptionSignal.WaitOne(TimeOut);
            
            receivedException.Should().BeTrue("exception should be caught");
            this.caughtException.Should().BeOfType<AsyncWorkerException>();

            AppDomain.CurrentDomain.UnhandledException -= this.UnhandledException;
        }

        [Fact]
        public void PassesExceptionThrownByWorkerMethodToCompletedEventHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += this.UnhandledException;

            AutoResetEvent workerExecuted = new AutoResetEvent(false);
            var exception = new InvalidOperationException("test");
            Exception receivedException = null;

            DoWorkEventHandler worker = delegate
                {
                    throw exception;
                };

            RunWorkerCompletedEventHandler completed = delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                receivedException = e.Error;
                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker, completed);

            testee.RunWorkerAsync();

            workerExecuted.WaitOne(TimeOut);

            this.caughtException.Should().BeNull("no exception should be handled globally.");
            receivedException.Should().BeSameAs(exception);

            AppDomain.CurrentDomain.UnhandledException -= this.UnhandledException;
        }

        [Fact]
        public void CanCancelOperation()
        {
            AutoResetEvent workerStarted = new AutoResetEvent(false);
            AutoResetEvent workerExecuted = new AutoResetEvent(false);
            AutoResetEvent workerCancelled = new AutoResetEvent(false);
            AutoResetEvent allowTerminating = new AutoResetEvent(false);

            bool? cancelled = null;

            DoWorkEventHandler worker = delegate(object sender, DoWorkEventArgs e)
            {
                AsyncWorker genericWorker = (AsyncWorker)sender;
                genericWorker.WorkerSupportsCancellation = true;

                workerStarted.Set();
                while (!genericWorker.CancellationPending)
                {
                    Thread.Sleep(1);
                }

                e.Cancel = true;
                workerCancelled.Set();
                allowTerminating.WaitOne();
            };

            RunWorkerCompletedEventHandler completed = delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                cancelled = e.Cancelled;
                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker, completed);

            testee.RunWorkerAsync();
            workerStarted.WaitOne(TimeOut).Should().BeTrue("worker should start.");

            testee.CancelAsync();
            workerCancelled.WaitOne(TimeOut).Should().BeTrue("worker should cancel.");

            allowTerminating.Set();
            workerExecuted.WaitOne(TimeOut).Should().BeTrue("worker should execute.");

            cancelled.Should().BeTrue("result should reflect canceled state.");
        }

        [Fact]
        public void CanNotifyAboutProgress()
        {
            AutoResetEvent workerExecuted = new AutoResetEvent(false);

            DoWorkEventHandler worker = delegate(object sender, DoWorkEventArgs e)
            {
                AsyncWorker genericWorker = (AsyncWorker)sender;
                genericWorker.WorkerReportsProgress = true;

                for (int i = 0; i <= 100; i += 10)
                {
                    genericWorker.ReportProgress(i, null);
                }
            };

            int count = 0;
            ProgressChangedEventHandler progress = (sender, e) =>
            {
                count++;
                if (count == 10)
                {
                    workerExecuted.Set();
                }
            };

            AsyncWorker testee = new AsyncWorker(worker, progress, null);

            testee.RunWorkerAsync();

            workerExecuted.WaitOne(TimeOut).Should().BeTrue();
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.caughtException = (Exception)e.ExceptionObject;
            this.caughtExceptionSignal.Set();
        }
    }
}
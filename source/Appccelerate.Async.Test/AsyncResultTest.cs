//-------------------------------------------------------------------------------
// <copyright file="AsyncResultTest.cs" company="Appccelerate">
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
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Lifetime;
    using System.Threading;

    using FluentAssertions;

    using Xunit;

    public class AsyncResultTest
    {
        /// <summary>
        /// Defines the constant value that is used to initialize the <see cref="AsyncResult"/>s
        /// sponsorshipTimeout.
        /// </summary>
        private readonly TimeSpan sponsorshipTimeout = TimeSpan.FromMilliseconds(5);

        /// <summary>
        /// Defines the constant value that is used to initialize the <see cref="AsyncResult"/>s
        /// initialLeaseTime.
        /// </summary>
        private readonly TimeSpan initialLeaseTime = TimeSpan.FromMilliseconds(10);
        
        /// <summary>
        /// Object under test
        /// </summary>
        private AsyncResult asyncResult;

        /// <summary>
        /// The asynchronous state that was passed in the callback function.
        /// </summary>
        private int callback;

        /// <summary>
        /// Delegate to invoke <see cref="AsyncResultComplete"/> asynchronously.
        /// </summary>
        private delegate void AsyncResultCompletionInvoker();

        /// <summary>
        /// Tests if synchronous completion works correctly.
        /// </summary>
        [Fact]
        public void SynchronousCompletion()
        {
            // Create AysncResult and complete it synchronously
            this.SetUp(null, null);
            this.asyncResult.SetAsCompleted(null, true);
            this.asyncResult.EndInvoke();

            // Verify that it was completed synchronously
            this.VerifyAsyncResultCompleted(false, true);
        }

        /// <summary>
        /// Tests if custom data can be assigned and read.
        /// </summary>
        [Fact]
        public void SetAndGetCustomData()
        {
            this.SetUp(null, null);
            this.asyncResult.Data["TestData"] = 0;
            this.asyncResult.SetAsCompleted(null, true);
            this.asyncResult.EndInvoke();
            this.asyncResult.Data["TestData"].As<int>().Should().Be(0, "custom data has changed.");
        }

        /// <summary>
        /// Tests if SetAsCompleted can only be executed once.
        /// </summary>
        [Fact]
        public void SetAsCompletedMultipleCallException()
        {
            this.SetUp(null, null);
            this.asyncResult.SetAsCompleted(null, true);

            this.asyncResult.Invoking(a => a.SetAsCompleted(null, true))
                .ShouldThrow<InvalidOperationException>();
        }

        /// <summary>
        /// Tests if EndInvoke can only be executed once.
        /// </summary>
        [Fact]
        public void EndInvokeMultipleCallException()
        {
            this.SetUp(null, null);
            this.asyncResult.SetAsCompleted(null, true);
            this.asyncResult.EndInvoke();

            this.asyncResult.Invoking(a => this.asyncResult.EndInvoke())
                .ShouldThrow<InvalidOperationException>();
        }

        /// <summary>
        /// Tests if asynchronous completion works correctly without callback and state set.
        /// </summary>
        [Fact]
        public void AsynchronousCompletionWithoutCallbackAndState()
        {
            this.SetUp(null, null);
            new AsyncResultCompletionInvoker(this.AsyncResultComplete).BeginInvoke(null, null);
            this.asyncResult.EndInvoke();
            this.VerifyAsyncResultCompleted(true, true);
        }

        /// <summary>
        /// Tests that the stack trace of the exception is kept when it is re thrown in the EndInvoke method.
        /// </summary>
        [Fact]
        public void ExceptionStackTracePreserved()
        {
            this.SetUp(null, null);
            new AsyncResultCompletionInvoker(this.AsyncResultException).BeginInvoke(null, null);

            this.asyncResult.Invoking(a => a.EndInvoke())
                .ShouldThrow<TimeoutException>()
                .Where(
                    exception => exception.StackTrace.StartsWith("   at Appccelerate.Async.AsyncResultTest.AsyncResultException()"),
                    "the stack trace should be preserved.");
            
            this.VerifyAsyncResultCompleted(true, true);
        }

        /// <summary>
        /// Tests if the callback function is called when it is configured.
        /// </summary>
        [Fact]
        public void SetAsCompletedExecutesCallback()
        {
            const int AsyncState = 3;
            this.SetUp(this.Callback, AsyncState);
            new AsyncResultCompletionInvoker(this.AsyncResultComplete).BeginInvoke(null, null);
            this.asyncResult.EndInvoke();
            this.VerifyAsyncResultCompleted(true, false);
            Thread.Sleep(20); // Give callback function time to finish
            
            this.callback
                .Should().Be(AsyncState, "the wrong asynchronous state should be passed to the callback function.");
        }

        /// <summary>
        /// Tests if the life time service is working correctly.
        /// </summary>
        [Fact]
        public void TestLifeTimeService()
        {
            ILease lease;
            try
            {
                this.SetUp(null, null);

                // Get the lease for the asyncResult from the RemotingServices
                RemotingServices.SetObjectUriForMarshal(this.asyncResult, "testUri");
                RemotingServices.Marshal(this.asyncResult);
                lease = RemotingServices.GetLifetimeService(this.asyncResult) as ILease;

                // Verify the initial state
                lease.Should().NotBeNull("initializeLifetimeService should return a lease.");
                lease.InitialLeaseTime.Should().Be(this.initialLeaseTime, "valid initial lease time.");
                lease.SponsorshipTimeout.Should().Be(this.sponsorshipTimeout, "valid sponsorship timeout.");
                lease.RenewOnCallTime.Should().Be(TimeSpan.Zero, "RenewOnCallTime is expected to be 0");
                lease.CurrentState.Should().Be(LeaseState.Active, "lease is expected to be active");
                this.VerifyLifetimeServiceRegistered(lease, true);

                // Renew lease and verify that the lease time is within 0 ms and the time configured by
                // initialLeaseTime
                lease.Renew(this.asyncResult.Renewal(lease));
                var leaseTime = lease.CurrentLeaseTime;
                this.initialLeaseTime.Should().BeLessOrEqualTo(leaseTime);
                leaseTime.Should().BeGreaterOrEqualTo(TimeSpan.FromMilliseconds(0));

                // Sleep some time and renew lease again and verify that the lease time is within 0 ms and 
                // the time configured by initialLeaseTime
                Thread.Sleep(this.initialLeaseTime.Add(TimeSpan.FromMilliseconds(5)));
                lease.Renew(this.asyncResult.Renewal(lease));

                leaseTime = lease.CurrentLeaseTime;
                this.initialLeaseTime.Should().BeLessOrEqualTo(leaseTime);
                leaseTime.Should().BeGreaterOrEqualTo(TimeSpan.FromMilliseconds(0));
            }
            finally
            {
                this.asyncResult.SetAsCompleted(null, true);
                this.asyncResult.EndInvoke();
            }

            this.VerifyLifetimeServiceRegistered(lease, false);
        }

        /// <summary>
        /// Sets the test up. Creates an instance of the <see cref="AsyncResult"/> and checks if
        /// was properly created.
        /// </summary>
        /// <param name="callbackMethod">The callback method.</param>
        /// <param name="state">The state.</param>
        private void SetUp(AsyncCallback callbackMethod, object state)
        {
            this.asyncResult = new AsyncResult(callbackMethod, state, this.initialLeaseTime, this.sponsorshipTimeout);
        }

        /// <summary>
        /// Verifies that the asyncResult is completed and that the type of completion is as specified.
        /// </summary>
        /// <param name="expectAsynchronousCompletion">if set to <c>true</c> an asynchronous completion is expected.
        /// </param>
        /// <param name="asyncStateIsNull">if set to <c>true</c> it is verified that the AsyncState is null.</param>
        private void VerifyAsyncResultCompleted(bool expectAsynchronousCompletion, bool asyncStateIsNull)
        {
            this.asyncResult.IsCompleted.Should().BeTrue("the asyncResult is expected to be completed.");
            if (expectAsynchronousCompletion)
            {
                this.asyncResult.CompletedSynchronously.Should().BeFalse("asynchronous completion is expected.");
            }
            else
            {
                this.asyncResult.CompletedSynchronously.Should().BeTrue("synchronous completion is expected.");
            }

            if (asyncStateIsNull)
            {
                this.asyncResult.AsyncState.Should().BeNull("the AsyncState is expected to be null.");
            }
        }

        /// <summary>
        /// Completes the asyncResult asynchronously.
        /// </summary>
        private void AsyncResultComplete()
        {
            Thread.Sleep(10); // Wait some time so that the thread calling EndInvoke is waiting for completion.
            this.asyncResult.SetAsCompleted(null, false);
        }

        /// <summary>
        /// Completes the asyncResult asynchronously with an exception.
        /// </summary>
        private void AsyncResultException()
        {
            Thread.Sleep(10); // Wait some time so that the thread calling EndInvoke is waiting for completion.
            try
            {
                throw new TimeoutException(); 
            }
            catch (Exception e)
            {
                this.asyncResult.SetAsCompleted(e, false);
            }
        }

        /// <summary>
        /// Callback that is called on completion of the asyncResult.
        /// </summary>
        /// <param name="result">The result.</param>
        private void Callback(IAsyncResult result)
        {
            this.callback = (int)result.AsyncState;
        }

        /// <summary>
        /// Verifies that a lease is registered/unregistered for the lifetime service.
        /// </summary>
        /// <param name="lease">The lease that is checked.</param>
        /// <param name="checkRegistered">if set to <c>true</c> it is checked that it is registered otherwise
        /// it must be unregistered.</param>
        private void VerifyLifetimeServiceRegistered(ILease lease, bool checkRegistered)
        {
            Hashtable sponsorTable = (Hashtable)lease.GetType()
                .GetField("sponsorTable", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(lease);
            string message = checkRegistered
                                 ? "Expected lifetime service to be registered."
                                 : "Expected lifetime service to be unregistered.";
            
            sponsorTable.Contains(this.asyncResult)
                .Should().Be(checkRegistered, message);
        }
    }
}

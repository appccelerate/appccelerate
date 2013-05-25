//-------------------------------------------------------------------------------
// <copyright file="AsyncResult.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Runtime.Remoting.Lifetime;
    using System.Threading;

    /// <summary>
    /// Result for asynchronous tasks
    /// </summary>
    public class AsyncResult : MarshalByRefObject, IAsyncResult, IDisposable, ISponsor
    {
        /// <summary>
        /// The default value for the InitialLeaseTime for ISponsor in seconds
        /// </summary>
        private const int DefaultInitialLeaseTime = 10;

        /// <summary>
        /// The default value for the Sponsorship Timeout for ISponsor in seconds
        /// </summary>
        private const int DefaultSponsorshipTimeout = 10;

        /// <summary>
        /// The callback function that is executed after completion.
        /// </summary>
        private readonly AsyncCallback asyncCallback;

        /// <summary>
        /// A user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        private readonly object asyncState;

        /// <summary>
        /// The initial lease time for the ISponsor
        /// </summary>
        private readonly TimeSpan initialLeaseTime = TimeSpan.FromSeconds(DefaultInitialLeaseTime);

        /// <summary>
        /// The sponsor ship timeout
        /// </summary>
        private readonly TimeSpan sponsorshipTimeout = TimeSpan.FromSeconds(DefaultSponsorshipTimeout);

        /// <summary>
        /// The current state of the asynchronous operation.
        /// </summary>
        private CompletionState completionState;

        /// <summary>
        /// Wait handle for waiting for completion of the operation.
        /// </summary>
        private ManualResetEvent asyncWaitHandle;

        /// <summary>
        /// The exception that occurred during execution of the operation.
        /// </summary>
        private Exception exception;

        /// <summary>
        /// Indicated if the object has been disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        /// A dictionary that is used to store user defined data.
        /// </summary>
        private Dictionary<string, object> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncResult"/> class.
        /// </summary>
        /// <param name="asyncCallback">The callback function that is called after the task has completed.</param>
        /// <param name="state">A user-defined object that qualifies or contains information about an asynchronous 
        /// operation.</param>
        public AsyncResult(AsyncCallback asyncCallback, object state)
        {
            this.asyncCallback = asyncCallback;
            this.asyncState = state;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncResult"/> class.
        /// </summary>
        /// <param name="asyncCallback">The callback function that is called after the task has completed.</param>
        /// <param name="state">A user-defined object that qualifies or contains information about an asynchronous
        /// operation.</param>
        /// <param name="initialLeaseTime">The initial sponsor lease time.</param>
        /// <param name="sponsorshipTimeout">The sponsorship timeout.</param>
        public AsyncResult(
            AsyncCallback asyncCallback,
            object state, 
            TimeSpan initialLeaseTime, 
            TimeSpan sponsorshipTimeout)
            : this(asyncCallback, state)
        {
            this.sponsorshipTimeout = sponsorshipTimeout;
            this.initialLeaseTime = initialLeaseTime;
        }

        /// <summary>
        /// The possible states of the asynchronous operation.
        /// </summary>
        private enum CompletionState
        {
            /// <summary>
            /// Operation is not finished yet
            /// </summary>
            Pending,

            /// <summary>
            /// Operation was executed synchronously
            /// </summary>
            CompletedSynchronously,

            /// <summary>
            /// Operation was executed asynchronously
            /// </summary>
            CompletedAsynchronously
        }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        /// <value></value>
        /// <returns>A user-defined object that qualifies or contains information about an asynchronous 
        /// operation.</returns>
        public object AsyncState
        {
            get
            {
                return this.asyncState;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the asynchronous operation completed synchronously.
        /// </summary>
        /// <value></value>
        /// <returns>true if the asynchronous operation completed synchronously; otherwise, false.</returns>
        public bool CompletedSynchronously
        {
            get
            {
                lock (this)
                {
                    return this.completionState == CompletionState.CompletedSynchronously;
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.WaitHandle"></see> that is used to wait for an asynchronous 
        /// operation to complete.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Threading.WaitHandle"></see> that is used to wait for an asynchronous 
        /// operation to complete.</returns>
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (this.asyncWaitHandle == null)
                {
                    bool done = this.IsCompleted;
                    ManualResetEvent mre = new ManualResetEvent(done);
                    if (Interlocked.CompareExchange(ref this.asyncWaitHandle, mre, null) != null)
                    {
                        // Another thread created this object's event; dispose 
                        // the event we just created
                        mre.Close();
                    }
                    else
                    {
                        if (!done && this.IsCompleted)
                        {
                            // If the operation wasn't done when we created 
                            // the event but now it is done, set the event
                            this.asyncWaitHandle.Set();
                        }
                    }
                }

                return this.asyncWaitHandle;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the asynchronous operation has completed.
        /// </summary>
        /// <value></value>
        /// <returns>true if the operation is complete; otherwise, false.</returns>
        public bool IsCompleted
        {
            get
            {
                lock (this)
                {
                    return this.completionState != CompletionState.Pending;
                }
            }
        }

        /// <summary>
        /// Gets a dictionary that can be used to store user defined data.
        /// </summary>
        public IDictionary<string, object> Data
        {
            get
            {
                return this.data ?? (this.data = new Dictionary<string, object>());
            }
        }

        /// <summary>
        /// Determines whether this instance is disposed.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDisposed()
        {
            return this.disposed;
        }

        /// <summary>
        /// Sets the task to the completed state. 
        /// </summary>
        /// <param name="occurredException">An exception that occurred while executing the task. Passing null for 
        /// exception means no error occurred. This is the common case.</param>
        /// <param name="completedSynchronously">True if the task run synchronously.</param>
        public void SetAsCompleted(
            Exception occurredException, bool completedSynchronously)
        {
            // Mutex for accessing member fields
            lock (this)
            {
                // Assert completed has not been called yet
                if (this.completionState != CompletionState.Pending)
                {
                    throw new InvalidOperationException("You can not set to completed because it is already completed.");
                }

                // Set new completion state
                this.completionState = completedSynchronously
                                      ? CompletionState.CompletedSynchronously
                                      : CompletionState.CompletedAsynchronously;

                // Set the exception
                this.exception = occurredException;
            }

            // If the event exists, set it
            if (this.asyncWaitHandle != null)
            {
                this.asyncWaitHandle.Set();
            }

            // If a callback method was set, call it
            if (this.asyncCallback != null)
            {
                this.asyncCallback(this);
            }
        }

        /// <summary>
        /// Waits until the task has completed. If an exception occurred it is re thrown.
        /// </summary>
        public void EndInvoke()
        {
            // Assert that the object is not disposed.
            if (this.IsDisposed())
            {
                throw new InvalidOperationException("AsyncResult is already disposed. Did you call EndInvoke twice?");    
            }

            try
            {
                // This method assumes that only one thread calls EndInvoke for this object
                if (!this.IsCompleted)
                {
                    // If the operation isn't done, wait for it
                    this.AsyncWaitHandle.WaitOne();
                    this.AsyncWaitHandle.Close();
                    this.asyncWaitHandle = null;
                }

                // Operation is done: if an exception occurred, re throw it
                if (this.exception != null)
                {
                    throw this.exception.PreserveStackTrace();
                }
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"></see> used to control the 
        /// lifetime policy for this instance. This is the current lifetime service object for this instance if 
        /// one exists; otherwise, a new lifetime service object initialized to the value of the 
        /// <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"></see> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure 
        /// permission. </exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, 
        /// Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" 
        /// Flags="RemotingConfiguration, Infrastructure"/></PermissionSet>
        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = this.initialLeaseTime;
                lease.Register(this);
                lease.SponsorshipTimeout = this.sponsorshipTimeout;
                lease.RenewOnCallTime = TimeSpan.Zero;
            }

            return lease;
        }

        /// <summary>
        /// Requests a sponsoring client to renew the lease for the specified object.
        /// </summary>
        /// <param name="lease">The lifetime lease of the object that requires lease renewal.</param>
        /// <returns>
        /// The additional lease time for the specified object.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller makes the call through 
        /// a reference to the interface and does not have infrastructure permission. </exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, 
        /// Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/>
        /// </PermissionSet>
        public TimeSpan Renewal(ILease lease)
        {
            Ensure.ArgumentNotNull(lease, "lease");

            return lease.InitialLeaseTime;
        }

        /// <summary>
        /// Releases lifetime service.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.IsDisposed())
                {
                    this.disposed = true;
                    
                    ILease lease = this.GetLifetimeService() as ILease;
                    if (lease != null)
                    {
                        lease.Unregister(this);
                    }
                }
            }
        }
    }
}

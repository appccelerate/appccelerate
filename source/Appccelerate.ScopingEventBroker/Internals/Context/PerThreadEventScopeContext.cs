//-------------------------------------------------------------------------------
// <copyright file="PerThreadEventScopeContext.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker.Internals.Context
{
    using System;
    using System.Threading;

    /// <summary>
    /// Allows to have an event scope per thread.
    /// </summary>
    public class PerThreadEventScopeContext : AbstractEventScopeContext, IDisposable
    {
        private readonly object disposeLock = new object();
        private ThreadLocal<IEventScopeInternal> current;

        public PerThreadEventScopeContext(IEventScopeFactory eventScopeFactory)
            : base(eventScopeFactory)
        {
            this.current = new ThreadLocal<IEventScopeInternal>();
        }

        protected override IEventScopeInternal CurrentScope
        {
            get { return this.current.Value; }
            set { this.current.Value = value; }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (this.disposeLock)
            {
                if (disposing && this.current != null)
                {
                    this.current.Dispose();
                    this.current = null;
                }
            }
        }

        protected override void ResetAction()
        {
            lock (this.disposeLock)
            {
                if (this.current != null)
                {
                    base.ResetAction();
                }
            }
        }
    }
}
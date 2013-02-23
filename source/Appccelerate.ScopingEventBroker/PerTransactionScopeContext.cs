//-------------------------------------------------------------------------------
// <copyright file="PerTransactionScopeContext.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Transactions;

    public sealed class PerTransactionScopeContext : AbstractEventScopeContext
    {
        private readonly ConcurrentDictionary<string, SinglePhaseScopeDecorator> scopesToTransactionIdentifier; 

        public PerTransactionScopeContext(IEventScopeFactory eventScopeFactory)
            : base(eventScopeFactory)
        {
            this.scopesToTransactionIdentifier = new ConcurrentDictionary<string, SinglePhaseScopeDecorator>();            
        }

        protected override IEventScopeInternal CurrentScope
        {
            get
            {
                return (IEventScopeInternal)this.Acquire();
            }

            set
            {
                throw new InvalidOperationException("Current scope should never be set!");
            }
        }

        public override IEventScope Acquire()
        {
            SinglePhaseScopeDecorator scope = null;
            Transaction transaction = Transaction.Current;

            if (transaction != null && !this.scopesToTransactionIdentifier.TryGetValue(
                    transaction.TransactionInformation.LocalIdentifier, out scope))
            {
                var localIdentifier = transaction.TransactionInformation.LocalIdentifier;

                scope = new SinglePhaseScopeDecorator(
                    this.ScopeFactory.CreateScope(), 
                    () =>
                    {
                        SinglePhaseScopeDecorator removed;
                        this.scopesToTransactionIdentifier.TryRemove(localIdentifier, out removed);
                    });

                transaction.EnlistVolatile(scope, EnlistmentOptions.None);

                this.scopesToTransactionIdentifier.AddOrUpdate(localIdentifier, scope, (k, v) => scope);
            }

            return scope;
        }

        protected override void ResetAction()
        {
            // no-op
        }
        
        private class SinglePhaseScopeDecorator : IEventScopeInternal, ISinglePhaseNotification
        {
            private readonly IEventScopeInternal scope;

            private readonly Action resetAction;

            public SinglePhaseScopeDecorator(IEventScopeInternal decorated, Action resetAction)
            {
                this.scope = decorated;
                this.resetAction = resetAction;
            }

            public void Release()
            {
                this.scope.Release();
            }

            public void Cancel()
            {
                this.scope.Cancel();
            }

            public void Register(Action releaseCallback)
            {
                this.scope.Register(releaseCallback);
            }

            public void Prepare(PreparingEnlistment preparingEnlistment)
            {
                preparingEnlistment.Prepared();
            }

            public void Commit(Enlistment enlistment)
            {
                this.Release();
                this.Dispose();

                enlistment.Done();
            }

            public void Rollback(Enlistment enlistment)
            {
                this.Cancel();
            }

            public void InDoubt(Enlistment enlistment)
            {
            }

            public void SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment)
            {
                this.Release();
                this.Dispose();

                singlePhaseEnlistment.Committed();
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    this.scope.Dispose();
                    this.resetAction();
                }
            }
        }
    }
}
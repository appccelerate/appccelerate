//-------------------------------------------------------------------------------
// <copyright file="PerTransactionScopeContext.cs" company="Appccelerate">
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
    using System.Collections.Concurrent;
    using System.Transactions;

    public class PerTransactionScopeContext : IEventScopeContextInternal
    {
        private readonly ConcurrentDictionary<string, SinglePhaseScopeDecorator> scopesToTransactionIdentifier;

        private readonly IEventScopeFactory scopeFactory;

        public PerTransactionScopeContext(IEventScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            this.scopesToTransactionIdentifier = new ConcurrentDictionary<string, SinglePhaseScopeDecorator>();
        }

        public IEventScopeInternal Current
        {
            get
            {
                return (IEventScopeInternal)this.Acquire();
            }
        }

        public IEventScope Acquire()
        {
            SinglePhaseScopeDecorator scope = null;
            Transaction transaction = Transaction.Current;

            if (transaction != null && !this.scopesToTransactionIdentifier.TryGetValue(
                    transaction.TransactionInformation.LocalIdentifier, out scope))
            {
                var localIdentifier = transaction.TransactionInformation.LocalIdentifier;

                scope = this.CreateSinglePhaseScopeDecoratorFor(localIdentifier);

                transaction.EnlistVolatile(scope, EnlistmentOptions.None);

                this.scopesToTransactionIdentifier.AddOrUpdate(localIdentifier, scope, (k, v) => scope);
            }

            return scope;
        }

        private SinglePhaseScopeDecorator CreateSinglePhaseScopeDecoratorFor(string localIdentifier)
        {
            return new SinglePhaseScopeDecorator(
                this.scopeFactory.CreateScope(), 
                () =>
                    {
                        SinglePhaseScopeDecorator removed;
                        this.scopesToTransactionIdentifier.TryRemove(localIdentifier, out removed);
                    });
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
                this.Dispose();
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
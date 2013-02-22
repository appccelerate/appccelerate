namespace Appccelerate.ScopingEventBroker
{
    using System;
    using System.Transactions;

    public sealed class PerTransactionScopeContext : AbstractEventScopeContext
    {
        private SinglePhaseScopeDecorator scope;

        public PerTransactionScopeContext(IEventScopeFactory eventScopeFactory)
            : base(eventScopeFactory)
        {
            
        }

        protected override IEventScopeInternal CurrentScope
        {
            get
            {
                this.Acquire();
                return this.scope;
            }
            set
            {
                this.scope = (SinglePhaseScopeDecorator)value;
            }

        }

        public override IEventScope Acquire()
        {
            Transaction transaction = Transaction.Current;
            if (transaction != null && this.scope == null)
            {
                this.scope = new SinglePhaseScopeDecorator(this.ScopeFactory.CreateScope(), this.ResetAction);

                transaction.EnlistVolatile(this.scope, EnlistmentOptions.None);
            }

            return this.scope;
        }
        
        private class SinglePhaseScopeDecorator : IEventScopeInternal, ISinglePhaseNotification
        {
            private IEventScopeInternal scope;

            private Action resetAction;

            public SinglePhaseScopeDecorator(IEventScopeInternal decorated, Action resetAction)
            {
                this.resetAction = resetAction;
                this.scope = decorated;
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
            }

            public void Commit(Enlistment enlistment)
            {
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
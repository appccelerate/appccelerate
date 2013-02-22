namespace Appccelerate.ScopingEventBroker
{
    public class TransactionScopeAwareEventScopeFactory : AbstractEventScopeFactory
    {
        protected override AbstractEventScopeContext CreateScope(IEventScopeFactory eventScopeFactory)
        {
            return new PerTransactionScopeContext(eventScopeFactory);
        }
    }
}
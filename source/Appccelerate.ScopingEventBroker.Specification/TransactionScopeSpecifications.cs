namespace Appccelerate.ScopingEventBroker.Specification
{
    using System.Transactions;

    using FluentAssertions;

    using Machine.Specifications;

    public class when_scope_disposed_and_not_completed : TransactionScopeSpecification
    {
        Because of = () =>
        {
            using (var tx = new TransactionScope(TransactionScopeOption.Required))
            {
                publisher.Publish();
            }
        };

        It should_not_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(NotCalled);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(Called);
    }

    public class when_scope_completed : TransactionScopeSpecification
    {
        Because of = () =>
            {
                using (var tx = new TransactionScope(TransactionScopeOption.Required))
                {
                    publisher.Publish();

                    tx.Complete();
                }
            };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(Called);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(Called);
    }

    public class TransactionScopeSpecification : ScopingEventBrokerSpecification
    {
        protected static long Called = 1;

        protected static long NotCalled = 0;

        Establish context = () => SetupScopingEventBrokerWith(new EventScopingStandardFactory(new TransactionScopeAwareEventScopeFactory()));
    }
}
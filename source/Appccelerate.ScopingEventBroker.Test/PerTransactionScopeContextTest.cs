namespace Appccelerate.ScopingEventBroker
{
    using System.Transactions;

    using FakeItEasy;

    using Xunit;

    using FluentAssertions;

    public class PerTransactionScopeContextTest
    {
        private readonly PerTransactionScopeContext testee;

        public PerTransactionScopeContextTest()
        {
            this.testee = new PerTransactionScopeContext(A.Fake<IEventScopeFactory>());
        }

        [Fact]
        public void Acquire_WithoutTransactionScope_ShouldReturnNull()
        {
            var scope = this.testee.Acquire();

            scope.Should().BeNull();
        }

        [Fact]
        public void Acquire_WithTransactionScope_ShouldReturnScope()
        {
            using (var tx = new TransactionScope(TransactionScopeOption.Required))
            {
                var scope = this.testee.Acquire();

                scope.Should().NotBeNull();
            }
        }

        [Fact]
        public void Acquire_WithInsideTransactionScope_ShouldReturnSameScope()
        {
            using (var tx = new TransactionScope(TransactionScopeOption.Required))
            {
                var firstScope = this.testee.Acquire();
                var secondScope = this.testee.Acquire();

                firstScope.Should().BeSameAs(secondScope);
            }
        }
    }
}
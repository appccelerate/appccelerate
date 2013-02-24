//-------------------------------------------------------------------------------
// <copyright file="PerTransactionScopeContextTest.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker.Internals.Context
{
    using System.Transactions;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

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
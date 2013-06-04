//-------------------------------------------------------------------------------
// <copyright file="PerTransactionScopeContextTest.cs" company="Appccelerate">
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
    using System.Transactions;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class PerTransactionScopeContextTest
    {
        private readonly IEventScopeInternal eventScope;

        private readonly IEventScopeFactory eventScopeFactory;

        private readonly PerTransactionScopeContext testee;

        public PerTransactionScopeContextTest()
        {
            this.eventScope = A.Fake<IEventScopeInternal>();
            this.eventScopeFactory = A.Fake<IEventScopeFactory>();

            this.testee = new PerTransactionScopeContext(this.eventScopeFactory);
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

        [Fact]
        public void Acquire_WithSupressedScope_ShouldReturnNull()
        {
            using (var tx = new TransactionScope(TransactionScopeOption.Suppress))
            {
                var scope = this.testee.Acquire();

                scope.Should().BeNull();
            }
        }

        [Fact]
        public void Current_WhenConstructed_ShouldBeNull()
        {
            this.SetupFactoryCreatesScope();

            this.testee.Current.Should().BeNull();
        }

        [Fact]
        public void Acquire_WithTransactionScope_ShouldSetCurrent()
        {
            this.SetupFactoryCreatesScope();

            using (var tx = new TransactionScope())
            {
                this.testee.Acquire();

                this.testee.Current.Should().NotBeNull();
            }
        }

        [Fact]
        public void Acquire_WithoutTransactionScope_ShouldNotSetCurrent()
        {
            this.SetupFactoryCreatesScope();

            this.testee.Acquire();

            this.testee.Current.Should().BeNull();
        }

        [Fact]
        public void Dispose_RemoveCurrent()
        {
            this.SetupFactoryCreatesScope();

            using (var tx = new TransactionScope())
            {
                this.testee.Acquire();
            }

            this.testee.Current.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldDisposeInner()
        {
            this.SetupFactoryCreatesScope();

            using (var tx = new TransactionScope())
            {
                this.testee.Acquire();
            }

            A.CallTo(() => this.eventScope.Dispose()).MustHaveHappened();
        }

        [Fact]
        public void Dispose_ShouldCancelInner()
        {
            this.SetupFactoryCreatesScope();

            using (var tx = new TransactionScope())
            {
                this.testee.Acquire();
            }

            A.CallTo(() => this.eventScope.Cancel()).MustHaveHappened();
        }

        [Fact]
        public void Complete_ShouldReleaseInner()
        {
            this.SetupFactoryCreatesScope();

            using (var tx = new TransactionScope())
            {
                this.testee.Acquire();

                tx.Complete();
            }

            A.CallTo(() => this.eventScope.Release()).MustHaveHappened();
        }

        [Fact]
        public void Complete_ShouldDisposeInner()
        {
            this.SetupFactoryCreatesScope();

            using (var tx = new TransactionScope())
            {
                this.testee.Acquire();

                tx.Complete();
            }

            A.CallTo(() => this.eventScope.Dispose()).MustHaveHappened();
        }

        [Fact]
        public void Register_WhenCurrent_ShouldRegisterOnAcquired()
        {
            this.SetupFactoryCreatesScope();

            using (var tx = new TransactionScope())
            {
                this.testee.Acquire();
                this.testee.Current.Register(() => { });
            }

            A.CallTo(() => this.eventScope.Register(A<Action>.Ignored)).MustHaveHappened();
        }

        private void SetupFactoryCreatesScope()
        {
            A.CallTo(() => this.eventScopeFactory.CreateScope()).Returns(this.eventScope);
        }
    }
}
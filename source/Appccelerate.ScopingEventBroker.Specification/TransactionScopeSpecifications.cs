//-------------------------------------------------------------------------------
// <copyright file="TransactionScopeSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker.Specification
{
    using System;
    using System.Threading.Tasks;
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

    public class when_nested_scope_completed_but_outer_not : TransactionScopeSpecification
    {
        Because of = () =>
        {
            using (var outer = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var nested = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    publisher.Publish();

                    nested.Complete();
                }

                publisher.Publish();
            }
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(Called);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledForInnerAndOuter);
    }

    public class when_nested_scope_and_outer_complete : TransactionScopeSpecification
    {
        Because of = () =>
        {
            using (var outer = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var nested = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    publisher.Publish();

                    nested.Complete();
                }

                publisher.Publish();

                outer.Complete();
            }
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(CalledForInnerAndOuter);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledForInnerAndOuter);
    }

    public class when_nested_scope_not_completed_but_outer : TransactionScopeSpecification
    {
        Because of = () =>
        {
            using (var outer = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var nested = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    publisher.Publish();
                }

                publisher.Publish();

                outer.Complete();
            }
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(Called);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledForInnerAndOuter);
    }

    public class when_nested_scope_suppressed_but_not_completed : TransactionScopeSpecification
    {
        Because of = () =>
        {
            using (var outer = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var nested = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    publisher.Publish();
                }

                publisher.Publish();

                outer.Complete();
            }
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(CalledForInnerAndOuter);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledForInnerAndOuter);
    }

    public class when_nested_scope_suppressed_and_completed : TransactionScopeSpecification
    {
        Because of = () =>
        {
            using (var outer = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var nested = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    publisher.Publish();

                    nested.Complete();
                }

                publisher.Publish();

                outer.Complete();
            }
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(CalledForInnerAndOuter);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledForInnerAndOuter);
    }

    public class when_dependent_scope_and_all_completed : TransactionScopeSpecification
    {
        Because of = () =>
        {
            using (var outer = new TransactionScope(TransactionScopeOption.Required))
            {
                Task.Factory.StartNew(
                    state =>
                        {
                            var dtx = (DependentTransaction)state;

                            using (var nested = new TransactionScope(dtx))
                            {
                                publisher.Publish();

                                nested.Complete();
                            }

                            dtx.Complete();
                        }, 
                        Transaction.Current.DependentClone(DependentCloneOption.BlockCommitUntilComplete));

                publisher.Publish();

                outer.Complete();
            }
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(CalledForInnerAndOuter);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledForInnerAndOuter);
    }

    public class when_dependent_scope_nested_not_completed : TransactionScopeSpecification
    {
        Because of = () => Catch.Exception(
            () =>
                {
                    using (var outer = new TransactionScope(TransactionScopeOption.Required))
                    {
                        Task.Factory.StartNew(
                            state =>
                                {
                                    var dtx = (DependentTransaction)state;

                                    using (var nested = new TransactionScope(dtx))
                                    {
                                        publisher.Publish();
                                    }

                                    dtx.Complete();
                                },
                            Transaction.Current.DependentClone(DependentCloneOption.BlockCommitUntilComplete));

                        publisher.Publish();

                        outer.Complete();
                    }
                });

        It should_not_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(NotCalled);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledForInnerAndOuter);
    }

    [Subject("Transaction Scope")]
    public class TransactionScopeSpecification : ScopingEventBrokerSpecification
    {
        protected static long CalledForInnerAndOuter = 2;

        protected static long Called = 1;

        protected static long NotCalled = 0;

        Establish context = () => SetupScopingEventBrokerWith(new EventScopingStandardFactory(new TransactionScopeAwareEventScopeFactory()));
    }
}
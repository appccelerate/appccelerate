//-------------------------------------------------------------------------------
// <copyright file="PerThreadScopeSpecifications.cs" company="Appccelerate">
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

    using FluentAssertions;

    using Machine.Specifications;

    public class when_no_scope_acquired_on_thread : PerThreadScopeSpecification
    {
        Because of = () => publisher.Publish();

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(CalledOnce);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledOnce);
    }

    public class when_scope_acquired_on_different_threads : PerThreadScopeSpecification
    {
        private static IEventScope firstScope;
        private static IEventScope secondScope;

        Because of = () =>
        {
            Tuple<IEventScope, IEventScope> result = 
                ExecuteOnDifferentThreads(() => scopeContext.Acquire(), () => scopeContext.Acquire());

            firstScope = result.Item1;
            secondScope = result.Item2;
        };

        Cleanup cleanup = () =>
        {
            firstScope.Dispose();
            secondScope.Dispose();
        };

        It should_use_different_scope_for_each_thread = () => firstScope.Should().NotBeSameAs(secondScope);
    }

    public class when_scope_cancelled_on_same_thread : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            using (IEventScope scope = scopeContext.Acquire())
            {
                publisher.Publish();

                scope.Cancel();
            }
        };

        It should_not_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(NotCalled);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledOnce);
    }

    public class when_scope_cancelled_on_another_thread : PerThreadScopeSpecification
    {
        Because of = () =>
            {
                Action action1 = () =>
                    {
                        using (IEventScope scope = scopeContext.Acquire())
                        {
                            publisher.Publish();

                            scope.Release();
                        }
                    };

                Action action2 = () =>
                    {
                        using (IEventScope scope = scopeContext.Acquire())
                        {
                            publisher.Publish();

                            scope.Cancel();
                        }
                    };

                ExecuteOnDifferentThreads(action1, action2);
        };

        It should_invoke_asynchronous_subscriber_for_releasing_thread = () => subscriber.Asynchronous.Should().Be(CalledOnce);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledTwice);
    }

    public class when_scope_cancelled_on_both_threads : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            Action action1 = () =>
            {
                using (IEventScope scope = scopeContext.Acquire())
                {
                    publisher.Publish();

                    scope.Cancel();
                }
            };

            Action action2 = () =>
            {
                using (IEventScope scope = scopeContext.Acquire())
                {
                    publisher.Publish();

                    scope.Cancel();
                }
            };

            ExecuteOnDifferentThreads(action1, action2);
        };

        It should_not_invoke_asynchronous_subscriber_for_both_threads = () => subscriber.Asynchronous.Should().Be(NotCalled);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledTwice);
    }

    public class when_scope_disposed_and_not_released_on_same_thread : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            using (scopeContext.Acquire())
            {
                publisher.Publish();
            }
        };

        It should_not_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(NotCalled);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledOnce);
    }

    public class when_scope_disposed_and_not_released_on_another_thread : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            Action action1 = () =>
            {
                using (IEventScope scope = scopeContext.Acquire())
                {
                    publisher.Publish();

                    scope.Release();
                }
            };

            Action action2 = () =>
            {
                using (scopeContext.Acquire())
                {
                    publisher.Publish();
                }
            };

            ExecuteOnDifferentThreads(action1, action2);
        };

        It should_invoke_asynchronous_subscriber_for_releasing_thread = () => subscriber.Asynchronous.Should().Be(CalledOnce);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledTwice);
    }

    public class when_scope_disposed_and_not_released_on_both_threads : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            Action action1 = () =>
            {
                using (scopeContext.Acquire())
                {
                    publisher.Publish();
                }
            };

            Action action2 = () =>
            {
                using (scopeContext.Acquire())
                {
                    publisher.Publish();
                }
            };

            ExecuteOnDifferentThreads(action1, action2);
        };

        It should_not_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(NotCalled);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledTwice);
    }

    public class when_scope_released_on_same_thread : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            using (IEventScope scope = scopeContext.Acquire())
            {
                publisher.Publish();

                scope.Release();
            }
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(CalledOnce);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledOnce);
    }

    public class when_scope_released_on_both_threads : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            Action action1 = () =>
            {
                using (IEventScope scope = scopeContext.Acquire())
                {
                    publisher.Publish();

                    scope.Release();
                }
            };

            Action action2 = () =>
            {
                using (IEventScope scope = scopeContext.Acquire())
                {
                    publisher.Publish();

                    scope.Release();
                }
            };

            ExecuteOnDifferentThreads(action1, action2);
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(CalledTwice);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(CalledTwice);
    }

    [Subject("Per Thread Scope")]
    public class PerThreadScopeSpecification : ScopingEventBrokerSpecification
    {
        protected static long CalledTwice = 2;

        protected static long CalledOnce = 1;

        protected static long NotCalled = 0;

        Establish context = () => SetupScopingEventBrokerWith(new EventScopingStandardFactory(new PerThreadEventScopeFactory()));

        protected static void ExecuteOnDifferentThreads(Action firstThreadAction, Action secondThreadAction)
        {
            var firstScopeTask = Task.Factory.StartNew(firstThreadAction);
            var secondScopeTask = Task.Factory.StartNew(secondThreadAction);

            Task.WaitAll(firstScopeTask, secondScopeTask);
        }

        protected static Tuple<IEventScope, IEventScope> ExecuteOnDifferentThreads(Func<IEventScope> firstThreadAction, Func<IEventScope> secondThreadAction)
        {
            var firstScopeTask = Task.Factory.StartNew(firstThreadAction);
            var secondScopeTask = Task.Factory.StartNew(secondThreadAction);

            Task.WaitAll(firstScopeTask, secondScopeTask);

            return new Tuple<IEventScope, IEventScope>(firstScopeTask.Result, secondScopeTask.Result);
        }
    }
}
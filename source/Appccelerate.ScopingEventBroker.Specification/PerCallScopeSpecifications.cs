//-------------------------------------------------------------------------------
// <copyright file="PerCallScopeSpecifications.cs" company="Appccelerate">
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
    using FluentAssertions;

    using Machine.Specifications;

    public class when_no_scope_acquired : PerCallScopeSpecification
    {
        Because of = () => publisher.Publish();

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(Called);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(Called);
    }

    public class when_scope_cancelled : PerCallScopeSpecification
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

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(Called);
    }

    public class when_scope_disposed_and_not_released : PerCallScopeSpecification
    {
        Because of = () =>
        {
            using (scopeContext.Acquire())
            {
                publisher.Publish();
            }
        };

        It should_not_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(NotCalled);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(Called);
    }

    public class when_scope_released : PerCallScopeSpecification
    {
        Because of = () =>
        {
            using (IEventScope scope = scopeContext.Acquire())
            {
                publisher.Publish();

                scope.Release();
            }
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.Asynchronous.Should().Be(Called);

        It should_invoke_synchronous_subscriber = () => subscriber.Synchronous.Should().Be(Called);
    }

    [Subject("Per Call Scope")]
    public class PerCallScopeSpecification : ScopingEventBrokerSpecification
    {
        protected static long Called = 1;

        protected static long NotCalled = 0;

        Establish context = () => SetupScopingEventBrokerWithDefaultFactory();
    }
}
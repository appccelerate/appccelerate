//-------------------------------------------------------------------------------
// <copyright file="PerThreadScopeSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker.Specification
{
    using FluentAssertions;

    using Machine.Specifications;

    public class when_no_scope_acquired2 : PerThreadScopeSpecification
    {
        Because of = () => publisher.Publish();

        It should_invoke_asynchronous_subscriber = () => subscriber.OnAsynchronousWasCalled.Should().BeTrue();

        It should_invoke_synchronous_subscriber = () => subscriber.OnSynchronousWasCalled.Should().BeTrue();
    }

    public class when_scope_cancelled2 : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            using (IEventScope scope = scopeContext.Acquire())
            {
                publisher.Publish();

                scope.Cancel();
            }
        };

        It should_not_invoke_asynchronous_subscriber = () => subscriber.OnAsynchronousWasCalled.Should().BeFalse();

        It should_invoke_synchronous_subscriber = () => subscriber.OnSynchronousWasCalled.Should().BeTrue();
    }

    public class when_scope_disposed_and_not_released2 : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            using (scopeContext.Acquire())
            {
                publisher.Publish();
            }
        };

        It should_not_invoke_asynchronous_subscriber = () => subscriber.OnAsynchronousWasCalled.Should().BeFalse();

        It should_invoke_synchronous_subscriber = () => subscriber.OnSynchronousWasCalled.Should().BeTrue();
    }

    public class when_scope_released2 : PerThreadScopeSpecification
    {
        Because of = () =>
        {
            using (IEventScope scope = scopeContext.Acquire())
            {
                publisher.Publish();

                scope.Release();
            }
        };

        It should_invoke_asynchronous_subscriber = () => subscriber.OnAsynchronousWasCalled.Should().BeTrue();

        It should_invoke_synchronous_subscriber = () => subscriber.OnSynchronousWasCalled.Should().BeTrue();
    }

    public class PerThreadScopeSpecification : ScopingEventBrokerSpecification
    {
        Establish context = () => SetupScopingEventBrokerWith(new PerThreadScopeFactory());

        private class PerThreadScopeFactory : DefaultEventScopeFactory
        {
            protected override AbstractEventScopeContext CreateScope(IEventScopeFactory eventScopeFactory)
            {
                return new PerThreadEventScopeContext(eventScopeFactory);
            }
        }
    }
}
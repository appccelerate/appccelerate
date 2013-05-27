//-------------------------------------------------------------------------------
// <copyright file="SubscriptionMatchersSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Matchers
{
    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.Matchers)]
    public class When_firing_an_event
    {
        protected static EventBroker eventBroker;
        protected static ScopeEvent.NamedPublisher publisher;
        protected static ScopeEvent.NamedSubscriber matchingSubscriber;
        protected static ScopeEvent.NamedSubscriber nonMatchingSubscriber;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
            publisher = new ScopeEvent.NamedPublisher("A.Name");
            matchingSubscriber = new ScopeEvent.NamedSubscriber("A.Name");
            nonMatchingSubscriber = new ScopeEvent.NamedSubscriber("A");

            eventBroker.Register(publisher);
            eventBroker.Register(matchingSubscriber);
            eventBroker.Register(nonMatchingSubscriber);
        };

        Because of = () =>
            publisher.FireEventGlobally();

        It should_call_subscriber_method_with_matching_subscriber_matcher = () =>
            matchingSubscriber.CalledFromParent
                .Should().BeTrue();

        It should_not_call_subscriber_method_with_non_matching_subscriber_matcher = () =>
            nonMatchingSubscriber.CalledFromParent
                .Should().BeFalse();
    }
}
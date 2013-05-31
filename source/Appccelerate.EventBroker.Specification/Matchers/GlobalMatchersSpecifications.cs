//-------------------------------------------------------------------------------
// <copyright file="GlobalMatchersSpecifications.cs" company="Appccelerate">
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
    using System;
    using System.IO;

    using FluentAssertions;

    using Machine.Specifications;

    public class When_firing_an_event_and_global_matchers_match : GlobalMatcherSpecification
    {
        Establish context = () => 
            eventBroker.AddGlobalMatcher(new Matcher());

        Because of = () =>
            publisher.FireEvent(EventArgs.Empty);

        It should_call_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeTrue("matcher should not block call");

        public class Matcher : IMatcher
        {
            public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
            {
                return true;
            }

            public void DescribeTo(TextWriter writer)
            {
            }
        }
    }

    public class When_firing_an_event_and_a_global_matcher_does_not_match : GlobalMatcherSpecification
    {
        Establish context = () => 
            eventBroker.AddGlobalMatcher(new Matcher());

        Because of = () =>
            publisher.FireEvent(EventArgs.Empty);

        It should_not_call_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeFalse("matcher should block call");

        public class Matcher : IMatcher
        {
            public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
            {
                return false;
            }

            public void DescribeTo(TextWriter writer)
            {
            }
        }
    }

    [Subject(Subjects.Matchers)]
    public class GlobalMatcherSpecification
    {
        protected static EventBroker eventBroker;
        protected static SimpleEvent.EventPublisher publisher;
        protected static SimpleEvent.EventSubscriber subscriber;

        Establish context = () =>
        {
            eventBroker = new EventBroker();
            publisher = new SimpleEvent.EventPublisher();
            subscriber = new SimpleEvent.EventSubscriber();

            eventBroker.Register(publisher);
            eventBroker.Register(subscriber);
        };
    }
}
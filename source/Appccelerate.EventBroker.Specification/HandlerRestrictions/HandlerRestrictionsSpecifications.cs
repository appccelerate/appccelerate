//-------------------------------------------------------------------------------
// <copyright file="HandlerRestrictionsSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.HandlerRestrictions
{
    using System;

    using Appccelerate.EventBroker.Internals.Exceptions;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.HandlerRestriction)]
    public class When_firing_an_event_with_handler_restriction_and_a_matching_subscriber
    {
        static EventBroker eventBroker;
        static HandlerRestrictionEvent.SynchronousSubscriber subscriber;

        Establish context = () =>
        {
            eventBroker = new EventBroker();

            subscriber = new HandlerRestrictionEvent.SynchronousSubscriber();

            eventBroker.Register(subscriber);
        };

        Because of = () => 
            eventBroker.Fire(HandlerRestrictionEvent.EventTopic, new object(), HandlerRestriction.Synchronous, new object(), EventArgs.Empty);

        It should_call_subscriber = () =>
            subscriber.HandledEvent
                .Should().BeTrue();
    }

    [Subject(Subjects.HandlerRestriction)]
    public class When_firing_an_event_with_handler_restriction_and_a_non_matching_subscriber
    {
        static EventBroker eventBroker;
        static HandlerRestrictionEvent.SynchronousSubscriber subscriber;
        static Exception exception;

        Establish context = () =>
        {
            eventBroker = new EventBroker();

            subscriber = new HandlerRestrictionEvent.SynchronousSubscriber();

            eventBroker.Register(subscriber);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() =>
                eventBroker.Fire(HandlerRestrictionEvent.EventTopic, new object(), HandlerRestriction.Asynchronous, new object(), EventArgs.Empty));
        };

        It should_throw_exception = () =>
            exception.Should().BeOfType<EventTopicException>();
    }
}
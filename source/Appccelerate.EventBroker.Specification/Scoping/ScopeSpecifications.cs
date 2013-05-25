//-------------------------------------------------------------------------------
// <copyright file="ScopeSpecifications.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Scoping
{
    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Subjects.Scope)]
    public class When_firing_an_event_globally : ScopeSpecification
    {
        Because of = () =>
            publisher.FireEventGlobally();

        It should_be_handled_by_global_handler_of_parent = () =>
            subscriberParent.CalledGlobally.Should().BeTrue();

        It should_be_handled_by_child_handler_of_parent = () =>
            subscriberParent.CalledFromChild.Should().BeTrue();

        It should_not_be_handled_by_parent_handler_of_parent = () =>
            subscriberParent.CalledFromParent.Should().BeFalse();

        It should_be_handled_by_global_handler_of_twin = () =>
            subscriberTwin.CalledGlobally.Should().BeTrue();

        It should_be_handled_by_child_handler_of_twin = () =>
            subscriberTwin.CalledFromChild.Should().BeTrue();

        It should_be_handled_by_parent_handler_of_twin = () =>
            subscriberTwin.CalledFromParent.Should().BeTrue();

        It should_be_handled_by_global_handler_of_sibling = () =>
            subscriberSibling.CalledGlobally.Should().BeTrue();

        It should_not_be_handled_by_child_handler_of_sibling = () =>
            subscriberSibling.CalledFromChild.Should().BeFalse();

        It should_not_be_handled_by_parent_handler_of_sibling = () =>
            subscriberSibling.CalledFromParent.Should().BeFalse();

        It should_be_handled_by_global_handler_of_child = () =>
            subscriberChild.CalledGlobally.Should().BeTrue();

        It should_not_be_handled_by_child_handler_of_child = () =>
            subscriberChild.CalledFromChild.Should().BeFalse();

        It should_be_handled_by_parent_handler_of_child = () =>
            subscriberChild.CalledFromParent.Should().BeTrue();
    }

    [Subject(Subjects.Scope)]
    public class When_firing_event_to_parents : ScopeSpecification
    {
        Because of = () =>
            publisher.FireEventToParentsAndSiblings();

        It should_be_handled_by_global_handler_of_parent = () =>
            subscriberParent.CalledGlobally.Should().BeTrue();

        It should_be_handled_by_child_handler_of_parent = () =>
            subscriberParent.CalledFromChild.Should().BeTrue();

        It should_not_be_handled_by_parent_handler_of_parent = () =>
            subscriberParent.CalledFromParent.Should().BeFalse();

        It should_be_handled_by_global_handler_of_twin = () =>
            subscriberTwin.CalledGlobally.Should().BeTrue();

        It should_be_handled_by_child_handler_of_twin = () =>
            subscriberTwin.CalledFromChild.Should().BeTrue();

        It should_be_handled_by_parent_handler_of_twin = () =>
            subscriberTwin.CalledFromParent.Should().BeTrue();

        It should_not_be_handled_by_global_handler_of_sibling = () =>
            subscriberSibling.CalledGlobally.Should().BeFalse();

        It should_not_be_handled_by_child_handler_of_sibling = () =>
            subscriberSibling.CalledFromChild.Should().BeFalse();

        It should_not_be_handled_by_parent_handler_of_sibling = () =>
            subscriberSibling.CalledFromParent.Should().BeFalse();

        It should_not_be_handled_by_global_handler_of_child = () =>
            subscriberChild.CalledGlobally.Should().BeFalse();

        It should_not_be_handled_by_child_handler_of_child = () =>
            subscriberChild.CalledFromChild.Should().BeFalse();

        It should_not_be_handled_by_parent_handler_of_child = () =>
            subscriberChild.CalledFromParent.Should().BeFalse();
    }

    [Subject(Subjects.Scope)]
    public class When_firing_events_to_children : ScopeSpecification
    {
        Because of = () =>
            publisher.FireEventToChildrenAndSiblings();

        It should_not_be_handled_by_global_handler_of_parent = () =>
            subscriberParent.CalledGlobally.Should().BeFalse();

        It should_not_be_handled_by_child_handler_of_parent = () =>
            subscriberParent.CalledFromChild.Should().BeFalse();

        It should_not_be_handled_by_parent_handler_of_parent = () =>
            subscriberParent.CalledFromParent.Should().BeFalse();

        It should_be_handled_by_global_handler_of_twin = () =>
            subscriberTwin.CalledGlobally.Should().BeTrue();

        It should_be_handled_by_child_handler_of_twin = () =>
            subscriberTwin.CalledFromChild.Should().BeTrue();

        It should_be_handled_by_parent_handler_of_twin = () =>
            subscriberTwin.CalledFromParent.Should().BeTrue();

        It should_not_be_handled_by_global_handler_of_sibling = () =>
            subscriberSibling.CalledGlobally.Should().BeFalse();

        It should_not_be_handled_by_child_handler_of_sibling = () =>
            subscriberSibling.CalledFromChild.Should().BeFalse();

        It should_not_be_handled_by_parent_handler_of_sibling = () =>
            subscriberSibling.CalledFromParent.Should().BeFalse();

        It should_be_handled_by_global_handler_of_child = () =>
            subscriberChild.CalledGlobally.Should().BeTrue();

        It should_not_be_handled_by_child_handler_of_child = () =>
            subscriberChild.CalledFromChild.Should().BeFalse();

        It should_be_handled_by_parent_handler_of_child = () =>
            subscriberChild.CalledFromParent.Should().BeTrue();            
    }

    [Subject(Subjects.Scope)]
    public class ScopeSpecification
    {
        protected static EventBroker eventBroker;
        protected static ScopeEvent.NamedPublisher publisher;
        protected static ScopeEvent.NamedSubscriber subscriberParent;
        protected static ScopeEvent.NamedSubscriber subscriberTwin;
        protected static ScopeEvent.NamedSubscriber subscriberSibling;
        protected static ScopeEvent.NamedSubscriber subscriberChild;

        Establish context = () =>
            {
                eventBroker = new EventBroker();

                publisher = new ScopeEvent.NamedPublisher("Test.One");
                subscriberParent = new ScopeEvent.NamedSubscriber("Test");
                subscriberTwin = new ScopeEvent.NamedSubscriber("Test.One");
                subscriberSibling = new ScopeEvent.NamedSubscriber("Test.Two");
                subscriberChild = new ScopeEvent.NamedSubscriber("Test.One.Child");

                eventBroker.Register(publisher);
                eventBroker.Register(subscriberParent);
                eventBroker.Register(subscriberTwin);
                eventBroker.Register(subscriberSibling);
                eventBroker.Register(subscriberChild);
            };
    }
}
//-------------------------------------------------------------------------------
// <copyright file="ReflectionMethodHandleProblem.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker
{
    using System;

    using Appccelerate.Events;

    using FluentAssertions;

    using Xunit;

    /// <summary>
    /// Tests that the reflection calls do not introduce additional ghost methods due to a defect in .net reflection.
    /// </summary>
    public class ReflectionMethodHandleProblem
    {
        private EventBroker testee;

        public interface ITestPublisher
        {
            [EventPublication(@"event://ITestPublisher/MyEvent")]
            event EventHandler<EventArgs<int>> MyEvent;

            void FireEvent();
        }

        public interface ITestSubscriber
        {
            int MyValue { get; }

            [EventSubscription(@"event://ITestPublisher/MyEvent", typeof(Handlers.OnPublisher))]
            void HandleMyEvent(object sender, EventArgs<int> e);
        }

        [Fact]
        public void DoesNotIntroduceGhostMethodsThroughRegistrationAndHandlingEvents()
        {
            this.testee = new EventBroker();

            ITestPublisher testPublisher = new MyPublisher();
            ITestSubscriber testSubscriber = new MySubscriber();

            int initialMethodCount = testSubscriber.GetType().GetMethods().GetLength(0);

            this.testee.Register(testPublisher);

            int methodCountAfterRegisteringPublisher = testSubscriber.GetType().GetMethods().GetLength(0);

            this.testee.Register(testSubscriber);

            int methodCountAfterRegisteringSubscriber = testSubscriber.GetType().GetMethods().GetLength(0);

            testPublisher.FireEvent();

            int methodCountAfterFiringEvent = testSubscriber.GetType().GetMethods().GetLength(0);

            methodCountAfterRegisteringPublisher.Should().Be(initialMethodCount, "registration of publisher should not introduce ghost methods");
            methodCountAfterRegisteringSubscriber.Should().Be(initialMethodCount, "registration of subscriber should not introduce ghost methods");
            methodCountAfterFiringEvent.Should().Be(initialMethodCount, "calling handler method should not introduce ghost methods");

            testSubscriber.MyValue.Should().Be(6);
        }

        public class MySubscriber : ITestSubscriber
        {
            public int MyValue
            {
                get;
                private set;
            }

            public void HandleMyEvent(object sender, EventArgs<int> e)
            {
                this.MyValue = e.Value;
            }
        }

        public class MyPublisher : ITestPublisher
        {
            public event EventHandler<EventArgs<int>> MyEvent;

            public void FireEvent()
            {
                if (null != this.MyEvent)
                {
                    this.MyEvent(this, new EventArgs<int>(6));
                }
            }
        }
    }
}
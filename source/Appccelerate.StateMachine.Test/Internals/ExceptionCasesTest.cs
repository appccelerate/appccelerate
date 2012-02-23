//-------------------------------------------------------------------------------
// <copyright file="ExceptionCasesTest.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Internals
{
    using System;

    using FluentAssertions;

    using Xunit;

    /// <summary>
    /// Tests exception behavior of the <see cref="StateMachine{TState,TEvent}"/>.
    /// </summary>
    public class ExceptionCasesTest
    {
        private readonly StateMachine<States, Events> testee;

        /// <summary>
        /// the state that was provided in the <see cref="StateMachine{TState,TEvent}.ExceptionThrown"/> event.
        /// </summary>
        private States? recordedStateId;

        /// <summary>
        /// the event that was provided in the <see cref="StateMachine{TState,TEvent}.ExceptionThrown"/> event.
        /// </summary>
        private Events? recordedEventId;

        /// <summary>
        /// the event argument that was provided in the <see cref="StateMachine{TState,TEvent}.ExceptionThrown"/> event.
        /// </summary>
        private object recordedEventArgument;

        /// <summary>
        /// the exception that was provided in the <see cref="StateMachine{TState,TEvent}.ExceptionThrown"/> event.
        /// </summary>
        private Exception recordedException;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCasesTest"/> class.
        /// </summary>
        public ExceptionCasesTest()
        {
            this.testee = new StateMachine<States, Events>();

            this.testee.ExceptionThrown += (sender, eventArgs) =>
                                               {
                                                   if (eventArgs != null)
                                                   {
                                                       this.recordedException = eventArgs.Exception;
                                                   }
                                               };

            this.testee.TransitionExceptionThrown += (sender, eventArgs) =>
                                                         {
                                                             this.recordedStateId = eventArgs.StateId;
                                                             this.recordedEventId = eventArgs.EventId;
                                                             this.recordedEventArgument = eventArgs.EventArgument;
                                                             this.recordedException = eventArgs.Exception;
                                                         };
        }

        /// <summary>
        /// When the state machine is not initialized then an exception is throw when firing events on it.
        /// </summary>
        [Fact]
        public void ExceptionIfNotInitialized()
        {
            Assert.Throws<InvalidOperationException>(() => this.testee.CurrentStateId);
            Assert.Throws<InvalidOperationException>(() => this.testee.Fire(Events.A));
        }

        /// <summary>
        /// When the state machine is initialized twice then an exception is thrown
        /// </summary>
        [Fact]
        public void ExceptionIfInitializeIsCalledTwice()
        {
            this.testee.Initialize(States.A);

            Assert.Throws<InvalidOperationException>(
                () => this.testee.Initialize(States.B));
        }

        /// <summary>
        /// When a guard throws an exception then it is catched and the <see cref="StateMachine{TState,TEvent}.ExceptionThrown"/> event is fired.
        /// The transition is not executed and if there is no other transition then the state machine remains in the same state.
        /// </summary>
        [Fact]
        public void ExceptionThrowingGuard()
        {
            var eventArguments = new object[] { 1, 2, "test" };
            Exception exception = new Exception();
            
            this.testee.In(States.A)
                .On(Events.B)
                    .If(() => { throw exception; }).Goto(States.B);

            bool transitionDeclined = false;
            this.testee.TransitionDeclined += (sender, e) => transitionDeclined = true;

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B, eventArguments);

            this.AssertException(States.A, Events.B, eventArguments, exception);
            Assert.Equal(States.A, this.testee.CurrentStateId);
            Assert.True(transitionDeclined, "transition was not declined.");
        }

        /// <summary>
        /// When a transition throws an exception then the exception is catched and the <see cref="StateMachine{TState,TEvent}.ExceptionThrown"/> event is fired.
        /// The transition is executed and the state machine is in the target state.
        /// </summary>
        [Fact]
        public void ExceptionThrowingAction()
        {
            var eventArguments = new object[] { 1, 2, "test" };
            Exception exception = new Exception();

            this.testee.In(States.A)
                .On(Events.B).Goto(States.B).Execute(() =>
                                                         {
                                                             throw exception;
                                                         });

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B, eventArguments);

            this.AssertException(States.A, Events.B, eventArguments, exception);
            Assert.Equal(States.B, this.testee.CurrentStateId);
        }

        [Fact]
        public void EntryActionWhenThrowingExceptionThenNotificationAndStateIsEntered()
        {
            var eventArguments = new object[] { 1, 2, "test" };
            Exception exception = new Exception();

            this.testee.In(States.A)
                .On(Events.B).Goto(States.B);

            this.testee.In(States.B)
                .ExecuteOnEntry(() =>
                                    {
                                        throw exception;
                                    });

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B, eventArguments);

            this.AssertException(States.A, Events.B, eventArguments, exception);
            Assert.Equal(States.B, this.testee.CurrentStateId);
        }

        [Fact]
        public void ExitActionWhenThrowingExceptionThenNotificationAndStateIsEntered()
        {
            var eventArguments = new object[] { 1, 2, "test" };
            Exception exception = new Exception();

            this.testee.In(States.A)
                .ExecuteOnExit(() =>
                                   {
                                       throw exception;
                                   })
                .On(Events.B).Goto(States.B);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B, eventArguments);

            this.AssertException(States.A, Events.B, eventArguments, exception);
            Assert.Equal(States.B, this.testee.CurrentStateId);
        }

        /// <summary>
        /// The state machine has to be initialized before events can be fired.
        /// </summary>
        [Fact]
        public void NotInitialized()
        {
            Assert.Throws<InvalidOperationException>(
                () => this.testee.Fire(Events.B));
        }

        /// <summary>
        /// When a state is added to two super states then an exception is thrown.
        /// </summary>
        [Fact]
        public void DefineNonTreeHierarchy()
        {
            this.testee.DefineHierarchyOn(States.A, States.B, HistoryType.None, States.B);
            
            Assert.Throws<InvalidOperationException>(
                () => this.testee.DefineHierarchyOn(States.C, States.B, HistoryType.None, States.B));
        }

        [Fact]
        public void MultipleTransitionsWithoutGuardsWhenDefiningAGotoTheninvalidOperationException()
        {
            this.testee.In(States.A)
                .On(Events.B).If(() => false).Goto(States.C)
                .On(Events.B).Goto(States.B);

            Action action = () => this.testee.In(States.A).On(Events.B).Goto(States.C);

            action.ShouldThrow<InvalidOperationException>().WithMessage(ExceptionMessages.OnlyOneTransitionMayHaveNoGuard);
        }

        [Fact]
        public void MultipleTransitionsWithoutGuardsWhenDefiningAnActionTheninvalidOperationException()
        {
            this.testee.In(States.A)
                .On(Events.B).Goto(States.B);

            Action action = () => this.testee.In(States.A).On(Events.B).Execute(() => { });

            action.ShouldThrow<InvalidOperationException>().WithMessage(ExceptionMessages.OnlyOneTransitionMayHaveNoGuard);
        }

        [Fact]
        public void TransitionWithoutGuardHasToBeLast()
        {
            this.testee.In(States.A)
                .On(Events.B).Goto(States.B);

            Action action = () => this.testee.In(States.A).On(Events.B).If(() => false).Execute(() => { });

            action.ShouldThrow<InvalidOperationException>().WithMessage(ExceptionMessages.TransitionWithoutGuardHasToBeLast);
        }

        /// <summary>
        /// Asserts that the correct exception was notified.
        /// </summary>
        /// <param name="expectedStateId">The expected state id.</param>
        /// <param name="expectedEventId">The expected event id.</param>
        /// <param name="expectedEventArguments">The expected event arguments.</param>
        /// <param name="expectedException">The expected exception.</param>
        private void AssertException(States expectedStateId, Events expectedEventId, object[] expectedEventArguments, Exception expectedException)
        {
            Assert.Equal(expectedStateId, this.recordedStateId);
            Assert.Equal(expectedEventId, this.recordedEventId);
            Assert.Equal(expectedEventArguments, this.recordedEventArgument);
            Assert.Equal(expectedException, this.recordedException);
        }
    }
}
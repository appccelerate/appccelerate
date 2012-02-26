//-------------------------------------------------------------------------------
// <copyright file="ExtensionTest.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine
{
    using System;
    using Appccelerate.StateMachine.Machine;
    using Appccelerate.StateMachine.Machine.Contexts;
    
    using FakeItEasy;

    using Xunit;

    /// <summary>
    /// Tests that the extensions can interact with the state machine.
    /// </summary>
    public class ExtensionTest
    {
        private readonly StateMachine<States, Events> testee;
        private readonly IExtension<States, Events> extension;
        private readonly OverrideExtension overrideExtension;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionTest"/> class.
        /// </summary>
        public ExtensionTest()
        {
            this.testee = new StateMachine<States, Events>();

            this.extension = A.Fake<IExtension<States, Events>>();
            this.overrideExtension = new OverrideExtension();
            this.testee.AddExtension(this.extension);
            this.testee.AddExtension(this.overrideExtension);
        }

        /// <summary>
        /// When the state machine is initialized then the extensions get notified.
        /// </summary>
        [Fact]
        public void Initialize()
        {
            States initialState = States.A;

            this.testee.Initialize(initialState);
            
            A.CallTo(() => this.extension.InitializingStateMachine(this.testee, ref initialState))
                .MustHaveHappened();

            A.CallTo(() => this.extension.InitializedStateMachine(this.testee, initialState))
                .MustHaveHappened();
        }

        [Fact]
        public void EnterInitialState()
        {
            const States InitialState = States.A;

            this.testee.Initialize(InitialState);
            this.testee.EnterInitialState();

            A.CallTo(() => this.extension.EnteringInitialState(this.testee, InitialState))
                .MustHaveHappened();

            A.CallTo(() => this.extension.EnteredInitialState(
                    this.testee,
                    InitialState,
                    A<StateContext<States, Events>>.That.Matches(context => context.State == null)))
                .MustHaveHappened();
        }

        /// <summary>
        /// An extension can override the state to which the state machine is initialized.
        /// </summary>
        [Fact]
        public void OverrideInitialState()
        {
            this.overrideExtension.OverriddenState = States.B;

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            States? actualState = this.testee.CurrentStateId;

            Assert.Equal(States.B, actualState);
        }

        /// <summary>
        /// When an event is fired on the state machine then the extensions are notified.
        /// </summary>
        [Fact]
        public void Fire()
        {
            this.testee.In(States.A).On(Events.B).Goto(States.B);
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            Events eventId = Events.B;
            var eventArgument = new object();

            this.testee.Fire(eventId, eventArgument);

            A.CallTo(() => this.extension.FiringEvent(this.testee, ref eventId, ref eventArgument))
                .MustHaveHappened();

            A.CallTo(() => this.extension.FiredEvent(
                    this.testee,
                    A<ITransitionContext<States, Events>>.That.Matches(
                        context => 
                            context.State.Id == States.A && 
                            context.EventId == eventId && 
                            context.EventArgument == eventArgument)))
                .MustHaveHappened();
        }

        /// <summary>
        /// An extension can override the event id and the event arguments.
        /// </summary>
        [Fact]
        public void OverrideFiredEvent()
        {
            this.testee.In(States.A)
                .On(Events.B).Goto(States.B)
                .On(Events.C).Goto(States.C);
       
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            const Events NewEvent = Events.C;
            var newEventArgument = new object();
            this.overrideExtension.OverriddenEvent = NewEvent;
            this.overrideExtension.OverriddenEventArgument = newEventArgument;
            
            this.testee.Fire(Events.B);

            A.CallTo(() => this.extension.FiredEvent(
                this.testee, 
                A<ITransitionContext<States, Events>>.That.Matches(
                    c => c.EventId == NewEvent && c.EventArgument == newEventArgument)))
                .MustHaveHappened();
        }

        /// <summary>
        /// Exceptions thrown by guards are passed to extensions.
        /// </summary>
        [Fact]
        public void ExceptionThrowingGuard()
        {
            Exception exception = new Exception();

            this.testee.TransitionExceptionThrown += (s, e) => { };

            this.testee.In(States.A).On(Events.B).If(() => { throw exception; }).Execute(() => { });
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B);

            A.CallTo(() => this.extension.HandlingGuardException(
                         this.testee,
                         Transition(States.A),
                         Context(States.A, Events.B),
                         ref exception))
                .MustHaveHappened();

            A.CallTo(() => this.extension.HandledGuardException(
                    this.testee, 
                    Transition(States.A),
                    Context(States.A, Events.B),
                    exception))
                .MustHaveHappened();
        }

        /// <summary>
        /// An extension can override the exception thrown by a transition guard.
        /// </summary>
        [Fact]
        public void OverrideGuardException()
        {
            Exception exception = new Exception();
            Exception overriddenException = new Exception();

            this.testee.TransitionExceptionThrown += (s, e) => { };

            this.testee.In(States.A).On(Events.B).If(() => { throw exception; }).Execute(() => { });
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.overrideExtension.OverriddenException = overriddenException;

            this.testee.Fire(Events.B);

            A.CallTo(() => this.extension.HandledGuardException(
                this.testee, 
                A<ITransition<States, Events>>._,
                A<ITransitionContext<States, Events>>._,
                overriddenException))
                .MustHaveHappened();
        }

        /// <summary>
        /// Exceptions thrown in actions are passed to the extensions.
        /// </summary>
        [Fact]
        public void ExceptionThrowingAction()
        {
            Exception exception = new Exception();

            this.testee.TransitionExceptionThrown += (s, e) => { };
            
            this.testee.In(States.A).On(Events.B).Execute(() => { throw exception; });
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B);

            A.CallTo(() => this.extension.HandlingTransitionException(
                    this.testee, 
                    Transition(States.A),
                    Context(States.A, Events.B), 
                    ref exception))
                .MustHaveHappened();

            A.CallTo(() => this.extension.HandledTransitionException(
                    this.testee, 
                    Transition(States.A),
                    Context(States.A, Events.B), 
                    exception))
                .MustHaveHappened();
        }

        /// <summary>
        /// An extension can override the exception thrown by an action.
        /// </summary>
        [Fact]
        public void OverrideActionException()
        {
            Exception exception = new Exception();
            Exception overriddenException = new Exception();

            this.testee.TransitionExceptionThrown += (s, e) => { };

            this.testee.In(States.A).On(Events.B).Execute(() => { throw exception; });
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.overrideExtension.OverriddenException = overriddenException;

            this.testee.Fire(Events.B);

            A.CallTo(() => this.extension.HandledTransitionException(
                    this.testee, 
                    A<ITransition<States, Events>>._,
                    A<ITransitionContext<States, Events>>._, 
                    overriddenException))
                .MustHaveHappened();
        }

        /// <summary>
        /// Exceptions thrown by entry actions are passed to extensions.
        /// </summary>
        [Fact]
        public void EntryActionException()
        {
            Exception exception = new Exception();

            this.testee.TransitionExceptionThrown += (s, e) => { };

            this.testee.In(States.A).On(Events.B).Goto(States.B);
            this.testee.In(States.B).ExecuteOnEntry(() => { throw exception; });
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B);

            A.CallTo(() => this.extension.HandlingEntryActionException(
                    this.testee, 
                    State(States.B),
                    Context(States.A, Events.B),
                    ref exception))
                .MustHaveHappened();

            A.CallTo(() => this.extension.HandledEntryActionException(
                    this.testee, 
                    State(States.B),
                    Context(States.A, Events.B),
                    exception))
                .MustHaveHappened();
        }

        /// <summary>
        /// An extension can override the exception thrown by an entry action.
        /// </summary>
        [Fact]
        public void OverrideEntryActionException()
        {
            Exception exception = new Exception();
            Exception overriddenException = new Exception();

            this.testee.TransitionExceptionThrown += (s, e) => { };

            this.testee.In(States.A).On(Events.B).Goto(States.B);
            this.testee.In(States.B).ExecuteOnEntry(() => { throw exception; });
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.overrideExtension.OverriddenException = overriddenException;

            this.testee.Fire(Events.B);

            A.CallTo(() => this.extension.HandledEntryActionException(
                    this.testee, 
                    A<IState<States, Events>>._,
                    A<ITransitionContext<States, Events>>._,
                    overriddenException))
                .MustHaveHappened();
        }

        /// <summary>
        /// Exceptions thrown by exit actions are passed to extensions.
        /// </summary>
        [Fact]
        public void ExitActionException()
        {
            Exception exception = new Exception();

            this.testee.TransitionExceptionThrown += (s, e) => { };

            this.testee.In(States.A)
                .ExecuteOnExit(() => { throw exception; })
                .On(Events.B).Goto(States.B);
                
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B);

            A.CallTo(() => this.extension.HandlingExitActionException(
                    this.testee, 
                    State(States.A),
                    Context(States.A, Events.B),
                    ref exception))
                .MustHaveHappened();

            A.CallTo(() => this.extension.HandledExitActionException(
                    this.testee, 
                    State(States.A),
                    Context(States.A, Events.B),
                    exception))
                .MustHaveHappened();
        }

        /// <summary>
        /// Exceptions thrown by exit actions can be overridden by extensions.
        /// </summary>
        [Fact]
        public void OverrideExitActionException()
        {
            Exception exception = new Exception();
            Exception overriddenException = new Exception();

            this.testee.TransitionExceptionThrown += (s, e) => { };

            this.testee.In(States.A)
                .ExecuteOnExit(() => { throw exception; })
                .On(Events.B).Goto(States.B);
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.overrideExtension.OverriddenException = overriddenException;

            this.testee.Fire(Events.B);

            A.CallTo(() => this.extension.HandledExitActionException(
                    this.testee, 
                    A<IState<States, Events>>._,
                    A<ITransitionContext<States, Events>>._,
                    overriddenException))
                .MustHaveHappened();
        }

        /// <summary>
        /// Exceptions thrown by entry actions during initialization are passed to extensions.
        /// </summary>
        [Fact]
        public void EntryActionExceptionDuringInitialization()
        {
            Exception exception = new Exception();

            this.testee.ExceptionThrown += (s, e) => { };

            this.testee.In(States.A).ExecuteOnEntry(() => { throw exception; });
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            A.CallTo(() => this.extension.HandlingEntryActionException(
                    this.testee,
                    State(States.A),
                    A<StateContext<States, Events>>.That.Matches(context => context.State == null),
                    ref exception))
                .MustHaveHappened();

            A.CallTo(() => this.extension.HandledEntryActionException(
                    this.testee, 
                    State(States.A),
                    A<StateContext<States, Events>>.That.Matches(context => context.State == null),
                    exception))
                .MustHaveHappened();
        }

        private static IState<States, Events> State(States stateId)
        {
            return A<IState<States, Events>>.That.Matches(state => state.Id == stateId);
        }

        private static ITransition<States, Events> Transition(States sourceState)
        {
            return A<ITransition<States, Events>>.That.Matches(transition => transition.Source.Id == sourceState);
        }

        private static ITransitionContext<States, Events> Context(States sourceState, Events eventId)
        {
            return A<ITransitionContext<States, Events>>.That.Matches(context => context.EventId == eventId && context.State.Id == sourceState);
        }

        /// <summary>
        /// Extension that can be used in tests to override exceptions, states and events passed to it.
        /// </summary>
        private class OverrideExtension : Extensions.ExtensionBase<States, Events>
        {
            /// <summary>
            /// Gets or sets the state used to override states passed in.
            /// </summary>
            /// <value>The state of the overridden.</value>
            public States? OverriddenState { get; set; }

            /// <summary>
            /// Gets or sets the event used to override events passed in.
            /// </summary>
            /// <value>The overridden event.</value>
            public Events? OverriddenEvent { get; set; }

            /// <summary>
            /// Gets or sets the event argument used to override event argument passed in.
            /// </summary>
            /// <value>The overridden event argument.</value>
            public object OverriddenEventArgument { get; set; }

            /// <summary>
            /// Gets or sets the exception used to override exceptions passed in.
            /// </summary>
            /// <value>The overridden exception.</value>
            public Exception OverriddenException { get; set; }

            /// <summary>
            /// Overrides the state passed in if <see cref="OverriddenState"/> is not null.
            /// </summary>
            /// <param name="stateMachine">The state machine.</param>
            /// <param name="initialState">The initial state. Can be replaced by the extension.</param>
            public override void InitializingStateMachine(IStateMachineInformation<States, Events> stateMachine, ref States initialState)
            {
                if (this.OverriddenState.HasValue)
                {
                    initialState = this.OverriddenState.Value;
                }
            }

            /// <summary>
            /// Overrides the event and event arguments with <see cref="OverriddenEvent"/> and <see cref="OverriddenEventArgument"/> if they are not null.
            /// </summary>
            /// <param name="stateMachine">The state machine.</param>
            /// <param name="eventId">The event id. Can be replaced by the extension.</param>
            /// <param name="eventArgument">The event argument. Can be replaced by the extension.</param>
            public override void FiringEvent(IStateMachineInformation<States, Events> stateMachine, ref Events eventId, ref object eventArgument)
            {
                if (this.OverriddenEvent.HasValue)
                {
                    eventId = this.OverriddenEvent.Value;
                }

                if (this.OverriddenEventArgument != null)
                {
                    eventArgument = this.OverriddenEventArgument;
                }
            }

            /// <summary>
            /// Overrides the exception if <see cref="OverriddenException"/> is not null.
            /// </summary>
            /// <param name="stateMachine">The state machine.</param>
            /// <param name="transition">The transition.</param>
            /// <param name="transitionContext">The transition context.</param>
            /// <param name="exception">The exception. Can be replaced by the extension.</param>
            public override void HandlingGuardException(IStateMachineInformation<States, Events> stateMachine, ITransition<States, Events> transition, ITransitionContext<States, Events> transitionContext, ref Exception exception)
            {
                if (this.OverriddenException != null)
                {
                    exception = this.OverriddenException;
                }
            }

            /// <summary>
            /// Overrides the exception if <see cref="OverriddenException"/> is not null.
            /// </summary>
            /// <param name="stateMachine">The state machine.</param>
            /// <param name="transition">The transition.</param>
            /// <param name="context">The context.</param>
            /// <param name="exception">The exception. Can be replaced by the extension.</param>
            public override void HandlingTransitionException(IStateMachineInformation<States, Events> stateMachine, ITransition<States, Events> transition, ITransitionContext<States, Events> context, ref Exception exception)
            {
                if (this.OverriddenException != null)
                {
                    exception = this.OverriddenException;
                }
            }

            /// <summary>
            /// Overrides the exception if <see cref="OverriddenException"/> is not null.
            /// </summary>
            /// <param name="stateMachine">The state machine.</param>
            /// <param name="state">The state.</param>
            /// <param name="stateContext">The state context.</param>
            /// <param name="exception">The exception. Can be replaced by the extension.</param>
            public override void HandlingEntryActionException(IStateMachineInformation<States, Events> stateMachine, IState<States, Events> state, IStateContext<States, Events> stateContext, ref Exception exception)
            {
                if (this.OverriddenException != null)
                {
                    exception = this.OverriddenException;
                }
            }

            /// <summary>
            /// Overrides the exception if <see cref="OverriddenException"/> is not null.
            /// </summary>
            /// <param name="stateMachine">The state machine.</param>
            /// <param name="state">The state.</param>
            /// <param name="stateContext">The state context.</param>
            /// <param name="exception">The exception. Can be replaced by the extension.</param>
            public override void HandlingExitActionException(IStateMachineInformation<States, Events> stateMachine, IState<States, Events> state, IStateContext<States, Events> stateContext, ref Exception exception)
            {
                if (this.OverriddenException != null)
                {
                    exception = this.OverriddenException;
                }
            }
        }
    }
}
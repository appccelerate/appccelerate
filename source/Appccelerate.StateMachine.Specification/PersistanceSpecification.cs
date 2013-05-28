//-------------------------------------------------------------------------------
// <copyright file="PersistanceSpecification.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine
{
    using System;
    using System.Collections.Generic;
    using Appccelerate.StateMachine.Machine;
    using Appccelerate.StateMachine.Persistence;
    using FluentAssertions;
    using global::Machine.Specifications;

    [Subject("Persistence")]
    public class When_resetting_a_state_machine_from_persisted_data
    {
        static PassiveStateMachine<State, Event> machine;
        static PassiveStateMachine<State, Event> loadedMachine;
        static StateMachineSaver<State> saver;
        static StateMachineLoader<State> loader;
        static State currentState;

        Establish context = () =>
            {
                machine = new PassiveStateMachine<State, Event>();
                DefineMachine(machine);
                machine.Initialize(State.A);
                machine.Start();
                machine.Fire(Event.B);

                saver = new StateMachineSaver<State>();
                loader = new StateMachineLoader<State>();
            };

        Because of = () =>
            {
                machine.Save(saver);
                loader.SetCurrentState(saver.CurrentStateId);
                
                loadedMachine = new PassiveStateMachine<State, Event>();
                DefineMachine(loadedMachine);
                loadedMachine.Load(loader);

                loadedMachine.TransitionCompleted += (sender, args) => currentState = args.NewStateId;
                
                loadedMachine.Start();
                loadedMachine.Fire(Event.X);
            };

        It should_reset_current_state = () =>
            currentState.Should().Be(State.B);

        It should_reset_all_history_states_of_super_states;

        enum State
        {
            A, B
        }

        enum Event
        {
            B, X
        }

        private static void DefineMachine(IStateMachine<State, Event> fsm)
        {
            fsm.In(State.A)
                   .On(Event.B).Goto(State.B)
                   .On(Event.X);

            fsm.In(State.B)
                   .On(Event.X);
        }
    }

    public class StateMachineSaver<TState> : IStateMachineSaver<TState>
        where TState : IComparable
    {
        public Initializable<TState> CurrentStateId { get; private set; }

        public void SaveCurrentState(Initializable<TState> currentState)
        {
            this.CurrentStateId = currentState;
        }
    }

    public class StateMachineLoader<TState> : IStateMachineLoader<TState>
        where TState : IComparable
    {
        private IEnumerable<TState> states;

        private Initializable<TState> currentState;

        public void SetCurrentState(Initializable<TState> state)
        {
            this.currentState = state;    
        }

        public Initializable<TState> GetCurrentState()
        {
            return this.currentState;
        }
    }
}
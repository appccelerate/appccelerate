//-------------------------------------------------------------------------------
// <copyright file="PersistanceSpecification.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Linq;
    using Appccelerate.StateMachine.Machine;
    using Appccelerate.StateMachine.Persistence;
    using FluentAssertions;
    using global::Machine.Specifications;

    [Subject("Persistence")]
    public class When_resetting_a_state_machine_from_persisted_data
    {
        static PassiveStateMachine<State, Event> machine;
        static PassiveStateMachine<State, Event> loadedMachine;
        static StateMachineSaver<State, Event> saver;
        static StateMachineLoader<State, Event> loader;
        static Reporter before;
        static Reporter after;

        Establish context = () =>
            {
                machine = new PassiveStateMachine<State, Event>();

                machine.In(State.A)
                    .On(Event.B).Goto(State.B);

                machine.Initialize(State.A);

                saver = new StateMachineSaver<State, Event>();
                loader = new StateMachineLoader<State, Event>();

                before = new Reporter();
                machine.Report(before);

                after = new Reporter();
            };

        Because of = () =>
            {
                machine.Save(saver);
                loader.SetStates(saver.StateIds);

                loadedMachine = new PassiveStateMachine<State, Event>();
                loadedMachine.Load(loader);

                loadedMachine.Report(after);
            };

        It should_reset_all_states = () =>
            before.States.Select(state => state.Id)
                .Should().BeEquivalentTo(after.States.Select(state => state.Id));

        It should_reset_all_transistions;

        It should_reset_all_history_states;

        It should_reset_all_guards;

        It should_reset_current_state;

        enum State
        {
            A, B
        }

        enum Event
        {
            B
        }

        private class Reporter : IStateMachineReport<State, Event>
        {
            public IEnumerable<IState<State, Event>> States { get; private set; }

            public void Report(string name, IEnumerable<IState<State, Event>> states, Initializable<State> initialStateId)
            {
                this.States = states;
            }
        }
    }

    public class StateMachineSaver<TState, TEvent> : IStateMachineSaver<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        public StateMachineSaver()
        {
            this.StateIds = new List<TState>();
        }

        public IList<TState> StateIds { get; private set; } 

        public void VisitState(IState<TState, TEvent> state)
        {
            this.StateIds.Add(state.Id);
        }
    }

    public class StateMachineLoader<TState, TEvent> : IStateMachineLoader<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        private IEnumerable<TState> states;

        public void SetStates(IEnumerable<TState> stateIds)
        {
            this.states = stateIds;
        }

        public void VisitStateMachine(IStateMachine<TState, TEvent> machine)
        {
            foreach (TState state in this.states)
            {
                machine.In(state);  
            }
        }
    }
}
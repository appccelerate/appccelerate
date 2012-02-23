//-------------------------------------------------------------------------------
// <copyright file="StateMachineBuildHierarchyTest.cs" company="Appccelerate">
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
    using Xunit;

    /// <summary>
    /// Tests hierarchy building in the <see cref="StateMachine{TState,TEvent}"/>.
    /// </summary>
    public class StateMachineBuildHierarchyTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private readonly StateMachine<States, Events> testee;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachineBuildHierarchyTest"/> class.
        /// </summary>
        public StateMachineBuildHierarchyTest()
        {
            this.testee = new StateMachine<States, Events>();
        }

        /// <summary>
        /// If a state is specified as the initial sub state that is not in the list of sub states then an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Fact]
        public void AddHierarchicalStatesInitialStateIsNotASubState()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.DefineHierarchyOn(States.B, States.A, HistoryType.None, States.B1, States.B2));
        }

        /// <summary>
        /// If the super-state is specified as the initial state of its sub-states then an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Fact]
        public void AddHierarchicalStatesInitialStateIsSuperStateItself()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.DefineHierarchyOn(States.B, States.B, HistoryType.None, States.B1, States.B2));
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="TransitionTestBase.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Machine.Transitions
{
    using FakeItEasy;

    public class TransitionTestBase
    {
        public enum States
        {
        }

        public enum Events
        {
        }

        protected readonly Transition<States, Events> Testee;

        private readonly IStateMachineInformation<States, Events> stateMachineInformation;
        private readonly IExtensionHost<States, Events> extensionHost;

        protected IState<States, Events> Source { get; set; }
        
        protected IState<States, Events> Target { get; set; }
        
        protected ITransitionContext<States, Events> TransitionContext { get; set; }

        public TransitionTestBase()
        {
            this.stateMachineInformation = A.Fake<IStateMachineInformation<States, Events>>();
            this.extensionHost = A.Fake<IExtensionHost<States, Events>>();

            this.Testee = new Transition<States, Events>(this.stateMachineInformation, this.extensionHost);
        } 
    }
}
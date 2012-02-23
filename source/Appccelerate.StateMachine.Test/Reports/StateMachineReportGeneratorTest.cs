//-------------------------------------------------------------------------------
// <copyright file="StateMachineReportGeneratorTest.cs" company="Appccelerate">
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
namespace Appccelerate.StateMachine.Reports
{
    using Appccelerate.StateMachine.Internals;

    using FluentAssertions;

    using Xunit;

    public class StateMachineReportGeneratorTest
    {
        private readonly StateMachine<States, Events> machine;

        public StateMachineReportGeneratorTest()
        {
            this.machine = new StateMachine<States, Events>("Test Machine");
        }

        [Fact]
        public void Report()
        {
            this.machine.DefineHierarchyOn(States.B, States.B1, HistoryType.None, States.B1, States.B2);
            this.machine.DefineHierarchyOn(States.C, States.C1, HistoryType.Shallow, States.C1, States.C2);
            this.machine.DefineHierarchyOn(States.C1, States.C1a, HistoryType.Shallow, States.C1a, States.C1b);
            this.machine.DefineHierarchyOn(States.D, States.D1, HistoryType.Deep, States.D1, States.D2);
            this.machine.DefineHierarchyOn(States.D1, States.D1a, HistoryType.Deep, States.D1a, States.D1b);

            this.machine.In(States.A)
                .ExecuteOnEntry(EnterA)
                .ExecuteOnExit(ExitA)
                .On(Events.A)
                .On(Events.B).Goto(States.B)
                .On(Events.C).If(() => true).Goto(States.C1)
                .On(Events.C).If(() => false).Goto(States.C2);

            this.machine.In(States.B)
                .On(Events.A).Goto(States.A).Execute(Action);

            this.machine.In(States.B1)
                .On(Events.B2).Goto(States.B1);

            this.machine.In(States.B2)
                .On(Events.B1).Goto(States.B2);

            this.machine.Initialize(States.A);

            var testee = new StateMachineReportGenerator<States, Events>();
            this.machine.Report(testee);

            string report = testee.Result;

            const string ExpectedReport =
@"Test Machine: initial state = A
    B: initial state = B1 history type = None
        entry action: 
        exit action: 
        A -> A actions: Action guard: 
        B1: initial state = None history type = None
            entry action: 
            exit action: 
            B2 -> B1 actions:  guard: 
        B2: initial state = None history type = None
            entry action: 
            exit action: 
            B1 -> B2 actions:  guard: 
    C: initial state = C1 history type = Shallow
        entry action: 
        exit action: 
        C1: initial state = C1a history type = Shallow
            entry action: 
            exit action: 
            C1a: initial state = None history type = None
                entry action: 
                exit action: 
            C1b: initial state = None history type = None
                entry action: 
                exit action: 
        C2: initial state = None history type = None
            entry action: 
            exit action: 
    D: initial state = D1 history type = Deep
        entry action: 
        exit action: 
        D1: initial state = D1a history type = Deep
            entry action: 
            exit action: 
            D1a: initial state = None history type = None
                entry action: 
                exit action: 
            D1b: initial state = None history type = None
                entry action: 
                exit action: 
        D2: initial state = None history type = None
            entry action: 
            exit action: 
    A: initial state = None history type = None
        entry action: EnterA
        exit action: ExitA
        A -> internal actions:  guard: 
        B -> B actions:  guard: 
        C -> C1 actions:  guard: anonymous
        C -> C2 actions:  guard: anonymous
";
            report.Replace("\n", string.Empty).Replace("\r", string.Empty)
                .Should().Be(ExpectedReport.Replace("\n", string.Empty).Replace("\r", string.Empty));
        }

        private static void EnterA()
        {
        }

        private static void ExitA()
        {
        }

        private static void Action()
        {
        }
    }
}
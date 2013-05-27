//-------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Appccelerate">
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

    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var numberOfRuns = int.Parse(args.Length > 0 ? args[0] : "10");
            var numberOfEvents = int.Parse(args.Length > 1 ? args[1] : "10000");
            var numberOfSubscribers = int.Parse(args.Length > 2 ? args[2] : "2");
            
            Console.WriteLine(string.Format("running {0} runs with {1} events and max {2} subscribers", numberOfRuns, numberOfEvents, numberOfSubscribers));
            Console.WriteLine();

            var runner = new Runner(numberOfRuns, numberOfEvents, numberOfSubscribers);

            runner.Run();

            Console.ReadLine();
        }
    }
}
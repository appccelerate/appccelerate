//-------------------------------------------------------------------------------
// <copyright file="Runner.cs" company="Appccelerate">
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
    using System.Diagnostics;

    public class Runner
    {
        private readonly int numberOfRuns;

        private readonly int numberOfEvents;

        private readonly int numberOfSubscribers;

        public Runner(int numberOfRuns, int numberOfEvents, int numberOfSubscribers)
        {
            this.numberOfRuns = numberOfRuns;
            this.numberOfEvents = numberOfEvents;
            this.numberOfSubscribers = numberOfSubscribers;
        }

        public void Run()
        {
            for (int i = 0; i < this.numberOfRuns; i++)
            {
                this.DotNetEvent(i);
                this.SimpleEvent(i);
                this.TrueMatcher(i);
                this.FalseMatcher(i);
                this.Subscribers(i);
            }
        }

        private void DotNetEvent(int runNumber)
        {
            var p = new Publisher();
            var s = new Subscriber();

            p.Event += s.HandleEvent;

            this.Run(
                p.FireEvent,
                runNumber + " .Net event",
                this.numberOfEvents * 9);
        }

        private void SimpleEvent(int runNumber)
        {
            EventBroker eventBroker = new EventBroker();
            
            var p = new Publisher();
            var s = new Subscriber();

            eventBroker.Register(p);
            eventBroker.Register(s);

            this.Run(
                p.FireEvent,
                runNumber + " simple event",
                0);
        }

        private void TrueMatcher(int runNumber)
        {
            EventBroker eventBroker = new EventBroker();

            eventBroker.AddGlobalMatcher(new Matcher(true));

            var p = new Publisher();
            var s = new Subscriber();

            eventBroker.Register(p);
            eventBroker.Register(s);

            this.Run(
                p.FireEvent,
                runNumber + " true matcher",
                0);
        }

        private void FalseMatcher(int runNumber)
        {
            EventBroker eventBroker = new EventBroker();

            eventBroker.AddGlobalMatcher(new Matcher(false));

            var p = new Publisher();
            var s = new Subscriber();

            eventBroker.Register(p);
            eventBroker.Register(s);

            this.Run(
                p.FireEvent,
                runNumber + " false matcher",
                0);
        }

        private void Subscribers(int runNumber)
        {
            for (int i = 1; i <= this.numberOfSubscribers; i *= 2)
            {
                this.Subscribers(runNumber, i);
            }
        }

        private void Subscribers(int runNumber, int numberOfSubscribersInThisRun)
        {
            EventBroker eventBroker = new EventBroker();

            var p = new Publisher();
            var subscribers = new Subscriber[numberOfSubscribersInThisRun];

            for (int i = 0; i < numberOfSubscribersInThisRun; i++)
            {
                subscribers[i] = new Subscriber();
                eventBroker.Register(subscribers[i]);
            }

            eventBroker.Register(p);
           
            this.Run(
                p.FireEvent,
                runNumber + " number of subscribers " + numberOfSubscribersInThisRun,
                -9 * this.numberOfEvents / 10);
        }

        private void Run(Action run, string message, int correction)
        {
            run();

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            int actualNumberOfEvents = this.numberOfEvents + correction;
            for (int i = 0; i < actualNumberOfEvents; i++)
            {
                run();
            }

            stopwatch.Stop();
            Console.WriteLine(string.Format(
                "{0} {1} per event {2:0.000}",
                message,
                stopwatch.ElapsedMilliseconds,
                (double)stopwatch.ElapsedMilliseconds / actualNumberOfEvents));
        }
    }
}
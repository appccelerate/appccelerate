//-------------------------------------------------------------------------------
// <copyright file="ModuleControllerMultiThreadModuleTest.cs" company="Appccelerate">
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

namespace Appccelerate.AsyncModule
{
    using System;
    using System.Threading;

    using FluentAssertions;

    using Xunit;

    public class ModuleControllerMultiThreadModuleTest
    {
        private ModuleController testee;
        private TestModule module;

        public ModuleControllerMultiThreadModuleTest()
        {
            this.module = new TestModule();
        }

        [Fact]
        public void ConsumeMessagesOnMultipleWorkerThreads()
        {
            const int NumerOfMessages = 1000;

            this.module.ConsumeDelay = TimeSpan.FromMilliseconds(0);
            this.testee = new ModuleController();
            this.testee.Initialize(this.module, 10);

            for (int i = 0; i < NumerOfMessages; i++)
            {
                this.testee.EnqueueMessage(i);
            }

            AutoResetEvent signal = new AutoResetEvent(false);
            int count = 0;
            object padlock = new object();
            this.testee.AfterConsumeMessage += delegate
                {
                    lock (padlock)
                    {
                        count++;
                        if (count == NumerOfMessages)
                        {
                            signal.Set();
                        }
                    }
                };

            this.testee.Start();

            signal.WaitOne(10000, false).Should().BeTrue("all messages should be consumed. Consumed " + this.module.Messages.Count);
            this.testee.Stop();
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="ModuleControllerTest.cs" company="Appccelerate">
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

    using Appccelerate.AsyncModule.Events;

    using Xunit;
    using Xunit.Sdk;

    public class ModuleControllerTest
    {
        private ModuleController testee;
        private TestModule module;

        public ModuleControllerTest()
        {
            this.module = new TestModule();
            this.testee = new ModuleController();
            this.testee.Initialize(this.module);

            this.module.Controller = this.testee;
        }

        [Fact]
        public void ControllerCanBeStartedAndStopped()
        {
            bool beforeModuleStartFired = false;
            bool afterModuleStartFired = false;

            bool beforeModuleStopFired = false;
            bool afterModuleStopFired = false;

            this.testee.BeforeModuleStart += delegate { beforeModuleStartFired = true; };
            this.testee.AfterModuleStart += delegate { afterModuleStartFired = true; };

            this.testee.BeforeModuleStop += delegate { beforeModuleStopFired = true; };
            this.testee.AfterModuleStop += delegate { afterModuleStopFired = true; };

            this.testee.Start();
            Assert.True(this.testee.IsAlive, "controller is not alive");

            this.testee.Stop();
            Assert.False(this.testee.IsAlive, "controller is still alive");

            Assert.True(beforeModuleStartFired, "BeforeModuleStart was not fired.");
            Assert.True(afterModuleStartFired, "AfterModuleStart was not fired.");
            Assert.True(beforeModuleStopFired, "BeforeModuleStop was not fired.");
            Assert.True(afterModuleStopFired, "AfterModuleStop was not fired.");
        }

        [Fact]
        public void StartOnAStartedControllerDoesNotCrash()
        {
            this.testee.Start();

            this.testee.BeforeModuleStart += delegate
                                            {
                                                throw new AssertException("no notification allowed.");
                                            };
            this.testee.Start();

            Assert.True(this.testee.IsAlive);
        }

        [Fact]
        public void StopOnAStoppedControllerDoesNotCrash()
        {
            this.testee.AfterModuleStop += delegate
                                            {
                                                throw new AssertException("no notification allowed.");
                                            };

            this.testee.Stop();
        }

        [Fact]
        public void StopAsyncOnAStoppedControllerDoesNotCrash()
        {
            this.testee.AfterModuleStop += delegate
                                            {
                                                throw new AssertException("no notification allowed.");
                                            };

            this.testee.StopAsync();
        }

        [Fact]
        public void MessagesAreConsumed()
        {
            const int NumberOfMessages = 6;

            this.EnqueueAndConsume(NumberOfMessages);
        }

        [Fact]
        public void MessagesAreConsumedMultithreaded()
        {
            this.module = new TestModule();
            this.testee = new ModuleController();
            this.testee.Initialize(this.module, 10);

            this.module.Controller = this.testee;

            const int NumberOfMessages = 1000;

            this.EnqueueAndConsume(NumberOfMessages);
        }

        [Fact]
        public void Stop()
        {
            this.module.ConsumeDelay = TimeSpan.FromMilliseconds(10);
            for (int i = 0; i < 10;  i++)
            {
                this.testee.EnqueueMessage(i);
            }

            // stop module when first message is consumed
            this.testee.BeforeConsumeMessage += delegate
                                               {
                                                   // asynchronously stop controller
                                                   ThreadPool.QueueUserWorkItem(
                                                       delegate
                                                           {
                                                               this.testee.Stop();
                                                           });
                                               };

            AutoResetEvent moduleStopped = new AutoResetEvent(false);
            this.testee.AfterModuleStop += delegate
                                          {
                                              moduleStopped.Set();
                                          };

            this.testee.Start();

            Assert.True(moduleStopped.WaitOne(1000, false), "module not stopped");

            Assert.Equal(9, this.testee.MessageCount);
            Assert.False(this.testee.IsAlive, "still alive");
        }

        [Fact]
        public void WorkerThreadCanNotStopModuleSynchronously()
        {
            AutoResetEvent exceptionThrown = new AutoResetEvent(false);
            this.testee.UnhandledModuleExceptionOccured += delegate
                                                          {
                                                              exceptionThrown.Set();
                                                          };

            this.testee.EnqueueMessage(new StopMessage());
            this.testee.Start();

            Assert.True(exceptionThrown.WaitOne(1000, false), "no exception was thrown");

            this.testee.Stop();
        }

        [Fact]
        public void WorkerThreadCanCallStopAsync()
        {
            AutoResetEvent moduleStopped = new AutoResetEvent(false);
            this.testee.AfterModuleStop += delegate
                                          {
                                              moduleStopped.Set();
                                          };

            this.testee.EnqueueMessage(new StopAsyncMessage());
            this.testee.Start();

            Assert.True(moduleStopped.WaitOne(1000, false), "module was not stopped.");
        }

        [Fact]
        public void ExceptionsInMessageConsumationDoNotCrashModule()
        {
            Exception exception = null;
            AutoResetEvent exceptionThrown = new AutoResetEvent(false);
            this.testee.UnhandledModuleExceptionOccured += delegate(object sender, UnhandledModuleExceptionEventArgs e)
                                                          {
                                                              exception = e.UnhandledException;
                                                              exceptionThrown.Set();
                                                          };

            this.testee.Start();
            this.testee.EnqueueMessage(new ThrowExceptionMessage());

            Assert.True(exceptionThrown.WaitOne(1000, false), "exception was not thrown");
            this.testee.Stop();

            Assert.Equal("test", exception.InnerException.Message);
        }

        [Fact]
        public void ExceptionsInMessageConsumationCanBeHandled()
        {
            this.testee.ConsumeMessageExceptionOccurred += delegate(object sender, ConsumeMessageExceptionEventArgs e)
                                                          {
                                                              e.ExceptionHandled = e.Exception.Message == "test";
                                                          };

            this.testee.Start();

            this.testee.EnqueueMessage(new ThrowExceptionMessage());

            this.testee.Stop();
        }

        [Fact]
        public void MessagesInQueueCanBeCleared()
        {
            this.module.ConsumeDelay = TimeSpan.FromMilliseconds(1);

            this.testee.EnqueueMessage(1);
            this.testee.EnqueueMessage(2);
            this.testee.EnqueueMessage(3);
            this.testee.EnqueueMessage(4);
            this.testee.EnqueueMessage(5);

            object[] messages = this.testee.ClearMessages();

            Assert.Equal(0, this.testee.MessageCount);
            Assert.Equal(5, messages.Length);
        }

        [Fact]
        public void MessagesCanBeSkippedAndAfterConsumeMessageIsAlwaysFired()
        {
            this.testee.BeforeConsumeMessage += delegate(object sender, BeforeConsumeMessageEventArgs e)
                                               {
                                                   // skip all string messages
                                                   e.Cancel = e.Message is string;
                                               };

            AutoResetEvent signal = new AutoResetEvent(false);
            int msgCount = 0;
            this.testee.AfterConsumeMessage += delegate
                                              {
                                                  msgCount++;
                                                  if (msgCount == 5)
                                                  {
                                                      signal.Set();
                                                  }
                                              };

            this.testee.Start();

            this.testee.EnqueueMessage(1);
            this.testee.EnqueueMessage("hello");
            this.testee.EnqueueMessage("world");
            this.testee.EnqueueMessage(2);
            this.testee.EnqueueMessage(3);

            Assert.True(signal.WaitOne(100000, false), "did not receive signal.");

            this.testee.Stop();

            Assert.Equal(3, this.module.Messages.Count);
        }

        [Fact]
        public void PriorityEnqueueMessage()
        {
            this.testee.EnqueueMessage(1);
            this.testee.EnqueueMessage(2);
            this.testee.EnqueuePriorityMessage(0);

            object[] consumedMessages = new object[3];
            int i = 0;
            AutoResetEvent allMessagesConsumed = new AutoResetEvent(false);

            this.testee.AfterConsumeMessage += (sender, e) =>
                                              {
                                                  consumedMessages[i++] = e.Message;

                                                  if (i == 3)
                                                  {
                                                      allMessagesConsumed.Set();
                                                  }
                                              };

            this.testee.Start();

            Assert.True(allMessagesConsumed.WaitOne(1000));
            Assert.Equal(0, consumedMessages[0]);
            Assert.Equal(1, consumedMessages[1]);
            Assert.Equal(2, consumedMessages[2]);
        }

        private void EnqueueAndConsume(int numberOfMessages)
        {
            int beforeEnqueueMessage = 0;
            int afterEnqueueMessage = 0;
            int beforeConsumeMessage = 0;
            int afterConsumeMessage = 0;

            AutoResetEvent allMessagesConsumed = new AutoResetEvent(false);

            this.testee.BeforeEnqueueMessage += delegate
                                                    {
                                                        lock (this)
                                                        {
                                                            beforeEnqueueMessage++;
                                                        }
                                                    };
            this.testee.AfterEnqueueMessage += delegate
                                                   {
                                                       lock (this)
                                                       {
                                                           afterEnqueueMessage++;
                                                       }
                                                   };

            this.testee.BeforeConsumeMessage += delegate
                                                    {
                                                        lock (this)
                                                        {
                                                            beforeConsumeMessage++;
                                                        }
                                                    };
            this.testee.AfterConsumeMessage += delegate
            {
                lock (this)
                {
                    afterConsumeMessage++;
                }

                if (afterConsumeMessage == numberOfMessages)
                {
                    allMessagesConsumed.Set();
                }
            };

            // enqueue messages before start
            for (int i = 0; i < numberOfMessages / 2; i++) 
            {
                this.testee.EnqueueMessage(i);
            }

            Assert.Equal(numberOfMessages / 2, this.testee.Messages.Length);

            this.testee.Start();

            // enqueue messages after start
            for (int i = 0; i < numberOfMessages - (numberOfMessages / 2); i++) 
            {
                this.testee.EnqueueMessage(i);
            }

            Assert.True(allMessagesConsumed.WaitOne(10000, false), "not all messages were consumed. Consumed " + afterConsumeMessage + " messages.");

            Assert.Equal(numberOfMessages, beforeEnqueueMessage);
            Assert.Equal(numberOfMessages, afterEnqueueMessage);
            Assert.Equal(numberOfMessages, beforeConsumeMessage);
            Assert.Equal(numberOfMessages, afterConsumeMessage);
        }
    }
}
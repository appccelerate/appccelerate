//-------------------------------------------------------------------------------
// <copyright file="TestModule.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Threading;

    internal class TestModule
    {
        public TestModule()
        {
            this.ConsumeDelay = TimeSpan.Zero;
            this.Messages = new List<object>();
        }

        public IModuleController Controller { get; set; }

        public List<object> Messages { get; private set; }

        public TimeSpan ConsumeDelay { get; set; }

        [MessageConsumer]
        public void ConsumeInt(int i)
        {
            this.ConsumeMessage(i);
        }

        [MessageConsumer]
        public void ConsumeString(string s)
        {
            this.ConsumeMessage(s);
        }

        [MessageConsumer]
        public void Stop(StopMessage m)
        {
            this.Controller.Stop();
        }

        [MessageConsumer]
        public void StopAsync(StopAsyncMessage m)
        {
            this.Controller.StopAsync();
        }

        [MessageConsumer]
        public void ThrowException(ThrowExceptionMessage m)
        {
            throw new Exception("test");
        }

        private void ConsumeMessage(object msg)
        {
            if (this.ConsumeDelay != TimeSpan.Zero)
            {
                Thread.Sleep(this.ConsumeDelay);
            }

            this.Messages.Add(msg);
        }
    }
}
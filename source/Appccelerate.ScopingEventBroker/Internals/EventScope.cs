//-------------------------------------------------------------------------------
// <copyright file="EventScope.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker.Internals
{
    using System;
    using System.Collections.ObjectModel;

    public class EventScope : IEventScopeInternal
    {
        private readonly Collection<Action> callbacks;

        public EventScope()
        {
            this.callbacks = new Collection<Action>();
        }

        public void Register(Action releaseCallback)
        {
            this.callbacks.Add(releaseCallback);
        }

        public void Release()
        {
            foreach (Action callback in new Collection<Action>(this.callbacks))
            {
                callback();
            }
        }

        public void Cancel()
        {
            this.callbacks.Clear();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Cancel();
            }
        }
    }
}
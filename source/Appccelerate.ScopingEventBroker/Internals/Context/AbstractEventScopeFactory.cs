//-------------------------------------------------------------------------------
// <copyright file="AbstractEventScopeFactory.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker.Internals.Context
{
    using Appccelerate.EventBroker;

    public abstract class AbstractEventScopeFactory : IEventScopeFactory
    {
        private IEventScopeContextInternal scopeContext;

        public virtual IEventScopeInternal CreateScope()
        {
            return new EventScope();
        }

        public virtual IEventScopeContext CreateScopeContext()
        {
            return this.GetOrCreate();
        }

        public virtual IEventScopeHolder CreateScopeHolder()
        {
            return this.GetOrCreate();
        }

        public virtual IHandler CreateHandlerDecorator(IHandler handler)
        {
            return new ScopingHandlerDecorator(handler, this.CreateScopeHolder());
        }

        protected abstract IEventScopeContextInternal CreateScope(IEventScopeFactory eventScopeFactory);

        private IEventScopeContextInternal GetOrCreate()
        {
            return this.scopeContext ?? (this.scopeContext = this.CreateScope(this));
        }
    }
}
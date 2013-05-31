//-------------------------------------------------------------------------------
// <copyright file="DelegateWrapper.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals.Subscriptions
{
    using System;
    using System.Reflection;

    public abstract class DelegateWrapper : IDelegateWrapper
    {
        protected DelegateWrapper(Type eventArgsType, Type eventHandlerType, MethodInfo handlerMethod)
        {
            this.HandlerMethod = handlerMethod;
            this.EventArgsType = eventArgsType;
            this.EventHandlerType = eventHandlerType;
        }

        public Type EventArgsType { get; set; }

        public Type EventHandlerType { get; set; }

        public MethodInfo HandlerMethod { get; private set; }

        public abstract void Invoke(object subscriber, object sender, EventArgs e);

        protected Delegate CreateSubscriptionDelegate(object subscriber)
        {
            return Delegate.CreateDelegate(this.EventHandlerType, subscriber, this.HandlerMethod);
        }
    }
}
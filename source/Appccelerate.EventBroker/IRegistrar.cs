//-------------------------------------------------------------------------------
// <copyright file="IRegistrar.cs" company="Appccelerate">
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

    using Appccelerate.EventBroker.Matchers;

    public interface IRegistrar
    {
        void AddPublication(string topic, object publisher, string eventName, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers);

        void RemovePublication(string topic, object publisher, string eventName);

        void AddSubscription(string topic, object subscriber, EventHandler handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers);

        void AddSubscription<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers) where TEventArgs : EventArgs;

        void AddSubscription(string topic, object subscriber, Action<EventArgs> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers);

        void AddSubscription<TEventArgValue>(string topic, object subscriber, Action<TEventArgValue> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers);
        
        void AddSubscription(string topic, object subscriber, Action handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers);

        void RemoveSubscription(string topic, object subscriber, EventHandler handlerMethod);

        void RemoveSubscription<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod) where TEventArgs : EventArgs;

        void RemoveSubscription(string topic, object subscriber, Action<EventArgs> handlerMethod);

        void RemoveSubscription(string topic, object subscriber, Action handlerMethod);

        void RemoveSubscription<TEventArgValue>(string topic, object subscriber, Action<TEventArgValue> handlerMethod);
    }
}
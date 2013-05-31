//-------------------------------------------------------------------------------
// <copyright file="IEventRegistrar.cs" company="Appccelerate">
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
    using Matchers;

    /// <summary>
    /// This interface is passed to the registered publishers and subscribers so that they can register publications
    /// and subscriptions by code.
    /// </summary>
    public interface IEventRegistrar : IRegistrar
    {
        void AddPublication(string topic, object publisher, ref EventHandler publishedEvent, params IPublicationMatcher[] matchers);

        void AddPublication(string topic, object publisher, ref EventHandler publishedEvent, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers);

        void AddPublication<TEventArgs>(string topic, object publisher, ref EventHandler<TEventArgs> publishedEvent, params IPublicationMatcher[] matchers) where TEventArgs : EventArgs;

        void AddPublication<TEventArgs>(string topic, object publisher, ref EventHandler<TEventArgs> publishedEvent, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers) where TEventArgs : EventArgs;

        void RemovePublication(string topic, object publisher, ref EventHandler publishedEvent);

        void RemovePublication<TEventArgs>(string topic, object publisher, ref EventHandler<TEventArgs> publishedEvent) where TEventArgs : EventArgs;

        void Register(object item);

        void Unregister(object item);
    }
}
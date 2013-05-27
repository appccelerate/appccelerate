//-------------------------------------------------------------------------------
// <copyright file="IHandler.cs" company="Appccelerate">
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
    using System.Reflection;

    using Appccelerate.EventBroker.Internals.Subscriptions;

    /// <summary>
    /// A handler defines how a subscription is executed (on which thread, sync, asynchronous, ...).
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Gets the kind of the handler, whether it is a synchronous or asynchronous handler.
        /// </summary>
        /// <value>The kind of the handler (synchronous or asynchronous).</value>
        HandlerKind Kind { get; }

        /// <summary>
        /// Initializes the handler.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">Name of the handler method on the subscriber.</param>
        /// <param name="extensionHost">Provides access to all registered extensions.</param>
        void Initialize(object subscriber, MethodInfo handlerMethod, IExtensionHost extensionHost);

        void Handle(IEventTopicInfo eventTopic, object subscriber, object sender, EventArgs e, IDelegateWrapper delegateWrapper);
    }
}
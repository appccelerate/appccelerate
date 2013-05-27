//-------------------------------------------------------------------------------
// <copyright file="IEventBrokerRegisterable.cs" company="Appccelerate">
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
    /// <summary>
    /// Publishers and subscribers implementing this interface or notified by a call to
    /// <see cref="Register"/> that they have been registered and a call to <see cref="Unregister"/> that they have been unregistered.
    /// </summary>
    public interface IEventBrokerRegisterable
    {
        /// <summary>
        /// The publisher or subscriber can register additional publications and subscriptions
        /// on the <paramref name="eventRegistrar"/>.
        /// </summary>
        /// <param name="eventRegistrar">The event registerer to register publications and subscriptions.</param>
        void Register(IEventRegistrar eventRegistrar);

        /// <summary>
        /// The publisher or subscribe has to clean-up all registrations made in call to <see cref="Register"/>.
        /// </summary>
        /// <param name="eventRegistrar">The event registerer.</param>
        void Unregister(IEventRegistrar eventRegistrar);
    }
}
//-------------------------------------------------------------------------------
// <copyright file="EventBrokerExtensionsForDistribution.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker
{
    using Appccelerate.EventBroker;

    /// <summary>
    /// Contains extension methods which simplify adding distributed extensions.
    /// </summary>
    public static class EventBrokerExtensionsForDistribution
    {
        /// <summary>
        /// Adds the distributed extension to the provided event broker.
        /// </summary>
        /// <typeparam name="TDistributedExtension">The type of the distributed extension.</typeparam>
        /// <param name="eventBroker">The event broker.</param>
        /// <param name="extension">The extension.</param>
        public static void AddDistributedExtension<TDistributedExtension>(this IEventBroker eventBroker, TDistributedExtension extension)
            where TDistributedExtension : IDistributedEventBrokerExtension
        {
            Ensure.ArgumentNotNull(eventBroker, "eventBroker");

            eventBroker.AddExtension(extension);
            extension.Manage(eventBroker);
        }

        /// <summary>
        /// Adds the distributed extension to the provided event broker.
        /// </summary>
        /// <typeparam name="TDistributedExtension">The type of the distributed extension.</typeparam>
        /// <param name="eventBroker">The event broker.</param>
        /// <param name="extension">The extension.</param>
        /// <param name="eventBrokerIdentification">The event broker identification.</param>
        public static void AddDistributedExtension<TDistributedExtension>(this IEventBroker eventBroker, TDistributedExtension extension, string eventBrokerIdentification)
            where TDistributedExtension : IDistributedEventBrokerExtension
        {
            Ensure.ArgumentNotNull(eventBroker, "eventBroker");

            eventBroker.AddExtension(extension);
            extension.Manage(eventBroker, eventBrokerIdentification);
        }
    }
}
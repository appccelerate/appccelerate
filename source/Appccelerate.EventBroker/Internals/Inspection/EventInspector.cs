//-------------------------------------------------------------------------------
// <copyright file="EventInspector.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Appccelerate.EventBroker.Internals.Exceptions;

    /// <summary>
    /// The <see cref="EventInspector"/> scans classes for publications or subscriptions.
    /// </summary>
    public class EventInspector : IEventInspector
    {
        private readonly IExtensionHost extensionHost;

        public EventInspector(IExtensionHost extensionHost)
        {
            this.extensionHost = extensionHost;
        }

        public ScanResult Scan(object instance)
        {
            var publications = ScanForPublications(instance);
            var subscriptions = ScanForSubscriptions(instance);

            this.extensionHost.ForEach(extension => extension.ScannedInstanceForPublicationsAndSubscriptions(instance, publications, subscriptions));

            return new ScanResult(publications, subscriptions);
        }

        public EventInfo ScanPublisherForEvent(object publisher, string eventName)
        {
            Ensure.ArgumentNotNull(publisher, "publisher");
            Ensure.ArgumentNotNullOrEmpty(eventName, "eventName");

            EventInfo eventInfo = publisher.GetType().GetEvent(eventName);

            CheckEventInfoFound(publisher, eventName, eventInfo);

            return eventInfo;
        }

        private static IEnumerable<PropertyPublicationScanResult> ScanForPublications(object instance)
        {
            var eventInfos = new List<EventInfo>();
            eventInfos.AddRange(instance.GetType().GetEvents());
            foreach (Type interfaceType in instance.GetType().GetInterfaces())
            {
                eventInfos.AddRange(interfaceType.GetEvents());
            }

            var publications = from eventInfo in eventInfos
                               from EventPublicationAttribute attr in
                                   eventInfo.GetCustomAttributes(typeof(EventPublicationAttribute), true)
                               select new PropertyPublicationScanResult(attr.Topic, eventInfo, attr.HandlerRestriction, attr.MatcherTypes);
            
            return publications.ToList();
        }

        private static IEnumerable<PropertySubscriptionScanResult> ScanForSubscriptions(object instance)
        {
            var methodInfos = new List<MethodInfo>();
            methodInfos.AddRange(instance.GetType().GetMethods());
            foreach (Type interfaceType in instance.GetType().GetInterfaces())
            {
                methodInfos.AddRange(interfaceType.GetMethods());
            }

            var subscriptions = from methodInfo in methodInfos
                                from EventSubscriptionAttribute attr in
                                    methodInfo.GetCustomAttributes(typeof(EventSubscriptionAttribute), true)
                                select new PropertySubscriptionScanResult(attr.Topic, methodInfo, attr.HandlerType, attr.MatcherTypes);

            return subscriptions.ToList();
        }

        private static void CheckEventInfoFound(object publisher, string eventName, EventInfo eventInfo)
        {
            if (eventInfo == null)
            {
                throw new PublisherEventNotFoundException(publisher.GetType(), eventName);
            }
        }
    }
}
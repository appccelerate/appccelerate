//-------------------------------------------------------------------------------
// <copyright file="PropertyPublicationScanResult.cs" company="Appccelerate">
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
    using System.Reflection;

    public class PropertyPublicationScanResult
    {
        public PropertyPublicationScanResult(
            string topic, 
            EventInfo @event, 
            HandlerRestriction handlerRestriction, 
            IEnumerable<Type> publicationMatcherTypes)
        {
            this.Topic = topic;
            this.Event = @event;
            this.HandlerRestriction = handlerRestriction;
            this.PublicationMatcherTypes = publicationMatcherTypes;
        }

        public string Topic { get; private set; }

        public EventInfo Event { get; private set; }
        
        public HandlerRestriction HandlerRestriction { get; private set; }
        
        public IEnumerable<Type> PublicationMatcherTypes { get; private set; }
    }
}
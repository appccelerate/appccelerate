//-------------------------------------------------------------------------------
// <copyright file="IMappingEventBrokerExtension.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker
{
    using System;

    using Appccelerate.EventBroker;

    /// <summary>
    /// Interface which defines an auto mapper event broker extension.
    /// </summary>
    public interface IMappingEventBrokerExtension : IEventBrokerExtension, IManageEventBroker
    {
        /// <summary>
        /// Sets the missing mapping action which is called when no mapping was previously defined.
        /// </summary>
        /// <param name="action">The missing mapping action.</param>
        void SetMissingMappingAction(Action<IMissingMappingContext> action);
    }
}
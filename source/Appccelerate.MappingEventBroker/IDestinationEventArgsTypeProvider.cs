//-------------------------------------------------------------------------------
// <copyright file="IDestinationEventArgsTypeProvider.cs" company="Appccelerate">
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

    /// <summary>
    /// The destination event argument types provider is responsible to return
    /// destination event arguments for a given destination topic.
    /// </summary>
    /// <remarks>When the destination event argument type is not found an null
    /// is returned instead the mapping process must be skipped.</remarks>
    public interface IDestinationEventArgsTypeProvider
    {
        /// <summary>
        /// Gets the destination event argument type for the given destination
        /// topic URI or <see langword="null"/> if nothing found.
        /// </summary>
        /// <param name="destinationTopic">The destination topic.</param>
        /// <param name="sourceEventArgsType">Type of the source event argument.</param>
        /// <returns>
        /// The destination event argument type or <see langword="null"/>.
        /// </returns>
        Type GetDestinationEventArgsType(string destinationTopic, Type sourceEventArgsType);
    }
}
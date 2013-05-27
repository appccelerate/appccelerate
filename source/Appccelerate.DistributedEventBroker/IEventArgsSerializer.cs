//-------------------------------------------------------------------------------
// <copyright file="IEventArgsSerializer.cs" company="Appccelerate">
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
    using System;

    /// <summary>
    /// Defines an serializer which can serialize/deserialize event arguments into a meaningful string representation.
    /// </summary>
    public interface IEventArgsSerializer
    {
        /// <summary>
        /// Serializes the specified event argument into a string representation.
        /// </summary>
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <returns>The string representation of the specified event argument.</returns>
        string Serialize(EventArgs eventArgs);

        /// <summary>
        /// Deserialize the specified event args type from its string representation.
        /// </summary>
        /// <param name="eventArgsType">Type of the event args.</param>
        /// <param name="eventArgs">The event args as string.</param>
        /// <returns>The deserialized event argument.</returns>
        EventArgs Deserialize(Type eventArgsType, string eventArgs);
    }
}
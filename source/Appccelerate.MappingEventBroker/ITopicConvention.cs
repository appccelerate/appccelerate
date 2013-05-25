//-------------------------------------------------------------------------------
// <copyright file="ITopicConvention.cs" company="Appccelerate">
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
    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals;

    /// <summary>
    /// Interface for topic conventions.
    /// </summary>
    public interface ITopicConvention
    {
        /// <summary>
        /// Determines whether the specified event topic is a candidate to process.
        /// </summary>
        /// <remarks>The mapped side must also be included!</remarks>
        /// <param name="eventTopic">The event topic.</param>
        /// <returns>
        /// <c>true</c> if the specified event topic is candidate; otherwise, <c>false</c>.
        /// </returns>
        bool IsCandidate(IEventTopicInfo eventTopic);

        /// <summary>
        /// Maps the topic from the source format to the destination format.
        /// </summary>
        /// <param name="topic">The source topic.</param>
        /// <returns>The mapped topic.</returns>
        string MapTopic(string topic);
    }
}
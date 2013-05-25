//-------------------------------------------------------------------------------
// <copyright file="FuncTopicConvention.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker.Conventions
{
    using System;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals;

    /// <summary>
    /// This topic convention implementation allows to use lambda expressions for dynamic topic convention.
    /// </summary>
    public class FuncTopicConvention : ITopicConvention
    {
        private readonly Func<string, string> topicMapper;

        private readonly Func<IEventTopicInfo, bool> candidateSelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncTopicConvention"/> class.
        /// </summary>
        /// <param name="topicMapper">The topic mapper.</param>
        public FuncTopicConvention(Func<string, string> topicMapper)
            : this(topic => true, topicMapper)
        {   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncTopicConvention"/> class.
        /// </summary>
        /// <param name="candidateSelector">The candidate selector.</param>
        /// <param name="topicMapper">The topic mapper.</param>
        public FuncTopicConvention(Func<IEventTopicInfo, bool> candidateSelector, Func<string, string> topicMapper)
        {
            this.candidateSelector = candidateSelector;
            this.topicMapper = topicMapper;
        }

        /// <summary>
        /// Determines whether the specified event topic is a candidate to process.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <returns>
        /// <c>true</c> if the specified event topic is candidate; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>The mapped side must also be included!</remarks>
        public bool IsCandidate(IEventTopicInfo eventTopic)
        {
            return this.candidateSelector(eventTopic);
        }

        /// <summary>
        /// Maps the topic from the source format to the destination format.
        /// </summary>
        /// <param name="topic">The source topic URI.</param>
        /// <returns>The mapped topic URI.</returns>
        public string MapTopic(string topic)
        {
            return this.topicMapper(topic);
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="FuncTopicSelectionStrategy.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.Strategies
{
    using System;

    using Appccelerate.EventBroker;

    using EventBroker.Internals;

    /// <summary>
    /// Provides callback mechanism for topic selection strategies.
    /// </summary>
    public class FuncTopicSelectionStrategy : ITopicSelectionStrategy
    {
        private readonly Func<IEventTopicInfo, bool> strategy;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="FuncTopicSelectionStrategy"/> class.
        /// </summary>
        /// <param name="strategy">The strategy delegate which is called when 
        /// <see cref="SelectTopic"/> is called.</param>
        public FuncTopicSelectionStrategy(Func<IEventTopicInfo, bool> strategy)
        {
            this.strategy = strategy;
        }

        /// <summary>
        /// Determines whether the current topic shall be tracked by the distributed event broker.
        /// </summary>
        /// <param name="topic">The event topic.</param>
        /// <returns>
        /// <see langword="true"/> if the topic shall be selected; otherwise <see langword="false"/>.
        /// </returns>
        public bool SelectTopic(IEventTopicInfo topic)
        {
            return this.strategy(topic);
        }
    }
}
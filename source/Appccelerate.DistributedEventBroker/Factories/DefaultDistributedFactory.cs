//-------------------------------------------------------------------------------
// <copyright file="DefaultDistributedFactory.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.Factories
{
    using Serializer;
    using Strategies;

    /// <summary>
    /// Default factory which creates the default types for the distributed event broker extension.
    /// </summary>
    public class DefaultDistributedFactory : IDistributedFactory
    {
        /// <summary>
        /// Creates the message factory.
        /// </summary>
        /// <returns>
        /// A new instance of an <see cref="IEventMessageFactory"/>.
        /// </returns>
        public virtual IEventMessageFactory CreateMessageFactory()
        {
            return new DefaultEventMessageFactory();
        }

        /// <summary>
        /// Creates the event arguments serializer.
        /// </summary>
        /// <returns>
        /// A new instance of an <see cref="IEventArgsSerializer"/>
        /// </returns>
        public virtual IEventArgsSerializer CreateEventArgsSerializer()
        {
            return new BinaryEventArgsSerializer();
        }

        /// <summary>
        /// Create the topic selection strategy.
        /// </summary>
        /// <returns>
        /// A new instance of an <see cref="ITopicSelectionStrategy"/>.
        /// </returns>
        public virtual ITopicSelectionStrategy CreateTopicSelectionStrategy()
        {
            return new DefaultTopicSelectionStrategy();
        }
    }
}
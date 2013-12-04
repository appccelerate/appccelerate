//-------------------------------------------------------------------------------
// <copyright file="AutoMapperEventBrokerExtension.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker.AutoMapper
{
    using Appccelerate.MappingEventBroker.Conventions;

    /// <summary>
    /// This extension allows to dynamically remap topics based on a convention
    /// from one event argument type to another event argument type using 
    /// <see cref="AutoMapperMapper"/>.
    /// <code>
    ///    public class Publisher
    ///    {
    ///        [EventPublication(@"topic://Original")]
    ///        public event EventHandler Event;
    ///        private void InvokeEvent(EventArgs e)
    ///        {
    ///            EventHandler handler = Event;
    ///            if (handler != null) handler(this, e);
    ///        }
    ///        public void Publish()
    ///        {
    ///            this.InvokeEvent(EventArgs.Empty);
    ///        }
    ///    }
    ///    public class SubscriberOriginal
    ///    {
    ///        [EventSubscription(@"topic://Original", typeof(Appccelerate.EventBroker.Handlers.OnPublisher))]
    ///        public void HandleOriginal(object sender, EventArgs e)
    ///        {
    ///        }
    ///    }
    ///    public class SubscriberMapped
    ///    {
    ///        [EventSubscription(@"mapped://Original", typeof(Appccelerate.EventBroker.Handlers.OnPublisher))]
    ///        public void HandleOriginal(object sender, CancelEventArgs e)
    ///        {
    ///        }
    ///    }
    /// </code>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Currently I have no better naming!")]
    public class AutoMapperEventBrokerExtension : MappingEventBrokerExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperEventBrokerExtension"/> class.
        /// </summary>
        /// <param name="typeProvider">The destination event argument type provider.</param>
        /// <remarks>
        /// Uses the <see cref="DefaultTopicConvention"/>.
        /// </remarks>
        public AutoMapperEventBrokerExtension(IDestinationEventArgsTypeProvider typeProvider)
            : this(new AutoMapperMapper(), typeProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperEventBrokerExtension"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="typeProvider">The destination event argument type provider.</param>
        public AutoMapperEventBrokerExtension(IMapper mapper, IDestinationEventArgsTypeProvider typeProvider)
            : base(mapper, typeProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperEventBrokerExtension"/> class.
        /// </summary>
        /// <param name="topicConvention">The topic convention which overrides the default behavior.</param>
        /// <param name="typeProvider">The destination event argument type provider.</param>
        public AutoMapperEventBrokerExtension(ITopicConvention topicConvention, IDestinationEventArgsTypeProvider typeProvider)
            : base(new AutoMapperMapper(), topicConvention, typeProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperEventBrokerExtension"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="topicConvention">The topic convention which overrides the default behavior.</param>
        /// <param name="typeProvider">The destination event argument type provider.</param>
        public AutoMapperEventBrokerExtension(IMapper mapper, ITopicConvention topicConvention, IDestinationEventArgsTypeProvider typeProvider)
            : base(mapper, topicConvention, typeProvider)
        {
        }
    }
}

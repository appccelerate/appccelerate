//-------------------------------------------------------------------------------
// <copyright file="EventTopicException.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals.Exceptions
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Runtime.Serialization;

    using Appccelerate.EventBroker.Exceptions;

    /// <summary>
    /// An <see cref="EventBrokerException"/> thrown by the <see cref="IEventTopic"/> when exceptions occurs
    /// on its subscriptions during a firing sequence.
    /// </summary>
    [Serializable]
    public class EventTopicException : EventBrokerException
    {
        /// <summary>
        /// The list of all exceptions that occurred on all subscription handlers.
        /// </summary>
        private readonly ReadOnlyCollection<Exception> exceptions;

        /// <summary>
        /// Link to the event topic that was fired.
        /// </summary>
        private readonly IEventTopic topic;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopicException"/> class.
        /// </summary>
        public EventTopicException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopicException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EventTopicException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopicException"/> class with a specified error message 
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, 
        /// or a null reference if no inner exception is specified.</param>
        public EventTopicException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopicException"/> class with the specified list of exceptions.
        /// </summary>
        /// <param name="topic">The <see cref="IEventTopic"/> instance whose subscribers incurred into an exception.</param>
        /// <param name="exceptions">The list of exceptions that occurred during the subscribers invocation.</param>
        public EventTopicException(IEventTopic topic, ReadOnlyCollection<Exception> exceptions)
            : base(string.Format(CultureInfo.InvariantCulture, "Exceptions occurred while firing the topic '{0}'.", topic != null ? topic.Uri : string.Empty))
        {
            this.topic = topic;
            this.exceptions = exceptions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopicException"/> class.
        /// </summary>
        /// <param name="topic">The <see cref="IEventTopic"/> instance whose subscribers incurred into an exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public EventTopicException(IEventTopic topic, Exception innerException)
            : base(string.Format(CultureInfo.InvariantCulture, "An exception occurred while firing the topic '{0}'.", topic.Uri), innerException)
        {
            this.topic = topic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopicException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected EventTopicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the list of exceptions that occurred during the subscribers invocation.
        /// </summary>
        public ReadOnlyCollection<Exception> Exceptions
        {
            get { return this.exceptions; }
        }

        /// <summary>
        /// Gets the <see cref="IEventTopic"/> which incurred into errors.
        /// </summary>
        public IEventTopic Topic
        {
            get { return this.topic; }
        }
    }
}

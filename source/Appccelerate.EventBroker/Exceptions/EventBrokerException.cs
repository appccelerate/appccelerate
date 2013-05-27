//-------------------------------------------------------------------------------
// <copyright file="EventBrokerException.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Exceptions
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// Base exception for all exceptions that are thrown within the event broker framework.
    /// </summary>
    public class EventBrokerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventBrokerException"/> class.
        /// </summary>
        public EventBrokerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBrokerException"/> class.
        /// </summary>
        /// <param name="format">Exception message as a format string.</param>
        /// <param name="args">Argument for the exception message format string.</param>
        public EventBrokerException(string format, params object[] args)
            : base(string.Format(CultureInfo.InvariantCulture, format, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBrokerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EventBrokerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBrokerException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected EventBrokerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

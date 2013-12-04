//-------------------------------------------------------------------------------
// <copyright file="BootstrapperException.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Bootstrapper exception.
    /// </summary>
    [Serializable]
    public class BootstrapperException : AggregateException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperException"/> class.
        /// </summary>
        public BootstrapperException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BootstrapperException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BootstrapperException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperException"/> class.
        /// </summary>
        /// <param name="innerExceptions">The inner exceptions.</param>
        public BootstrapperException(IEnumerable<Exception> innerExceptions)
            : base(innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperException"/> class.
        /// </summary>
        /// <param name="innerExceptions">The inner exceptions.</param>
        public BootstrapperException(params Exception[] innerExceptions)
            : base(innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerExceptions">The inner exceptions.</param>
        public BootstrapperException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerExceptions">The inner exceptions.</param>
        public BootstrapperException(string message, params Exception[] innerExceptions)
            : base(message, innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> argument is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The exception could not be deserialized correctly.</exception>
        protected BootstrapperException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
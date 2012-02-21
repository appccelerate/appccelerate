//-------------------------------------------------------------------------------
// <copyright file="AsyncWorkerException.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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

namespace Appccelerate.Async
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is thrown if an error occurs in the asynchronous operation and there is no completed event handler.
    /// </summary>
    [Serializable]
    public class AsyncWorkerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorkerException"/> class.
        /// </summary>
        public AsyncWorkerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorkerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AsyncWorkerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorkerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AsyncWorkerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorkerException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected AsyncWorkerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
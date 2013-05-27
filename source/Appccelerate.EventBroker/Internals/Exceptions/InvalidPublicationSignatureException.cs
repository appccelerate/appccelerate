//-------------------------------------------------------------------------------
// <copyright file="InvalidPublicationSignatureException.cs" company="Appccelerate">
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
    using System.Reflection;

    using Appccelerate.EventBroker.Exceptions;

    /// <summary>
    /// An <see cref="EventBrokerException"/> thrown when an invalid publication signature is found while registering a publisher.
    /// </summary>
    public class InvalidPublicationSignatureException : EventBrokerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPublicationSignatureException"/> class.
        /// </summary>
        /// <param name="eventInfo">The event info.</param>
        public InvalidPublicationSignatureException(EventInfo eventInfo)
            : base("Invalid publisher signature: '{0}.{1}'", eventInfo.DeclaringType.FullName, eventInfo.Name)
        {
        }
    }
}

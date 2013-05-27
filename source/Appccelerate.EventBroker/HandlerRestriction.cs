//-------------------------------------------------------------------------------
// <copyright file="HandlerRestriction.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker
{
    /// <summary>
    /// Defines restrictions the publisher makes on how the event has to be handled (synchronously or asynchronously).
    /// </summary>
    public enum HandlerRestriction
    {
        /// <summary>
        /// Synchronous and asynchronous handling is allowed.
        /// </summary>
        None = -1,

        /// <summary>
        /// Only synchronous handling is allowed, e.g. the publisher wants to evaluate the event arguments afterwards.
        /// </summary>
        Synchronous = HandlerKind.Synchronous,

        /// <summary>
        /// Only asynchronous handling is allowed. The publisher does not want to be blocked.
        /// </summary>
        Asynchronous = HandlerKind.Asynchronous,
    }
}
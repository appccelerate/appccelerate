//-------------------------------------------------------------------------------
// <copyright file="HandlerKind.cs" company="Appccelerate">
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
    /// Defines the way the subscription handler handles events (synchronously or asynchronously).
    /// </summary>
    public enum HandlerKind
    {
        /// <summary>
        /// Synchronous handling. The event invoker is blocked until event is handled. 
        /// The event invoker can evaluate the event arguments on return.
        /// </summary>
        Synchronous = 0,

        /// <summary>
        /// Asynchronous handling. The event invoker is not blocked until event is handled. 
        /// The event invoker must not evaluate the event arguments on return.
        /// </summary>
        Asynchronous = 1,
    }
}
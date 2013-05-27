//-------------------------------------------------------------------------------
// <copyright file="EventTopics.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Sample
{
    /// <summary>
    /// Contains commonly used event topics.
    /// </summary>
    public static class EventTopics
    {
        /// <summary>
        /// Event topic for ping from UI thread
        /// </summary>
        public const string PingUIFromUIThread = "topic://EventBrokerSample.PingUIFromUIThread";

        /// <summary>
        /// Event topic for pong from UI thread
        /// </summary>
        public const string PongUIFromUIThread = "topic://EventBrokerSample.PongUIFromUIThread";

        /// <summary>
        /// Event topic for ping from UI asynchronous.
        /// </summary>
        public const string PingUIFromAsync = "topic://EventBrokerSample.PingUIFromAsync";

        /// <summary>
        /// Event topic for pong from UI async.
        /// </summary>
        public const string PongUIFromAsync = "topic://EventBrokerSample.PongUIFromAsync";

        public const string BurstPing = "topic://EventBrokerSample.BurstPing";

        public const string BurstPong = "topic://EventBrokerSample.BurstPong";
    }
}

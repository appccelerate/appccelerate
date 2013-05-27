//-------------------------------------------------------------------------------
// <copyright file="EventTopicFireDelegate.cs" company="Appccelerate">
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
    using System;

    using Appccelerate.EventBroker.Internals;

    /// <summary>
    /// Represents the signature for the subscription objects to get called from the <see cref="EventTopic"/> during
    /// a firing sequence.
    /// </summary>
    /// <param name="eventTopic">The event topic that is fired.</param>
    /// <param name="sender">The publisher object firing the topic.</param>
    /// <param name="e">The <see cref="EventArgs"/> data to be passed to the subscribers.</param>
    /// <param name="publication">The publication firing the event.</param>
    public delegate void EventTopicFireDelegate(IEventTopicInfo eventTopic, object sender, EventArgs e, IPublication publication);
}
//-------------------------------------------------------------------------------
// <copyright file="NotAlreadyCanceled.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Matchers
{
    using System;
    using System.ComponentModel;
    using System.IO;

    /// <summary>
    /// Subscription matcher that matches only if the event arguments are <see cref="CancelEventArgs"/> and
    /// the event arguments are not yet canceled.
    /// </summary>
    public class NotAlreadyCanceled : ISubscriptionMatcher
    {
        /// <summary>
        /// Returns whether the publication and subscription match and the event published by the
        /// publisher will be relayed to the subscriber.
        /// <para>
        /// Matches if the event arguments are <see cref="CancelEventArgs"/> that are not yet canceled.
        /// </para>
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <returns><code>true</code> if event has to be sent to the subscriber.</returns>
        public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
        {
            var cancelEventArgs = e as CancelEventArgs;
            return cancelEventArgs != null && !cancelEventArgs.Cancel;
        }

        /// <summary>
        /// Describes this subscription matcher.
        /// </summary>
        /// <param name="writer">The writer the description is written to.</param>
        public void DescribeTo(TextWriter writer)
        {
            Ensure.ArgumentNotNull(writer, "writer");

            writer.Write("event arguments (CancelEventArgs) are not yet canceled");
        }
    }
}
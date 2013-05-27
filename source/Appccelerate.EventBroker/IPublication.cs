//-------------------------------------------------------------------------------
// <copyright file="IPublication.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.IO;

    using Appccelerate.EventBroker.Internals.Publications;
    using Appccelerate.EventBroker.Matchers;

    /// <summary>
    /// Defines a publication. Either a real <see cref="Publication"/> or a <see cref="SpontaneousPublication"/>.
    /// </summary>
    public interface IPublication : IDisposable
    {
        /// <summary>
        /// Gets the publisher of the event.
        /// </summary>
        object Publisher { get; }

        /// <summary>
        /// Gets the publication matchers.
        /// </summary>
        /// <value>The publication matchers.</value>
        IList<IPublicationMatcher> PublicationMatchers
        {
            get;
        }

        /// <summary>
        /// Gets the subscriber handler restriction.
        /// </summary>
        /// <value>The subscriber handler restriction.</value>
        HandlerRestriction HandlerRestriction { get; }

        /// <summary>
        /// Gets the name of the event on the <see cref="Publication.Publisher"/>.
        /// </summary>
        string EventName { get; }

        /// <summary>
        /// Gets the type of the event arguments.
        /// </summary>
        /// <value>The type of the event arguments.</value>
        Type EventArgsType { get; }

        /// <summary>
        /// Describes this publication
        /// name, scope, event handler.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void DescribeTo(TextWriter writer);
    }
}
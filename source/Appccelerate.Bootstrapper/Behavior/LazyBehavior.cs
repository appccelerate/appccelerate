//-------------------------------------------------------------------------------
// <copyright file="LazyBehavior.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Behavior
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;

    using Appccelerate.Formatters;

    /// <summary>
    /// The lazy behavior is responsible for creating a behavior at the time of the behave call.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class LazyBehavior<TExtension> : IBehavior<TExtension>
        where TExtension : IExtension
    {
        private readonly Func<IBehavior<TExtension>> behaviorProvider;
        private readonly Expression<Func<IBehavior<TExtension>>> behaviorProviderExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyBehavior&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <param name="behaviorProvider">The behavior provider.</param>
        public LazyBehavior(Expression<Func<IBehavior<TExtension>>> behaviorProvider)
        {
            Ensure.ArgumentNotNull(behaviorProvider, "behaviorProvider");

            this.behaviorProviderExpression = behaviorProvider;
            this.behaviorProvider = this.behaviorProviderExpression.Compile();
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.GetType().FullNameToString();
            }
        }

        /// <inheritdoc />
        /// <remarks>Creates the behavior with the specified behavior provider and executes behave on the lazy initialized behavior.</remarks>
        public void Behave(IEnumerable<TExtension> extensions)
        {
            IBehavior<TExtension> behavior = this.behaviorProvider();

            behavior.Behave(extensions);
        }

        /// <inheritdoc />
        public string Describe()
        {
            return string.Format(
                CultureInfo.InvariantCulture, "Creates the behavior with {0} and executes behave on the lazy initialized behavior.", this.behaviorProviderExpression);
        }
    }
}
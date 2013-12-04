//-------------------------------------------------------------------------------
// <copyright file="Behavior.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Specification.Dummies
{
    using System.Collections.Generic;
    using System.Globalization;

    using Appccelerate.Formatters;

    public class Behavior : IBehavior<ICustomExtension>
    {
        private readonly string access;

        /// <summary>
        /// Initializes a new instance of the <see cref="Behavior"/> class.
        /// </summary>
        /// <param name="access">The access.</param>
        public Behavior(string access)
        {
            this.access = access;
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
        public void Behave(IEnumerable<ICustomExtension> extensions)
        {
            foreach (ICustomExtension extension in extensions)
            {
                extension.Dump(this.access);
            }
        }

        /// <inheritdoc />
        public string Describe()
        {
            return string.Format(CultureInfo.InvariantCulture, "Dumps \"{0}\" on all extensions.", this.access);
        }
    }
}
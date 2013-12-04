//-------------------------------------------------------------------------------
// <copyright file="BehaviorWithStringContext.cs" company="Appccelerate">
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
    using System.Linq;

    using Appccelerate.Formatters;

    public class BehaviorWithStringContext : IBehavior<ICustomExtension>
    {
        private readonly string addition;

        private string input;

        public BehaviorWithStringContext(string input, string addition)
        {
            this.addition = addition;
            this.input = input;
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
            extensions.ToList().ForEach(e => e.Dump(string.Format(CultureInfo.InvariantCulture, "input modification with {0}", this.addition)));

            this.input += " " + this.addition;
        }

        /// <inheritdoc />
        public string Describe()
        {
            return string.Format(CultureInfo.InvariantCulture, "Dumps \"{0}\" on all extensions.", this.addition);
        }
    }
}
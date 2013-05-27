//-------------------------------------------------------------------------------
// <copyright file="ExtensionContext.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Reporting
{
    /// <summary>
    /// Extension context implementation which release the IDescribable right after creation.
    /// </summary>
    public class ExtensionContext : IExtensionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionContext"/> class.
        /// </summary>
        /// <param name="describable">The describable.</param>
        public ExtensionContext(IDescribable describable)
        {
            Ensure.ArgumentNotNull(describable, "describable");

            this.Name = describable.Name;
            this.Description = describable.Describe();
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public string Description { get; private set; }
    }
}
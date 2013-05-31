//-------------------------------------------------------------------------------
// <copyright file="ComplexExtensionBase.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Complex
{
    using System.Collections.Generic;
    using Appccelerate.Formatters;
    using Funq;

    /// <summary>
    /// Base class for complex extensions.
    /// </summary>
    public abstract class ComplexExtensionBase : IComplexExtension
    {
        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.GetType().FullNameToString();
            }
        }

        /// <summary>
        /// Gets the container which is registered when the extension point ContainerInitialized is called.
        /// </summary>
        protected Container Container
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public virtual void Start()
        {
        }

        /// <inheritdoc />
        public virtual void ContainerInitializing(ICollection<IFunqlet> funqlets)
        {
        }

        /// <inheritdoc />
        public virtual void ContainerInitialized(Container container)
        {
            this.Container = container;
        }

        /// <inheritdoc />
        public virtual void Ready()
        {
        }

        /// <inheritdoc />
        public virtual void Shutdown()
        {
        }

        /// <inheritdoc />
        public abstract string Describe();
    }
}
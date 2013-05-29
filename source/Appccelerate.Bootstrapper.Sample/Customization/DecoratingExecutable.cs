//-------------------------------------------------------------------------------
// <copyright file="DecoratingExecutable.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Customization
{
    using System;
    using System.Collections.Generic;

    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Bootstrapper.Syntax;

    /// <summary>
    /// Decorates an executable and outputs additional information on the console.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class DecoratingExecutable<TExtension> : IExecutable<TExtension>
        where TExtension : IExtension
    {
        private readonly IExecutable<TExtension> decoratedExecutable;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecoratingExecutable&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <param name="decoratedExecutable">The decorated executable.</param>
        public DecoratingExecutable(IExecutable<TExtension> decoratedExecutable)
        {
            this.decoratedExecutable = decoratedExecutable;
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.decoratedExecutable.Name;
            }
        }

        /// <inheritdoc />
        public void Add(IBehavior<TExtension> behavior)
        {
            Console.WriteLine("::: Adding behavior {0}", behavior.GetType().Name);

            this.decoratedExecutable.Add(behavior);

            Console.WriteLine("::: Added behavior {0}", behavior.GetType().Name);
        }

        /// <inheritdoc />
        public void Execute(IEnumerable<TExtension> extensions, IExecutableContext executableContext)
        {
            Ensure.ArgumentNotNull(executableContext, "executableContext");

            Console.WriteLine("::: Executing executable {0}", executableContext.Name);

            this.decoratedExecutable.Execute(extensions, executableContext);

            Console.WriteLine("::: Executed executable {0}", executableContext.Name);
        }

        /// <inheritdoc />
        public string Describe()
        {
            return this.decoratedExecutable.Describe();
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="DependencyUsingBehavior.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Complex.Behaviors
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    using Appccelerate.Formatters;

    using log4net;

    /// <summary>
    /// Behavior which uses the IDependency.
    /// </summary>
    public class DependencyUsingBehavior : IBehavior<IComplexExtension>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDependency dependency;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyUsingBehavior"/> class.
        /// </summary>
        /// <param name="dependency">The dependency.</param>
        public DependencyUsingBehavior(IDependency dependency)
        {
            this.dependency = dependency;
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
        public void Behave(IEnumerable<IComplexExtension> extensions)
        {
            Log.Info(" - DependencyUsingBehavior is behaving.");

            this.dependency.Hello();
        }

        /// <inheritdoc />
        public string Describe()
        {
            return string.Format(CultureInfo.InvariantCulture, "Behaves on \"{0}\" during bootstrapping.", this.dependency);
        }
    }
}
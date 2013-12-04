//-------------------------------------------------------------------------------
// <copyright file="ExtensionWhichNeedsDependency.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Complex.Extensions
{
    using System.Reflection;

    using Funq;

    using log4net;

    /// <summary>
    /// Extension which uses IDependency.
    /// </summary>
    public class ExtensionWhichNeedsDependency : ComplexExtensionBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IDependency Dependency { get; set; }

        /// <inheritdoc />
        public override void ContainerInitialized(Container container)
        {
            base.ContainerInitialized(container);

            Log.Info("ExtensionWhichNeedsDependency is using the initialized container.");

            this.Dependency = this.Container.Resolve<IDependency>();
        }

        /// <inheritdoc />
        public override void Ready()
        {
            base.Ready();

            Log.Info("ExtensionWhichNeedsDependency uses dependency in ready.");

            this.Dependency.Hello();
        }

        /// <inheritdoc />
        public override void Shutdown()
        {
            base.Shutdown();

            Log.Info("ExtensionWhichNeedsDependency uses dependency in shutdown.");

            this.Dependency.Goodbye();
        }

        /// <inheritdoc />
        public override string Describe()
        {
            return "Extension which needs a custom dependency";
        }
    }
}
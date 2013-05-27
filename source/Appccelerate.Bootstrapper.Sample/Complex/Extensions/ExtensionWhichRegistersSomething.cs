//-------------------------------------------------------------------------------
// <copyright file="ExtensionWhichRegistersSomething.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Reflection;

    using Funq;

    using log4net;

    /// <summary>
    /// Extension which registers CustomFunqlet
    /// </summary>
    public class ExtensionWhichRegistersSomething : ComplexExtensionBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <inheritdoc />
        public override void ContainerInitializing(ICollection<IFunqlet> funqlets)
        {
            Ensure.ArgumentNotNull(funqlets, "funqlets");

            base.ContainerInitializing(funqlets);

            Log.Info("ExtensionWhichRegistersSomething is initializing the container.");

            funqlets.Add(new CustomFunqlet());
        }

        /// <inheritdoc />
        public override string Describe()
        {
            return "Extension which registers something on the container";
        }
    }
}
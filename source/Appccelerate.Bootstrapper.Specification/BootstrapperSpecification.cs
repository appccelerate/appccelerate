//-------------------------------------------------------------------------------
// <copyright file="BootstrapperSpecification.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Specification
{
    using Appccelerate.Bootstrapper.Specification.Dummies;

    using Machine.Specifications;

    public class BootstrapperSpecification
    {
        protected const string Concern = "Bootstrapping";

        protected static CustomExtensionStrategy Strategy;

        protected static CustomExtensionBase First;

        protected static CustomExtensionBase Second;

        protected static IBootstrapper<ICustomExtension> Bootstrapper;

        Establish context = () =>
        {
            Bootstrapper = new DefaultBootstrapper<ICustomExtension>();

            Strategy = new CustomExtensionStrategy();
            First = new FirstExtension();
            Second = new SecondExtension();
        };
    }
}
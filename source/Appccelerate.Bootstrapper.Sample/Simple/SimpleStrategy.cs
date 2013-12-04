//-------------------------------------------------------------------------------
// <copyright file="SimpleStrategy.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Simple
{
    using Appccelerate.Bootstrapper.Behavior;
    using Appccelerate.Bootstrapper.Syntax;

    /// <summary>
    /// Strategy which tells the bootstrapper how to boot up the simple scenario.
    /// </summary>
    public class SimpleStrategy : AbstractStrategy<ISimpleExtension>
    {
        /// <inheritdoc />
        protected override void DefineRunSyntax(ISyntaxBuilder<ISimpleExtension> builder)
        {
            builder
                .Execute(x => x.Start());
        }

        /// <inheritdoc />
        protected override void DefineShutdownSyntax(ISyntaxBuilder<ISimpleExtension> builder)
        {
            builder.Execute(x => x.Shutdown())
                .End.With(new DisposeExtensionBehavior());
        }
    }
}
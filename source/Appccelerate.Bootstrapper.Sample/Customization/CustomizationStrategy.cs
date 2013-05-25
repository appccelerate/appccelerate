//-------------------------------------------------------------------------------
// <copyright file="CustomizationStrategy.cs" company="Appccelerate">
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
    using Appccelerate.Bootstrapper.Sample.Complex;
    using Appccelerate.Bootstrapper.Syntax;

    /// <summary>
    /// Strategy which inherits from <see cref="ComplexStrategy"/> but customizes the core infrastructure
    /// </summary>
    public class CustomizationStrategy : ComplexStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomizationStrategy"/> class.
        /// </summary>
        public CustomizationStrategy()
            : base(new SyntaxBuilder<IComplexExtension>(new DecoratingExecutableFactory<IComplexExtension>()), new SyntaxBuilder<IComplexExtension>(new DecoratingExecutableFactory<IComplexExtension>()))
        {
        }

        /// <inheritdoc />
        /// <remarks>Creates a <see cref="CustomExtensionResolver"/></remarks>
        public override IExtensionResolver<IComplexExtension> CreateExtensionResolver()
        {
            return new CustomExtensionResolver();
        }

        /// <inheritdoc />
        /// <remarks>Creates a <see cref="AsynchronousRunExecutor"/></remarks>
        public override IExecutor<IComplexExtension> CreateRunExecutor()
        {
            return new AsynchronousRunExecutor();
        }

        /// <inheritdoc />
        /// <remarks>Creates a <see cref="AsynchronousShutdownExecutor"/></remarks>
        public override IExecutor<IComplexExtension> CreateShutdownExecutor()
        {
            return new AsynchronousShutdownExecutor();
        }
    }
}
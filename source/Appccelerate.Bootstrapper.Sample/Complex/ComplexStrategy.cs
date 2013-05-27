//-------------------------------------------------------------------------------
// <copyright file="ComplexStrategy.cs" company="Appccelerate">
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
    using System;
    using System.Collections.ObjectModel;

    using Appccelerate.Bootstrapper.Behavior;
    using Appccelerate.Bootstrapper.Configuration;
    using Appccelerate.Bootstrapper.Sample.Complex.Behaviors;
    using Appccelerate.Bootstrapper.Syntax;

    using Funq;

    /// <summary>
    /// Strategy which tells the bootstrapper how to boot up the complex scenario.
    /// </summary>
    public class ComplexStrategy : AbstractStrategy<IComplexExtension>
    {
        private readonly Collection<IFunqlet> funqlets;

        private readonly Lazy<Container> container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexStrategy"/> class.
        /// </summary>
        public ComplexStrategy()
        {
            this.funqlets = new Collection<IFunqlet>();
            this.container = new Lazy<Container>(() => new Container());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexStrategy"/> class.
        /// </summary>
        /// <param name="runSyntaxBuilder">The run syntax builder.</param>
        /// <param name="shutdownSyntaxBuilder">The shutdown syntax builder.</param>
        protected ComplexStrategy(ISyntaxBuilder<IComplexExtension> runSyntaxBuilder, ISyntaxBuilder<IComplexExtension> shutdownSyntaxBuilder)
            : base(runSyntaxBuilder, shutdownSyntaxBuilder)
        {
            this.funqlets = new Collection<IFunqlet>();
            this.container = new Lazy<Container>(() => new Container());
        }

        /// <inheritdoc />
        protected override void DefineRunSyntax(ISyntaxBuilder<IComplexExtension> builder)
        {
            builder
                .Begin
                    .With(new ConfigurationSectionBehavior())
                    .With(new ExtensionConfigurationSectionBehavior())
                .Execute(e => e.Start())
                .Execute(() => this.funqlets, (e, ctx) => e.ContainerInitializing(ctx))
                    .With(flts => new FunqletProvidingBehavior(flts))
                .Execute(() => this.BuildContainer(), (e, ctx) => e.ContainerInitialized(ctx))
                    .With(ctx => ctx.Resolve<IBehavior<IComplexExtension>>())
                .Execute(e => e.Ready());
        }

        /// <inheritdoc />
        protected override void DefineShutdownSyntax(ISyntaxBuilder<IComplexExtension> builder)
        {
            builder
                .Execute(e => e.Shutdown())
                .End.With(new DisposeExtensionBehavior());
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && !this.IsDisposed)
            {
                if (this.container.IsValueCreated)
                {
                    this.container.Value.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private Container BuildContainer()
        {
            var lazyContainer = this.container.Value;

            foreach (IFunqlet funqlet in this.funqlets)
            {
                funqlet.Configure(lazyContainer);
            }

            return lazyContainer;
        }
    }
}
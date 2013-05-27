//-------------------------------------------------------------------------------
// <copyright file="DefaultBootstrapper.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper
{
    using System;

    using Appccelerate.Bootstrapper.Extension;
    using Appccelerate.Bootstrapper.Reporting;

    /// <summary>
    /// The bootstrapper.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class DefaultBootstrapper<TExtension> : IBootstrapper<TExtension>
        where TExtension : IExtension
    {
        private readonly IExtensionHost<TExtension> extensionHost;

        private readonly IReporter reporter;

        private IStrategy<TExtension> strategy;

        private IReportingContext reportingContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBootstrapper{TExtension}"/> class.
        /// </summary>
        public DefaultBootstrapper()
            : this(new ExtensionHost<TExtension>(), new NullReporter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBootstrapper&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <param name="reporter">The bootstrapping process reporter.</param>
        public DefaultBootstrapper(IReporter reporter)
            : this(new ExtensionHost<TExtension>(), reporter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBootstrapper&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <param name="extensionHost">The extension host.</param>
        public DefaultBootstrapper(IExtensionHost<TExtension> extensionHost)
            : this(extensionHost, new NullReporter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBootstrapper{TExtension}"/> class.
        /// </summary>
        /// <param name="extensionHost">The extension host.</param>
        /// <param name="reporter">The bootstrapping process reporter.</param>
        public DefaultBootstrapper(IExtensionHost<TExtension> extensionHost, IReporter reporter)
        {
            this.extensionHost = extensionHost;
            this.reporter = reporter;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DefaultBootstrapper{TExtension}"/> class.
        /// </summary>
        ~DefaultBootstrapper()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        protected bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public void AddExtension(TExtension extension)
        {
            this.CheckIsInitialized();

            this.reportingContext.CreateExtensionContext(extension);

            this.extensionHost.AddExtension(extension);
        }

        /// <inheritdoc />
        public void Initialize(IStrategy<TExtension> strategy)
        {
            Ensure.ArgumentNotNull(strategy, "strategy");

            this.CheckAlreadyInitialized();

            this.strategy = strategy;
            this.reportingContext = this.strategy.CreateReportingContext();
        }

        /// <inheritdoc />
        public void Run()
        {
            this.CheckIsInitialized();

            var extensionResolver = this.strategy.CreateExtensionResolver();
            extensionResolver.Resolve(this);

            var syntax = this.strategy.BuildRunSyntax();

            IExecutor<TExtension> runExecutor = this.strategy.CreateRunExecutor();
            IExecutionContext runExecutionContext = this.reportingContext.CreateRunExecutionContext(runExecutor);

            runExecutor.Execute(syntax, this.extensionHost.Extensions, runExecutionContext);
        }

        /// <inheritdoc />
        public void Shutdown()
        {
            this.CheckIsInitialized();

            this.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed && disposing)
            {
                var syntax = this.strategy.BuildShutdownSyntax();

                IExecutor<TExtension> shutdownExecutor = this.strategy.CreateShutdownExecutor();
                IExecutionContext shutdownExecutionContext = this.reportingContext.CreateShutdownExecutionContext(shutdownExecutor);

                shutdownExecutor.Execute(syntax, this.extensionHost.Extensions, shutdownExecutionContext);

                this.reporter.Report(this.reportingContext);

                this.strategy.Dispose();

                this.IsDisposed = true;
            }
        }

        private void CheckIsInitialized()
        {
            if (this.strategy == null)
            {
                throw new InvalidOperationException("Bootstrapper must be initialized before run or shutdown.");
            }
        }

        private void CheckAlreadyInitialized()
        {
            if (this.strategy != null)
            {
                throw new InvalidOperationException("Bootstrapper can only be initialized once.");
            }
        }
    }
}
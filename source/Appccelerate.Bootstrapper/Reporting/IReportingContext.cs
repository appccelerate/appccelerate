//-------------------------------------------------------------------------------
// <copyright file="IReportingContext.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Reporting
{
    using System.Collections.Generic;

    using Appccelerate.Bootstrapper.Syntax;

    /// <summary>
    /// Marks the implementing class as reporting context.
    /// </summary>
    public interface IReportingContext : IRunExecutionContextFactory, IShutdownExecutionContextFactory, IExtensionContextFactory
    {
        /// <summary>
        /// Gets information about the run phase of the bootstrapping process.
        /// </summary>
        IExecutionContext Run { get; }

        /// <summary>
        /// Gets information about the shutdown phase of the bootstrapping process.
        /// </summary>
        IExecutionContext Shutdown { get; }

        /// <summary>
        /// Gets information about all extensions which participate in the bootstrapping process.
        /// </summary>
        IEnumerable<IExtensionContext> Extensions { get; }
    }

    /// <summary>
    /// The executable context factory is responsible to create an <see cref="IExecutableContext"/>
    /// and track the created executable context on the current <see cref="IExecutionContext"/>.
    /// </summary>
    public interface IExecutableContextFactory
    {
        /// <summary>
        /// Creates a new <see cref="IExecutableContext"/> and tracks it on the current <see cref="IExecutionContext"/>.
        /// </summary>
        /// <param name="describable">The describable.</param>
        /// <returns>A newly created executable context.</returns>
        IExecutableContext CreateExecutableContext(IDescribable describable);
    }

    /// <summary>
    /// The run execution context factory is responsible to create an <see cref="IExecutionContext"/>
    /// and track the created run execution context on the current <see cref="IReportingContext"/>.
    /// </summary>
    public interface IRunExecutionContextFactory
    {
        /// <summary>
        /// Creates a new <see cref="IExecutionContext"/> and tracks it on the current <see cref="IReportingContext"/>.
        /// </summary>
        /// <param name="describable">The describable.</param>
        /// <returns>A newly created run execution context.</returns>
        IExecutionContext CreateRunExecutionContext(IDescribable describable);
    }

    /// <summary>
    /// The shutdown execution context factory is responsible to create an <see cref="IExecutionContext"/>
    /// and track the created shutdown execution context on the current <see cref="IReportingContext"/>.
    /// </summary>
    public interface IShutdownExecutionContextFactory
    {
        /// <summary>
        /// Creates a new <see cref="IExecutionContext"/> and tracks it on the current <see cref="IReportingContext"/>.
        /// </summary>
        /// <param name="describable">The describable.</param>
        /// <returns>A newly created shutdown execution context.</returns>
        IExecutionContext CreateShutdownExecutionContext(IDescribable describable);
    }

    /// <summary>
    /// The extension context factory is responsible to create an <see cref="IExtensionContext"/>
    /// and track the created extension context on the current <see cref="IReportingContext"/>.
    /// </summary>
    public interface IExtensionContextFactory
    {
        /// <summary>
        /// Creates a new <see cref="IExtensionContext"/> and tracks it on the current <see cref="IReportingContext"/>.
        /// </summary>
        /// <param name="describable">The describable.</param>
        /// <returns>A newly created extension context.</returns>
        IExtensionContext CreateExtensionContext(IDescribable describable);
    }

    /// <summary>
    /// The extension context describes an <see cref="IExtension"/>
    /// </summary>
    public interface IExtensionContext
    {
        /// <summary>
        /// Gets the name of the extension.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the extension.
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// The execution context describes an <see cref="IExecutor{TExtension}"/>
    /// </summary>
    public interface IExecutionContext : IExecutableContextFactory
    {
        /// <summary>
        /// Gets the name of the executor.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the executor.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets information about all executables which belong the executor.
        /// </summary>
        IEnumerable<IExecutableContext> Executables { get; }
    }

    /// <summary>
    /// The executable context describes an <see cref="IExecutable{TExtension}"/>
    /// </summary>
    public interface IExecutableContext : IBehaviorContextFactory
    {
        /// <summary>
        /// Gets the name of the executable.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the executable.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets information about all behaviors which belong the executable.
        /// </summary>
        IEnumerable<IBehaviorContext> Behaviors { get; }
    }

    /// <summary>
    /// The behavior context factory is responsible to create an <see cref="IBehaviorContext"/>
    /// and track the created behavior context on the current <see cref="IExecutableContext"/>.
    /// </summary>
    public interface IBehaviorContextFactory
    {
        /// <summary>
        /// Creates a new <see cref="IBehaviorContext"/> and tracks it on the current <see cref="IExecutableContext"/>.
        /// </summary>
        /// <param name="describable">The describable.</param>
        /// <returns>A newly created behavior context.</returns>
        IBehaviorContext CreateBehaviorContext(IDescribable describable);
    }

    /// <summary>
    /// The behavior context describes an IBehavior.
    /// </summary>
    public interface IBehaviorContext
    {
        /// <summary>
        /// Gets the name of the behavior.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the behavior.
        /// </summary>
        string Description { get; }
    }
}
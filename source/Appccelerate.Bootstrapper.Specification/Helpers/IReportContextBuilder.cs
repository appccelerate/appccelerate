//-------------------------------------------------------------------------------
// <copyright file="IReportContextBuilder.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Specification.Helpers
{
    using Appccelerate.Bootstrapper.Reporting;

    public interface IReportContextBuilder : IRunBuilder, IExtensionBuilder
    {
    }

    public interface IBuilder
    {
        IReportingContext Build();
    }

    public interface IExtensionBuilder
    {
        IReportContextBuilder Extension(string name, string description);
    }

    public interface IRunBuilder
    {
        IExecutableBuilder Run(string name, string description);
    }

    public interface IExecutableBuilder : IShutdownBuilder, IBuilder
    {
        IBehaviorBuilder Executable(string name, string description);
    }

    public interface IBehaviorBuilder : IExecutableBuilder
    {
        IBehaviorBuilder Behavior(string name, string description);
    }

    public interface IShutdownBuilder
    {
        IExecutableBuilder Shutdown(string name, string description);
    }
}
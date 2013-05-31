//-------------------------------------------------------------------------------
// <copyright file="ReportingContextBuilder.cs" company="Appccelerate">
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

    public class ReportingContextBuilder : IReportContextBuilder, IBehaviorBuilder
    {
        private readonly ReportingContext reportingContext;

        private IExecutionContext currentExecutionContext;

        private IExecutableContext currentExecutableContext;

        private IExecutionContext shutdownExecutionContext;

        private ReportingContextBuilder()
        {
            this.reportingContext = new ReportingContext();
        }

        public static IReportContextBuilder Create()
        {
            return new ReportingContextBuilder();
        }

        public IReportContextBuilder Extension(string name, string description)
        {
            this.reportingContext.CreateExtensionContext(new Describable(name, description));

            return this;
        }

        public IExecutableBuilder Run(string name, string description)
        {
            this.currentExecutionContext = this.reportingContext.CreateRunExecutionContext(new Describable(name, description));

            return this;
        }

        public IExecutableBuilder Shutdown(string name, string description)
        {
            this.currentExecutionContext = this.reportingContext.CreateShutdownExecutionContext(new Describable(name, description));

            return this;
        }

        public IBehaviorBuilder Executable(string name, string description)
        {
            this.currentExecutableContext = this.currentExecutionContext.CreateExecutableContext(new Describable(name, description));

            return this;
        }

        public IBehaviorBuilder Behavior(string name, string description)
        {
            this.currentExecutableContext.CreateBehaviorContext(new Describable(name, description));

            return this;
        }

        public IReportingContext Build()
        {
            return this.reportingContext;
        }

        private class Describable : IDescribable
        {
            private readonly string description;

            public Describable(string name, string description)
            {
                this.description = description;
                this.Name = name;
            }

            public string Name { get; private set; }

            public string Describe()
            {
                return this.description;
            }
        }
    }
}
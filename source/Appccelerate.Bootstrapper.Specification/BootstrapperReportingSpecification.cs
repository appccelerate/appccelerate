//-------------------------------------------------------------------------------
// <copyright file="BootstrapperReportingSpecification.cs" company="Appccelerate">
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
    using System;
    using System.Collections.ObjectModel;

    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Bootstrapper.Specification.Dummies;
    using Machine.Specifications;

    public class BootstrapperReportingSpecification
    {
        protected const string Concern = "Bootstrapper reporting";

        protected static CustomExtensionWithBehaviorStrategy Strategy;

        protected static CustomExtensionBase First;

        protected static CustomExtensionBase Second;

        protected static IBootstrapper<ICustomExtension> Bootstrapper;

        protected static IReportingContext ReportingContext;

        private static ReporterCollection Reporters;

        Establish context = () =>
        {
            Reporters = new ReporterCollection();

            Bootstrapper = new DefaultBootstrapper<ICustomExtension>(Reporters);

            Strategy = new CustomExtensionWithBehaviorStrategy();
            First = new FirstExtension();
            Second = new SecondExtension();

            RegisterReporter(new InterceptingReporter(ctx => ReportingContext = ctx));
        };

        protected static void RegisterReporter(IReporter reporter)
        {
            Reporters.Add(reporter);
        }

        private class ReporterCollection : Collection<IReporter>, IReporter
        {
            public void Report(IReportingContext context)
            {
                foreach (IReporter reporter in this.Items)
                {
                    reporter.Report(context);
                }
            }
        }

        private class InterceptingReporter : IReporter
        {
            private readonly Action<IReportingContext> contextInterceptor;

            public InterceptingReporter(Action<IReportingContext> contextInterceptor)
            {
                this.contextInterceptor = contextInterceptor;
            }

            public void Report(IReportingContext context)
            {
                this.contextInterceptor(context);
            }
        }
    }
}
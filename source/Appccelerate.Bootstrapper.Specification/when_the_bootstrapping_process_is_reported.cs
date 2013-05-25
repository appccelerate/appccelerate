//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapping_process_is_reported.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Bootstrapper.Specification.Helpers;
    using FluentAssertions;
    using Machine.Specifications;
    using Machine.Specifications.Runner.Impl;

    [Subject(Concern)]
    public class when_the_bootstrapping_process_is_reported : BootstrapperReportingSpecification
    {
        private static StringReporter ExpectedContextReporter;
        private static StringReporter InterceptingContextReporter;

        Establish context = () =>
        {
            ExpectedContextReporter = new StringReporter();
            InterceptingContextReporter = new StringReporter();

            Bootstrapper.Initialize(Strategy);
            Bootstrapper.AddExtension(First);
            Bootstrapper.AddExtension(Second);

            RegisterReporter(InterceptingContextReporter);
        };

        Because of = () =>
        {
            Bootstrapper.Run();
            Bootstrapper.Shutdown();
            Bootstrapper.Dispose();
        };

        It should_report_names_and_descriptions_of_all_extensions_including_executables_with_behaviors_attached_to_it_and_run_and_shutdown_executors = () =>
            {
                const string ActionExecutableCustomExtension = "Appccelerate.Bootstrapper.Syntax.Executables.ActionExecutable<Appccelerate.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string ActionOnExtensionExecutableCustomExtension = "Appccelerate.Bootstrapper.Syntax.Executables.ActionOnExtensionExecutable<Appccelerate.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string ActionExecutableWithDictionaryContextCustomExtension = "Appccelerate.Bootstrapper.Syntax.Executables.ActionOnExtensionWithInitializerExecutable<System.Collections.Generic.IDictionary<System.String,System.String>,Appccelerate.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string ActionExecutableWithStringContextCustomExtension = "Appccelerate.Bootstrapper.Syntax.Executables.ActionOnExtensionWithInitializerExecutable<System.String,Appccelerate.Bootstrapper.Specification.Dummies.ICustomExtension>";

                const string BehaviorCustomExtension = "Appccelerate.Bootstrapper.Specification.Dummies.Behavior";
                const string LazyBehaviorCustomExtension = "Appccelerate.Bootstrapper.Behavior.LazyBehavior<Appccelerate.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string BehaviorWithConfigurationContextCustomExtension = "Appccelerate.Bootstrapper.Specification.Dummies.BehaviorWithConfigurationContext";
                const string BehaviorWithStringContextCustomExtension = "Appccelerate.Bootstrapper.Specification.Dummies.BehaviorWithStringContext";

                var expectedContext = ReportingContextBuilder.Create()
                    .Extension("Appccelerate.Bootstrapper.Specification.Dummies.FirstExtension", "First Extension")
                    .Extension("Appccelerate.Bootstrapper.Specification.Dummies.SecondExtension", "Second Extension")
                    .Run("Appccelerate.Bootstrapper.Execution.SynchronousExecutor<Appccelerate.Bootstrapper.Specification.Dummies.ICustomExtension>", "Runs all executables synchronously on the extensions in the order which they were added.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.BeginWith)\" during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, BehaviorCustomExtensionDescriptionWith("run first beginning"))
                            .Behavior(LazyBehaviorCustomExtension, LazyBehaviorCustomExtensionDescriptionWith("() => new Behavior(\"run second beginning\")"))
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => DumpAction(\"CustomRun\")\" during bootstrapping.")
                        .Executable(ActionOnExtensionExecutableCustomExtension, "Executes \"extension => extension.Start()\" on each extension during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, BehaviorCustomExtensionDescriptionWith("run first start"))
                            .Behavior(LazyBehaviorCustomExtension, LazyBehaviorCustomExtensionDescriptionWith("() => new Behavior(\"run second start\")"))
                        .Executable(ActionExecutableWithDictionaryContextCustomExtension, "Initializes the context once with \"() => value(Appccelerate.Bootstrapper.Specification.Dummies.CustomExtensionWithBehaviorStrategy).RunInitializeConfiguration()\" and executes \"(extension, dictionary) => extension.Configure(dictionary)\" on each extension during bootstrapping.")
                            .Behavior(BehaviorWithConfigurationContextCustomExtension, BehaviorWithConfigurationContextCustomExtensionDescriptionWith("RunFirstValue", "RunTestValue"))
                            .Behavior(BehaviorWithConfigurationContextCustomExtension, BehaviorWithConfigurationContextCustomExtensionDescriptionWith("RunSecondValue", "RunTestValue"))
                        .Executable(ActionOnExtensionExecutableCustomExtension, "Executes \"extension => extension.Initialize()\" on each extension during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, BehaviorCustomExtensionDescriptionWith("run first initialize"))
                            .Behavior(LazyBehaviorCustomExtension, LazyBehaviorCustomExtensionDescriptionWith("() => new Behavior(\"run second initialize\")"))
                        .Executable(ActionExecutableWithStringContextCustomExtension, "Initializes the context once with \"() => \"RunTest\"\" and executes \"(extension, context) => extension.Register(context)\" on each extension during bootstrapping.")
                            .Behavior(BehaviorWithStringContextCustomExtension, BehaviorWithStringContextCustomExtensionDescriptionWith("RunTestValueFirst"))
                            .Behavior(BehaviorWithStringContextCustomExtension, BehaviorWithStringContextCustomExtensionDescriptionWith("RunTestValueSecond"))
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.EndWith)\" during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, BehaviorCustomExtensionDescriptionWith("run first end"))
                            .Behavior(LazyBehaviorCustomExtension, LazyBehaviorCustomExtensionDescriptionWith("() => new Behavior(\"run second end\")"))
                    .Shutdown("Appccelerate.Bootstrapper.Execution.SynchronousReverseExecutor<Appccelerate.Bootstrapper.Specification.Dummies.ICustomExtension>", "Runs all executables synchronously on the extensions in the reverse order which they were added.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.BeginWith)\" during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, BehaviorCustomExtensionDescriptionWith("shutdown first beginning"))
                            .Behavior(LazyBehaviorCustomExtension, LazyBehaviorCustomExtensionDescriptionWith("() => new Behavior(\"shutdown second beginning\")"))
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => DumpAction(\"CustomShutdown\")\" during bootstrapping.")
                        .Executable(ActionExecutableWithStringContextCustomExtension, "Initializes the context once with \"() => \"ShutdownTest\"\" and executes \"(extension, ctx) => extension.Unregister(ctx)\" on each extension during bootstrapping.")
                            .Behavior(BehaviorWithStringContextCustomExtension, BehaviorWithStringContextCustomExtensionDescriptionWith("ShutdownTestValueFirst"))
                            .Behavior(BehaviorWithStringContextCustomExtension, BehaviorWithStringContextCustomExtensionDescriptionWith("ShutdownTestValueSecond"))
                        .Executable(ActionExecutableWithDictionaryContextCustomExtension, "Initializes the context once with \"() => value(Appccelerate.Bootstrapper.Specification.Dummies.CustomExtensionWithBehaviorStrategy).ShutdownInitializeConfiguration()\" and executes \"(extension, dictionary) => extension.DeConfigure(dictionary)\" on each extension during bootstrapping.")
                            .Behavior(BehaviorWithConfigurationContextCustomExtension, BehaviorWithConfigurationContextCustomExtensionDescriptionWith("ShutdownFirstValue", "ShutdownTestValue"))
                            .Behavior(BehaviorWithConfigurationContextCustomExtension, BehaviorWithConfigurationContextCustomExtensionDescriptionWith("ShutdownSecondValue", "ShutdownTestValue"))
                        .Executable(ActionOnExtensionExecutableCustomExtension, "Executes \"extension => extension.Stop()\" on each extension during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, BehaviorCustomExtensionDescriptionWith("shutdown first stop"))
                            .Behavior(LazyBehaviorCustomExtension, LazyBehaviorCustomExtensionDescriptionWith("() => new Behavior(\"shutdown second stop\")"))
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.EndWith)\" during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, BehaviorCustomExtensionDescriptionWith("shutdown first end"))
                            .Behavior(LazyBehaviorCustomExtension, LazyBehaviorCustomExtensionDescriptionWith("() => new Behavior(\"shutdown second end\")"))
                            .Behavior("Appccelerate.Bootstrapper.Behavior.DisposeExtensionBehavior", "Disposes all extensions which implement IDisposable.")
                    .Build();

                ExpectedContextReporter.Report(expectedContext);

                InterceptingContextReporter.ToString().Should().Be(ExpectedContextReporter.ToString());
            };

        private static string LazyBehaviorCustomExtensionDescriptionWith(string action)
        {
            const string LazyBehaviorDescriptionFormat = "Creates the behavior with {0} and executes behave on the lazy initialized behavior.";

            return string.Format(CultureInfo.InvariantCulture, LazyBehaviorDescriptionFormat, action);
        }

        private static string BehaviorCustomExtensionDescriptionWith(string action)
        {
            const string BehaviorDescription = "Dumps \"{0}\" on all extensions.";

            return string.Format(CultureInfo.InvariantCulture, BehaviorDescription, action);
        }

        private static string BehaviorWithConfigurationContextCustomExtensionDescriptionWith(string key, string value)
        {
            const string BehaviorWithConfigurationContextCustomExtensionDescription = "Dumps the key \"{0}\" and value \"{1}\" and modifies the configuration with it.";

            return string.Format(CultureInfo.InvariantCulture, BehaviorWithConfigurationContextCustomExtensionDescription, key, value);
        }

        private static string BehaviorWithStringContextCustomExtensionDescriptionWith(string value)
        {
            const string BehaviorWithStringContextCustomExtensionDescription = "Dumps \"{0}\" on all extensions.";

            return string.Format(CultureInfo.InvariantCulture, BehaviorWithStringContextCustomExtensionDescription, value);
        }

        private class StringReporter : IReporter
        {
            private IReportingContext context;

            public void Report(IReportingContext context)
            {
                this.context = context;
            }

            public override string ToString()
            {
                return Dump(this.context);
            }

            private static string Dump(IReportingContext context)
            {
                var builder = new StringBuilder();

                context.Extensions.ToList().ForEach(e => Dump(e.Name, e.Description, builder, 0));

                Dump(context.Run, builder);
                Dump(context.Shutdown, builder);

                return builder.ToString();
            }

            private static void Dump(IExecutionContext executionContext, StringBuilder sb)
            {
                Dump(executionContext.Name, executionContext.Description, sb, 3);

                Dump(executionContext.Executables, sb);
            }

            private static void Dump(IEnumerable<IExecutableContext> executableContexts, StringBuilder sb)
            {
                foreach (IExecutableContext executableContext in executableContexts)
                {
                    Dump(executableContext.Name, executableContext.Description, sb, 6);

                    executableContext.Behaviors.ToList().ForEach(b => Dump(b.Name, b.Description, sb, 9));
                }
            }

            private static void Dump(string name, string description, StringBuilder sb, int indent)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}[Name = {1}, Description = {2}]", string.Empty.PadLeft(indent), name, description));
            }
        }
    }
}
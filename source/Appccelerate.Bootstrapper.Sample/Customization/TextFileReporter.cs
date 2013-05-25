//-------------------------------------------------------------------------------
// <copyright file="TextFileReporter.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Appccelerate.Bootstrapper.Reporting;

    /// <summary>
    /// Reports to a text file.
    /// </summary>
    public class TextFileReporter : IReporter
    {
        private readonly StringReporter decoratedReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFileReporter"/> class.
        /// </summary>
        public TextFileReporter()
        {
            this.decoratedReporter = new StringReporter();
        }

        /// <inheritdoc />
        public void Report(IReportingContext context)
        {
            this.decoratedReporter.Report(context);

            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "CustomizationReport.txt"), this.decoratedReporter.ToString());
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
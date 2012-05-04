//-------------------------------------------------------------------------------
// <copyright file="EnvironmentLogExtension.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using Appccelerate.IO.Access;

    using log4net;

    public class EnvironmentLogExtension : EnvironmentExtensionBase
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentLogExtension"/> class.
        /// </summary>
        public EnvironmentLogExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EnvironmentLogExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EnvironmentLogExtension(ILog logger)
        {
            this.log = logger;
        }

        public override void BeginExit(int exitCode)
        {
            base.BeginExit(exitCode);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Exiting with code {0}.", exitCode);
        }

        public override void EndExit(int exitCode)
        {
            base.EndExit(exitCode);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Exited with code {0}.", exitCode);
        }

        public override void FailExit(ref Exception exception)
        {
            base.FailExit(ref exception);
        }

        public override void BeginExpandEnvironmentVariables(string name)
        {
            base.BeginExpandEnvironmentVariables(name);
        }

        public override void EndExpandEnvironmentVariables(string result, string name)
        {
            base.EndExpandEnvironmentVariables(result, name);
        }

        public override void FailExpandEnvironmentVariables(ref Exception exception)
        {
            base.FailExpandEnvironmentVariables(ref exception);
        }

        public override void BeginFailFast(string message, Exception exception)
        {
            base.BeginFailFast(message, exception);
        }

        public override void EndFailFast(string message, Exception exception)
        {
            base.EndFailFast(message, exception);
        }

        public override void FailFailFast(ref Exception exception)
        {
            base.FailFailFast(ref exception);
        }

        public override void BeginFailFast(string message)
        {
            base.BeginFailFast(message);
        }

        public override void EndFailFast(string message)
        {
            base.EndFailFast(message);
        }

        public override void BeginGetCommandLineArgs()
        {
            base.BeginGetCommandLineArgs();
        }

        public override void EndGetCommandLineArgs(string[] result)
        {
            base.EndGetCommandLineArgs(result);
        }

        public override void FailGetCommandLineArgs(ref Exception exception)
        {
            base.FailGetCommandLineArgs(ref exception);
        }
    }
}
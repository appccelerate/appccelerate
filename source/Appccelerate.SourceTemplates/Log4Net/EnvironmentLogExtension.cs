//-------------------------------------------------------------------------------
// <copyright file="EnvironmentLogExtension.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Collections;
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

            this.log.Error("Exception occurred while exiting.", exception);
        }

        public override void BeginExpandEnvironmentVariables(string name)
        {
            base.BeginExpandEnvironmentVariables(name);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Expanding environment variables {0}.", name);
        }

        public override void EndExpandEnvironmentVariables(string result, string name)
        {
            base.EndExpandEnvironmentVariables(result, name);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Expanded environment variables {0} to {1}.", name, result);
        }

        public override void FailExpandEnvironmentVariables(ref Exception exception)
        {
            base.FailExpandEnvironmentVariables(ref exception);

            this.log.Error("Exception occurred while expanding the environment variables.", exception);
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

            this.log.Error("Exception occurred while failing fast.", exception);
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

            this.log.Error("Exception occurred while getting the command line arguments.", exception);
        }

        public override void BeginGetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            base.BeginGetEnvironmentVariable(variable, target);
        }

        public override void EndGetEnvironmentVariable(string result, string variable, EnvironmentVariableTarget target)
        {
            base.EndGetEnvironmentVariable(result, variable, target);
        }

        public override void FailGetEnvironmentVariable(ref Exception exception)
        {
            base.FailGetEnvironmentVariable(ref exception);

            this.log.Error("Exception occurred while getting an environment variable.", exception);
        }

        public override void BeginGetEnvironmentVariable(string variable)
        {
            base.BeginGetEnvironmentVariable(variable);
        }

        public override void EndGetEnvironmentVariable(string result, string variable)
        {
            base.EndGetEnvironmentVariable(result, variable);
        }

        public override void BeginGetEnvironmentVariables(EnvironmentVariableTarget target)
        {
            base.BeginGetEnvironmentVariables(target);
        }

        public override void EndGetEnvironmentVariables(IDictionary result, EnvironmentVariableTarget target)
        {
            base.EndGetEnvironmentVariables(result, target);
        }

        public override void FailGetEnvironmentVariables(ref Exception exception)
        {
            base.FailGetEnvironmentVariables(ref exception);
        }

        public override void BeginGetEnvironmentVariables()
        {
            base.BeginGetEnvironmentVariables();
        }

        public override void EndGetEnvironmentVariables(IDictionary result)
        {
            base.EndGetEnvironmentVariables(result);
        }

        public override void BeginGetFolderPath(Environment.SpecialFolder folder)
        {
            base.BeginGetFolderPath(folder);
        }

        public override void EndGetFolderPath(string result, Environment.SpecialFolder folder)
        {
            base.EndGetFolderPath(result, folder);
        }

        public override void FailGetFolderPath(ref Exception exception)
        {
            base.FailGetFolderPath(ref exception);
        }

        public override void BeginGetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        {
            base.BeginGetFolderPath(folder, option);
        }

        public override void EndGetFolderPath(string result, Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        {
            base.EndGetFolderPath(result, folder, option);
        }

        public override void BeginGetLogicalDrives()
        {
            base.BeginGetLogicalDrives();
        }

        public override void EndGetLogicalDrives(string[] result)
        {
            base.EndGetLogicalDrives(result);
        }

        public override void FailGetLogicalDrives(ref Exception exception)
        {
            base.FailGetLogicalDrives(ref exception);

            this.log.Error("Exception occurred while getting logical drives.", exception);
        }

        public override void BeginSetEnvironmentVariable(string variable, string value)
        {
            base.BeginSetEnvironmentVariable(variable, value);
        }

        public override void EndSetEnvironmentVariable(string variable, string value)
        {
            base.EndSetEnvironmentVariable(variable, value);
        }

        public override void FailSetEnvironmentVariable(ref Exception exception)
        {
            base.FailSetEnvironmentVariable(ref exception);

            this.log.Error("Exception occurred while setting an environment variable.", exception);
        }

        public override void BeginSetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            base.BeginSetEnvironmentVariable(variable, value, target);
        }

        public override void EndSetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            base.EndSetEnvironmentVariable(variable, value, target);
        }
    }
}
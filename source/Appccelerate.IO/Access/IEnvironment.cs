//-------------------------------------------------------------------------------
// <copyright file="IEnvironment.cs" company="Appccelerate">
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

namespace Appccelerate.IO.Access
{
    using System;
    using System.Collections.Generic;

    public interface IEnvironment
    {
        string CommandLine { get; }

        string CurrentDirectory { get; set; }

        int ExitCode { get; set; }

        bool HasShutdownStarted { get; }

        bool Is64BitOperatingSystem { get; }

        bool Is64BitProcess { get; }

        string MachineName { get; }

        string NewLine { get; }

        OperatingSystem OSVersion { get; }

        int ProcessorCount { get; }

        string StackTrace { get; }

        string SystemDirectory { get; }

        int SystemPageSize { get; }

        int TickCount { get; }

        string UserDomainName { get; }

        bool UserInteractive { get; }

        string UserName { get; }

        Version Version { get; }

        long WorkingSet { get; }

        void Exit(int exitCode);

        string ExpandEnvironmentVariables(string name);

        void FailFast(string message, Exception exception);

        void FailFast(string message);

        IEnumerable<string> GetCommandLineArgs();

        string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);

        string GetEnvironmentVariable(string variable);

        IDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target);

        IDictionary<string, string> GetEnvironmentVariables();

        string GetFolderPath(Environment.SpecialFolder folder);

        string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option);

        IEnumerable<string> GetLogicalDrives();

        void SetEnvironmentVariable(string variable, string value);

        void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target);
    }
}
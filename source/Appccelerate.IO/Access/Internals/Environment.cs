//-------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="Appccelerate">
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

namespace Appccelerate.IO.Access.Internals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Environment : IEnvironment, IExtensionProvider<IEnvironmentExtension>
    {
        private readonly List<IEnvironmentExtension> extensions;

        public Environment(IEnumerable<IEnvironmentExtension> extensions)
        {
            this.extensions = extensions.ToList();
        }

        public IEnumerable<IEnvironmentExtension> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        public string CommandLine
        {
            get
            {
                return System.Environment.CommandLine;
            }
        }

        public string CurrentDirectory
        {
            get
            {
                return System.Environment.CurrentDirectory;
            }

            set
            {
                System.Environment.CurrentDirectory = value;
            }
        }

        public int ExitCode
        {
            get
            {
                return System.Environment.ExitCode;
            }

            set
            {
                System.Environment.ExitCode = value;
            }
        }

        public bool HasShutdownStarted
        {
            get
            {
                return System.Environment.HasShutdownStarted;
            }
        }

        public bool Is64BitOperatingSystem
        {
            get
            {
                return System.Environment.Is64BitOperatingSystem;
            }
        }

        public bool Is64BitProcess
        {
            get
            {
                return System.Environment.Is64BitProcess;
            }
        }

        public string MachineName
        {
            get
            {
                return System.Environment.MachineName;
            }
        }

        public string NewLine
        {
            get
            {
                return System.Environment.NewLine;
            }
        }

        public OperatingSystem OSVersion
        {
            get
            {
                return System.Environment.OSVersion;
            }
        }

        public int ProcessorCount
        {
            get
            {
                return System.Environment.ProcessorCount;
            }
        }

        public string StackTrace
        {
            get
            {
                return System.Environment.StackTrace;
            }
        }

        public string SystemDirectory
        {
            get
            {
                return System.Environment.SystemDirectory;
            }
        }

        public int SystemPageSize
        {
            get
            {
                return System.Environment.SystemPageSize;
            }
        }

        public int TickCount
        {
            get
            {
                return System.Environment.TickCount;
            }
        }

        public string UserDomainName
        {
            get
            {
                return System.Environment.UserDomainName;
            }
        }

        public bool UserInteractive
        {
            get
            {
                return System.Environment.UserInteractive;
            }
        }

        public string UserName
        {
            get
            {
                return System.Environment.UserName;
            }
        }

        public Version Version
        {
            get
            {
                return System.Environment.Version;
            }
        }

        public long WorkingSet
        {
            get
            {
                return System.Environment.WorkingSet;
            }
        }

        public void Exit(int exitCode)
        {
            this.SurroundWithExtension(() => System.Environment.Exit(exitCode), exitCode);
        }

        public string ExpandEnvironmentVariables(string name)
        {
            return this.SurroundWithExtension(() => System.Environment.ExpandEnvironmentVariables(name), name);
        }

        public void FailFast(string message, Exception exception)
        {
            this.SurroundWithExtension(() => System.Environment.FailFast(message, exception), message, exception);
        }

        public void FailFast(string message)
        {
            this.SurroundWithExtension(() => System.Environment.FailFast(message), message);
        }

        public IEnumerable<string> GetCommandLineArgs()
        {
            return this.SurroundWithExtension(() => System.Environment.GetCommandLineArgs());
        }

        public string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            return this.SurroundWithExtension(() => System.Environment.GetEnvironmentVariable(variable, target), variable, target);
        }

        public string GetEnvironmentVariable(string variable)
        {
            return this.SurroundWithExtension(() => System.Environment.GetEnvironmentVariable(variable), variable);
        }

        public IDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target)
        {
            return this.SurroundWithExtension(() => System.Environment.GetEnvironmentVariables(target), target).OfType<DictionaryEntry>().ToDictionary(e => (string)e.Key, v => (string)v.Value);
        }

        public IDictionary<string, string> GetEnvironmentVariables()
        {
            return this.SurroundWithExtension(() => System.Environment.GetEnvironmentVariables()).OfType<DictionaryEntry>().ToDictionary(e => (string)e.Key, v => (string)v.Value);
        }

        public string GetFolderPath(System.Environment.SpecialFolder folder)
        {
            return this.SurroundWithExtension(() => System.Environment.GetFolderPath(folder), folder);
        }

        public string GetFolderPath(System.Environment.SpecialFolder folder, System.Environment.SpecialFolderOption option)
        {
            return this.SurroundWithExtension(() => System.Environment.GetFolderPath(folder, option), folder, option);
        }

        public IEnumerable<string> GetLogicalDrives()
        {
            return this.SurroundWithExtension(() => System.Environment.GetLogicalDrives());
        }

        public void SetEnvironmentVariable(string variable, string value)
        {
            this.SurroundWithExtension(() => System.Environment.SetEnvironmentVariable(variable, value), variable, value);
        }

        public void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            this.SurroundWithExtension(() => System.Environment.SetEnvironmentVariable(variable, value, target), variable, value, target);
        }
    }
}
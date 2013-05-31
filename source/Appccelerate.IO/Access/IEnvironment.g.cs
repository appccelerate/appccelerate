//-------------------------------------------------------------------------------
// <copyright file="IEnvironment.cs" company="Appccelerate">
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

namespace Appccelerate.IO.Access
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [CompilerGenerated]
    public interface IEnvironment
    {
        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.CommandLine"]' />
        string CommandLine { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.CurrentDirectory"]' />
        string CurrentDirectory { get; set; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.ExitCode"]' />
        int ExitCode { get; set; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.HasShutdownStarted"]' />
        bool HasShutdownStarted { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.Is64BitOperatingSystem"]' />
        bool Is64BitOperatingSystem { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.Is64BitProcess"]' />
        bool Is64BitProcess { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.MachineName"]' />
        string MachineName { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.NewLine"]' />
        string NewLine { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.OSVersion"]' />
        OperatingSystem OSVersion { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.ProcessorCount"]' />
        int ProcessorCount { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.StackTrace"]' />
        string StackTrace { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.SystemDirectory"]' />
        string SystemDirectory { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.SystemPageSize"]' />
        int SystemPageSize { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.TickCount"]' />
        int TickCount { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.UserDomainName"]' />
        string UserDomainName { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.UserInteractive"]' />
        bool UserInteractive { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.UserName"]' />
        string UserName { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.Version"]' />
        Version Version { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="P:System.Environment.WorkingSet"]' />
        long WorkingSet { get; }

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.Exit(System.Int32)"]' />
        void Exit(int exitCode);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.ExpandEnvironmentVariables(System.String)"]' />
        string ExpandEnvironmentVariables(string name);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.FailFast(System.String,System.Exception)"]' />
        void FailFast(string message, Exception exception);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.FailFast(System.String)"]' />
        void FailFast(string message);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.GetCommandLineArgs"]' />
        IEnumerable<string> GetCommandLineArgs();

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.GetEnvironmentVariable(System.String,System.EnvironmentVariableTarget)"]' />
        string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.GetEnvironmentVariable(System.String)"]' />
        string GetEnvironmentVariable(string variable);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.GetEnvironmentVariables(System.EnvironmentVariableTarget)"]' />
        IDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.GetEnvironmentVariables"]' />
        IDictionary<string, string> GetEnvironmentVariables();

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder)"]' />
        string GetFolderPath(Environment.SpecialFolder folder);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder,System.Environment.SpecialFolderOption)"]' />
        string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.GetLogicalDrives"]' />
        IEnumerable<string> GetLogicalDrives();

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.SetEnvironmentVariable(System.String,System.String)"]' />
        void SetEnvironmentVariable(string variable, string value);

        /// <include file='IEnvironment.doc.xml' path='doc/members/member[@name="M:System.Environment.SetEnvironmentVariable(System.String,System.String,System.EnvironmentVariableTarget)"]' />
        void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target);
    }
}
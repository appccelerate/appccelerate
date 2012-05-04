//-------------------------------------------------------------------------------
// <copyright file="IEnvironmentExtension.cs" company="Appccelerate">
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
    using System.Runtime.CompilerServices;

    [CompilerGenerated]
    public interface IEnvironmentExtension
    {
        void BeginExit(int exitCode);

        void EndExit(int exitCode);

        void FailExit(ref Exception exception);

        void BeginExpandEnvironmentVariables(string name);

        void EndExpandEnvironmentVariables(string result, string name);

        void FailExpandEnvironmentVariables(ref Exception exception);

        void BeginFailFast(string message, Exception exception);

        void EndFailFast(string message, Exception exception);

        void FailFailFast(ref Exception exception);

        void BeginFailFast(string message);

        void EndFailFast(string message);

        void BeginGetCommandLineArgs();

        void EndGetCommandLineArgs(string[] result);

        void FailGetCommandLineArgs(ref Exception exception);

        /*----------------

        string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);

        string GetEnvironmentVariable(string variable);

        IDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target);

        IDictionary<string, string> GetEnvironmentVariables();

        string GetFolderPath(Environment.SpecialFolder folder);

        string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option);

        IEnumerable<string> GetLogicalDrives();

        void SetEnvironmentVariable(string variable, string value);

        void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target);
         -------*/
    }
}
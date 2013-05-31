//-------------------------------------------------------------------------------
// <copyright file="EnvironmentExtensionBase.cs" company="Appccelerate">
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
    using System.Collections;

    public class EnvironmentExtensionBase : IEnvironmentExtension
    {
        public virtual void BeginExit(int exitCode)
        {
        }

        public virtual void EndExit(int exitCode)
        {
        }

        public virtual void FailExit(ref Exception exception)
        {
        }

        public virtual void BeginExpandEnvironmentVariables(string name)
        {
        }

        public virtual void EndExpandEnvironmentVariables(string result, string name)
        {
        }

        public virtual void FailExpandEnvironmentVariables(ref Exception exception)
        {
        }

        public virtual void BeginFailFast(string message, Exception exception)
        {
        }

        public virtual void EndFailFast(string message, Exception exception)
        {
        }

        public virtual void FailFailFast(ref Exception exception)
        {
        }

        public virtual void BeginFailFast(string message)
        {
        }

        public virtual void EndFailFast(string message)
        {
        }

        public virtual void BeginGetCommandLineArgs()
        {
        }

        public virtual void EndGetCommandLineArgs(string[] result)
        {
        }

        public virtual void FailGetCommandLineArgs(ref Exception exception)
        {
        }

        public virtual void BeginGetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
        }

        public virtual void EndGetEnvironmentVariable(string result, string variable, EnvironmentVariableTarget target)
        {
        }

        public virtual void FailGetEnvironmentVariable(ref Exception exception)
        {
        }

        public virtual void BeginGetEnvironmentVariable(string variable)
        {
        }

        public virtual void EndGetEnvironmentVariable(string result, string variable)
        {
        }

        public virtual void EndSetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
        }

        public virtual void BeginSetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
        }

        public virtual void FailSetEnvironmentVariable(ref Exception exception)
        {
        }

        public virtual void EndSetEnvironmentVariable(string variable, string value)
        {
        }

        public virtual void BeginSetEnvironmentVariable(string variable, string value)
        {
        }

        public virtual void FailGetLogicalDrives(ref Exception exception)
        {
        }

        public virtual void EndGetLogicalDrives(string[] result)
        {
        }

        public virtual void BeginGetLogicalDrives()
        {
        }

        public virtual void EndGetFolderPath(string result, Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        {
        }

        public virtual void BeginGetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        {
        }

        public virtual void FailGetFolderPath(ref Exception exception)
        {
        }

        public virtual void EndGetFolderPath(string result, Environment.SpecialFolder folder)
        {
        }

        public virtual void BeginGetFolderPath(Environment.SpecialFolder folder)
        {
        }

        public virtual void EndGetEnvironmentVariables(IDictionary result)
        {
        }

        public virtual void BeginGetEnvironmentVariables()
        {
        }

        public virtual void FailGetEnvironmentVariables(ref Exception exception)
        {
        }

        public virtual void EndGetEnvironmentVariables(IDictionary result, EnvironmentVariableTarget target)
        {
        }

        public virtual void BeginGetEnvironmentVariables(EnvironmentVariableTarget target)
        {
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="EnvironmentExtensionBase.cs" company="Appccelerate">
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
    }
}
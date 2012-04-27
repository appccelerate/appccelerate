//-------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="Appccelerate">
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

        public void Exit(int exitCode)
        {
            this.SurroundWithExtension(() => System.Environment.Exit(exitCode));
        }

        public string ExpandEnvironmentVariables(string name)
        {
            return this.SurroundWithExtension(() => System.Environment.ExpandEnvironmentVariables(name));
        }

        public void FailFast(string message, Exception exception)
        {
            this.SurroundWithExtension(() => System.Environment.FailFast(message, exception));
        }

        public void FailFast(string message)
        {
            this.SurroundWithExtension(() => System.Environment.FailFast(message));
        }

        public IEnumerable<string> GetCommandLineArgs()
        {
            return this.SurroundWithExtension(() => System.Environment.GetCommandLineArgs());
        }

        public string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            return this.SurroundWithExtension(() => System.Environment.GetEnvironmentVariable(variable, target));
        }

        public string GetEnvironmentVariable(string variable)
        {
            return this.SurroundWithExtension(() => System.Environment.GetEnvironmentVariable(variable));
        }

        public IDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target)
        {
            return this.SurroundWithExtension(() => System.Environment.GetEnvironmentVariables(target)).OfType<DictionaryEntry>().ToDictionary(e => (string)e.Key, v => (string)v.Value);
        }

        public IDictionary<string, string> GetEnvironmentVariables()
        {
            return this.SurroundWithExtension(() => System.Environment.GetEnvironmentVariables()).OfType<DictionaryEntry>().ToDictionary(e => (string)e.Key, v => (string)v.Value);
        }
    }
}
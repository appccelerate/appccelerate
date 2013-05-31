//-------------------------------------------------------------------------------
// <copyright file="CustomExtensionExtensions.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Specification.Dummies
{
    using System;
    using System.Globalization;
    using System.Reflection;

    public static class CustomExtensionExtensions
    {
        public static void Dump(this ICustomExtension extension, string message)
        {
            var dumpMethod = typeof(CustomExtensionBase).GetMethod("Dump", BindingFlags.Instance | BindingFlags.NonPublic);
            var action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), extension, dumpMethod);
            action(string.Format(CultureInfo.InvariantCulture, "Behaving on {0} at {1}.", extension, message));
        }
    }
}
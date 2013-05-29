//-------------------------------------------------------------------------------
// <copyright file="ExceptionExtensionMethods.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Sample
{
    using System;
    using System.Reflection;

    public static class ExceptionExtensionMethods
    {
        /// <summary>
        /// Preserves the stack trace of the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>Returns the specified exception to allow writing throw exception.preserveStackTrace().</returns>
        public static Exception PreserveStackTrace(this Exception exception)
        {
            Ensure.ArgumentNotNull(exception, "exception");

#if !SILVERLIGHT
            var remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

            remoteStackTraceString.SetValue(exception, exception.StackTrace + Environment.NewLine);
#endif

            return exception;
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="ExceptionHandlingContext.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Exceptions
{
    /// <summary>
    /// Provides context information for handling exceptions by extensions.
    /// </summary>
    public class ExceptionHandlingContext
    {
        /// <summary>
        /// Gets a value indicating whether the exception is handled by an extension.
        /// When this value is set to true with <see cref="SetHandled"/> then the exception is not re-thrown.
        /// </summary>
        /// <value><c>true</c> if handled; otherwise, <c>false</c>.</value>
        public bool Handled { get; private set; }

        /// <summary>
        /// Sets <see cref="Handled"/> to true.
        /// </summary>
        public void SetHandled()
        {
            this.Handled = true;
        }
    }
}
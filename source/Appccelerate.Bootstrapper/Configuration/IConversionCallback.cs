//-------------------------------------------------------------------------------
// <copyright file="IConversionCallback.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Configuration
{
    using System.Reflection;

    /// <summary>
    /// A conversion callback is responsible for converting a string value to the target property type.
    /// </summary>
    public interface IConversionCallback
    {
        /// <summary>
        /// Converts the specified value to the target property type.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetProperty">The target property.</param>
        /// <returns>The converted value</returns>
        object Convert(string value, PropertyInfo targetProperty);
    }
}
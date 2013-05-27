//-------------------------------------------------------------------------------
// <copyright file="DefaultConversionCallback.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Configuration.Internals
{
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Default conversion callback which uses System.Convert and invariant culture to convert to the target property type.
    /// </summary>
    public class DefaultConversionCallback : IConversionCallback
    {
        /// <inheritdoc />
        public object Convert(string value, PropertyInfo targetProperty)
        {
            Ensure.ArgumentNotNull(targetProperty, "targetProperty");

            return System.Convert.ChangeType(value, targetProperty.PropertyType, CultureInfo.InvariantCulture);
        }
    }
}
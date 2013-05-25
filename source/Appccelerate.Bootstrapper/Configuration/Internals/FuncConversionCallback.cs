//-------------------------------------------------------------------------------
// <copyright file="FuncConversionCallback.cs" company="Appccelerate">
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
    using System;
    using System.Reflection;

    /// <summary>
    /// Conversion callback which uses a function delegate for conversion.
    /// </summary>
    public class FuncConversionCallback : IConversionCallback
    {
        private readonly Func<string, PropertyInfo, object> conversionCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncConversionCallback"/> class.
        /// </summary>
        /// <param name="conversionCallback">The conversion callback which will be called for conversion.</param>
        public FuncConversionCallback(Func<string, PropertyInfo, object> conversionCallback)
        {
            this.conversionCallback = conversionCallback;
        }

        /// <inheritdoc />
        public object Convert(string value, PropertyInfo targetProperty)
        {
            return this.conversionCallback(value, targetProperty);
        }
    }
}
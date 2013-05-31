//-------------------------------------------------------------------------------
// <copyright file="HaveConversionCallbacks.cs" company="Appccelerate">
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
    using System.Collections.Generic;

    /// <summary>
    /// Default IHaveConversionCallbacks
    /// </summary>
    public class HaveConversionCallbacks : IHaveConversionCallbacks
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HaveConversionCallbacks"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public HaveConversionCallbacks(IExtension extension)
        {
            var callbacksProvider = extension as IHaveConversionCallbacks;

            this.ConversionCallbacks = callbacksProvider != null
                ? callbacksProvider.ConversionCallbacks
                : new Dictionary<string, IConversionCallback>();
        }

        /// <inheritdoc />
        public IDictionary<string, IConversionCallback> ConversionCallbacks { get; private set; }
    }
}
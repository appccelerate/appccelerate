//-------------------------------------------------------------------------------
// <copyright file="AssignExtensionProperties.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Default IAssignExtensionProperties
    /// </summary>
    public class AssignExtensionProperties : IAssignExtensionProperties
    {
        /// <inheritdoc />
        public void Assign(IReflectExtensionProperties reflector, IExtension extension, IConsumeConfiguration consumer, IHaveConversionCallbacks conversionCallbacksProvider, IHaveDefaultConversionCallback defaultConversionCallbackProvider)
        {
            Ensure.ArgumentNotNull(reflector, "reflector");
            Ensure.ArgumentNotNull(consumer, "consumer");
            Ensure.ArgumentNotNull(conversionCallbacksProvider, "conversionCallbacksProvider");
            Ensure.ArgumentNotNull(defaultConversionCallbackProvider, "defaultConversionCallbackProvider");

            var properties = reflector.Reflect(extension).ToList();
            IDictionary<string, IConversionCallback> conversionCallbacks = conversionCallbacksProvider.ConversionCallbacks;
            IConversionCallback defaultCallback = defaultConversionCallbackProvider.DefaultConversionCallback;

            foreach (KeyValuePair<string, string> keyValuePair in consumer.Configuration)
            {
                KeyValuePair<string, string> pair = keyValuePair;

                var matchedProperty = properties.SingleOrDefault(property => property.Name.Equals(pair.Key, StringComparison.OrdinalIgnoreCase));

                if (matchedProperty == null)
                {
                    continue;
                }

                IConversionCallback conversionCallback;
                if (!conversionCallbacks.TryGetValue(pair.Key, out conversionCallback))
                {
                    conversionCallback = defaultCallback;
                }

                matchedProperty.SetValue(extension, conversionCallback.Convert(pair.Value, matchedProperty), null);
            }
        }
    }
}
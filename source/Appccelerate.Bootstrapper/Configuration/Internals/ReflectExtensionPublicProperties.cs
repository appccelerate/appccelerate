//-------------------------------------------------------------------------------
// <copyright file="ReflectExtensionPublicProperties.cs" company="Appccelerate">
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
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// IReflectExtensionProperties implementations which gives back PropertyInfo about all public instance 
    /// properties which are writeable of the reflected extension
    /// </summary>
    public class ReflectExtensionPublicProperties : IReflectExtensionProperties
    {
        /// <inheritdoc />
        public IEnumerable<PropertyInfo> Reflect(IExtension extension)
        {
            Ensure.ArgumentNotNull(extension, "extension");

            return extension.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite);
        }
    }
}
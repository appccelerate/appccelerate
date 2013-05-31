//-------------------------------------------------------------------------------
// <copyright file="CustomExtensionResolver.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Customization
{
    using Appccelerate.Bootstrapper.Sample.Complex;
    using Appccelerate.Bootstrapper.Sample.Complex.Extensions;

    /// <summary>
    /// Custom extension resolver.
    /// </summary>
    public class CustomExtensionResolver : IExtensionResolver<IComplexExtension>
    {
        /// <inheritdoc />
        public void Resolve(IExtensionPoint<IComplexExtension> extensionPoint)
        {
            Ensure.ArgumentNotNull(extensionPoint, "extensionPoint");

            extensionPoint.AddExtension(new ExtensionWhichNeedsDependency());
            extensionPoint.AddExtension(new ExtensionWhichIsFunqlet());
            extensionPoint.AddExtension(new ExtensionWithExtensionConfigurationSection());
            extensionPoint.AddExtension(
                new ExtensionWithExtensionConfigurationSectionWithConversionAndCustomizedLoading());
            extensionPoint.AddExtension(new ExtensionWithExtensionConfigurationSectionWithDictionary());
            extensionPoint.AddExtension(new ExtensionWithCustomConfigurationSection());
        }
    }
}
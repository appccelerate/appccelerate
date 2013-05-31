//-------------------------------------------------------------------------------
// <copyright file="IResourceLoader.cs" company="Appccelerate">
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

namespace Appccelerate.IO.Resources
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.XPath;

    /// <summary>
    /// Provides functionality to load resources.
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// Loads an XML file into an <see cref="XmlNode"/>
        /// </summary>
        /// <param name="type">The type to find the assembly and namespace containing the resource</param>
        /// <param name="resourceName">The name of the resource relative to the namespace of <paramref name="type"/>.</param>
        /// <returns>A <see cref="IXPathNavigable"/> containing the contents of the embedded XML file</returns>
        IXPathNavigable LoadResourceAsXml(Type type, string resourceName);

        /// <summary>
        /// Loads an embedded XML file into a<see cref="XmlNode" />
        /// </summary>
        /// <param name="assembly">The Assembly to look for the resource.</param>
        /// <param name="resourceName">The full name including the namespace of the resource.</param>
        /// <returns>
        /// A<see cref="IXPathNavigable" />containing the contents of the embedded XML file
        /// </returns>
        IXPathNavigable LoadResourceAsXml(Assembly assembly, string resourceName);

        /// <summary>
        /// Loads an embedded file into a <see cref="string"/>
        /// </summary>
        /// <param name="type">The type to find the assembly containing the resource</param>
        /// <param name="resourceName">The name of the resource relative to the namespace of <paramref name="type"/>.</param>
        /// <returns>A <see cref="string"/> containing the contents of the embedded resource file</returns>
        string LoadResourceAsString(Type type, string resourceName);

        /// <summary>
        /// Loads an embedded file into a <see cref="string"/>
        /// </summary>
        /// <param name="assembly">The Assembly to look for the resource.</param>
        /// <param name="resourceName">The full name including the namespace of the resource.</param>
        /// <returns>A <see cref="string"/> containing the contents of the embedded resource file</returns>
        string LoadResourceAsString(Assembly assembly, string resourceName);

        /// <summary>
        /// Loads an embedded file into a <see cref="Stream"/>
        /// </summary>
        /// <param name="type">The type to find the assembly containing the resource</param>
        /// <param name="resourceName">The name of the resource relative to the namespace of <paramref name="type"/>./// </param>
        /// <returns>A <see cref="Stream"/> containing the contents of the embedded resource file</returns>
        MemoryStream LoadResourceAsStream(Type type, string resourceName);

        /// <summary>
        /// Loads an embedded file into a <see cref="Stream"/>
        /// </summary>
        /// <param name="assembly">The Assembly to look for the resource.</param>
        /// <param name="resourceName">The full name including the namespace of the resource.</param>
        /// <returns>A <see cref="Stream"/> containing the contents of the embedded resource file</returns>
        MemoryStream LoadResourceAsStream(Assembly assembly, string resourceName);
    }
}
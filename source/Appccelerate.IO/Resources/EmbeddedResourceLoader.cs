//-------------------------------------------------------------------------------
// <copyright file="EmbeddedResourceLoader.cs" company="Appccelerate">
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
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.XPath;
    
    /// <summary>
    /// Implements the <see cref="IResourceLoader"/> to load embedded resources from assemblies
    /// </summary>
    public class EmbeddedResourceLoader : IResourceLoader
    {
        /// <summary>
        /// Loads an XML file into an <see cref="XmlNode"/>
        /// </summary>
        /// <param name="type">The type to find the assembly and namespace containing the resource</param>
        /// <param name="resourceName">The name of the resource relative to the namespace of <paramref name="type"/>.</param>
        /// <returns>
        /// A <see cref="IXPathNavigable"/> containing the contents of the embedded XML file
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="resourceName"/> does not exist</exception>
        public IXPathNavigable LoadResourceAsXml(Type type, string resourceName)
        {
            Ensure.ArgumentNotNull(type, "type");

            return this.LoadResourceAsXml(type.Assembly, string.Format(CultureInfo.InvariantCulture, "{0}.{1}", type.Namespace, resourceName));
        }

        /// <summary>
        /// Loads an embedded XML file into a<see cref="XmlNode"/>
        /// </summary>
        /// <param name="assembly">The Assembly to look for the resource.</param>
        /// <param name="resourceName">The full name including the namespace of the resource.</param>
        /// <returns>
        /// A<see cref="IXPathNavigable"/>containing the contents of the embedded XML file
        /// </returns>
        public IXPathNavigable LoadResourceAsXml(Assembly assembly, string resourceName)
        {
            string xml = this.LoadResourceAsString(assembly, resourceName);
            XmlDocument doc = new XmlDocument
                                  {
                                      PreserveWhitespace = true
                                  };
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }

        /// <summary>
        /// Loads an embedded file into a <see cref="string"/>
        /// </summary>
        /// <param name="type">The type to find the assembly containing the resource</param>
        /// <param name="resourceName">The name of the resource relative to the namespace of <paramref name="type"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the contents of the embedded resource file
        /// </returns>
        public string LoadResourceAsString(Type type, string resourceName)
        {
            Ensure.ArgumentNotNull(type, "type");

            return this.LoadResourceAsString(type.Assembly, string.Format(CultureInfo.InvariantCulture, "{0}.{1}", type.Namespace, resourceName));
        }

        /// <summary>
        /// Loads an embedded file into a <see cref="string"/>
        /// </summary>
        /// <param name="assembly">The Assembly to look for the resource.</param>
        /// <param name="resourceName">The full name including the namespace of the resource.</param>
        /// <returns>
        /// A <see cref="string"/> containing the contents of the embedded resource file
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times",
            Justification = "cannot be used otherwise")]
        public string LoadResourceAsString(Assembly assembly, string resourceName)
        {
            Ensure.ArgumentNotNull(assembly, "assembly");

            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Resource {0} does not exist", resourceName));
                }

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Loads an embedded file into a <see cref="Stream"/>
        /// </summary>
        /// <param name="type">The type to find the assembly containing the resource</param>
        /// <param name="resourceName">The name of the resource relative to the namespace of <paramref name="type"/>.///</param>
        /// <returns>
        /// A <see cref="Stream"/> containing the contents of the embedded resource file
        /// </returns>
        public MemoryStream LoadResourceAsStream(Type type, string resourceName)
        {
            Ensure.ArgumentNotNull(type, "type");

            return this.LoadResourceAsStream(type.Assembly, string.Format(CultureInfo.InvariantCulture, "{0}.{1}", type.Namespace, resourceName));
        }

        /// <summary>
        /// Loads an embedded file into a <see cref="Stream"/>
        /// </summary>
        /// <param name="assembly">The Assembly to look for the resource.</param>
        /// <param name="resourceName">The full name including the namespace of the resource.</param>
        /// <returns>
        /// A <see cref="Stream"/> containing the contents of the embedded resource file
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "it's a memory stream and has to be disposed by caller")]
        public MemoryStream LoadResourceAsStream(Assembly assembly, string resourceName)
        {
            Ensure.ArgumentNotNull(assembly, "assembly");

            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Resource {0} does not exist", resourceName));
                }

                var output = new MemoryStream();
                resourceStream.CopyTo(output);
                output.Position = 0;
                return output;
            }
        }
    }
}
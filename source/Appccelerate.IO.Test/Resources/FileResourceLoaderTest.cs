//-------------------------------------------------------------------------------
// <copyright file="FileResourceLoaderTest.cs" company="Appccelerate">
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
    using System.IO;
    using System.Xml.XPath;

    using Appccelerate.IO.Streams;

    using IO;

    using Xunit;

    public class FileResourceLoaderTest
    {
        /// <summary>
        /// File name for a parameters XML file
        /// </summary>
        private const string FileName = "EmbeddedTestResources.Parameters.xml";

        /// <summary>
        /// Name for an XML resource
        /// </summary>
        private const string XmlResourceName = "EmbeddedTestResources.XmlResource.xml";

        /// <summary>
        /// Name for a text resource
        /// </summary>
        private const string TextResourceName = "EmbeddedTestResources.StringResource.txt";

        /// <summary>
        /// The object under test
        /// </summary>
        private readonly FileResourceLoader testee;

        private readonly string filepath;

        /// <summary>
        /// The embedded resource loader to load the resources from the test assembly
        /// </summary>
        private readonly EmbeddedResourceLoader resourceLoader;

        public FileResourceLoaderTest()
        {
            this.testee = new FileResourceLoader();
            this.resourceLoader = new EmbeddedResourceLoader();
            this.filepath =
                Path.Combine(Path.GetDirectoryName(typeof(FileResourceLoaderTest).Assembly.Location), FileName);
        }
        
        /// <summary>
        /// Loads the resource as stream from file. Ensures that the stream contents are identical.
        /// </summary>
        [Fact]
        public void LoadResourceAsStreamFromAssembly()
        {
            Stream expected = this.resourceLoader.LoadResourceAsStream(typeof(FileResourceLoaderTest), XmlResourceName);

            using (new TemporaryFileHolder(this.filepath, expected))
            {
                Stream resource = this.testee.LoadResourceAsStream(typeof(FileResourceLoaderTest).Assembly, FileName);

                Assert.True(resource.CompareStreamContentsTo(expected));
            }
        }

        /// <summary>
        /// Loads the resource as stream from file. Ensures that the stream contents are identical.
        /// </summary>
        [Fact]
        public void LoadResourceAsStreamFromType()
        {
            Stream expected = this.resourceLoader.LoadResourceAsStream(typeof(FileResourceLoaderTest), XmlResourceName);

            using (new TemporaryFileHolder(this.filepath, expected))
            {
                Stream resource = this.testee.LoadResourceAsStream(typeof(FileResourceLoaderTest), FileName);

                Assert.True(resource.CompareStreamContentsTo(expected));
            }
        }

        /// <summary>
        /// Loads the resource as string from file. Ensures that the string contents are identical.
        /// </summary>
        [Fact]
        public void LoadResourceAsStringFromAssembly()
        {
            string expected = this.resourceLoader.LoadResourceAsString(typeof(FileResourceLoaderTest), TextResourceName);
            using (new TemporaryFileHolder(this.filepath, expected))
            {
                string resource = this.testee.LoadResourceAsString(typeof(FileResourceLoaderTest).Assembly, FileName);
                Assert.Equal(expected, resource);
            }
        }

        /// <summary>
        /// Loads the resource as string from file. Ensures that the string contents are identical.
        /// </summary>
        [Fact]
        public void LoadResourceAsStringFromType()
        {
            string expected = this.resourceLoader.LoadResourceAsString(typeof(FileResourceLoaderTest), TextResourceName);
            using (new TemporaryFileHolder(this.filepath, expected))
            {
                string resource = this.testee.LoadResourceAsString(typeof(FileResourceLoaderTest), FileName);
                Assert.Equal(expected, resource);
            }
        }

        /// <summary>
        /// Loads the resource as XML from file. Ensures that the xml contents are identical.
        /// </summary>
        [Fact]
        public void LoadResourceAsXmlFromAssembly()
        {
            Stream resourceStream = this.resourceLoader.LoadResourceAsStream(typeof(FileResourceLoaderTest), XmlResourceName);

            using (new TemporaryFileHolder(this.filepath, resourceStream))
            {
                IXPathNavigable expected = this.resourceLoader.LoadResourceAsXml(typeof(FileResourceLoaderTest), XmlResourceName);
                IXPathNavigable resource = this.testee.LoadResourceAsXml(typeof(FileResourceLoaderTest).Assembly, FileName);

                Assert.Equal(expected.CreateNavigator().InnerXml, resource.CreateNavigator().InnerXml);
            }
        }

        /// <summary>
        /// Loads the resource as XML from file. Ensures that the xml contents are identical.
        /// </summary>
        [Fact]
        public void LoadResourceAsXmlFromType()
        {
            Stream resourceStream = this.resourceLoader.LoadResourceAsStream(typeof(FileResourceLoaderTest), XmlResourceName);

            using (new TemporaryFileHolder(this.filepath, resourceStream))
            {
                IXPathNavigable expected = this.resourceLoader.LoadResourceAsXml(typeof(FileResourceLoaderTest), XmlResourceName);
                IXPathNavigable resource = this.testee.LoadResourceAsXml(typeof(FileResourceLoaderTest), FileName);

                Assert.Equal(expected.CreateNavigator().InnerXml, resource.CreateNavigator().InnerXml);
            }
        }
    }
}
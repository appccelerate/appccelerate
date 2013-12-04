//-------------------------------------------------------------------------------
// <copyright file="EmbeddedResourceLoaderTest.cs" company="Appccelerate">
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
    using System.Xml.XPath;

    using FluentAssertions;

    using Xunit;

    public class EmbeddedResourceLoaderTest
    {
        /// <summary>
        /// Name for a non-existing text resource
        /// </summary>
        private const string NoTextResourceName = "EmbeddedTestResources.NoResource.txt";

        /// <summary>
        /// Name for a non-existing XML resource
        /// </summary>
        private const string NoXmlResourceName = "EmbeddedTestResources.NoResource.xml";

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
        private readonly EmbeddedResourceLoader testee;

        public EmbeddedResourceLoaderTest()
        {
            this.testee = new EmbeddedResourceLoader();
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Fact]
        public void LoadNotExistingStreamResourceFromAssembly()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsString(
                          Assembly.GetExecutingAssembly(),
                          string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, NoTextResourceName)));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Fact]
        public void LoadNotExistingStreamResourceFromType()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsXml(typeof(EmbeddedResourceLoaderTest), NoTextResourceName));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Fact]
        public void LoadNotExistingStringResourceFromAssembly()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsString(
                          Assembly.GetExecutingAssembly(),
                          string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, NoTextResourceName)));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Fact]
        public void LoadNotExistingStringResourceFromType()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsString(typeof(EmbeddedResourceLoaderTest), NoTextResourceName));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Fact]
        public void LoadNotExistingXmlResourceFromAssembly()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsXml(
                    Assembly.GetExecutingAssembly(),
                    string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, NoTextResourceName)));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Fact]
        public void LoadNotExistingXmlResourceFromType()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsXml(typeof(EmbeddedResourceLoaderTest), NoXmlResourceName));
        }

        /// <summary>
        /// Loads a resource from assembly and verifies the size of the resulting stream
        /// </summary>
        [Fact]
        public void LoadStreamResourceFromAssembly()
        {
            Stream stream = this.testee.LoadResourceAsStream(
                Assembly.GetExecutingAssembly(),
                string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, XmlResourceName));

            stream.Length.Should().BeGreaterOrEqualTo(200);
            stream.Position.Should().Be(0);
        }

        /// <summary>
        /// Loads a resource from type and verifies the size of the resulting stream
        /// </summary>
        [Fact]
        public void LoadStreamResourceFromType()
        {
            Stream stream = this.testee.LoadResourceAsStream(typeof(EmbeddedResourceLoaderTest), XmlResourceName);

            stream.Length.Should().BeGreaterOrEqualTo(200);
            stream.Position.Should().Be(0);
        }

        [Fact]
        public void LoadNotExistingResourceAsStream()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsStream(typeof(EmbeddedResourceLoaderTest), NoTextResourceName));
        }

        /// <summary>
        /// Loads a string resource from assembly and verifies the string is correct
        /// </summary>
        [Fact]
        public void LoadStringResourceFromAssembly()
        {
            string result = this.testee.LoadResourceAsString(
                Assembly.GetExecutingAssembly(),
                string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, TextResourceName));

            Assert.Equal("MyString", result);
        }

        /// <summary>
        /// Loads a string resource from type and verifies the string is correct
        /// </summary>
        [Fact]
        public void LoadStringResourceFromType()
        {
            string result = this.testee.LoadResourceAsString(typeof(EmbeddedResourceLoaderTest), TextResourceName);

            Assert.Equal("MyString", result);
        }

        /// <summary>
        /// Loads a XML resource from assembly and verifies it is correct.
        /// </summary>
        [Fact]
        public void LoadXmlResourceFromAssembly()
        {
            IXPathNavigable xml =
                this.testee.LoadResourceAsXml(
                    Assembly.GetExecutingAssembly(),
                    string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, XmlResourceName));

            Assert.True(xml.CreateNavigator().HasChildren);
        }

        /// <summary>
        /// Loads a XML resource from type and verifies it is correct
        /// </summary>
        [Fact]
        public void LoadXmlResourceFromType()
        {
            IXPathNavigable xmlNode =
                this.testee.LoadResourceAsXml(typeof(EmbeddedResourceLoaderTest), XmlResourceName);
            Assert.True(xmlNode.CreateNavigator().HasChildren);
        }
    }
}
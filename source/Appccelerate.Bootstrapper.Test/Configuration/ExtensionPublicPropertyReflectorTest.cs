//-------------------------------------------------------------------------------
// <copyright file="ExtensionPublicPropertyReflectorTest.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Configuration
{
    using Appccelerate.Bootstrapper.Configuration.Internals;
    using Appccelerate.Formatters;
    using FluentAssertions;
    using Xunit;

    public class ExtensionPublicPropertyReflectorTest
    {
        private readonly CustomExtension extension;

        private readonly ReflectExtensionPublicProperties testee;

        public ExtensionPublicPropertyReflectorTest()
        {
            this.extension = new CustomExtension();

            this.testee = new ReflectExtensionPublicProperties();
        }

        [Fact]
        public void Reflect_ShouldReturnPublicInstanceProperties()
        {
            var properties = this.testee.Reflect(this.extension);

            properties.Should()
                .HaveCount(1)
                .And.Contain(x => x.Name == "PublicProperty")
                .And.NotContain(x => x.Name == "InternalProperty")
                .And.NotContain(x => x.Name == "ProtectedProperty")
                .And.NotContain(x => x.Name == "PrivateProperty");
        }

        [Fact]
        public void Reflect_ShouldReturnOnlyWrittableProperties()
        {
            var properties = this.testee.Reflect(this.extension);

            properties.Should()
                .Contain(x => x.Name == "PublicProperty")
                .And.NotContain(x => x.Name == "PublicWriteOnlyProperty");
        }

        [Fact]
        public void Reflect_ShouldNotReturnStaticProperties()
        {
            var properties = this.testee.Reflect(this.extension);

            properties.Should()
                .NotContain(x => x.Name == "PublicStaticProperty")
                .And.NotContain(x => x.Name == "InternalStaticProperty")
                .And.NotContain(x => x.Name == "ProtectedStaticProperty")
                .And.NotContain(x => x.Name == "PrivateStaticProperty");
        }

        private class CustomExtension : IExtension
        {
            public static string PublicStaticProperty { get; set; }

            public string PublicProperty { get; set; }

            public string PublicWriteOnlyProperty
            {
                get
                {
                    return this.ToString();
                }
            }

            /// <inheritdoc />
            public string Name
            {
                get
                {
                    return this.GetType().FullNameToString();
                }
            }

            internal static string InternalStaticProperty { get; set; }

            internal string InternalProperty { get; set; }

            protected static string ProtectedStaticProperty { get; set; }

            protected string ProtectedProperty { get; set; }

            private static string PrivateStaticProperty { get; set; }

            private string PrivateProperty { get; set; }

            /// <inheritdoc />
            public string Describe()
            {
                return string.Empty;
            }
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="AssignExtensionPropertiesTest.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Reflection;
    using Appccelerate.Bootstrapper.Configuration.Internals;
    using Appccelerate.Formatters;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class AssignExtensionPropertiesTest
    {
        private const string SomeExtensionPropertyName = "SomeProperty";

        private const string SomeExtensionPropertyValue = "AnyValue";

        private readonly Mock<IConsumeConfiguration> consumer;

        private readonly Mock<IHaveConversionCallbacks> conversionCallbacksProvider;

        private readonly Mock<IReflectExtensionProperties> extensionPropertyReflector;

        private readonly Mock<IConversionCallback> conversionCallback;

        private readonly Mock<IHaveDefaultConversionCallback> defaultConversionCallbackProvider;

        private readonly AssignExtensionProperties testee;

        public AssignExtensionPropertiesTest()
        {
            this.consumer = new Mock<IConsumeConfiguration>();
            this.extensionPropertyReflector = new Mock<IReflectExtensionProperties>();
            this.conversionCallbacksProvider = new Mock<IHaveConversionCallbacks>();
            this.defaultConversionCallbackProvider = new Mock<IHaveDefaultConversionCallback>();
            this.conversionCallback = new Mock<IConversionCallback>();

            this.testee = new AssignExtensionProperties();
        }

        [Fact]
        public void Assign_ShouldReflectPropertiesOfExtensions()
        {
            this.SetupEmptyConsumerConfiguration();

            this.testee.Assign(this.extensionPropertyReflector.Object, Mock.Of<IExtension>(), this.consumer.Object, this.conversionCallbacksProvider.Object, this.defaultConversionCallbackProvider.Object);

            this.extensionPropertyReflector.Verify(r => r.Reflect(It.IsAny<IExtension>()));
        }

        [Fact]
        public void Assign_ShouldAcquireConversionCallbacks()
        {
            this.SetupEmptyConsumerConfiguration();

            this.testee.Assign(this.extensionPropertyReflector.Object, Mock.Of<IExtension>(), this.consumer.Object, this.conversionCallbacksProvider.Object, this.defaultConversionCallbackProvider.Object);

            this.conversionCallbacksProvider.VerifyGet(x => x.ConversionCallbacks);
        }

        [Fact]
        public void Assign_ShouldAcquireDefaultConversionCallback()
        {
            this.SetupEmptyConsumerConfiguration();

            this.testee.Assign(this.extensionPropertyReflector.Object, Mock.Of<IExtension>(), this.consumer.Object, this.conversionCallbacksProvider.Object, this.defaultConversionCallbackProvider.Object);

            this.defaultConversionCallbackProvider.VerifyGet(x => x.DefaultConversionCallback);
        }

        [Fact]
        public void Assign_WhenReflectedPropertyMatchesConfiguration_ShouldAcquireCallback()
        {
            this.SetupConversionCallbackReturnsInput();

            var dictionary = new Dictionary<string, IConversionCallback> { { SomeExtensionPropertyName, this.conversionCallback.Object } };
            this.conversionCallbacksProvider.Setup(x => x.ConversionCallbacks).Returns(dictionary);

            PropertyInfo propertyInfo = GetSomePropertyPropertyInfo();
            this.extensionPropertyReflector.Setup(x => x.Reflect(It.IsAny<IExtension>())).Returns(new List<PropertyInfo> { propertyInfo });
            this.consumer.Setup(x => x.Configuration).Returns(new Dictionary<string, string> { { SomeExtensionPropertyName, SomeExtensionPropertyValue } });

            var someExtension = new SomeExtension();
            this.testee.Assign(this.extensionPropertyReflector.Object, someExtension, this.consumer.Object, this.conversionCallbacksProvider.Object, this.defaultConversionCallbackProvider.Object);

            this.conversionCallback.Verify(callback => callback.Convert(SomeExtensionPropertyValue, propertyInfo));
            someExtension.SomeProperty.Should().Be(SomeExtensionPropertyValue);
        }

        [Fact]
        public void Assign_WhenNoConversionCallbackFound_ShouldUseDefaultCallback()
        {
            this.SetupConversionCallbackReturnsInput();

            this.conversionCallbacksProvider.Setup(x => x.ConversionCallbacks).Returns(new Dictionary<string, IConversionCallback>());
            this.defaultConversionCallbackProvider.Setup(x => x.DefaultConversionCallback).Returns(this.conversionCallback.Object);

            PropertyInfo propertyInfo = GetSomePropertyPropertyInfo();
            this.extensionPropertyReflector.Setup(x => x.Reflect(It.IsAny<IExtension>())).Returns(new List<PropertyInfo> { propertyInfo });
            this.consumer.Setup(x => x.Configuration).Returns(new Dictionary<string, string> { { SomeExtensionPropertyName, SomeExtensionPropertyValue } });

            var someExtension = new SomeExtension();
            this.testee.Assign(this.extensionPropertyReflector.Object, someExtension, this.consumer.Object, this.conversionCallbacksProvider.Object, this.defaultConversionCallbackProvider.Object);

            this.conversionCallback.Verify(callback => callback.Convert(SomeExtensionPropertyValue, propertyInfo));
            someExtension.SomeProperty.Should().Be(SomeExtensionPropertyValue);
        }

        private static PropertyInfo GetSomePropertyPropertyInfo()
        {
            return typeof(SomeExtension).GetProperty(SomeExtensionPropertyName);
        }

        private void SetupEmptyConsumerConfiguration()
        {
            this.consumer.Setup(x => x.Configuration).Returns(new Dictionary<string, string>());
        }

        private void SetupConversionCallbackReturnsInput()
        {
            string interceptedValue = null;
            this.conversionCallback.Setup(callback => callback.Convert(It.IsAny<string>(), It.IsAny<PropertyInfo>()))
                .Callback<string, PropertyInfo>((value, info) => interceptedValue = value)
                .Returns(() => interceptedValue);
        }

        private class SomeExtension : IExtension
        {
            public string SomeProperty { get; private set; }

            /// <inheritdoc />
            public string Name
            {
                get
                {
                    return this.GetType().FullNameToString();
                }
            }

            public string Describe()
            {
                return string.Empty;
            }
        }
    }
}
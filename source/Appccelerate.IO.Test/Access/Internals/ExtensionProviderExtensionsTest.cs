//-------------------------------------------------------------------------------
// <copyright file="ExtensionProviderExtensionsTest.cs" company="Appccelerate">
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

namespace Appccelerate.IO.Access.Internals
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using FakeItEasy;
    using FluentAssertions;
    using Xunit;

    public class ExtensionProviderExtensionsTest
    {
        private const int ExpectedReturnValue = 1;

        private readonly IExtensionProvider<IExtension> provider;

        private readonly MemoryStream expectedReturnStream = new MemoryStream();

        private readonly IExtension extension;

        private Exception exception;

        private string stringParameter;

        private bool boolParameter;

        private bool throwException;

        private int intParameter;

        public ExtensionProviderExtensionsTest()
        {
            this.provider = A.Fake<IExtensionProvider<IExtension>>();
            this.extension = A.Fake<IExtension>();

            this.exception = new Exception();
        }

        public interface IExtension
        {
            void BeginDo(bool s);

            void EndDo(bool s);

            void BeginDo(string s);

            void EndDo(string s);

            void FailDo(ref Exception exception);

            void BeginDoReturn(string s);

            void EndDoReturn(int result, string s);

            void BeginDoReturn(bool s);

            void EndDoReturn(int result, bool s);

            void BeginDoReturn(int s);

            void EndDoReturn(Stream result, int s);

            void FailDoReturn(ref Exception exception);
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingAction_MustCallBeginWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.BeginDo(ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingAction_MustCallEndWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.EndDo(ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingAction_MustCallActionWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            this.stringParameter.Should().Be(ExpectedParameter);
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingAction_MustCallFailWithCorrectExceptionAndRethrow()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<Exception>();
            A.CallTo(() => this.extension.FailDo(ref this.exception)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingAction_MustCallFailWithCorrectExceptionAndRethrowExchangedException()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensionsWithExceptionExchangingExtension();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingActionWithOverload_MustCallBeginWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.BeginDo(ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingActionWithOverload_MustCallEndWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.EndDo(ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingActionWithOverload_MustCallActionWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            this.boolParameter.Should().Be(ExpectedParameter);
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingActionWithOverload_MustCallFailWithCorrectExceptionAndRethrow()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<Exception>();

            A.CallTo(() => this.extension.FailDo(ref this.exception)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingActionWithOverload_MustCallFailWithCorrectExceptionAndRethrowExchangedException()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensionsWithExceptionExchangingExtension();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFunc_MustCallBeginWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.BeginDoReturn(ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFunc_MustCallEndWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.EndDoReturn(ExpectedReturnValue, ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFunc_MustCallActionWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            int result = this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            this.stringParameter.Should().Be(ExpectedParameter);
            result.Should().Be(ExpectedReturnValue);
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFunc_MustCallFailWithCorrectExceptionAndRethrow()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<Exception>();

            A.CallTo(() => this.extension.FailDoReturn(ref this.exception)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFunc_MustCallFailWithCorrectExceptionAndRethrowExchangedException()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensionsWithExceptionExchangingExtension();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverload_MustCallBeginWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.BeginDoReturn(ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverload_MustCallEndWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.EndDoReturn(ExpectedReturnValue, ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverload_MustCallActionWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            int result = this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            this.boolParameter.Should().Be(ExpectedParameter);
            result.Should().Be(ExpectedReturnValue);
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverload_MustCallFailWithCorrectExceptionAndRethrow()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<Exception>();
            A.CallTo(() => this.extension.FailDoReturn(ref this.exception)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverload_MustCallFailWithCorrectExceptionAndRethrowExchangedException()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensionsWithExceptionExchangingExtension();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverloadAndExtensionUsingReturnTypeMapping_MustCallBeginWithCorrectParameters()
        {
            const int ExpectedParameter = 42;

            this.SetupExtensions();

            this.provider.SurroundWithExtension<IExtension, Stream>(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.BeginDoReturn(ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverloadAndExtensionUsingReturnTypeMapping_MustCallEndWithCorrectParameters()
        {
            const int ExpectedParameter = 42;

            this.SetupExtensions();

            this.provider.SurroundWithExtension<IExtension, Stream>(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            A.CallTo(() => this.extension.EndDoReturn(this.expectedReturnStream, ExpectedParameter)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverloadAndExtensionUsingReturnTypeMapping_MustCallActionWithCorrectParameters()
        {
            const int ExpectedParameter = 42;

            this.SetupExtensions();

            Stream result = this.provider.SurroundWithExtension<IExtension, Stream>(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            this.intParameter.Should().Be(ExpectedParameter);
            result.Should().Be(this.expectedReturnStream);
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverloadAndExtensionUsingReturnTypeMapping_MustCallFailWithCorrectExceptionAndRethrow()
        {
            const int ExpectedParameter = 42;

            this.SetupExtensions();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension<IExtension, Stream>(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<Exception>();
            A.CallTo(() => this.extension.FailDoReturn(ref this.exception)).MustHaveHappened();
        }

        [Fact]
        public void SurroundWithExtensions_WhenUsingFuncWithOverloadAndExtensionUsingReturnTypeMapping_MustCallFailWithCorrectExceptionAndRethrowExchangedException()
        {
            const int ExpectedParameter = 42;

            this.SetupExtensionsWithExceptionExchangingExtension();
            this.SetupThrowsException();

            Action action = () => this.provider.SurroundWithExtension<IExtension, Stream>(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            action.ShouldThrow<InvalidOperationException>();
        }

        private void Do(bool parameter)
        {
            if (this.throwException)
            {
                throw this.exception;
            }

            this.boolParameter = parameter;
        }

        private void Do(string parameter)
        {
            if (this.throwException)
            {
                throw this.exception;
            }

            this.stringParameter = parameter;
        }

        private int DoReturn(string parameter)
        {
            if (this.throwException)
            {
                throw this.exception;
            }

            this.stringParameter = parameter;

            return ExpectedReturnValue;
        }

        private int DoReturn(bool parameter)
        {
            if (this.throwException)
            {
                throw this.exception;
            }

            this.boolParameter = parameter;

            return ExpectedReturnValue;
        }

        private MemoryStream DoReturn(int parameter)
        {
            if (this.throwException)
            {
                throw this.exception;
            }

            this.intParameter = parameter;

            return this.expectedReturnStream;
        }

        private void SetupThrowsException()
        {
            this.throwException = true;
        }

        private void SetupExtensions()
        {
            A.CallTo(() => this.provider.Extensions).Returns(new List<IExtension> { this.extension });
        }

        private void SetupExtensionsWithExceptionExchangingExtension()
        {
            A.CallTo(() => this.provider.Extensions).Returns(new List<IExtension> { new ExceptionExchangingExtension(this.exception), new SecondExceptionExchangingExtension(this.exception) });
        }

        private class ExceptionExchangingExtension : IExtension
        {
            private readonly Exception exceptionToThrow;

            public ExceptionExchangingExtension(Exception exceptionToThrow)
            {
                this.exceptionToThrow = exceptionToThrow;
            }

            public void BeginDo(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDo(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void BeginDo(string s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDo(string s)
            {
                throw this.exceptionToThrow;
            }

            public void FailDo(ref Exception exception)
            {
                exception = new InvalidOperationException();
            }

            public void BeginDoReturn(string s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(int result, string s)
            {
                throw this.exceptionToThrow;
            }

            public void BeginDoReturn(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(int result, bool s)
            {
                throw this.exceptionToThrow;
            }

            public void BeginDoReturn(int s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(Stream result, int s)
            {
                throw this.exceptionToThrow;
            }

            public void FailDoReturn(ref Exception exception)
            {
                exception = new InvalidOperationException();
            }
        }

        private class SecondExceptionExchangingExtension : IExtension
        {
            private readonly Exception exceptionToThrow;

            public SecondExceptionExchangingExtension(Exception exceptionToThrow)
            {
                this.exceptionToThrow = exceptionToThrow;
            }

            public void BeginDo(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDo(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void BeginDo(string s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDo(string s)
            {
                throw this.exceptionToThrow;
            }

            public void FailDo(ref Exception exception)
            {
                exception = new ArgumentNullException();
            }

            public void BeginDoReturn(string s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(int result, string s)
            {
                throw this.exceptionToThrow;
            }

            public void BeginDoReturn(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(int result, bool s)
            {
                throw this.exceptionToThrow;
            }

            public void BeginDoReturn(int s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(Stream result, int s)
            {
                throw this.exceptionToThrow;
            }

            public void FailDoReturn(ref Exception exception)
            {
                exception = new ArgumentNullException();
            }
        }
    }
}
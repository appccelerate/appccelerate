//-------------------------------------------------------------------------------
// <copyright file="EnsureTest.cs" company="Appccelerate">
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

namespace Appccelerate
{
    using System;

    using FluentAssertions;

    using Xunit;

    public class EnsureTest
    {
        [Fact]
        public void ArgumentNotNull_WhenArgumentNull_MustThrow()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentNotNull((string)null, "argument"));
            ex.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentNotNull_WhenArgumentNameIsNullOrEmpty_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNull(new object(), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNull(new object(), string.Empty));

            var ex1 = Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentNotNull((object)null, null));
            ex1.ParamName.Should().BeNull();

            var ex2 = Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentNotNull((object)null, string.Empty));
            ex2.ParamName.Should().Be(string.Empty);
        }

        [Fact]
        public void ArgumentNotNull_MustThrowWithProvidedArgumentName()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentNotNull((object)null, "argument"));
            ex.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentNotNullOrEmpty_WhenArgumentNullOrEmpty_MustThrow()
        {
            var ex1 = Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentNotNullOrEmpty(null, "argument"));
            ex1.ParamName.Should().Be("argument");

            var ex2 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentNotNullOrEmpty(string.Empty, "argument"));
            ex2.ParamName.Should().Be("argument");

            var ex3 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentNotNullOrEmpty(string.Empty, "argument"));
            ex3.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentNotNullOrEmpty_WhenArgumentNotNullOrEmpty_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNullOrEmpty("value", "argument"));
        }

        [Fact]
        public void ArgumentNotNullOrEmpty_WhenArgumentNameIsNullOrEmpty_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNullOrEmpty("string", null));
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNullOrEmpty("string", string.Empty));

            var ex1 = Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentNotNullOrEmpty(null, null));
            ex1.ParamName.Should().BeNull();

            var ex2 = Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentNotNullOrEmpty(null, string.Empty));
            ex2.ParamName.Should().Be(string.Empty);

            var ex3 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentNotNullOrEmpty(string.Empty, null));
            ex3.ParamName.Should().BeNull();

            var ex4 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentNotNullOrEmpty(string.Empty, string.Empty));
            ex4.ParamName.Should().Be(string.Empty);
        }

        [Fact]
        public void ArgumentNotNullOrEmpty_MustThrowWithProvidedArgumentName()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentNotNullOrEmpty(null, "argument"));
            ex.ParamName.Should().Be("argument");

            var ex2 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentNotNullOrEmpty(string.Empty, "argument"));
            ex2.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentNotNegative_WhenArgumentNegative_MustThrow()
        {
            var ex1 = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentNotNegative(-1, "argument"));
            ex1.ParamName.Should().Be("argument");

            var ex2 = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentNotNegative(int.MinValue, "argument"));
            ex2.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentNotNegative_WhenArgumentPositive_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNegative(1, "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNegative(int.MaxValue, "argument"));
        }

        [Fact]
        public void ArgumentNotNegative_WhenArgumentZero_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNegative(0, "argument"));
        }

        [Fact]
        public void ArgumentNotNegative_WhenArgumentNameIsNullOrEmpty_MustNotThrow()
        {
            var ex1 = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentNotNegative(-1, null));
            ex1.ParamName.Should().BeNull();

            var ex2 = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentNotNegative(-1, string.Empty));
            ex2.ParamName.Should().Be(string.Empty);

            var ex3 = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentNotNegative(int.MinValue, null));
            ex3.ParamName.Should().BeNull();

            var ex4 = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentNotNegative(int.MinValue, string.Empty));
            ex4.ParamName.Should().Be(string.Empty);

            Assert.DoesNotThrow(() => Ensure.ArgumentNotNegative(1, null));
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNegative(1, string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNegative(int.MaxValue, null));
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNegative(int.MaxValue, string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNegative(0, null));
            Assert.DoesNotThrow(() => Ensure.ArgumentNotNegative(0, string.Empty));
        }

        [Fact]
        public void ArgumentNotNegative_MustThrowWithProvidedArgumentName()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentNotNegative(-1, "argument"));
            ex.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentNotNegative_MustThrowWithProvidedArgumentValue()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentNotNegative(-1, "argument"));
            ex.ActualValue.Should().Be(-1);
        }

        [Fact]
        public void ArgumentAssignableFrom_WhenArgumentNotAssignable_MustThrow()
        {
            var ex1 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new NotImplementedClass(), "argument"));
            ex1.ParamName.Should().Be("argument");

            var ex2 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentAssignableFrom(typeof(ImplementedClass), new ImplementedInheritedClass(), "argument"));
            ex2.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentAssignableFrom_WhenArgumentAsssignable_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new ImplementedClass(), "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new InheritedClass(), "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new ImplementedInheritedClass(), "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInheritedInterface), new ImplementedInheritedClass(), "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(ImplementedClass), new InheritedClass(), "argument"));
        }

        [Fact]
        public void ArgumentAssignableFrom_WhenArgumentNameIsNullOrEmpty_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new ImplementedClass(), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new InheritedClass(), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new ImplementedInheritedClass(), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInheritedInterface), new ImplementedInheritedClass(), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(ImplementedClass), new InheritedClass(), null));

            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new ImplementedClass(), string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new InheritedClass(), string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new ImplementedInheritedClass(), string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(IInheritedInterface), new ImplementedInheritedClass(), string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentAssignableFrom(typeof(ImplementedClass), new InheritedClass(), string.Empty));

            var ex1 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new NotImplementedClass(), null));
            ex1.ParamName.Should().BeNull();

            var ex2 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentAssignableFrom(typeof(ImplementedClass), new ImplementedInheritedClass(), null));
            ex2.ParamName.Should().BeNull();

            var ex3 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new NotImplementedClass(), string.Empty));
            ex3.ParamName.Should().Be(string.Empty);

            var ex4 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentAssignableFrom(typeof(ImplementedClass), new ImplementedInheritedClass(), string.Empty));
            ex4.ParamName.Should().Be(string.Empty);
        }

        [Fact]
        public void ArgumentAssignableFrom_MustThrowWithProvidedArgumentName()
        {
            var ex = Assert.Throws<ArgumentException>(() => Ensure.ArgumentAssignableFrom(typeof(IInterface), new NotImplementedClass(), "argument"));
            ex.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentTypeAssignableFrom_WhenTypeIsNotAsssignable_MustThrow()
        {
            var ex1 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(NotImplementedClass), "argument"));
            ex1.ParamName.Should().Be("argument");

            var ex2 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(IOtherInterface), "argument"));
            ex2.ParamName.Should().Be("argument");

            var ex3 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(ImplementedClass), typeof(IInterface), "argument"));
            ex3.ParamName.Should().Be("argument");

            var ex4 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInheritedInterface), typeof(IInterface), "argument"));
            ex4.ParamName.Should().Be("argument");

            var ex5 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(ImplementedClass), typeof(ImplementedInheritedClass), "argument"));
            ex5.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentTypeAssignableFrom_WhenTypeIsAsssignable_MustNotThrows()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(ImplementedClass), "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(IInheritedInterface), "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(InheritedClass), "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(ImplementedInheritedClass), "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInheritedInterface), typeof(ImplementedInheritedClass), "argument"));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(ImplementedClass), typeof(InheritedClass), "argument"));
        }

        [Fact]
        public void ArgumentTypeAssignableFrom_WhenArgumentNameIsNullOrEmpty_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(ImplementedClass), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(IInheritedInterface), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(InheritedClass), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(ImplementedInheritedClass), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInheritedInterface), typeof(ImplementedInheritedClass), null));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(ImplementedClass), typeof(InheritedClass), null));

            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(ImplementedClass), string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(IInheritedInterface), string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(InheritedClass), string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(ImplementedInheritedClass), string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInheritedInterface), typeof(ImplementedInheritedClass), string.Empty));
            Assert.DoesNotThrow(() => Ensure.ArgumentTypeAssignableFrom(typeof(ImplementedClass), typeof(InheritedClass), string.Empty));

            var ex1 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(NotImplementedClass), null));
            ex1.ParamName.Should().BeNull();

            var ex2 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(IOtherInterface), null));
            ex2.ParamName.Should().BeNull();

            var ex3 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(ImplementedClass), typeof(IInterface), null));
            ex3.ParamName.Should().BeNull();

            var ex4 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInheritedInterface), typeof(IInterface), null));
            ex4.ParamName.Should().BeNull();

            var ex5 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(ImplementedClass), typeof(ImplementedInheritedClass), null));
            ex5.ParamName.Should().BeNull();

            var ex6 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(NotImplementedClass), string.Empty));
            ex6.ParamName.Should().Be(string.Empty);

            var ex7 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(IOtherInterface), string.Empty));
            ex7.ParamName.Should().Be(string.Empty);

            var ex8 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(ImplementedClass), typeof(IInterface), string.Empty));
            ex8.ParamName.Should().Be(string.Empty);

            var ex9 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInheritedInterface), typeof(IInterface), string.Empty));
            ex9.ParamName.Should().Be(string.Empty);

            var ex0 = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(ImplementedClass), typeof(ImplementedInheritedClass), string.Empty));
            ex0.ParamName.Should().Be(string.Empty);
        }

        [Fact]
        public void ArgumentTypeAssignableFrom_MustThrowWithProvidedArgumentName()
        {
            var ex = Assert.Throws<ArgumentException>(() => Ensure.ArgumentTypeAssignableFrom(typeof(IInterface), typeof(NotImplementedClass), "argument"));
            ex.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentInRange_WhenMessageIsNullOrEmpty_MustThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentInRange(true, string.Empty, "argument", null));
            Assert.Throws<ArgumentException>(() => Ensure.ArgumentInRange(true, string.Empty, "argument", string.Empty));
        }

        [Fact]
        public void ArgumentInRange_WhenArgumentValueIsNull_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentInRange(true, (string)null, "argument", "message"));
        }

        [Fact]
        public void ArgumentInRange_WhenArgumentNameIsNullOrEmpty_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentInRange(true, string.Empty, null, "message"));
            Assert.DoesNotThrow(() => Ensure.ArgumentInRange(true, string.Empty, string.Empty, "message"));
        }

        [Fact]
        public void ArgumentInRange_WhenMatch_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentInRange(true, "value", "argument", "message"));
        }

        [Fact]
        public void ArgumentInRange_WhenNotMatch_MustThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentInRange(false, "value", "argument", "message"));
        }

        [Fact]
        public void ArgumentInRange_MustThrowWithProvidedMessage()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentInRange(false, "value", "argument", "message"));
            ex.Message.StartsWith("message").Should().BeTrue();
        }

        [Fact]
        public void ArgumentInRange_MustThrowWithProvidedArgumentName()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentInRange(false, "value", "argument", "message"));
            ex.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentInRange_MustThrowWithProvidedArgumentValue()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentInRange(false, "value", "argument", "message"));
            ex.ActualValue.Should().Be("value");
        }

        [Fact]
        public void ArgumentInRange_MustFormatMessageUsingParameters()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentInRange(false, "value", "argument", "{0}"));
            ex.Message.StartsWith("value").Should().BeTrue();

            ex = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentInRange(false, "value", "argument", "{1}"));
            ex.Message.StartsWith("argument").Should().BeTrue();
        }

        [Fact]
        public void ArgumentInRangeWithoutParameter_WhenMessageIsNullOrEmpty_MustThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentInRange(true, null));
            Assert.Throws<ArgumentException>(() => Ensure.ArgumentInRange(true, string.Empty));
        }

        [Fact]
        public void ArgumentInRangeWithoutParameter_WhenMatch_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentInRange(true, "message"));
        }

        [Fact]
        public void ArgumentInRangeWithoutParameter_WhenNotMatch_MustThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentInRange(false, "message"));
        }

        [Fact]
        public void ArgumentInRangeWithoutParameter_MustThrowWithProvidedMessage()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ArgumentInRange(false, "message"));
            "message".Should().Be(ex.Message);
        }

        [Fact]
        public void ArgumentMatches_WhenMessageIsNullOrEmpty_MustThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentMatches(true, string.Empty, "argument", null));
            Assert.Throws<ArgumentException>(() => Ensure.ArgumentMatches(true, string.Empty, "argument", string.Empty));
        }

        [Fact]
        public void ArgumentMatches_WhenArgumentValueIsNull_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentMatches<string>(true, null, "argument", "message"));
        }

        [Fact]
        public void ArgumentMatches_WhenArgumentNameIsNullOrEmpty_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentMatches(true, string.Empty, null, "message"));
            Assert.DoesNotThrow(() => Ensure.ArgumentMatches(true, string.Empty, string.Empty, "message"));
        }

        [Fact]
        public void ArgumentMatches_WhenMatch_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentMatches(true, "value", "argument", "message"));
        }

        [Fact]
        public void ArgumentMatches_WhenNotMatch_MustThrow()
        {
            Assert.Throws<ArgumentException>(() => Ensure.ArgumentMatches(false, "value", "argument", "message"));
        }

        [Fact]
        public void ArgumentMatches_MustThrowWithProvidedMessage()
        {
            var ex = Assert.Throws<ArgumentException>(() => Ensure.ArgumentMatches(false, "value", "argument", "message"));
            ex.Message.StartsWith("message").Should().BeTrue();
        }

        [Fact]
        public void ArgumentMatches_MustThrowWithProvidedArgumentName()
        {
            var ex = Assert.Throws<ArgumentException>(() => Ensure.ArgumentMatches(false, "value", "argument", "message"));
            ex.ParamName.Should().Be("argument");
        }

        [Fact]
        public void ArgumentMatches_MustFormatMessageUsingParameters()
        {
            var ex = Assert.Throws<ArgumentException>(() => Ensure.ArgumentMatches(false, "value", "argument", "{0}"));
            ex.Message.StartsWith("value").Should().BeTrue();

            ex = Assert.Throws<ArgumentException>(() => Ensure.ArgumentMatches(false, "value", "argument", "{1}"));
            ex.Message.StartsWith("argument").Should().BeTrue();
        }

        [Fact]
        public void ArgumentMatchesWithoutParameter_WhenMessageIsNullOrEmpty_MustThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Ensure.ArgumentMatches(true, null));
            Assert.Throws<ArgumentException>(() => Ensure.ArgumentMatches(true, string.Empty));
        }

        [Fact]
        public void ArgumentMatchesWithoutParameter_WhenMatch_MustNotThrows()
        {
            Assert.DoesNotThrow(() => Ensure.ArgumentMatches(true, "message"));
        }

        [Fact]
        public void ArgumentMatchesWithoutParameter_WhenNotMatch_MustThrow()
        {
            Assert.Throws<ArgumentException>(() => Ensure.ArgumentMatches(false, "message"));
        }

        [Fact]
        public void ArgumentMatchesWithoutParameter_MustThrowWithProvidedMessage()
        {
            var ex = Assert.Throws<ArgumentException>(() => Ensure.ArgumentMatches(false, "message"));
            ex.Message.StartsWith("message").Should().BeTrue();
        }

        [Fact]
        public void OperationValid_WhenMessageIsNullOrEmpty_MustThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Ensure.OperationValid(true, null));
            Assert.Throws<ArgumentException>(() => Ensure.OperationValid(true, string.Empty));
        }

        [Fact]
        public void OperationValid_WhenMatch_MustNotThrow()
        {
            Assert.DoesNotThrow(() => Ensure.OperationValid(true, "message"));
        }

        [Fact]
        public void OperationValid_WhenNotMatch_MustThrow()
        {
            Assert.Throws<InvalidOperationException>(() => Ensure.OperationValid(false, "message"));
        }

        [Fact]
        public void OperationValid_MustThrowWithProvidedMessage()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => Ensure.OperationValid(false, "message"));
            ex.Message.Should().Be("message");
        }

        [Fact]
        public void OperationValid_MustFormatProvidedMessageWithProvidedArguments()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => Ensure.OperationValid(false, "{0};{1}", "argument1", "argument2"));
            ex.Message.Should().Be("argument1;argument2");

            ex = Assert.Throws<InvalidOperationException>(() => Ensure.OperationValid(false, "message", new object[0]));
            ex.Message.Should().Be("message");

            ex = Assert.Throws<InvalidOperationException>(() => Ensure.OperationValid(false, "message", null));
            ex.Message.Should().Be("message");
        }

        [Fact]
        public void OperationNotValid_WhenMessageIsNullOrEmpty_MustThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Ensure.OperationNotValid(null));
            Assert.Throws<ArgumentException>(() => Ensure.OperationNotValid(string.Empty));
        }

        [Fact]
        public void OperationNotValid_MustThrowAllways()
        {
            Assert.Throws<InvalidOperationException>(() => Ensure.OperationNotValid("message"));
        }

        [Fact]
        public void OperationNotValid_MustThrowWithProvidedMessage()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => Ensure.OperationNotValid("message"));
            ex.Message.Should().Be("message");
        }

        [Fact]
        public void OperationNotValid_MustFormatProvidedMessageWithProvidedArguments()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => Ensure.OperationNotValid("{0};{1}", "argument1", "argument2"));
            ex.Message.Should().Be("argument1;argument2");

            ex = Assert.Throws<InvalidOperationException>(() => Ensure.OperationNotValid("message", new object[0]));
            ex.Message.Should().Be("message");

            ex = Assert.Throws<InvalidOperationException>(() => Ensure.OperationNotValid("message", null));
            ex.Message.Should().Be("message");
        }

        [Fact]
        public void OperationSupported_WhenMessageIsNullOrEmpty_MustThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Ensure.OperationSupported(true, null));
            Assert.Throws<ArgumentException>(() => Ensure.OperationSupported(true, string.Empty));
        }

        [Fact]
        public void OperationSupported_WhenMatch_MustThrow()
        {
            Assert.DoesNotThrow(() => Ensure.OperationSupported(true, "message"));
        }

        [Fact]
        public void OperationSupported_WhenNotMatch_MustThrow()
        {
            Assert.Throws<NotSupportedException>(() => Ensure.OperationSupported(false, "message"));
        }

        [Fact]
        public void OperationSupported_MustThrowWithProvidedMessage()
        {
            var ex = Assert.Throws<NotSupportedException>(() => Ensure.OperationSupported(false, "message"));
            ex.Message.Should().Be("message");
        }

        [Fact]
        public void OperationSupported_MustFormatProvidedMessageWithProvidedArguments()
        {
            var ex = Assert.Throws<NotSupportedException>(() => Ensure.OperationSupported(false, "{0};{1}", "argument1", "argument2"));
            ex.Message.Should().Be("argument1;argument2");

            ex = Assert.Throws<NotSupportedException>(() => Ensure.OperationSupported(false, "message", new object[0]));
            ex.Message.Should().Be("message");

            ex = Assert.Throws<NotSupportedException>(() => Ensure.OperationSupported(false, "message", null));
            ex.Message.Should().Be("message");
        }

        [Fact]
        public void OperationNotSupported_WhenMessageIsNullOrEmpty_MustThrow()
        {
            Assert.Throws<ArgumentNullException>(() => Ensure.OperationNotSupported(null));
            Assert.Throws<ArgumentException>(() => Ensure.OperationNotSupported(string.Empty));
        }

        [Fact]
        public void OperationNotSupported_MustThrowAllways()
        {
            Assert.Throws<NotSupportedException>(() => Ensure.OperationNotSupported("message"));
        }

        [Fact]
        public void OperationNotSupported_MustThrowWithProvidedMessage()
        {
            var ex = Assert.Throws<NotSupportedException>(() => Ensure.OperationNotSupported("message"));
            ex.Message.Should().Be("message");
        }

        [Fact]
        public void OperationNotSupported_MustFormatProvidedMessageWithProvidedArguments()
        {
            var ex = Assert.Throws<NotSupportedException>(() => Ensure.OperationNotSupported("{0};{1}", "argument1", "argument2"));
            ex.Message.Should().Be("argument1;argument2");

            ex = Assert.Throws<NotSupportedException>(() => Ensure.OperationNotSupported("message", new object[0]));
            ex.Message.Should().Be("message");

            ex = Assert.Throws<NotSupportedException>(() => Ensure.OperationNotSupported("message", null));
            ex.Message.Should().Be("message");
        }

        private interface IInterface
        {
        }

        private interface IInheritedInterface : IInterface
        {
        }

        private interface IOtherInterface
        {
        }

        private class ImplementedClass : IInterface
        {
        }

        private class NotImplementedClass
        {
        }

        private class InheritedClass : ImplementedClass
        {
        }

        private class ImplementedInheritedClass : IInheritedInterface
        {
        }
    }
}
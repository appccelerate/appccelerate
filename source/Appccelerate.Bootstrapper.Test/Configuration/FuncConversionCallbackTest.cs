//-------------------------------------------------------------------------------
// <copyright file="FuncConversionCallbackTest.cs" company="Appccelerate">
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
    using System.Reflection;

    using Appccelerate.Bootstrapper.Configuration.Internals;
    using FluentAssertions;
    using Xunit;

    public class FuncConversionCallbackTest
    {
        public string TestProperty { get; set; }

        [Fact]
        public void Convert_ShouldExecuteConversionCallback()
        {
            bool wasCalled = false;
            var testee = new FuncConversionCallback((value, info) =>
            {
                wasCalled = true;
                return null;
            });

            testee.Convert("AnyValue", GetTestPropertyInfo());

            wasCalled.Should().BeTrue();
        }

        [Fact]
        public void Convert_ShouldReturnConvertedValue()
        {
            var expected = new object();
            var testee = new FuncConversionCallback((value, info) => expected);

            var result = testee.Convert("AnyValue", GetTestPropertyInfo());

            result.Should().Be(expected);
        }

        private static PropertyInfo GetTestPropertyInfo()
        {
            return typeof(FuncConversionCallback).GetProperty("TestProperty");
        }
    }
}
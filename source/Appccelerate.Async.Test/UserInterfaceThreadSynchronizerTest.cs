//-------------------------------------------------------------------------------
// <copyright file="UserInterfaceThreadSynchronizerTest.cs" company="Appccelerate">
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

namespace Appccelerate.Async
{
    using System;
    using System.Threading;

    using FakeItEasy;

    using FluentAssertions;
    
    using Xunit;

    public class UserInterfaceThreadSynchronizerTest
    {
        private readonly UserInterfaceThreadSynchronizer testee;

        private readonly TestSynchronizationContext synchronizationContext;
        private readonly IUserInterfaceThreadSynchronizerLogExtension logExtension;

        public UserInterfaceThreadSynchronizerTest()
        {
            this.synchronizationContext = new TestSynchronizationContext();
            this.logExtension = A.Fake<IUserInterfaceThreadSynchronizerLogExtension>();

            this.testee = new UserInterfaceThreadSynchronizer(this.synchronizationContext, this.logExtension);
        }
        
        [Fact]
        public void LoggingWhenSynchronousOperationThenCallsLogExtensionSynchronousLogging()
        {
            Action action = () => { };  
            this.testee.Execute(action);

            A.CallTo(() => 
                this.logExtension.LogSynchronous(action, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name))
                .MustHaveHappened();
        }

        [Fact]
        public void LoggingWhenSynchronousOperationWithReturnValueThenCallsLogExtensionSynchronousLogging()
        {
            Action action = () => { };
            this.testee.Execute(action);

            A.CallTo(() =>
                this.logExtension.LogSynchronous(action, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name))
                .MustHaveHappened();
        }

        [Fact]
        public void LoggingWhenAsynchronousOperationThenCallsLogExtensionAsynchronousLogging()
        {
            Action action = () => { };
            this.testee.ExecuteAsync(action);

            A.CallTo(() =>
                this.logExtension.LogAsynchronous(action, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name))
                .MustHaveHappened();
        }

        [Fact]
        public void Execute()
        {
            bool executed = false;

            Action action = () => { executed = true; };

            this.testee.Execute(action);

            executed
                .Should().BeTrue("action not executed.");

            this.AssertSyncExecution();
        }

        [Fact]
        public void ExecuteOneParameter()
        {
            const int Value = 42;
            int? parameter = null;

            this.testee.Execute(value => { parameter = value; }, Value);

            parameter.Should().Be(Value);
            this.AssertSyncExecution();
        }

        [Fact]
        public void ExecuteTwoParameters()
        {
            const int Value1 = 42;
            const string Value2 = "fortyTwo";
            int? parameter1 = null;
            string parameter2 = null;

            this.testee.Execute(
                (value1, value2) =>
                    {
                        parameter1 = value1;
                        parameter2 = value2;
                    },
                Value1,
                Value2);

            parameter1.Should().Be(Value1);
            parameter2.Should().Be(Value2);
            this.AssertSyncExecution();
        }

        [Fact]
        public void ExecuteThreeParameters()
        {
            const int Value1 = 42;
            const string Value2 = "fortyTwo";
            const decimal Value3 = 4.5m;
            int? parameter1 = null;
            string parameter2 = null;
            decimal? parameter3 = null;

            this.testee.Execute(
                (value1, value2, value3) =>
                    {
                        parameter1 = value1;
                        parameter2 = value2;
                        parameter3 = value3;
                    },
                Value1,
                Value2,
                Value3);

            parameter1.Should().Be(Value1);
            parameter2.Should().Be(Value2);
            parameter3.Should().Be(Value3);
            this.AssertSyncExecution();
        }

        [Fact]
        public void ExecuteFourParameters()
        {
            const int Value1 = 42;
            const string Value2 = "fortyTwo";
            const decimal Value3 = 4.5m;
            const int Value4 = 17;
            int? parameter1 = null;
            string parameter2 = null;
            decimal? parameter3 = null;
            int? parameter4 = null;

            this.testee.Execute(
                (value1, value2, value3, value4) =>
                    {
                        parameter1 = value1;
                        parameter2 = value2;
                        parameter3 = value3;
                        parameter4 = value4;
                    },
                Value1,
                Value2,
                Value3,
                Value4);

            parameter1.Should().Be(Value1);
            parameter2.Should().Be(Value2);
            parameter3.Should().Be(Value3);
            parameter4.Should().Be(Value4);
            this.AssertSyncExecution();
        }

        [Fact]
        public void ExecuteAsync()
        {
            bool executed = false;

            this.testee.ExecuteAsync(() => { executed = true; });

            executed.Should().BeTrue("action not executed.");
            this.AssertAsyncExecution();
        }

        [Fact]
        public void ExecuteOneParameterAsync()
        {
            const int Value = 42;
            int? parameter = null;

            this.testee.ExecuteAsync(value => { parameter = value; }, Value);

            parameter.Should().Be(Value);
            this.AssertAsyncExecution();
        }

        [Fact]
        public void ExecuteTwoParametersAsync()
        {
            const int Value1 = 42;
            const string Value2 = "fortyTwo";
            int? parameter1 = null;
            string parameter2 = null;

            this.testee.ExecuteAsync(
                (value1, value2) =>
                {
                    parameter1 = value1;
                    parameter2 = value2;
                },
                Value1,
                Value2);

            parameter1.Should().Be(Value1);
            parameter2.Should().Be(Value2);
            this.AssertAsyncExecution();
        }

        [Fact]
        public void ExecuteThreeParametersAsync()
        {
            const int Value1 = 42;
            const string Value2 = "fortyTwo";
            const decimal Value3 = 4.5m;
            int? parameter1 = null;
            string parameter2 = null;
            decimal? parameter3 = null;

            this.testee.ExecuteAsync(
                (value1, value2, value3) =>
                {
                    parameter1 = value1;
                    parameter2 = value2;
                    parameter3 = value3;
                },
                Value1,
                Value2,
                Value3);

            parameter1.Should().Be(Value1);
            parameter2.Should().Be(Value2);
            parameter3.Should().Be(Value3);
            this.AssertAsyncExecution();
        }

        [Fact]
        public void ExecuteFourParametersAsync()
        {
            const int Value1 = 42;
            const string Value2 = "fortyTwo";
            const decimal Value3 = 4.5m;
            const int Value4 = 17;
            int? parameter1 = null;
            string parameter2 = null;
            decimal? parameter3 = null;
            int? parameter4 = null;

            this.testee.ExecuteAsync(
                (value1, value2, value3, value4) =>
                {
                    parameter1 = value1;
                    parameter2 = value2;
                    parameter3 = value3;
                    parameter4 = value4;
                },
                Value1,
                Value2,
                Value3,
                Value4);

            parameter1.Should().Be(Value1);
            parameter2.Should().Be(Value2);
            parameter3.Should().Be(Value3);
            parameter4.Should().Be(Value4);
            this.AssertAsyncExecution();
        }

        [Fact]
        public void ExecuteWithResult()
        {
            const int Value = 17;

            int result = this.testee.Execute(() => Value);

            result.Should().Be(Value);
            this.AssertSyncExecution();
        }

        [Fact]
        public void ExecuteOneParameterWithResult()
        {
            const int Value = 17;

            const int Value1 = 42;
            int? parameter1 = null;

            int result = this.testee.Execute(
                value =>
                    {
                        parameter1 = value;
                        return Value;
                    },
                Value1);

            result.Should().Be(Value);
            parameter1.Should().Be(Value1);
            this.AssertSyncExecution();
        }

        [Fact]
        public void ExecuteTwoParametersWithResult()
        {
            const int Value = 17;

            const int Value1 = 42;
            const string Value2 = "fortyTwo";
            int? parameter1 = null;
            string parameter2 = null;

            int result = this.testee.Execute(
                (value1, value2) =>
                {
                    parameter1 = value1;
                    parameter2 = value2;

                    return Value;
                },
                Value1,
                Value2);

            result.Should().Be(Value);
            parameter1.Should().Be(Value1);
            parameter2.Should().Be(Value2);
            this.AssertSyncExecution();
        }

        [Fact]
        public void ExecuteThreeParametersWithResult()
        {
            const int Value = 17;

            const int Value1 = 42;
            const string Value2 = "fortyTwo";
            const decimal Value3 = 4.5m;
            int? parameter1 = null;
            string parameter2 = null;
            decimal? parameter3 = null;

            int result = this.testee.Execute(
                (value1, value2, value3) =>
                {
                    parameter1 = value1;
                    parameter2 = value2;
                    parameter3 = value3;

                    return Value;
                },
                Value1,
                Value2,
                Value3);

            result.Should().Be(Value);
            parameter1.Should().Be(Value1);
            parameter2.Should().Be(Value2);
            parameter3.Should().Be(Value3);
            this.AssertSyncExecution();
        }

        [Fact]
        public void ExecuteFourParametersWithResult()
        {
            const int Value = 17;

            const int Value1 = 42;
            const string Value2 = "fortyTwo";
            const decimal Value3 = 4.5m;
            const int Value4 = 17;
            int? parameter1 = null;
            string parameter2 = null;
            decimal? parameter3 = null;
            int? parameter4 = null;

            int result = this.testee.Execute(
                (value1, value2, value3, value4) =>
                {
                    parameter1 = value1;
                    parameter2 = value2;
                    parameter3 = value3;
                    parameter4 = value4;

                    return Value;
                },
                Value1,
                Value2,
                Value3,
                Value4);

            result.Should().Be(Value);
            parameter1.Should().Be(Value1);
            parameter2.Should().Be(Value2);
            parameter3.Should().Be(Value3);
            parameter4.Should().Be(Value4);
            this.AssertSyncExecution();
        }

        private void AssertSyncExecution()
        {
            this.synchronizationContext.SendCalled
                .Should().BeTrue("was not synchronous");
        }

        private void AssertAsyncExecution()
        {
            this.synchronizationContext.PostCalled
                .Should().BeTrue("was not asynchronous");            
        }

        private class TestSynchronizationContext : SynchronizationContext
        {
            public bool SendCalled
            {
                get; private set;
            }

            public bool PostCalled
            {
                get;
                private set;
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                base.Send(d, state);

                this.SendCalled = true;
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                // use send to keep unit test single threaded.
                base.Send(d, state);

                this.PostCalled = true;
            }
        }
    }
}
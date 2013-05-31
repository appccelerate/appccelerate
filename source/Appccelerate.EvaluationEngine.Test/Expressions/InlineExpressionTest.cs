//-------------------------------------------------------------------------------
// <copyright file="InlineExpressionTest.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.Expressions
{
    using FluentAssertions;

    using Xunit;

    public class InlineExpressionTest
    {
        private const string AParameter = "P";
        private const int AResult = 42;

        private readonly TestQuestion question;

        public InlineExpressionTest()
        {
            this.question = new TestQuestion();
        }

        [Fact]
        public void EvaluatesFunc()
        {
            const int Result = 3;
            
            var testee = new InlineExpression<TestQuestion, string, int>(this.question, (q, p) => Result);

            int answer = testee.Evaluate(AParameter);

            answer.Should().Be(Result);
        }

        [Fact]
        public void PassesQuestionToFunc()
        {
            TestQuestion receivedQuestion = null;

            var testee = new InlineExpression<TestQuestion, string, int>(
                this.question, 
                (q, p) => this.InterceptQuestion(q, p, out receivedQuestion));

            testee.Evaluate(AParameter);

            receivedQuestion.Should().BeSameAs(this.question);
        }

        [Fact]
        public void PassesParameterToFunc()
        {
            const string Parameter = "Parameter";

            string receivedParameter = null;

            var testee = new InlineExpression<TestQuestion, string, int>(
                this.question,
                (q, p) => this.InterceptParameter(q, p, out receivedParameter));

            testee.Evaluate(Parameter);

            receivedParameter.Should().Be(Parameter);
        }

        [Fact]
        public void Describe()
        {
            var testee = new InlineExpression<TestQuestion, string, int>(this.question, (q, p) => AResult);

            string description = testee.Describe();

            description.Should().StartWith("inline expression = ");
        }

        private int InterceptQuestion(TestQuestion q, string parameter, out TestQuestion interceptedQuestion)
        {
            interceptedQuestion = q;

            return AResult;
        }

        private int InterceptParameter(TestQuestion q, string parameter, out string interceptedParameter)
        {
            interceptedParameter = parameter;

            return AResult;
        }

        private class TestQuestion : Question<string>
        {
        }
    }
}
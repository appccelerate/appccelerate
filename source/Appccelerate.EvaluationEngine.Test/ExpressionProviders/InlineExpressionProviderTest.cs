//-------------------------------------------------------------------------------
// <copyright file="InlineExpressionProviderTest.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine.ExpressionProviders
{
    using System.Collections.Generic;
    using System.Linq;

    using Appccelerate.EvaluationEngine.Expressions;

    using FluentAssertions;

    using Xunit;

    public class InlineExpressionProviderTest
    {
        [Fact]
        public void ReturnsExpressionForSpecifiedFunc()
        {
            var testee = new InlineExpressionProvider<TestQuestion, string, string, string>((q, p) => q.Value + p);

            IEnumerable<IExpression<string, string>> expressions = testee.GetExpressions(new TestQuestion { Value = "Q" }).ToList();

            expressions
                .Should().HaveCount(1);

            expressions.ElementAt(0).Evaluate("P").Should().Be("QP", "question and parameter must be passed to inline expression.");
        }

        private class TestQuestion : Question<string, string>
        {
            public string Value { get; set; }
        }
    }
}
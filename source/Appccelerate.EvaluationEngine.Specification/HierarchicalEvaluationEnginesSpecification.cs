//-------------------------------------------------------------------------------
// <copyright file="HierarchicalEvaluationEnginesSpecification.cs" company="Appccelerate">
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

namespace Appccelerate.EvaluationEngine
{
    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern.HierarchicalEngines)]
    public class When_calling_answer_on_a_child_evaluation_engine : HierarchicalEngineContext
    {
        private static string childAnswer;

        Because of = () =>
            {
                childAnswer = ChildEngine.Answer(new Question());
            };

        It should_override_parent_aggregator_with_child_aggregator = () =>
            {
                childAnswer.Should().Contain(ChildAggregator);
            };

        It should_use_expressions_from_child_and_parent = () =>
            {
                childAnswer
                    .Should().Contain(ParentExpression)
                    .And.Contain(ChildExpression);
            };

        It should_evaluate_expressions_from_parent_first = () =>
            {
                childAnswer.EndsWith(ParentExpression + ChildExpression);
            };
    }

    [Subject(Concern.HierarchicalEngines)]
    public class When_calling_answer_on_a_parent_evaluation_engine : HierarchicalEngineContext
    {
        private static string parentAnswer;

        Because of = () =>
            {
                parentAnswer = ParentEngine.Answer(new Question());
            };

        It should_use_parent_aggregator = () =>
            {
                parentAnswer.Should().Contain(ParentAggregator);
            };

        It should_use_expressions_only_from_parent = () =>
            {
                parentAnswer.ShouldNotContain(ChildExpression);
            };
    }

    [Subject(Concern.HierarchicalEngines)]
    public class HierarchicalEngineContext
    {
        protected const string ParentAggregator = "parentAggregator";
        protected const string ChildAggregator = "childAggregator";
        protected const string ParentExpression = "parentExpression";
        protected const string ChildExpression = "childExpression";
        
        Establish context = () =>
        {
            ParentEngine = new EvaluationEngine();
            ChildEngine = new EvaluationEngine(ParentEngine);

            ParentEngine.Solve<Question, string>()
            .AggregateWithExpressionAggregator(ParentAggregator, (aggregate, value) => aggregate + value)
            .ByEvaluating((q, p) => ParentExpression);

            ChildEngine.Solve<Question, string>()
                .AggregateWithExpressionAggregator(ChildAggregator, (aggregate, value) => aggregate + value)
                .ByEvaluating((q, p) => ChildExpression);
        };

        protected static IEvaluationEngine ParentEngine { get; private set; }

        protected static IEvaluationEngine ChildEngine { get; private set; }

        protected class Question : IQuestion<string>
        {
            public string Describe()
            {
                return "test question";
            }
        }
    }
}
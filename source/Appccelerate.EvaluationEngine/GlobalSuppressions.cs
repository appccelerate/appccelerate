//-------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Appccelerate">
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

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Appccelerate.EvaluationEngine.Context+ExpressionInfo")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope = "type", Target = "Appccelerate.EvaluationEngine.EvaluationEngine")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.IAggregator`3")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.IDefinition`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Scope = "type", Target = "Appccelerate.EvaluationEngine.IQuestion`2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Appccelerate.EvaluationEngine.StrategyExtensionMethods.#WithAggregatorStrategy`4(Appccelerate.EvaluationEngine.Syntax.IDefinitionSyntax`4<!!0,!!1,!!2,!!3>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Aggregators.ExpressionAggregator`3")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "SingleExpressionAggregator", Scope = "member", Target = "Appccelerate.EvaluationEngine.Aggregators.SingleExpressionAggregator`2.#CheckSingleExpression(System.Collections.Generic.IEnumerable`1<Appccelerate.EvaluationEngine.Expressions.IExpression`2<!0,!1>>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.ExpressionProviders.ExpressionProviderSet`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.ExpressionProviders.IExpressionProvider`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.ExpressionProviders.IExpressionProviderSet`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.ExpressionProviders.InlineExpressionProvider`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.ExpressionProviders.MultipleExpressionsProvider`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Expressions.InlineExpression`3")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.ExpressionProviders.SingleExpressionProvider`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Internals.Definition`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Strategies.AggregatorStrategy`3")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Strategies.AggregatorStrategy`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Syntax.DefinitionBuilder`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Syntax.IAggregatorSyntax`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Syntax.IConstraintSyntax`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "When", Scope = "member", Target = "Appccelerate.EvaluationEngine.Syntax.IConstraintSyntax`4.#When(System.Func`2<!0,System.Boolean>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Syntax.IDefinitionSyntax`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Syntax.IExpressionSyntax`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Syntax.IStrategySyntax`4")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "With", Scope = "member", Target = "Appccelerate.EvaluationEngine.Syntax.IStrategySyntax`4.#With(Appccelerate.EvaluationEngine.IStrategy`2<!1,!2>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Scope = "type", Target = "Appccelerate.EvaluationEngine.Validation.IValidationResult")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "Appccelerate.EvaluationEngine.Validation.Aggregators.ValidationAggregator`3")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Appccelerate.EvaluationEngine.Internals.Engine.CheckValueNotNull(System.Object,System.String)", Scope = "member", Target = "Appccelerate.EvaluationEngine.Internals.Engine.#Answer`2(Appccelerate.EvaluationEngine.IQuestion`2<!!0,!!1>,!!1)")]

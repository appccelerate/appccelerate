﻿//-------------------------------------------------------------------------------
// <copyright file="ConsoleLogger.cs" company="Appccelerate">
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
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Appccelerate.EvaluationEngine.Extensions;

    public class ConsoleLogger : ILogExtension
    {
        /// <summary>
        /// Logs the found answer together with information how it was found.
        /// </summary>
        /// <param name="context">The context.</param>
        public void FoundAnswer(Context context)
        {
            var expressions = from expression in context.Expressions
                              select
                                  string.Format(
                                      CultureInfo.InvariantCulture,
                                      "{0} => {1}",
                                      expression.Expression.Describe(),
                                      expression.ExpressionResult);

            string format = context.Parameter == Missing.Value
                                ? "Question = {1}{0}Answer = {3}{0}Used strategy = {4}{0}Used Aggregator = {5}{0}Expressions = {6}"
                                : "Question = {1}{0}Parameter = {2}{0}Answer = {3}{0}Used strategy = {4}{0}Used Aggregator = {5}{0}Expressions = {6}";

            string message = string.Format(
                CultureInfo.InvariantCulture,
                format,
                Environment.NewLine,
                context.Question.Describe(),
                context.Parameter,
                context.Answer,
                context.Strategy.Describe(),
                context.Aggregator.Describe(),
                expressions.Aggregate(
                    string.Empty, (aggregate, value) => aggregate + Environment.NewLine + "    " + value));

            Console.WriteLine(message);
        }
    }
}
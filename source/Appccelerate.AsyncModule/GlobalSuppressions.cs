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

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "Appccelerate.AsyncModule.Extensions.ModuleExtensionCollection.#RemoveAndDetach(System.Type)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "Appccelerate.AsyncModule.Extensions.ModuleExtensionCollection.#AddAndAttach(System.Type,System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "Appccelerate.AsyncModule.Extensions.ModuleExtensionCollection")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Scope = "member", Target = "Appccelerate.AsyncModule.Extensions.IModuleExtension.#ModuleController")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#BeforeModuleStop")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#BeforeModuleStart")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#BeforeEnqueueMessage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#BeforeConsumeMessage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#AfterModuleStop")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#AfterModuleStart")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#AfterEnqueueMessage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#AfterConsumeMessage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StopMessage", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#Stop(System.TimeSpan)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#Start()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#Run(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ModuleController", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#Initialize(System.Object,System.Int32,System.Boolean,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)", Scope = "member", Target = "Appccelerate.AsyncModule.ModuleController.#GetConsumeMessageMethodInfos(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Occured", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#UnhandledModuleExceptionOccured")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#BeforeModuleStop")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#BeforeModuleStart")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#BeforeEnqueueMessage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#BeforeConsumeMessage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#AfterModuleStop")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#AfterModuleStart")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#AfterEnqueueMessage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#AfterConsumeMessage")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#Stop(System.TimeSpan)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#Stop()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#Messages")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Module", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#Initialize(System.Object,System.Int32,System.Boolean,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Module", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#Initialize(System.Object,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Module", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#Initialize(System.Object,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Module", Scope = "member", Target = "Appccelerate.AsyncModule.IModuleController.#Initialize(System.Object)")]

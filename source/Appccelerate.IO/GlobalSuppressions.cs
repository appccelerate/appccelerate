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

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Scope = "type", Target = "Appccelerate.IO.Csv.CsvParseException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Appccelerate.IO.Csv.CsvParser.#Parse(System.String,System.Char)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable", Scope = "type", Target = "Appccelerate.IO.Csv.CsvParseException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes", Scope = "member", Target = "Appccelerate.IO.Access.IFileExtension.#EndWriteAllBytes(System.String,System.Byte[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes", Scope = "member", Target = "Appccelerate.IO.Access.IFileExtension.#BeginWriteAllBytes(System.String,System.Byte[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Appccelerate.IO.Access.IFileInfo.#MoveTo(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Appccelerate.IO.Access.IFileInfo.#CopyTo(System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Appccelerate.IO.Access.IFileInfo.#CopyTo(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Appccelerate.IO.Access.IFileExtension.#EndMove(System.String,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Appccelerate.IO.Access.IFileExtension.#EndCopy(System.String,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Appccelerate.IO.Access.IFileExtension.#EndCopy(System.String,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Appccelerate.IO.Access.IFileExtension.#BeginMove(System.String,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest", Scope = "member", Target = "Appccelerate.IO.Access.IFileExtension.#BeginCopy(System.String,System.String,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Appccelerate.IO.Access.Internals.ExtensionProviderExtensions.#SurroundWithExtension`2(Appccelerate.IO.Access.Internals.IExtensionProvider`1<!!0>,System.Linq.Expressions.Expression`1<System.Func`1<!!1>>,System.Object[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Appccelerate.IO.Access.Internals.ExtensionProviderExtensions.#SurroundWithExtension`1(Appccelerate.IO.Access.Internals.IExtensionProvider`1<!!0>,System.Linq.Expressions.Expression`1<System.Action>,System.Object[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Appccelerate.IO.Access.Internals.ExtensionProviderExtensions+Cache.#GetKeyForItem(Appccelerate.IO.Access.Internals.ExtensionProviderExtensions+Item)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "Appccelerate.IO.Access.Internals.FileInfo.#.ctor(System.IO.FileInfo)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Exit", Scope = "member", Target = "Appccelerate.IO.Access.IEnvironment.#Exit(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Option", Scope = "member", Target = "Appccelerate.IO.Access.IEnvironment.#GetFolderPath(System.Environment+SpecialFolder,System.Environment+SpecialFolderOption)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Appccelerate.IO.Access.IEnvironment.#GetLogicalDrives()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Appccelerate.IO.Access.IEnvironment.#GetCommandLineArgs()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Appccelerate.IO.Access.Internals.DriveInfo.#GetDrives()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Appccelerate.IO.Access.Internals.DriveInfo.#GetDrives()")]

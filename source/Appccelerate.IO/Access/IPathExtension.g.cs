//-------------------------------------------------------------------------------
// <copyright file="IPathExtension.cs" company="Appccelerate">
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
namespace Appccelerate.IO.Access
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Interface for path access extensions
    /// </summary>
    [CompilerGenerated]
    public interface IPathExtension
    {
        void BeginGetDirectoryName(string path);

        void EndGetDirectoryName(string result, string path);

        void FailGetDirectoryName(ref Exception exception);

        void BeginGetFileName(string path);

        void EndGetFileName(string result, string path);

        void FailGetFileName(ref Exception exception);

        void BeginGetFileNameWithoutExtension(string path);

        void EndGetFileNameWithoutExtension(string result, string path);

        void FailGetFileNameWithoutExtension(ref Exception exception);

        void BeginCombine(string path1, string path2);

        void EndCombine(string result, string path1, string path2);

        void FailCombine(ref Exception exception);

        void BeginGetRandomFileName();

        void EndGetRandomFileName(string result);

        void FailGetRandomFileName(ref Exception exception);

        void BeginChangeExtension(string path, string extension);

        void EndChangeExtension(string result, string path, string extension);

        void FailChangeExtension(ref Exception exception);

        void BeginGetExtension(string path);

        void EndGetExtension(string result, string path);

        void FailGetExtension(ref Exception exception);

        void BeginGetFullPath(string path);

        void EndGetFullPath(string result, string path);

        void FailGetFullPath(ref Exception exception);

        void BeginGetInvalidFileNameChars();

        void EndGetInvalidFileNameChars(char[] result);

        void FailGetInvalidFileNameChars(ref Exception exception);

        void BeginGetInvalidPathChars();

        void EndGetInvalidPathChars(char[] result);

        void FailGetInvalidPathChars(ref Exception exception);

        void BeginGetPathRoot(string path);

        void EndGetPathRoot(string result, string path);

        void FailGetPathRoot(ref Exception exception);

        void BeginGetTempFileName();

        void EndGetTempFileName(string result);

        void FailGetTempFileName(ref Exception exception);

        void BeginGetTempPath();

        void EndGetTempPath(string result);

        void FailGetTempPath(ref Exception exception);

        void BeginHasExtension(string path);

        void EndHasExtension(bool result, string path);

        void FailHasExtension(ref Exception exception);

        void BeginIsPathRooted(string path);

        void EndIsPathRooted(bool result, string path);

        void FailIsPathRooted(ref Exception exception);
    }
}
//-------------------------------------------------------------------------------
// <copyright file="IPathExtension.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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
        /// <see cref="IPath.GetDirectoryName"/>
        void BeginGetDirectoryName(string path);

        /// <see cref="IPath.GetDirectoryName"/>
        void EndGetDirectoryName(string result, string path);

        /// <see cref="IPath.GetDirectoryName"/>
        void FailGetDirectoryName(ref Exception exception);

        /// <see cref="IPath.GetFileName"/>
        void BeginGetFileName(string path);

        /// <see cref="IPath.GetFileName"/>
        void EndGetFileName(string result, string path);

        /// <see cref="IPath.GetFileName"/>
        void FailGetFileName(ref Exception exception);

        /// <see cref="IPath.GetFileNameWithoutExtension"/>
        void BeginGetFileNameWithoutExtension(string path);

        /// <see cref="IPath.GetFileNameWithoutExtension"/>
        void EndGetFileNameWithoutExtension(string result, string path);

        /// <see cref="IPath.GetFileNameWithoutExtension"/>
        void FailGetFileNameWithoutExtension(ref Exception exception);

        /// <see cref="IPath.Combine"/>
        void BeginCombine(string path1, string path2);

        /// <see cref="IPath.Combine"/>
        void EndCombine(string result, string path1, string path2);

        /// <see cref="IPath.Combine"/>
        void FailCombine(ref Exception exception);

        /// <see cref="IPath.GetRandomFileName"/>
        void BeginGetRandomFileName();

        /// <see cref="IPath.GetRandomFileName"/>
        void EndGetRandomFileName(string result);

        /// <see cref="IPath.GetRandomFileName"/>
        void FailGetRandomFileName(ref Exception exception);

        /// <see cref="IPath.ChangeExtension"/>
        void BeginChangeExtension(string path, string extension);

        /// <see cref="IPath.ChangeExtension"/>
        void EndChangeExtension(string result, string path, string extension);

        /// <see cref="IPath.ChangeExtension"/>
        void FailChangeExtension(ref Exception exception);

        /// <see cref="IPath.GetExtension"/>
        void BeginGetExtension(string path);

        /// <see cref="IPath.GetExtension"/>
        void EndGetExtension(string result, string path);

        /// <see cref="IPath.GetExtension"/>
        void FailGetExtension(ref Exception exception);

        /// <see cref="IPath.GetFullPath"/>
        void BeginGetFullPath(string path);

        /// <see cref="IPath.GetFullPath"/>
        void EndGetFullPath(string result, string path);

        /// <see cref="IPath.GetFullPath"/>
        void FailGetFullPath(ref Exception exception);

        /// <see cref="IPath.GetInvalidFileNameChars"/>
        void BeginGetInvalidFileNameChars();

        /// <see cref="IPath.GetInvalidFileNameChars"/>
        void EndGetInvalidFileNameChars(char[] result);

        /// <see cref="IPath.GetInvalidFileNameChars"/>
        void FailGetInvalidFileNameChars(ref Exception exception);

        /// <see cref="IPath.GetInvalidPathChars"/>
        void BeginGetInvalidPathChars();

        /// <see cref="IPath.GetInvalidPathChars"/>
        void EndGetInvalidPathChars(char[] result);

        /// <see cref="IPath.GetInvalidPathChars"/>
        void FailGetInvalidPathChars(ref Exception exception);

        /// <see cref="IPath.GetPathRoot"/>
        void BeginGetPathRoot(string path);

        /// <see cref="IPath.GetPathRoot"/>
        void EndGetPathRoot(string result, string path);

        /// <see cref="IPath.GetPathRoot"/>
        void FailGetPathRoot(ref Exception exception);

        /// <see cref="IPath.GetTempFileName"/>
        void BeginGetTempFileName();

        /// <see cref="IPath.GetTempFileName"/>
        void EndGetTempFileName(string result);

        /// <see cref="IPath.GetTempFileName"/>
        void FailGetTempFileName(ref Exception exception);

        /// <see cref="IPath.GetTempPath"/>
        void BeginGetTempPath();

        /// <see cref="IPath.GetTempPath"/>
        void EndGetTempPath(string result);

        /// <see cref="IPath.GetTempPath"/>
        void FailGetTempPath(ref Exception exception);

        /// <see cref="IPath.HasExtension"/>
        void BeginHasExtension(string path);

        /// <see cref="IPath.HasExtension"/>
        void EndHasExtension(bool result, string path);

        /// <see cref="IPath.HasExtension"/>
        void FailHasExtension(ref Exception exception);

        /// <see cref="IPath.IsPathRooted"/>
        void BeginIsPathRooted(string path);

        /// <see cref="IPath.IsPathRooted"/>
        void EndIsPathRooted(bool result, string path);

        /// <see cref="IPath.IsPathRooted"/>
        void FailIsPathRooted(ref Exception exception);
    }
}
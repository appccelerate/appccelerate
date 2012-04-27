//-------------------------------------------------------------------------------
// <copyright file="PathExtensionBase.cs" company="Appccelerate">
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

    /// <summary>
    /// Base extension for path extensions.
    /// </summary>
    public class PathExtensionBase : IPathExtension
    {
        /// <inheritdoc />
        public virtual void BeginGetDirectoryName(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetDirectoryName(string result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetDirectoryName(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetFileName(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetFileName(string result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetFileName(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetFileNameWithoutExtension(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetFileNameWithoutExtension(string result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetFileNameWithoutExtension(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCombine(string path1, string path2)
        {
        }

        /// <inheritdoc />
        public virtual void EndCombine(string result, string path1, string path2)
        {
        }

        /// <inheritdoc />
        public virtual void FailCombine(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetRandomFileName()
        {
        }

        /// <inheritdoc />
        public virtual void EndGetRandomFileName(string result)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetRandomFileName(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginChangeExtension(string path, string extension)
        {
        }

        /// <inheritdoc />
        public virtual void EndChangeExtension(string result, string path, string extension)
        {
        }

        /// <inheritdoc />
        public virtual void FailChangeExtension(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetExtension(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetExtension(string result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetExtension(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetFullPath(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetFullPath(string result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetFullPath(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetInvalidFileNameChars()
        {
        }

        /// <inheritdoc />
        public virtual void EndGetInvalidFileNameChars(char[] result)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetInvalidFileNameChars(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetInvalidPathChars()
        {
        }

        /// <inheritdoc />
        public virtual void EndGetInvalidPathChars(char[] result)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetInvalidPathChars(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetPathRoot(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetPathRoot(string result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetPathRoot(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetTempFileName()
        {
        }

        /// <inheritdoc />
        public virtual void EndGetTempFileName(string result)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetTempFileName(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetTempPath()
        {
        }

        /// <inheritdoc />
        public virtual void EndGetTempPath(string result)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetTempPath(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginHasExtension(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndHasExtension(bool result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailHasExtension(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginIsPathRooted(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndIsPathRooted(bool result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailIsPathRooted(ref Exception exception)
        {
        }
    }
}
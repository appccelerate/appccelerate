//-------------------------------------------------------------------------------
// <copyright file="PathExtensionBase.cs" company="Appccelerate">
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

    /// <summary>
    /// Base extension for path extensions.
    /// </summary>
    public class PathExtensionBase : IPathExtension
    {
        public virtual void BeginGetDirectoryName(string path)
        {
        }

        public virtual void EndGetDirectoryName(string result, string path)
        {
        }

        public virtual void FailGetDirectoryName(ref Exception exception)
        {
        }

        public virtual void BeginGetFileName(string path)
        {
        }

        public virtual void EndGetFileName(string result, string path)
        {
        }

        public virtual void FailGetFileName(ref Exception exception)
        {
        }

        public virtual void BeginGetFileNameWithoutExtension(string path)
        {
        }

        public virtual void EndGetFileNameWithoutExtension(string result, string path)
        {
        }

        public virtual void FailGetFileNameWithoutExtension(ref Exception exception)
        {
        }

        public virtual void BeginCombine(string path1, string path2)
        {
        }

        public virtual void EndCombine(string result, string path1, string path2)
        {
        }

        public virtual void FailCombine(ref Exception exception)
        {
        }

        public virtual void BeginGetRandomFileName()
        {
        }

        public virtual void EndGetRandomFileName(string result)
        {
        }

        public virtual void FailGetRandomFileName(ref Exception exception)
        {
        }

        public virtual void BeginChangeExtension(string path, string extension)
        {
        }

        public virtual void EndChangeExtension(string result, string path, string extension)
        {
        }

        public virtual void FailChangeExtension(ref Exception exception)
        {
        }

        public virtual void BeginGetExtension(string path)
        {
        }

        public virtual void EndGetExtension(string result, string path)
        {
        }

        public virtual void FailGetExtension(ref Exception exception)
        {
        }

        public virtual void BeginGetFullPath(string path)
        {
        }

        public virtual void EndGetFullPath(string result, string path)
        {
        }

        public virtual void FailGetFullPath(ref Exception exception)
        {
        }

        public virtual void BeginGetInvalidFileNameChars()
        {
        }

        public virtual void EndGetInvalidFileNameChars(char[] result)
        {
        }

        public virtual void FailGetInvalidFileNameChars(ref Exception exception)
        {
        }

        public virtual void BeginGetInvalidPathChars()
        {
        }

        public virtual void EndGetInvalidPathChars(char[] result)
        {
        }

        public virtual void FailGetInvalidPathChars(ref Exception exception)
        {
        }

        public virtual void BeginGetPathRoot(string path)
        {
        }

        public virtual void EndGetPathRoot(string result, string path)
        {
        }

        public virtual void FailGetPathRoot(ref Exception exception)
        {
        }

        public virtual void BeginGetTempFileName()
        {
        }

        public virtual void EndGetTempFileName(string result)
        {
        }

        public virtual void FailGetTempFileName(ref Exception exception)
        {
        }

        public virtual void BeginGetTempPath()
        {
        }

        public virtual void EndGetTempPath(string result)
        {
        }

        public virtual void FailGetTempPath(ref Exception exception)
        {
        }

        public virtual void BeginHasExtension(string path)
        {
        }

        public virtual void EndHasExtension(bool result, string path)
        {
        }

        public virtual void FailHasExtension(ref Exception exception)
        {
        }

        public virtual void BeginIsPathRooted(string path)
        {
        }

        public virtual void EndIsPathRooted(bool result, string path)
        {
        }

        public virtual void FailIsPathRooted(ref Exception exception)
        {
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="DirectoryExtensionBase.cs" company="Appccelerate">
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
    using System.IO;
    using System.Security.AccessControl;

    /// <summary>
    /// Base extension for directory extensions.
    /// </summary>
    public class DirectoryExtensionBase : IDirectoryExtension
    {
        /// <inheritdoc />
        public virtual void BeginExists(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndExists(bool result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailExists(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCreateDirectory(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndCreateDirectory(DirectoryInfo result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailCreateDirectory(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCreateDirectory(string path, DirectorySecurity directorySecurity)
        {
        }

        /// <inheritdoc />
        public virtual void EndCreateDirectory(DirectoryInfo result, string path, DirectorySecurity directorySecurity)
        {
        }

        /// <inheritdoc />
        public virtual void BeginDelete(string path, bool recursive)
        {
        }

        /// <inheritdoc />
        public virtual void EndDelete(string path, bool recursive)
        {
        }

        /// <inheritdoc />
        public virtual void FailDelete(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginDelete(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndDelete(string path)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetFiles(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetFiles(string[] result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetFiles(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetFiles(string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetFiles(string[] result, string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetFiles(string path, string searchPattern, SearchOption searchOption)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetFiles(string[] result, string path, string searchPattern, SearchOption searchOption)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetDirectories(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetDirectories(string[] result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetDirectories(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetDirectories(string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetDirectories(string[] result, string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetDirectories(string[] result, string path, string searchPattern, SearchOption searchOption)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetAccessControl(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetAccessControl(DirectorySecurity result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetAccessControl(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetAccessControl(string path, AccessControlSections includeSections)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetAccessControl(DirectorySecurity result, string path, AccessControlSections includeSections)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetCreationTime(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetCreationTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetCreationTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetCreationTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetCreationTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetCreationTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetCurrentDirectory()
        {
        }

        /// <inheritdoc />
        public virtual void EndGetCurrentDirectory(string result)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetCurrentDirectory(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetDirectoryRoot(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetDirectoryRoot(string result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetDirectoryRoot(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetFileSystemEntries(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetFileSystemEntries(string[] result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetFileSystemEntries(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetFileSystemEntries(string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetFileSystemEntries(string[] result, string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetLastAccessTime(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetLastAccessTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetLastAccessTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetLastAccessTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetLastAccessTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetLastAccessTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetLastWriteTime(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetLastWriteTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetLastWriteTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetLastWriteTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetLastWriteTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetLastWriteTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetLogicalDrives()
        {
        }

        /// <inheritdoc />
        public virtual void EndGetLogicalDrives(string[] result)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetLogicalDrives(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetParent(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetParent(DirectoryInfo result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetParent(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginMove(string sourceDirName, string destDirName)
        {
        }

        /// <inheritdoc />
        public virtual void EndMove(string sourceDirName, string destDirName)
        {
        }

        /// <inheritdoc />
        public virtual void FailMove(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetAccessControl(string path, DirectorySecurity directorySecurity)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetAccessControl(string path, DirectorySecurity directorySecurity)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetAccessControl(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetCreationTime(string path, DateTime creationTime)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetCreationTime(string path, DateTime creationTime)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetCreationTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetCreationTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetCurrentDirectory(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetCurrentDirectory(string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetCurrentDirectory(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetLastAccessTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetLastAccessTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetLastWriteTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetLastWriteTimeUtc(ref Exception exception)
        {
        }
    }
}
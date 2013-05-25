//-------------------------------------------------------------------------------
// <copyright file="DirectoryExtensionBase.cs" company="Appccelerate">
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
    using System.IO;
    using System.Security.AccessControl;

    /// <summary>
    /// Base extension for directory extensions.
    /// </summary>
    public class DirectoryExtensionBase : IDirectoryExtension
    {
        public virtual void BeginExists(string path)
        {
        }

        public virtual void EndExists(bool result, string path)
        {
        }

        public virtual void FailExists(ref Exception exception)
        {
        }

        public virtual void BeginCreateDirectory(string path)
        {
        }

        public virtual void EndCreateDirectory(DirectoryInfo result, string path)
        {
        }

        public virtual void FailCreateDirectory(ref Exception exception)
        {
        }

        public virtual void BeginCreateDirectory(string path, DirectorySecurity directorySecurity)
        {
        }

        public virtual void EndCreateDirectory(DirectoryInfo result, string path, DirectorySecurity directorySecurity)
        {
        }

        public virtual void BeginDelete(string path, bool recursive)
        {
        }

        public virtual void EndDelete(string path, bool recursive)
        {
        }

        public virtual void FailDelete(ref Exception exception)
        {
        }

        public virtual void BeginDelete(string path)
        {
        }

        public virtual void EndDelete(string path)
        {
        }

        public virtual void BeginGetFiles(string path)
        {
        }

        public virtual void EndGetFiles(string[] result, string path)
        {
        }

        public virtual void FailGetFiles(ref Exception exception)
        {
        }

        public virtual void BeginGetFiles(string path, string searchPattern)
        {
        }

        public virtual void EndGetFiles(string[] result, string path, string searchPattern)
        {
        }

        public virtual void BeginGetFiles(string path, string searchPattern, SearchOption searchOption)
        {
        }

        public virtual void EndGetFiles(string[] result, string path, string searchPattern, SearchOption searchOption)
        {
        }

        public virtual void BeginGetDirectories(string path)
        {
        }

        public virtual void EndGetDirectories(string[] result, string path)
        {
        }

        public virtual void FailGetDirectories(ref Exception exception)
        {
        }

        public virtual void BeginGetDirectories(string path, string searchPattern)
        {
        }

        public virtual void EndGetDirectories(string[] result, string path, string searchPattern)
        {
        }

        public virtual void BeginGetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
        }

        public virtual void EndGetDirectories(string[] result, string path, string searchPattern, SearchOption searchOption)
        {
        }

        public virtual void BeginGetAccessControl(string path)
        {
        }

        public virtual void EndGetAccessControl(DirectorySecurity result, string path)
        {
        }

        public virtual void FailGetAccessControl(ref Exception exception)
        {
        }

        public virtual void BeginGetAccessControl(string path, AccessControlSections includeSections)
        {
        }

        public virtual void EndGetAccessControl(DirectorySecurity result, string path, AccessControlSections includeSections)
        {
        }

        public virtual void BeginGetCreationTime(string path)
        {
        }

        public virtual void EndGetCreationTime(DateTime result, string path)
        {
        }

        public virtual void FailGetCreationTime(ref Exception exception)
        {
        }

        public virtual void BeginGetCreationTimeUtc(string path)
        {
        }

        public virtual void EndGetCreationTimeUtc(DateTime result, string path)
        {
        }

        public virtual void FailGetCreationTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginGetCurrentDirectory()
        {
        }

        public virtual void EndGetCurrentDirectory(string result)
        {
        }

        public virtual void FailGetCurrentDirectory(ref Exception exception)
        {
        }

        public virtual void BeginGetDirectoryRoot(string path)
        {
        }

        public virtual void EndGetDirectoryRoot(string result, string path)
        {
        }

        public virtual void FailGetDirectoryRoot(ref Exception exception)
        {
        }

        public virtual void BeginGetFileSystemEntries(string path)
        {
        }

        public virtual void EndGetFileSystemEntries(string[] result, string path)
        {
        }

        public virtual void FailGetFileSystemEntries(ref Exception exception)
        {
        }

        public virtual void BeginGetFileSystemEntries(string path, string searchPattern)
        {
        }

        public virtual void EndGetFileSystemEntries(string[] result, string path, string searchPattern)
        {
        }

        public virtual void BeginGetLastAccessTime(string path)
        {
        }

        public virtual void EndGetLastAccessTime(DateTime result, string path)
        {
        }

        public virtual void FailGetLastAccessTime(ref Exception exception)
        {
        }

        public virtual void BeginGetLastAccessTimeUtc(string path)
        {
        }

        public virtual void EndGetLastAccessTimeUtc(DateTime result, string path)
        {
        }

        public virtual void FailGetLastAccessTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginGetLastWriteTime(string path)
        {
        }

        public virtual void EndGetLastWriteTime(DateTime result, string path)
        {
        }

        public virtual void FailGetLastWriteTime(ref Exception exception)
        {
        }

        public virtual void BeginGetLastWriteTimeUtc(string path)
        {
        }

        public virtual void EndGetLastWriteTimeUtc(DateTime result, string path)
        {
        }

        public virtual void FailGetLastWriteTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginGetLogicalDrives()
        {
        }

        public virtual void EndGetLogicalDrives(string[] result)
        {
        }

        public virtual void FailGetLogicalDrives(ref Exception exception)
        {
        }

        public virtual void BeginGetParent(string path)
        {
        }

        public virtual void EndGetParent(DirectoryInfo result, string path)
        {
        }

        public virtual void FailGetParent(ref Exception exception)
        {
        }

        public virtual void BeginMove(string sourceDirName, string destDirName)
        {
        }

        public virtual void EndMove(string sourceDirName, string destDirName)
        {
        }

        public virtual void FailMove(ref Exception exception)
        {
        }

        public virtual void BeginSetAccessControl(string path, DirectorySecurity directorySecurity)
        {
        }

        public virtual void EndSetAccessControl(string path, DirectorySecurity directorySecurity)
        {
        }

        public virtual void FailSetAccessControl(ref Exception exception)
        {
        }

        public virtual void BeginSetCreationTime(string path, DateTime creationTime)
        {
        }

        public virtual void EndSetCreationTime(string path, DateTime creationTime)
        {
        }

        public virtual void FailSetCreationTime(ref Exception exception)
        {
        }

        public virtual void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        public virtual void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        public virtual void FailSetCreationTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginSetCurrentDirectory(string path)
        {
        }

        public virtual void EndSetCurrentDirectory(string path)
        {
        }

        public virtual void FailSetCurrentDirectory(ref Exception exception)
        {
        }

        public virtual void BeginSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        public virtual void EndSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        public virtual void FailSetLastAccessTime(ref Exception exception)
        {
        }

        public virtual void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        public virtual void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        public virtual void FailSetLastAccessTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        public virtual void EndSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        public virtual void FailSetLastWriteTime(ref Exception exception)
        {
        }

        public virtual void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        public virtual void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        public virtual void FailSetLastWriteTimeUtc(ref Exception exception)
        {
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="IDirectoryExtension.cs" company="Appccelerate">
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
    using System.Runtime.CompilerServices;
    using System.Security.AccessControl;

    /// <summary>
    /// Interface for directory access extensions
    /// </summary>
    [CompilerGenerated]
    public interface IDirectoryExtension
    {
        void BeginExists(string path);

        void EndExists(bool result, string path);

        void FailExists(ref Exception exception);

        void BeginCreateDirectory(string path);

        void EndCreateDirectory(DirectoryInfo result, string path);

        void FailCreateDirectory(ref Exception exception);

        void BeginCreateDirectory(string path, DirectorySecurity directorySecurity);

        void EndCreateDirectory(DirectoryInfo result, string path, DirectorySecurity directorySecurity);

        void BeginDelete(string path, bool recursive);

        void EndDelete(string path, bool recursive);

        void FailDelete(ref Exception exception);

        void BeginDelete(string path);

        void EndDelete(string path);

        void BeginGetFiles(string path);

        void EndGetFiles(string[] result, string path);

        void FailGetFiles(ref Exception exception);

        void BeginGetFiles(string path, string searchPattern);

        void EndGetFiles(string[] result, string path, string searchPattern);

        void BeginGetFiles(string path, string searchPattern, SearchOption searchOption);

        void EndGetFiles(string[] result, string path, string searchPattern, SearchOption searchOption);

        void BeginGetDirectories(string path);

        void EndGetDirectories(string[] result, string path);

        void FailGetDirectories(ref Exception exception);

        void BeginGetDirectories(string path, string searchPattern);

        void EndGetDirectories(string[] result, string path, string searchPattern);

        void BeginGetDirectories(string path, string searchPattern, SearchOption searchOption);

        void EndGetDirectories(string[] result, string path, string searchPattern, SearchOption searchOption);

        void BeginGetAccessControl(string path);

        void EndGetAccessControl(DirectorySecurity result, string path);

        void FailGetAccessControl(ref Exception exception);

        void BeginGetAccessControl(string path, AccessControlSections includeSections);

        void EndGetAccessControl(DirectorySecurity result, string path, AccessControlSections includeSections);

        void BeginGetCreationTime(string path);

        void EndGetCreationTime(DateTime result, string path);

        void FailGetCreationTime(ref Exception exception);

        void BeginGetCreationTimeUtc(string path);

        void EndGetCreationTimeUtc(DateTime result, string path);

        void FailGetCreationTimeUtc(ref Exception exception);

        void BeginGetCurrentDirectory();

        void EndGetCurrentDirectory(string result);

        void FailGetCurrentDirectory(ref Exception exception);

        void BeginGetDirectoryRoot(string path);

        void EndGetDirectoryRoot(string result, string path);

        void FailGetDirectoryRoot(ref Exception exception);

        void BeginGetFileSystemEntries(string path);

        void EndGetFileSystemEntries(string[] result, string path);

        void FailGetFileSystemEntries(ref Exception exception);

        void BeginGetFileSystemEntries(string path, string searchPattern);

        void EndGetFileSystemEntries(string[] result, string path, string searchPattern);

        void BeginGetLastAccessTime(string path);

        void EndGetLastAccessTime(DateTime result, string path);

        void FailGetLastAccessTime(ref Exception exception);

        void BeginGetLastAccessTimeUtc(string path);

        void EndGetLastAccessTimeUtc(DateTime result, string path);

        void FailGetLastAccessTimeUtc(ref Exception exception);

        void BeginGetLastWriteTime(string path);

        void EndGetLastWriteTime(DateTime result, string path);

        void FailGetLastWriteTime(ref Exception exception);

        void BeginGetLastWriteTimeUtc(string path);

        void EndGetLastWriteTimeUtc(DateTime result, string path);

        void FailGetLastWriteTimeUtc(ref Exception exception);

        void BeginGetLogicalDrives();

        void EndGetLogicalDrives(string[] result);

        void FailGetLogicalDrives(ref Exception exception);

        void BeginGetParent(string path);

        void EndGetParent(DirectoryInfo result, string path);

        void FailGetParent(ref Exception exception);

        void BeginMove(string sourceDirName, string destDirName);

        void EndMove(string sourceDirName, string destDirName);

        void FailMove(ref Exception exception);

        void BeginSetAccessControl(string path, DirectorySecurity directorySecurity);

        void EndSetAccessControl(string path, DirectorySecurity directorySecurity);

        void FailSetAccessControl(ref Exception exception);

        void BeginSetCreationTime(string path, DateTime creationTime);

        void EndSetCreationTime(string path, DateTime creationTime);

        void FailSetCreationTime(ref Exception exception);

        void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        void FailSetCreationTimeUtc(ref Exception exception);

        void BeginSetCurrentDirectory(string path);

        void EndSetCurrentDirectory(string path);

        void FailSetCurrentDirectory(ref Exception exception);

        void BeginSetLastAccessTime(string path, DateTime lastAccessTime);

        void EndSetLastAccessTime(string path, DateTime lastAccessTime);

        void FailSetLastAccessTime(ref Exception exception);

        void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        void FailSetLastAccessTimeUtc(ref Exception exception);

        void BeginSetLastWriteTime(string path, DateTime lastWriteTime);

        void EndSetLastWriteTime(string path, DateTime lastWriteTime);

        void FailSetLastWriteTime(ref Exception exception);

        void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        void FailSetLastWriteTimeUtc(ref Exception exception);
    }
}
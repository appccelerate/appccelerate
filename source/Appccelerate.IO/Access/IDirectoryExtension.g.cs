//-------------------------------------------------------------------------------
// <copyright file="IDirectoryExtension.cs" company="Appccelerate">
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
    using System.Runtime.CompilerServices;
    using System.Security.AccessControl;

    /// <summary>
    /// Interface for directory access extensions
    /// </summary>
    [CompilerGenerated]
    public interface IDirectoryExtension
    {
        /// <see cref="IDirectory.Exists(System.String)" />
        void BeginExists(string path);

        /// <see cref="IDirectory.Exists(System.String)" />
        void EndExists(bool result, string path);

        /// <see cref="IDirectory.Exists(System.String)" />
        void FailExists(ref Exception exception);

        /// <see cref="IDirectory.CreateDirectory(System.String)" />
        void BeginCreateDirectory(string path);

        /// <see cref="IDirectory.CreateDirectory(System.String)" />
        void EndCreateDirectory(DirectoryInfo result, string path);

        /// <see cref="IDirectory.CreateDirectory(System.String)" />
        void FailCreateDirectory(ref Exception exception);

        /// <see cref="IDirectory.CreateDirectory(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void BeginCreateDirectory(string path, DirectorySecurity directorySecurity);

        /// <see cref="IDirectory.CreateDirectory(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void EndCreateDirectory(DirectoryInfo result, string path, DirectorySecurity directorySecurity);

        /// <see cref="IDirectory.Delete(System.String,System.Boolean)" />
        void BeginDelete(string path, bool recursive);

        /// <see cref="IDirectory.Delete(System.String,System.Boolean)" />
        void EndDelete(string path, bool recursive);

        /// <see cref="IDirectory.Delete(System.String,System.Boolean)" />
        void FailDelete(ref Exception exception);

        /// <see cref="IDirectory.Delete(System.String)" />
        void BeginDelete(string path);

        /// <see cref="IDirectory.Delete(System.String)" />
        void EndDelete(string path);

        /// <see cref="IDirectory.GetFiles(System.String)" />
        void BeginGetFiles(string path);

        /// <see cref="IDirectory.GetFiles(System.String)" />
        void EndGetFiles(string[] result, string path);

        /// <see cref="IDirectory.GetFiles(System.String)" />
        void FailGetFiles(ref Exception exception);

        /// <see cref="IDirectory.GetFiles(System.String,System.String)" />
        void BeginGetFiles(string path, string searchPattern);

        /// <see cref="IDirectory.GetFiles(System.String,System.String)" />
        void EndGetFiles(string[] result, string path, string searchPattern);

        /// <see cref="IDirectory.GetFiles(System.String,System.String,System.IO.SearchOption)" />
        void BeginGetFiles(string path, string searchPattern, SearchOption searchOption);

        /// <see cref="IDirectory.GetFiles(System.String,System.String,System.IO.SearchOption)" />
        void EndGetFiles(string[] result, string path, string searchPattern, SearchOption searchOption);

        /// <see cref="IDirectory.GetDirectories(System.String)" />
        void BeginGetDirectories(string path);

        /// <see cref="IDirectory.GetDirectories(System.String)" />
        void EndGetDirectories(string[] result, string path);

        /// <see cref="IDirectory.GetDirectories(System.String)" />
        void FailGetDirectories(ref Exception exception);

        /// <see cref="IDirectory.GetDirectories(System.String,System.String)" />
        void BeginGetDirectories(string path, string searchPattern);

        /// <see cref="IDirectory.GetDirectories(System.String,System.String)" />
        void EndGetDirectories(string[] result, string path, string searchPattern);

        /// <see cref="IDirectory.GetDirectories(System.String,System.String,System.IO.SearchOption)" />
        void BeginGetDirectories(string path, string searchPattern, SearchOption searchOption);

        /// <see cref="IDirectory.GetDirectories(System.String,System.String,System.IO.SearchOption)" />
        void EndGetDirectories(string[] result, string path, string searchPattern, SearchOption searchOption);

        /// <see cref="IDirectory.GetAccessControl(System.String)" />
        void BeginGetAccessControl(string path);

        /// <see cref="IDirectory.GetAccessControl(System.String)" />
        void EndGetAccessControl(DirectorySecurity result, string path);

        /// <see cref="IDirectory.GetAccessControl(System.String)" />
        void FailGetAccessControl(ref Exception exception);

        /// <see cref="IDirectory.GetAccessControl(System.String,System.Security.AccessControl.AccessControlSections)" />
        void BeginGetAccessControl(string path, AccessControlSections includeSections);

        /// <see cref="IDirectory.GetAccessControl(System.String,System.Security.AccessControl.AccessControlSections)" />
        void EndGetAccessControl(DirectorySecurity result, string path, AccessControlSections includeSections);

        /// <see cref="IDirectory.GetCreationTime(System.String)" />
        void BeginGetCreationTime(string path);

        /// <see cref="IDirectory.GetCreationTime(System.String)" />
        void EndGetCreationTime(DateTime result, string path);

        /// <see cref="IDirectory.GetCreationTime(System.String)" />
        void FailGetCreationTime(ref Exception exception);

        /// <see cref="IDirectory.GetCreationTimeUtc(System.String)" />
        void BeginGetCreationTimeUtc(string path);

        /// <see cref="IDirectory.GetCreationTimeUtc(System.String)" />
        void EndGetCreationTimeUtc(DateTime result, string path);

        /// <see cref="IDirectory.GetCreationTimeUtc(System.String)" />
        void FailGetCreationTimeUtc(ref Exception exception);

        /// <see cref="IDirectory.GetCurrentDirectory" />
        void BeginGetCurrentDirectory();

        /// <see cref="IDirectory.GetCurrentDirectory" />
        void EndGetCurrentDirectory(string result);

        /// <see cref="IDirectory.GetCurrentDirectory" />
        void FailGetCurrentDirectory(ref Exception exception);

        /// <see cref="IDirectory.GetDirectoryRoot(System.String)" />
        void BeginGetDirectoryRoot(string path);

        /// <see cref="IDirectory.GetDirectoryRoot(System.String)" />
        void EndGetDirectoryRoot(string result, string path);

        /// <see cref="IDirectory.GetDirectoryRoot(System.String)" />
        void FailGetDirectoryRoot(ref Exception exception);

        /// <see cref="IDirectory.GetFileSystemEntries(System.String)" />
        void BeginGetFileSystemEntries(string path);

        /// <see cref="IDirectory.GetFileSystemEntries(System.String)" />
        void EndGetFileSystemEntries(string[] result, string path);

        /// <see cref="IDirectory.GetFileSystemEntries(System.String)" />
        void FailGetFileSystemEntries(ref Exception exception);

        /// <see cref="IDirectory.GetFileSystemEntries(System.String,System.String)" />
        void BeginGetFileSystemEntries(string path, string searchPattern);

        /// <see cref="IDirectory.GetFileSystemEntries(System.String,System.String)" />
        void EndGetFileSystemEntries(string[] result, string path, string searchPattern);

        /// <see cref="IDirectory.GetLastAccessTime(System.String)" />
        void BeginGetLastAccessTime(string path);

        /// <see cref="IDirectory.GetLastAccessTime(System.String)" />
        void EndGetLastAccessTime(DateTime result, string path);

        /// <see cref="IDirectory.GetLastAccessTime(System.String)" />
        void FailGetLastAccessTime(ref Exception exception);

        /// <see cref="IDirectory.GetLastAccessTimeUtc(System.String)" />
        void BeginGetLastAccessTimeUtc(string path);

        /// <see cref="IDirectory.GetLastAccessTimeUtc(System.String)" />
        void EndGetLastAccessTimeUtc(DateTime result, string path);

        /// <see cref="IDirectory.GetLastAccessTimeUtc(System.String)" />
        void FailGetLastAccessTimeUtc(ref Exception exception);

        /// <see cref="IDirectory.GetLastWriteTime(System.String)" />
        void BeginGetLastWriteTime(string path);

        /// <see cref="IDirectory.GetLastWriteTime(System.String)" />
        void EndGetLastWriteTime(DateTime result, string path);

        /// <see cref="IDirectory.GetLastWriteTime(System.String)" />
        void FailGetLastWriteTime(ref Exception exception);

        /// <see cref="IDirectory.GetLastWriteTimeUtc(System.String)" />
        void BeginGetLastWriteTimeUtc(string path);

        /// <see cref="IDirectory.GetLastWriteTimeUtc(System.String)" />
        void EndGetLastWriteTimeUtc(DateTime result, string path);

        /// <see cref="IDirectory.GetLastWriteTimeUtc(System.String)" />
        void FailGetLastWriteTimeUtc(ref Exception exception);

        /// <see cref="IDirectory.GetLogicalDrives" />
        void BeginGetLogicalDrives();

        /// <see cref="IDirectory.GetLogicalDrives" />
        void EndGetLogicalDrives(string[] result);

        /// <see cref="IDirectory.GetLogicalDrives" />
        void FailGetLogicalDrives(ref Exception exception);

        /// <see cref="IDirectory.GetParent(System.String)" />
        void BeginGetParent(string path);

        /// <see cref="IDirectory.GetParent(System.String)" />
        void EndGetParent(DirectoryInfo result, string path);

        /// <see cref="IDirectory.GetParent(System.String)" />
        void FailGetParent(ref Exception exception);

        /// <see cref="IDirectory.Move(System.String,System.String)" />
        void BeginMove(string sourceDirName, string destDirName);

        /// <see cref="IDirectory.Move(System.String,System.String)" />
        void EndMove(string sourceDirName, string destDirName);

        /// <see cref="IDirectory.Move(System.String,System.String)" />
        void FailMove(ref Exception exception);

        /// <see cref="IDirectory.SetAccessControl(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void BeginSetAccessControl(string path, DirectorySecurity directorySecurity);

        /// <see cref="IDirectory.SetAccessControl(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void EndSetAccessControl(string path, DirectorySecurity directorySecurity);

        /// <see cref="IDirectory.SetAccessControl(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void FailSetAccessControl(ref Exception exception);

        /// <see cref="IDirectory.SetCreationTime(System.String,System.DateTime)" />
        void BeginSetCreationTime(string path, DateTime creationTime);

        /// <see cref="IDirectory.SetCreationTime(System.String,System.DateTime)" />
        void EndSetCreationTime(string path, DateTime creationTime);

        /// <see cref="IDirectory.SetCreationTime(System.String,System.DateTime)" />
        void FailSetCreationTime(ref Exception exception);

        /// <see cref="IDirectory.SetCreationTimeUtc(System.String,System.DateTime)" />
        void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <see cref="IDirectory.SetCreationTimeUtc(System.String,System.DateTime)" />
        void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <see cref="IDirectory.SetCreationTimeUtc(System.String,System.DateTime)" />
        void FailSetCreationTimeUtc(ref Exception exception);

        /// <see cref="IDirectory.SetCurrentDirectory(System.String)" />
        void BeginSetCurrentDirectory(string path);

        /// <see cref="IDirectory.SetCurrentDirectory(System.String)" />
        void EndSetCurrentDirectory(string path);

        /// <see cref="IDirectory.SetCurrentDirectory(System.String)" />
        void FailSetCurrentDirectory(ref Exception exception);

        /// <see cref="IDirectory.SetLastAccessTime(System.String,System.DateTime)" />
        void BeginSetLastAccessTime(string path, DateTime lastAccessTime);

        /// <see cref="IDirectory.SetLastAccessTime(System.String,System.DateTime)" />
        void EndSetLastAccessTime(string path, DateTime lastAccessTime);

        /// <see cref="IDirectory.SetLastAccessTime(System.String,System.DateTime)" />
        void FailSetLastAccessTime(ref Exception exception);

        /// <see cref="IDirectory.SetLastAccessTimeUtc(System.String,System.DateTime)" />
        void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <see cref="IDirectory.SetLastAccessTimeUtc(System.String,System.DateTime)" />
        void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <see cref="IDirectory.SetLastAccessTimeUtc(System.String,System.DateTime)" />
        void FailSetLastAccessTimeUtc(ref Exception exception);

        /// <see cref="IDirectory.SetLastWriteTime(System.String,System.DateTime)" />
        void BeginSetLastWriteTime(string path, DateTime lastWriteTime);

        /// <see cref="IDirectory.SetLastWriteTime(System.String,System.DateTime)" />
        void EndSetLastWriteTime(string path, DateTime lastWriteTime);

        /// <see cref="IDirectory.SetLastWriteTime(System.String,System.DateTime)" />
        void FailSetLastWriteTime(ref Exception exception);

        /// <see cref="IDirectory.SetLastWriteTimeUtc(System.String,System.DateTime)" />
        void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <see cref="IDirectory.SetLastWriteTimeUtc(System.String,System.DateTime)" />
        void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <see cref="IDirectory.SetLastWriteTimeUtc(System.String,System.DateTime)" />
        void FailSetLastWriteTimeUtc(ref Exception exception);
    }
}
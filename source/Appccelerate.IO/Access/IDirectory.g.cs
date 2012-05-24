//-------------------------------------------------------------------------------
// <copyright file="IDirectory.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Security.AccessControl;

    /// <summary>
    /// Abstraction layer which simplifies access to directories.
    /// </summary>
    [CompilerGenerated]
    public interface IDirectory
    {
        bool Exists(string path);

        IDirectoryInfo CreateDirectory(string path);

        IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity);

        void Delete(string path, bool recursive);

        void Delete(string path);

        IEnumerable<string> GetFiles(string path);

        IEnumerable<string> GetFiles(string path, string searchPattern);

        IEnumerable<string> GetFiles(string path, string searchPattern, SearchOption searchOption);

        IEnumerable<string> GetDirectories(string path);

        IEnumerable<string> GetDirectories(string path, string searchPattern);

        IEnumerable<string> GetDirectories(string path, string searchPattern, SearchOption searchOption);

        DirectorySecurity GetAccessControl(string path);

        DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections);

        DateTime GetCreationTime(string path);

        DateTime GetCreationTimeUtc(string path);

        string GetCurrentDirectory();

        string GetDirectoryRoot(string path);

        IEnumerable<string> GetFileSystemEntries(string path);

        IEnumerable<string> GetFileSystemEntries(string path, string searchPattern);

        DateTime GetLastAccessTime(string path);

        DateTime GetLastAccessTimeUtc(string path);

        DateTime GetLastWriteTime(string path);

        DateTime GetLastWriteTimeUtc(string path);

        IEnumerable<string> GetLogicalDrives();

        IDirectoryInfo GetParent(string path);

        void Move(string sourceDirName, string destDirName);

        void SetAccessControl(string path, DirectorySecurity directorySecurity);

        void SetCreationTime(string path, DateTime creationTime);

        void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

        void SetCurrentDirectory(string path);

        void SetLastAccessTime(string path, DateTime lastAccessTime);

        void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        void SetLastWriteTime(string path, DateTime lastWriteTime);

        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);
    }
}
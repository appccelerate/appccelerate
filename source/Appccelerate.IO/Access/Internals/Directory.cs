//-------------------------------------------------------------------------------
// <copyright file="Directory.cs" company="Appccelerate">
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

namespace Appccelerate.IO.Access.Internals
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.AccessControl;

    /// <summary>
    /// Wrapper class which simplifies the access to directories.
    /// </summary>
    public class Directory : IDirectory, IExtensionProvider<IDirectoryExtension>
    {
        private readonly List<IDirectoryExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Directory"/> class.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public Directory(IEnumerable<IDirectoryExtension> extensions)
        {
            this.extensions = extensions.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<IDirectoryExtension> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <inheritdoc />
        public bool Exists(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.Exists(path), path);
        }

        /// <inheritdoc />
        public IDirectoryInfo CreateDirectory(string path)
        {
            var directoryInfo = this.SurroundWithExtension(() => System.IO.Directory.CreateDirectory(path), path);
            return new DirectoryInfo(directoryInfo);
        }

        /// <inheritdoc />
        public IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            var directoryInfo = this.SurroundWithExtension(() => System.IO.Directory.CreateDirectory(path, directorySecurity), path, directorySecurity);
            return new DirectoryInfo(directoryInfo);
        }

        /// <inheritdoc />
        public void Delete(string path, bool recursive)
        {
            this.SurroundWithExtension(() => System.IO.Directory.Delete(path, recursive), path, recursive);
        }

        /// <inheritdoc />
        public void Delete(string path)
        {
            this.SurroundWithExtension(() => System.IO.Directory.Delete(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFiles(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetFiles(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFiles(string path, string searchPattern)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetFiles(path, searchPattern), path, searchPattern);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetFiles(path, searchPattern, searchOption), path, searchPattern, searchOption);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetDirectories(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetDirectories(path), path);
        }

        /// <inheritdoc />
        public DirectorySecurity GetAccessControl(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetAccessControl(path), path);
        }

        /// <inheritdoc />
        public DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetAccessControl(path, includeSections), path, includeSections);
        }

        /// <inheritdoc />
        public DateTime GetCreationTime(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetCreationTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetCreationTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetCreationTimeUtc(path), path);
        }

        /// <inheritdoc />
        public string GetCurrentDirectory()
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetCurrentDirectory());
        }

        /// <inheritdoc />
        public IEnumerable<string> GetDirectories(string path, string searchPattern)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetDirectories(path, searchPattern), path, searchPattern);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetDirectories(path, searchPattern, searchOption), path, searchPattern, searchOption);
        }

        /// <inheritdoc />
        public string GetDirectoryRoot(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetDirectoryRoot(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFileSystemEntries(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetFileSystemEntries(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFileSystemEntries(string path, string searchPattern)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetFileSystemEntries(path, searchPattern), path, searchPattern);
        }

        /// <inheritdoc />
        public DateTime GetLastAccessTime(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetLastAccessTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastAccessTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetLastAccessTimeUtc(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastWriteTime(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetLastWriteTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastWriteTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetLastWriteTimeUtc(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetLogicalDrives()
        {
            return this.SurroundWithExtension(() => System.IO.Directory.GetLogicalDrives());
        }

        /// <inheritdoc />
        public IDirectoryInfo GetParent(string path)
        {
            var directoryInfo = this.SurroundWithExtension(() => System.IO.Directory.GetParent(path));
            return new DirectoryInfo(directoryInfo);
        }

        /// <inheritdoc />
        public void Move(string sourceDirName, string destDirName)
        {
            this.SurroundWithExtension(() => System.IO.Directory.Move(sourceDirName, destDirName), sourceDirName, destDirName);
        }

        /// <inheritdoc />
        public void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            this.SurroundWithExtension(() => System.IO.Directory.SetAccessControl(path, directorySecurity), path, directorySecurity);
        }

        /// <inheritdoc />
        public void SetCreationTime(string path, DateTime creationTime)
        {
            this.SurroundWithExtension(() => System.IO.Directory.SetCreationTime(path, creationTime), path, creationTime);
        }

        /// <inheritdoc />
        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            this.SurroundWithExtension(() => System.IO.Directory.SetCreationTimeUtc(path, creationTimeUtc), path, creationTimeUtc);
        }

        /// <inheritdoc />
        public void SetCurrentDirectory(string path)
        {
            this.SurroundWithExtension(() => System.IO.Directory.SetCurrentDirectory(path), path);
        }

        /// <inheritdoc />
        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            this.SurroundWithExtension(() => System.IO.Directory.SetLastAccessTime(path, lastAccessTime), path, lastAccessTime);
        }

        /// <inheritdoc />
        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            this.SurroundWithExtension(() => System.IO.Directory.SetLastAccessTimeUtc(path, lastAccessTimeUtc), path, lastAccessTimeUtc);
        }

        /// <inheritdoc />
        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            this.SurroundWithExtension(() => System.IO.Directory.SetLastWriteTime(path, lastWriteTime), path, lastWriteTime);
        }

        /// <inheritdoc />
        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            this.SurroundWithExtension(() => System.IO.Directory.SetLastWriteTimeUtc(path, lastWriteTimeUtc), path, lastWriteTimeUtc);
        }
    }
}
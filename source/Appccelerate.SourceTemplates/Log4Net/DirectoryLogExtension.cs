//-------------------------------------------------------------------------------
// <copyright file="DirectoryLogExtension.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Security.AccessControl;

    using Appccelerate.IO.Access;

    using log4net;

    /// <summary>
    /// Directory access extension which logs actions with log4net.
    /// </summary>
    public class DirectoryLogExtension : DirectoryExtensionBase
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryLogExtension"/> class.
        /// </summary>
        public DirectoryLogExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DirectoryLogExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DirectoryLogExtension(ILog logger)
        {
            this.log = logger;
        }

        /// <inheritdoc />
        public override void BeginExists(string path)
        {
            base.BeginExists(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Checking if file {0} exists.", path);
        }

        /// <inheritdoc />
        public override void EndExists(bool result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailExists(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginCreateDirectory(string path)
        {
        }

        /// <inheritdoc />
        public override void EndCreateDirectory(DirectoryInfo result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailCreateDirectory(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginCreateDirectory(string path, DirectorySecurity directorySecurity)
        {
        }

        /// <inheritdoc />
        public override void EndCreateDirectory(DirectoryInfo result, string path, DirectorySecurity directorySecurity)
        {
        }

        /// <inheritdoc />
        public override void BeginDelete(string path, bool recursive)
        {
        }

        /// <inheritdoc />
        public override void EndDelete(string path, bool recursive)
        {
        }

        /// <inheritdoc />
        public override void FailDelete(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginDelete(string path)
        {
        }

        /// <inheritdoc />
        public override void EndDelete(string path)
        {
        }

        /// <inheritdoc />
        public override void BeginGetFiles(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetFiles(string[] result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetFiles(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetFiles(string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public override void EndGetFiles(string[] result, string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public override void BeginGetFiles(string path, string searchPattern, SearchOption searchOption)
        {
        }

        /// <inheritdoc />
        public override void EndGetFiles(string[] result, string path, string searchPattern, SearchOption searchOption)
        {
        }

        /// <inheritdoc />
        public override void BeginGetDirectories(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetDirectories(string[] result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetDirectories(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetDirectories(string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public override void EndGetDirectories(string[] result, string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public override void BeginGetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
        }

        /// <inheritdoc />
        public override void FailGetLastAccessTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetCreationTime(string path, DateTime creationTime)
        {
        }

        /// <inheritdoc />
        public override void EndSetCreationTime(string path, DateTime creationTime)
        {
        }

        /// <inheritdoc />
        public override void FailSetCreationTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void FailSetCreationTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetCurrentDirectory(string path)
        {
        }

        /// <inheritdoc />
        public override void EndSetCurrentDirectory(string path)
        {
        }

        /// <inheritdoc />
        public override void FailSetCurrentDirectory(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        /// <inheritdoc />
        public override void EndSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        /// <inheritdoc />
        public override void FailSetLastAccessTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void FailSetLastAccessTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        /// <inheritdoc />
        public override void EndSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        /// <inheritdoc />
        public override void FailSetLastWriteTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void FailSetLastWriteTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetLastAccessTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetLastAccessTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetLastAccessTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetLastWriteTime(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetLastWriteTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetLastWriteTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetLastWriteTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetLastWriteTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetLastWriteTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetLogicalDrives()
        {
        }

        /// <inheritdoc />
        public override void EndGetLogicalDrives(string[] result)
        {
        }

        /// <inheritdoc />
        public override void FailGetLogicalDrives(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetParent(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetParent(DirectoryInfo result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetParent(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginMove(string sourceDirName, string destDirName)
        {
        }

        /// <inheritdoc />
        public override void EndMove(string sourceDirName, string destDirName)
        {
        }

        /// <inheritdoc />
        public override void FailMove(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetAccessControl(string path, DirectorySecurity directorySecurity)
        {
        }

        /// <inheritdoc />
        public override void EndSetAccessControl(string path, DirectorySecurity directorySecurity)
        {
        }

        /// <inheritdoc />
        public override void FailSetAccessControl(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void EndGetDirectories(string[] result, string path, string searchPattern, SearchOption searchOption)
        {
        }

        /// <inheritdoc />
        public override void BeginGetAccessControl(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetAccessControl(DirectorySecurity result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetAccessControl(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetAccessControl(string path, AccessControlSections includeSections)
        {
        }

        /// <inheritdoc />
        public override void EndGetAccessControl(DirectorySecurity result, string path, AccessControlSections includeSections)
        {
        }

        /// <inheritdoc />
        public override void BeginGetCreationTime(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetCreationTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetCreationTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetCreationTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetCreationTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetCreationTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetCurrentDirectory()
        {
        }

        /// <inheritdoc />
        public override void EndGetCurrentDirectory(string result)
        {
        }

        /// <inheritdoc />
        public override void FailGetCurrentDirectory(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetDirectoryRoot(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetDirectoryRoot(string result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetDirectoryRoot(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetFileSystemEntries(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetFileSystemEntries(string[] result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetFileSystemEntries(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetFileSystemEntries(string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public override void EndGetFileSystemEntries(string[] result, string path, string searchPattern)
        {
        }

        /// <inheritdoc />
        public override void BeginGetLastAccessTime(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetLastAccessTime(DateTime result, string path)
        {
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="AccessFactory.cs" company="Appccelerate">
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
    using System.Linq;

    using Appccelerate.IO.Access.Internals;

    using DirectoryInfo = Appccelerate.IO.Access.Internals.DirectoryInfo;
    using DriveInfo = Appccelerate.IO.Access.Internals.DriveInfo;
    using File = Appccelerate.IO.Access.Internals.File;
    using FileInfo = Appccelerate.IO.Access.Internals.FileInfo;
    using Path = Appccelerate.IO.Access.Internals.Path;

    /// <summary>
    /// The IO access factory which implements <see cref="IAccessFactory"/>.
    /// </summary>
    public class AccessFactory : IAccessFactory
    {
        private Func<IEnumerable<IFileExtension>> fileExtensionsProvider = Enumerable.Empty<IFileExtension>;

        private Func<IEnumerable<IDirectoryExtension>> directoryExtensionsProvider = Enumerable.Empty<IDirectoryExtension>;

        private Func<IEnumerable<IPathExtension>> pathExtensionsProvider = Enumerable.Empty<IPathExtension>;

        /// <inheritdoc />
        public IDirectory CreateDirectory()
        {
            return new Directory(this.directoryExtensionsProvider());
        }

        /// <inheritdoc />
        public IFile CreateFile()
        {
            return new File(this.fileExtensionsProvider());
        }

        /// <inheritdoc />
        public IPath CreatePath()
        {
            return new Path(this.pathExtensionsProvider());
        }

        /// <inheritdoc />
        public IFileInfo CreateFileInfo(System.IO.FileInfo fileInfo)
        {
            return new FileInfo(fileInfo);
        }

        /// <inheritdoc />
        public IFileInfo CreateFileInfo(string pathToFile)
        {
            return new FileInfo(new System.IO.FileInfo(pathToFile));
        }

        /// <inheritdoc />
        public IDirectoryInfo CreateDirectoryInfo(System.IO.DirectoryInfo directoryInfo)
        {
            return new DirectoryInfo(directoryInfo);
        }

        /// <inheritdoc />
        public IDirectoryInfo CreateDirectoryInfo(string pathToDirectory)
        {
            return new DirectoryInfo(new System.IO.DirectoryInfo(pathToDirectory));
        }

        /// <inheritdoc />
        public IDriveInfo CreateDriveInfo(System.IO.DriveInfo driveInfo)
        {
            return new DriveInfo(driveInfo);
        }

        /// <inheritdoc />
        public IDriveInfo CreateDriveInfo(string driveName)
        {
            return new DriveInfo(new System.IO.DriveInfo(driveName));
        }

        /// <inheritdoc />
        public void RegisterFileExtensionsProvider(Func<IEnumerable<IFileExtension>> extensionsProvider)
        {
            this.fileExtensionsProvider = extensionsProvider;
        }

        /// <inheritdoc />
        public void RegisterDirectoryExtensionsProvider(Func<IEnumerable<IDirectoryExtension>> extensionsProvider)
        {
            this.directoryExtensionsProvider = extensionsProvider;
        }

        /// <inheritdoc />
        public void RegisterPathExtensionsProvider(Func<IEnumerable<IPathExtension>> extensionsProvider)
        {
            this.pathExtensionsProvider = extensionsProvider;
        }
    }
}
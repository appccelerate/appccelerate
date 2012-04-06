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
    using System.IO;
    using System.Linq;

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
            return new DirectoryAccess(this.directoryExtensionsProvider());
        }

        /// <inheritdoc />
        public IFile CreateFile()
        {
            return new FileAccess(this.fileExtensionsProvider());
        }

        /// <inheritdoc />
        public IPath CreatePath()
        {
            return new PathAccess(this.pathExtensionsProvider());
        }

        /// <inheritdoc />
        public IFileInfo CreateFileInfo(FileInfo fileInfo)
        {
            return new FileInfoAccess(fileInfo);
        }

        /// <inheritdoc />
        public IFileInfo CreateFileInfo(string pathToFile)
        {
            return new FileInfoAccess(new FileInfo(pathToFile));
        }

        /// <inheritdoc />
        public IDirectoryInfo CreateDirectoryInfo(DirectoryInfo directoryInfo)
        {
            return new DirectoryInfoAccess(directoryInfo);
        }

        /// <inheritdoc />
        public IDirectoryInfo CreateDirectoryInfo(string pathToDirectory)
        {
            return new DirectoryInfoAccess(new DirectoryInfo(pathToDirectory));
        }

        /// <inheritdoc />
        public IDriveInfo CreateDriveInfo(DriveInfo driveInfo)
        {
            return new DriveInfoAccess(driveInfo);
        }

        /// <inheritdoc />
        public IDriveInfo CreateDriveInfo(string driveName)
        {
            return new DriveInfoAccess(new DriveInfo(driveName));
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
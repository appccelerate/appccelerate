//-------------------------------------------------------------------------------
// <copyright file="AccessFactory.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Linq;

    using Appccelerate.IO.Access.Internals;

    using DirectoryInfo = Appccelerate.IO.Access.Internals.DirectoryInfo;
    using DriveInfo = Appccelerate.IO.Access.Internals.DriveInfo;
    using Environment = Appccelerate.IO.Access.Internals.Environment;
    using File = Appccelerate.IO.Access.Internals.File;
    using FileInfo = Appccelerate.IO.Access.Internals.FileInfo;
    using Path = Appccelerate.IO.Access.Internals.Path;

    public class AccessFactory : IAccessFactory
    {
        private Func<IEnumerable<IFileExtension>> fileExtensionsProvider = Enumerable.Empty<IFileExtension>;

        private Func<IEnumerable<IDirectoryExtension>> directoryExtensionsProvider = Enumerable.Empty<IDirectoryExtension>;

        private Func<IEnumerable<IPathExtension>> pathExtensionsProvider = Enumerable.Empty<IPathExtension>;

        private Func<IEnumerable<IEnvironmentExtension>> environmentExtensionsProvider = Enumerable.Empty<IEnvironmentExtension>;

        private Func<IEnumerable<IDriveExtension>> driveExtensionsProvider = Enumerable.Empty<IDriveExtension>;

        public IDirectory CreateDirectory()
        {
            return new Directory(this.directoryExtensionsProvider());
        }

        public IFile CreateFile()
        {
            return new File(this.fileExtensionsProvider());
        }

        public IPath CreatePath()
        {
            return new Path(this.pathExtensionsProvider());
        }

        public IEnvironment CreateEnvironment()
        {
            return new Environment(this.environmentExtensionsProvider());
        }

        public IDrive CreateDrive()
        {
            return new Drive(this.driveExtensionsProvider());
        }

        public IFileInfo CreateFileInfo(System.IO.FileInfo fileInfo)
        {
            return new FileInfo(fileInfo);
        }

        public IFileInfo CreateFileInfo(string pathToFile)
        {
            return new FileInfo(new System.IO.FileInfo(pathToFile));
        }

        public IDirectoryInfo CreateDirectoryInfo(System.IO.DirectoryInfo directoryInfo)
        {
            return new DirectoryInfo(directoryInfo);
        }

        public IDirectoryInfo CreateDirectoryInfo(string pathToDirectory)
        {
            return new DirectoryInfo(new System.IO.DirectoryInfo(pathToDirectory));
        }

        public IDriveInfo CreateDriveInfo(System.IO.DriveInfo driveInfo)
        {
            return new DriveInfo(driveInfo);
        }

        public IDriveInfo CreateDriveInfo(string driveName)
        {
            return new DriveInfo(new System.IO.DriveInfo(driveName));
        }

        public void RegisterFileExtensionsProvider(Func<IEnumerable<IFileExtension>> extensionsProvider)
        {
            this.fileExtensionsProvider = extensionsProvider;
        }

        public void RegisterDirectoryExtensionsProvider(Func<IEnumerable<IDirectoryExtension>> extensionsProvider)
        {
            this.directoryExtensionsProvider = extensionsProvider;
        }

        public void RegisterDriveExtensionsProvider(Func<IEnumerable<IDriveExtension>> extensionsProvider)
        {
            this.driveExtensionsProvider = extensionsProvider;
        }

        public void RegisterPathExtensionsProvider(Func<IEnumerable<IPathExtension>> extensionsProvider)
        {
            this.pathExtensionsProvider = extensionsProvider;
        }

        public void RegisterEnvironmentExtensionsProvider(Func<IEnumerable<IEnvironmentExtension>> extensionsProvider)
        {
            this.environmentExtensionsProvider = extensionsProvider;
        }
    }
}
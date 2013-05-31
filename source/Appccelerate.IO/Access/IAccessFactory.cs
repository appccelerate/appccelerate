//-------------------------------------------------------------------------------
// <copyright file="IAccessFactory.cs" company="Appccelerate">
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
    using System.IO;
    using System.Security;

    /// <summary>
    /// Factory interface for factories which are responsible to create IO
    /// components.
    /// </summary>
    public interface IAccessFactory
    {
        IDirectory CreateDirectory();

        IFile CreateFile();

        IPath CreatePath();

        IEnvironment CreateEnvironment();

        IDrive CreateDrive();

        IFileInfo CreateFileInfo(FileInfo fileInfo);

        /// <summary>
        /// Creates the file info.
        /// </summary>
        /// <param name="pathToFile">The file path.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pathToFile"/> is null.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required
        /// permission.</exception>
        /// <exception cref="ArgumentException">The file name is empty, contains only white
        /// spaces, or contains invalid characters.</exception>
        /// <exception cref="UnauthorizedAccessException">Access to 
        /// <paramref name="pathToFile"/> is denied.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both
        /// exceed the system-defined maximum length. For example, on Windows-based
        /// platforms, paths must be less than 248 characters, and file names must be less
        /// than 260 characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="pathToFile"/> contains
        /// a colon (:) in the middle of the string.</exception>
        /// <returns>An instance implementing <see cref="IFileInfo"/>.</returns>
        IFileInfo CreateFileInfo(string pathToFile);

        /// <summary>
        /// Creates the directory info.
        /// </summary>
        /// <param name="directoryInfo">The directory info.</param>
        /// <returns>An instance implementing <see cref="IDirectoryInfo"/>.</returns>
        IDirectoryInfo CreateDirectoryInfo(DirectoryInfo directoryInfo);

        /// <summary>
        /// Creates the directory info.
        /// </summary>
        /// <param name="pathToDirectory">The directory path.</param>
        /// <exception cref="ArgumentNullException">path is null.</exception>
        /// <exception cref="SecurityException">The caller does not have the required
        /// permission.</exception>
        /// <exception cref="ArgumentException">path contains invalid characters.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both
        /// exceed the system-defined maximum length. For example, on Windows-based
        /// platforms, paths must be less than 248 characters, and file names must be less
        /// than 260 characters. The specified path, file name, or both are too long.
        /// </exception>
        /// <returns>An instance implementing <see cref="IDirectoryInfo"/>.</returns>
        IDirectoryInfo CreateDirectoryInfo(string pathToDirectory);

        /// <summary>
        /// Creates the drive info.
        /// </summary>
        /// <param name="driveInfo">The drive info.</param>
        /// <returns>An instance implementing <see cref="IDriveInfo"/>.</returns>
        IDriveInfo CreateDriveInfo(DriveInfo driveInfo);

        /// <summary>
        /// Creates the drive info.
        /// </summary>
        /// <param name="driveName">A valid drive path or drive letter. This can be either
        /// uppercase or lowercase, 'a' to 'z'. A null value is not valid.</param>
        /// <exception cref="ArgumentNullException">The drive letter cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">The first letter of 
        /// <paramref name="driveName"/> is not an uppercase or lowercase letter from 'a'
        /// to 'z'.  -or- 
        /// <paramref name="driveName"/> does not refer to a valid drive.</exception>
        /// <returns>An instance implementing <see cref="IDriveInfo"/>.</returns>
        IDriveInfo CreateDriveInfo(string driveName);

        /// <summary>
        /// Registers an extensions provider for file extensions. The extensions provider is called on each
        /// CreateFile request.
        /// </summary>
        /// <param name="extensionsProvider">The file extension provider</param>
        void RegisterFileExtensionsProvider(Func<IEnumerable<IFileExtension>> extensionsProvider);

        /// <summary>
        /// Registers an extensions provider for directory extensions. The extensions provider is called on each
        /// CreateDirectory request.
        /// </summary>
        /// <param name="extensionsProvider">The directory extension provider</param>
        void RegisterDirectoryExtensionsProvider(Func<IEnumerable<IDirectoryExtension>> extensionsProvider);

        /// <summary>
        /// Registers an extensions provider for drive extensions. The extensions provider is called on each
        /// CreateDrive request.
        /// </summary>
        /// <param name="extensionsProvider">The drive extension provider</param>
        void RegisterDriveExtensionsProvider(Func<IEnumerable<IDriveExtension>> extensionsProvider);

        /// <summary>
        /// Registers an extensions provider for path extensions. The extensions provider is called on each
        /// CreatePath request.
        /// </summary>
        /// <param name="extensionsProvider">The path extension provider</param>
        void RegisterPathExtensionsProvider(Func<IEnumerable<IPathExtension>> extensionsProvider);

        /// <summary>
        /// Registers an extensions provider for environment extensions. The extensions provider is called on each
        /// CreateEnvironment request.
        /// </summary>
        /// <param name="extensionsProvider">The environment extension provider</param>
        void RegisterEnvironmentExtensionsProvider(Func<IEnumerable<IEnvironmentExtension>> extensionsProvider);
    }
}
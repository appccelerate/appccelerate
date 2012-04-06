//-------------------------------------------------------------------------------
// <copyright file="IDriveInfo.cs" company="Appccelerate">
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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.Serialization;

    /// <summary>
    /// Interface which simplifies the access to directory info.
    /// </summary>
    public interface IDriveInfo : ISerializable
    {
        /// <summary>
        /// Gets the amount of available free space on a drive.
        /// </summary>
        /// <value>The amount of free space available on the drive, in bytes.</value>
        /// <exception cref="IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        long AvailableFreeSpace { get; }

        /// <summary>
        /// Gets the name of the file system, such as NTFS or FAT32.
        /// </summary>
        /// <value>Provides access to information on a drive.</value>
        string DriveFormat { get; }

        /// <summary>
        /// Gets the drive type.
        /// </summary>
        /// <value>One of the <see cref="DriveType"/> values.</value>
        DriveType DriveType { get; }

        /// <summary>
        /// Gets a value indicating whether a drive is ready.
        /// </summary>
        /// <value><see langword="true"/> if the drive is ready; 
        /// <see langword="false"/> if the drive is not ready.</value>
        bool IsReady { get; }

        /// <summary>
        /// Gets the root directory of a drive.
        /// </summary>
        /// <value>An <see cref="IDirectoryInfo"/> object that contains the
        /// root directory of the drive.</value>
        IDirectoryInfo RootDirectory { get; }

        /// <summary>
        /// Gets the total amount of free space available on a drive.
        /// </summary>
        /// <value>The total free space available on a drive, in bytes.</value>
        /// <exception cref="IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        long TotalFreeSpace { get; }

        /// <summary>
        /// Gets the total size of storage space on a drive.
        /// </summary>
        /// <value>The total size of the drive, in bytes.</value>
        /// <exception cref="IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        long TotalSize { get; }

        /// <summary>
        /// Gets the name of a drive.
        /// </summary>
        /// <value>The name of the drive.</value>
        string Name { get; }

        /// <summary>
        /// Retrieves the drive names of all logical drives on a computer.
        /// </summary>
        /// <returns>An enumerable of type <see cref="IDriveInfo"/> that represents the logical drives on a computer.</returns>
        /// <exception cref="IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "The drive info access shall have the same interface as DriveInfo.")]
        IEnumerable<IDriveInfo> GetDrives();
    }
}
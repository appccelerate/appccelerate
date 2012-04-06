//-------------------------------------------------------------------------------
// <copyright file="IDirectoryInfo.cs" company="Appccelerate">
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
    using System.IO;
    using System.Security;

    /// <summary>
    /// Interface which simplifies the access to the directory info.
    /// </summary>
    public interface IDirectoryInfo : IFileSystemInfo
    {
        /// <summary>
        /// Gets the parent directory of a specified subdirectory.
        /// </summary>
        /// <value>The parent directory, or null if the path is null or if the file path
        /// denotes a root (such as "\", "C:", or * "\\server\share").</value>
        /// <exception cref="SecurityException">The caller does not have the required
        /// permission.</exception>
        IDirectoryInfo Parent { get; }

        /// <summary>
        /// Gets the root portion of a path.
        /// </summary>
        /// <value>A <see cref="IDirectoryInfo"/> object representing the root of a path.</value>
        /// <exception cref="SecurityException">The caller does not have the required
        /// permission.</exception>
        IDirectoryInfo Root { get; }

        /// <summary>
        /// Creates a directory.
        /// </summary>
        /// <exception cref="IOException">The directory cannot be created.</exception>
        void Create();
    }
}
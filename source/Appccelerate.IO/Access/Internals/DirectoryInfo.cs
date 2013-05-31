//-------------------------------------------------------------------------------
// <copyright file="DirectoryInfo.cs" company="Appccelerate">
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
    using System.IO;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    /// Wrapper class which simplifies the access to directory information.
    /// </summary>
    [Serializable]
    public sealed class DirectoryInfo : FileSystemInfo<System.IO.DirectoryInfo>, IDirectoryInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryInfo"/> class.
        /// </summary>
        /// <param name="directoryInfo">The directory info.</param>
        public DirectoryInfo(System.IO.DirectoryInfo directoryInfo)
            : base(directoryInfo)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryInfo"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private DirectoryInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the parent directory of a specified subdirectory.
        /// </summary>
        /// <value>The parent directory, or null if the path is null or if the file path
        /// denotes a root (such as "\", "C:", or * "\\server\share").</value>
        /// <exception cref="SecurityException">The caller does not have the required
        /// permission.</exception>
        public IDirectoryInfo Parent
        {
            get { return this.Info != null ? new DirectoryInfo(this.Info.Parent) : null; }
        }

        /// <summary>
        /// Gets the root portion of a path.
        /// </summary>
        /// <value>A <see cref="IDirectoryInfo"/> object representing the root of a path.</value>
        /// <exception cref="SecurityException">The caller does not have the required
        /// permission.</exception>
        public IDirectoryInfo Root
        {
            get
            {
                return this.Info != null ? new DirectoryInfo(this.Info.Root) : null;
            }
        }

        /// <summary>
        /// Creates a directory.
        /// </summary>
        /// <exception cref="IOException">The directory cannot be created.</exception>
        public void Create()
        {
            this.Info.Create();
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with
        /// the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The
        /// <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with
        /// data.</param>
        /// <param name="context">The destination (see
        /// <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this
        /// serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">The caller does
        /// not have the required permission.
        /// </exception>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="DriveInfo.cs" company="Appccelerate">
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
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    /// Wrapper class which simplifies the access to drive information.
    /// </summary>
    [Serializable]
    public sealed class DriveInfo : IDriveInfo
    {
        /// <summary>
        /// The wrapped drive info.
        /// </summary>
        private readonly System.IO.DriveInfo driveInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveInfo"/> class.
        /// </summary>
        /// <param name="driveInfo">The drive info.</param>
        public DriveInfo(System.IO.DriveInfo driveInfo)
        {
            this.driveInfo = driveInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveInfo"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private DriveInfo(SerializationInfo info, StreamingContext context)
        {
        }

        /// <inheritdoc />
        public long AvailableFreeSpace
        {
            get { return this.driveInfo.AvailableFreeSpace; }
        }

        /// <inheritdoc />
        public string DriveFormat
        {
            get { return this.driveInfo.DriveFormat; }
        }

        /// <inheritdoc />
        public DriveType DriveType
        {
            get { return this.driveInfo.DriveType; }
        }

        /// <inheritdoc />
        public bool IsReady
        {
            get { return this.driveInfo.IsReady; }
        }

        /// <inheritdoc />
        public IDirectoryInfo RootDirectory
        {
            get { return new DirectoryInfo(this.driveInfo.RootDirectory); }
        }

        /// <inheritdoc />
        public long TotalFreeSpace
        {
            get { return this.driveInfo.TotalFreeSpace; }
        }

        /// <inheritdoc />
        public long TotalSize
        {
            get { return this.driveInfo.TotalSize; }
        }

        /// <inheritdoc />
        public string Name
        {
            get { return this.driveInfo.Name; }
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with
        /// the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The 
        /// <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with
        /// data. 
        /// </param><param name="context">The destination (see 
        /// <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this
        /// serialization. 
        /// </param><exception cref="T:System.Security.SecurityException">The caller does
        /// not have the required permission. 
        /// </exception>
        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)this.driveInfo).GetObjectData(info, context);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            return this.driveInfo.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.driveInfo.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.driveInfo.ToString();
        }
    }
}
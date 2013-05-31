//-------------------------------------------------------------------------------
// <copyright file="FileInfo.cs" company="Appccelerate">
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
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.AccessControl;

    /// <summary>
    /// Wrapper class which simplifies the access to file information.
    /// </summary>
    [Serializable]
    public sealed class FileInfo : FileSystemInfo<System.IO.FileInfo>, IFileInfo
    {
        /// <summary>
        /// The directory info.
        /// </summary>
        private readonly IDirectoryInfo directoryInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfo"/> class.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        public FileInfo(System.IO.FileInfo fileInfo)
            : base(fileInfo)
        {
            this.directoryInfo = new DirectoryInfo(fileInfo.Directory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfo"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private FileInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <inheritdoc />
        public IDirectoryInfo Directory
        {
            get { return this.directoryInfo; }
        }

        /// <inheritdoc />
        public string DirectoryName
        {
            get { return this.Info.DirectoryName; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return this.Info.IsReadOnly; }
            set { this.Info.IsReadOnly = value; }
        }

        /// <inheritdoc />
        public long Length
        {
            get { return this.Info.Length; }
        }

        /// <inheritdoc />
        public StreamWriter AppendText()
        {
            return this.Info.AppendText();
        }

        /// <inheritdoc />
        public IFileInfo CopyTo(string destFileName)
        {
            return new FileInfo(this.Info.CopyTo(destFileName));
        }

        /// <inheritdoc />
        public IFileInfo CopyTo(string destFileName, bool overwrite)
        {
            return new FileInfo(this.Info.CopyTo(destFileName, overwrite));
        }

        /// <inheritdoc />
        public Stream Create()
        {
            return this.Info.Create();
        }

        /// <inheritdoc />
        public StreamWriter CreateText()
        {
            return this.Info.CreateText();
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public void Decrypt()
        {
            this.Info.Decrypt();
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public void Encrypt()
        {
            this.Info.Encrypt();
        }

        /// <inheritdoc />
        public FileSecurity GetAccessControl()
        {
            return this.Info.GetAccessControl();
        }

        /// <inheritdoc />
        public FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            return this.Info.GetAccessControl(includeSections);
        }

        /// <inheritdoc />
        public void MoveTo(string destFileName)
        {
            this.Info.MoveTo(destFileName);
        }

        /// <inheritdoc />
        public Stream Open(FileMode mode)
        {
            return this.Info.Open(mode);
        }

        /// <inheritdoc />
        public Stream Open(FileMode mode, FileAccess access)
        {
            return this.Info.Open(mode, access);
        }

        /// <inheritdoc />
        public Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return this.Info.Open(mode, access, share);
        }

        /// <inheritdoc />
        public Stream OpenRead()
        {
            return this.Info.OpenRead();
        }

        /// <inheritdoc />
        public StreamReader OpenText()
        {
            return this.Info.OpenText();
        }

        /// <inheritdoc />
        public Stream OpenWrite()
        {
            return this.Info.OpenWrite();
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public IFileInfo Replace(string destinationFileName, string destinationBackupFileName)
        {
            return new FileInfo(this.Info.Replace(destinationFileName, destinationBackupFileName));
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            return new FileInfo(this.Info.Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors));
        }

        /// <inheritdoc />
        public void SetAccessControl(FileSecurity fileSecurity)
        {
            this.Info.SetAccessControl(fileSecurity);
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
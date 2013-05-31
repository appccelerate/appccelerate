//-------------------------------------------------------------------------------
// <copyright file="FileSystemInfo{TInfo}.cs" company="Appccelerate">
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

    /// <summary>
    /// Abstract file system info access base class.
    /// </summary>
    /// <typeparam name="TInfo">The type of the file system info.</typeparam>
    [Serializable]
    public abstract class FileSystemInfo<TInfo> : MarshalByRefObject, IFileSystemInfo
        where TInfo : FileSystemInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemInfo{TInfo}"/> class.
        /// </summary>
        /// <param name="fileSystemInfo">The file system info.</param>
        protected FileSystemInfo(TInfo fileSystemInfo)
        {
            this.Info = fileSystemInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemInfo{TInfo}"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected FileSystemInfo(SerializationInfo info, StreamingContext context)
        {
        }

        /// <inheritdoc />
        public DateTime CreationTime
        {
            get { return this.Info.CreationTime; }
            set { this.Info.CreationTime = value; }
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public DateTime CreationTimeUtc
        {
            get { return this.Info.CreationTimeUtc; }
            set { this.Info.CreationTimeUtc = value; }
        }

        /// <inheritdoc />
        public bool Exists
        {
            get { return this.Info.Exists; }
        }

        /// <inheritdoc />
        public string FullName
        {
            get { return this.Info.FullName; }
        }

        /// <inheritdoc />
        public string Name
        {
            get { return this.Info.Name; }
        }

        /// <inheritdoc />
        public FileAttributes Attributes
        {
            get { return this.Info.Attributes; }
            set { this.Info.Attributes = value; }
        }

        /// <inheritdoc />
        public string Extension
        {
            get { return this.Info.Extension; }
        }

        /// <inheritdoc />
        public DateTime LastAccessTime
        {
            get { return this.Info.LastAccessTime; }
            set { this.Info.LastAccessTime = value; }
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public DateTime LastAccessTimeUtc
        {
            get { return this.Info.LastAccessTimeUtc; }
            set { this.Info.LastAccessTimeUtc = value; }
        }

        /// <inheritdoc />
        public DateTime LastWriteTime
        {
            get { return this.Info.LastWriteTime; }
            set { this.Info.LastWriteTime = value; }
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public DateTime LastWriteTimeUtc
        {
            get { return this.Info.LastWriteTimeUtc; }
            set { this.Info.LastWriteTimeUtc = value; }
        }

        /// <summary>
        /// Gets the file system info.
        /// </summary>
        /// <value>The file system info.</value>
        protected TInfo Info { get; private set; }

        /// <inheritdoc />
        public void Refresh()
        {
            this.Info.Refresh();
        }

        /// <inheritdoc />
        public void Delete()
        {
            this.Info.Delete();
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
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.Info.GetObjectData(info, context);
        }

        /// <summary>
        /// Creates an object that contains all the relevant information required to
        /// generate a proxy used to communicate with a remote object.
        /// </summary>
        /// <param name="requestedType">The <see cref="T:System.Type"/> of the object that
        /// the new <see cref="T:System.Runtime.Remoting.ObjRef"/> will reference.</param>
        /// <returns>
        /// Information required to generate a proxy.
        /// </returns>
        /// <exception cref="T:System.Runtime.Remoting.RemotingException">
        /// This instance is not a valid remoting object.
        /// </exception>
        /// <exception cref="T:System.Security.SecurityException">
        /// The immediate caller does not have infrastructure permission.
        /// </exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/>
        /// </PermissionSet>
        [SecurityCritical]
        public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        {
            return this.Info.CreateObjRef(requestedType);
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">
        /// The immediate caller does not have infrastructure permission.
        /// </exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        [SecurityCritical]
        public override object InitializeLifetimeService()
        {
            return this.Info.InitializeLifetimeService();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            return this.Info.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Info.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.Info.ToString();
        }
    }
}
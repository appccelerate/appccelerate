//-------------------------------------------------------------------------------
// <copyright file="IFileSystemInfo.cs" company="Appccelerate">
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
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    /// <summary>
    /// Base interface definition for file system information access.
    /// </summary>
    [CompilerGenerated]
    public interface IFileSystemInfo : ISerializable
    {
        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.Attributes"]' />
        FileAttributes Attributes { get; set; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.CreationTime"]' />
        DateTime CreationTime { get; set; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.CreationTimeUtc"]' />
        DateTime CreationTimeUtc { get; set; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.Exists"]' />
        bool Exists { get; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.FullName"]' />
        string FullName { get; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.Name"]' />
        string Name { get; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.Extension"]' />
        string Extension { get; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.LastAccessTime"]' />
        DateTime LastAccessTime { get; set; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.LastAccessTimeUtc"]' />
        [ComVisible(false)]
        DateTime LastAccessTimeUtc { get; set; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.LastWriteTime"]' />
        DateTime LastWriteTime { get; set; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="P:System.IO.FileSystemInfo.LastWriteTimeUtc"]' />
        [ComVisible(false)]
        DateTime LastWriteTimeUtc { get; set; }

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="M:System.IO.FileSystemInfo.Refresh"]' />
        void Refresh();

        /// <include file='IFileSystemInfo.doc.xml' path='doc/members/member[@name="M:System.IO.FileSystemInfo.Delete"]' />
        void Delete();
    }
}
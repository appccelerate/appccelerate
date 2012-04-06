//-------------------------------------------------------------------------------
// <copyright file="IFileInfo.cs" company="Appccelerate">
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
    using System.Runtime.InteropServices;
    using System.Security.AccessControl;

#pragma warning disable 1734

    /// <summary>
    /// Interface which simplifies the access to the file info.
    /// </summary>
    public interface IFileInfo : IFileSystemInfo
    {
        /// <include file='mscorlib.xml' path='/doc/members/member[@name="P:System.IO.FileInfo.Directory"]/*' />
        IDirectoryInfo Directory { get; }

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="P:System.IO.FileInfo.DirectoryName"]/*' />
        string DirectoryName { get; }

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="P:System.IO.FileInfo.IsReadOnly"]/*' />
        bool IsReadOnly { get; set; }

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="P:System.IO.FileInfo.Length"]/*' />
        long Length { get; }

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.AppendText"]/*' />
        StreamWriter AppendText();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.CopyTo(System.String)"]/*' />
        IFileInfo CopyTo(string destFileName);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.CopyTo(System.String, System.Boolean)"]/*' />
        IFileInfo CopyTo(string destFileName, bool overwrite);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.Create"]/*' />
        Stream Create();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.CreateText"]/*' />
        StreamWriter CreateText();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.Decrypt"]/*' />
        [ComVisible(false)]
        void Decrypt();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.Encrypt"]/*' />
        [ComVisible(false)]
        void Encrypt();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.GetAccessControl"]/*' />
        FileSecurity GetAccessControl();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.GetAccessControl(System.Security.AccessControl.AccessControlSections)"]/*' />
        FileSecurity GetAccessControl(AccessControlSections includeSections);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.MoveTo(System.String)"]/*' />
        void MoveTo(string destFileName);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.Open(System.IO.FileMode)"]/*' />
        Stream Open(FileMode mode);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.Open(System.IO.FileMode,System.IO.FileAccess)"]/*' />
        Stream Open(FileMode mode, System.IO.FileAccess access);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.Open(System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)"]/*' />
        Stream Open(FileMode mode, System.IO.FileAccess access, FileShare share);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.OpenRead"]/*' />
        Stream OpenRead();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.OpenText"]/*' />
        StreamReader OpenText();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.OpenWrite"]/*' />
        Stream OpenWrite();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.Replace(System.String,System.String)"]/*' />
        [ComVisible(false)]
        IFileInfo Replace(string destinationFileName, string destinationBackupFileName);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.Replace(System.String,System.String,System.Boolean)"]/*' />
        [ComVisible(false)]
        IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.FileInfo.SetAccessControl(System.Security.AccessControl.FileSecurity)"]/*' />
        void SetAccessControl(FileSecurity fileSecurity);
    }

#pragma warning restore 1734
}
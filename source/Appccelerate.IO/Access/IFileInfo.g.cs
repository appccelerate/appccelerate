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

    /// <summary>
    /// Interface which simplifies the access to the file info.
    /// </summary>
    public interface IFileInfo : IFileSystemInfo
    {
        IDirectoryInfo Directory { get; }

        string DirectoryName { get; }

        bool IsReadOnly { get; set; }

        long Length { get; }

        StreamWriter AppendText();

        IFileInfo CopyTo(string destFileName);

        IFileInfo CopyTo(string destFileName, bool overwrite);

        Stream Create();

        StreamWriter CreateText();

        [ComVisible(false)]
        void Decrypt();

        [ComVisible(false)]
        void Encrypt();

        FileSecurity GetAccessControl();

        FileSecurity GetAccessControl(AccessControlSections includeSections);

        void MoveTo(string destFileName);

        Stream Open(FileMode mode);

        Stream Open(FileMode mode, FileAccess access);

        Stream Open(FileMode mode, FileAccess access, FileShare share);

        Stream OpenRead();

        StreamReader OpenText();

        Stream OpenWrite();

        [ComVisible(false)]
        IFileInfo Replace(string destinationFileName, string destinationBackupFileName);

        [ComVisible(false)]
        IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);

        void SetAccessControl(FileSecurity fileSecurity);
    }
}
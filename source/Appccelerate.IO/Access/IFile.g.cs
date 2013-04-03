//-------------------------------------------------------------------------------
// <copyright file="IFile.cs" company="Appccelerate">
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
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Security.AccessControl;
    using System.Text;

    /// <summary>
    /// Interface which simplifies the access to the file system.
    /// </summary>
    [CompilerGenerated]
    public interface IFile
    {
        void Delete(string path);

        void Copy(string sourceFileName, string destFileName);

        void Copy(string sourceFileName, string destFileName, bool overwrite);

        StreamWriter CreateText(string path);

        FileAttributes GetAttributes(string path);

        void SetLastWriteTime(string path, DateTime lastWriteTime);

        void SetAttributes(string path, FileAttributes fileAttributes);

        bool Exists(string path);

        IEnumerable<byte> ReadAllBytes(string path);

        IEnumerable<string> ReadAllLines(string path, Encoding encoding);

        IEnumerable<string> ReadAllLines(string path);

        string ReadAllText(string path, Encoding encoding);

        string ReadAllText(string path);

        IEnumerable<string> ReadLines(string path);

        IEnumerable<string> ReadLines(string path, Encoding encoding);

        void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        void WriteAllLines(string path, IEnumerable<string> contents);

        void WriteAllText(string path, string contents);

        void WriteAllText(string path, string contents, Encoding encoding);

        void WriteAllBytes(string path, IEnumerable<byte> bytes);

        Stream Open(string path, FileMode mode);

        Stream Open(string path, FileMode mode, FileAccess access);

        Stream Open(string path, FileMode mode, FileAccess access, FileShare share);

        void AppendAllLines(string path, IEnumerable<string> contents);

        void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        void AppendAllText(string path, string contents);

        void AppendAllText(string path, string contents, Encoding encoding);

        StreamWriter AppendText(string path);

        Stream Create(string path);

        Stream Create(string path, int bufferSize);

        Stream Create(string path, int bufferSize, FileOptions options);

        Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity);

        void Decrypt(string path);

        void Encrypt(string path);

        FileSecurity GetAccessControl(string path);

        FileSecurity GetAccessControl(string path, AccessControlSections includeSections);

        DateTime GetCreationTime(string path);

        DateTime GetCreationTimeUtc(string path);

        DateTime GetLastAccessTime(string path);

        DateTime GetLastAccessTimeUtc(string path);

        DateTime GetLastWriteTime(string path);

        DateTime GetLastWriteTimeUtc(string path);

        void Move(string sourceFileName, string destFileName);

        Stream OpenRead(string path);

        StreamReader OpenText(string path);

        Stream OpenWrite(string path);

        void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName);

        void Replace(
            string sourceFileName,
            string destinationFileName,
            string destinationBackupFileName,
            bool ignoreMetadataErrors);

        void SetAccessControl(string path, FileSecurity fileSecurity);

        void SetCreationTime(string path, DateTime creationTime);

        void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

        void SetLastAccessTime(string path, DateTime lastAccessTime);

        void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);
    }
}
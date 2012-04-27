//-------------------------------------------------------------------------------
// <copyright file="FileExtensionBase.cs" company="Appccelerate">
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
    using System.Security.AccessControl;
    using System.Text;

    /// <summary>
    /// Base extension for file extensions.
    /// </summary>
    public class FileExtensionBase : IFileExtension
    {
        /// <inheritdoc />
        public virtual void BeginGetLastWriteTime(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetLastWriteTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetLastWriteTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetLastWriteTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetLastWriteTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetLastWriteTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginMove(string sourceFileName, string destFileName)
        {
        }

        /// <inheritdoc />
        public virtual void EndMove(string sourceFileName, string destFileName)
        {
        }

        /// <inheritdoc />
        public virtual void FailMove(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginOpenRead(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndOpenRead(FileStream result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailOpenRead(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginOpenText(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndOpenText(StreamReader result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailOpenText(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginOpenWrite(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndOpenWrite(FileStream result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailOpenWrite(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
        }

        /// <inheritdoc />
        public virtual void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
        }

        /// <inheritdoc />
        public virtual void FailReplace(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
        }

        /// <inheritdoc />
        public virtual void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetAccessControl(string path, FileSecurity fileSecurity)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetAccessControl(string path, FileSecurity fileSecurity)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetAccessControl(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetCreationTime(string path, DateTime creationTime)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetCreationTime(string path, DateTime creationTime)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetCreationTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetCreationTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetLastAccessTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetLastAccessTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetLastWriteTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginDelete(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndDelete(string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailDelete(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCopy(string sourceFileName, string destFileName)
        {
        }

        /// <inheritdoc />
        public virtual void EndCopy(string sourceFileName, string destFileName)
        {
        }

        /// <inheritdoc />
        public virtual void FailCopy(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCopy(string sourceFileName, string destFileName, bool overwrite)
        {
        }

        /// <inheritdoc />
        public virtual void EndCopy(string sourceFileName, string destFileName, bool overwrite)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCreateText(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndCreateText(StreamWriter result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailCreateText(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetAttributes(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetAttributes(FileAttributes result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetAttributes(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetLastWriteTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginSetAttributes(string path, FileAttributes fileAttributes)
        {
        }

        /// <inheritdoc />
        public virtual void EndSetAttributes(string path, FileAttributes fileAttributes)
        {
        }

        /// <inheritdoc />
        public virtual void FailSetAttributes(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginExists(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndExists(bool result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailExists(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginReadAllBytes(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndReadAllBytes(byte[] result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailReadAllBytes(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginReadAllLines(string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void EndReadAllLines(string[] result, string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void FailReadAllLines(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginReadAllLines(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndReadAllLines(string[] result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void BeginReadAllText(string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void EndReadAllText(string result, string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void BeginReadAllText(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndReadAllText(string result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailReadAllText(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void EndWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void FailWriteAllLines(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginWriteAllLines(string path, IEnumerable<string> contents)
        {
        }

        /// <inheritdoc />
        public virtual void EndWriteAllLines(string path, IEnumerable<string> contents)
        {
        }

        /// <inheritdoc />
        public virtual void BeginWriteAllText(string path, string contents)
        {
        }

        /// <inheritdoc />
        public virtual void EndWriteAllText(string path, string contents)
        {
        }

        /// <inheritdoc />
        public virtual void FailWriteAllText(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginWriteAllText(string path, string contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void EndWriteAllText(string path, string contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void BeginWriteAllBytes(string path, byte[] bytes)
        {
        }

        /// <inheritdoc />
        public virtual void EndWriteAllBytes(string path, byte[] bytes)
        {
        }

        /// <inheritdoc />
        public virtual void FailWriteAllBytes(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginOpen(string path, FileMode mode)
        {
        }

        /// <inheritdoc />
        public virtual void EndOpen(FileStream result, string path, FileMode mode)
        {
        }

        /// <inheritdoc />
        public virtual void FailOpen(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginOpen(string path, FileMode mode, FileAccess access)
        {
        }

        /// <inheritdoc />
        public virtual void EndOpen(FileStream result, string path, FileMode mode, FileAccess access)
        {
        }

        /// <inheritdoc />
        public virtual void BeginOpen(string path, FileMode mode, FileAccess access, FileShare share)
        {
        }

        /// <inheritdoc />
        public virtual void EndOpen(FileStream result, string path, FileMode mode, FileAccess access, FileShare share)
        {
        }

        /// <inheritdoc />
        public virtual void BeginAppendAllText(string path, string contents)
        {
        }

        /// <inheritdoc />
        public virtual void EndAppendAllText(string path, string contents)
        {
        }

        /// <inheritdoc />
        public virtual void FailAppendAllText(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginAppendAllText(string path, string contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void EndAppendAllText(string path, string contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public virtual void BeginAppendText(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndAppendText(StreamWriter result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailAppendText(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCreate(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndCreate(FileStream result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailCreate(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCreate(string path, int bufferSize)
        {
        }

        /// <inheritdoc />
        public virtual void EndCreate(FileStream result, string path, int bufferSize)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCreate(string path, int bufferSize, FileOptions options)
        {
        }

        /// <inheritdoc />
        public virtual void EndCreate(FileStream result, string path, int bufferSize, FileOptions options)
        {
        }

        /// <inheritdoc />
        public virtual void BeginCreate(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
        }

        /// <inheritdoc />
        public virtual void EndCreate(FileStream result, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
        }

        /// <inheritdoc />
        public virtual void BeginDecrypt(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndDecrypt(string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailDecrypt(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginEncrypt(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndEncrypt(string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailEncrypt(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetAccessControl(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetAccessControl(FileSecurity result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetAccessControl(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetAccessControl(string path, AccessControlSections includeSections)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetAccessControl(FileSecurity result, string path, AccessControlSections includeSections)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetCreationTime(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetCreationTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetCreationTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetCreationTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetCreationTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetCreationTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetLastAccessTime(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetLastAccessTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetLastAccessTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public virtual void BeginGetLastAccessTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public virtual void EndGetLastAccessTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public virtual void FailGetLastAccessTimeUtc(ref Exception exception)
        {
        }
    }
}
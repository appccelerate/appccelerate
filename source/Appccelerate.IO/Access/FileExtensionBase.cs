//-------------------------------------------------------------------------------
// <copyright file="FileExtensionBase.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Security.AccessControl;
    using System.Text;

    /// <summary>
    /// Base extension for file extensions.
    /// </summary>
    public class FileExtensionBase : IFileExtension
    {
        public virtual void BeginGetLastWriteTime(string path)
        {
        }

        public virtual void EndGetLastWriteTime(DateTime result, string path)
        {
        }

        public virtual void FailGetLastWriteTime(ref Exception exception)
        {
        }

        public virtual void BeginGetLastWriteTimeUtc(string path)
        {
        }

        public virtual void EndGetLastWriteTimeUtc(DateTime result, string path)
        {
        }

        public virtual void FailGetLastWriteTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginMove(string sourceFileName, string destFileName)
        {
        }

        public virtual void EndMove(string sourceFileName, string destFileName)
        {
        }

        public virtual void FailMove(ref Exception exception)
        {
        }

        public virtual void BeginOpenRead(string path)
        {
        }

        public virtual void EndOpenRead(FileStream result, string path)
        {
        }

        public virtual void FailOpenRead(ref Exception exception)
        {
        }

        public virtual void BeginOpenText(string path)
        {
        }

        public virtual void EndOpenText(StreamReader result, string path)
        {
        }

        public virtual void FailOpenText(ref Exception exception)
        {
        }

        public virtual void BeginOpenWrite(string path)
        {
        }

        public virtual void EndOpenWrite(FileStream result, string path)
        {
        }

        public virtual void FailOpenWrite(ref Exception exception)
        {
        }

        public virtual void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
        }

        public virtual void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
        }

        public virtual void FailReplace(ref Exception exception)
        {
        }

        public virtual void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
        }

        public virtual void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
        }

        public virtual void BeginSetAccessControl(string path, FileSecurity fileSecurity)
        {
        }

        public virtual void EndSetAccessControl(string path, FileSecurity fileSecurity)
        {
        }

        public virtual void FailSetAccessControl(ref Exception exception)
        {
        }

        public virtual void BeginSetCreationTime(string path, DateTime creationTime)
        {
        }

        public virtual void EndSetCreationTime(string path, DateTime creationTime)
        {
        }

        public virtual void FailSetCreationTime(ref Exception exception)
        {
        }

        public virtual void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        public virtual void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        public virtual void FailSetCreationTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        public virtual void EndSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        public virtual void FailSetLastAccessTime(ref Exception exception)
        {
        }

        public virtual void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        public virtual void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        public virtual void FailSetLastAccessTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        public virtual void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        public virtual void FailSetLastWriteTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginDelete(string path)
        {
        }

        public virtual void EndDelete(string path)
        {
        }

        public virtual void FailDelete(ref Exception exception)
        {
        }

        public virtual void BeginCopy(string sourceFileName, string destFileName)
        {
        }

        public virtual void EndCopy(string sourceFileName, string destFileName)
        {
        }

        public virtual void FailCopy(ref Exception exception)
        {
        }

        public virtual void BeginCopy(string sourceFileName, string destFileName, bool overwrite)
        {
        }

        public virtual void EndCopy(string sourceFileName, string destFileName, bool overwrite)
        {
        }

        public virtual void BeginCreateText(string path)
        {
        }

        public virtual void EndCreateText(StreamWriter result, string path)
        {
        }

        public virtual void FailCreateText(ref Exception exception)
        {
        }

        public virtual void BeginGetAttributes(string path)
        {
        }

        public virtual void EndGetAttributes(FileAttributes result, string path)
        {
        }

        public virtual void FailGetAttributes(ref Exception exception)
        {
        }

        public virtual void BeginSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        public virtual void EndSetLastWriteTime(string path, DateTime lastWriteTime)
        {
        }

        public virtual void FailSetLastWriteTime(ref Exception exception)
        {
        }

        public virtual void BeginSetAttributes(string path, FileAttributes fileAttributes)
        {
        }

        public virtual void EndSetAttributes(string path, FileAttributes fileAttributes)
        {
        }

        public virtual void FailSetAttributes(ref Exception exception)
        {
        }

        public virtual void BeginExists(string path)
        {
        }

        public virtual void EndExists(bool result, string path)
        {
        }

        public virtual void FailExists(ref Exception exception)
        {
        }

        public virtual void BeginReadAllBytes(string path)
        {
        }

        public virtual void EndReadAllBytes(byte[] result, string path)
        {
        }

        public virtual void FailReadAllBytes(ref Exception exception)
        {
        }

        public virtual void BeginReadAllLines(string path, Encoding encoding)
        {
        }

        public virtual void EndReadAllLines(string[] result, string path, Encoding encoding)
        {
        }

        public virtual void FailReadAllLines(ref Exception exception)
        {
        }

        public virtual void BeginReadAllLines(string path)
        {
        }

        public virtual void EndReadAllLines(string[] result, string path)
        {
        }

        public virtual void BeginReadAllText(string path, Encoding encoding)
        {
        }

        public virtual void EndReadAllText(string result, string path, Encoding encoding)
        {
        }

        public virtual void BeginReadAllText(string path)
        {
        }

        public virtual void EndReadAllText(string result, string path)
        {
        }

        public virtual void BeginReadLines(string path)
        {
        }

        public virtual void EndReadLines(IEnumerable<string> result, string path)
        {
        }

        public virtual void FailReadLines(ref Exception exception)
        {
        }

        public virtual void BeginReadLines(string path, Encoding encoding)
        {
        }

        public virtual void EndReadLines(IEnumerable<string> result, string path, Encoding encoding)
        {
        }

        public virtual void FailReadAllText(ref Exception exception)
        {
        }

        public virtual void BeginWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        public virtual void EndWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        public virtual void FailWriteAllLines(ref Exception exception)
        {
        }

        public virtual void BeginWriteAllLines(string path, IEnumerable<string> contents)
        {
        }

        public virtual void EndWriteAllLines(string path, IEnumerable<string> contents)
        {
        }

        public virtual void BeginWriteAllText(string path, string contents)
        {
        }

        public virtual void EndWriteAllText(string path, string contents)
        {
        }

        public virtual void FailWriteAllText(ref Exception exception)
        {
        }

        public virtual void BeginWriteAllText(string path, string contents, Encoding encoding)
        {
        }

        public virtual void EndWriteAllText(string path, string contents, Encoding encoding)
        {
        }

        public virtual void BeginWriteAllBytes(string path, byte[] bytes)
        {
        }

        public virtual void EndWriteAllBytes(string path, byte[] bytes)
        {
        }

        public virtual void FailWriteAllBytes(ref Exception exception)
        {
        }

        public virtual void BeginOpen(string path, FileMode mode)
        {
        }

        public virtual void EndOpen(FileStream result, string path, FileMode mode)
        {
        }

        public virtual void FailOpen(ref Exception exception)
        {
        }

        public virtual void BeginOpen(string path, FileMode mode, FileAccess access)
        {
        }

        public virtual void EndOpen(FileStream result, string path, FileMode mode, FileAccess access)
        {
        }

        public virtual void BeginOpen(string path, FileMode mode, FileAccess access, FileShare share)
        {
        }

        public virtual void EndOpen(FileStream result, string path, FileMode mode, FileAccess access, FileShare share)
        {
        }

        public virtual void BeginAppendAllLines(string path, IEnumerable<string> contents)
        {
        }

        public virtual void EndAppendAllLines(string path, IEnumerable<string> contents)
        {
        }

        public virtual void FailAppendAllLines(ref Exception exception)
        {
        }

        public virtual void BeginAppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        public virtual void EndAppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        public virtual void BeginAppendAllText(string path, string contents)
        {
        }

        public virtual void EndAppendAllText(string path, string contents)
        {
        }

        public virtual void FailAppendAllText(ref Exception exception)
        {
        }

        public virtual void BeginAppendAllText(string path, string contents, Encoding encoding)
        {
        }

        public virtual void EndAppendAllText(string path, string contents, Encoding encoding)
        {
        }

        public virtual void BeginAppendText(string path)
        {
        }

        public virtual void EndAppendText(StreamWriter result, string path)
        {
        }

        public virtual void FailAppendText(ref Exception exception)
        {
        }

        public virtual void BeginCreate(string path)
        {
        }

        public virtual void EndCreate(FileStream result, string path)
        {
        }

        public virtual void FailCreate(ref Exception exception)
        {
        }

        public virtual void BeginCreate(string path, int bufferSize)
        {
        }

        public virtual void EndCreate(FileStream result, string path, int bufferSize)
        {
        }

        public virtual void BeginCreate(string path, int bufferSize, FileOptions options)
        {
        }

        public virtual void EndCreate(FileStream result, string path, int bufferSize, FileOptions options)
        {
        }

        public virtual void BeginCreate(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
        }

        public virtual void EndCreate(FileStream result, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
        }

        public virtual void BeginDecrypt(string path)
        {
        }

        public virtual void EndDecrypt(string path)
        {
        }

        public virtual void FailDecrypt(ref Exception exception)
        {
        }

        public virtual void BeginEncrypt(string path)
        {
        }

        public virtual void EndEncrypt(string path)
        {
        }

        public virtual void FailEncrypt(ref Exception exception)
        {
        }

        public virtual void BeginGetAccessControl(string path)
        {
        }

        public virtual void EndGetAccessControl(FileSecurity result, string path)
        {
        }

        public virtual void FailGetAccessControl(ref Exception exception)
        {
        }

        public virtual void BeginGetAccessControl(string path, AccessControlSections includeSections)
        {
        }

        public virtual void EndGetAccessControl(FileSecurity result, string path, AccessControlSections includeSections)
        {
        }

        public virtual void BeginGetCreationTime(string path)
        {
        }

        public virtual void EndGetCreationTime(DateTime result, string path)
        {
        }

        public virtual void FailGetCreationTime(ref Exception exception)
        {
        }

        public virtual void BeginGetCreationTimeUtc(string path)
        {
        }

        public virtual void EndGetCreationTimeUtc(DateTime result, string path)
        {
        }

        public virtual void FailGetCreationTimeUtc(ref Exception exception)
        {
        }

        public virtual void BeginGetLastAccessTime(string path)
        {
        }

        public virtual void EndGetLastAccessTime(DateTime result, string path)
        {
        }

        public virtual void FailGetLastAccessTime(ref Exception exception)
        {
        }

        public virtual void BeginGetLastAccessTimeUtc(string path)
        {
        }

        public virtual void EndGetLastAccessTimeUtc(DateTime result, string path)
        {
        }

        public virtual void FailGetLastAccessTimeUtc(ref Exception exception)
        {
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="IFileExtension.cs" company="Appccelerate">
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
    using System.Runtime.CompilerServices;
    using System.Security.AccessControl;
    using System.Text;
    /// <summary>
    /// Interface for file access extensions
    /// </summary>
    [CompilerGenerated]
    public interface IFileExtension
    {
        void BeginGetLastWriteTime(string path);

        void EndGetLastWriteTime(DateTime result, string path);

        void FailGetLastWriteTime(ref Exception exception);

        void BeginGetLastWriteTimeUtc(string path);

        void EndGetLastWriteTimeUtc(DateTime result, string path);

        void FailGetLastWriteTimeUtc(ref Exception exception);

        void BeginMove(string sourceFileName, string destFileName);

        void EndMove(string sourceFileName, string destFileName);

        void FailMove(ref Exception exception);

        void BeginOpenRead(string path);

        void EndOpenRead(FileStream result, string path);

        void FailOpenRead(ref Exception exception);

        void BeginOpenText(string path);

        void EndOpenText(StreamReader result, string path);

        void FailOpenText(ref Exception exception);

        void BeginOpenWrite(string path);

        void EndOpenWrite(FileStream result, string path);

        void FailOpenWrite(ref Exception exception);

        void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName);

        void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName);

        void FailReplace(ref Exception exception);

        void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);

        void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);

        void BeginSetAccessControl(string path, FileSecurity fileSecurity);

        void EndSetAccessControl(string path, FileSecurity fileSecurity);

        void FailSetAccessControl(ref Exception exception);

        void BeginSetCreationTime(string path, DateTime creationTime);

        void EndSetCreationTime(string path, DateTime creationTime);

        void FailSetCreationTime(ref Exception exception);

        void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        void FailSetCreationTimeUtc(ref Exception exception);

        void BeginSetLastAccessTime(string path, DateTime lastAccessTime);

        void EndSetLastAccessTime(string path, DateTime lastAccessTime);

        void FailSetLastAccessTime(ref Exception exception);

        void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        void FailSetLastAccessTimeUtc(ref Exception exception);

        void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        void FailSetLastWriteTimeUtc(ref Exception exception);

        void BeginDelete(string path);

        void EndDelete(string path);

        void FailDelete(ref Exception exception);

        void BeginCopy(string sourceFileName, string destFileName);

        void EndCopy(string sourceFileName, string destFileName);

        void FailCopy(ref Exception exception);

        void BeginCopy(string sourceFileName, string destFileName, bool overwrite);

        void EndCopy(string sourceFileName, string destFileName, bool overwrite);

        void BeginCreateText(string path);

        void EndCreateText(StreamWriter result, string path);

        void FailCreateText(ref Exception exception);

        void BeginGetAttributes(string path);

        void EndGetAttributes(FileAttributes result, string path);

        void FailGetAttributes(ref Exception exception);

        void BeginSetLastWriteTime(string path, DateTime lastWriteTime);

        void EndSetLastWriteTime(string path, DateTime lastWriteTime);

        void FailSetLastWriteTime(ref Exception exception);

        void BeginSetAttributes(string path, FileAttributes fileAttributes);

        void EndSetAttributes(string path, FileAttributes fileAttributes);

        void FailSetAttributes(ref Exception exception);

        void BeginExists(string path);

        void EndExists(bool result, string path);

        void FailExists(ref Exception exception);

        void BeginReadAllBytes(string path);

        void EndReadAllBytes(byte[] result, string path);

        void FailReadAllBytes(ref Exception exception);

        void BeginReadAllLines(string path, Encoding encoding);

        void EndReadAllLines(string[] result, string path, Encoding encoding);

        void FailReadAllLines(ref Exception exception);

        void BeginReadAllLines(string path);

        void EndReadAllLines(string[] result, string path);

        void BeginReadAllText(string path, Encoding encoding);

        void EndReadAllText(string result, string path, Encoding encoding);

        void FailReadAllText(ref Exception exception);

        void BeginReadAllText(string path);

        void EndReadAllText(string result, string path);

        void BeginReadLines(string path);

        void EndReadLines(IEnumerable<string> result, string path);

        void FailReadLines(ref Exception exception);

        void BeginReadLines(string path, Encoding encoding);

        void EndReadLines(IEnumerable<string> result, string path, Encoding encoding);

        void BeginWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        void EndWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        void FailWriteAllLines(ref Exception exception);

        void BeginWriteAllLines(string path, IEnumerable<string> contents);

        void EndWriteAllLines(string path, IEnumerable<string> contents);

        void BeginWriteAllText(string path, string contents);

        void EndWriteAllText(string path, string contents);

        void FailWriteAllText(ref Exception exception);

        void BeginWriteAllText(string path, string contents, Encoding encoding);

        void EndWriteAllText(string path, string contents, Encoding encoding);

        void BeginWriteAllBytes(string path, byte[] bytes);

        void EndWriteAllBytes(string path, byte[] bytes);

        void FailWriteAllBytes(ref Exception exception);

        void BeginOpen(string path, FileMode mode);

        void EndOpen(FileStream result, string path, FileMode mode);

        void FailOpen(ref Exception exception);

        void BeginOpen(string path, FileMode mode, FileAccess access);

        void EndOpen(FileStream result, string path, FileMode mode, FileAccess access);

        void BeginOpen(string path, FileMode mode, FileAccess access, FileShare share);

        void EndOpen(FileStream result, string path, FileMode mode, FileAccess access, FileShare share);

        void BeginAppendAllLines(string path, IEnumerable<string> contents);

        void EndAppendAllLines(string path, IEnumerable<string> contents);

        void FailAppendAllLines(ref Exception exception);

        void BeginAppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        void EndAppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        void BeginAppendAllText(string path, string contents);

        void EndAppendAllText(string path, string contents);

        void FailAppendAllText(ref Exception exception);

        void BeginAppendAllText(string path, string contents, Encoding encoding);

        void EndAppendAllText(string path, string contents, Encoding encoding);

        void BeginAppendText(string path);

        void EndAppendText(StreamWriter result, string path);

        void FailAppendText(ref Exception exception);

        void BeginCreate(string path);

        void EndCreate(FileStream result, string path);

        void FailCreate(ref Exception exception);

        void BeginCreate(string path, int bufferSize);

        void EndCreate(FileStream result, string path, int bufferSize);

        void BeginCreate(string path, int bufferSize, FileOptions options);

        void EndCreate(FileStream result, string path, int bufferSize, FileOptions options);

        void BeginCreate(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity);

        void EndCreate(FileStream result, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity);

        void BeginDecrypt(string path);

        void EndDecrypt(string path);

        void FailDecrypt(ref Exception exception);

        void BeginEncrypt(string path);

        void EndEncrypt(string path);

        void FailEncrypt(ref Exception exception);

        void BeginGetAccessControl(string path);

        void EndGetAccessControl(FileSecurity result, string path);

        void FailGetAccessControl(ref Exception exception);

        void BeginGetAccessControl(string path, AccessControlSections includeSections);

        void EndGetAccessControl(FileSecurity result, string path, AccessControlSections includeSections);

        void BeginGetCreationTime(string path);

        void EndGetCreationTime(DateTime result, string path);

        void FailGetCreationTime(ref Exception exception);

        void BeginGetCreationTimeUtc(string path);

        void EndGetCreationTimeUtc(DateTime result, string path);

        void FailGetCreationTimeUtc(ref Exception exception);

        void BeginGetLastAccessTime(string path);

        void EndGetLastAccessTime(DateTime result, string path);

        void FailGetLastAccessTime(ref Exception exception);

        void BeginGetLastAccessTimeUtc(string path);

        void EndGetLastAccessTimeUtc(DateTime result, string path);

        void FailGetLastAccessTimeUtc(ref Exception exception);
    }
}
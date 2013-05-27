//-------------------------------------------------------------------------------
// <copyright file="FileLogExtension.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Security.AccessControl;
    using System.Text;

    using Appccelerate.IO.Access;

    using log4net;

    /// <summary>
    /// File access extension which logs actions with log4net.
    /// </summary>
    public class FileLogExtension : FileExtensionBase
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogExtension"/> class.
        /// </summary>
        public FileLogExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FileLogExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FileLogExtension(ILog logger)
        {
            this.log = logger;
        }

        /// <inheritdoc />
        public override void BeginGetLastWriteTime(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetLastWriteTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetLastWriteTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetLastWriteTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetLastWriteTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetLastWriteTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginMove(string sourceFileName, string destFileName)
        {
        }

        /// <inheritdoc />
        public override void EndMove(string sourceFileName, string destFileName)
        {
        }

        /// <inheritdoc />
        public override void FailMove(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginOpenRead(string path)
        {
        }

        /// <inheritdoc />
        public override void EndOpenRead(FileStream result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailOpenRead(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginOpenText(string path)
        {
        }

        /// <inheritdoc />
        public override void EndOpenText(StreamReader result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailOpenText(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginOpenWrite(string path)
        {
        }

        /// <inheritdoc />
        public override void EndOpenWrite(FileStream result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailOpenWrite(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
        }

        /// <inheritdoc />
        public override void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
        }

        /// <inheritdoc />
        public override void FailReplace(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
        }

        /// <inheritdoc />
        public override void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
        }

        /// <inheritdoc />
        public override void BeginSetAccessControl(string path, FileSecurity fileSecurity)
        {
        }

        /// <inheritdoc />
        public override void EndSetAccessControl(string path, FileSecurity fileSecurity)
        {
        }

        /// <inheritdoc />
        public override void FailSetAccessControl(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetCreationTime(string path, DateTime creationTime)
        {
        }

        /// <inheritdoc />
        public override void EndSetCreationTime(string path, DateTime creationTime)
        {
        }

        /// <inheritdoc />
        public override void FailSetCreationTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void FailSetCreationTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        /// <inheritdoc />
        public override void EndSetLastAccessTime(string path, DateTime lastAccessTime)
        {
        }

        /// <inheritdoc />
        public override void FailSetLastAccessTime(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void FailSetLastAccessTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
        }

        /// <inheritdoc />
        public override void FailSetLastWriteTimeUtc(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginDelete(string path)
        {
            base.BeginDelete(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Deleting {0}.", path);
        }

        /// <inheritdoc />
        public override void EndDelete(string path)
        {
            base.EndDelete(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Deleted {0}.", path);
        }

        /// <inheritdoc />
        public override void FailDelete(ref Exception exception)
        {
            base.FailDelete(ref exception);

            this.log.Error("Exception occured while deleting!", exception);
        }

        /// <inheritdoc />
        public override void BeginCopy(string sourceFileName, string destFileName)
        {
            base.BeginCopy(sourceFileName, destFileName);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Copying from {0} to {1}.", sourceFileName, destFileName);
        }

        /// <inheritdoc />
        public override void EndCopy(string sourceFileName, string destFileName)
        {
            base.EndCopy(sourceFileName, destFileName);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Copied from {0} to {1}.", sourceFileName, destFileName);
        }

        /// <inheritdoc />
        public override void BeginCopy(string sourceFileName, string destFileName, bool overwrite)
        {
            base.BeginCopy(sourceFileName, destFileName, overwrite);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Copying from {0} to {1} {2} overwritting.", sourceFileName, destFileName, overwrite ? "with" : "without");
        }

        /// <inheritdoc />
        public override void EndCopy(string sourceFileName, string destFileName, bool overwrite)
        {
            base.EndCopy(sourceFileName, destFileName, overwrite);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Copied from {0} to {1} {2} overwritting.", sourceFileName, destFileName, overwrite ? "with" : "without");
        }

        /// <inheritdoc />
        public override void BeginCreateText(string path)
        {
            base.BeginCreateText(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Creating text in {0}.", path);
        }

        /// <inheritdoc />
        public override void EndCreateText(StreamWriter result, string path)
        {
            base.EndCreateText(result, path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Created text in {0}.", path);
        }

        /// <inheritdoc />
        public override void FailCreateText(ref Exception exception)
        {
            base.FailCreateText(ref exception);

            this.log.Error("Exception occured while creating text!", exception);
        }

        /// <inheritdoc />
        public override void BeginGetAttributes(string path)
        {
            base.BeginGetAttributes(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting attributes from {0}.", path);
        }

        /// <inheritdoc />
        public override void EndGetAttributes(FileAttributes result, string path)
        {
            base.EndGetAttributes(result, path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Got attributes {0} from {1}.", result, path);
        }

        /// <inheritdoc />
        public override void FailGetAttributes(ref Exception exception)
        {
            base.FailGetAttributes(ref exception);

            this.log.Error("Exception occured while getting attributes!", exception);
        }

        /// <inheritdoc />
        public override void BeginSetLastWriteTime(string path, DateTime lastWriteTime)
        {
            base.BeginSetLastWriteTime(path, lastWriteTime);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Setting last write time from {0} to {1}.", path, lastWriteTime);
        }

        /// <inheritdoc />
        public override void EndSetLastWriteTime(string path, DateTime lastWriteTime)
        {
            base.EndSetLastWriteTime(path, lastWriteTime);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Set last write time from {0} to {1}.", path, lastWriteTime);
        }

        /// <inheritdoc />
        public override void FailSetLastWriteTime(ref Exception exception)
        {
            base.FailSetLastWriteTime(ref exception);

            this.log.Error("Exception occured while setting last write time!", exception);
        }

        /// <inheritdoc />
        public override void BeginSetAttributes(string path, FileAttributes fileAttributes)
        {
            base.BeginSetAttributes(path, fileAttributes);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Setting file attribute from {0} to {1}.", path, fileAttributes);
        }

        /// <inheritdoc />
        public override void EndSetAttributes(string path, FileAttributes fileAttributes)
        {
            base.EndSetAttributes(path, fileAttributes);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Set file attribute from {0} to {1}.", path, fileAttributes);
        }

        /// <inheritdoc />
        public override void FailSetAttributes(ref Exception exception)
        {
            base.FailSetAttributes(ref exception);

            this.log.Error("Exception occured while setting file attributes!", exception);
        }

        /// <inheritdoc />
        public override void BeginExists(string path)
        {
            base.BeginExists(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Checking file {0} for existance.", path);
        }

        /// <inheritdoc />
        public override void EndExists(bool result, string path)
        {
            base.EndExists(result, path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "FIle {0} {1}.", path, result ? "exists" : "does not exist");
        }

        /// <inheritdoc />
        public override void FailExists(ref Exception exception)
        {
            base.FailExists(ref exception);

            this.log.Error("Exception occured while checking file existance!", exception);
        }

        /// <inheritdoc />
        public override void BeginReadAllBytes(string path)
        {
        }

        /// <inheritdoc />
        public override void EndReadAllBytes(byte[] result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailReadAllBytes(ref Exception exception)
        {
            base.FailReadAllBytes(ref exception);

            this.log.Error("Exception occurred while reading all bytes.", exception);
        }

        /// <inheritdoc />
        public override void BeginReadAllLines(string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void EndReadAllLines(string[] result, string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void FailReadAllLines(ref Exception exception)
        {
            base.FailReadAllLines(ref exception);

            this.log.Error("Exception occurred while reading all lines.", exception);
        }

        /// <inheritdoc />
        public override void BeginReadAllLines(string path)
        {
        }

        /// <inheritdoc />
        public override void EndReadAllLines(string[] result, string path)
        {
        }

        /// <inheritdoc />
        public override void BeginReadAllText(string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void EndReadAllText(string result, string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void BeginReadLines(string path)
        {
        }

        /// <inheritdoc />
        public override void EndReadLines(IEnumerable<string> result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailReadLines(ref Exception exception)
        {
            base.FailReadLines(ref exception);

            this.log.Error("Exception occurred while reading lines.", exception);
        }

        /// <inheritdoc />
        public override void BeginReadLines(string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void EndReadLines(IEnumerable<string> result, string path, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void FailReadAllText(ref Exception exception)
        {
            base.FailReadAllText(ref exception);

            this.log.Error("Exception occurred while reading all text.", exception);
        }

        /// <inheritdoc />
        public override void BeginWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void EndWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void FailWriteAllLines(ref Exception exception)
        {
            base.FailWriteAllLines(ref exception);

            this.log.Error("Exception occurred while writing all lines.", exception);
        }

        /// <inheritdoc />
        public override void BeginWriteAllLines(string path, IEnumerable<string> contents)
        {
        }

        /// <inheritdoc />
        public override void EndWriteAllLines(string path, IEnumerable<string> contents)
        {
        }

        /// <inheritdoc />
        public override void BeginWriteAllText(string path, string contents)
        {
        }

        /// <inheritdoc />
        public override void EndWriteAllText(string path, string contents)
        {
        }

        /// <inheritdoc />
        public override void FailWriteAllText(ref Exception exception)
        {
            base.FailWriteAllText(ref exception);

            this.log.Error("Exception occurred while writing all text.", exception);
        }

        /// <inheritdoc />
        public override void BeginWriteAllText(string path, string contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void EndWriteAllText(string path, string contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void BeginWriteAllBytes(string path, byte[] bytes)
        {
        }

        /// <inheritdoc />
        public override void EndWriteAllBytes(string path, byte[] bytes)
        {
        }

        /// <inheritdoc />
        public override void FailWriteAllBytes(ref Exception exception)
        {
            base.FailWriteAllBytes(ref exception);

            this.log.Error("Exception occurred while writing all bytes.", exception);
        }

        /// <inheritdoc />
        public override void BeginOpen(string path, FileMode mode)
        {
        }

        /// <inheritdoc />
        public override void EndOpen(FileStream result, string path, FileMode mode)
        {
        }

        /// <inheritdoc />
        public override void FailOpen(ref Exception exception)
        {
            base.FailOpen(ref exception);

            this.log.Error("Exception occurred while opening a file or stream.", exception);
        }

        /// <inheritdoc />
        public override void BeginOpen(string path, FileMode mode, FileAccess access)
        {
        }

        /// <inheritdoc />
        public override void EndOpen(FileStream result, string path, FileMode mode, FileAccess access)
        {
        }

        /// <inheritdoc />
        public override void BeginOpen(string path, FileMode mode, FileAccess access, FileShare share)
        {
        }

        /// <inheritdoc />
        public override void EndOpen(FileStream result, string path, FileMode mode, FileAccess access, FileShare share)
        {
        }

        /// <inheritdoc />
        public override void BeginAppendAllLines(string path, IEnumerable<string> contents)
        {
        }

        /// <inheritdoc />
        public override void EndAppendAllLines(string path, IEnumerable<string> contents)
        {
        }

        /// <inheritdoc />
        public override void FailAppendAllLines(ref Exception exception)
        {
            base.FailAppendAllLines(ref exception);

            this.log.Error("Exception occurred while appending all lines.", exception);
        }

        /// <inheritdoc />
        public override void BeginAppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void EndAppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void BeginAppendAllText(string path, string contents)
        {
        }

        /// <inheritdoc />
        public override void EndAppendAllText(string path, string contents)
        {
        }

        /// <inheritdoc />
        public override void FailAppendAllText(ref Exception exception)
        {
            base.FailAppendAllText(ref exception);

            this.log.Error("Exception occurred while appending all text.", exception);
        }

        /// <inheritdoc />
        public override void BeginAppendAllText(string path, string contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void EndAppendAllText(string path, string contents, Encoding encoding)
        {
        }

        /// <inheritdoc />
        public override void BeginAppendText(string path)
        {
        }

        /// <inheritdoc />
        public override void EndAppendText(StreamWriter result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailAppendText(ref Exception exception)
        {
            base.FailAppendText(ref exception);

            this.log.Error("Exception occurred while appending text.", exception);
        }

        /// <inheritdoc />
        public override void BeginCreate(string path)
        {
        }

        /// <inheritdoc />
        public override void EndCreate(FileStream result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailCreate(ref Exception exception)
        {
            base.FailCreate(ref exception);

            this.log.Error("Exception occurred while creating a file or stream.", exception);
        }

        /// <inheritdoc />
        public override void BeginCreate(string path, int bufferSize)
        {
        }

        /// <inheritdoc />
        public override void EndCreate(FileStream result, string path, int bufferSize)
        {
        }

        /// <inheritdoc />
        public override void BeginCreate(string path, int bufferSize, FileOptions options)
        {
        }

        /// <inheritdoc />
        public override void EndCreate(FileStream result, string path, int bufferSize, FileOptions options)
        {
        }

        /// <inheritdoc />
        public override void BeginCreate(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
        }

        /// <inheritdoc />
        public override void EndCreate(FileStream result, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
        }

        /// <inheritdoc />
        public override void BeginDecrypt(string path)
        {
        }

        /// <inheritdoc />
        public override void EndDecrypt(string path)
        {
        }

        /// <inheritdoc />
        public override void FailDecrypt(ref Exception exception)
        {
            base.FailDecrypt(ref exception);

            this.log.Error("Exception occurred while decrypting a file.", exception);
        }

        /// <inheritdoc />
        public override void BeginEncrypt(string path)
        {
        }

        /// <inheritdoc />
        public override void EndEncrypt(string path)
        {
        }

        /// <inheritdoc />
        public override void FailEncrypt(ref Exception exception)
        {
            base.FailEncrypt(ref exception);

            this.log.Error("Exception occurred while encrypting a file.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetAccessControl(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetAccessControl(FileSecurity result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetAccessControl(ref Exception exception)
        {
            base.FailGetAccessControl(ref exception);

            this.log.Error("Exception occurred while getting access control.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetAccessControl(string path, AccessControlSections includeSections)
        {
        }

        /// <inheritdoc />
        public override void EndGetAccessControl(FileSecurity result, string path, AccessControlSections includeSections)
        {
        }

        /// <inheritdoc />
        public override void BeginGetCreationTime(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetCreationTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetCreationTime(ref Exception exception)
        {
            base.FailGetCreationTime(ref exception);

            this.log.Error("Exception occurred while getting creation time.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetCreationTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetCreationTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetCreationTimeUtc(ref Exception exception)
        {
            base.FailGetCreationTimeUtc(ref exception);

            this.log.Error("Exception occurred while getting creation time in UTC.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetLastAccessTime(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetLastAccessTime(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetLastAccessTime(ref Exception exception)
        {
            base.FailGetLastAccessTime(ref exception);

            this.log.Error("Exception occurred while getting last access time.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetLastAccessTimeUtc(string path)
        {
        }

        /// <inheritdoc />
        public override void EndGetLastAccessTimeUtc(DateTime result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetLastAccessTimeUtc(ref Exception exception)
        {
            base.FailGetLastAccessTime(ref exception);

            this.log.Error("Exception occurred while getting last access time in UTC.", exception);
        }

        /// <inheritdoc />
        public override void BeginReadAllText(string path)
        {
        }

        /// <inheritdoc />
        public override void EndReadAllText(string result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailCopy(ref Exception exception)
        {
            base.FailCopy(ref exception);

            this.log.Error("Exception occured while copying!", exception);
        }
    }
}
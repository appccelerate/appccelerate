//-------------------------------------------------------------------------------
// <copyright file="File.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Text;

    /// <summary>
    /// Wrapper class which simplifies the access to the file layer.
    /// </summary>
    public class File : IFile, IExtensionProvider<IFileExtension>
    {
        private readonly List<IFileExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public File(IEnumerable<IFileExtension> extensions)
        {
            this.extensions = extensions.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<IFileExtension> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <inheritdoc />
        public void Delete(string path)
        {
            this.SurroundWithExtension(() => System.IO.File.Delete(path), path);
        }

        /// <inheritdoc />
        public void Copy(string sourceFileName, string destFileName)
        {
            this.SurroundWithExtension(() => System.IO.File.Copy(sourceFileName, destFileName), sourceFileName, destFileName);
        }

        /// <inheritdoc />
        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            this.SurroundWithExtension(() => System.IO.File.Copy(sourceFileName, destFileName, overwrite), sourceFileName, destFileName, overwrite);
        }

        /// <inheritdoc />
        public StreamWriter CreateText(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.CreateText(path), path);
        }

        /// <inheritdoc />
        public FileAttributes GetAttributes(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.GetAttributes(path), path);
        }

        /// <inheritdoc />
        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            this.SurroundWithExtension(() => System.IO.File.SetLastWriteTime(path, lastWriteTime), path, lastWriteTime);
        }

        /// <inheritdoc />
        public void SetAttributes(string path, FileAttributes fileAttributes)
        {
            this.SurroundWithExtension(() => System.IO.File.SetAttributes(path, fileAttributes), path, fileAttributes);
        }

        /// <inheritdoc />
        public bool Exists(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.Exists(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<byte> ReadAllBytes(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.ReadAllBytes(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> ReadAllLines(string path, Encoding encoding)
        {
            return this.SurroundWithExtension(() => System.IO.File.ReadAllLines(path, encoding), path, encoding);
        }

        /// <inheritdoc />
        public IEnumerable<string> ReadAllLines(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.ReadAllLines(path), path);
        }

        /// <inheritdoc />
        public string ReadAllText(string path, Encoding encoding)
        {
            return this.SurroundWithExtension(() => System.IO.File.ReadAllText(path, encoding), path, encoding);
        }

        /// <inheritdoc />
        public string ReadAllText(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.ReadAllText(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> ReadLines(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.ReadLines(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return this.SurroundWithExtension(() => System.IO.File.ReadLines(path, encoding), path, encoding);
        }

        /// <inheritdoc />
        public void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            this.SurroundWithExtension(() => System.IO.File.WriteAllLines(path, contents, encoding), path, contents, encoding);
        }

        /// <inheritdoc />
        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            this.SurroundWithExtension(() => System.IO.File.WriteAllLines(path, contents), path, contents);
        }

        /// <inheritdoc />
        public void WriteAllText(string path, string contents)
        {
            this.SurroundWithExtension(() => System.IO.File.WriteAllText(path, contents), path, contents);
        }

        /// <inheritdoc />
        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            this.SurroundWithExtension(() => System.IO.File.WriteAllText(path, contents, encoding), path, contents, encoding);
        }

        /// <inheritdoc />
        public void WriteAllBytes(string path, IEnumerable<byte> bytes)
        {
            byte[] bytesAsArray = bytes.ToArray();

            this.SurroundWithExtension(() => System.IO.File.WriteAllBytes(path, bytesAsArray), path, bytesAsArray);
        }

        /// <inheritdoc />
        public Stream Open(string path, FileMode mode)
        {
            return this.SurroundWithExtension(() => System.IO.File.Open(path, mode), path, mode);
        }

        /// <inheritdoc />
        public Stream Open(string path, FileMode mode, System.IO.FileAccess access)
        {
            return this.SurroundWithExtension(() => System.IO.File.Open(path, mode, access), path, mode, access);
        }

        /// <inheritdoc />
        public Stream Open(string path, FileMode mode, System.IO.FileAccess access, FileShare share)
        {
            return this.SurroundWithExtension(() => System.IO.File.Open(path, mode, access, share), path, mode, access, share);
        }

        /// <inheritdoc />
        public void AppendAllLines(string path, IEnumerable<string> contents)
        {
            this.SurroundWithExtension(() => System.IO.File.AppendAllLines(path, contents), path, contents);
        }

        /// <inheritdoc />
        public void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            this.SurroundWithExtension(() => System.IO.File.AppendAllLines(path, contents, encoding), path, contents, encoding);
        }

        /// <inheritdoc />
        public void AppendAllText(string path, string contents)
        {
            this.SurroundWithExtension(() => System.IO.File.AppendAllText(path, contents), path, contents);
        }

        /// <inheritdoc />
        public void AppendAllText(string path, string contents, Encoding encoding)
        {
            this.SurroundWithExtension(() => System.IO.File.AppendAllText(path, contents, encoding), path, contents, encoding);
        }

        /// <inheritdoc />
        public StreamWriter AppendText(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.AppendText(path), path);
        }

        /// <inheritdoc />
        public Stream Create(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.Create(path), path);
        }

        /// <inheritdoc />
        public Stream Create(string path, int bufferSize)
        {
            return this.SurroundWithExtension(() => System.IO.File.Create(path, bufferSize), path, bufferSize);
        }

        /// <inheritdoc />
        public Stream Create(string path, int bufferSize, FileOptions options)
        {
            return this.SurroundWithExtension(() => System.IO.File.Create(path, bufferSize, options), path, bufferSize, options);
        }

        /// <inheritdoc />
        public Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            return this.SurroundWithExtension(() => System.IO.File.Create(path, bufferSize, options, fileSecurity), path, bufferSize, options, fileSecurity);
        }

        /// <inheritdoc />
        public void Decrypt(string path)
        {
            this.SurroundWithExtension(() => System.IO.File.Decrypt(path), path);
        }

        /// <inheritdoc />
        public void Encrypt(string path)
        {
            this.SurroundWithExtension(() => System.IO.File.Encrypt(path), path);
        }

        /// <inheritdoc />
        public FileSecurity GetAccessControl(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.GetAccessControl(path), path);
        }

        /// <inheritdoc />
        public FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return this.SurroundWithExtension(() => System.IO.File.GetAccessControl(path, includeSections), path, includeSections);
        }

        /// <inheritdoc />
        public DateTime GetCreationTime(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.GetCreationTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetCreationTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.GetCreationTimeUtc(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastAccessTime(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.GetLastAccessTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastAccessTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.GetLastAccessTimeUtc(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastWriteTime(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.GetLastWriteTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastWriteTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.GetLastWriteTimeUtc(path), path);
        }

        /// <inheritdoc />
        public void Move(string sourceFileName, string destFileName)
        {
            this.SurroundWithExtension(() => System.IO.File.Move(sourceFileName, destFileName), sourceFileName, destFileName);
        }

        /// <inheritdoc />
        public Stream OpenRead(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.OpenRead(path), path);
        }

        /// <inheritdoc />
        public StreamReader OpenText(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.OpenText(path), path);
        }

        /// <inheritdoc />
        public Stream OpenWrite(string path)
        {
            return this.SurroundWithExtension(() => System.IO.File.OpenWrite(path), path);
        }

        /// <inheritdoc />
        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            this.SurroundWithExtension(() => System.IO.File.Replace(sourceFileName, destinationFileName, destinationBackupFileName), sourceFileName, destinationFileName, destinationBackupFileName);
        }

        /// <inheritdoc />
        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            this.SurroundWithExtension(() => System.IO.File.Replace(sourceFileName, destinationFileName, destinationBackupFileName), sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
        }

        /// <inheritdoc />
        public void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            this.SurroundWithExtension(() => System.IO.File.SetAccessControl(path, fileSecurity), path, fileSecurity);
        }

        /// <inheritdoc />
        public void SetCreationTime(string path, DateTime creationTime)
        {
            this.SurroundWithExtension(() => System.IO.File.SetCreationTime(path, creationTime), path, creationTime);
        }

        /// <inheritdoc />
        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            this.SurroundWithExtension(() => System.IO.File.SetCreationTimeUtc(path, creationTimeUtc), path, creationTimeUtc);
        }

        /// <inheritdoc />
        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            this.SurroundWithExtension(() => System.IO.File.SetLastAccessTime(path, lastAccessTime), path, lastAccessTime);
        }

        /// <inheritdoc />
        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            this.SurroundWithExtension(() => System.IO.File.SetLastAccessTimeUtc(path, lastAccessTimeUtc), path, lastAccessTimeUtc);
        }

        /// <inheritdoc />
        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            this.SurroundWithExtension(() => System.IO.File.SetLastWriteTimeUtc(path, lastWriteTimeUtc), path, lastWriteTimeUtc);
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="TemporaryFileHolder.cs" company="Appccelerate">
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

namespace Appccelerate.IO
{
    using System;
    using System.IO;

    /// <summary>
    /// Helper class to create (and destroy) a temporary file for UnitTests that depend on file system operations
    /// </summary>
    public class TemporaryFileHolder : IDisposable
    {
        /// <summary>
        /// path of the file.
        /// </summary>
        private readonly string filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryFileHolder"/> class.
        /// </summary>
        /// <param name="filePath">The path were the file should be created.</param>
        /// <param name="fileContent">Content of the file.</param>
        public TemporaryFileHolder(string filePath, Stream fileContent)
        {
            Ensure.ArgumentNotNull(fileContent, "fileContent");

            this.filePath = filePath;
            using (FileStream fileStream = File.Create(filePath))
            {
                fileContent.CopyTo(fileStream);
                fileStream.Flush();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryFileHolder"/> class.
        /// </summary>
        /// <param name="filePath">The path were the file should be created.</param>
        /// <param name="fileContent">Content of the file.</param>
        public TemporaryFileHolder(string filePath, string fileContent)
        {
            this.filePath = filePath;
            using (StreamWriter fileStream = File.CreateText(filePath))
            {
                fileStream.Write(fileContent);
                fileStream.Flush();
            }
        }

        /// <summary>
        /// Gets the file info.
        /// </summary>
        /// <value>The file info.</value>
        public FileInfo FileInfo
        {
            get { return new FileInfo(this.filePath); }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
            Justification = "should not throw exception to caller.")]
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                File.Delete(this.filePath);
            }
            catch
            {
                // No chance to delete the file
            }
        }
    }
}
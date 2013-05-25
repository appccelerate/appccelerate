//-------------------------------------------------------------------------------
// <copyright file="PathLogExtension.cs" company="Appccelerate">
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
    using System.Globalization;
    using System.Reflection;

    using Appccelerate.IO.Access;

    using log4net;

    /// <summary>
    /// Path access extension which logs actions with log4net.
    /// </summary>
    public class PathLogExtension : PathExtensionBase
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathLogExtension"/> class
        /// with standard logger name (this class's name).
        /// </summary>
        public PathLogExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PathLogExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PathLogExtension(ILog logger)
        {
            this.log = logger;
        }

        /// <inheritdoc />
        public override void BeginGetDirectoryName(string path)
        {
            base.BeginGetDirectoryName(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting directory name of {0}.", path);
        }

        /// <inheritdoc />
        public override void EndGetDirectoryName(string result, string path)
        {
            base.EndGetDirectoryName(result, path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Got directory name {0} of {1}.", result, path);
        }

        /// <inheritdoc />
        public override void FailGetDirectoryName(ref Exception exception)
        {
            base.FailGetDirectoryName(ref exception);

            this.log.Error("Exception occured while getting directory name!", exception);
        }

        /// <inheritdoc />
        public override void BeginGetFileName(string path)
        {
            base.BeginGetFileName(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting file name of {0}.", path);
        }

        /// <inheritdoc />
        public override void EndGetFileName(string result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetFileName(ref Exception exception)
        {
        }

        /// <inheritdoc />
        public override void BeginGetFileNameWithoutExtension(string path)
        {
            base.BeginGetFileNameWithoutExtension(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting file name without extension of {0}.", path);
        }

        /// <inheritdoc />
        public override void EndGetFileNameWithoutExtension(string result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetFileNameWithoutExtension(ref Exception exception)
        {
            base.FailGetFileNameWithoutExtension(ref exception);

            this.log.Error("Exception occurred while getting a file name without extension.", exception);
        }

        /// <inheritdoc />
        public override void BeginCombine(string path1, string path2)
        {
            base.BeginCombine(path1, path2);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Combinding {0} with {1}.", path1, path2);
        }

        /// <inheritdoc />
        public override void EndCombine(string result, string path1, string path2)
        {
        }

        /// <inheritdoc />
        public override void FailCombine(ref Exception exception)
        {
            base.FailCombine(ref exception);

            this.log.Error("Exception occurred while combinding paths.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetRandomFileName()
        {
            base.BeginGetRandomFileName();

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting random file name.");
        }

        /// <inheritdoc />
        public override void EndGetRandomFileName(string result)
        {
        }

        /// <inheritdoc />
        public override void FailGetRandomFileName(ref Exception exception)
        {
            base.FailGetRandomFileName(ref exception);

            this.log.Error("Exception occurred while getting a random file name.", exception);
        }

        /// <inheritdoc />
        public override void BeginChangeExtension(string path, string extension)
        {
            base.BeginChangeExtension(path, extension);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Changing extension of {0} to {1}.", path, extension);
        }

        /// <inheritdoc />
        public override void EndChangeExtension(string result, string path, string extension)
        {
        }

        /// <inheritdoc />
        public override void FailChangeExtension(ref Exception exception)
        {
            base.FailChangeExtension(ref exception);

            this.log.Error("Exception occurred while changing a file extension.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetExtension(string path)
        {
            base.BeginGetExtension(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting extension of {0}.", path);
        }

        /// <inheritdoc />
        public override void EndGetExtension(string result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetExtension(ref Exception exception)
        {
            base.FailGetExtension(ref exception);

            this.log.Error("Exception occurred while getting a file extension.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetFullPath(string path)
        {
            base.BeginGetFullPath(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting full path of {0}.", path);
        }

        /// <inheritdoc />
        public override void EndGetFullPath(string result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetFullPath(ref Exception exception)
        {
            base.FailGetFullPath(ref exception);

            this.log.Error("Exception occurred while getting the full path.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetInvalidFileNameChars()
        {
            base.BeginGetInvalidFileNameChars();

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting invalid file name characters.");
        }

        /// <inheritdoc />
        public override void EndGetInvalidFileNameChars(char[] result)
        {
        }

        /// <inheritdoc />
        public override void FailGetInvalidFileNameChars(ref Exception exception)
        {
            base.FailGetInvalidFileNameChars(ref exception);

            this.log.Error("Exception occurred while getting invalid file name chars.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetInvalidPathChars()
        {
            base.BeginGetInvalidPathChars();

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting invalid path characters.");
        }

        /// <inheritdoc />
        public override void EndGetInvalidPathChars(char[] result)
        {
        }

        /// <inheritdoc />
        public override void FailGetInvalidPathChars(ref Exception exception)
        {
            base.FailGetInvalidPathChars(ref exception);

            this.log.Error("Exception occurred while getting invalid path name chars.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetPathRoot(string path)
        {
            base.BeginGetPathRoot(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting path root of {0}.", path);
        }

        /// <inheritdoc />
        public override void EndGetPathRoot(string result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailGetPathRoot(ref Exception exception)
        {
            base.FailGetPathRoot(ref exception);

            this.log.Error("Exception occurred while getting path root.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetTempFileName()
        {
            base.BeginGetTempFileName();

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting temp file name.");
        }

        /// <inheritdoc />
        public override void EndGetTempFileName(string result)
        {
        }

        /// <inheritdoc />
        public override void FailGetTempFileName(ref Exception exception)
        {
            base.FailGetTempFileName(ref exception);

            this.log.Error("Exception occurred while getting temp file name.", exception);
        }

        /// <inheritdoc />
        public override void BeginGetTempPath()
        {
            base.BeginGetTempPath();

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting temp path.");
        }

        /// <inheritdoc />
        public override void EndGetTempPath(string result)
        {
        }

        /// <inheritdoc />
        public override void FailGetTempPath(ref Exception exception)
        {
            base.FailGetTempPath(ref exception);

            this.log.Error("Exception occurred while getting temp path.", exception);
        }

        /// <inheritdoc />
        public override void BeginHasExtension(string path)
        {
            base.BeginHasExtension(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Verifying if {0} has an extension.", path);
        }

        /// <inheritdoc />
        public override void EndHasExtension(bool result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailHasExtension(ref Exception exception)
        {
            base.FailHasExtension(ref exception);

            this.log.Error("Exception occurred while verifying if the path has an extension.", exception);
        }

        /// <inheritdoc />
        public override void BeginIsPathRooted(string path)
        {
            base.BeginIsPathRooted(path);

            this.log.DebugFormat(CultureInfo.InvariantCulture, "Verifying if {0} is a rooted path.", path);
        }

        /// <inheritdoc />
        public override void EndIsPathRooted(bool result, string path)
        {
        }

        /// <inheritdoc />
        public override void FailIsPathRooted(ref Exception exception)
        {
            base.FailIsPathRooted(ref exception);

            this.log.Error("Exception occurred while verifying if the path is rooted.", exception);
        }
    }
}
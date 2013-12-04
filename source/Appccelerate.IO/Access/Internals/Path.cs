//-------------------------------------------------------------------------------
// <copyright file="Path.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Wrapper class which simplifies access to paths.
    /// </summary>
    public class Path : IPath, IExtensionProvider<IPathExtension>
    {
        private readonly List<IPathExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Path"/> class.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public Path(IEnumerable<IPathExtension> extensions)
        {
            this.extensions = extensions.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<IPathExtension> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <inheritdoc />
        public string GetDirectoryName(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetDirectoryName(path), path);
        }

        /// <inheritdoc />
        public string GetFileName(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetFileName(path), path);
        }

        /// <inheritdoc />
        public string GetFileNameWithoutExtension(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetFileNameWithoutExtension(path), path);
        }

        /// <inheritdoc />
        public string Combine(string path1, string path2)
        {
            return this.SurroundWithExtension(() => System.IO.Path.Combine(path1, path2), path1, path2);
        }

        /// <inheritdoc />
        public string GetRandomFileName()
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetRandomFileName());
        }

        /// <inheritdoc />
        public string ChangeExtension(string path, string extension)
        {
            return this.SurroundWithExtension(() => System.IO.Path.ChangeExtension(path, extension), path, extension);
        }

        /// <inheritdoc />
        public string GetExtension(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetExtension(path), path);
        }

        /// <inheritdoc />
        public string GetFullPath(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetFullPath(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<char> GetInvalidFileNameChars()
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetInvalidFileNameChars());
        }

        /// <inheritdoc />
        public IEnumerable<char> GetInvalidPathChars()
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetInvalidPathChars());
        }

        /// <inheritdoc />
        public string GetPathRoot(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetPathRoot(path), path);
        }

        /// <inheritdoc />
        public string GetTempFileName()
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetTempFileName());
        }

        /// <inheritdoc />
        public string GetTempPath()
        {
            return this.SurroundWithExtension(() => System.IO.Path.GetTempPath());
        }

        /// <inheritdoc />
        public bool HasExtension(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Path.HasExtension(path), path);
        }

        /// <inheritdoc />
        public bool IsPathRooted(string path)
        {
            return this.SurroundWithExtension(() => System.IO.Path.IsPathRooted(path), path);
        }
    }
}
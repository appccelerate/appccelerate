//-------------------------------------------------------------------------------
// <copyright file="IPath.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Abstraction layer which simplifies access to paths.
    /// </summary>
    [CompilerGenerated]
    public interface IPath
    {
        string GetDirectoryName(string path);

        string GetFileName(string path);

        string GetFileNameWithoutExtension(string path);

        string Combine(string path1, string path2);

        string GetRandomFileName();

        string ChangeExtension(string path, string extension);

        string GetExtension(string path);

        string GetFullPath(string path);

        IEnumerable<char> GetInvalidFileNameChars();

        IEnumerable<char> GetInvalidPathChars();

        string GetPathRoot(string path);

        string GetTempFileName();

        string GetTempPath();

        bool HasExtension(string path);

        bool IsPathRooted(string path);
    }
}
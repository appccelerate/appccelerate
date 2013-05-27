//-------------------------------------------------------------------------------
// <copyright file="IDirectoryInfo.cs" company="Appccelerate">
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
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Interface which simplifies the access to the directory info.
    /// </summary>
    [CompilerGenerated]
    public interface IDirectoryInfo : IFileSystemInfo
    {
        /// <include file='IDirectoryInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DirectoryInfo.Parent"]' />
        IDirectoryInfo Parent { get; }

        /// <include file='IDirectoryInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DirectoryInfo.Root"]' />
        IDirectoryInfo Root { get; }

        /// <include file='IDirectoryInfo.doc.xml' path='doc/members/member[@name="M:System.IO.DirectoryInfo.Create"]' />
        void Create();
    }
}
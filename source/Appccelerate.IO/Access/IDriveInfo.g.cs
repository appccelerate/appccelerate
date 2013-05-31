//-------------------------------------------------------------------------------
// <copyright file="IDriveInfo.cs" company="Appccelerate">
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
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    /// <summary>
    /// Interface which simplifies the access to directory info.
    /// </summary>
    [CompilerGenerated]
    public interface IDriveInfo : ISerializable
    {
        /// <include file='IDriveInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DriveInfo.AvailableFreeSpace"]' />
        long AvailableFreeSpace { get; }

        /// <include file='IDriveInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DriveInfo.DriveFormat"]' />
        string DriveFormat { get; }

        /// <include file='IDriveInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DriveInfo.DriveType"]' />
        DriveType DriveType { get; }

        /// <include file='IDriveInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DriveInfo.IsReady"]' />
        bool IsReady { get; }

        /// <include file='IDriveInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DriveInfo.RootDirectory"]' />
        IDirectoryInfo RootDirectory { get; }

        /// <include file='IDriveInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DriveInfo.TotalFreeSpace"]' />
        long TotalFreeSpace { get; }

        /// <include file='IDriveInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DriveInfo.TotalSize"]' />
        long TotalSize { get; }

        /// <include file='IDriveInfo.doc.xml' path='doc/members/member[@name="P:System.IO.DriveInfo.Name"]' />
        string Name { get; }
    }
}
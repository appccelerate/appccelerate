//-------------------------------------------------------------------------------
// <copyright file="Drive.cs" company="Appccelerate">
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

    public class Drive : IDrive, IExtensionProvider<IDriveExtension>
    {
        private readonly List<IDriveExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Drive"/> class.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public Drive(IEnumerable<IDriveExtension> extensions)
        {
            this.extensions = extensions.ToList();
        }

        public IEnumerable<IDriveExtension> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        public IEnumerable<IDriveInfo> GetDrives()
        {
            return this.SurroundWithExtension(() => System.IO.DriveInfo.GetDrives()).Select(info => new DriveInfo(info));
        }
    }
}
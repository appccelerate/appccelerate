//-------------------------------------------------------------------------------
// <copyright file="DriveLogExtension.cs" company="Appccelerate">
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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Appccelerate.IO.Access;

    using log4net;

    /// <summary>
    /// Drive access extension which logs actions with log4net.
    /// </summary>
    public class DriveLogExtension : DriveExtensionBase
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveLogExtension"/> class.
        /// </summary>
        public DriveLogExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DriveLogExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DriveLogExtension(ILog logger)
        {
            this.log = logger;
        }

        public override void BeginGetDrives()
        {
            this.log.DebugFormat(CultureInfo.InvariantCulture, "Getting all drives.");
        }

        public override void EndGetDrives(DriveInfo[] result)
        {
            this.log.DebugFormat(
                CultureInfo.InvariantCulture, "Got all drives {0}.", string.Join(";", result.Select(info => info.Name)));
        }

        public override void FailGetDrives(ref System.Exception exception)
        {
            this.log.Error("Error occurred while getting all drives.", exception);
        }
    }
}
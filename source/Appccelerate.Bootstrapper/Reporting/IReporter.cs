//-------------------------------------------------------------------------------
// <copyright file="IReporter.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Reporting
{
    /// <summary>
    /// Marks the implementing class as reporter of the bootstrapping process.
    /// </summary>
    public interface IReporter
    {
        /// <summary>
        /// Reports the whole bootstrapping process.
        /// </summary>
        /// <remarks>The reporter is called right before IBootstrapper disposal.</remarks>
        /// <param name="context">The reporting context containing detailed information about the reporting process.</param>
        void Report(IReportingContext context);
    }
}
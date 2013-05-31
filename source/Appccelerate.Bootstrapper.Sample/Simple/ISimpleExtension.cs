//-------------------------------------------------------------------------------
// <copyright file="ISimpleExtension.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Simple
{
    /// <summary>
    /// Interface for all simple extensions
    /// </summary>
    public interface ISimpleExtension : IExtension
    {
        /// <summary>
        /// Called when the system is starting
        /// </summary>
        void Start();

        /// <summary>
        /// Called when the system is shutting down.
        /// </summary>
        void Shutdown();
    }
}
//-------------------------------------------------------------------------------
// <copyright file="ThirdSimpleExtension.cs" company="Appccelerate">
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
    using System;

    /// <summary>
    /// Third simple extension.
    /// </summary>
    public class ThirdSimpleExtension : SimpleExtensionBase, IDisposable
    {
        private bool disposed;

        /// <inheritdoc />
        public override void Start()
        {
            base.Start();

            Console.WriteLine("Third Simple Extension is starting.");
        }

        /// <inheritdoc />
        public override void Shutdown()
        {
            base.Shutdown();

            Console.WriteLine("Third Simple Extension is shutting down.");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public override string Describe()
        {
            return "Third simple extension";
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                Console.WriteLine("Third Simple Extension is disposing.");

                this.disposed = true;
            }
        }
    }
}
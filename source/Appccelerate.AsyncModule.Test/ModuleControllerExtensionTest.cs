//-------------------------------------------------------------------------------
// <copyright file="ModuleControllerExtensionTest.cs" company="Appccelerate">
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

namespace Appccelerate.AsyncModule
{
    using Appccelerate.AsyncModule.Extensions;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class ModuleControllerExtensionTest
    {
        private ModuleController testee;

        private TestModule module;

        public ModuleControllerExtensionTest()
        {
            this.module = new TestModule();

            this.testee = new ModuleController();
            this.testee.Initialize(this.module);
        }

        public interface IExtension : IModuleExtension
        {
        }

        [Fact]
        public void ExtensionsCanBeAttachedAndQueried()
        {
            IExtension extension = A.Fake<IExtension>();

            this.testee.AddExtension(extension);

            ////extension.ModuleController.Should().BeSameAs(this.testee);
            A.CallTo(() => extension.Attach()).MustHaveHappened();

            this.testee.GetExtension<IExtension>().Should().BeSameAs(extension);
        }
    }
}
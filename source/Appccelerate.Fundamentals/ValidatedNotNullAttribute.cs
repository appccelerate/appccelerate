//-------------------------------------------------------------------------------
// <copyright file="ValidatedNotNullAttribute.cs" company="Appccelerate">
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

namespace Appccelerate
{
    using System;

    /// <summary>
    /// The attribute is used to decorate an argument in a argument validating method that it is being
    /// validated against <c>null</c> in the validating method. The attribute is used within the <see cref="Ensure"/> 
    /// methods and should not be used elsewhere.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
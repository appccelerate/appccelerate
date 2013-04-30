﻿//-------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Appccelerate">
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

namespace Appccelerate.Formatters
{
    using System;
    using System.Linq;

    /// <summary>
    /// Contains extension methods for Type.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Correctly formats the FullName of the specified type by taking generics into consideration.
        /// </summary>
        /// <param name="type">The type whose full name is formatted.</param>
        /// <returns>A correctly formatted full name.</returns>
        public static string FullNameToString(this Type type)
        {
            Ensure.ArgumentNotNull(type, "type");

            if (!type.IsGenericType)
            {
                return type.FullName;
            }

            var partName = type.FullName.Substring(0, type.FullName.IndexOf('`'));
            var genericArgumentNames = type.GetGenericArguments().Select(arg => arg.FullNameToString());
            return string.Concat(partName, "<", string.Join(",", genericArgumentNames), ">");
        }
    }
}
//-------------------------------------------------------------------------------
// <copyright file="AbsoluteFolderPath.cs" company="Appccelerate">
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
namespace Appccelerate.IO
{
    public class AbsoluteFolderPath : AbsolutePath
    {
        public AbsoluteFolderPath(string absoluteFolderPath)
            : base(absoluteFolderPath)
        {
        }

        public static implicit operator AbsoluteFolderPath(string absoluteFolderPath)
        {
            return new AbsoluteFolderPath(absoluteFolderPath);
        }

        public static AbsoluteFilePath FromString(string absoluteFolderPath)
        {
            return new AbsoluteFilePath(absoluteFolderPath);
        }
    }
}
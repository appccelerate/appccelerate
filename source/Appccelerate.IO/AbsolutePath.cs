//-------------------------------------------------------------------------------
// <copyright file="AbsolutePath.cs" company="Appccelerate">
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
    using System;
    using System.IO;

    public class AbsolutePath
    {
        public AbsolutePath(string absolutePath)
        {
            if (!Path.IsPathRooted(absolutePath))
            {
                throw new ArgumentException("Expected absolute path but is `" + absolutePath + "`.");
            }

            this.Value = absolutePath;
        }

        public string Value { get; private set; }

        public AbsoluteFolderPath AsAbsoluteFolderPath
        {
            get
            {
                return new AbsoluteFolderPath(this.Value);
            }
        }

        public AbsoluteFilePath AsAbsoluteFilePath
        {
            get
            {
                return new AbsoluteFilePath(this.Value);
            }
        }

        public static implicit operator AbsolutePath(string absolutePath)
        {
            return new AbsolutePath(absolutePath);
        }

        public static implicit operator string(AbsolutePath absolutePath)
        {
            Ensure.ArgumentNotNull(absolutePath, "absolutePath");

            return absolutePath.Value;
        }

        public static bool operator ==(AbsolutePath a, AbsolutePath b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

// ReSharper disable RedundantCast.0 because otherwise it results in recursion.
            if (((object)a == null) || ((object)b == null))
// ReSharper restore RedundantCast.0
            {
                return false;
            }

            return a.Value == b.Value;
        }

        public static bool operator !=(AbsolutePath a, AbsolutePath b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((AbsolutePath)obj);
        }

        public override int GetHashCode()
        {
            return this.Value != null ? this.Value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return this.Value;
        }

        protected bool Equals(AbsolutePath other)
        {
            Ensure.ArgumentNotNull(other, "other");

            return string.Equals(this.Value, other.Value);
        }
    }
}
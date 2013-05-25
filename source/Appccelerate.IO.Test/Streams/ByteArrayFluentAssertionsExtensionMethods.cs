//-------------------------------------------------------------------------------
// <copyright file="ByteArrayFluentAssertionsExtensionMethods.cs" company="Appccelerate">
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

namespace Appccelerate.IO.Streams
{
    using FluentAssertions;
    using FluentAssertions.Collections;
    using FluentAssertions.Execution;

    public static class ByteArrayFluentAssertionsExtensionMethods
    {
        public static void Be(
            this GenericCollectionAssertions<byte> assertions,
            byte[] expected,
            int startIndex,
            int length)
        {
            var actual = assertions.Subject.As<byte[]>();

            bool mismatch = false;

            mismatch |= expected == null && actual != null;
            mismatch |= expected != null && actual == null;
            mismatch |= actual.Length != length;

            for (int i = 0; i < length && !mismatch; i++)
            {
                mismatch |= expected[i + startIndex] != actual[i];
            }

            Execute.Verification.ForCondition(!mismatch).FailWith(
                "byte arrays mismatch expected {0} from index {1} with length {2} but found {3}",
                expected,
                startIndex,
                length,
                actual);
        }
    }
}
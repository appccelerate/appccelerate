//-------------------------------------------------------------------------------
// <copyright file="Matcher.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker
{
    using System;
    using System.IO;

    using Appccelerate.EventBroker.Internals;
    using Appccelerate.EventBroker.Matchers;

    public class Matcher : IMatcher
    {
        private readonly bool match;

        public Matcher(bool match)
        {
            this.match = match;
        }

        public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
        {
            return this.match;
        }

        public void DescribeTo(TextWriter writer)
        {
        }
    }
}
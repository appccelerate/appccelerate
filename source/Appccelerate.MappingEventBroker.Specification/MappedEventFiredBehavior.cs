//-------------------------------------------------------------------------------
// <copyright file="MappedEventFiredBehavior.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker
{
    using Machine.Specifications;

    [Behaviors]
    public class MappedEventFiredBehavior
    {
        protected static Publisher source;

        protected static Subscriber destination;

        protected static string sourceEventDescription;

        It should_convert_from_source_to_destination = () =>
        {
            destination.MappedSubscriptionEventArgs.Destination.ShouldEqual(sourceEventDescription);
        };

        It should_fire_source = () =>
        {
            destination.SubscriptionEventArgs.ShouldNotBeNull();
        };

        It should_fire_destination = () =>
        {
            destination.MappedSubscriptionEventArgs.ShouldNotBeNull();
        };
    }
}
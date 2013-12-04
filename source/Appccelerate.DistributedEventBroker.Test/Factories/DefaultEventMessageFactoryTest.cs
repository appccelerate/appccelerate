//-------------------------------------------------------------------------------
// <copyright file="DefaultEventMessageFactoryTest.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.Factories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Appccelerate.DistributedEventBroker.Messages;

    using FluentAssertions;

    using Xunit;
    using Xunit.Extensions;

    public class DefaultEventMessageFactoryTest
    {
        private readonly DefaultEventMessageFactory testee;

        public DefaultEventMessageFactoryTest()
        {
            this.testee = new DefaultEventMessageFactory();
        }

        public static IEnumerable<object[]> InitializerInput
        {
            get
            {
                const string DistributedBrokerId = "DistributedEventBroker";
                yield return new object[] { DistributedBrokerId, Guid.NewGuid(), "topic://Simple", EventArgs.Empty };
                yield return new object[] { DistributedBrokerId, Guid.NewGuid(), "topic://Complex", new CancelEventArgs() };
            }
        }

        [Fact]
        public void CreatesEventFiredMessage()
        {
            var message = this.testee.CreateEventFiredMessage(x => { });

            message.Should().BeOfType<EventFired>();
        }

        [Theory]
        [PropertyData("InitializerInput")]
        public void AssignsPropertiesWithInitializerOnCreatedMessage(string distributedIdentification, Guid identification, string topic, EventArgs eventArgs)
        {
            var result = this.testee.CreateEventFiredMessage(x =>
                                                    {
                                                        x.DistributedEventBrokerIdentification =
                                                            distributedIdentification;
                                                        x.EventBrokerIdentification = identification.ToString();
                                                        x.Topic = topic;
                                                        x.EventArgs = eventArgs.ToString();
                                                    });

            result.DistributedEventBrokerIdentification.Should().Be(distributedIdentification);
            result.EventBrokerIdentification.Should().Be(identification.ToString());
            result.Topic.Should().Be(topic);
            result.EventArgs.Should().Be(eventArgs.ToString());
        }
    }
}
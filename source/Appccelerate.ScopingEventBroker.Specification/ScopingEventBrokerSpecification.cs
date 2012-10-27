//-------------------------------------------------------------------------------
// <copyright file="ScopingEventBrokerSpecification.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker.Specification
{
    using System;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Handlers;

    using Machine.Specifications;

    public class ScopingEventBrokerSpecification
    {
        protected static EventBroker eventBroker;

        protected static Func<IEventScopeFactory> scopeFactoryFactory;

        protected static IEventScopeContext scopeContext;

        protected static Publisher publisher;

        protected static Subscriber subscriber;

        Establish context = () =>
            {
                publisher = new Publisher();
                subscriber = new Subscriber();

                var defaultEventScopeFactory = scopeFactoryFactory();
                scopeContext = defaultEventScopeFactory.CreateScopeContext();

                var factory = new EventScopingStandardFactory(defaultEventScopeFactory);

                eventBroker = new EventBroker(factory);

                eventBroker.Register(publisher);
                eventBroker.Register(subscriber);
            };

        protected class Publisher
        {
            [EventPublication("topic://Event")]
            public event EventHandler Event = delegate { };

            public void Publish()
            {
                this.Event(this, EventArgs.Empty);
            }
        }

        protected class Subscriber
        {
            public bool OnBackgroundWasCalled { get; private set; }

            public bool OnPublisherWasCalled { get; private set; }

            [EventSubscription("topic://Event", typeof(OnBackground))]
            public void HandleAsyncOnBackground(object sender, EventArgs e)
            {
                this.OnBackgroundWasCalled = true;
            }

            [EventSubscription("topic://Event", typeof(OnPublisher))]
            public void HandleSyncOnPublisher(object sender, EventArgs e)
            {
                this.OnPublisherWasCalled = true;
            }
        } 
    }
}
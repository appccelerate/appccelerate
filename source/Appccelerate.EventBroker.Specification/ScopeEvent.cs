//-------------------------------------------------------------------------------
// <copyright file="ScopeEvent.cs" company="Appccelerate">
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

    using Appccelerate.EventBroker.Matchers.Scope;

    public class ScopeEvent
    {
        private const string EventTopic = "topic://scope";

        public class NamedPublisher : INamedItem
        {
            private readonly string name;

            public NamedPublisher(string name)
            {
                this.name = name;
            }

            [EventPublication(EventTopic)]
            public event EventHandler EventFiredGlobally;

            [EventPublication(EventTopic, typeof(PublishToParents))]
            public event EventHandler EventFiredToParentsAndSiblings;

            [EventPublication(EventTopic, typeof(PublishToChildren))]
            public event EventHandler EventFiredToChildrenAndSiblings;

            public string EventBrokerItemName
            {
                get
                {
                    return this.name;
                }
            }

            public void FireEventGlobally()
            {
                this.EventFiredGlobally(this, EventArgs.Empty);
            }

            public void FireEventToParentsAndSiblings()
            {
                this.EventFiredToParentsAndSiblings(this, EventArgs.Empty);
            }

            public void FireEventToChildrenAndSiblings()
            {
                this.EventFiredToChildrenAndSiblings(this, EventArgs.Empty);
            }
        }

        public class NamedSubscriber : INamedItem
        {
            private readonly string name;

            public NamedSubscriber(string name)
            {
                this.name = name;
            }

            public bool CalledGlobally { get; set; }

            public bool CalledFromParent { get; set; }

            public bool CalledFromChild { get; set; }

            public string EventBrokerItemName
            {
                get
                {
                    return this.name;
                }
            }

            [EventSubscription(EventTopic, typeof(Handlers.OnPublisher))]
            public void GlobalHandler(object sender, EventArgs e)
            {
                this.CalledGlobally = true;
            }

            [EventSubscription(EventTopic, typeof(Handlers.OnPublisher), typeof(SubscribeToParents))]
            public void ParentHandler(object sender, EventArgs e)
            {
                this.CalledFromParent = true;
            }

            [EventSubscription(EventTopic, typeof(Handlers.OnPublisher), typeof(SubscribeToChildren))]
            public void ChildrenHandler(object sender, EventArgs e)
            {
                this.CalledFromChild = true;
            }
        } 
    }
}
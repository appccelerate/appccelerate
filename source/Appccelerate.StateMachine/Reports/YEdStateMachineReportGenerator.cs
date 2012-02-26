//-------------------------------------------------------------------------------
// <copyright file="YEdStateMachineReportGenerator.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using Appccelerate.StateMachine.Machine;
    using Appccelerate.StateMachine.Machine.States;
    using Appccelerate.StateMachine.Machine.Transitions;

    /// <summary>
    /// generates a graphml file that can be read by yEd.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public class YEdStateMachineReportGenerator<TState, TEvent> : IStateMachineReport<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        private static readonly XNamespace n = "http://graphml.graphdrawing.org/xmlns";
        private static readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly XNamespace y = "http://www.yworks.com/xml/graphml";
        private static readonly XNamespace yed = "http://www.yworks.com/xml/yed/3";
        private static readonly XNamespace schemaLocation = "http://graphml.graphdrawing.org/xmlns http://www.yworks.com/xml/schema/graphml/1.1/ygraphml.xsd";

        private readonly TextWriter textWriter;

        private int edgeId;

        private Initializable<TState> initialStateId;

        /// <summary>
        /// Initializes a new instance of the <see cref="YEdStateMachineReportGenerator&lt;TState, TEvent&gt;"/> class.
        /// </summary>
        /// <param name="textWriter">The output writer.</param>
        public YEdStateMachineReportGenerator(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        /// <summary>
        /// Generates a report of the state machine.
        /// </summary>
        /// <param name="name">The name of the state machine.</param>
        /// <param name="states">The states.</param>
        /// <param name="initialStateId">The initial state id.</param>
        public void Report(string name, IEnumerable<IState<TState, TEvent>> states, Initializable<TState> initialStateId)
        {
            var statesList = states.ToList();

            this.edgeId = 0;

            this.initialStateId = initialStateId;
            Ensure.ArgumentNotNull(statesList, "states");

            XElement graph = CreateGraph();

            this.AddNodes(graph, statesList);
            this.AddEdges(graph, statesList);
            
            XDocument doc = CreateXmlDocument(graph);

            doc.Save(this.textWriter);
        }

        private static XElement CreateGraph()
        {
            return new XElement(n + "graph", new XAttribute("edgedefault", "directed"), new XAttribute("id", "G"));
        }

        private static XDocument CreateXmlDocument(XElement graph)
        {
            var doc = new XDocument(
                new XElement(
                    n + "graphml",
                    new XComment("Created by Appccelerate.StateMachine.YEdStateMachineReportGenerator"),
                    new XElement(n + "key", new XAttribute("for", "graphml"), new XAttribute("id", "d0"), new XAttribute("yfiles.type", "resources")),
                    new XElement(n + "key", new XAttribute("for", "port"), new XAttribute("id", "d1"), new XAttribute("yfiles.type", "portgraphics")),
                    new XElement(n + "key", new XAttribute("for", "port"), new XAttribute("id", "d2"), new XAttribute("yfiles.type", "portgeometry")),
                    new XElement(n + "key", new XAttribute("for", "port"), new XAttribute("id", "d3"), new XAttribute("yfiles.type", "portuserdata")),
                    new XElement(n + "key", new XAttribute("attr.name", "url"), new XAttribute("attr.type", "string"), new XAttribute("for", "node"), new XAttribute("id", "d4")),
                    new XElement(n + "key", new XAttribute("attr.name", "description"), new XAttribute("attr.type", "string"), new XAttribute("for", "node"), new XAttribute("id", "d5")),
                    new XElement(n + "key", new XAttribute("for", "node"), new XAttribute("id", "d6"), new XAttribute("yfiles.type", "nodegraphics")),
                    new XElement(n + "key", new XAttribute("attr.name", "Beschreibung"), new XAttribute("attr.type", "string"), new XAttribute("for", "graph"), new XAttribute("id", "d7"), new XElement(n + "default")),
                    new XElement(n + "key", new XAttribute("attr.name", "url"), new XAttribute("attr.type", "string"), new XAttribute("for", "edge"), new XAttribute("id", "d8")),
                    new XElement(n + "key", new XAttribute("attr.name", "description"), new XAttribute("attr.type", "string"), new XAttribute("for", "edge"), new XAttribute("id", "d9")),
                    new XElement(n + "key", new XAttribute("for", "edge"), new XAttribute("id", "d10"), new XAttribute("yfiles.type", "edgegraphics")),
                    graph,    
                    new XElement(n + "data", new XAttribute("key", "d0"), new XElement(y + "Resources"))));

            doc.Root.SetAttributeValue(XNamespace.Xmlns + "y", y);
            doc.Root.SetAttributeValue(XNamespace.Xmlns + "xsi", xsi);
            doc.Root.SetAttributeValue(XNamespace.Xmlns + "yed", yed);
            doc.Root.SetAttributeValue(XNamespace.Xmlns + "schemaLocation", schemaLocation);

            return doc;
        }

        private static string CreateExitActionsDescription(IState<TState, TEvent> state)
        {
            return state.ExitActions.Any()
                       ? (state.ExitActions.Aggregate(Environment.NewLine + "(", (aggregate, action) => (aggregate.Length > 3 ? aggregate + ", " : aggregate) + action.Describe()) + ")")
                       : string.Empty;
        }

        private static string CreateEntryActionDescription(IState<TState, TEvent> state)
        {
            return state.EntryActions.Any() 
                       ? (state.EntryActions.Aggregate("(", (aggregate, action) => (aggregate.Length > 1 ? aggregate + ", " : aggregate) + action.Describe()) + ")" + Environment.NewLine) 
                       : string.Empty;
        }

        private static string CreateGuardDescription(TransitionInfo<TState, TEvent> transition)
        {
            return transition.Guard != null ? "[" + transition.Guard.Describe() + "]" : string.Empty;
        }

        private static string CreateActionsDescription(TransitionInfo<TState, TEvent> transition)
        {
            return transition.Actions.Any() ? (transition.Actions.Aggregate("(", (aggregate, action) => (aggregate.Length > 1 ? aggregate + ", " : aggregate) + action.Describe()) + ")") : string.Empty;
        }

        private void AddEdges(XElement graph, IEnumerable<IState<TState, TEvent>> states)
        {
            foreach (var state in states)
            {
                foreach (var transition in state.Transitions.GetTransitions())
                {
                    this.AddEdge(graph, transition);
                }
            }
        }

        private void AddEdge(XElement graph, TransitionInfo<TState, TEvent> transition)
        {
            string actions = CreateActionsDescription(transition);
            string guard = CreateGuardDescription(transition);

            string arrow;
            string lineStyle;
            string targetId;
            if (transition.Target != null)
            {
                arrow = "standard";
                lineStyle = "line";
                targetId = transition.Target.Id.ToString();
            }
            else
            {
                arrow = "plain";
                lineStyle = "dashed";
                targetId = transition.Source.Id.ToString();
            }

            var edge = new XElement(
                n + "edge", 
                new XAttribute("id", transition.EventId + (this.edgeId++).ToString(CultureInfo.InvariantCulture)), 
                new XAttribute("source", transition.Source.Id), 
                new XAttribute("target", targetId));
 
            edge.Add(new XElement(
                n + "data", 
                new XAttribute("key", "d10"), 
                new XElement(y + "PolyLineEdge", new XElement(y + "LineStyle", new XAttribute("type", lineStyle)), new XElement(y + "Arrows", new XAttribute("source", "none"), new XAttribute("target", arrow)), new XElement(y + "EdgeLabel", guard + transition.EventId + actions))));
            
            graph.Add(edge);
        }

        private void AddNodes(XElement graph, IEnumerable<IState<TState, TEvent>> states)
        {
            foreach (var state in states.Where(s => s.SuperState == null))
            {
                this.AddNode(graph, state);
            }
        }

        private void AddNode(XElement graph, IState<TState, TEvent> state)
        {
            var node = new XElement(n + "node", new XAttribute("id", state.Id.ToString()));

            bool initialState = this.DetermineWhetherThisIsAnInitialState(state);

            string entryActions = CreateEntryActionDescription(state);
            string exitActions = CreateExitActionsDescription(state);
            
            if (state.SubStates.Any())
            {
                var label = new XElement(y + "NodeLabel", entryActions + state.Id + exitActions, new XAttribute("alignment", "right"), new XAttribute("autoSizePolicy", "node_width"), new XAttribute("backgroundColor", "#EBEBEB"), new XAttribute("modelName", "internal"), new XAttribute("modelPosition", "t"));

                var groupNode = new XElement(y + "GroupNode", label, new XElement(y + "State", new XAttribute("closed", "false"), new XAttribute("innerGraphDisplayEnabled", "true")));
                node.Add(new XElement(n + "data", new XAttribute("key", "d6"), new XElement(y + "ProxyAutoBoundsNode", new XElement(y + "Realizers", new XAttribute("active", "0"), groupNode))));

                if (initialState)
                {
                    groupNode.Add(new XElement(y + "BorderStyle", new XAttribute("width", "2.0")));
                }

                var subGraph = new XElement(n + "graph", new XAttribute("edgedefault", "directed"), new XAttribute("id", state.Id + ":"));
                node.Add(subGraph);
                foreach (var subState in state.SubStates)
                {
                    this.AddNode(subGraph, subState);
                }
            }
            else
            {
                var shape = new XElement(
                    y + "ShapeNode", 
                    new XElement(y + "NodeLabel", entryActions + state.Id + exitActions),
                    new XElement(y + "Shape", new XAttribute("type", "ellipse")));

                if (initialState)
                {
                    shape.Add(new XElement(y + "BorderStyle", new XAttribute("width", "2.0")));
                }

                node.Add(new XElement(n + "data", new XAttribute("key", "d6"), shape));
            }

            graph.Add(node);
        }

        private bool DetermineWhetherThisIsAnInitialState(IState<TState, TEvent> state)
        {
            return (this.initialStateId.IsInitialized && state.Id.ToString() == this.initialStateId.Value.ToString()) || (state.SuperState != null && state.SuperState.InitialState == state);
        }
    }
}
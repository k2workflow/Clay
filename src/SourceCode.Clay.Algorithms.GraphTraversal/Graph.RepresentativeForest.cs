#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Algorithms.GraphTraversal
{
    partial struct Graph<T> // .RepresentativeForest
    {
        /// <summary>
        /// Converts the graph into a representative forest.
        /// </summary>
        /// <param name="onCycle">A callback that is invoked when a cycle is detected.</param>
        /// <param name="onTreeNode">A callback that is invoked when a tree node is detected.</param>
        /// <remarks>
        /// A representative forest is a collection of trees and cycles that is used to answer traversal
        /// queries.
        /// 
        /// Traversing the forest (towards either parents or children) will traverse the original
        /// graph in that direction.  Note that a single node may appear multiple times within the
        /// forest.
        /// 
        /// Each node in the forest is represented by a cycle that has at minimum one element. Nodes that
        /// are represented by multiple elements are virtualized cycles: traversal queries will always visit
        /// every node in the cycle no matter the traversal direction.
        /// 
        /// To correctly perform a traversal query the following steps should be performed, given node <c>N</c>.
        /// 
        /// <list type="number">
        ///     <item><description>
        ///         Find the unique set of <see cref="Edge{T}.From"/> where the associated <see cref="Edge{T}.To"/> is <c>N</c>.
        ///     </description></item>
        ///     <item><description>
        ///         Find the unique list of <see cref="TreeNode{T}.Hierarchy"/> where <see cref="TreeNode{T}.Node"/> appears in the set created in (1).
        ///     </description></item>
        ///     <item><description>
        ///         Find the unique set of <see cref="TreeNode{T}.Node"/> according to traversing the forest, starting at the hierarchies discovered in (2).
        ///     </description></item>
        ///     <item><description>
        ///         Find the unique set of <see cref="Edge{T}.To"/> where the associated <see cref="Edge{T}.From"/> appears in the set created in (3).
        ///     </description></item>
        /// </list>
        /// 
        /// A single list instance is used across <paramref name="onTreeNode"/> invocations, <see cref="TreeNode{T}.Clone"/> must be called if 
        /// <see cref="TreeNode{T}"/> instances are persisted in memory after the callback completes.
        /// </remarks>
        public void ToRepresentativeForest(Action<Edge<T>> onCycle, Action<TreeNode<T>> onTreeNode)
        {
            if (onCycle is null) throw new ArgumentNullException(nameof(onCycle));
            if (onTreeNode is null) throw new ArgumentNullException(nameof(onTreeNode));
            if (_nodes is null) throw new InvalidOperationException();
            if (_nodes.Count == 0) return;

            // Capture
            System.Collections.Concurrent.ConcurrentDictionary<T, Node> nodes = _nodes;
            IEqualityComparer<T> equalityComparer = _equalityComparer;

            // Identify cycles.
            var stack = new Stack<T>();
            var index = 0;

            void StrongConnect(T v, ref Node vstate)
            {
                // Set the depth index for v to the smallest unused index.
                vstate.Index = index;
                vstate.LowLink = index;
                vstate.Options |= NodeOptions.OnStack | NodeOptions.StrongConnectExecuted;
                index++;
                nodes[v] = vstate;
                stack.Push(v);

                if (!(vstate.Edges is null))
                {
                    foreach (KeyValuePair<T, EdgeOptions> w in vstate.Edges)
                    {
                        Node wstate = nodes[w.Key];
                        if (!wstate.Options.HasFlag(NodeOptions.StrongConnectExecuted))
                        {
                            // Successor w has not yet been visited; recurse on it.
                            StrongConnect(w.Key, ref wstate);
                            vstate.LowLink = Math.Min(vstate.LowLink, wstate.LowLink);
                            nodes[v] = vstate;
                        }
                        else if (wstate.Options.HasFlag(NodeOptions.OnStack))
                        {
                            // Successor w is in stack S and hence in the current SCC
                            // If w is not on stack, then (v, w) is a cross-edge in the DFS tree and
                            // must be ignored.
                            vstate.LowLink = Math.Min(vstate.LowLink, wstate.Index);
                            nodes[v] = vstate;
                        }
                    }
                }

                // If v is a root node, pop the stack and generate an SCC.
                if (vstate.LowLink == vstate.Index)
                {
                    T w;
                    do
                    {
                        w = stack.Pop();

                        // Use vstate so that it's current when the method returns (this
                        // loop ends with w == v).
                        vstate = nodes[w].SetOptions(remove: NodeOptions.OnStack);
                        vstate.Cycle = v;
                        nodes[w] = vstate;

                        onCycle(new Edge<T>(v, w));
                    } while (!equalityComparer.Equals(v, w));
                }
            }

            foreach (KeyValuePair<T, Node> node in nodes)
            {
                if (node.Value.Options.HasFlag(NodeOptions.Descendant)) continue;
                if (node.Value.Options.HasFlag(NodeOptions.StrongConnectExecuted)) continue;

                Node state = node.Value;
                StrongConnect(node.Key, ref state);
            }

            // Fallback if no nodes are roots.
            if (index == 0)
            {
                foreach (KeyValuePair<T, Node> node in nodes)
                {
                    if (node.Value.Options.HasFlag(NodeOptions.StrongConnectExecuted)) continue;

                    Node state = node.Value;

                    // Lie about it being a root so that other loops can start at this node.
                    state.Options &= ~NodeOptions.Descendant;
                    StrongConnect(node.Key, ref state);
                }
            }

            // Add exit nodes.
            void FindExits(T v, Node vstate)
            {
                if (vstate.Edges is null) return;

                // The entry-point for cycles contains the list of exit nodes.
                Node cycleState = nodes[vstate.Cycle];

                foreach (KeyValuePair<T, EdgeOptions> w in vstate.Edges)
                {
                    Node wstate = nodes[w.Key];

                    // If w is in a different cycle, then it is an escape from the current cycle.
                    if (!equalityComparer.Equals(wstate.Cycle, vstate.Cycle))
                    {
                        if (cycleState.Exits is null)
                        {
                            cycleState.Exits = new HashSet<T>(equalityComparer);
                            nodes[vstate.Cycle] = cycleState;
                        }
                        cycleState.Exits.Add(w.Key);
                    }

                    if (!w.Value.HasFlag(EdgeOptions.FindExitsExecuted))
                    {
                        vstate.Edges[w.Key] |= EdgeOptions.FindExitsExecuted;
                        FindExits(w.Key, wstate);
                    }
                }
            }

            foreach (KeyValuePair<T, Node> node in nodes)
            {
                if (node.Value.Options.HasFlag(NodeOptions.Descendant)) continue;

                FindExits(node.Key, node.Value);
            }

            // Traverse exit nodes to find trees.
            var hierarchy = new List<int> { 0 };

            void FindTrees(T v, Node vstate)
            {
                // We don't need to check epoch, as StrongConnect and FindExits have created an
                // acyclic graph.

                onTreeNode(new TreeNode<T>(v, equalityComparer, hierarchy));

                if (vstate.Exits is null) return;

                var currentIndex = hierarchy.Count;
                hierarchy.Add(0);

                foreach (T w in vstate.Exits)
                {
                    FindTrees(w, nodes[w]);
                    hierarchy[currentIndex]++;
                }

                hierarchy.RemoveAt(currentIndex);
            }

            index = 0;
            foreach (KeyValuePair<T, Node> node in nodes)
            {
                if (node.Value.Options.HasFlag(NodeOptions.Descendant)) continue;

                FindTrees(node.Key, node.Value);
                hierarchy[0]++;
            }
        }
    }
}

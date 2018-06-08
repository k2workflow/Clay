#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Algorithms
{
    public partial struct Graph<T>
    {
        private void StrongConnect(T v, ref Node vstate, ref int index, Stack<T> stack, Action<Edge<T>> onCycle)
        {
            // Set the depth index for v to the smallest unused index.
            vstate.Index = index;
            vstate.LowLink = index;
            vstate.Options |= NodeOptions.OnStack | NodeOptions.StrongConnectExecuted;
            index++;
            _nodes[v] = vstate;
            stack.Push(v);

            if (vstate.Edges != null)
            {
                foreach (var w in vstate.Edges)
                {
                    var wstate = _nodes[w.Key];
                    if (!wstate.Options.HasFlag(NodeOptions.StrongConnectExecuted))
                    {
                        // Successor w has not yet been visited; recurse on it.
                        StrongConnect(w.Key, ref wstate, ref index, stack, onCycle);
                        vstate.LowLink = Math.Min(vstate.LowLink, wstate.LowLink);
                        _nodes[v] = vstate;
                    }
                    else if (wstate.Options.HasFlag(NodeOptions.OnStack))
                    {
                        // Successor w is in stack S and hence in the current SCC
                        // If w is not on stack, then (v, w) is a cross-edge in the DFS tree and
                        // must be ignored.
                        vstate.LowLink = Math.Min(vstate.LowLink, wstate.Index);
                        _nodes[v] = vstate;
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
                    vstate = _nodes[w].SetOptions(remove: NodeOptions.OnStack);
                    vstate.Cycle = v;
                    _nodes[w] = vstate;

                    onCycle(new Edge<T>(v, w));
                } while (!_equalityComparer.Equals(v, w));
            }
        }

        private void FindExits(T v, Node vstate)
        {
            if (vstate.Edges == null) return;

            // The entry-point for cycles contains the list of exit nodes.
            var cycleState = _nodes[vstate.Cycle];

            foreach (var w in vstate.Edges)
            {
                var wstate = _nodes[w.Key];

                // If w is in a different cycle, then it is an escape from the current cycle.
                if (!_equalityComparer.Equals(wstate.Cycle, vstate.Cycle))
                {
                    if (cycleState.Exits == null)
                    {
                        cycleState.Exits = new HashSet<T>(_equalityComparer);
                        _nodes[vstate.Cycle] = cycleState;
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

        private void FindTrees(T v, Node vstate, List<int> hierarchy, Action<TreeNode<T>> onTreeNode)
        {
            // We don't need to check epoch, as StrongConnect and FindExits have created an
            // acyclic graph.

            onTreeNode(new TreeNode<T>(v, hierarchy));

            if (vstate.Exits == null) return;

            var index = hierarchy.Count;
            hierarchy.Add(0);

            foreach (var w in vstate.Exits)
            {
                FindTrees(w, _nodes[w], hierarchy, onTreeNode);
                hierarchy[index]++;
            }

            hierarchy.RemoveAt(index);
        }

        public void ToRepresentativeForest(Action<Edge<T>> onCycle, Action<TreeNode<T>> onTreeNode)
        {
            if (onCycle == null) throw new ArgumentNullException(nameof(onCycle));
            if (onTreeNode == null) throw new ArgumentNullException(nameof(onTreeNode));
            if (_nodes == null) throw new InvalidOperationException();
            if (_nodes.Count == 0) return;
            var first = _nodes.First();

            // Identify cycles.
            var stack = new Stack<T>();
            var index = 0;
            foreach (var node in _nodes)
            {
                if (node.Value.Options.HasFlag(NodeOptions.Descendant)) continue;
                if (node.Value.Options.HasFlag(NodeOptions.StrongConnectExecuted)) continue;

                var state = node.Value;
                StrongConnect(node.Key, ref state, ref index, stack, onCycle);
            }

            // Fallback if no nodes are roots.
            if (index == 0)
            {
                foreach (var node in _nodes)
                {
                    if (node.Value.Options.HasFlag(NodeOptions.StrongConnectExecuted)) continue;

                    var state = node.Value;

                    // Lie about it being a root so that other loops can start at this node.
                    state.Options &= ~NodeOptions.Descendant;
                    StrongConnect(node.Key, ref state, ref index, stack, onCycle);
                }
            }

            // Add exit nodes.
            foreach (var node in _nodes)
            {
                if (node.Value.Options.HasFlag(NodeOptions.Descendant)) continue;

                FindExits(node.Key, node.Value);
            }

            // Traverse exit nodes to find trees.
            var hierarchy = new List<int> { 0 };
            foreach (var node in _nodes)
            {
                if (node.Value.Options.HasFlag(NodeOptions.Descendant)) continue;

                FindTrees(node.Key, node.Value, hierarchy, onTreeNode);
                hierarchy[0]++;
            }
        }
    }
}

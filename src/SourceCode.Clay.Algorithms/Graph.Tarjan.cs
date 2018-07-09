using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Algorithms
{
    partial struct Graph<T> // .Tarjan
    {
        /// <summary>
        /// Identifies strongly connected components (cycles) in a graph and performs a topological sort, by using
        /// Tarjan's Strongly Connected Components Algorithm.
        /// </summary>
        /// <remarks>
        /// All nodes in the graph appear in at least one strongly connected component (that may only contain the node itself). The
        /// graph is only cyclic if any cycle contains more than one node.
        /// </remarks>
        /// <returns>The list of strongly connected components.</returns>
        public IReadOnlyList<IReadOnlyList<T>> Tarjan()
        {
            var index = 0;
            var stack = new Stack<T>();
            var nodes = _nodes;
            var equalityComparer = _equalityComparer;
            var result = new List<IReadOnlyList<T>>(nodes.Count);

            void StrongConnect(T v, ref Node vstate)
            {
                // Set the depth index for v to the smallest unused index.
                vstate.Index = index;
                vstate.LowLink = index;
                vstate.Options |= NodeOptions.OnStack | NodeOptions.StrongConnectExecuted;
                index++;
                nodes[v] = vstate;
                stack.Push(v);

                if (vstate.Edges != null)
                {
                    foreach (var w in vstate.Edges)
                    {
                        var wstate = nodes[w.Key];
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
                    var cycle = new List<T>();

                    T w;
                    do
                    {
                        w = stack.Pop();

                        // Use vstate so that it's current when the method returns (this
                        // loop ends with w == v).
                        vstate = nodes[w].SetOptions(remove: NodeOptions.OnStack);
                        vstate.Cycle = v;
                        nodes[w] = vstate;

                        cycle.Add(w);
                    } while (!equalityComparer.Equals(v, w));

                    result.Add(cycle);
                }
            }

            foreach (var node in nodes)
            {
                if (node.Value.Options.HasFlag(NodeOptions.StrongConnectExecuted)) continue;

                var state = node.Value;
                StrongConnect(node.Key, ref state);
            }

            return result;
        }
    }
}

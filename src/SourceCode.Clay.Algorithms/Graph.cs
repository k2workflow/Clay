#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace SourceCode.Clay.Algorithms
{
    /// <summary>
    /// Factory methods for <see cref="Graph{T}"/>.
    /// </summary>
    public static class Graph
    {
        /// <summary>
        /// Creates a new <see cref="Graph{T}"/> value.
        /// </summary>
        /// <typeparam name="T">The type of the vertices in the graph.</typeparam>
        /// <param name="capacity">The initial capacity of the graph.</param>
        /// <param name="equalityComparer">The equality comparer for vertices.</param>
        /// <returns>The <see cref="Graph{T}"/>.</returns>
        public static Graph<T> Create<T>(int capacity = 10, IEqualityComparer<T> equalityComparer = null)
            => new Graph<T>(capacity, equalityComparer);
    }

#pragma warning disable CA1815

    // Override equals and operator equals on value types
    // No best-fit implementation

    /// <summary>
    /// Represents several algorithms that can be performed on a graph.
    /// </summary>
    /// <typeparam name="T">The type of the vertices in the graph.</typeparam>
    /// <remarks>Multiple algorithms should not be executed against a single instance of this structure.</remarks>
    [DebuggerDisplay("{_nodes.Count,ac}")]
    public readonly partial struct Graph<T>
    {
        private readonly ConcurrentDictionary<T, Node> _nodes;
        private readonly IEqualityComparer<T> _equalityComparer;

        /// <summary>
        /// Creates a new <see cref="Graph{T}"/> value.
        /// </summary>
        /// <param name="capacity">The initial capacity of the graph.</param>
        /// <param name="equalityComparer">The equality comparer for vertices.</param>
        public Graph(int capacity, IEqualityComparer<T> equalityComparer)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            _nodes = new ConcurrentDictionary<T, Node>(1, capacity, _equalityComparer);
        }

        /// <summary>
        /// Adds the specified directed edge to the graph.
        /// </summary>
        /// <param name="from">The vertex that the edge originates from.</param>
        /// <param name="to">The vertex that the edge terminates at.</param>
        public void Add(T from, T to)
        {
            ConcurrentDictionary<T, Node> nodes = _nodes;

            Node fromState = GetOrAdd(from);
            if (fromState.Edges is null)
            {
                fromState.Edges = new ConcurrentDictionary<T, EdgeOptions>(_equalityComparer);
                _nodes[from] = fromState;
            }

            fromState.Edges.TryAdd(to, EdgeOptions.None);
            _nodes[to] = GetOrAdd(to).SetOptions(add: NodeOptions.Descendant);
        }

        private Node GetOrAdd(T key) => _nodes.GetOrAdd(key, CreateValue);

        private static Node CreateValue(T key) => new Node();
    }

#pragma warning restore CA1815
}

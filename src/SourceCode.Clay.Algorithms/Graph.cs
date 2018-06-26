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
    public static class Graph
    {
        public static Graph<T> Create<T>(IEqualityComparer<T> equalityComparer) => new Graph<T>(equalityComparer);

        public static Graph<T> Create<T>() => Create<T>(null);
    }

#pragma warning disable CA1815

    // Override equals and operator equals on value types
    // No best-fit implementation
    [DebuggerDisplay("{_nodes.Count,ac}")]
    public readonly partial struct Graph<T>
    {
        private readonly ConcurrentDictionary<T, Node> _nodes;
        private readonly IEqualityComparer<T> _equalityComparer;

        public Graph(int capacity, IEqualityComparer<T> equalityComparer)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            _nodes = new ConcurrentDictionary<T, Node>(1, capacity, _equalityComparer);
        }

        public Graph(IEqualityComparer<T> equalityComparer)
            : this(1000, equalityComparer)
        { }

        public void Add(T from, T to)
        {
            var nodes = _nodes;

            var fromState = GetOrAdd(from);
            if (fromState.Edges == null)
            {
                fromState.Edges = new ConcurrentDictionary<T, EdgeOptions>(_equalityComparer);
                _nodes[from] = fromState;
            }

            fromState.Edges.TryAdd(to, EdgeOptions.None);
            _nodes[to] = GetOrAdd(to).SetOptions(add: NodeOptions.Descendant);

            Node GetOrAdd(T key) => nodes.GetOrAdd(key, CreateValue);

            Node CreateValue(T key) => new Node();
        }
    }

#pragma warning restore CA1815
}

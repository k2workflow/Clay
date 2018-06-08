#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Concurrent;
using System.Collections.Generic;

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
    public readonly partial struct Graph<T>
    {
        private readonly ConcurrentDictionary<T, Node> _nodes;
        private readonly IEqualityComparer<T> _equalityComparer;

        private static Node CreateValue(T key) => new Node();

        private Node GetOrAdd(T key) => _nodes.GetOrAdd(key, CreateValue);

        public Graph(IEqualityComparer<T> equalityComparer)
        {
            if (equalityComparer == null) equalityComparer = EqualityComparer<T>.Default;
            _equalityComparer = equalityComparer;
            _nodes = new ConcurrentDictionary<T, Node>(1, 1000, equalityComparer);
        }

        public void Add(T from, T to)
        {
            var fromState = GetOrAdd(from);
            if (fromState.Edges == null)
            {
                fromState.Edges = new ConcurrentDictionary<T, EdgeOptions>(_equalityComparer);
                _nodes[from] = fromState;
            }

            fromState.Edges.TryAdd(to, EdgeOptions.None);
            _nodes[to] = GetOrAdd(to).SetOptions(add: NodeOptions.Descendant);
        }
    }

#pragma warning restore CA1815
}

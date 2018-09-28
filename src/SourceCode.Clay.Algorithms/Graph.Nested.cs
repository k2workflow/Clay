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
#pragma warning disable CA1815

    partial struct Graph<T> // .Nested
    {
        [Flags]
        private enum EdgeOptions : byte
        {
            None = 0b_0000_0000,
            FindExitsExecuted = 0b_0001_0000
        }

        [Flags]
        private enum NodeOptions : byte
        {
            None = 0b_0000_0000,
            Descendant = 0b_0000_0001,
            OnStack = 0b_0000_0010,

            StrongConnectExecuted = 0b_0001_0000,
        }

        [DebuggerDisplay("Cycle={Cycle,ac} Count={Edges.Count,ac}")]
        private struct Node
        {
            public ConcurrentDictionary<T, EdgeOptions> Edges;
            public HashSet<T> Exits;
            public T Cycle;
            public int Index;
            public int LowLink;
            public NodeOptions Options;

            public Node SetOptions(NodeOptions remove = NodeOptions.None, NodeOptions add = NodeOptions.None)
            {
                Node node = this;
                node.Options = (node.Options & ~remove) | add;
                return node;
            }
        }
    }

#pragma warning restore CA1815
}

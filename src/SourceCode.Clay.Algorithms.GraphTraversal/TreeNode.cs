#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace SourceCode.Clay.Algorithms.GraphTraversal
{
    /// <summary>
    /// Represents a tree node that uses a hierarchy path for positioning.
    /// </summary>
    /// <typeparam name="T">The type of the node in the tree.</typeparam>
    [DebuggerDisplay("{HierarchyPath,nq} {Node}")]
    public readonly struct TreeNode<T> : IEquatable<TreeNode<T>>, ICloneable
    {
        private readonly IEqualityComparer<T> _equalityComparer;

        /// <summary>
        /// Gets the list of integers that index to the tree node position.
        /// </summary>
        public IReadOnlyList<int> Hierarchy { get; }

        /// <summary>
        /// Gets the node value.
        /// </summary>
        public T Node { get; }

        /// <summary>
        /// Gets a path containing <see cref="Hierarchy"/> elements separated by a '/' character.
        /// </summary>
        public string HierarchyPath
        {
            get
            {
                if (Hierarchy is null) return null;

                var capacity = 1 + Hierarchy.Count * (3 + 1); // Initial '/', plus ~3 chars for each int, plus '/' for each segment

                var sb = new StringBuilder("/", capacity);
                for (var i = 0; i < Hierarchy.Count; i++)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}/", Hierarchy[i]);

                return sb.ToString();
            }
        }

        /// <summary>
        /// Creates a new <see cref="TreeNode{T}"/> value.
        /// </summary>
        /// <param name="node">The node value.</param>
        /// <param name="equalityComparer">The equality comparer used to compare values.</param>
        /// <param name="hierarchy">The list of integers that index to the tree node position.</param>
        public TreeNode(T node, IEqualityComparer<T> equalityComparer, IReadOnlyList<int> hierarchy)
        {
            Hierarchy = hierarchy ?? throw new ArgumentNullException(nameof(hierarchy));
            Node = node;
            _equalityComparer = equalityComparer; // No point coalescing in a struct
        }

        /// <summary>
        /// Creates a new <see cref="TreeNode{T}"/> value.
        /// </summary>
        /// <param name="node">The node value.</param>
        /// <param name="equalityComparer">The equality comparer used to compare values.</param>
        /// <param name="hierarchy">The list of integers that index to the tree node position.</param>
        public TreeNode(T node, IEqualityComparer<T> equalityComparer, params int[] hierarchy)
            : this(node, equalityComparer, (IReadOnlyList<int>)hierarchy)
        { }

        /// <summary>
        /// Creates a new <see cref="TreeNode{T}"/> value.
        /// </summary>
        /// <param name="node">The node value.</param>
        /// <param name="hierarchy">The list of integers that index to the tree node position.</param>
        public TreeNode(T node, IReadOnlyList<int> hierarchy)
            : this(node, null, hierarchy)
        { }

        /// <summary>
        /// Creates a new <see cref="TreeNode{T}"/> value.
        /// </summary>
        /// <param name="node">The node value.</param>
        /// <param name="hierarchy">The list of integers that index to the tree node position.</param>
        public TreeNode(T node, params int[] hierarchy)
            : this(node, null, (IReadOnlyList<int>)hierarchy)
        { }

        object ICloneable.Clone() => Clone();

        /// <summary>
        /// Clones the <see cref="TreeNode{T}"/> so that it contains a unique instance of <see cref="Hierarchy"/>.
        /// </summary>
        /// <returns>The <see cref="TreeNode{T}"/>.</returns>
        public TreeNode<T> Clone()
        {
            if (Hierarchy is null) return default;

            var array = new int[Hierarchy.Count];
            for (var i = 0; i < array.Length; i++)
                array[i] = Hierarchy[i];

            return new TreeNode<T>(Node, _equalityComparer, array);
        }

        /// <summary>
        /// Returns a string representation of the tree node.
        /// </summary>
        /// <returns>A string representation of the tree node.</returns>
        public override string ToString() => $"{HierarchyPath} {Node}";

        /// <summary>
        /// Determines if this tree node is equal to another tree node.
        /// </summary>
        /// <param name="obj">The other tree node.</param>
        /// <returns>A value indicating whether this tree node is equal to another.</returns>
        public override bool Equals(object obj)
            => obj is TreeNode<T> other
            && Equals(other);

        /// <summary>
        /// Determines if this tree node is equal to another tree node.
        /// </summary>
        /// <param name="other">The other tree node.</param>
        /// <returns>A value indicating whether this tree node is equal to another.</returns>
        public bool Equals(TreeNode<T> other)
        {
            if (Hierarchy is null) return other.Hierarchy is null;
            if (other.Hierarchy is null) return false;

            if (Hierarchy.Count != other.Hierarchy.Count) return false;

            IEqualityComparer<T> equalityComparer = _equalityComparer ?? EqualityComparer<T>.Default;
            if (!equalityComparer.Equals(Node, other.Node)) return false;

            // Hierarchies have a greater probability of being different starting from the end
            for (var i = Hierarchy.Count - 1; i >= 0; i--)
            {
                if (Hierarchy[i] != other.Hierarchy[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            var hashCode = -1781160927;

            // Equal hierarchies probably represent the same node, losing this entropy is less
            // important than the hierarchy itself.
            IEqualityComparer<T> equalityComparer = _equalityComparer ?? EqualityComparer<T>.Default;
            hashCode = hashCode * -1521134295 + equalityComparer.GetHashCode(Node);

            if (!(Hierarchy is null))
            {
                var step = Math.Max(1, Hierarchy.Count / 10);

                // Hierarchies have a greater probability of being different starting from the end,
                // that value should be mixed in last (as opposed to checked first as-per equals).
                for (var i = 0; i < Hierarchy.Count; i += step)
                {
                    hashCode = hashCode * -1521134295 + Hierarchy[i].GetHashCode();
                }

                hashCode = hashCode * -1521134295 + Hierarchy.Count;
            }

            return hashCode;
        }

        /// <summary>
        /// Implements the equality operator for <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="node1">The first tree node to compare.</param>
        /// <param name="node2">The second tree node to compare.</param>
        /// <returns>A value indicating whether the tree nodes are equal.</returns>
        public static bool operator ==(TreeNode<T> node1, TreeNode<T> node2)
            => node1.Equals(node2);

        /// <summary>
        /// Implements the inequality operator for <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="node1">The first tree node to compare.</param>
        /// <param name="node2">The second tree node to compare.</param>
        /// <returns>A value indicating whether the tree nodes are not equal.</returns>
        public static bool operator !=(TreeNode<T> node1, TreeNode<T> node2)
            => !(node1 == node2);
    }
}

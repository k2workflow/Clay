#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SourceCode.Clay.Algorithms
{
    /// <summary>
    /// Represents a directed graph edge.
    /// </summary>
    /// <typeparam name="T">The type of the vertices in the edge.</typeparam>
    [DebuggerDisplay("{ToString(),nq}")]
    public readonly struct Edge<T> : IEquatable<Edge<T>>
    {
        private readonly IEqualityComparer<T> _equalityComparer;

        /// <summary>
        /// The vertex that the edge originates from.
        /// </summary>
        public T From { get; }

        /// <summary>
        /// The vertex that the edge terminates at.
        /// </summary>
        public T To { get; }

        /// <summary>
        /// Creates a new <see cref="Edge{T}"/> value.
        /// </summary>
        /// <param name="from">The vertex that the edge originates from.</param>
        /// <param name="to">The vertex that the edge terminates at.</param>
        /// <param name="equalityComparer"></param>
        public Edge(T from, T to, IEqualityComparer<T> equalityComparer = null)
        {
            From = from;
            To = to;
            _equalityComparer = equalityComparer; // No point coalescing in a struct since default ctor won't assign
        }

        /// <summary>
        /// Returns a string representation of the edge.
        /// </summary>
        /// <returns>A string representation of the edge.</returns>
        public override string ToString() => $"{From} -> {To}";

        /// <summary>
        /// Determines if this edge is equal to another edge.
        /// </summary>
        /// <param name="obj">The other edge.</param>
        /// <returns>A value indicating whether this edge is equal to another.</returns>
        public override bool Equals(object obj)
            => obj is Edge<T> other
            && Equals(other);

        /// <summary>
        /// Determines if this edge is equal to another edge.
        /// </summary>
        /// <param name="other">The other edge.</param>
        /// <returns>A value indicating whether this edge is equal to another.</returns>
        public bool Equals(Edge<T> other)
        {
            IEqualityComparer<T> equalityComparer = _equalityComparer ?? EqualityComparer<T>.Default;

            return equalityComparer.Equals(From, other.From)
                   && equalityComparer.Equals(To, other.To);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            IEqualityComparer<T> equalityComparer = _equalityComparer ?? EqualityComparer<T>.Default;

            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + equalityComparer.GetHashCode(From);
            hashCode = hashCode * -1521134295 + equalityComparer.GetHashCode(To);
            return hashCode;
        }

        /// <summary>
        /// Implements the equality operator for <see cref="Edge{T}"/>.
        /// </summary>
        /// <param name="edge1">The first edge to compare.</param>
        /// <param name="edge2">The second edge to compare.</param>
        /// <returns>A value indicating whether the edges are equal.</returns>
        public static bool operator ==(Edge<T> edge1, Edge<T> edge2)
            => edge1.Equals(edge2);

        /// <summary>
        /// Implements the inequality operator for <see cref="Edge{T}"/>.
        /// </summary>
        /// <param name="edge1">The first edge to compare.</param>
        /// <param name="edge2">The second edge to compare.</param>
        /// <returns>A value indicating whether the edges are not equal.</returns>
        public static bool operator !=(Edge<T> edge1, Edge<T> edge2)
            => !(edge1 == edge2);
    }
}

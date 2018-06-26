#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SourceCode.Clay.Algorithms
{
    [DebuggerDisplay("{ToString(),nq}")]
    public readonly struct Edge<T> : IEquatable<Edge<T>>, IStructuralEquatable
    {
        private readonly IEqualityComparer<T> _equalityComparer;

        public T From { get; }

        public T To { get; }

        public Edge(T from, T to, IEqualityComparer<T> equalityComparer = null)
        {
            From = from;
            To = to;
            _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        public override string ToString() => $"{From} -> {To}";

        public override bool Equals(object obj)
            => obj is Edge<T> other
            && Equals(other);

        public bool Equals(Edge<T> other)
            => _equalityComparer.Equals(From, other.From)
            && _equalityComparer.Equals(To, other.To);

        public override int GetHashCode()
        {
            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + _equalityComparer.GetHashCode(From);
            hashCode = hashCode * -1521134295 + _equalityComparer.GetHashCode(To);
            return hashCode;
        }

        public static bool operator ==(Edge<T> edge1, Edge<T> edge2) 
            => edge1.Equals(edge2);

        public static bool operator !=(Edge<T> edge1, Edge<T> edge2) 
            => !(edge1 == edge2);

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + comparer.GetHashCode(From);
            hashCode = hashCode * -1521134295 + comparer.GetHashCode(To);
            return hashCode;
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
            => other is Edge<T> e
            && (comparer ?? throw new ArgumentNullException(nameof(comparer))).Equals(From, e.From)
            && comparer.Equals(To, e.To);
    }
}

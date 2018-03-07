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
    [DebuggerDisplay("{From} -> {To}")]
    public struct Edge<T> : IEquatable<Edge<T>>, IStructuralEquatable
    {
        #region Properties

        public T From { get; }
        public T To { get; }

        #endregion

        #region Constructors

        public Edge(T from, T to)
        {
            From = from;
            To = to;
        }

        #endregion

        #region Methods

        public static bool operator ==(Edge<T> edge1, Edge<T> edge2) => edge1.Equals(edge2);

        public static bool operator !=(Edge<T> edge1, Edge<T> edge2) => !(edge1 == edge2);

        public override string ToString() => $"{From} -> {To}";

        public override bool Equals(object obj)
            => obj is Edge<T> o && Equals(o);

        public bool Equals(Edge<T> other)
            => EqualityComparer<T>.Default.Equals(From, other.From)
            && EqualityComparer<T>.Default.Equals(To, other.To);

        public override int GetHashCode()
        {
            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(From);
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(To);
            return hashCode;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + comparer.GetHashCode(From);
            hashCode = hashCode * -1521134295 + comparer.GetHashCode(To);
            return hashCode;
        }

        public bool Equals(object other, IEqualityComparer comparer)
            => other is Edge<T> e
            && (comparer ?? throw new ArgumentNullException(nameof(comparer))).Equals(From, e.From)
            && comparer.Equals(To, e.To);

        #endregion
    }
}

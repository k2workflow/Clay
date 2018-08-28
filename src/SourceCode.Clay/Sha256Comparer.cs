#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a way to compare different <see cref="Sha256"/> values.
    /// </summary>
    public abstract class Sha256Comparer : IEqualityComparer<Sha256>, IComparer<Sha256>
    {
        /// <summary>
        /// Gets a <see cref="Sha256Comparer"/> that compares all fields of a <see cref="Sha256"/> value.
        /// </summary>
        public static Sha256Comparer Default { get; } = new DefaultComparer();

        private Sha256Comparer()
        { }

        /// <inheritdoc/>
        public abstract int Compare(Sha256 x, Sha256 y);

        /// <inheritdoc/>
        public abstract bool Equals(Sha256 x, Sha256 y);

        /// <inheritdoc/>
        public abstract int GetHashCode(Sha256 obj);

        private sealed class DefaultComparer : Sha256Comparer
        {
            public override int Compare(Sha256 x, Sha256 y) => x.CompareTo(y);

            public override bool Equals(Sha256 x, Sha256 y) => x.Equals(y);

            public override int GetHashCode(Sha256 obj) => obj.GetHashCode();
        }
    }
}

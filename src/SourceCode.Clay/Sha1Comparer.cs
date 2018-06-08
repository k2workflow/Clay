#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a way to compare different <see cref="Sha1"/> values.
    /// </summary>
    public abstract class Sha1Comparer : IEqualityComparer<Sha1>, IComparer<Sha1>
    {
        /// <summary>
        /// Gets a <see cref="Sha1Comparer"/> that compares all fields of a <see cref="Sha1"/> value.
        /// </summary>
        public static Sha1Comparer Default { get; } = new DefaultComparer();

        private Sha1Comparer()
        { }

        /// <inheritdoc/>
        public abstract int Compare(Sha1 x, Sha1 y);

        /// <inheritdoc/>
        public abstract bool Equals(Sha1 x, Sha1 y);

        /// <inheritdoc/>
        public abstract int GetHashCode(Sha1 obj);

        private sealed class DefaultComparer : Sha1Comparer
        {
            public override int Compare(Sha1 x, Sha1 y) => x.CompareTo(y);

            public override bool Equals(Sha1 x, Sha1 y) => x.Equals(y);

            public override int GetHashCode(Sha1 obj) => obj.GetHashCode();
        }
    }
}

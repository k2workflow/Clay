using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    partial class BufferComparer : IEqualityComparer<IReadOnlyList<byte>>, IComparer<IReadOnlyList<byte>>
    {
        #region IComparer

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        int IComparer<IReadOnlyList<byte>>.Compare(IReadOnlyList<byte> x, IReadOnlyList<byte> y)
            => CompareReadOnlyListImpl(x, y);

        [SecuritySafeCritical]
        private static int CompareReadOnlyListImpl(IReadOnlyList<byte> x, IReadOnlyList<byte> y)
        {
            if (ReferenceEquals(x, y)) return 0; // (null, null) or (x, x)
            if (x == null) return -1; // (null, y)
            if (y == null) return 1; // (x, null)

            var cmp = x.Count.CompareTo(y.Count); // (x, y)
            if (cmp != 0) return cmp;

            // Use fast path if both are arrays
            if (x is byte[] bax && y is byte[] bay)
                return CompareArrayImpl(bax, bay);

            // Use fast path if both are ArraySegments
            if (x is ArraySegment<byte> sgx && y is ArraySegment<byte> sgy)
                return CompareArraySegmentImpl(sgx, sgy);

            // Else forced to use slow path
            for (var i = 0; i < x.Count; i++)
            {
                cmp = x[i].CompareTo(y[i]);
                if (cmp != 0) return cmp;
            }

            return 0;
        }

        #endregion

        #region IEqualityComparer

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        bool IEqualityComparer<IReadOnlyList<byte>>.Equals(IReadOnlyList<byte> x, IReadOnlyList<byte> y)
            => CompareReadOnlyListImpl(x, y) == 0;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        int IEqualityComparer<IReadOnlyList<byte>>.GetHashCode(IReadOnlyList<byte> obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (_hashCodeFidelity <= 0 || obj.Count <= _hashCodeFidelity)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(obj.Take(_hashCodeFidelity));
        }

        #endregion
    }
}

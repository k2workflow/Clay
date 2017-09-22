using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    partial class BufferComparer : IEqualityComparer<IEnumerable<byte>>, IComparer<IEnumerable<byte>>
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
        int IComparer<IEnumerable<byte>>.Compare(IEnumerable<byte> x, IEnumerable<byte> y)
            => CompareEnumerableImpl(x, y);

        [SecuritySafeCritical]
        private static int CompareEnumerableImpl(IEnumerable<byte> x, IEnumerable<byte> y)
        {
            if (ReferenceEquals(x, y)) return 0; // (null, null) or (x, x)
            if (x == null) return -1; // (null, y)
            if (y == null) return 1; // (x, null)

            // Use fast path if both are arrays
            if (x is byte[] bax && y is byte[] bay)
                return CompareArrayImpl(bax, bay);

            // Use fast path if both are ArraySegments
            if (x is ArraySegment<byte> sgx && y is ArraySegment<byte> sgy)
                return CompareArraySegmentImpl(sgx, sgy);

            // Else forced to use slow path
            using (var xe = x.GetEnumerator())
            using (var ye = y.GetEnumerator())
            {
                while (xe.MoveNext())
                {
                    if (!ye.MoveNext()) return 1; // x.Count > y.Count
                    var c = xe.Current.CompareTo(ye.Current);
                    if (c != 0) return c;
                }

                if (ye.MoveNext()) return -1; // y.Count > x.Count
                return 0;
            }
        }

        #endregion

        #region IEqualityComparer

        bool IEqualityComparer<IEnumerable<byte>>.Equals(IEnumerable<byte> x, IEnumerable<byte> y)
            => CompareEnumerableImpl(x, y) == 0;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        int IEqualityComparer<IEnumerable<byte>>.GetHashCode(IEnumerable<byte> obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (_hashCodeFidelity <= 0)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(obj.Take(_hashCodeFidelity));
        }

        #endregion
    }
}

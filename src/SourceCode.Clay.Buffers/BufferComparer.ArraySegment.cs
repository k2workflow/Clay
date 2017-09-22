using System;
using System.Collections.Generic;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    partial class BufferComparer : IEqualityComparer<ArraySegment<byte>>, IComparer<ArraySegment<byte>>
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
        int IComparer<ArraySegment<byte>>.Compare(ArraySegment<byte> x, ArraySegment<byte> y)
            => CompareArraySegmentImpl(x, y);

        [SecuritySafeCritical]
        private static int CompareArraySegmentImpl(ArraySegment<byte> x, ArraySegment<byte> y)
        {
            if (x.Array == null)
            {
                if (y.Array == null) return 0; // (null, null)
                return -1; // (null, y)
            }
            if (y.Array == null) return 1; // (x, null)

            var cmp = x.Count.CompareTo(y.Count);
            if (cmp != 0) return cmp; // (m, n)

            switch (x.Count)
            {
                // (0, 0)
                case 0:
                    return 0;

                // (m[0], n[0])
                case 1:
                    cmp = x.Array[x.Offset].CompareTo(y.Array[y.Offset]);
                    return cmp;

                // (m[0..N], n[0..N])
                default:
                    unsafe
                    {
                        fixed (byte* xp = x.Array, yp = y.Array)
                        {
                            cmp = NativeMethods.MemCompare(xp + x.Offset, yp + y.Offset, x.Count);
                            return cmp;
                        }
                    }
            }
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
        bool IEqualityComparer<ArraySegment<byte>>.Equals(ArraySegment<byte> x, ArraySegment<byte> y)
            => CompareArraySegmentImpl(x, y) == 0;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        int IEqualityComparer<ArraySegment<byte>>.GetHashCode(ArraySegment<byte> obj)
        {
            ReadOnlySpan<byte> span = obj;
            if (_hashCodeFidelity > 0 && obj.Count > _hashCodeFidelity)
                span = new ReadOnlySpan<byte>(obj.Array, obj.Offset, _hashCodeFidelity);

            var fnv = HashCode.Fnv(span);
            return fnv;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    partial class BufferComparer : IEqualityComparer<byte[]>, IComparer<byte[]>
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
        int IComparer<byte[]>.Compare(byte[] x, byte[] y)
            => CompareArrayImpl(x, y);

        [SecuritySafeCritical]
        private static int CompareArrayImpl(byte[] x, byte[] y)
        {
            if (ReferenceEquals(x, y)) return 0; // (null, null) or (x, x)
            if (x == null) return -1; // (null, y)
            if (y == null) return 1; // (x, null)

            var cmp = x.Length.CompareTo(y.Length); // (x, y)
            if (cmp != 0) return cmp; // (m, n)

            switch (x.Length)
            {
                // (0, 0)
                case 0:
                    return 0;

                // (m[0], n[0])
                case 1:
                    cmp = x[0].CompareTo(y[0]);
                    return cmp;

                // (m[0..N], n[0..N])
                default:
                    unsafe
                    {
                        fixed (byte* xp = x, yp = y)
                        {
                            cmp = NativeMethods.MemCompare(xp, yp, x.Length);
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
        bool IEqualityComparer<byte[]>.Equals(byte[] x, byte[] y)
            => CompareArrayImpl(x, y) == 0;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        int IEqualityComparer<byte[]>.GetHashCode(byte[] obj)
        {
            ReadOnlySpan<byte> span = obj;
            if (_hashCodeFidelity > 0 && obj.Length > _hashCodeFidelity)
                span = new ReadOnlySpan<byte>(obj, 0, _hashCodeFidelity);

            var fnv = HashCode.Fnv(span);
            return fnv;
        }

        #endregion
    }
}

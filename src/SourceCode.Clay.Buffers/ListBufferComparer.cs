using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare the contents of <see cref="IList{T}"/> buffers.
    /// </summary>
    public sealed class ListBufferComparer : BufferComparer<IList<byte>>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class, that considers the full
        /// buffer when calculating the hashcode.
        /// </summary>
        public ListBufferComparer()
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class.
        /// </summary>
        /// <param name="hashCodeFidelity">
        /// The maximum number of octets that processed when calculating a hashcode. Pass zero or a negative value to
        /// disable the limit.
        /// </param>
        public ListBufferComparer(int hashCodeFidelity)
            : base(hashCodeFidelity)
        { }

        #endregion

        #region IComparer

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public override int Compare(IList<byte> x, IList<byte> y)
            => CompareList(x, y);

        [SecuritySafeCritical]
        private static int CompareList(IList<byte> x, IList<byte> y)
        {
            if (ReferenceEquals(x, y)) return 0; // (null, null) or (x, x)
            if (x == null) return -1; // (null, y)
            if (y == null) return 1; // (x, null)

            var cmp = x.Count.CompareTo(y.Count); // (x, y)
            if (cmp != 0) return cmp;

            // Use fast path if both are arrays
            if (x is byte[] bax && y is byte[] bay)
                return BufferComparer.CompareSpan(bax, bay);

            // Use fast path if both are ArraySegments
            if (x is ArraySegment<byte> sgx && y is ArraySegment<byte> sgy)
                return BufferComparer.CompareSpan(sgx, sgy);

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
        public override bool Equals(IList<byte> x, IList<byte> y)
            => CompareList(x, y) == 0;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        public override int GetHashCode(IList<byte> obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (HashCodeFidelity <= 0 || obj.Count <= HashCodeFidelity)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(obj.Take(HashCodeFidelity));
        }

        #endregion
    }
}

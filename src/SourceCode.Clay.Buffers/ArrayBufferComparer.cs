using System;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare the contents of <see cref="Byte[]"/>.
    /// </summary>
    public sealed class ArrayBufferComparer : BufferComparer<byte[]>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ArrayBufferComparer"/> class, that considers the full
        /// buffer when calculating the hashcode.
        /// </summary>
        public ArrayBufferComparer()
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="ArrayBufferComparer"/> class.
        /// </summary>
        /// <param name="hashCodeFidelity">
        /// The maximum number of octets that processed when calculating a hashcode. Pass zero or a negative value to
        /// disable the limit.
        /// </param>
        public ArrayBufferComparer(int hashCodeFidelity)
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
        public override int Compare(byte[] x, byte[] y)
            => BufferComparer.CompareArray(x, y);

        #endregion

        #region IEqualityComparer

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode(byte[] obj)
        {
            // Fnv has consistent handling for Null
            if (obj == null)
                return HashCode.Fnv(obj);

            // Calculate on full length
            if (HashCodeFidelity <= 0 || obj.Length <= HashCodeFidelity) // Also handles Empty
                return HashCode.Fnv(obj);

            // Calculate on prefix
            var span = new ReadOnlySpan<byte>(obj, 0, HashCodeFidelity);
            var hc = HashCode.Fnv(span);
            return hc;
        }

        #endregion
    }
}

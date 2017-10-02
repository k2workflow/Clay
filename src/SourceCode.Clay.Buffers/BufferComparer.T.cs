using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    public abstract class BufferComparer<T> : IEqualityComparer<T>, IComparer<T>
    {
        #region Fields

        /// <summary>
        /// The prefix of the buffer that is considered for hashcode calculation.
        /// </summary>
        public int HashCodeFidelity { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class, that considers the full
        /// buffer when calculating the hashcode.
        /// </summary>
        protected BufferComparer()
        {
            HashCodeFidelity = 0;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class.
        /// </summary>
        /// <param name="hashCodeFidelity">
        /// The maximum number of octets processed when calculating a hashcode.
        /// Pass zero to disable the limit.
        /// </param>
        protected BufferComparer(int hashCodeFidelity)
        {
            if (hashCodeFidelity < 0) throw new ArgumentOutOfRangeException(nameof(hashCodeFidelity));

            HashCodeFidelity = hashCodeFidelity;
        }

        #endregion

        #region IComparer

        public abstract int Compare(T x, T y);

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
        public bool Equals(T x, T y)
            => Compare(x, y) == 0;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public abstract int GetHashCode(T obj);

        #endregion

        #region Comparison

        /// <summary>
        /// Returns a <see cref="Comparison{T}"/> delegate for use in methods such as <see cref="Array.Sort{T}(T[], Comparison{T})"/>.
        /// </summary>
        public int Comparison(T x, T y)
            => Compare(x, y);

        #endregion
    }
}

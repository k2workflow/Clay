using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    public abstract class BufferComparer<T> : IEqualityComparer<T>, IComparer<T>, IEqualityComparer, IComparer
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

        int IComparer.Compare(object x, object y)
        {
            if (ReferenceEquals(x, y)) return 0; // (null, null) or (x, x)
            if (x == null) return -1; // (null, y)
            if (y == null) return 1; // (x, null)

            if (x is T xt && y is T yt)
                return Compare(xt, yt);

            throw new ArgumentException($"Arguments for {nameof(Compare)} should both be of type {nameof(T)}");
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

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (ReferenceEquals(x, y)) return true; // (null, null) or (x, x)
            if (x == null) return false; // (null, y)
            if (y == null) return false; // (x, null)

            if (x is T xt && y is T yt)
                return Equals(xt, yt);

            return false;
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            if (obj == null) return 0;

            if (obj is T ot)
                return GetHashCode(ot);

            return obj.GetHashCode();
        }

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

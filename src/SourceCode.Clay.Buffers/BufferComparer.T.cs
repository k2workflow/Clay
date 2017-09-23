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

        public readonly int HashCodeFidelity;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class, that considers the full
        /// buffer when calculating the hashcode.
        /// </summary>
        protected BufferComparer()
            : this(-1)
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class.
        /// </summary>
        /// <param name="hashCodeFidelity">
        /// The maximum number of octets that processed when calculating a hashcode. Pass zero or a negative value to
        /// disable the limit.
        /// </param>
        protected BufferComparer(int hashCodeFidelity)
        {
            HashCodeFidelity = hashCodeFidelity;
        }

        #endregion

        #region IComparer

        public abstract int Compare(T x, T y);

        public int Compare(object x, object y)
        {
            if (ReferenceEquals(x, y)) return 0; // (null, null) or (x, x)
            if (x == null) return -1; // (null, y)
            if (y == null) return 1; // (x, null)

            if (x is T xt && y is T yt)
                return Compare(xt, yt);

            throw new ArgumentException($"Arguments for {nameof(Compare)} should both be {nameof(T)}");
        }

        #endregion

        #region IEqualityComparer

        public abstract bool Equals(T x, T y);

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y)) return true; // (null, null) or (x, x)
            if (x == null) return false; // (null, y)
            if (y == null) return false; // (x, null)

            if (x is T xt && y is T yt)
                return Equals(xt, yt);

            return false;
        }

        public abstract int GetHashCode(T obj);

        public int GetHashCode(object obj)
        {
            if (obj == null) return 0; ;

            if (obj is T ot)
                return GetHashCode(ot);

            return obj.GetHashCode();
        }

        #endregion
    }
}

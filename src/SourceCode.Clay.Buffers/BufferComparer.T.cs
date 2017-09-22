using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;

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
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is T ot)
                return GetHashCode(ot);

            return obj.GetHashCode();
        }

        #endregion

        #region Helpers

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int CompareReadOnlySpan(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y)
        {
            // From https://github.com/dotnet/corefx/blob/master/src/System.Memory/src/System/ReadOnlySpan.cs
            // public static bool operator ==
            // Returns true if left and right point at the same memory and have the same length.  Note that
            // this does *not* check to see if the *contents* are equal.
            if (x == y) return 0;

            if (x.IsEmpty)
            {
                if (y.IsEmpty) return 0; // (null, null)
                return -1; // (null, y)
            }
            if (y.IsEmpty) return 1; // (x, null)

            var cmp = x.Length.CompareTo(y.Length);
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
                    {
                        unsafe
                        {
                            fixed (byte* xp = &x.DangerousGetPinnableReference())
                            fixed (byte* yp = &y.DangerousGetPinnableReference())
                            {
                                cmp = NativeMethods.MemCompare(xp, yp, x.Length);
                                return cmp;
                            }
                        }
                    }
            }
        }

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int CompareArray(byte[] x, byte[] y)
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
                    {
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
        }

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int CompareArraySegment(ArraySegment<byte> x, ArraySegment<byte> y)
        {
            if (ReferenceEquals(x.Array, y.Array)) return 0; // (null, null) or (x, x)
            if (x.Array == null) return -1; // (null, y)
            if (y.Array == null) return 1; // (x, null)

            var cmp = x.Count.CompareTo(y.Count); // (x, y)
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
                    {
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
        }

        #endregion
    }
}

using System;
using System.Runtime.CompilerServices;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    public static class BufferComparer
    {
        #region Constants

        /// <summary>
        /// Gets the number of octets that will be processed when calculating a hashcode.
        /// </summary>
        public const int DefaultHashCodeFidelity = 512;

        #endregion

        #region Array

        /// <summary>
        /// Gets the default instance of the <see cref="Byte[]"/> buffer comparer that uses FNV with default fidelity.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<byte[]> Default { get; } = new ArrayBufferComparer(DefaultHashCodeFidelity);

        #endregion

        #region Span

        /// <summary>
        /// Gets the default instance of the <see cref="ReadOnlySpan{T}"/> buffer comparer that uses FNV with default fidelity.
        /// This also supports comparison of <see cref="byte[]"/> and <see cref="ArraySegment{T}"/> due to their implicit conversion to <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<ReadOnlySpan<byte>> DefaultSpan { get; } = new SpanBufferComparer(DefaultHashCodeFidelity);

        #endregion

        #region Methods

        /// <summary>
        /// Compare the contexts of two <see cref="ReadOnlySpan{T}"/> buffers.
        /// </summary>
        /// <param name="x">Span 1</param>
        /// <param name="y">Span 2</param>
        /// <returns></returns>
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareSpan(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y)
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

        /// <summary>
        /// Compare the contents of two <see cref="Byte[]"/> buffers.
        /// </summary>
        /// <param name="x">Buffer 1</param>
        /// <param name="y">Buffer 2</param>
        /// <returns></returns>
        [SecuritySafeCritical]
        public static int CompareArray(byte[] x, byte[] y)
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

        /// <summary>
        /// Compare the contexts of two <see cref="ArraySegment{T}{T}"/> buffers.
        /// </summary>
        /// <param name="x">Span 1</param>
        /// <param name="y">Span 2</param>
        /// <returns></returns>
        [SecuritySafeCritical]
        public static int CompareArraySegment(ArraySegment<byte> x, ArraySegment<byte> y)
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

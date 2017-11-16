#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
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
        /// The prefix of octets processed when calculating a hashcode.
        /// </summary>
        public const int DefaultHashCodeFidelity = 512;

        /// <summary>
        /// Gets the default instance of the <see cref="Byte[]"/> buffer comparer that uses FNV with default fidelity.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<byte[]> Array { get; } = new ArrayBufferComparer(DefaultHashCodeFidelity);

        /// <summary>
        /// Gets the default instance of the <see cref="ReadOnlyMemory{T}"/> buffer comparer that uses FNV with default fidelity.
        /// This also supports comparison of <see cref="byte[]"/> and <see cref="ArraySegment{T}"/> due to their implicit conversion to <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<ReadOnlyMemory<byte>> Memory { get; } = new MemoryBufferComparer(DefaultHashCodeFidelity);

        #endregion

        #region Methods

        /// <summary>
        /// Compare the contexts of two <see cref="ReadOnlySpan{T}"/> buffers.
        /// </summary>
        /// <param name="x">Span 1</param>
        /// <param name="y">Span 2</param>
        /// <returns></returns>
        [SecuritySafeCritical]
        public static int CompareSpan(in ReadOnlySpan<byte> x, in ReadOnlySpan<byte> y)
        {
            // From https://github.com/dotnet/corefx/blob/master/src/System.Memory/src/System/ReadOnlySpan.cs
            // public static bool operator ==
            // Returns true if left and right point at the same memory and have the same length.
            // Note that this does *not* check to see if the *contents* are equal.
            if (x == y) return 0;

            var cmp = x.Length.CompareTo(y.Length);
            if (cmp != 0) return cmp; // ([n], [m])

            // ([n], [n])
            switch (x.Length)
            {
                case 0:
                    return 0;

                case 1:
                    cmp = x[0].CompareTo(y[0]);
                    return cmp;

                case 2:
                    cmp = x[0].CompareTo(y[0]);
                    if (cmp != 0) return cmp;

                    cmp = x[1].CompareTo(y[1]);
                    return cmp;

                case 3:
                    cmp = x[0].CompareTo(y[0]);
                    if (cmp != 0) return cmp;

                    cmp = x[1].CompareTo(y[1]);
                    if (cmp != 0) return cmp;

                    cmp = x[2].CompareTo(y[2]);
                    return cmp;

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
        /// Compare the contexts of two <see cref="ReadOnlyMemory{T}"/> buffers.
        /// </summary>
        /// <param name="x">Memory 1</param>
        /// <param name="y">Memory 2</param>
        /// <returns></returns>
        public static int CompareMemory(in ReadOnlyMemory<byte> x, in ReadOnlyMemory<byte> y)
            => CompareSpan(x.Span, y.Span);

        /// <summary>
        /// Compare the contents of two <see cref="Byte[]"/> buffers.
        /// </summary>
        /// <param name="x">Buffer 1</param>
        /// <param name="y">Buffer 2</param>
        /// <returns></returns>
        public static int CompareArray(in byte[] x, in byte[] y)
        {
            if (ReferenceEquals(x, y)) return 0; // (null, null) or (x, x)
            if (x == null) return -1; // (null, y)
            if (y == null) return 1; // (x, null)

            return CompareSpan(x, y);
        }

        #endregion
    }
}

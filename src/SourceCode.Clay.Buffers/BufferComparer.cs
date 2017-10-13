#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

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
        /// The prefix of octets processed when calculating a hashcode.
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
        public static BufferComparer<byte[]> Array { get; } = new ArrayBufferComparer(DefaultHashCodeFidelity);

        #endregion

        #region Memory

        /// <summary>
        /// Gets the default instance of the <see cref="ReadOnlyMemory{T}"/> buffer comparer that uses FNV with default fidelity.
        /// This also supports comparison of <see cref="byte[]"/> and <see cref="ArraySegment{T}"/> due to their implicit conversion to <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<ReadOnlyMemory<byte>> Memory { get; } = new MemoryBufferComparer(DefaultHashCodeFidelity);

        #endregion

        #region Helpers

        /// <summary>
        /// Compare the contexts of two <see cref="ReadOnlyMemory{T}"/> buffers.
        /// </summary>
        /// <param name="x">Memory 1</param>
        /// <param name="y">Memory 2</param>
        /// <returns></returns>
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareMemory(ReadOnlyMemory<byte> x, ReadOnlyMemory<byte> y)
        {
            if (x.IsEmpty)
            {
                if (y.IsEmpty) return 0; // (null, null)
                return -1; // (null, y)
            }
            if (y.IsEmpty) return 1; // (x, null)

            var cmp = CompareSpan(x.Span, y.Span);
            return cmp;
        }

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
            // Returns true if left and right point at the same memory and have the same length.
            // Note that this does *not* check to see if the *contents* are equal.
            if (x == y) return 0;

            if (x.IsEmpty)
            {
                if (y.IsEmpty) return 0; // (null, null)
                return -1; // (null, y)
            }
            if (y.IsEmpty) return 1; // (x, null)

            if (x.Length < y.Length) return -1; // (m, n)
            if (x.Length > y.Length) return 1;

            switch (x.Length) // (n, n)
            {
                // (0, 0)
                case 0: return 0;

                // (m[0], n[0])
                case 1:
                    if (x[0] < y[0]) return -1;
                    if (x[0] > y[0]) return 1;
                    return 0;

                // (m[0..N], n[0..N])
                default:
                    {
                        unsafe
                        {
                            fixed (byte* xp = &x.DangerousGetPinnableReference())
                            fixed (byte* yp = &y.DangerousGetPinnableReference())
                            {
                                var cmp = NativeMethods.MemCompare(xp, yp, x.Length);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareArray(byte[] x, byte[] y)
        {
            if (ReferenceEquals(x, y)) return 0; // (null, null) or (x, x)
            if (x == null) return -1; // (null, y)
            if (y == null) return 1; // (x, null)

            if (x.Length < y.Length) return -1; // (m, n)
            if (x.Length > y.Length) return 1;

            switch (x.Length) // (n, n)
            {
                // (0, 0)
                case 0: return 0;

                // (m[0], n[0])
                case 1:
                    if (x[0] < y[0]) return -1;
                    if (x[0] > y[0]) return 1;
                    return 0;

                // (m[0..N], n[0..N])
                default:
                    {
                        unsafe
                        {
                            fixed (byte* xp = x, yp = y)
                            {
                                var cmp = NativeMethods.MemCompare(xp, yp, x.Length);
                                return cmp;
                            }
                        }
                    }
            }
        }

        #endregion
    }
}

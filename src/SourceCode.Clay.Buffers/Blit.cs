#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents additional blit methods.
    /// </summary>
    public static class Blit
    {
        /// <summary>
        /// Rotates the specified <see cref="byte"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateLeft(byte value, byte bits)
        {
            var b = bits & 7; // mod 8

            // Intrinsic not available for byte/ushort
            return (byte)((value << b) | (value >> (8 - b)));
        }

        /// <summary>
        /// Rotates the specified <see cref="byte"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateRight(byte value, byte bits)
        {
            var b = bits & 7; // mod 8

            // Intrinsic not available for byte/ushort
            return (byte)((value >> b) | (value << (8 - b)));
        }

        /// <summary>
        /// Rotates the specified <see cref="ushort"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateLeft(ushort value, byte bits)
        {
            var b = bits & 15; // mod 16

            // Intrinsic not available for byte/ushort
            return (ushort)((value << b) | (value >> (16 - b)));
        }

        /// <summary>
        /// Rotates the specified <see cref="ushort"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateRight(ushort value, byte bits)
        {
            var b = bits & 15; // mod 16

            // Intrinsic not available for byte/ushort
            return (ushort)((value >> b) | (value << (16 - b)));
        }

        /// <summary>
        /// Rotates the specified <see cref="uint"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, byte bits)
        {
            var b = bits & 31; // mod 32

            // Will compile to instrinsic if pattern complies (uint/ulong):
            // https://github.com/dotnet/coreclr/pull/1830
            return (value << b) | (value >> (32 - b));
        }

        /// <summary>
        /// Rotates the specified <see cref="uint"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(uint value, byte bits)
        {
            var b = bits & 31; // mod 32

            // Will compile to instrinsic if pattern complies (uint/ulong):
            // https://github.com/dotnet/coreclr/pull/1830
            return (value >> b) | (value << (32 - b));
        }

        /// <summary>
        /// Rotates the specified <see cref="ulong"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(ulong value, byte bits)
        {
            var b = bits & 63; // mod 64

            // Will compile to instrinsic if pattern complies (uint/ulong):
            // https://github.com/dotnet/coreclr/pull/1830
            return (value << b) | (value >> (64 - b));
        }

        /// <summary>
        /// Rotates the specified <see cref="ulong"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateRight(ulong value, byte bits)
        {
            var b = bits & 63; // mod 64

            // Will compile to instrinsic if pattern complies (uint/ulong):
            // https://github.com/dotnet/coreclr/pull/1830
            return (value >> b) | (value << (64 - b));
        }

        private static readonly byte[] s_deBruijn = new byte[32]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of Math.Floor(Math.Log(<paramref name="n"/>, 2)).
        /// </summary>
        /// <param name="n">The value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static byte FloorLog2(int n)
        {
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));

            // Uses de Bruijn
            // https://stackoverflow.com/questions/15967240/fastest-implementation-of-log2int-and-log2float

            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;

            const uint c = 0x_07C4_ACDD;
            var u = (uint)(n * c) >> 27;

            var result = s_deBruijn[u];
            return result;
        }
    }
}

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
        public static byte RotateLeft(in byte value, in byte bits)
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
        public static byte RotateRight(in byte value, in byte bits)
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
        public static ushort RotateLeft(in ushort value, in byte bits)
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
        public static ushort RotateRight(in ushort value, in byte bits)
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
        public static uint RotateLeft(in uint value, in byte bits)
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
        public static uint RotateRight(in uint value, in byte bits)
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
        public static ulong RotateLeft(in ulong value, in byte bits)
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
        public static ulong RotateRight(in ulong value, in byte bits)
        {
            var b = bits & 63; // mod 64

            // Will compile to instrinsic if pattern complies (uint/ulong):
            // https://github.com/dotnet/coreclr/pull/1830
            return (value >> b) | (value << (64 - b));
        }

        private static readonly byte[] s_deBruijn32 = new byte[32]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte FloorLog2Impl(in uint value)
        {
            // Perf: Do not use guard clauses; callers must be trusted

            // Uses de Bruijn (many sources)
            // https://stackoverflow.com/questions/15967240/fastest-implementation-of-log2int-and-log2float
            // https://gist.github.com/mburbea/c9a71ac1b1a25762c38c9fee7de0ddc2

            var val = value;

            val |= val >> 01;
            val |= val >> 02;
            val |= val >> 04;
            val |= val >> 08;
            val |= val >> 16;

            const uint c = 0x_07C4_ACDD;
            var i = (val * c) >> 27;

            var floor = s_deBruijn32[i];
            return floor;
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of Math.Floor(Math.Log(<paramref name="value"/>, 2)).
        /// </summary>
        /// <param name="value">The <see cref="uint"/> value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static byte FloorLog2(in uint value)
        {
            if (value == 0) throw new ArgumentOutOfRangeException(nameof(value));
            if (value == uint.MaxValue) return 31; // Math.Log(0xFFFF_FFFF, 2) == 31.999999999664098

            return FloorLog2Impl(value);
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of Math.Floor(Math.Log(<paramref name="value"/>, 2)).
        /// </summary>
        /// <param name="value">The <see cref="int"/> value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static byte FloorLog2(in int value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            if (value == int.MaxValue) return 30; // Math.Log(0x7FFF_FFFF, 2) == 30.999999999328196

            return FloorLog2Impl((uint)value);
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of Math.Floor(Math.Log(<paramref name="value"/>, 2)).
        /// </summary>
        /// <param name="value">The <see cref="ulong"/> value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static byte FloorLog2(in ulong value)
        {
            if (value == 0) throw new ArgumentOutOfRangeException(nameof(value));
            if (value == ulong.MaxValue) return 64;

            // Heuristic: smaller numbers are more likely
            if (value <= uint.MaxValue) // 0xFFFF_FFFF
            {
                return FloorLog2Impl((uint)value);
            }

            var val = (uint)(value >> 32);
            return (byte)(32 + FloorLog2Impl(val));
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of Math.Floor(Math.Log(<paramref name="value"/>, 2)).
        /// </summary>
        /// <param name="value">The <see cref="long"/> value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static byte FloorLog2(in long value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            if (value == long.MaxValue) return 63;

            // Heuristic: smaller numbers are more likely
            if (value <= uint.MaxValue) // 0xFFFF_FFFF
            {
                return FloorLog2Impl((uint)value);
            }

            var val = (uint)(value >> 32);
            return (byte)(32 + FloorLog2Impl(val));
        }
    }
}

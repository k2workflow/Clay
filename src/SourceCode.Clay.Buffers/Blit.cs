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
        private static int FloorLog2Impl(in uint value)
        {
            // Perf: Do not use guard clauses; callers must be trusted

            // Short-circuit lower boundary using optimization trick (n >> 1)
            // 0 (000) => 0 (000) <- n/a, 0 trapped @ callsite
            // 1 (001) => 0 (000) <- good
            // 2 (010) => 1 (001)
            // 3 (011) => 1 (001)
            // 4 (100) => 2 (010)
            // 5 (101) => 2 (010)
            // 6 (110) => 3 (011) <- bad
            if (value <= 5)
                return (int)(value >> 1);

            // Uses de Bruijn (many sources)
            // https://stackoverflow.com/questions/15967240/fastest-implementation-of-log2int-and-log2float
            // https://gist.github.com/mburbea/c9a71ac1b1a25762c38c9fee7de0ddc2

            var val = value;
            val |= val >> 01;
            val |= val >> 02;
            val |= val >> 04;
            val |= val >> 08;
            val |= val >> 16;

            var ix = (val * 0x_07C4_ACDD) >> 27;

            return s_deBruijn32[ix];
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of Math.Floor(Math.Log(<paramref name="value"/>, 2)).
        /// </summary>
        /// <param name="value">The <see cref="uint"/> value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static int FloorLog2(in uint value)
        {
            if (value == 0) throw new ArgumentOutOfRangeException(nameof(value));

            // Short-circuit upper boundary
            // 2^31             = 2,147,483,648
            // uint.MaxValue    = 4,294,967,295
            // 2^32             = 4,294,967,296
            const uint hi = 1U << 31;
            if (value >= hi) return 31;

            return FloorLog2Impl(value);
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of Math.Floor(Math.Log(<paramref name="value"/>, 2)).
        /// </summary>
        /// <param name="value">The <see cref="int"/> value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static int FloorLog2(in int value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));

            // Short-circuit upper boundary
            // 2^30             = 1,073,741,824
            // int.MaxValue     = 2,147,483,647
            // 2^31             = 2,147,483,648
            const int hi = 1 << 30;
            if (value >= hi) return 30;

            return FloorLog2Impl((uint)value);
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of Math.Floor(Math.Log(<paramref name="value"/>, 2)).
        /// </summary>
        /// <param name="value">The <see cref="ulong"/> value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static int FloorLog2(in ulong value)
        {
            if (value == 0) throw new ArgumentOutOfRangeException(nameof(value));

            // Short-circuit upper boundary
            // 2^63             = 9,223,372,036,854,775,808
            // ulong.MaxValue   = 18,446,744,073,709,551,615
            // 2^64             = 18,446,744,073,709,551,616
            const ulong hi = 1UL << 63;
            if (value >= hi) return 63;

            // Heuristic: hot path assumes small numbers more likely
            var val = (uint)value;
            var inc = 0;

            if (value > uint.MaxValue) // 0xFFFF_FFFF
            {
                val = (uint)(value >> 32);
                inc = 32;
            }

            return inc + FloorLog2Impl(val);
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of Math.Floor(Math.Log(<paramref name="value"/>, 2)).
        /// </summary>
        /// <param name="value">The <see cref="long"/> value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static int FloorLog2(in long value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));

            // Short-circuit upper boundary
            // 2^62             = 4,611,686,018,427,387,904
            // long.MaxValue    = 9,223,372,036,854,775,807
            // 2^63             = 9,223,372,036,854,775,808
            const long hi = 1L << 62;
            if (value >= hi) return 62;

            // Heuristic: hot path assumes small numbers more likely
            var val = (uint)value;
            var inc = 0;

            if (value > uint.MaxValue) // 0xFFFF_FFFF
            {
                val = (uint)(value >> 32);
                inc = 32;
            }

            return inc + FloorLog2Impl(val);
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        public static int PopCount(in uint value)
        {
            if (value == 0) return 0;

            // Uses SWAR (SIMD Within A Register)

            const uint c0 = 0x_5555_5555;
            const uint c1 = 0x_3333_3333;
            const uint c2 = 0x_0F0F_0F0F;
            const uint c3 = 0x_0101_0101;

            var val = value;

            val -= (val >> 1) & c0;
            val = (val & c1) + ((val >> 2) & c1);
            val = (val + (val >> 4)) & c2;
            val *= c3;
            val >>= 24; // 32 - 8

            return (int)val;
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        public static int PopCount(in int value) => PopCount((uint)value);

        /// <summary>
        /// Returns the population count (number of bits set) of a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        public static int PopCount(in ulong value)
        {
            if (value == 0) return 0;

            // Uses SWAR (SIMD Within A Register)

            const ulong c0 = 0x_5555_5555_5555_5555;
            const ulong c1 = 0x_3333_3333_3333_3333;
            const ulong c2 = 0x_0F0F_0F0F_0F0F_0F0F;
            const ulong c3 = 0x_0101_0101_0101_0101;

            var val = value;

            val -= (value >> 1) & c0;
            val = (val & c1) + ((val >> 2) & c1);
            val = (val + (val >> 4)) & c2;
            val *= c3;
            val >>= 56; // 64 - 8

            return (int)val;
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        public static int PopCount(in long value) => PopCount((ulong)value);
    }
}

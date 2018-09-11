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
        #region Rotate

        /// <summary>
        /// Rotates the specified <see cref="byte"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateLeft(in byte value, in byte offset)
        {
            var shft = offset & 7; // mod 8 safely ignores boundary checks

            // Intrinsic not available for byte/ushort
            return (byte)((value << shft) | (value >> (8 - shft)));
        }

        /// <summary>
        /// Rotates the specified <see cref="byte"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateRight(in byte value, in byte offset)
        {
            var shft = offset & 7; // mod 8 safely ignores boundary checks

            // Intrinsic not available for byte/ushort
            return (byte)((value >> shft) | (value << (8 - shft)));
        }

        /// <summary>
        /// Rotates the specified <see cref="ushort"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateLeft(in ushort value, in byte offset)
        {
            var shft = offset & 15; // mod 16 safely ignores boundary checks

            // Intrinsic not available for byte/ushort
            return (ushort)((value << shft) | (value >> (16 - shft)));
        }

        /// <summary>
        /// Rotates the specified <see cref="ushort"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateRight(in ushort value, in byte offset)
        {
            var shft = offset & 15; // mod 16 safely ignores boundary checks

            // Intrinsic not available for byte/ushort
            return (ushort)((value >> shft) | (value << (16 - shft)));
        }

        /// <summary>
        /// Rotates the specified <see cref="uint"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(in uint value, in byte offset)
        {
            var shft = offset & 31; // mod 32 safely ignores boundary checks

            // Will compile to instrinsic if pattern complies (uint/ulong):
            // https://github.com/dotnet/coreclr/pull/1830
            return (value << shft) | (value >> (32 - shft));
        }

        /// <summary>
        /// Rotates the specified <see cref="uint"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(in uint value, in byte offset)
        {
            var shft = offset & 31; // mod 32 safely ignores boundary checks

            // Will compile to instrinsic if pattern complies (uint/ulong):
            // https://github.com/dotnet/coreclr/pull/1830
            return (value >> shft) | (value << (32 - shft));
        }

        /// <summary>
        /// Rotates the specified <see cref="ulong"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(in ulong value, in byte offset)
        {
            var shft = offset & 63; // mod 64 safely ignores boundary checks

            // Will compile to instrinsic if pattern complies (uint/ulong):
            // https://github.com/dotnet/coreclr/pull/1830
            return (value << shft) | (value >> (64 - shft));
        }

        /// <summary>
        /// Rotates the specified <see cref="ulong"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateRight(in ulong value, in byte offset)
        {
            var shft = offset & 63; // mod 64 safely ignores boundary checks

            // Will compile to instrinsic if pattern complies (uint/ulong):
            // https://github.com/dotnet/coreclr/pull/1830
            return (value >> shft) | (value << (64 - shft));
        }

        #endregion

        #region FloorLog2

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
            // 0 (000) => 0 (000) ✖ (n/a, 0 trapped @ callsite)
            // 1 (001) => 0 (000) ✔
            // 2 (010) => 1 (001) ✔
            // 3 (011) => 1 (001) ✔
            // 4 (100) => 2 (010) ✔
            // 5 (101) => 2 (010) ✔
            // 6 (110) => 3 (011) ✖ (trick fails)

            if (value <= 5)
                return (int)(value >> 1);

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
        /// It is a fast equivalent of <code>Math.Floor(Math.Log(<paramref name="value"/>, 2))</code>.
        /// </summary>
        /// <param name="value">The value.</param>
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
        /// It is a fast equivalent of <code>Math.Floor(Math.Log(<paramref name="value"/>, 2))</code>.
        /// </summary>
        /// <param name="value">The value.</param>
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
        /// It is a fast equivalent of <code>Math.Floor(Math.Log(<paramref name="value"/>, 2))</code>.
        /// </summary>
        /// <param name="value">The value.</param>
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
        /// It is a fast equivalent of <code>Math.Floor(Math.Log(<paramref name="value"/>, 2))</code>.
        /// </summary>
        /// <param name="value">The value.</param>
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

        #endregion

        #region PopCount

        /// <summary>
        /// Returns the population count (number of bits set) of a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(in uint value)
        {
            // Truth table (1):
            // Short-circuit lower boundary using optimization trick (n+1 >> 1)
            // 0 (000) -> 1 (001) -> 0 (000) ✔
            // 1 (001) -> 2 (010) -> 1 (001) ✔
            // 2 (010) -> 3 (011) -> 1 (001) ✔
            // 3 (011) -> 4 (100) -> 2 (010) ✔
            // 4 (100) -> 5 (101) -> 2 (010) ✖ (trick fails)
            
            if (value <= 3)
                return (int)((value + 1) >> 1);

            // Use a SWAR (SIMD Within A Register) approach

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(in int value) 
            => PopCount((uint)value);

        /// <summary>
        /// Returns the population count (number of bits set) of a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(in ulong value)
        {
            // See truth table (1) above
            if (value <= 3)
                return (int)((value + 1) >> 1);

            // Use a SWAR (SIMD Within A Register) approach

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(in long value) 
            => PopCount((ulong)value);

        #endregion

        #region ReadBit

        /// <summary>
        /// Reads whether the specified bit in a bit mask is set to true.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBit(in uint value, in byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (value & mask) > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Reads whether the specified bit in a bit mask is set to true.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBit(in int value, in byte offset) 
            => ReadBit((uint)value, offset);

        /// <summary>
        /// Reads whether the specified bit in a bit mask is set to true.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBit(in ulong value, in byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            return (value & mask) > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Reads whether the specified bit in a bit mask is set to true.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBit(in long value, in byte offset)
            => ReadBit((ulong)value, offset);

        #endregion

        #region WriteBit

        /// <summary>
        /// Sets the specified bit in a bit mask to true or false.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref uint value, in byte offset, in bool on)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            value = on ? 
                value | mask : 
                value & ~mask;

            return rsp > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Sets the specified bit in a bit mask to true or false.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref int value, in byte offset, in bool on)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var val = (uint)value;
            var rsp = val & mask;

            value = (int)(on ?
                val | mask :
                val & ~mask);

            return rsp > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Sets the specified bit in a bit mask to true or false.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref ulong value, in byte offset, in bool on)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var rsp = value & mask;

            value = on ? 
                value | mask : 
                value & ~mask;

            return rsp > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Sets the specified bit in a bit mask to true or false.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref long value, in byte offset, in bool on)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var val = (ulong)value;
            var rsp = val & mask;

            value = (long)(on ?
                val | mask :
                val & ~mask);

            return rsp > 0; // Cheaper than comparing to mask
        }

        #endregion

        #region FlipBit

        /// <summary>
        /// Negates a single bit in a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlipBit(ref uint value, in byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            // Truth table (2):
            // v   m  | ~m  ^v  ~
            // 00  01 | 10  10  01
            // 01  01 | 10  11  00
            // 10  01 | 10  00  11
            // 11  01 | 10  01  10
            //                      
            // 00  10 | 01  01  10
            // 01  10 | 01  00  11
            // 10  10 | 01  11  00
            // 11  10 | 01  10  01

            value = ~(~mask ^ value);

            return rsp > 0;
        }

        /// <summary>
        /// Negates a single bit in a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FlipBit(in int value, in byte offset)
            => (int)FlipBit((uint)value, offset);

        /// <summary>
        /// Negates a single bit in a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong FlipBit(in ulong value, in byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var rsp = value & mask;

            // See Truth table (2) above
            return ~(~mask ^ value);
        }

        /// <summary>
        /// Negates a single bit in a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long FlipBit(in long value, in byte offset)
            => (long)FlipBit((ulong)value, offset);

        /// <summary>
        /// Negates a single bit in a bit mask.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        //public static void FlipBit(ref Span<byte> value, in int offset)
        //{
        //    var ix = (value.Length << 3) - 1;
        //    if (offset <= ix) ix = offset; // Design choice ignores out-of-range values
        //    ix >>= 3; // div 8

        //    ref byte byt = ref value[ix];

        //    ix = offset & 7; // mod 8
        //    var mask = (byte)(1 << ix);

        //    // See Truth table (2) above
        //    byt = (byte)~(~mask ^ byt);
        //}

        #endregion

        #region BTC

        /// <summary>
        /// Negates a single bit in a bit mask and returns whether it was originally set or not.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExchangeBit(ref uint value, in byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            // See Truth table (2) above
            value = ~(~mask ^ value);

            return rsp > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Negates a single bit in a bit mask and returns whether it was originally set or not.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExchangeBit(ref int value, in byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var val = (uint)value;
            var rsp = val & mask;

            // See Truth table (2) above
            value = (int)(~(~mask ^ val));

            return rsp > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Negates a single bit in a bit mask and returns whether it was originally set or not.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExchangeBit(ref ulong value, in byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var rsp = value & mask;

            // See Truth table (2) above
            value = ~(~mask ^ value);

            return rsp > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Negates a single bit in a bit mask and returns whether it was originally set or not.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExchangeBit(ref long value, in byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var val = (ulong)value;
            var rsp = val & mask;

            // See Truth table (2) above
            value = (long)(~(~mask ^ val));

            return rsp > 0; // Cheaper than comparing to mask
        }

        #endregion

        #region BTS/BTR

        /// <summary>
        /// Sets the specified bit in a bit mask to true or false, and returns whether it was originally set or not.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExchangeBit(ref uint value, in byte offset, in bool on)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            value = on ?
                value | mask :
                value & ~mask;

            return rsp > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Sets the specified bit in a bit mask to true or false, and returns whether it was originally set or not.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExchangeBit(ref int value, in byte offset, in bool on)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var val = (uint)value;
            var rsp = val & mask;

            value = (int)(on ?
                val | mask :
                val & ~mask);

            return rsp > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Sets the specified bit in a bit mask to true or false, and returns whether it was originally set or not.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExchangeBit(ref ulong value, in byte offset, in bool on)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var rsp = value & mask;

            value = on ?
                value | mask :
                value & ~mask;

            return rsp > 0; // Cheaper than comparing to mask
        }

        /// <summary>
        /// Sets the specified bit in a bit mask to true or false, and returns whether it was originally set or not.
        /// </summary>
        /// <param name="value">The bit mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExchangeBit(ref long value, in byte offset, in bool on)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var val = (ulong)value;
            var rsp = val & mask;

            value = (long)(on ?
                val | mask :
                val & ~mask);

            return rsp > 0; // Cheaper than comparing to mask
        }

        #endregion
    }
}

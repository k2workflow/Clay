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
    public static partial class BitOps // .Primitive
    {
        #region ExtractBit

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(in byte value, in byte offset)
        {
            var shft = offset & 7; // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (value & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(in ushort value, in byte offset)
        {
            var shft = offset & 15; // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (value & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(in uint value, in byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (value & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(in ulong value, in byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            return (value & mask) != 0;
        }

        #endregion

        #region InsertBit

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref byte value, in byte offset, in bool on)
        {
            var shft = offset & 7; // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;
            
            value = (byte)(on ?
                value | mask :
                value & ~mask);

            return rsp != 0; // BTS/BTR (inlining should prune if unused)
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref ushort value, in byte offset, in bool on)
        {
            var shft = offset & 15; // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            value = (ushort)(on ?
                value | mask :
                value & ~mask);

            return rsp != 0; // BTS/BTR (inlining should prune if unused)
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref uint value, in byte offset, in bool on)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            value = on ? 
                value | mask : 
                value & ~mask;

            return rsp != 0; // BTS/BTR (inlining should prune if unused)
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref ulong value, in byte offset, in bool on)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var rsp = value & mask;

            value = on ? 
                value | mask : 
                value & ~mask;

            return rsp != 0; // BTS/BTR (inlining should prune if unused)
        }

        #endregion

        #region FlipBit

        /// <summary>
        /// Negates the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlipBit(ref byte value, in byte offset)
        {
            var shft = offset & 7; // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            // See Truth table (2) below
            value = (byte)~(~mask ^ value);

            return rsp != 0; // BTC (inlining should prune if unused)
        }

        /// <summary>
        /// Negates the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlipBit(ref ushort value, in byte offset)
        {
            var shft = offset & 15; // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            // See Truth table (2) below
            value = (ushort)~(~mask ^ value);

            return rsp != 0; // BTC (inlining should prune if unused)
        }

        /// <summary>
        /// Negates the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
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

            return rsp != 0; // BTC (inlining should prune if unused)
        }

        /// <summary>
        /// Negates the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlipBit(ref ulong value, in byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var rsp = value & mask;

            // See Truth table (2) above
            value = ~(~mask ^ value);

            return rsp != 0; // BTC (inlining should prune if unused)
        }

        #endregion

        #region Rotate

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateLeft(in byte value, in byte offset)
        {
            var shft = offset & 7; // mod 8 safely ignores boundary checks
            var val = (uint)value;

            // Intrinsic not available for byte/ushort
            return (byte)((val << shft) | (val >> (8 - shft)));
        }

        /// <summary>
        /// Rotates the specified value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateRight(in byte value, in byte offset)
        {
            var shft = offset & 7; // mod 8 safely ignores boundary checks
            var val = (uint)value;

            // Intrinsic not available for byte/ushort
            return (byte)((val >> shft) | (val << (8 - shft)));
        }

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateLeft(in ushort value, in byte offset)
        {
            var shft = offset & 15; // mod 16 safely ignores boundary checks
            var val = (uint)value;

            // Intrinsic not available for byte/ushort
            return (ushort)((val << shft) | (val >> (16 - shft)));
        }

        /// <summary>
        /// Rotates the specified value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateRight(in ushort value, in byte offset)
        {
            var shft = offset & 15; // mod 16 safely ignores boundary checks
            var val = (uint)value;

            // Intrinsic not available for byte/ushort
            return (ushort)((val >> shft) | (val << (16 - shft)));
        }

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
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
        /// Rotates the specified value right by the specified number of bits.
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
        /// Rotates the specified value left by the specified number of bits.
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
        /// Rotates the specified value right by the specified number of bits.
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

        #region PopCount

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(in byte value)
        {
            // 22 ops
            // TODO: Benchmark whether other algo is faster
            var val
                = (value & 1)
                + (value >> 1 & 1)
                + (value >> 2 & 1)
                + (value >> 3 & 1)
                + (value >> 4 & 1)
                + (value >> 5 & 1)
                + (value >> 6 & 1)
                + (value >> 7 & 1);

            return val;
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(in ushort value)
            => PopCount((uint)value);

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
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

            // Uses a SWAR (SIMD Within A Register) approach

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
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
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

        #endregion

        #region LeadingCount        

        /// <summary>
        /// Count the number of leading bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="on">True to count each 1, or false to count each 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingCount(in byte value, in bool on)
        {
            if (value == 0)
                return on ? 0 : 8;

            if (value == byte.MaxValue)
                return on ? 8 : 0;

            var val = value;
            if (on)
                val = (byte)~val;

            return 7 - FloorLog2Impl(val);
        }

        /// <summary>
        /// Count the number of leading bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="on">True to count each 1, or false to count each 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingCount(in ushort value, in bool on)
        {
            if (value == 0)
                return on ? 0 : 16;

            if (value == ushort.MaxValue)
                return on ? 16 : 0;

            var val = value;
            if (on)
                val = (ushort)~val;

            return 15 - FloorLog2Impl(val);
        }

        /// <summary>
        /// Count the number of leading bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="on">True to count each 1, or false to count each 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingCount(in uint value, in bool on)
        {
            if (value == 0)
                return on ? 0 : 32;

            if (value == uint.MaxValue)
                return on ? 32 : 0;

            var val = on ? ~value : value;

            return 31 - FloorLog2Impl(val);
        }

        /// <summary>
        /// Count the number of leading bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="on">True to count each 1, or false to count each 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingCount(in ulong value, in bool on)
        {
            if (value == 0)
                return on ? 0 : 64;

            if (value == ulong.MaxValue)
                return on ? 64 : 0;

            var val = on ? ~value : value;
            
            return 63 - FloorLog2(val);
        }

        #endregion

        #region TrailingCount

        // eg 64 % 11 = 9. Since 64 = 0100 0000 which has 6 trailing zeros, [9] = 6
        private static readonly byte[] s_trail8u = new byte[11] // mod 11
        {
            8, 0, 1, 8, 2, 4, 9, 7,
            3, 6, 5
        };

        /// <summary>
        /// Count the number of trailing bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="on">True to count each 1, or false to count each 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingCount(in byte value, in bool on)
        {
            uint val = value;
            if (on) val = ~val;

            var lsb = val & -val; // eg 0010 1100 => 44 & -44 = 4
            return s_trail8u[lsb % 11];
        }

        // eg 64 % 19 = 7. Since 64 = 0100 0000 which has 6 trailing zeros, [7] = 6
        private static readonly byte[] s_trail16u = new byte[19] // mod 19
        {
            16, 00, 01, 13, 02, 16, 14, 06,
            03, 08, 17, 12, 15, 05, 07, 11,
            04, 10, 09
        };

        /// <summary>
        /// Count the number of trailing bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="on">True to count each 1, or false to count each 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingCount(in ushort value, in bool on)
        {
            uint val = value;
            if (on)
                val = ~val;

            var lsb = val & -val; // eg 0010 1100 => 44 & -44 = 4
            return s_trail16u[lsb % 19];
        }

        // eg 64 % 37 = 27. Since 64 = 0100 0000 which has 6 trailing zeros, [27] = 6
        private static readonly byte[] s_trail32u = new byte[37] // mod 37
        {
            32, 00, 01, 26, 02, 23, 27, 00,
            03, 16, 24, 30, 28, 11, 00, 13,
            04, 07, 17, 00, 25, 22, 31, 15,
            29, 10, 12, 06, 00, 21, 14, 09,
            05, 20, 08, 19, 18
        };

        /// <summary>
        /// Count the number of trailing bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="on">True to count each 1, or false to count each 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingCount(in uint value, in bool on)
        {
            //if (value == 0)
            //    return on ? 0 : 32;

            if (value == uint.MaxValue)
                return on ? 32 : 0;

            var val = value;
            if (on)
                val = ~val;

            var lsb = val & -val; // eg 0010 1100 => 44 & -44 = 4
            return s_trail32u[lsb % 37];
        }

        // eg 64 % 67 = 27. Since 64 = 0100 0000 which has 6 trailing zeros, [27] = 6
        private static readonly byte[] s_trail64u = new byte[67] // mod 71
        {
            64, 64, 1, 39, 2, 15, 40, 23,
            3, 12, 16, 59, 41, 19, 24, 54,
            4, 0, 13, 10, 17, 62, 60, 28,
            42, 30, 20, 51, 25, 44, 55, 47,
            5, 32, 0, 38, 14, 22, 11, 58,
            18, 53, 63, 9, 61, 27, 29, 50,
            43, 46, 31, 37, 21, 57, 52, 8,
            26, 49, 45, 36, 56, 7, 48, 35,
            6, 34, 33
        };

        /// <summary>
        /// Count the number of trailing bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="on">True to count each 1, or false to count each 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingCount(in ulong value, in bool on)
        {
            if (value == 0)
                return on ? 0 : 64;

            if (value == ulong.MaxValue)
                return on ? 64 : 0;

            if (value > uint.MaxValue)
            {
                if ((uint)value == 0)
                    return 32 + TrailingCount((uint)(value >> 32), on);

                return TrailingCount((uint)(value >> 32), on);
            }

            return TrailingCount((uint)value, on);

            //if (value == ulong.MaxValue)
            //    return on ? 0 : 64;

            //var val = value;
            //if (on)
            //    val = ~val;

            //var lsb = val & (ulong)-(long)val; // eg 0010 1100 => 44 & -44 = 4
            //return s_trail64u[lsb % 67];

            //if (value == 0)
            //    return on ? 0 : 64;

            //if (value == ulong.MaxValue)
            //    return on ? 64 : 0;

            //var val = on ? ~value : value;

            //return s_deBruijn64[((val ^ (val - 1)) * Debruijn64) >> 58];
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

        private const uint Debruijn32 = 0x07C4_ACDDu;

        private static readonly byte[] s_deBruijn64 = new byte[64]
        {
            00, 47, 01, 56, 48, 27, 02, 60,
            57, 49, 41, 37, 28, 16, 03, 61,
            54, 58, 35, 52, 50, 42, 21, 44,
            38, 32, 29, 23, 17, 11, 04, 62,
            46, 55, 26, 59, 40, 36, 15, 53,
            34, 51, 20, 43, 31, 22, 10, 45,
            25, 39, 14, 33, 19, 30, 09, 24,
            13, 18, 08, 12, 07, 06, 05, 63
        };

        private const ulong Debruijn64 = 0x03F7_9D71_B4CB_0A89ul;
    
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
        public static int FloorLog2(in byte value)
        {
            if (value == 0) throw new ArgumentOutOfRangeException(nameof(value));

            // Short-circuit upper boundary
            // 2^7              = 128
            // byte.MaxValue    = 255
            // 2^8              = 256

            const uint hi = 1U << 7;
            if (value >= hi) return 7;

            return FloorLog2Impl(value);
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of <code>Math.Floor(Math.Log(<paramref name="value"/>, 2))</code>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static int FloorLog2(in sbyte value)
        {
            if (value == 0) throw new ArgumentOutOfRangeException(nameof(value));

            // Short-circuit upper boundary
            // 2^6              = 63
            // sbyte.MaxValue   = 127
            // 2^7              = 128

            const uint hi = 1U << 6;
            if (value >= hi) return 6;

            return FloorLog2Impl((uint)value);
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of <code>Math.Floor(Math.Log(<paramref name="value"/>, 2))</code>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static int FloorLog2(in ushort value)
        {
            if (value == 0) throw new ArgumentOutOfRangeException(nameof(value));

            // Short-circuit upper boundary
            // 2^15             = 32,768
            // byte.MaxValue    = 65,535
            // 2^16             = 65,536

            const uint hi = 1U << 15;
            if (value >= hi) return 15;

            return FloorLog2Impl(value);
        }

        /// <summary>
        /// Finds the floor of the base-2 log of the specified value.
        /// It is a fast equivalent of <code>Math.Floor(Math.Log(<paramref name="value"/>, 2))</code>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The floor(log2) of the value.</returns>
        public static int FloorLog2(in short value)
        {
            if (value == 0) throw new ArgumentOutOfRangeException(nameof(value));

            // Short-circuit upper boundary
            // 2^14             = 16,384
            // short.MaxValue   = 32,767
            // 2^15             = 32,768

            const uint hi = 1U << 14;
            if (value >= hi) return 14;

            return FloorLog2Impl((uint)value);
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
    }
}

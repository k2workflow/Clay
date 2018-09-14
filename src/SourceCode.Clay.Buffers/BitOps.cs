#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Diagnostics;
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
        public static bool ExtractBit(byte value, byte offset)
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
        public static bool ExtractBit(ushort value, byte offset)
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
        public static bool ExtractBit(uint value, byte offset)
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
        public static bool ExtractBit(ulong value, byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            return (value & mask) != 0;
        }

        #endregion

        #region ClearBit (Scalar)

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ClearBit(byte value, byte offset)
        {
            var shft = offset & 7; // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (byte)(value & ~mask);
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ClearBit(ushort value, byte offset)
        {
            var shft = offset & 15; // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (ushort)(value & ~mask);
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ClearBit(uint value, byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;

            return value & ~mask;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ClearBit(ulong value, byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            return value & ~mask;
        }

        #endregion

        #region ClearBit (Ref)

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref byte value, byte offset)
        {
            var shft = offset & 7; // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            value = (byte)(value & ~mask);

            return rsp != 0; // BTR
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref ushort value, byte offset)
        {
            var shft = offset & 15; // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            value = (ushort)(value & ~mask);

            return rsp != 0; // BTR
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref uint value, byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            value = value & ~mask;

            return rsp != 0; // BTR
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref ulong value, byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var rsp = value & mask;

            value = value & ~mask;

            return rsp != 0; // BTR
        }

        #endregion

        #region InsertBit (Scalar)

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte InsertBit(byte value, byte offset)
        {
            var shft = offset & 7; // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (byte)(value | mask);
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort InsertBit(ushort value, byte offset)
        {
            var shft = offset & 15; // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (ushort)(value | mask);
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint InsertBit(uint value, byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;

            return value | mask;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong InsertBit(ulong value, byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            return value | mask;
        }

        #endregion

        #region InsertBit (Ref)

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref byte value, byte offset)
        {
            var shft = offset & 7; // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;
            
            value = (byte)(value | mask);

            return rsp != 0; // BTS
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref ushort value, byte offset)
        {
            var shft = offset & 15; // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            value = (ushort)(value | mask);

            return rsp != 0; // BTS
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref uint value, byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            value = value | mask;

            return rsp != 0; // BTS
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref ulong value, byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var rsp = value & mask;

            value = value | mask;

            return rsp != 0; // BTS
        }

        #endregion

        #region ComplementBit (Scalar)

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

        /// <summary>
        /// Complements the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ComplementBit(byte value, byte offset)
        {
            var shft = offset & 7; // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;

            // See Truth table (2) above
            var val = (byte)~(~mask ^ value);
            return val;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ComplementBit(ushort value, byte offset)
        {
            var shft = offset & 15; // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;

            // See Truth table (2) above
            var val = (ushort)~(~mask ^ value);
            return val;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ComplementBit(uint value, byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;

            // See Truth table (2) above
            var val = ~(~mask ^ value);
            return val;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ComplementBit(ulong value, byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            // See Truth table (2) above
            var val = ~(~mask ^ value);
            return val;
        }

        #endregion

        #region ComplementBit (Ref)

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref byte value, byte offset)
        {
            var shft = offset & 7; // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            // See Truth table (2) above
            value = (byte)~(~mask ^ value);

            return rsp != 0; // BTC
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref ushort value, byte offset)
        {
            var shft = offset & 15; // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            // See Truth table (2) above
            value = (ushort)~(~mask ^ value);

            return rsp != 0; // BTC
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref uint value, byte offset)
        {
            var shft = offset & 31; // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;
            var rsp = value & mask;

            // See Truth table (2) above
            value = ~(~mask ^ value);

            return rsp != 0; // BTC
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref ulong value, byte offset)
        {
            var shft = offset & 63; // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;
            var rsp = value & mask;

            // See Truth table (2) above
            value = ~(~mask ^ value);

            return rsp != 0; // BTC
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
        public static byte RotateLeft(byte value, byte offset)
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
        public static byte RotateRight(byte value, byte offset)
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
        public static ushort RotateLeft(ushort value, byte offset)
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
        public static ushort RotateRight(ushort value, byte offset)
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
        public static uint RotateLeft(uint value, byte offset)
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
        public static uint RotateRight(uint value, byte offset)
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
        public static ulong RotateLeft(ulong value, byte offset)
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
        public static ulong RotateRight(ulong value, byte offset)
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
        public static int PopCount(byte value)
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
        public static int PopCount(ushort value)
            => PopCount((uint)value);

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(uint value)
        {
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
            val >>= 24;

            return (int)val;
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(ulong value)
        {
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
            val >>= 56;

            return (int)val;
        }

        #endregion

        #region LeadingCount

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(byte value)
            // FloorLog2 does not handle 0
            => value == 0 
            ? 8 
            : 7 - FloorLog2Impl(value);

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(byte value)
        {
            // FloorLog2 does not handle 0
            if (value == 0)
                return 0;

            // Negation of Max == 0; see above
            if (value == byte.MaxValue)
                return 8;

            // Negate mask but remember to truncate carry-bits
            var val = (uint)(byte)~(uint)value;

            return 7 - FloorLog2Impl(val);
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(ushort value)
            // FloorLog2 does not handle 0
            => value == 0 
            ? 16 
            : 15 - FloorLog2Impl(value);

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(ushort value)
        {
            // FloorLog2 does not handle 0
            if (value == 0)
                return 0;

            // Negation of Max == 0; see above
            if (value == ushort.MaxValue)
                return 16;

            // Negate mask but remember to truncate carry-bits
            var val = (uint)(ushort)~(uint)value;

            return 15 - FloorLog2Impl(val);
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(uint value)
            // FloorLog2 does not handle 0
            => value == 0 
            ? 32 
            : 31 - FloorLog2Impl(value);

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(uint value)
        {
            // FloorLog2 does not handle 0
            if (value == 0)
                return 0;

            // Negation of Max == 0; see above
            if (value == uint.MaxValue)
                return 32;

            // Negate mask but remember to truncate carry-bits
            var val = ~value;

            return 31 - FloorLog2Impl(val);
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(ulong value)
            // FloorLog2 does not handle 0
            => value == 0 
            ? 64 
            : 63 - FloorLog2Impl(value);

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(ulong value)
        {
            // FloorLog2 does not handle 0
            if (value == 0)
                return 0;

            // Negation of Max == 0; see above
            if (value == ulong.MaxValue)
                return 64;

            // Negate mask but remember to truncate carry-bits
            var val = ~value;

            return 63 - FloorLog2Impl(val);
        }

        #endregion

        #region TrailingCount

        // Build this table by taking n = 0,1,2,4,...,512
        // [2^n % 11] = tz(n)
        private static readonly byte[] s_trail8u = new byte[11] // mod 11
        {
            //    2^n  % 11     b=bin(n)   z=tz(b)
            8, //   0  [ 0]     0000_0000  8
            0, //   1  [ 1]     0000_0001  0 
            1, //   2  [ 2]     0000_0010  1
            8, // 256  [ 3]  01_0000_0000  8 (n/a) 1u << 8
               
            2, //   4  [ 4]     0000_0100  2
            4, //  16  [ 5]     0001_0000  4
            9, // 512  [ 6]  10_0000_0000  9 (n/a) 1u << 9
            7, // 128  [ 7]     1000_0000  7
               
            3, //   8  [ 8]     0000_1000  3
            6, //  64  [ 9]     0100_0000  6
            5, //  32  [10]     0010_0000  5
        };

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(byte value)
        {
            var val = value;

            // The expression (n & -n) returns lsb(n).
            // Only possible values are therefore [0,1,2,4,...,128]
            var lsb = val & -val; // eg 44==0010 1100 -> (44 & -44) -> 4. 4==0100, which is the lsb of 44.

            // We want to map [0...128] to the smallest contiguous range, ideally [0..9] since 9 is the range cardinality.
            // Mod-11 is a simple perfect-hashing scheme over this range, where 11 is chosen as the closest prime greater than 9.
            lsb = lsb % 11; // eg 44 -> 4 % 11 -> 4

            // NoOp: Hashing scheme has unused outputs (inputs 256 and higher do not fit a byte)
            Debug.Assert(!(lsb == 3 || lsb == 6), $"{nameof(TrailingZeros)}({value}) resulted in unexpected {typeof(byte)} hash {lsb}");

            // TODO: For such a small range, would a switch be faster?
            var cnt = s_trail8u[lsb]; // eg 44 -> 4 -> 2 (44==0010 1100 has 2 trailing zeros)
            return cnt;
        }

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(byte value)
        {
            // Negate mask but remember to truncate carry-bits
            var val = (uint)(byte)~(uint)value;

            // The expression (n & -n) returns lsb(n).
            // Only possible values are therefore [0,1,2,4,...,128]
            var lsb = val & -val; // eg 44==0010 1100 -> (44 & -44) -> 4. 4==0100, which is the lsb of 44.

            // We want to map [0...128] to the smallest contiguous range, ideally [0..9] since 9 is the range cardinality.
            // Mod-11 is a simple perfect-hashing scheme over this range, where 11 is chosen as the closest prime greater than 9.
            lsb = lsb % 11; // eg 44 -> 4 % 11 -> 4

            // NoOp: Hashing scheme has unused outputs (inputs 256 and higher do not fit a byte)
            Debug.Assert(!(lsb == 3 || lsb == 6), $"{nameof(TrailingOnes)}({value}) resulted in unexpected {typeof(byte)} hash {lsb}");

            // TODO: For such a small range, would a switch be faster?
            var cnt = s_trail8u[lsb]; // eg 44 -> 4 -> 2 (44==0010 1100 has 2 trailing zeros)
            return cnt;
        }

        // See algorithm notes in TrailingCount(byte)
        private static readonly byte[] s_trail16u = new byte[19] // mod 19
        {
            //        2^n  % 19     b=bin(n)             z=tz(b)
            16, //      0  [ 0]     0000_0000_0000_0000  16
            00, //      1  [ 1]     0000_0000_0000_0001   0
            01, //      2  [ 2]     0000_0000_0000_0010   1
            13, //   8192  [ 3]     0010_0000_0000_0000  13

            02, //      4  [ 4]     0000_0000_0000_0100   2
            16, //  65536  [ 5]  01_0000_0000_0000_0000  16 (n/a) 1u << 16
            14, //  16384  [ 6]     0100_0000_0000_0000  14
            06, //     64  [ 7]     0000_0000_0100_0000   6

            03, //      8  [ 8]     0000_0000_0000_1000   3
            08, //    256  [ 9]     0000_0001_0000_0000   8
            17, // 131072  [10]  10_0000_0000_0000_0000  17 (n/a) 1u << 17
            12, //   4096  [11]     0001_0000_0000_0000  12

            15, //  32768  [12]     1000_0000_0000_0000  15
            05, //     32  [13]     0000_0000_0010_0000   5
            07, //    128  [14]     0000_0000_1000_0000   7
            11, //   2048  [15]     0000_1000_0000_0000  11

            04, //     16  [16]     0000_0000_0001_0000   4
            10, //   1024  [17]     0000_0100_0000_0000  10
            09  //    512  [18]     0000_0010_0000_0000   9
        };

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="ones">True to count ones, or false to count zeros.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(ushort value)
        {
            var val = value;

            // See algorithm notes in TrailingZeros(byte)
            // 19 is the closest prime greater than the range's cardinality of 17.
            var lsb = val & -val;
            lsb = lsb % 19; // mod 19

            // NoOp: Hashing scheme has unused outputs (inputs 65536 and higher do not fit a ushort)
            Debug.Assert(!(lsb == 5 || lsb == 10), $"{nameof(TrailingZeros)}({value}) resulted in unexpected {typeof(ushort)} hash {lsb}");

            var cnt = s_trail16u[lsb];
            return cnt;
        }

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="ones">True to count ones, or false to count zeros.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(ushort value)
        {
            // Negate mask but remember to truncate carry-bits
            var val = (uint)(ushort)~(uint)value;

            // See algorithm notes in TrailingOnes(byte)
            // 19 is the closest prime greater than the range's cardinality of 17.
            var lsb = val & -val;
            lsb = lsb % 19; // mod 19

            // NoOp: Hashing scheme has unused outputs (inputs 65536 and higher do not fit a ushort)
            Debug.Assert(!(lsb == 5 || lsb == 10), $"{nameof(TrailingOnes)}({value}) resulted in unexpected {typeof(ushort)} hash {lsb}");

            var cnt = s_trail16u[lsb];
            return cnt;
        }

        // See algorithm notes in TrailingCount(byte)
        private static readonly byte[] s_trail32u = new byte[37] // mod 37
        {
            //                2^n  % 37       b=bin(n)                                 z=tz(b)
            32, //              0  [ 0]       0000_0000_0000_0000_0000_0000_0000_0000  32
            00, //              1  [ 1]       0000_0000_0000_0000_0000_0000_0000_0001   0
            01, //              2  [ 2]       0000_0000_0000_0000_0000_0000_0000_0010   1
            26,

            02, //              4  [ 4]       0000_0000_0000_0000_0000_0000_0000_0100   2
            23,
            27,                  
            32, //  4,294,967,296  [ 7]  0001_0000_0000_0000_0000_0000_0000_0000_0000  32 (n/a) 1ul << 32

            03, //              8  [ 8]       0000_0000_0000_0000_0000_0000_0000_1000   3
            16,
            24,
            30,

            28,
            11, //           2048  [13]       0000_0000_0000_0000_0000_1000_0000_0000  11
            33, //  8,589,934,592  [14]  0010_0000_0000_0000_0000_0000_0000_0000_0000  33 (n/a) 1ul << 33
            13,

            04, //             16  [16]       0000_0000_0000_0000_0000_0000_0001_0000   4
            07, //            128  [17]       0000_0000_0000_0000_0000_0000_1000_0000   7
            17,
            35, // 34,359,738,368  [19]  1000_0000_0000_0000_0000_0000_0000_0000_0000  35 (n/a) 1ul << 35

            25,
            22,
            31,
            15, //           8192  [15]       0000_0000_0000_0000_0010_0000_0000_0000  13

            29,
            10, //           1024  [25]       0000_0000_0000_0000_0000_0100_0000_0000  10
            12, //           4096  [26]       0000_0000_0000_0000_0001_0000_0000_0000  12
            06, //             64  [27]       0000_0000_0000_0000_0000_0000_0100_0000   6

            34, // 17,179,869,184  [28]  0100_0000_0000_0000_0000_0000_0000_0000_0000  34 (n/a) 1ul << 34
            21,
            14,
            09, //            512  [31]       0000_0000_0000_0000_0000_0010_0000_0000   9

            05, //             32  [32]       0000_0000_0000_0000_0000_0000_0010_0000   5
            20,
            08, //            256  [34]       0000_0000_0000_0000_0000_0001_0000_0000   8
            19,

            18  //        262,144  [36]
        };

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(uint value)
        {
            var val = value;

            // See algorithm notes in TrailingZeros(byte)
            // 37 is the closest prime greater than the range's cardinality of 33.
            var lsb = val & -val;
            lsb = lsb % 37; // mod 37

            // NoOp: Hashing scheme has unused outputs (inputs 4,294,967,296 and higher do not fit a uint)
            Debug.Assert(!(lsb == 7 || lsb == 14 || lsb == 19 || lsb == 28), $"{nameof(TrailingZeros)}({value}) resulted in unexpected {typeof(uint)} hash {lsb}");

            var cnt = s_trail32u[lsb];
            return cnt;
        }

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(uint value)
        {
            // Negate mask
            var val = ~value;

            // See algorithm notes in TrailingOnes(byte)
            // 37 is the closest prime greater than the range's cardinality of 33.
            var lsb = val & -val;
            lsb = lsb % 37; // mod 37

            // NoOp: Hashing scheme has unused outputs (inputs 4,294,967,296 and higher do not fit a uint)
            Debug.Assert(!(lsb == 7 || lsb == 14 || lsb == 19 || lsb == 28), $"{nameof(TrailingOnes)}({value}) resulted in unexpected {typeof(uint)} hash {lsb}");

            var cnt = s_trail32u[lsb];
            return cnt;
        }

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(ulong value)
        {
            // 0 is a special case since calling into the uint overload
            // will return 32 instead of 64
            if (value == 0)
                return 64;

            // We only have to count the low-32 or the high-32, depending on limits

            // Assume we need only examine low-32
            var val = (uint)value;
            var inc = 0;

            // If high-32 is non-zero and low-32 is zero
            if (value > uint.MaxValue && val == 0)
            {
                // Then we need only examine high-32 (and add 32 to the result)
                val = (uint)(value >> 32); // Use high-32 instead
                inc = 32;
            }

            // Examine 32
            return inc + TrailingZeros(val);
        }

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(ulong value)
        {
            // We only have to count the low-32 or the high-32, depending on limits

            // Assume we need only examine low-32
            var val = (uint)value;
            var inc = 0;

            // If high-32 is non-zero and low-32 is Max
            if (value > uint.MaxValue && val == uint.MaxValue)
            {
                // Then we need only examine high-32 (and add 32 to the result)
                val = (uint)(value >> 32); // Use high-32 instead
                inc = 32;
            }

            // Examine 32
            return inc + TrailingOnes(val);
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
        internal static int FloorLog2Impl(uint value)
        {
            // Perf: Do not use guard clauses; callers must be trusted
            Debug.Assert(value > 0);

            var val = value;
            val |= val >> 01;
            val |= val >> 02;
            val |= val >> 04;
            val |= val >> 08;
            val |= val >> 16;

            const uint c32 = 0x07C4_ACDDu;
            var ix = (val * c32) >> 27;

            return s_deBruijn32[ix];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int FloorLog2Impl(ulong value)
        {
            // Perf: Do not use guard clauses; callers MUST be trusted
            Debug.Assert(value > 0);

            // We only have to count the low-32 or the high-32, depending on limits

            // Assume we need only examine low-32
            var val = (uint)value;
            var inc = 0;

            // If high-32 is non-zero
            if (value > uint.MaxValue)
            {
                // Then we need only examine high-32 (and add 32 to the result)
                val = (uint)(value >> 32); // Use high-32 instead
                inc = 32;
            }

            // Examine 32
            return inc + FloorLog2Impl(val);
        }

        #endregion
    }
}

#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Buffers
{
    partial class BitOps // .Span
    {
        #region ExtractBit

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<byte> value, uint offset)
        {
            var ix = (int)(offset >> 3); // div 8
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 7); // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (value[ix] & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<ushort> value, uint offset)
        {
            var ix = (int)(offset >> 4); // div 16
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            
            var shft = (byte)(offset & 15); // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (value[ix] & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<uint> value, uint offset)
        {
            var ix = (int)(offset >> 5); // div 32
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            
            var shft = (byte)(offset & 31); // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;

            return (value[ix] & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<ulong> value, uint offset)
        {
            var ix = (int)(offset >> 6); // div 64
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 63); // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            return (value[ix] & mask) != 0;
        }

        #endregion

        #region ClearBit

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<byte> value, uint offset)
        {
            var ix = (int)(offset >> 3); // div 8
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 7); // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;

            ref byte val = ref value[ix];
            var rsp = val & mask;

            val = (byte)(val & ~mask);

            return rsp != 0; // BTR
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<ushort> value, uint offset)
        {
            var ix = (int)(offset >> 4); // div 16
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 15); // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;

            ref ushort val = ref value[ix];
            var rsp = val & mask;

            val = (ushort)(val & ~mask);

            return rsp != 0; // BTR
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<uint> value, uint offset)
        {
            var ix = (int)(offset >> 5); // div 32
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 31); // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;

            ref uint val = ref value[ix];
            var rsp = val & mask;

            val = val & ~mask;

            return rsp != 0; // BTR
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<ulong> value, uint offset)
        {
            var ix = (int)(offset >> 6); // div 64
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 63); // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            ref ulong val = ref value[ix];
            var rsp = val & mask;

            val = val & ~mask;

            return rsp != 0; // BTR
        }

        #endregion

        #region InsertBit

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<byte> value, uint offset)
        {
            var ix = (int)(offset >> 3); // div 8
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            
            var shft = (byte)(offset & 7); // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;

            ref byte val = ref value[ix];
            var rsp = val & mask;

            val = (byte)(val | mask);

            return rsp != 0; // BTS
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<ushort> value, uint offset)
        {
            var ix = (int)(offset >> 4); // div 16
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 15); // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;

            ref ushort val = ref value[ix];
            var rsp = val & mask;

            val = (ushort)(val | mask);

            return rsp != 0; // BTS
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<uint> value, uint offset)
        {
            var ix = (int)(offset >> 5); // div 32
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 31); // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;

            ref uint val = ref value[ix];
            var rsp = val & mask;

            val = val | mask;

            return rsp != 0; // BTS
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<ulong> value, uint offset)
        {
            var ix = (int)(offset >> 6); // div 64
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 63); // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            ref ulong val = ref value[ix];
            var rsp = val & mask;

            val = val | mask;

            return rsp != 0; // BTS
        }

        #endregion

        #region ComplementBit

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<byte> value, uint offset)
        {
            var ix = (int)(offset >> 3); // div 8
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 7); // mod 8: design choice ignores out-of-range values
            var mask = 1U << shft;

            ref byte val = ref value[ix];
            var rsp = val & mask;

            val = (byte)~(~mask ^ val);

            return rsp != 0; // BTC
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<ushort> value, uint offset)
        {
            var ix = (int)(offset >> 4); // div 16
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 15); // mod 16: design choice ignores out-of-range values
            var mask = 1U << shft;

            ref ushort val = ref value[ix];
            var rsp = val & mask;

            val = (ushort)~(~mask ^ val);

            return rsp != 0; // BTC
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<uint> value, uint offset)
        {
            var ix = (int)(offset >> 5); // div 32
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 31); // mod 32: design choice ignores out-of-range values
            var mask = 1U << shft;

            ref uint val = ref value[ix];
            var rsp = val & mask;

            val = ~(~mask ^ val);

            return rsp != 0; // BTC
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<ulong> value, uint offset)
        {
            var ix = (int)(offset >> 6); // div 64
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 63); // mod 64: design choice ignores out-of-range values
            var mask = 1UL << shft;

            ref ulong val = ref value[ix];
            var rsp = val & mask;

            val = ~(~mask ^ val);

            return rsp != 0; // BTC
        }

        #endregion

        #region PopCount

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long PopCount(ReadOnlySpan<byte> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var sum = 0L;

            for (var i = 0; i < value.Length; i++)
            {
                sum += PopCount(value[i]);
            }

            return sum;
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long PopCount(ReadOnlySpan<ushort> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var sum = 0L;

            for (var i = 0; i < value.Length; i++)
            {
                sum += PopCount(value[i]);
            }

            return sum;
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long PopCount(ReadOnlySpan<uint> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var sum = 0L;

            for (var i = 0; i < value.Length; i++)
            {
                sum += PopCount(value[i]);
            }

            return sum;
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long PopCount(ReadOnlySpan<ulong> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var sum = 0L;

            for (var i = 0; i < value.Length; i++)
            {
                sum += PopCount(value[i]);
            }

            return sum;
        }

        #endregion

        #region LeadingCount

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingZeros(ReadOnlySpan<byte> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            if (value[ix] == byte.MaxValue)
                return 0;

            return 7 - FloorLog2Impl(value[ix]) + (ix << 3); // mul 8
        }

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingOnes(ReadOnlySpan<byte> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            if (value[ix] == byte.MaxValue)
                return 8;

            // Negate mask but remember to truncate carry-bits
            var val = (uint)(byte)~(uint)value[ix];

            return 7 - FloorLog2Impl(val) + (ix << 3); // mul 8
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingZeros(ReadOnlySpan<ushort> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            if (value[ix] == ushort.MaxValue)
                return 0;

            return 15 - FloorLog2Impl(value[ix]) + (ix << 4); // mul 16
        }

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingOnes(ReadOnlySpan<ushort> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            if (value[ix] == ushort.MaxValue)
                return 16;

            // Negate mask but remember to truncate carry-bits
            var val = (uint)(ushort)~(uint)value[ix];

            return 15 - FloorLog2Impl(val) + (ix << 4); // mul 16
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingZeros(ReadOnlySpan<uint> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            if (value[ix] == uint.MaxValue)
                return 0;

            return 31 - FloorLog2Impl(value[ix]) + (ix << 5); // mul 32
        }

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingOnes(ReadOnlySpan<uint> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            if (value[ix] == uint.MaxValue)
                return 32;

            // Negate mask but remember to truncate carry-bits
            var val = ~value[ix];

            return 31 - FloorLog2Impl(val) + (ix << 5); // mul 32
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingZeros(ReadOnlySpan<ulong> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            if (value[ix] == ulong.MaxValue)
                return 0;

            return 63 - FloorLog2Impl(value[ix]) + (ix << 6); // mul 64
        }

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingOnes(ReadOnlySpan<ulong> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            if (value[ix] == ulong.MaxValue)
                return 64;

            // Negate mask but remember to truncate carry-bits
            var val = ~value[ix];

            return 63 - FloorLog2Impl(val) + (ix << 6); // mul 64
        }

        #endregion

        #region TrailingCount

        /// <summary>
        /// Count the number of zero trailing bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingZeros(ReadOnlySpan<byte> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == 0) ix++;

            return TrailingZeros(value[last - ix]) + (ix << 3); // mul 8
        }

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingOnes(ReadOnlySpan<byte> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == 0) ix++;

            return TrailingOnes(value[last - ix]) + (ix << 3); // mul 8
        }

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingZeros(ReadOnlySpan<ushort> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == 0) ix++;

            return TrailingZeros(value[last - ix]) + (ix << 4); // mul 16
        }

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingOnes(ReadOnlySpan<ushort> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == 0) ix++;

            return TrailingOnes(value[last - ix]) + (ix << 4); // mul 16
        }

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingZeros(ReadOnlySpan<uint> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == 0) ix++;

            return TrailingZeros(value[last - ix]) + (ix << 5); // mul 32
        }

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingOnes(ReadOnlySpan<uint> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == 0) ix++;

            return TrailingOnes(value[last - ix]) + (ix << 5); // mul 32
        }

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingZeros(ReadOnlySpan<ulong> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == 0) ix++;

            return TrailingZeros(value[last - ix]) + (ix << 6); // mul 64
        }

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="ones">True to count ones, or false to count zeros.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingOnes(ReadOnlySpan<ulong> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == 0) ix++;

            return TrailingOnes(value[last - ix]) + (ix << 6); // mul 64
        }

        #endregion
    }
}

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(in ReadOnlySpan<byte> value, in uint offset)
        {
            var ix = (int)(offset >> 3); // div 8
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 7); // mod 8: design choice ignores out-of-range values

            return ExtractBit(value[ix], shft);
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(in ReadOnlySpan<ushort> value, in uint offset)
        {
            var ix = (int)(offset >> 4); // div 16
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            
            var shft = (byte)(offset & 15); // mod 16: design choice ignores out-of-range values

            return ExtractBit(value[ix], shft);
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(in ReadOnlySpan<uint> value, in uint offset)
        {
            var ix = (int)(offset >> 5); // div 32
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            
            var shft = (byte)(offset & 31); // mod 32: design choice ignores out-of-range values

            return ExtractBit(value[ix], shft);
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(in ReadOnlySpan<ulong> value, in uint offset)
        {
            var ix = (int)(offset >> 6); // div 64
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 63); // mod 64: design choice ignores out-of-range values

            return ExtractBit(value[ix], shft);
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
        public static bool InsertBit(ref Span<byte> value, in uint offset, in bool on)
        {
            var ix = (int)(offset >> 3); // div 8
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            
            var shft = (byte)(offset & 7); // mod 8: design choice ignores out-of-range values

            return InsertBit(ref value[ix], shft, on);
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref Span<ushort> value, in uint offset, in bool on)
        {
            var ix = (int)(offset >> 4); // div 16
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 15); // mod 16: design choice ignores out-of-range values

            return InsertBit(ref value[ix], shft, on);
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref Span<uint> value, in uint offset, in bool on)
        {
            var ix = (int)(offset >> 5); // div 32
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 31); // mod 32: design choice ignores out-of-range values

            return InsertBit(ref value[ix], shft, on);
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref Span<ulong> value, in uint offset, in bool on)
        {
            var ix = (int)(offset >> 6); // div 64
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 63); // mod 64: design choice ignores out-of-range values

            return InsertBit(ref value[ix], shft, on);
        }

        #endregion

        #region FlipBit

        /// <summary>
        /// Negates the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlipBit(ref Span<byte> value, in uint offset)
        {
            var ix = (int)(offset >> 3); // div 8
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 7); // mod 8: design choice ignores out-of-range values

            return FlipBit(ref value[ix], shft);
        }

        /// <summary>
        /// Negates the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlipBit(ref Span<ushort> value, in uint offset)
        {
            var ix = (int)(offset >> 4); // div 16
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 15); // mod 16: design choice ignores out-of-range values

            return FlipBit(ref value[ix], shft);
        }

        /// <summary>
        /// Negates the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlipBit(ref Span<uint> value, in uint offset)
        {
            var ix = (int)(offset >> 5); // div 32
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 31); // mod 32: design choice ignores out-of-range values

            return FlipBit(ref value[ix], shft);
        }

        /// <summary>
        /// Negates the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to flip.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlipBit(ref Span<ulong> value, in uint offset)
        {
            var ix = (int)(offset >> 6); // div 64
            if (ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var shft = (byte)(offset & 63); // mod 64: design choice ignores out-of-range values

            return FlipBit(ref value[ix], shft);
        }

        #endregion

        #region PopCount

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long PopCount(in ReadOnlySpan<byte> value)
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
        public static long PopCount(in ReadOnlySpan<ushort> value)
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
        public static long PopCount(in ReadOnlySpan<uint> value)
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
        public static long PopCount(in ReadOnlySpan<ulong> value)
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

        #region LeadingZeros

        /// <summary>
        /// Count the number of leading false bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingZeros(in ReadOnlySpan<byte> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            return LeadingZeros(value[ix]) + (ix << 3); // mul 8
        }

        /// <summary>
        /// Count the number of leading false bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingZeros(in ReadOnlySpan<ushort> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            return LeadingZeros(value[ix]) + (ix << 4); // mul 16
        }

        /// <summary>
        /// Count the number of leading false bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingZeros(in ReadOnlySpan<uint> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            return LeadingZeros(value[ix]) + (ix << 5); // mul 32
        }

        /// <summary>
        /// Count the number of leading false bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingZeros(in ReadOnlySpan<ulong> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == 0) ix++;

            return LeadingZeros(value[ix]) + (ix << 6); // mul 64
        }

        #endregion

        #region LeadingOnes

        /// <summary>
        /// Count the number of leading true bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingOnes(in ReadOnlySpan<byte> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == byte.MaxValue) ix++;

            return LeadingOnes(value[ix]) + (ix << 3); // mul 8
        }

        /// <summary>
        /// Count the number of leading true bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingOnes(in ReadOnlySpan<ushort> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == ushort.MaxValue) ix++;

            return LeadingOnes(value[ix]) + (ix << 4); // mul 16
        }

        /// <summary>
        /// Count the number of leading true bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingOnes(in ReadOnlySpan<uint> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == uint.MaxValue) ix++;

            return LeadingOnes(value[ix]) + (ix << 5); // mul 32
        }

        /// <summary>
        /// Count the number of leading true bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long LeadingOnes(in ReadOnlySpan<ulong> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            while (value[ix] == ulong.MaxValue) ix++;

            return LeadingOnes(value[ix]) + (ix << 6); // mul 64
        }

        #endregion

        #region TrailingZeros

        /// <summary>
        /// Count the number of trailing false bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingZeros(in ReadOnlySpan<byte> value)
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
        /// Count the number of trailing false bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingZeros(in ReadOnlySpan<ushort> value)
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
        /// Count the number of trailing false bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingZeros(in ReadOnlySpan<uint> value)
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
        /// Count the number of trailing false bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingZeros(in ReadOnlySpan<ulong> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == 0) ix++;

            return TrailingZeros(value[last - ix]) + (ix << 6); // mul 64
        }

        #endregion

        #region TrailingOnes

        /// <summary>
        /// Count the number of trailing true bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingOnes(in ReadOnlySpan<byte> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == byte.MaxValue) ix++;

            return TrailingOnes(value[last - ix]) + (ix << 3); // mul 8
        }

        /// <summary>
        /// Count the number of trailing true bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingOnes(in ReadOnlySpan<ushort> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == ushort.MaxValue) ix++;

            return TrailingOnes(value[last - ix]) + (ix << 4); // mul 16
        }

        /// <summary>
        /// Count the number of trailing true bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingOnes(in ReadOnlySpan<uint> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == uint.MaxValue) ix++;

            return TrailingOnes(value[last - ix]) + (ix << 5); // mul 32
        }

        /// <summary>
        /// Count the number of trailing true bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TrailingOnes(in ReadOnlySpan<ulong> value)
        {
            if (value.Length == 0)
                return 0;

            // TODO: Vectorize

            var ix = 0;
            var last = value.Length - 1;
            while (value[last - ix] == ulong.MaxValue) ix++;

            return TrailingOnes(value[last - ix]) + (ix << 6); // mul 64
        }

        #endregion
    }
}

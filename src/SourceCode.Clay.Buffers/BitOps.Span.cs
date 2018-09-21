namespace System
{
#pragma warning disable IDE0007 // Use implicit type

    partial class BitOps // .Span
    {
        #region ExtractBit

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<byte> span, int offset)
        {
            int ix = offset >> 3; // div 8
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset)); // TODO: Perf; do we want these guards?

            var val = ExtractBit(span[ix], offset);
            return val;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<ushort> span, int offset)
        {
            int ix = offset >> 4; // div 16
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var val = ExtractBit(span[ix], offset);
            return val;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<uint> span, int offset)
        {
            int ix = offset >> 5; // div 32
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var val = ExtractBit(span[ix], offset);
            return val;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<ulong> span, int offset)
        {
            int ix = offset >> 6; // div 64
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            var val = ExtractBit(span[ix], offset);
            return val;
        }

        #endregion

        #region WriteBit

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<byte> span, int offset, bool on)
        {
            var ix = offset >> 3; // div 8
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref byte val = ref span[ix];

            var wrt = WriteBit(ref val, offset, on);
            return wrt;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<ushort> span, int offset, bool on)
        {
            var ix = offset >> 4; // div 16
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ushort val = ref span[ix];

            var wrt = WriteBit(ref val, offset, on);
            return wrt;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<uint> span, int offset, bool on)
        {
            var ix = offset >> 5; // div 32
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref uint val = ref span[ix];

            var wrt = WriteBit(ref val, offset, on);
            return wrt;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<ulong> span, int offset, bool on)
        {
            var ix = offset >> 6; // div 64
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ulong val = ref span[ix];

            var wrt = WriteBit(ref val, offset, on);
            return wrt;
        }

        #endregion

        #region ClearBit

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<byte> span, int offset)
        {
            var ix = offset >> 3; // div 8
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref byte val = ref span[ix];

            var btr =  ClearBit(ref val, offset);
            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<ushort> span, int offset)
        {
            var ix = offset >> 4; // div 16
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ushort val = ref span[ix];

            var btr = ClearBit(ref val, offset);
            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<uint> span, int offset)
        {
            var ix = offset >> 5; // div 32
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref uint val = ref span[ix];

            var btr = ClearBit(ref val, offset);
            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<ulong> span, int offset)
        {
            var ix = offset >> 6; // div 64
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ulong val = ref span[ix];

            var btr = ClearBit(ref val, offset);
            return btr;
        }

        #endregion

        #region InsertBit

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<byte> span, int offset)
        {
            var ix = offset >> 3; // div 8
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref byte val = ref span[ix];

            var bts = InsertBit(ref val, offset);
            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<ushort> span, int offset)
        {
            var ix = offset >> 4; // div 16
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ushort val = ref span[ix];

            var bts = InsertBit(ref val, offset);
            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<uint> span, int offset)
        {
            var ix = offset >> 5; // div 32
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref uint val = ref span[ix];

            var bts = InsertBit(ref val, offset);
            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<ulong> span, int offset)
        {
            var ix = offset >> 6; // div 64
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ulong val = ref span[ix];

            var bts = InsertBit(ref val, offset);
            return bts;
        }

        #endregion

        #region ComplementBit

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<byte> span, int offset)
        {
            var ix = offset >> 3; // div 8
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref byte val = ref span[ix];

            var btc = ComplementBit(ref val, offset);
            return btc;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<ushort> span, int offset)
        {
            var ix = offset >> 4; // div 16
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ushort val = ref span[ix];

            var btc = ComplementBit(ref val, offset);
            return btc;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<uint> span, int offset)
        {
            var ix = offset >> 5; // div 32
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref uint val = ref span[ix];

            var btc = ComplementBit(ref val, offset);
            return btc;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<ulong> span, int offset)
        {
            var ix = offset >> 6; // div 64
            if (ix < 0 || ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ulong val = ref span[ix];

            var btc = ComplementBit(ref val, offset);
            return btc;
        }

        #endregion

        #region ExtractByte

        public static byte ExtractByte(ReadOnlySpan<byte> span, int bitOffset)
        {
            var spanBitLen = span.Length * 8;

            int ix = bitOffset >> 3; // div 8
            if (ix >= spanBitLen) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ushort value = span[ix];
            if (ix + 1 < spanBitLen)
                value |= (ushort)(span[ix + 1] << 8);

            var ofs = bitOffset & 7; // mod 8

            var val = ExtractByte(value, ofs);
            return val;
        }

        public static byte ExtractByte(ReadOnlySpan<ushort> span, int bitOffset)
        {
            var spanBitLen = span.Length * 16;

            int ix = bitOffset >> 4; // div 16
            if (ix >= spanBitLen) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            uint value = span[ix];
            if (ix + 1 < spanBitLen)
                value |= (uint)(span[ix + 1] << 16);

            var ofs = bitOffset & 15; // mod 16

            var val = ExtractByte(value, bitOffset & ofs);
            return val;
        }

        public static byte ExtractByte(ReadOnlySpan<uint> span, int bitOffset)
        {
            var spanBitLen = span.Length * 32;

            int ix = bitOffset >> 5; // div 32
            if (ix >= spanBitLen) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ulong value = span[ix];
            if (ix + 1 < spanBitLen)
                value |= span[ix + 1] << 32;

            var ofs = bitOffset & 31; // mod 32

            var val = ExtractByte(value, bitOffset & ofs);
            return val;
        }

        public static byte ExtractByte(ReadOnlySpan<ulong> span, int bitOffset)
        {
            var spanBitLen = span.Length * sizeof(uint); // 32

            int ix = bitOffset >> 6; // div 64
            if (ix >= spanBitLen) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            return 0;
        }

        #endregion

        #region InsertByte

        public static byte InsertByte(Span<byte> span, int bitOffset, byte insert)
        {
            return 0;
        }

        public static byte InsertByte(Span<ushort> span, int bitOffset, byte insert)
        {
            return 0;
        }

        public static byte InsertByte(Span<uint> span, int bitOffset, byte insert)
        {
            return 0;
        }

        public static byte InsertByte(Span<ulong> span, int bitOffset, byte insert)
        {
            return 0;
        }

        #endregion

        #region ExtractUInt16

        public static ushort ExtractUInt16(ReadOnlySpan<byte> span, int bitOffset)
        {
            return 0;
        }

        public static ushort ExtractUInt16(ReadOnlySpan<ushort> span, int bitOffset)
        {
            return 0;
        }

        public static ushort ExtractUInt16(ReadOnlySpan<uint> span, int bitOffset)
        {
            return 0;
        }

        public static ushort ExtractUInt16(ReadOnlySpan<ulong> span, int bitOffset)
        {
            return 0;
        }

        #endregion

        #region InsertUInt16

        public static ushort InsertUInt16(Span<byte> span, int bitOffset, ushort insert)
        {
            return 0;
        }

        public static ushort InsertUInt16(Span<ushort> span, int bitOffset, ushort insert)
        {
            return 0;
        }

        public static ushort InsertUInt16(Span<uint> span, int bitOffset, ushort insert)
        {
            return 0;
        }

        public static ushort InsertUInt16(Span<ulong> span, int bitOffset, ushort insert)
        {
            return 0;
        }

        #endregion

        #region ExtractUInt32

        public static uint ExtractUInt32(ReadOnlySpan<byte> span, int bitOffset)
        {
            return 0;
        }

        public static uint ExtractUInt32(ReadOnlySpan<ushort> span, int bitOffset)
        {
            return 0;
        }

        public static uint ExtractUInt32(ReadOnlySpan<uint> span, int bitOffset)
        {
            return 0;
        }

        public static uint ExtractUInt32(ReadOnlySpan<ulong> span, int bitOffset)
        {
            return 0;
        }

        #endregion

        #region InsertUInt32

        public static uint InsertUInt32(Span<byte> span, int bitOffset, uint insert)
        {
            return 0;
        }

        public static uint InsertUInt32(Span<ushort> span, int bitOffset, uint insert)
        {
            return 0;
        }

        public static uint InsertUInt32(Span<uint> span, int bitOffset, uint insert)
        {
            return 0;
        }

        public static uint InsertUInt32(Span<ulong> span, int bitOffset, uint insert)
        {
            return 0;
        }

        #endregion

        #region ExtractUInt64

        public static ulong ExtractUInt64(ReadOnlySpan<byte> span, int bitOffset)
        {
            return 0;
        }

        public static ulong ExtractUInt64(ReadOnlySpan<ushort> span, int bitOffset)
        {
            return 0;
        }

        public static ulong ExtractUInt64(ReadOnlySpan<uint> span, int bitOffset)
        {
            return 0;
        }

        public static ulong ExtractUInt64(ReadOnlySpan<ulong> span, int bitOffset)
        {
            return 0;
        }

        #endregion

        #region InsertUInt64

        public static ulong InsertUInt64(Span<byte> span, int bitOffset, ulong insert)
        {
            return 0;
        }

        public static ulong InsertUInt64(Span<ushort> span, int bitOffset, ulong insert)
        {
            return 0;
        }

        public static ulong InsertUInt64(Span<uint> span, int bitOffset, ulong insert)
        {
            return 0;
        }

        public static ulong InsertUInt64(Span<ulong> span, int bitOffset, ulong insert)
        {
            return 0;
        }

        #endregion
    }

#pragma warning restore IDE0007 // Use implicit type
}

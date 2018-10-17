namespace System
{
    partial class BitOps // .Span
    {
        #region ExtractBit

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<byte> value, int offset)
        {
            int ix = offset >> 3; // div 8
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset)); // TODO: Perf; do we want these guards?

            bool val = ExtractBit(value[ix], offset);
            return val;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<ushort> value, int offset)
        {
            int ix = offset >> 4; // div 16
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            bool val = ExtractBit(value[ix], offset);
            return val;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<uint> value, int offset)
        {
            int ix = offset >> 5; // div 32
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            bool val = ExtractBit(value[ix], offset);
            return val;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<ulong> value, int offset)
        {
            int ix = offset >> 6; // div 64
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            bool val = ExtractBit(value[ix], offset);
            return val;
        }

        #endregion

        #region WriteBit

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<byte> value, int offset, bool on)
        {
            int ix = offset >> 3; // div 8
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref byte val = ref value[ix];

            bool wrt = WriteBit(ref val, offset, on);
            return wrt;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<ushort> value, int offset, bool on)
        {
            int ix = offset >> 4; // div 16
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ushort val = ref value[ix];

            bool wrt = WriteBit(ref val, offset, on);
            return wrt;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<uint> value, int offset, bool on)
        {
            int ix = offset >> 5; // div 32
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref uint val = ref value[ix];

            bool wrt = WriteBit(ref val, offset, on);
            return wrt;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<ulong> value, int offset, bool on)
        {
            int ix = offset >> 6; // div 64
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ulong val = ref value[ix];

            bool wrt = WriteBit(ref val, offset, on);
            return wrt;
        }

        #endregion

        #region ClearBit

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<byte> value, int offset)
        {
            int ix = offset >> 3; // div 8
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref byte val = ref value[ix];

            bool btr =  ClearBit(ref val, offset);
            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<ushort> value, int offset)
        {
            int ix = offset >> 4; // div 16
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ushort val = ref value[ix];

            bool btr = ClearBit(ref val, offset);
            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<uint> value, int offset)
        {
            int ix = offset >> 5; // div 32
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref uint val = ref value[ix];

            bool btr = ClearBit(ref val, offset);
            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<ulong> value, int offset)
        {
            int ix = offset >> 6; // div 64
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ulong val = ref value[ix];

            bool btr = ClearBit(ref val, offset);
            return btr;
        }

        #endregion

        #region InsertBit

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<byte> value, int offset)
        {
            int ix = offset >> 3; // div 8
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref byte val = ref value[ix];

            bool bts = InsertBit(ref val, offset);
            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<ushort> value, int offset)
        {
            int ix = offset >> 4; // div 16
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ushort val = ref value[ix];

            bool bts = InsertBit(ref val, offset);
            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<uint> value, int offset)
        {
            int ix = offset >> 5; // div 32
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref uint val = ref value[ix];

            bool bts = InsertBit(ref val, offset);
            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<ulong> value, int offset)
        {
            int ix = offset >> 6; // div 64
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ulong val = ref value[ix];

            bool bts = InsertBit(ref val, offset);
            return bts;
        }

        #endregion

        #region ComplementBit

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<byte> value, int offset)
        {
            int ix = offset >> 3; // div 8
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref byte val = ref value[ix];

            bool btc = ComplementBit(ref val, offset);
            return btc;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<ushort> value, int offset)
        {
            int ix = offset >> 4; // div 16
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ushort val = ref value[ix];

            bool btc = ComplementBit(ref val, offset);
            return btc;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<uint> value, int offset)
        {
            int ix = offset >> 5; // div 32
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref uint val = ref value[ix];

            bool btc = ComplementBit(ref val, offset);
            return btc;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="offset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<ulong> value, int offset)
        {
            int ix = offset >> 6; // div 64
            if (ix < 0 || ix >= value.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ulong val = ref value[ix];

            bool btc = ComplementBit(ref val, offset);
            return btc;
        }

        #endregion
    }
}

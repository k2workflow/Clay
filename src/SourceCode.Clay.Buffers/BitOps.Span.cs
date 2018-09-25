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
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset)); // TODO: Perf; do we want these guards?

            var val = ExtractBit(span[ix], bitOffset);
            return val;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            var val = ExtractBit(span[ix], bitOffset);
            return val;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            var val = ExtractBit(span[ix], bitOffset);
            return val;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static bool ExtractBit(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            var val = ExtractBit(span[ix], bitOffset);
            return val;
        }

        #endregion

        #region WriteBit

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<byte> span, int bitOffset, bool on)
        {
            var ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref byte val = ref span[ix];

            var wrt = WriteBit(ref val, bitOffset, on);
            return wrt;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<ushort> span, int bitOffset, bool on)
        {
            var ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ushort val = ref span[ix];

            var wrt = WriteBit(ref val, bitOffset, on);
            return wrt;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<uint> span, int bitOffset, bool on)
        {
            var ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref uint val = ref span[ix];

            var wrt = WriteBit(ref val, bitOffset, on);
            return wrt;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Executes without branching.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        public static bool WriteBit(Span<ulong> span, int bitOffset, bool on)
        {
            var ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ulong val = ref span[ix];

            var wrt = WriteBit(ref val, bitOffset, on);
            return wrt;
        }

        #endregion

        #region ClearBit

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<byte> span, int bitOffset)
        {
            var ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref byte val = ref span[ix];

            var btr =  ClearBit(ref val, bitOffset);
            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<ushort> span, int bitOffset)
        {
            var ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ushort val = ref span[ix];

            var btr = ClearBit(ref val, bitOffset);
            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<uint> span, int bitOffset)
        {
            var ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref uint val = ref span[ix];

            var btr = ClearBit(ref val, bitOffset);
            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.</param>
        public static bool ClearBit(Span<ulong> span, int bitOffset)
        {
            var ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ulong val = ref span[ix];

            var btr = ClearBit(ref val, bitOffset);
            return btr;
        }

        #endregion

        #region InsertBit

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<byte> span, int bitOffset)
        {
            var ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref byte val = ref span[ix];

            var bts = InsertBit(ref val, bitOffset);
            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<ushort> span, int bitOffset)
        {
            var ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ushort val = ref span[ix];

            var bts = InsertBit(ref val, bitOffset);
            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<uint> span, int bitOffset)
        {
            var ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref uint val = ref span[ix];

            var bts = InsertBit(ref val, bitOffset);
            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.</param>
        public static bool InsertBit(Span<ulong> span, int bitOffset)
        {
            var ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ulong val = ref span[ix];

            var bts = InsertBit(ref val, bitOffset);
            return bts;
        }

        #endregion

        #region ComplementBit

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<byte> span, int bitOffset)
        {
            var ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref byte val = ref span[ix];

            var btc = ComplementBit(ref val, bitOffset);
            return btc;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<ushort> span, int bitOffset)
        {
            var ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ushort val = ref span[ix];

            var btc = ComplementBit(ref val, bitOffset);
            return btc;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<uint> span, int bitOffset)
        {
            var ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref uint val = ref span[ix];

            var btc = ComplementBit(ref val, bitOffset);
            return btc;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="span">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.</param>
        public static bool ComplementBit(Span<ulong> span, int bitOffset)
        {
            var ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ulong val = ref span[ix];

            var btc = ComplementBit(ref val, bitOffset);
            return btc;
        }

        #endregion

        #region ExtractByte

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static byte ExtractByte(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            uint val = 0;
            switch (len)
            {
                // Need at least 1+1 bytes
                default:
                case 2: val |= (uint)span[ix + 1] << (8 - shft); goto case 1;
                case 1: val |= (uint)span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return (byte)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static byte ExtractByte(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 15; // mod 16

            uint val = 0;
            switch (len)
            {
                // Need at least 1+1 ushorts
                default:
                case 2: val |= (uint)span[ix + 1] << (16 - shft); goto case 1;
                case 1: val |= (uint)span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return (byte)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static byte ExtractByte(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 31; // mod 32

            uint val = 0;
            switch (len)
            {
                // Need at least 1+1 uints
                default:
                case 2: val |= span[ix + 1] << (32 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return (byte)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static byte ExtractByte(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 63; // mod 64

            ulong val = 0;
            switch (len)
            {
                // Need at least 1+1 ulongs
                default:
                case 2: val |= span[ix + 1] << (64 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return (byte)val;
        }

        #endregion

        #region InsertByte

        /// <summary>
        /// Writes the specified value to a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        /// <param name="value">The value to write.</param>
        public static void InsertByte(Span<byte> span, int bitOffset, byte value)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            //switch (len)
            //{
            //    // Need at least 1+1 bytes
            //    default:
            //    case 2: span[ix + 1] = (uint)value >> (8 - shft); goto case 1;
            //    case 1: span[ix + 0] = (uint)value << shft; break;

            //    case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            //}
        }

        public static void InsertByte(Span<ushort> span, int bitOffset, byte value)
        {
        }

        public static void InsertByte(Span<uint> span, int bitOffset, byte value)
        {
        }

        public static void InsertByte(Span<ulong> span, int bitOffset, byte value)
        {
        }

        #endregion

        #region ExtractUInt16

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            uint val = 0;
            switch (len)
            {
                // Need at least 2+1 bytes
                default:
                case 3: val |= (uint)span[ix + 2] << (2 * 8 - shft); goto case 2;
                case 2: val |= (uint)span[ix + 1] << (1 * 8 - shft); goto case 1;
                case 1: val |= (uint)span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return (ushort)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 15; // mod 16

            uint val = 0;
            switch (len)
            {
                // Need at least 1+1 ushorts
                default:
                case 2: val |= (uint)span[ix + 1] << (16 - shft); goto case 1;
                case 1: val |= (uint)span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return (ushort)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 31; // mod 31

            uint val = 0;
            switch (len)
            {
                // Need at least 1+1 uints
                default:
                case 2: val |= span[ix + 1] << (32 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return (ushort)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 63; // mod 64

            ulong val = 0;
            switch (len)
            {
                // Need at least 1+1 ulongs
                default:
                case 2: val |= span[ix + 1] << (64 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return (ushort)val;
        }

        #endregion

        #region InsertUInt16

        public static void InsertUInt16(Span<byte> span, int bitOffset, ushort value)
        {
        }

        public static void InsertUInt16(Span<ushort> span, int bitOffset, ushort value)
        {
        }

        public static void InsertUInt16(Span<uint> span, int bitOffset, ushort value)
        {
        }

        public static void InsertUInt16(Span<ulong> span, int bitOffset, ushort value)
        {
        }

        #endregion

        #region ExtractUInt32

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            uint val = 0;
            switch (len)
            {
                // Need at least 4+1 bytes
                default:
                case 5: val |= (uint)span[ix + 4] << (4 * 8 - shft); goto case 4;
                case 4: val |= (uint)span[ix + 3] << (3 * 8 - shft); goto case 3;
                case 3: val |= (uint)span[ix + 2] << (2 * 8 - shft); goto case 2;
                case 2: val |= (uint)span[ix + 1] << (1 * 8 - shft); goto case 1;
                case 1: val |= (uint)span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 15; // mod 16

            uint val = 0;
            switch (len)
            {
                // Need at least 2+1 ushorts
                default:
                case 3: val |= (uint)span[ix + 2] << (2 * 16 - shft); goto case 2;
                case 2: val |= (uint)span[ix + 1] << (1 * 16 - shft); goto case 1;
                case 1: val |= (uint)span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 31; // mod 32

            uint val = 0;
            switch (len)
            {
                // Need at least 1+1 uints
                default:
                case 2: val |= span[ix + 1] << (1 * 32 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 63; // mod 64

            ulong val = 0;
            switch (len)
            {
                // Need at least 1+1 ulongs
                default:
                case 2: val |= span[ix + 1] << (1 * 64 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return (uint)val;
        }

        #endregion

        #region InsertUInt32

        public static void InsertUInt32(Span<byte> span, int bitOffset, uint value)
        {
        }

        public static void InsertUInt32(Span<ushort> span, int bitOffset, uint value)
        {
        }

        public static void InsertUInt32(Span<uint> span, int bitOffset, uint value)
        {
        }

        public static void InsertUInt32(Span<ulong> span, int bitOffset, uint value)
        {
        }

        #endregion

        #region ExtractUInt64

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            ulong val = 0;
            switch (len)
            {
                // Need at least 8+1 bytes
                default:
                case 9: val |= (ulong)span[ix + 8] << (8 * 8 - shft); goto case 8;
                case 8: val |= (ulong)span[ix + 7] << (7 * 8 - shft); goto case 7;
                case 7: val |= (ulong)span[ix + 6] << (6 * 8 - shft); goto case 6;
                case 6: val |= (ulong)span[ix + 5] << (5 * 8 - shft); goto case 5;
                case 5: val |= (ulong)span[ix + 4] << (4 * 8 - shft); goto case 4;
                case 4: val |= (ulong)span[ix + 3] << (3 * 8 - shft); goto case 3;
                case 3: val |= (ulong)span[ix + 2] << (2 * 8 - shft); goto case 2;
                case 2: val |= (ulong)span[ix + 1] << (1 * 8 - shft); goto case 1;
                case 1: val |= (ulong)span[ix + 0] >> shft; return val;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 15; // mod 16

            ulong val = 0;
            switch (len)
            {
                // Need at least 4+1 ushorts
                default:
                case 5: val |= (ulong)span[ix + 4] << (4 * 16 - shft); goto case 4;
                case 4: val |= (ulong)span[ix + 3] << (3 * 16 - shft); goto case 3;
                case 3: val |= (ulong)span[ix + 2] << (2 * 16 - shft); goto case 2;
                case 2: val |= (ulong)span[ix + 1] << (1 * 16 - shft); goto case 1;
                case 1: val |= (ulong)span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 31; // mod 32

            ulong val = 0;
            switch (len)
            {
                // Need at least 2+1 uints
                default:
                case 3: val |= (ulong)span[ix + 2] << (2 * 32 - shft); goto case 2;
                case 2: val |= (ulong)span[ix + 1] << (1 * 32 - shft); goto case 1;
                case 1: val |= (ulong)span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 63; // mod 64

            ulong val = 0;
            switch (len)
            {
                // Need at least 1+1 ulongs
                default:
                case 2: val |= span[ix + 1] << (64 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;

                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));
            }

            return val;
        }

        #endregion

        #region InsertUInt64

        public static void InsertUInt64(Span<byte> span, int bitOffset, ulong value)
        {
        }

        public static void InsertUInt64(Span<ushort> span, int bitOffset, ulong value)
        {
        }

        public static void InsertUInt64(Span<uint> span, int bitOffset, ulong value)
        {
        }

        public static void InsertUInt64(Span<ulong> span, int bitOffset, ulong value)
        {
        }

        #endregion
    }

#pragma warning restore IDE0007 // Use implicit type
}

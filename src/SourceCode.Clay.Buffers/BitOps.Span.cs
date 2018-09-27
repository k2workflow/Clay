using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
            int ix = bitOffset >> 3;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset)); // TODO: Perf; do we want these guards?

            bool val = ExtractBit(span[ix], bitOffset);
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
            int ix = bitOffset >> 4;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            bool val = ExtractBit(span[ix], bitOffset);
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
            int ix = bitOffset >> 5;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            bool val = ExtractBit(span[ix], bitOffset);
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
            int ix = bitOffset >> 6;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            bool val = ExtractBit(span[ix], bitOffset);
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
            int ix = bitOffset >> 3;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref byte val = ref span[ix];

            bool wrt = WriteBit(ref val, bitOffset, on);
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
            int ix = bitOffset >> 4;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ushort val = ref span[ix];

            bool wrt = WriteBit(ref val, bitOffset, on);
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
            int ix = bitOffset >> 5;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref uint val = ref span[ix];

            bool wrt = WriteBit(ref val, bitOffset, on);
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
            int ix = bitOffset >> 6;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ulong val = ref span[ix];

            bool wrt = WriteBit(ref val, bitOffset, on);
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
            int ix = bitOffset >> 3;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref byte val = ref span[ix];

            bool btr =  ClearBit(ref val, bitOffset);
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
            int ix = bitOffset >> 4;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ushort val = ref span[ix];

            bool btr = ClearBit(ref val, bitOffset);
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
            int ix = bitOffset >> 5;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref uint val = ref span[ix];

            bool btr = ClearBit(ref val, bitOffset);
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
            int ix = bitOffset >> 6;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ulong val = ref span[ix];

            bool btr = ClearBit(ref val, bitOffset);
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
            int ix = bitOffset >> 3;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref byte val = ref span[ix];

            bool bts = InsertBit(ref val, bitOffset);
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
            int ix = bitOffset >> 4;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ushort val = ref span[ix];

            bool bts = InsertBit(ref val, bitOffset);
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
            int ix = bitOffset >> 5;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref uint val = ref span[ix];

            bool bts = InsertBit(ref val, bitOffset);
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
            int ix = bitOffset >> 6;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ulong val = ref span[ix];

            bool bts = InsertBit(ref val, bitOffset);
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
            int ix = bitOffset >> 3;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref byte val = ref span[ix];

            bool btc = ComplementBit(ref val, bitOffset);
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
            int ix = bitOffset >> 4;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ushort val = ref span[ix];

            bool btc = ComplementBit(ref val, bitOffset);
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
            int ix = bitOffset >> 5;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref uint val = ref span[ix];

            bool btc = ComplementBit(ref val, bitOffset);
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
            int ix = bitOffset >> 6;
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ref ulong val = ref span[ix];

            bool btc = ComplementBit(ref val, bitOffset);
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
            => (byte)ExtractUInt16(span, bitOffset);

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static byte ExtractByte(ReadOnlySpan<ushort> span, int bitOffset)
            => (byte)ExtractUInt16(span, bitOffset);

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static byte ExtractByte(ReadOnlySpan<uint> span, int bitOffset)
            => (byte)ExtractUInt32(span, bitOffset);

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static byte ExtractByte(ReadOnlySpan<ulong> span, int bitOffset)
            => (byte)ExtractUInt32(span, bitOffset);

        #endregion

        #region InsertByte

        /// <summary>
        /// Writes the specified value to a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        /// <param name="insert">The value to write.</param>
        public static byte InsertByte(Span<byte> span, int bitOffset, byte insert)
        {
            int ix = bitOffset >> 3;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 7;

            // Need at most 1+1 bytes
            byte @null = 0;
            ref byte r1 = ref @null;
            ref byte r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Extract original mask & value
            uint mask = (uint)r1 << 8 | r0;
            uint orig = mask >> shft;

            // Build new mask
            uint hole = ~((uint)byte.MaxValue << shft);
            uint ins = (uint)insert << shft;
            mask = (mask & hole) | ins;

            // Write element refs
            r1 = (byte)(mask >> 8);
            r0 = (byte)mask;

            return (byte)orig;
        }

        public static byte InsertByte(Span<ushort> span, int bitOffset, byte insert)
        {
            int ix = bitOffset >> 4;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 15;

            // Need at most 1+1 ushorts
            ushort @null = 0;
            ref ushort r1 = ref @null;
            ref ushort r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Extract original mask & value
            uint mask = (uint)r1 << 16 | r0;
            uint orig = mask >> shft;

            // Build new mask
            uint hole = ~((uint)byte.MaxValue << shft);
            uint ins = (uint)insert << shft;
            mask = (mask & hole) | ins;

            // Write element refs
            r1 = (ushort)(mask >> 16);
            r0 = (ushort)mask;

            return (byte)orig;
        }

        public static byte InsertByte(Span<uint> span, int bitOffset, byte insert)
        {
            int ix = bitOffset >> 5;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 31;

            // Need at most 1+1 uints
            uint @null = 0;
            ref uint r1 = ref @null;
            ref uint r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Extract original mask & value
            ulong mask = r1 << 32 | r0;
            ulong orig = mask >> shft;

            // Build new mask
            ulong hole = ~((ulong)byte.MaxValue << shft);
            ulong ins = (ulong)insert << shft;
            mask = (mask & hole) | ins;

            // Write element refs
            r1 = (uint)(mask >> 32);
            r0 = (uint)mask;

            return (byte)orig;
        }

        public static byte InsertByte(Span<ulong> span, int bitOffset, byte insert)
        {
            int ix = bitOffset >> 6;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 63;

            // Need at most 1+1 ulongs
            ulong @null = 0;
            ref ulong r1 = ref @null;
            ref ulong r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Build original value
            ulong orig = r0 >> shft;
            orig |= r1 << (64 - shft);

            // Build new masks
            ulong hole1 = ~((ulong)byte.MaxValue >> (64 - shft));
            ulong ins1 = (ulong)insert >> (64 - shft);
            ulong mask1 = (r1 & hole1) | ins1;

            ulong hole0 = ~((ulong)byte.MaxValue << shft);
            ulong ins0 = (ulong)insert << shft;
            ulong mask0 = (r0 & hole0) | ins0;

            // Write element refs
            r1 = mask1;
            r0 = mask0;

            return (byte)orig;
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
            int ix = bitOffset >> 3;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            
            // Need at most 2+1 bytes
            uint blit = 0;
            switch (len)
            {
                default:
                case 3: blit = (uint)span[ix + 2] << 16; goto case 2;
                case 2: blit |= (uint)span[ix + 1] << 8; goto case 1;
                case 1: blit |= span[ix]; break;
            }

            blit >>= bitOffset & 7;
            return (ushort)blit;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at most 1+1 ushorts
            uint blit = 0;
            switch (len)
            {
                default:
                case 2: blit = (uint)span[ix + 1] << 16; goto case 1;
                case 1: blit |= span[ix]; break;
            }

            blit >>= bitOffset & 15;
            return (ushort)blit;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<uint> span, int bitOffset)
            => (ushort)ExtractUInt32(span, bitOffset);

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<ulong> span, int bitOffset)
            => (ushort)ExtractUInt32(span, bitOffset);

        #endregion

        #region InsertUInt16

        public static ushort InsertUInt16(Span<byte> span, int bitOffset, ushort insert)
        {
            int ix = bitOffset >> 3;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 7;

            // Need at most 2+1 bytes
            byte @null = 0;
            ref byte r2 = ref @null;
            ref byte r1 = ref @null;
            ref byte r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 3: r2 = ref span[ix + 2]; goto case 2;
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Extract original mask & value
            uint mask = (uint)r2 << 16 | (uint)r1 << 8 | r0;
            uint orig = mask >> shft;

            // Build new mask
            uint hole = ~((uint)ushort.MaxValue << shft);
            uint ins = (uint)insert << shft;
            mask = (mask & hole) | ins;

            // Write element refs
            r2 = (byte)(mask >> 16);
            r1 = (byte)(mask >> 8);
            r0 = (byte)mask;

            return (ushort)orig;
        }

        public static ushort InsertUInt16(Span<ushort> span, int bitOffset, ushort insert)
        {
            int ix = bitOffset >> 4;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 15;

            // Need at most 1+1 ushorts
            ushort @null = 0;
            ref ushort r1 = ref @null;
            ref ushort r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Extract original mask & value
            uint mask = (uint)r1 << 16 | r0;
            uint orig = mask >> shft;

            // Build new mask
            uint hole = ~((uint)ushort.MaxValue << shft);
            uint ins = (uint)insert << shft;
            mask = (mask & hole) | ins;

            // Write element refs
            r1 = (ushort)(mask >> 16);
            r0 = (ushort)mask;

            return (ushort)orig;
        }

        public static ushort InsertUInt16(Span<uint> span, int bitOffset, ushort insert)
        {
            int ix = bitOffset >> 5;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 31;

            // Need at most 1+1 uints
            uint @null = 0;
            ref uint r1 = ref @null;
            ref uint r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Extract original mask & value
            ulong mask = (ulong)r1 << 32 | r0;
            ulong orig = mask >> shft;

            // Build new mask
            ulong hole = ~((ulong)ushort.MaxValue << shft);
            ulong ins = (ulong)insert << shft;
            mask = (mask & hole) | ins;

            // Write element refs
            r1 = (uint)(mask >> 32);
            r0 = (uint)mask;

            return (ushort)orig;
        }

        public static ushort InsertUInt16(Span<ulong> span, int bitOffset, ushort insert)
        {
            int ix = bitOffset >> 6;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 63;

            // Need at most 1+1 ulongs
            ulong @null = 0;
            ref ulong r1 = ref @null;
            ref ulong r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Build original value
            ulong orig = r0 >> shft;
            orig |= r1 << (64 - shft);

            // Build new masks
            ulong hole1 = ~((ulong)ushort.MaxValue >> (64 - shft));
            ulong ins1 = (ulong)insert >> (64 - shft);
            ulong mask1 = (r1 & hole1) | ins1;

            ulong hole0 = ~((ulong)ushort.MaxValue << shft);
            ulong ins0 = (ulong)insert << shft;
            ulong mask0 = (r0 & hole0) | ins0;

            // Write element refs
            r1 = mask1;
            r0 = mask0;

            return (ushort)orig;
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
            int ix = bitOffset >> 3;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ReadOnlySpan<byte> slice = span.Slice(ix);

            // Need at most 4+1 bytes
            ulong blit = 0;
            switch (len)
            {
                default:
                case 5: blit = (ulong)slice[4] << 32; goto case 4;
                case 4: blit |= ReadUInt32(slice); break;

                case 3: blit = (ulong)slice[2] << 16; goto case 2;
                case 2: blit |= (ulong)slice[1] << 8; goto case 1;
                case 1: blit |= slice[0]; break;
            }

            blit >>= bitOffset & 7;
            return (uint)blit;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at most 2+1 ushorts
            ulong blit = 0;
            switch (len)
            {
                default:
                case 3: blit = (ulong)span[ix + 2] << 32; goto case 2;
                case 2: blit |= (ulong)span[ix + 1] << 16; goto case 1;
                case 1: blit |= span[ix]; break;
            }

            blit >>= bitOffset & 15;
            return (uint)blit;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at most 1+1 uints
            ulong blit = 0;
            switch (len)
            {
                default:
                case 2: blit = (ulong)span[ix + 1] << 32; goto case 1;
                case 1: blit |= span[ix]; break;
            }

            blit >>= bitOffset & 31;
            return (uint)blit;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<ulong> span, int bitOffset)
            => (uint)ExtractUInt64(span, bitOffset);

        #endregion

        #region InsertUInt32

        public static uint InsertUInt32(Span<byte> span, int bitOffset, uint insert)
        {
            int ix = bitOffset >> 3;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 7;

            // Need at most 4+1 bytes
            byte @null = 0;
            ref byte r4 = ref @null;
            ref byte r3 = ref @null;
            ref byte r2 = ref @null;
            ref byte r1 = ref @null;
            ref byte r0 = ref @null;

            // Read element refs
            var slice = span.Slice(ix);
            switch (len)
            {
                default:
                case 5: r4 = ref slice[4]; goto case 4;
                case 4: r3 = ref slice[3]; goto case 3;
                case 3: r2 = ref slice[2]; goto case 2;
                case 2: r1 = ref slice[1]; goto case 1;
                case 1: r0 = ref slice[0]; break;
            }

            // Extract original mask & value
            ulong mask = (ulong)r4 << 32 | (ulong)r3 << 24 | (ulong)r2 << 16 | (ulong)r1 << 8 | r0;
            ulong orig = mask >> shft;

            // Build new mask
            ulong hole = ~((ulong)uint.MaxValue << shft);
            ulong ins = (ulong)insert << shft;
            mask = (mask & hole) | ins;

            // Write element refs
            r4 = (byte)(mask >> 32);
            r3 = (byte)(mask >> 24);
            r2 = (byte)(mask >> 16);
            r1 = (byte)(mask >> 8);
            r0 = (byte)mask;

            return (uint)orig;
        }

        public static uint InsertUInt32(Span<ushort> span, int bitOffset, uint insert)
        {
            int ix = bitOffset >> 4;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 15;

            // Need at most 2+1 ushorts
            ushort @null = 0;
            ref ushort r2 = ref @null;
            ref ushort r1 = ref @null;
            ref ushort r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 3: r2 = ref span[ix + 2]; goto case 2;
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Extract original mask & value
            ulong mask = (ulong)r2 << 32 | (ulong)r1 << 16 | r0;
            ulong orig = mask >> shft;

            // Build new mask
            ulong hole = ~((ulong)uint.MaxValue << shft);
            ulong ins = (ulong)insert << shft;
            mask = (mask & hole) | ins;

            // Write element refs
            r2 = (ushort)(mask >> 32);
            r1 = (ushort)(mask >> 16);
            r0 = (ushort)mask;

            return (uint)orig;
        }

        public static uint InsertUInt32(Span<uint> span, int bitOffset, uint insert)
        {
            int ix = bitOffset >> 5;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 31;

            // Need at most 1+1 uints
            uint @null = 0;
            ref uint r1 = ref @null;
            ref uint r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Extract original mask & value
            ulong mask = (ulong)r1 << 32 | r0;
            ulong orig = mask >> shft;

            // Build new mask
            ulong hole = ~((ulong)uint.MaxValue << shft);
            ulong ins = (ulong)insert << shft;
            mask = (mask & hole) | ins;

            // Write element refs
            r1 = (uint)(mask >> 32);
            r0 = (uint)mask;

            return (uint)orig;
        }

        public static uint InsertUInt32(Span<ulong> span, int bitOffset, uint insert)
        {
            int ix = bitOffset >> 6;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 63;

            // Need at most 1+1 ulongs
            ulong @null = 0;
            ref ulong r1 = ref @null;
            ref ulong r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Build original value
            ulong orig = r0 >> shft;
            orig |= r1 << (64 - shft);

            // Build new masks
            ulong hole1 = ~((ulong)uint.MaxValue >> (64 - shft));
            ulong ins1 = (ulong)insert >> (64 - shft);
            ulong mask1 = (r1 & hole1) | ins1;

            ulong hole0 = ~((ulong)uint.MaxValue << shft);
            ulong ins0 = (ulong)insert << shft;
            ulong mask0 = (r0 & hole0) | ins0;

            // Write element refs
            r1 = mask1;
            r0 = mask0;

            return (uint)orig;
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
            int ix = bitOffset >> 3;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ReadOnlySpan<byte> slice = span.Slice(ix);

            // Need at most 8+1 bytes
            ulong left = 0;
            ulong blit = 0;
            switch (len)
            {
                default:
                case 9: left = slice[8]; goto case 8;
                case 8: blit = ReadUInt64(slice); break;

                case 7: blit = (ulong)slice[6] << 48; goto case 6;
                case 6: blit |= (ulong)slice[5] << 40; goto case 5;
                case 5: blit |= (ulong)slice[4] << 32; goto case 4;
                case 4: blit |= ReadUInt32(slice); break;

                case 3: blit = (ulong)slice[2] << 16; goto case 2;
                case 2: blit |= (ulong)slice[1] << 8; goto case 1;
                case 1: blit |= slice[0]; break;
            }

            int shft = bitOffset & 7;
            blit = (left << (64 - shft)) | (blit >> shft);
            return blit;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ReadOnlySpan<ushort> slice = span.Slice(ix);

            // Need at most 4+1 ushorts
            ulong left = 0;
            ulong blit = 0;
            switch (len)
            {
                default:
                case 5: left = slice[4]; goto case 4;
                case 4: blit = (ulong)slice[3] << 48; goto case 3;
                case 3: blit |= (ulong)slice[2] << 32; goto case 2;
                case 2: blit |= (ulong)slice[1] << 16; goto case 1;
                case 1: blit |= slice[0]; break;
            }

            int shft = bitOffset & 15;
            blit = (left << (64 - shft)) | (blit >> shft);
            return blit;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at most 2+1 uints
            ulong left = 0;
            ulong blit = 0;
            switch (len)
            {
                default:
                case 3: left = span[ix + 2]; goto case 2;
                case 2: blit = (ulong)span[ix + 1] << 32; goto case 1;
                case 1: blit |= span[ix]; break;
            }

            int shft = bitOffset & 31;
            blit = (left << (64 - shft)) | (blit >> shft);
            return blit;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at most 1+1 ulongs
            ulong left = 0;
            ulong blit = 0;
            switch (len)
            {
                default:
                case 2: left = span[ix + 1]; goto case 1;
                case 1: blit = span[ix]; break;
            }

            int shft = bitOffset & 63;
            blit >>= shft;
            blit |= left << (64 - shft);

            return blit;
        }

        #endregion

        #region InsertUInt64

        public static ulong InsertUInt64(Span<byte> span, int bitOffset, ulong insert)
        {
            return 0;
        }

        public static ulong InsertUInt64(Span<ushort> span, int bitOffset, ulong insert)
        {
            int ix = bitOffset >> 4;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 15;

            // Need at most 4+1 ushorts
            ushort @null = 0;
            ref ushort r4 = ref @null;
            ref ushort r3 = ref @null;
            ref ushort r2 = ref @null;
            ref ushort r1 = ref @null;
            ref ushort r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 5: r4 = ref span[ix + 4]; goto case 4;
                case 4: r3 = ref span[ix + 3]; goto case 3;
                case 3: r2 = ref span[ix + 2]; goto case 2;
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Extract original mask & value
            ulong orig = (ulong)r0 >> shft;
            orig |= (ulong)r1 << (16 - shft);
            orig |= (ulong)r2 << (32 - shft);
            orig |= (ulong)r3 << (48 - shft);
            orig |= (ulong)r4 << (64 - shft);

            // Build new mask
            ulong hole4 = ~((ulong)uint.MaxValue << shft);
            ulong hole3 = ~((ulong)uint.MaxValue << shft);
            ulong hole2 = ~((ulong)uint.MaxValue << shft);
            ulong hole1 = ~((ulong)uint.MaxValue >> (32 - shft));
            ulong hole0 = ~((ulong)uint.MaxValue << shft);
            ulong ins4 = insert << shft;
            ulong ins3 = insert << shft;
            ulong ins2 = insert << shft;
            ulong ins1 = insert >> (32 - shft);
            ulong ins0 = insert << shft;

            // Write element refs
            r2 = (ushort)((r4 & hole4) | ins4);
            r2 = (ushort)((r3 & hole3) | ins3);
            r2 = (ushort)((r2 & hole2) | ins2);
            r1 = (ushort)((r1 & hole1) | ins1);
            r0 = (ushort)((r0 & hole0) | ins0);

            // Return original value
            return orig;
        }

        public static ulong InsertUInt64(Span<uint> span, int bitOffset, ulong insert)
        {
            int ix = bitOffset >> 5;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 31;

            // Need at most 2+1 uints
            uint @null = 0;
            ref uint r2 = ref @null;
            ref uint r1 = ref @null;
            ref uint r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 3: r2 = ref span[ix + 2]; goto case 2;
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Build original value
            ulong orig = r0 >> shft;
            orig |= r1 << (32 - shft);
            orig |= r2 >> shft; // BUG: Offsets not correct

            // Build new masks
            uint hole2 = ~(uint.MaxValue << shft);
            uint ins2 = (uint)(insert >> shft);
            uint mask2 = (r2 & hole2) | ins2;

            uint hole1 = ~(uint.MaxValue >> (32 - shft));
            uint ins1 = (uint)(insert >> (32 - shft));
            uint mask1 = (r1 & hole1) | ins1;

            uint hole0 = ~(uint.MaxValue << shft);
            uint ins0 = (uint)(insert << shft);
            uint mask0 = (r0 & hole0) | ins0;

            // Write element refs
            r2 = mask2;
            r1 = mask1;
            r0 = mask0;

            return orig;
        }

        public static ulong InsertUInt64(Span<ulong> span, int bitOffset, ulong insert)
        {
            int ix = bitOffset >> 6;
            int len = span.Length - ix;
            if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 63;

            // Need at most 1+1 ulongs
            ulong @null = 0;
            ref ulong r1 = ref @null;
            ref ulong r0 = ref @null;

            // Read element refs
            switch (len)
            {
                default:
                case 2: r1 = ref span[ix + 1]; goto case 1;
                case 1: r0 = ref span[ix]; break;
            }

            // Build original value
            ulong orig = r0 >> shft;
            orig |= r1 << (64 - shft);

            // Build new masks
            ulong hole1 = ~(ulong.MaxValue >> (64 - shft));
            ulong ins1 = insert >> (64 - shft);
            ulong mask1 = (r1 & hole1) | ins1;

            ulong hole0 = ~(ulong.MaxValue << shft);
            ulong ins0 = insert << shft;
            ulong mask0 = (r0 & hole0) | ins0;

            // Write element refs
            r1 = mask1;
            r0 = mask0;

            return orig;
        }

        #endregion

        #region Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint ReadUInt16(ReadOnlySpan<byte> bytes)
            => Unsafe.ReadUnaligned<ushort>(ref MemoryMarshal.GetReference(bytes));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint ReadUInt32(ReadOnlySpan<byte> bytes)
            => Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(bytes));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ulong ReadUInt64(ReadOnlySpan<byte> bytes)
            => Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(bytes));

        #endregion
    }

#pragma warning restore IDE0007 // Use implicit type
}

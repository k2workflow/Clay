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

            var blit = new Blit32();
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 1+1 bytes
                default:
                case 2: blit.b1 = span[ix + 1]; goto case 1;
                case 1: blit.b0 = span[ix + 0]; break;
            }

            int shft = bitOffset & 7; // mod 8
            blit.u32 >>= shft;
            return blit.b0;
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

            var blit = new Blit32();
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 1+1 ushorts
                default:
                case 2: blit.s1 = span[ix + 1]; goto case 1;
                case 1: blit.s0 = span[ix + 0]; break;
            }

            int shft = bitOffset & 15; // mod 16
            blit.u32 >>= shft;
            return blit.b0;
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

            var blit = new Blit64();
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 1+1 uints
                default:
                case 2: blit.i1 = span[ix + 1]; goto case 1;
                case 1: blit.i0 = span[ix + 0]; break;
            }

            int shft = bitOffset & 31; // mod 32
            blit.u64 >>= shft;
            return blit.b0;
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
                case 02: val = span[ix + 1] << (64 - shft); goto case 1;
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

            var blit = new Blit32();
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 2+1 bytes
                default:
                case 3: blit.b2 = span[ix + 2]; goto case 2;
                case 2: blit.b1 = span[ix + 1]; goto case 1;
                case 1: blit.b0 = span[ix + 0]; break;
            }

            int shft = bitOffset & 7; // mod 8
            blit.u32 >>= shft;
            return blit.s0;
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

            var blit = new Blit32();
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 1+1 ushorts
                default:
                case 2: blit.s1 = span[ix + 1]; goto case 1;
                case 1: blit.s0 = span[ix + 0]; break;
            }

            int shft = bitOffset & 15; // mod 16
            blit.u32 >>= shft;
            return blit.s0;
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

            var blit = new Blit64();
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 1+1 uints
                default:
                case 2: blit.i1 = span[ix + 1]; goto case 1;
                case 1: blit.i0 = span[ix + 0]; break;
            }

            int shft = bitOffset & 31; // mod 32
            blit.u64 >>= shft;
            return blit.s0;
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
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 1+1 ulongs
                default:
                case 02: val = span[ix + 1] << (64 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;
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

            var blit = new Blit64();
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 4+1 bytes
                default:
                case 5: blit.b4 = span[ix + 4]; goto case 4;
                case 4: blit.b3 = span[ix + 3]; goto case 3;
                case 3: blit.b2 = span[ix + 2]; goto case 2;
                case 2: blit.b1 = span[ix + 1]; goto case 1;
                case 1: blit.b0 = span[ix + 0]; break;
            }

            int shft = bitOffset & 7; // mod 8
            blit.u64 >>= shft;
            return blit.i0;
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

            var blit = new Blit64();
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 2+1 ushorts
                default:
                case 3: blit.s2 = span[ix + 2]; goto case 2;
                case 2: blit.s1 = span[ix + 1]; goto case 1;
                case 1: blit.s0 = span[ix + 0]; break;
            }

            int shft = bitOffset & 15; // mod 16
            blit.u64 >>= shft;
            return blit.i0;
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

            var blit = new Blit64();
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 1+1 uints
                default:
                case 2: blit.i1 = span[ix + 1]; goto case 1;
                case 1: blit.i0 = span[ix + 0]; break;
            }

            int shft = bitOffset & 31; // mod 32
            blit.u64 >>= shft;
            return blit.i0;
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
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 1+1 ulongs
                default:
                case 02: val = span[ix + 1] << (1 * 64 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;
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
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ExtractUInt64(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            var blit = new Blit64();
            ulong val = 0;

            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 8+1 bytes
                default:
                    val = (ulong)span[ix + 8] << (64 - shft);
                    goto case 8;

                case 8: blit.u64 = MemoryMarshal.Cast<byte, ulong>(span.Slice(ix, 8))[0]; break;

                case 7: blit.b6 = span[ix + 6]; goto case 6;
                case 6: blit.b5 = span[ix + 5]; goto case 5;
                case 5: blit.b4 = span[ix + 4]; goto case 4;
                case 4: blit.i0 = MemoryMarshal.Cast<byte, uint>(span.Slice(ix, 4))[0]; break;

                case 3: blit.b2 = span[ix + 2]; goto case 2;
                case 2: blit.s0 = MemoryMarshal.Cast<byte, ushort>(span.Slice(ix, 2))[0]; break;

                case 1: blit.b0 = span[ix + 0]; break;
            }

            val |= blit.u64 >> shft;
            return val;
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

            var blit = new Blit64();
            ulong val = 0;

            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 4+1 ushorts
                default:
                    val = (ulong)span[ix + 4] << (64 - shft);
                    goto case 4;

                case 4: blit.s3 = span[ix + 3]; goto case 3;
                case 3: blit.s2 = span[ix + 2]; goto case 2;
                case 2: blit.s1 = span[ix + 1]; goto case 1;
                case 1: blit.s0 = span[ix + 0]; break;
            }

            val |= blit.u64 >> shft;
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

            var blit = new Blit64();
            ulong val = 0;

            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 2+1 uints
                default:
                    val = (ulong)span[ix + 2] << (64 - shft);
                    goto case 2;

                case 2: blit.i1 = span[ix + 1]; goto case 1;
                case 1: blit.i0 = span[ix + 0]; break;
            }

            val |= blit.u64 >> shft;
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
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 1+1 ulongs
                default:
                case 02: val = span[ix + 1] << (64 - shft); goto case 1;
                case 1: val |= span[ix + 0] >> shft; break;
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

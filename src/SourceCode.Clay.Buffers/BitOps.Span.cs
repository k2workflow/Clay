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
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static byte ExtractByte(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 bytes
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<byte> slice = span.Slice(ix, len);
            Span<byte> aligned = stackalloc byte[2];
            slice.CopyTo(aligned);

            Span<ushort> cast = MemoryMarshal.Cast<byte, ushort>(aligned);
            int shft = bitOffset & 7; // mod 8
            uint val = (uint)cast[0] >> shft;

            return (byte)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static byte ExtractByte(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 ushorts
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<ushort> slice = span.Slice(ix, len);
            Span<ushort> aligned = stackalloc ushort[2];
            slice.CopyTo(aligned);

            Span<uint> cast = MemoryMarshal.Cast<ushort, uint>(aligned);
            int shft = bitOffset & 15; // mod 16
            uint val = cast[0] >> shft;

            return (byte)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static byte ExtractByte(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 uints
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<uint> slice = span.Slice(ix, len);
            Span<uint> aligned = stackalloc uint[2];
            slice.CopyTo(aligned);

            Span<ulong> cast = MemoryMarshal.Cast<uint, ulong>(aligned);
            int shft = bitOffset & 31; // mod 32
            ulong val = cast[0] >> shft;

            return (byte)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static byte ExtractByte(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 ulongs
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<ulong> slice = span.Slice(ix, len);
            Span<ulong> aligned = stackalloc ulong[2];
            slice.CopyTo(aligned);

            int shft = bitOffset & 63; // mod 64
            ulong val = aligned[0] >> shft;
            val |= aligned[1] << (64 - shft);

            return (byte)val;
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

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 2+1 bytes, so align on 4
            int len = Math.Min(span.Length - ix, 4);
            ReadOnlySpan<byte> slice = span.Slice(ix, len);
            Span<byte> aligned = stackalloc byte[4];
            slice.CopyTo(aligned);

            Span<uint> cast = MemoryMarshal.Cast<byte, uint>(aligned);
            int shft = bitOffset & 7; // mod 8
            uint val = cast[0] >> shft;

            return (ushort)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 ushorts
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<ushort> slice = span.Slice(ix, len);
            Span<ushort> aligned = stackalloc ushort[2];
            slice.CopyTo(aligned);

            Span<uint> cast = MemoryMarshal.Cast<ushort, uint>(aligned);
            int shft = bitOffset & 15; // mod 16
            uint val = cast[0] >> shft;

            return (ushort)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 uints
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<uint> slice = span.Slice(ix, len);
            Span<uint> aligned = stackalloc uint[2];
            slice.CopyTo(aligned);

            Span<ulong> cast = MemoryMarshal.Cast<uint, ulong>(aligned);
            int shft = bitOffset & 31; // mod 32
            ulong val = cast[0] >> shft;

            return (ushort)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static ushort ExtractUInt16(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 ulongs
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<ulong> slice = span.Slice(ix, len);
            Span<ulong> aligned = stackalloc ulong[2];
            slice.CopyTo(aligned);

            int shft = bitOffset & 63; // mod 64
            ulong val = aligned[0] >> shft;
            val |= aligned[1] << (64 - shft);

            return (ushort)val;
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

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 4+1 bytes, so align on 8
            int len = Math.Min(span.Length - ix, 8);
            ReadOnlySpan<byte> slice = span.Slice(ix, len);
            Span<byte> aligned = stackalloc byte[8];
            slice.CopyTo(aligned);

            Span<ulong> cast = MemoryMarshal.Cast<byte, ulong>(aligned);
            int shft = bitOffset & 7; // mod 8
            ulong val = cast[0] >> shft;

            return (uint)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 2+1 ushorts, so align on 4
            int len = Math.Min(span.Length - ix, 4);
            ReadOnlySpan<ushort> slice = span.Slice(ix, len);
            Span<ushort> aligned = stackalloc ushort[4];
            slice.CopyTo(aligned);

            Span<ulong> cast = MemoryMarshal.Cast<ushort, ulong>(aligned);
            int shft = bitOffset & 15; // mod 16
            ulong val = cast[0] >> shft;

            return (uint)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 uints
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<uint> slice = span.Slice(ix, len);
            Span<uint> aligned = stackalloc uint[2];
            slice.CopyTo(aligned);

            Span<ulong> cast = MemoryMarshal.Cast<uint, ulong>(aligned);
            int shft = bitOffset & 31; // mod 32
            ulong val = cast[0] >> shft;

            return (uint)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 ulongs
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<ulong> slice = span.Slice(ix, len);
            Span<ulong> aligned = stackalloc ulong[2];
            slice.CopyTo(aligned);

            int shft = bitOffset & 63; // mod 64
            ulong val = aligned[0] >> shft;
            val |= aligned[1] << (64 - shft);

            return (uint)val;
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

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 8+1 bytes, so align on 16
            int len = Math.Min(span.Length - ix, 16);
            ReadOnlySpan<byte> slice = span.Slice(ix, len);
            Span<byte> aligned = stackalloc byte[16];
            slice.CopyTo(aligned);

            Span<ulong> cast = MemoryMarshal.Cast<byte, ulong>(aligned);
            int shft = bitOffset & 7; // mod 8
            ulong val = cast[0] >> shft;
            val |= cast[1] << (64 - shft);

            return val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 4+1 ushorts, so align on 8
            int len = Math.Min(span.Length - ix, 8);
            ReadOnlySpan<ushort> slice = span.Slice(ix, len);
            Span<ushort> aligned = stackalloc ushort[8];
            slice.CopyTo(aligned);

            Span<ulong> cast = MemoryMarshal.Cast<ushort, ulong>(aligned);
            int shft = bitOffset & 15; // mod 16
            ulong val = cast[0] >> shft;
            val |= cast[1] << (64 - shft);

            return val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 2+1 uints, so align on 4
            int len = Math.Min(span.Length - ix, 4);
            ReadOnlySpan<uint> slice = span.Slice(ix, len);
            Span<uint> aligned = stackalloc uint[8];
            slice.CopyTo(aligned);

            Span<ulong> cast = MemoryMarshal.Cast<uint, ulong>(aligned);
            int shft = bitOffset & 31; // mod 32
            ulong val = cast[0] >> shft;
            val |= cast[1] << (64 - shft);

            return val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.</param>
        public static ulong ExtractUInt64(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 ulongs
            int len = Math.Min(span.Length - ix, 2);
            ReadOnlySpan<ulong> slice = span.Slice(ix, len);
            Span<ulong> aligned = stackalloc ulong[2];
            slice.CopyTo(aligned);

            int shft = bitOffset & 63; // mod 64
            ulong val = aligned[0] >> shft;
            val |= aligned[1] << (64 - shft);

            return val;
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

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 bytes
            ReadOnlySpan<byte> byts = span.Slice(ix);
            uint val = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(byts));

            val >>= bitOffset & 7; // mod 8
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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 ushorts
            ReadOnlySpan<byte> byts = MemoryMarshal.AsBytes(span.Slice(ix));
            uint val = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(byts));

            val >>= bitOffset & 15; // mod 16
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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 uints
            ReadOnlySpan<byte> byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));

            val >>= bitOffset & 31; // mod 32
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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 63; // mod 64

            ReadOnlySpan<byte> byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));
            val >>= shft;

            // Need at least 1+1 ulongs
            var ix1 = ix + 1;
            if (ix1 < span.Length)
                val |= span[ix1] << (64 - shft); // rem 64

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

            // Need at least 1+1 bytes
            var byts = span.Slice(ix);
            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(byts), (ushort)value);
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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 2+1 bytes
            ReadOnlySpan<byte> byts = span.Slice(ix);
            uint val = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(byts));

            val >>= bitOffset & 7; // mod 8
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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 ushorts
            ReadOnlySpan<byte> byts = MemoryMarshal.AsBytes(span.Slice(ix));
            uint val = Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(byts));

            val >>= bitOffset & 15; // mod 16
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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 uints
            ReadOnlySpan<byte> byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));

            val >>= bitOffset & 31; // mod 32
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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 63; // mod 64

            ReadOnlySpan<byte> byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));
            val >>= shft;

            // Need at least 1+1 ulongs
            var ix1 = ix + 1;
            if (ix1 < span.Length)
                val |= span[ix1] << (64 - shft); // rem 64

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 4+1 bytes
            ReadOnlySpan<byte> byts = span.Slice(ix);
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));

            val >>= bitOffset & 7; // mod 8
            return (uint)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 2+1 ushorts
            ReadOnlySpan<byte> byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));

            val >>= bitOffset & 15; // mod 16
            return (uint)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 1+1 uints
            ReadOnlySpan<byte> byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));

            val >>= bitOffset & 31; // mod 32
            return (uint)val;
        }

        /// <summary>
        /// Reads the specified byte from a span, given the bit offset.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="bitOffset">The ordinal position to read.</param>
        public static uint ExtractUInt32(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 63; // mod 64

            ReadOnlySpan<byte> byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));
            val >>= shft;
            
            // Need at least 1+1 ulongs
            var ix1 = ix + 1;
            if (ix1 < span.Length)
                val |= span[ix1] << (64 - shft); // rem 64

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 7; // mod 8

            ReadOnlySpan<byte> byts = span.Slice(ix);
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));
            val >>= shft;

            // Need at least 8+1 bytes
            var ix8 = ix + 8;
            if (ix8 < span.Length)
                val |= (ulong)span[ix8] << (8 - shft); // rem 8

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 15; // mod 16

            var byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));
            val >>= shft;

            // Need at least 4+1 ushorts
            var ix4 = ix + 4;
            if (ix4 < span.Length)
                val |= (ulong)span[ix4] << (16 - shft); // rem 16

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 31; // mod 32

            var byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));
            val >>= shft;

            // Need at least 2+1 uints
            var ix2 = ix + 2;
            if (ix2 < span.Length)
                val |= (ulong)span[ix2] << (32 - shft); // rem 32

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));
            int shft = bitOffset & 63; // mod 64

            var byts = MemoryMarshal.AsBytes(span.Slice(ix));
            ulong val = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(byts));
            val >>= shft;

            // Need at least 1+1 ulongs
            var ix1 = ix + 1;
            if (ix1 < span.Length)
                val |= span[ix1] << (64 - shft); // rem 64

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

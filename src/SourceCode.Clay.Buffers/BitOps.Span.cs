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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ExtractByteImpl(ReadOnlySpan<byte> span, int ix, byte bit)
        {
            // Avoid branching by treating True/False as 1/0
            byte t_1 = BoolToByte.TrueTo1(ix + 1 < span.Length); // 1 if within range, else 0
            int iy = ix + t_1; // (ix + 1) if within range, else (ix)

            var num = (ushort)span[iy] * t_1; // ([iy] x 1) if within range, else (0)
            num <<= sizeof(byte);
            num |= span[ix];

            byte val = ExtractByte((ushort)num, bit);
            return val;
        }

        public static byte ExtractByte(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            var bit = (byte)(bitOffset & 7); // mod 8

            byte val = ExtractByteImpl(span, ix, bit);
            return val;
        }

        public static byte ExtractByte(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ReadOnlySpan<byte> cast = MemoryMarshal.Cast<ushort, byte>(span);
            var bit = (byte)(bitOffset & 15); // mod 16

            byte val = ExtractByteImpl(cast, ix, bit);
            return val;
        }

        public static byte ExtractByte(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ReadOnlySpan<byte> cast = MemoryMarshal.Cast<uint, byte>(span);
            var bit = (byte)(bitOffset & 31); // mod 32

            byte val = ExtractByteImpl(cast, ix, bit);
            return val;
        }

        public static byte ExtractByte(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ReadOnlySpan<byte> cast = MemoryMarshal.Cast<ulong, byte>(span);
            var bit = (byte)(bitOffset & 63); // mod 64

            byte val = ExtractByteImpl(cast, ix, bit);
            return val;
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
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            var bit = (byte)(bitOffset & 7); // mod 8

            // Byte 0
            uint num = span[ix];

            // Byte 1
            byte t_1 = BoolToByte.TrueTo1(ix + 1 < span.Length);
            var n = (uint)span[ix + t_1] * t_1;
            num |= n << sizeof(ushort);

            // Byte 2
            t_1 += BoolToByte.TrueTo1(ix + 2 < span.Length);
            n = (uint)span[ix + t_1] * t_1;
            num |= n << sizeof(ushort);

            // Byte 3
            t_1 += BoolToByte.TrueTo1(ix + 3 < span.Length);
            n = (uint)span[ix + t_1] * t_1;
            num |= n << sizeof(ushort);

            ushort val = ExtractUInt16(num, bit);
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ushort ExtractUInt16Impl(ReadOnlySpan<ushort> span, int ix, byte bit)
        {
            byte t_1 = BoolToByte.TrueTo1(ix + 1 < span.Length);
            var num = (uint)span[ix + t_1] * t_1;
            num <<= sizeof(ushort);
            num |= span[ix];

            ushort val = ExtractUInt16(num, bit);
            return val;
        }

        public static ushort ExtractUInt16(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            var bit = (byte)(bitOffset & 15); // mod 16

            ushort val = ExtractUInt16Impl(span, ix, bit);
            return val;
        }

        public static ushort ExtractUInt16(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ReadOnlySpan<ushort> cast = MemoryMarshal.Cast<uint, ushort>(span);
            var bit = (byte)(bitOffset & 31); // mod 32

            ushort val = ExtractUInt16Impl(cast, ix, bit);
            return val;
        }

        public static ushort ExtractUInt16(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ReadOnlySpan<ushort> cast = MemoryMarshal.Cast<ulong, ushort>(span);
            var bit = (byte)(bitOffset & 63); // mod 64

            ushort val = ExtractUInt16Impl(cast, ix, bit);
            return val;
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
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            var bit = (byte)(bitOffset & 7); // mod 8

            // Byte 0
            ulong num = span[ix];

            // Byte 1
            byte t_1 = BoolToByte.TrueTo1(ix + 1 < span.Length);
            var n = (ulong)span[ix + t_1] * t_1;
            num |= n << sizeof(uint);

            // Byte 2
            t_1 += BoolToByte.TrueTo1(ix + 2 < span.Length);
            n = (ulong)span[ix + t_1] * t_1;
            num |= n << sizeof(uint);

            // Byte 3
            t_1 += BoolToByte.TrueTo1(ix + 3 < span.Length);
            n = (ulong)span[ix + t_1] * t_1;
            num |= n << sizeof(uint);

            // Byte 4
            t_1 = BoolToByte.TrueTo1(ix + 4 < span.Length);
            n = (ulong)span[ix + t_1] * t_1;
            num |= n << sizeof(uint);

            // Byte 5
            t_1 += BoolToByte.TrueTo1(ix + 5 < span.Length);
            n = (ulong)span[ix + t_1] * t_1;
            num |= n << sizeof(uint);

            // Byte 6
            t_1 += BoolToByte.TrueTo1(ix + 6 < span.Length);
            n = (ulong)span[ix + t_1] * t_1;
            num |= n << sizeof(uint);

            // Byte 7
            t_1 += BoolToByte.TrueTo1(ix + 7 < span.Length);
            n = (ulong)span[ix + t_1] * t_1;
            num |= n << sizeof(uint);

            uint val = ExtractUInt32(num, bit);
            return val;
        }

        public static uint ExtractUInt32(ReadOnlySpan<ushort> span, int bitOffset)
        {
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ExtractUInt32Impl(ReadOnlySpan<uint> span, int ix, byte bit)
        {
            byte t_1 = BoolToByte.TrueTo1(ix + 1 < span.Length);
            var num = (ulong)span[ix + t_1] * t_1;
            num <<= sizeof(uint);
            num |= span[ix];

            uint val = ExtractUInt32(num, bit);
            return val;
        }

        public static uint ExtractUInt32(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            var bit = (byte)(bitOffset & 31); // mod 32

            uint val = ExtractUInt32Impl(span, ix, bit);
            return val;
        }

        public static uint ExtractUInt32(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ReadOnlySpan<uint> cast = MemoryMarshal.Cast<ulong, uint>(span);
            var bit = (byte)(bitOffset & 63); // mod 64

            uint val = ExtractUInt32Impl(cast, ix, bit);
            return val;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ExtractUInt64Impl(ReadOnlySpan<ulong> span, int ix, byte bit)
        {
            byte t_1 = BoolToByte.TrueTo1(ix + 1 < span.Length);
            var num = (ulong)span[ix + t_1] * t_1;
            num <<= sizeof(uint);
            num |= span[ix];

            //ulong val = ExtractUInt64(num, bit);
            //return val;

            return 0;
        }

        public static ulong ExtractUInt64(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            var bit = (byte)(bitOffset & 63); // mod 64

            ulong val = ExtractUInt64Impl(span, ix, bit);
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

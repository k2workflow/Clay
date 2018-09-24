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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset)); // TODO: Perf; do we want these guards?

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

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
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            ref ulong val = ref span[ix];

            var btc = ComplementBit(ref val, offset);
            return btc;
        }

        #endregion

        #region ExtractByte

        public static byte ExtractByte(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(byte), sizeof(byte), bitOffset);

            // 0
            var val = (uint)span[ix] >> iy;

            // TODO: Benchmark vs BoolToByte
            
            // 1
            if (ix + 1 < span.Length)
            {
                val |= (uint)span[ix + 1] << (sizeof(byte) - iy);
            }

            return (byte)val;
        }

        public static byte ExtractByte(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(byte), sizeof(ushort), bitOffset);

            // 0
            var val = (uint)span[ix] >> iy;

            // 1
            if (ix + 1 < span.Length)
            {
                val |= (uint)span[ix + 1] << (sizeof(ushort) - iy);
            }

            return (byte)val;
        }

        public static byte ExtractByte(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(byte), sizeof(uint), bitOffset);

            // 0
            uint val = span[ix] >> iy;

            // 1
            if (ix + 1 < span.Length)
            {
                val |= span[ix + 1] << (sizeof(uint) - iy);
            }

            return (byte)val;
        }

        public static byte ExtractByte(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(byte), sizeof(ulong), bitOffset);

            // 0
            ulong val = span[ix] >> iy;

            // 1
            if (ix + 1 < span.Length)
            {
                val |= span[ix + 1] << (sizeof(ulong) - iy);
            }

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

        public static ushort ExtractUInt16(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(ushort), sizeof(byte), bitOffset);

            // 0
            var val = (uint)span[ix] >> iy;

            switch (span.Length - 1 - ix)
            {
                // 0
                case 0: break;

                // 1
                case 1:
                    val |= (uint)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 2..N
                default:
                    val |= (uint)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (uint)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;
            }

            return (byte)val;
        }

        public static ushort ExtractUInt16(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(ushort), sizeof(ushort), bitOffset);

            // 0
            var val = (uint)span[ix] >> iy;

            // 1
            if (ix + 1 < span.Length)
            {
                val |= (uint)span[ix + 1] << (sizeof(ushort) - iy);
            }

            return (ushort)val;
        }

        public static ushort ExtractUInt16(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(ushort), sizeof(uint), bitOffset);

            // 0
            uint val = span[ix] >> iy;

            // 1
            if (ix + 1 < span.Length)
            {
                val |= span[ix + 1] << (sizeof(uint) - iy);
            }

            return (ushort)val;
        }

        public static ushort ExtractUInt16(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(ushort), sizeof(ulong), bitOffset);

            // 0
            ulong val = span[ix] >> iy;

            // 1
            if (ix + 1 < span.Length)
            {
                val |= span[ix + 1] << (sizeof(ulong) - iy);
            }

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

        public static uint ExtractUInt32(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(uint), sizeof(byte), bitOffset);

            // 0
            var val = (uint)span[ix] >> iy;

            switch (span.Length - 1 - ix)
            {
                // 0
                case 0: break;

                // 1
                case 1:
                    val |= (uint)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 2
                case 2:
                    val |= (uint)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (uint)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 3
                case 3:
                    val |= (uint)span[ix + 3] << (3 * sizeof(byte) - iy);
                    val |= (uint)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (uint)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 4..N
                default:
                    val |= (uint)span[ix + 4] << (4 * sizeof(byte) - iy);
                    val |= (uint)span[ix + 3] << (3 * sizeof(byte) - iy);
                    val |= (uint)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (uint)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;
            }

            return val;
        }

        public static uint ExtractUInt32(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(uint), sizeof(ushort), bitOffset);

            // 0
            var val = (uint)span[ix] >> iy;

            switch (span.Length - 1 - ix)
            {
                // 0
                case 0: break;

                // 1
                case 1:
                    val |= (uint)span[ix + 1] << (1 * sizeof(ushort) - iy);
                    break;

                // 2..N
                case 2:
                    val |= (uint)span[ix + 2] << (2 * sizeof(ushort) - iy);
                    val |= (uint)span[ix + 1] << (1 * sizeof(ushort) - iy);
                    break;
            }

            return val;
        }

        public static uint ExtractUInt32(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(uint), sizeof(uint), bitOffset);

            // 0
            uint val = span[ix] >> iy;

            // 1
            if (ix + 1 < span.Length)
            {
                val |= span[ix + 1] << (sizeof(uint) - iy);
            }

            return val;
        }

        public static uint ExtractUInt32(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(uint), sizeof(ulong), bitOffset);

            // 0
            var val = (uint)span[ix] >> iy;

            // 1
            if (ix + 1 < span.Length)
            {
                val |= (uint)span[ix + 1] << (sizeof(uint) - iy);
            }

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
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(ulong), sizeof(byte), bitOffset);

            // 0
            var val = (ulong)span[ix] >> iy;

            switch (span.Length - 1 - ix)
            {
                // 0
                case 0: break;

                // 1
                case 1:
                    val |= (ulong)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 2
                case 2:
                    val |= (ulong)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 3
                case 3:
                    val |= (ulong)span[ix + 3] << (3 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 4
                case 4:
                    val |= (ulong)span[ix + 4] << (4 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 3] << (3 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 5
                case 5:
                    val |= (ulong)span[ix + 5] << (5 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 4] << (4 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 3] << (3 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 6
                case 6:
                    val |= (ulong)span[ix + 6] << (6 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 5] << (5 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 4] << (4 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 3] << (3 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 7
                case 7:
                    val |= (ulong)span[ix + 7] << (7 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 6] << (6 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 5] << (5 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 4] << (4 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 3] << (3 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;

                // 8..N
                case 8:
                    val |= (ulong)span[ix + 8] << (8 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 7] << (7 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 6] << (6 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 5] << (5 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 4] << (4 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 3] << (3 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 2] << (2 * sizeof(byte) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(byte) - iy);
                    break;
            }

            return val;
        }

        public static ulong ExtractUInt64(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(ulong), sizeof(ushort), bitOffset);

            // 0
            var val = (ulong)span[ix] >> iy;

            switch (span.Length - 1 - ix)
            {
                // 0
                case 0: break;

                // 1
                case 1:
                    val |= (ulong)span[ix + 1] << (1 * sizeof(ushort) - iy);
                    break;

                // 2
                case 2:
                    val |= (ulong)span[ix + 2] << (2 * sizeof(ushort) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(ushort) - iy);
                    break;

                // 3
                case 3:
                    val |= (ulong)span[ix + 3] << (3 * sizeof(ushort) - iy);
                    val |= (ulong)span[ix + 2] << (2 * sizeof(ushort) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(ushort) - iy);
                    break;

                // 4..N
                case 4:
                    val |= (ulong)span[ix + 4] << (4 * sizeof(ushort) - iy);
                    val |= (ulong)span[ix + 3] << (3 * sizeof(ushort) - iy);
                    val |= (ulong)span[ix + 2] << (2 * sizeof(ushort) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(ushort) - iy);
                    break;
            }

            return val;
        }

        public static ulong ExtractUInt64(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(ulong), sizeof(uint), bitOffset);

            // 0
            var val = (ulong)span[ix] >> iy;

            switch (span.Length - 1 - ix)
            {
                // 0
                case 0: break;

                // 1
                case 1:
                    val |= (ulong)span[ix + 1] << (1 * sizeof(uint) - iy);
                    break;

                // 2..N
                case 2:
                    val |= (ulong)span[ix + 2] << (2 * sizeof(uint) - iy);
                    val |= (ulong)span[ix + 1] << (1 * sizeof(uint) - iy);
                    break;
            }

            return val;
        }

        public static ulong ExtractUInt64(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            byte iy = Mod(sizeof(ulong), sizeof(ulong), bitOffset);

            // 0
            ulong val = span[ix] >> iy;

            // 1
            if (ix + 1 < span.Length)
            {
                val |= span[ix + 1] << (sizeof(ulong) - iy);
            }

            return (byte)val;
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

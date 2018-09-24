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

        public static byte ExtractByte(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = span.Length - ix;
            byte shft = Mod(sizeof(byte), sizeof(byte), bitOffset);

            uint val = 0;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    val = (uint)span[ix + 1] << (sizeof(byte) - shft);
                    goto case 1; // Fallthru

                // [0]
                case 1:
                    val |= (uint)span[ix] >> shft;
                    break;
            }

            return (byte)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }
    
        public static byte ExtractByte(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            var len = span.Length - ix;
            byte shft = Mod(sizeof(byte), sizeof(ushort), bitOffset);

            uint val;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    ReadOnlySpan<uint> cast2 = MemoryMarshal.Cast<ushort, uint>(span);
                    val = cast2[ix << 1] >> shft;
                    break;

                // [0]
                case 1:
                    val = (uint)span[ix] >> shft;
                    break;
            }

            return (byte)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static byte ExtractByte(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            var len = span.Length - ix;
            byte shft = Mod(sizeof(byte), sizeof(uint), bitOffset);

            uint val = 0;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    val = span[ix + 1] << (sizeof(uint) - shft);
                    goto case 1; // Fallthru

                // [0]
                case 1:
                    val |= span[ix] >> shft;
                    break;
            }

            return (byte)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static byte ExtractByte(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            var len = span.Length - ix;
            byte shft = Mod(sizeof(byte), sizeof(ulong), bitOffset);

            ulong val = 0;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    val = span[ix + 1] << (sizeof(ulong) - shft);
                    goto case 1; // Fallthru

                // [0]
                case 1:
                    val |= span[ix] >> shft;
                    break;
            }

            return (byte)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
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
            var len = span.Length - ix;
            byte shft = Mod(sizeof(ushort), sizeof(byte), bitOffset);

            uint val = 0;
            switch (len)
            {
                // [0,1,2,3,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 4;

                // [0,1,2,3]
                case 4:
                    ReadOnlySpan<uint> cast4 = MemoryMarshal.Cast<byte, uint>(span);
                    val = cast4[ix << 2] >> shft;
                    break;

                // [0,1,2]
                case 3:
                    val = (uint)span[ix + 2] << (sizeof(ushort) - shft);
                    goto case 2; // Fallthru

                // [0,1]
                case 2:
                    ReadOnlySpan<ushort> cast2 = MemoryMarshal.Cast<byte, ushort>(span);
                    val |= (uint)cast2[ix << 1] >> shft;
                    break;

                // [0]
                case 1:
                    val |= (uint)span[ix] >> shft;
                    break;
            }

            return (byte)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static ushort ExtractUInt16(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            var len = span.Length - ix;
            byte shft = Mod(sizeof(ushort), sizeof(ushort), bitOffset);

            uint val;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    ReadOnlySpan<uint> cast2 = MemoryMarshal.Cast<ushort, uint>(span);
                    val = cast2[ix << 1] >> shft;
                    break;
         
                // [0]
                case 1:
                    val = (uint)span[ix] >> shft;
                    break;
            }

            return (ushort)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static ushort ExtractUInt16(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            var len = span.Length - ix;
            byte shft = Mod(sizeof(ushort), sizeof(uint), bitOffset);

            ulong val;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    ReadOnlySpan<ulong> cast2 = MemoryMarshal.Cast<uint, ulong>(span);
                    val = cast2[ix << 1] >> shft;
                    break;

                // [0]
                case 1:
                    val = span[ix] >> shft;
                    break;
            }

            return (ushort)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static ushort ExtractUInt16(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            var len = span.Length - ix;
            byte shft = Mod(sizeof(ushort), sizeof(ulong), bitOffset);

            ulong val = 0;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    val = span[ix + 1] << (sizeof(ulong) - shft);
                    goto case 1; // Fallthru

                // [0]
                case 1:
                    val |= span[ix] >> shft;
                    break;
            }

            return (ushort)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
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
            var len = span.Length - ix;
            byte shft = Mod(sizeof(uint), sizeof(byte), bitOffset);

            ulong val = 0;
            switch (len)
            {
                // [0,1,2,3,4,5,6,7,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 8;

                // [0,1,2,3,4,5,6,7]
                case 8:
                    ReadOnlySpan<ulong> cast8 = MemoryMarshal.Cast<byte, ulong>(span);
                    val = cast8[ix << 3] >> shft;
                    break;

                // [0,1,2,3,4,5,6]
                case 7:
                    val = (ulong)span[ix + 6] << (6 * sizeof(byte) - shft);
                    goto case 6; // Fallthru

                // [0,1,2,3,4,5]
                case 6:
                    val |= (ulong)span[ix + 5] << (5 * sizeof(byte) - shft);
                    goto case 5; // Fallthru

                // [0,1,2,3,4]
                case 5:
                    val |= (ulong)span[ix + 4] << (4 * sizeof(byte) - shft);
                    goto case 4; // Fallthru

                // [0,1,2,3]
                case 4:
                    ReadOnlySpan<uint> cast4 = MemoryMarshal.Cast<byte, uint>(span);
                    val |= cast4[ix << 2] >> shft;
                    break;

                // [0,1,2]
                case 3:
                    val = (ulong)span[ix + 2] << (2 * sizeof(byte) - shft);
                    goto case 2; // Fallthru

                // [0,1]
                case 2:
                    ReadOnlySpan<ushort> cast2 = MemoryMarshal.Cast<byte, ushort>(span);
                    val |= (ulong)cast2[ix << 1] >> shft;
                    break;
                
                // [0]
                case 1:
                    val = (ulong)span[ix] >> shft;
                    break;
            }

            return (uint)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static uint ExtractUInt32(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            var len = span.Length - ix;
            byte shft = Mod(sizeof(uint), sizeof(ushort), bitOffset);

            ulong val = 0;
            switch (len)
            {
                // [0,1,2,3,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 4;

                // [0,1,2,3]
                case 4:
                    ReadOnlySpan<ulong> cast4 = MemoryMarshal.Cast<ushort, ulong>(span);
                    val = cast4[ix << 2] >> shft;
                    break;

                // [0,1,2]
                case 3:
                    val = (ulong)span[ix + 2] << (sizeof(uint) - shft);
                    goto case 2; // Fallthru

                // [0,1]
                case 2:
                    ReadOnlySpan<uint> cast2 = MemoryMarshal.Cast<ushort, uint>(span);
                    val |= cast2[ix << 1] >> shft;
                    break;

                // [0]
                case 1:
                    val = (uint)span[ix] >> shft;
                    break;
            }

            return (uint)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static uint ExtractUInt32(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            var len = span.Length - ix;
            byte shft = Mod(sizeof(uint), sizeof(uint), bitOffset);

            ulong val;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    ReadOnlySpan<ulong> cast2 = MemoryMarshal.Cast<uint, ulong>(span);
                    val = cast2[ix << 1] >> shft;
                    break;

                // [0]
                case 1:
                    val = span[ix] >> shft;
                    break;
            }

            return (uint)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static uint ExtractUInt32(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            var len = span.Length - ix;
            byte shft = Mod(sizeof(uint), sizeof(ulong), bitOffset);

            ulong val = 0;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    val = span[ix + 1] << (sizeof(ulong) - shft);
                    goto case 1; // Fallthru

                // [0]
                case 1:
                    val |= span[ix] >> shft;
                    break;
            }

            return (uint)val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
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
            var len = span.Length - ix;
            byte shft = Mod(sizeof(ulong), sizeof(byte), bitOffset);

            ulong val = 0;
            switch (len)
            {
                // [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 16;

                // [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15]
                case 16:
                    ReadOnlySpan<ulong> cast16 = MemoryMarshal.Cast<byte, ulong>(span);
                    val = cast16[ix << 3] >> shft;
                    val |= cast16[ix << 3 + 1] >> (15 * sizeof(byte) - shft);
                    break;

                // [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14]
                case 15:
                    val = (ulong)span[ix + 14] << (14 * sizeof(byte) - shft);
                    goto case 14; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9,10,11,12,13]
                case 14:
                    val |= (ulong)span[ix + 13] << (13 * sizeof(byte) - shft);
                    goto case 13; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9,10,11,12]
                case 13:
                    val |= (ulong)span[ix + 12] << (12 * sizeof(byte) - shft);
                    goto case 12; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9,10,11]
                case 12:
                    val |= (ulong)span[ix + 11] << (11 * sizeof(byte) - shft);
                    goto case 11; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9,10]
                case 11:
                    val |= (ulong)span[ix + 10] << (10 * sizeof(byte) - shft);
                    goto case 10; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9]
                case 10:
                    val |= (ulong)span[ix + 9] << (9 * sizeof(byte) - shft);
                    goto case 9; // Fallthru

                // [0,1,2,3,4,5,6,7,8]
                case 9:
                    val |= (ulong)span[ix + 8] << (8 * sizeof(byte) - shft);
                    goto case 8; // Fallthru

                // [0,1,2,3,4,5,6,7]
                case 8:
                    ReadOnlySpan<ulong> cast8 = MemoryMarshal.Cast<byte, ulong>(span);
                    val |= cast8[ix << 3] >> shft;
                    break;

                // [0,1,2,3,4,5,6]
                case 7:
                    val = (ulong)span[ix + 6] << (6 * sizeof(byte) - shft);
                    goto case 6; // Fallthru

                // [0,1,2,3,4,5]
                case 6:
                    val |= (ulong)span[ix + 5] << (5 * sizeof(byte) - shft);
                    goto case 5; // Fallthru

                // [0,1,2,3,4]
                case 5:
                    val |= (ulong)span[ix + 4] << (4 * sizeof(byte) - shft);
                    goto case 4; // Fallthru

                // [0,1,2,3]
                case 4:
                    ReadOnlySpan<uint> cast4 = MemoryMarshal.Cast<byte, uint>(span);
                    val |= (ulong)cast4[ix << 2] >> shft;
                    break;

                // [0,1,2]
                case 3:
                    val = (ulong)span[ix + 2] << (2 * sizeof(byte) - shft);
                    goto case 2; // Fallthru

                // [0,1]
                case 2:
                    ReadOnlySpan<ushort> cast2 = MemoryMarshal.Cast<byte, ushort>(span);
                    val |= (ulong)cast2[ix << 1] >> shft;
                    break;

                // [0]
                case 1:
                    val = (ulong)span[ix] >> shft;
                    break;
            }

            return val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static ulong ExtractUInt64(ReadOnlySpan<ushort> span, int bitOffset)
        {
            int ix = bitOffset >> 4; // div 16
            var len = span.Length - ix;
            byte shft = Mod(sizeof(ulong), sizeof(ushort), bitOffset);

            ulong val = 0;
            switch (len)
            {
                // [0,1,2,3,4,5,6,7,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 8;

                // [0,1,2,3,4,5,6,7]
                case 8:
                    ReadOnlySpan<ulong> cast8 = MemoryMarshal.Cast<ushort, ulong>(span);
                    val = cast8[ix << 2] >> shft;
                    val |= cast8[ix << 2 + 1] >> (sizeof(ulong) - shft);
                    break;

                // [0,1,2,3,4,5,6]
                case 7:
                    val = (ulong)span[ix + 6] << (6 * sizeof(ushort) - shft);
                    goto case 6; // Fallthru

                // [0,1,2,3,4,5]
                case 6:
                    val |= (ulong)span[ix + 5] << (5 * sizeof(ushort) - shft);
                    goto case 5; // Fallthru

                // [0,1,2,3,4]
                case 5:
                    val |= (ulong)span[ix + 4] << (4 * sizeof(ushort) - shft);
                    goto case 4; // Fallthru

                // [0,1,2,3]
                case 4:
                    ReadOnlySpan<ulong> cast4 = MemoryMarshal.Cast<ushort, ulong>(span);
                    val |= cast4[ix << 2] >> shft;
                    break;

                // [0,1,2]
                case 3:
                    val |= (ulong)span[ix + 2] << (2 * sizeof(ushort) - shft);
                    goto case 2; // Fallthru

                // [0,1]
                case 2:
                    ReadOnlySpan<uint> cast2 = MemoryMarshal.Cast<ushort, uint>(span);
                    val |= (ulong)cast2[ix << 1] >> shft;
                    break;

                // [0]
                case 1:
                    val = (ulong)span[ix] >> shft;
                    break;
            }

            return val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static ulong ExtractUInt64(ReadOnlySpan<uint> span, int bitOffset)
        {
            int ix = bitOffset >> 5; // div 32
            var len = span.Length - ix;
            byte shft = Mod(sizeof(ulong), sizeof(uint), bitOffset);

            ulong val = 0;
            switch (len)
            {
                // [0,1,2,3,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 4;

                // [0,1,2,3]
                case 4:
                    ReadOnlySpan<ulong> cast4 = MemoryMarshal.Cast<uint, ulong>(span);
                    val = cast4[ix << 1] >> shft;
                    val |= cast4[ix << 1 + 1] >> (sizeof(ulong) - shft);
                    break;

                // [0,1,2]
                case 3:
                    val = (ulong)span[ix + 2] << (sizeof(ulong) - shft);
                    goto case 2; // Fallthru

                // [0,1]
                case 2:
                    ReadOnlySpan<ulong> cast2 = MemoryMarshal.Cast<uint, ulong>(span);
                    val |= cast2[ix << 1] >> shft;
                    break;

                // [0]
                case 1:
                    val = (ulong)span[ix] >> shft;
                    break;
            }

            return val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }

        public static ulong ExtractUInt64(ReadOnlySpan<ulong> span, int bitOffset)
        {
            int ix = bitOffset >> 6; // div 64
            var len = span.Length - ix;
            byte shft = Mod(sizeof(ulong), sizeof(ulong), bitOffset);

            ulong val = 0;
            switch (len)
            {
                // [0,1,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 2;

                // [0,1]
                case 2:
                    val = span[ix + 1] << (sizeof(ulong) - shft);
                    goto case 1; // Fallthru

                // [0]
                case 1:
                    val |= span[ix] >> shft;
                    break;
            }

            return val;

        Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
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

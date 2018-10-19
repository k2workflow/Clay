using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System
{
    partial class BitOps // .Other
    {
        #region ExtractByte

        public static byte ExtractByte(ushort value, int bitOffset)
            => (byte)(value >> (bitOffset & 15));

        public static byte ExtractByte(uint value, int bitOffset)
            => (byte)(value >> (bitOffset & 31));

        public static byte ExtractByte(ulong value, int bitOffset)
            => (byte)(value >> (bitOffset & 63));

        #endregion

        #region InsertByte

        public static ushort InsertByte(ushort value, int bitOffset, byte insert)
        {
            // eg, offset = 3
            // value = 1100_1100_1001_1101
            // insert =      100 0011 1

            // eg 27->3, int.Max -> 7
            int shft = bitOffset & 15; // mod 16
            uint ins = (uint)(insert << shft); //           0000_0 | 100_0 011_1 | 000

            //                                             15  12 | 11  8 7   3 |   0
            uint mask = ~((uint)byte.MaxValue << shft); // 1111_1 | 000_0 000_0 | 111

            // value                                       1100_1 | 100_1 001_1 | 101
            uint val = value & mask; //                    1100_1 | 000_0 000_0 | 101
            val |= ins; //                                 1100_1 | 100_0 011_1 | 101

            // value               1100_1 1001 0011 101
            // insert                     1000 0111
            return (ushort)val; // 1100_1 1000 0111 101
        }

        public static uint InsertByte(uint value, int bitOffset, byte insert)
        {
            int shft = bitOffset & 31;
            uint ins = (uint)(insert << shft);

            uint mask = ~((uint)byte.MaxValue << shft);

            uint val = (value & mask) | ins;
            return val;
        }

        public static ulong InsertByte(ulong value, int bitOffset, byte insert)
        {
            int shft = bitOffset & 63;
            ulong ins = (ulong)(insert << shft);

            ulong mask = ~((ulong)byte.MaxValue << shft);

            ulong val = (value & mask) | ins;
            return val;
        }

        #endregion

        #region ExtractUInt16

        public static ushort ExtractUInt16(uint value, int bitOffset)
            => (ushort)(value >> (bitOffset & 31));

        public static ushort ExtractUInt16(ulong value, int bitOffset)
            => (ushort)(value >> (bitOffset & 63));

        #endregion

        #region InsertUInt16

        public static uint InsertUInt16(uint value, int bitOffset, ushort insert)
        {
            int shft = bitOffset & 31;
            uint ins = (uint)(insert << shft);

            uint mask = ~((uint)ushort.MaxValue << shft);

            uint val = (value & mask) | ins;
            return val;
        }

        public static ulong InsertUInt16(ulong value, int bitOffset, ushort insert)
        {
            int shft = bitOffset & 63;
            ulong ins = (ulong)(insert << shft);

            ulong mask = ~((ulong)ushort.MaxValue << shft);

            ulong val = (value & mask) | ins;
            return val;
        }

        #endregion

        #region ExtractUInt32

        public static uint ExtractUInt32(ulong value, int bitOffset)
            => (uint)(value >> (bitOffset & 63));

        #endregion

        #region InsertUInt32

        public static ulong InsertUInt32(ulong value, int bitOffset, uint insert)
        {
            int shft = bitOffset & 63;
            ulong ins = insert << shft;

            ulong mask = ~((ulong)uint.MaxValue << shft);

            ulong val = (value & mask) | ins;
            return val;
        }

        #endregion

        #region IsPowerOf2        

        /// <summary>
        /// Returns True if the value is a power of 2, else False.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOf2(uint value)
            => (value != 0)
            && // Note: && causes branch
            (value & (value - 1)) == 0;

        /// <summary>
        /// Returns True if the value is a power of 2, else False.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOf2(ulong value)
            => (value != 0)
            && // Note: && causes branch
            (value & (value - 1)) == 0;

        #endregion

        #region Parity

        /// <summary>
        /// Returns 1 if the bit count is odd, else 0.
        /// Logically equivalent to PopCount mod 2.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Parity(uint value)
        {
            uint val = value;

            val ^= val >> 16;
            val ^= val >> 08;
            val ^= val >> 04;
            val &= 15;

            val = 0b_0110_1001_1001_0110u >> (int)val;
            val &= 1;

            return (int)val;
        }

        /// <summary>
        /// Returns 1 if the bit count is odd, else 0.
        /// Logically equivalent to PopCount mod 2.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Parity(ulong value)
        {
            ulong val = value;

            val ^= val >> 32;
            val ^= val >> 16;
            val ^= val >> 08;
            val ^= val >> 04;
            val &= 15;

            val = 0b_0110_1001_1001_0110u >> (int)val;
            val &= 1;

            return (int)val;
        }

        #endregion

        #region Log2Low

        /// <summary>
        /// Computes the highest power of 2 lower than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2Low(uint value)
        {
            // Perf: Do not use guard clauses; callers must be trusted
            Debug.Assert(value > 0);

            uint val = CascadeTrailing(value);
            uint ix = (val * DeBruijn32) >> 27;

            byte log = s_deBruijn32[ix];
            return log;
        }

        /// <summary>
        /// Computes the highest power of 2 lower than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2Low(ulong value)
        {
            // Perf: Do not use guard clauses; callers MUST be trusted
            Debug.Assert(value > 0);

            // We only have to count the low-32 or the high-32, depending on limits

            // Assume we need only examine low-32
            uint val = (uint)value;
            byte inc = 0;

            // If high-32 is non-zero
            if (value > uint.MaxValue)
            {
                // Then we need only examine high-32 (and add 32 to the result)
                val = (uint)(value >> 32); // Use high-32 instead
                inc = 32;
            }

            // Examine 32
            return inc + Log2Low(val);
        }

        #endregion

        #region Log2High

        /// <summary>
        /// Computes the lowest power of 2 greater than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2High(uint value)
            => Log2Low(value) + 1;

        /// <summary>
        /// Computes the lowest power of 2 greater than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2High(ulong value)
            => Log2Low(value) + 1;

        #endregion

        #region Pow2Low

        // TODO: Perf

        /// <summary>
        /// Computes the next power of 2 greater than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint Pow2Low(uint value)
        {
            Debug.Assert(value < uint.MaxValue);

            uint val = value;

            // If zero, add 1
            val += If(value == 0, 1U);

            //         77                   0100 1101
            val--; //  76                   0100 1100 (for exact powers of 2)
            val = CascadeTrailing(val); //  0111 1111
            val++; // 128                   1000 0000 (for exact powers of 2)

            return val;
        }

        /// <summary>
        /// Computes the next power of 2 greater than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ulong Pow2Low(ulong value)
        {
            Debug.Assert(value < ulong.MaxValue);

            ulong val = value;

            // If zero, add 1
            val += If(value == 0, 1UL);

            val--;
            val = CascadeTrailing(val);
            val++;

            return val;
        }

        #endregion

        #region Pow2High

        /// <summary>
        /// Computes the previous power of 2 less than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint Pow2High(uint value)
            => Pow2Low(value) - 1;

        /// <summary>
        /// Computes the previous power of 2 less than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ulong Pow2High(ulong value)
            => Pow2Low(value) - 1;

        #endregion
    }
}

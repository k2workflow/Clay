using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System
{
    partial class BitOps // .Other
    {
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
            CascadeTrailing(ref val); //    0111 1111
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
            CascadeTrailing(ref val);
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

using System.Diagnostics;
using System.Runtime.CompilerServices;

// Utilize when/if non-Experimental:
//using System.Runtime.Intrinsics;
//using System.Runtime.Intrinsics.X86;

namespace System
{
#pragma warning disable IDE0007 // Use implicit type

    /// <summary>
    /// Represents additional blit methods.
    /// </summary>
    [CLSCompliant(false)]
    public static partial class BitOps // .Primitive
    {
        #region ExtractBit

        // For bitlength N, it is conventional to treat N as congruent modulo-N 
        // under the shift operation.
        // So for uint, 1 << 33 == 1 << 1, and likewise 1 << -46 == 1 << +18.
        // Note -46 % 32 == -14. But -46 & 31 (0011_1111) == +18. So we use & not %.
        // Software & hardware intrinsics already do this for uint/ulong, but
        // we need to emulate for byte/ushort.

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(byte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            return (value & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(ushort value, int bitOffset)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            return (value & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(uint value, int bitOffset)
        {
            uint mask = 1U << bitOffset;

            return (value & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.
        /// Any value outside the range [0..63] is treated as congruent mod 63.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(ulong value, int bitOffset)
        {
            ulong mask = 1UL << bitOffset;

            return (value & mask) != 0;
        }

        #endregion

        #region WriteBit (Scalar)

        /// <summary>
        /// Writes the specified bit in a mask and returns the new value.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteBit(byte value, int bitOffset, bool on)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            uint onn = Evaluate(on, 1);
            onn <<= shft;

            return (byte)((value & ~mask) | onn);
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns the new value.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort WriteBit(ushort value, int bitOffset, bool on)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            uint onn = Evaluate(on, 1);
            onn <<= shft;

            return (ushort)((value & ~mask) | onn);
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns the new value.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint WriteBit(uint value, int bitOffset, bool on)
        {
            uint mask = 1U << bitOffset;

            uint onn = Evaluate(on, 1);
            onn <<= bitOffset;

            return (value & ~mask) | onn;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns the new value.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong WriteBit(ulong value, int bitOffset, bool on)
        {
            ulong mask = 1UL << bitOffset;

            ulong onn = Evaluate(on, 1);
            onn <<= bitOffset;

            return (value & ~mask) | onn;
        }

        #endregion

        #region WriteBit (Ref)

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref byte value, int bitOffset, bool on)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            uint onn = Evaluate(on, 1);
            onn <<= shft;

            uint btw = value & mask;
            value = (byte)((value & ~mask) | onn);

            return btw != 0;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref ushort value, int bitOffset, bool on)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            uint onn = Evaluate(on, 1);
            onn <<= shft;

            uint btw = value & mask;
            value = (ushort)((value & ~mask) | onn);

            return btw != 0;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref uint value, int bitOffset, bool on)
        {
            uint mask = 1U << bitOffset;

            uint onn = Evaluate(on, 1);
            onn <<= bitOffset;

            uint btw = value & mask;
            value = (value & ~mask) | onn;

            return btw != 0;
        }

        /// <summary>
        /// Writes the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <param name="on"/>True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref ulong value, int bitOffset, bool on)
        {
            ulong mask = 1UL << bitOffset;

            ulong onn = Evaluate(on, 1);
            onn <<= bitOffset;

            ulong btw = value & mask;
            value = (value & ~mask) | onn;

            return btw != 0;
        }

        #endregion

        #region ClearBit (Scalar)

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ClearBit(byte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            return (byte)(value & ~mask);
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ClearBit(ushort value, int bitOffset)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            return (ushort)(value & ~mask);
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ClearBit(uint value, int bitOffset)
        {
            uint mask = 1U << bitOffset;

            return value & ~mask;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ClearBit(ulong value, int bitOffset)
        {
            ulong mask = 1UL << bitOffset;

            return value & ~mask;
        }

        #endregion

        #region ClearBit (Ref)

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref byte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            uint btr = value & mask;
            value = (byte)(value & ~mask);

            return btr != 0;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref ushort value, int bitOffset)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            uint btr = value & mask;
            value = (ushort)(value & ~mask);

            return btr != 0;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref uint value, int bitOffset)
        {
            uint mask = 1U << bitOffset;

            uint btr = value & mask;
            value = value & ~mask;

            return btr != 0;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref ulong value, int bitOffset)
        {
            ulong mask = 1UL << bitOffset;

            ulong btr = value & mask;
            value = value & ~mask;

            return btr != 0;
        }

        #endregion

        #region InsertBit (Scalar)

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte InsertBit(byte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            return (byte)(value | mask);
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..15] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort InsertBit(ushort value, int bitOffset)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            return (ushort)(value | mask);
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint InsertBit(uint value, int bitOffset)
        {
            uint mask = 1U << bitOffset;

            return value | mask;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong InsertBit(ulong value, int bitOffset)
        {
            ulong mask = 1UL << bitOffset;

            return value | mask;
        }

        #endregion

        #region InsertBit (Ref)

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref byte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;
            
            uint bts = value & mask;
            value = (byte)(value | mask);

            return bts != 0;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref ushort value, int bitOffset)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            uint bts = value & mask;
            value = (ushort)(value | mask);

            return bts != 0;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref uint value, int bitOffset)
        {
            uint mask = 1U << bitOffset;

            uint bts = value & mask;
            value = value | mask;

            return bts != 0;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref ulong value, int bitOffset)
        {
            ulong mask = 1UL << bitOffset;

            ulong bts = value & mask;
            value = value | mask;

            return bts != 0;
        }

        #endregion

        #region ComplementBit (Scalar)

        // Truth table (1)
        // v   m  | ~m  ^v  ~
        // 00  01 | 10  10  01
        // 01  01 | 10  11  00
        // 10  01 | 10  00  11
        // 11  01 | 10  01  10
        //                      
        // 00  10 | 01  01  10
        // 01  10 | 01  00  11
        // 10  10 | 01  11  00
        // 11  10 | 01  10  01

        /// <summary>
        /// Complements the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ComplementBit(byte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            mask = ~(~mask ^ value);
            return (byte)mask;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ComplementBit(ushort value, int bitOffset)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            mask = ~(~mask ^ value);
            return (ushort)mask;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ComplementBit(uint value, int bitOffset)
        {            
            uint mask = 1U << bitOffset;

            mask = ~(~mask ^ value);
            return mask;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ComplementBit(ulong value, int bitOffset)
        {
            ulong mask = 1UL << bitOffset;

            mask = ~(~mask ^ value);
            return mask;
        }

        #endregion

        #region ComplementBit (Ref)

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref byte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            uint btc = value & mask;
            value = (byte)~(~mask ^ value);

            return btc != 0;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref ushort value, int bitOffset)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            uint btc = value & mask;
            value = (ushort)~(~mask ^ value);

            return btc != 0;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref uint value, int bitOffset)
        {
            uint mask = 1U << bitOffset;

            uint btc = value & mask;
            value = ~(~mask ^ value);

            return btc != 0;
        }

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref ulong value, int bitOffset)
        {
            ulong mask = 1UL << bitOffset;

            ulong btc = value & mask;
            value = ~(~mask ^ value);

            return btc != 0;
        }

        #endregion

        #region Rotate

        // Will compile to instrinsics if pattern complies (uint/ulong):
        // https://github.com/dotnet/coreclr/pull/1830
        // There is NO intrinsics support for byte/ushort rotation.

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateLeft(byte value, int offset)
        {
            int shft = offset & 7;
            var val = (uint)value;
            
            // Will NOT compile to instrinsics
            val = (val << shft) | (val >> (8 - shft));
            return (byte)val;
        }

        /// <summary>
        /// Rotates the specified value right by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROR.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateRight(byte value, int offset)
        {
            int shft = offset & 7;
            var val = (uint)value;

            // Will NOT compile to instrinsics
            val = (val >> shft) | (val << (8 - shft));
            return (byte)val;
        }

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateLeft(ushort value, int offset)
        {
            int shft = offset & 15;
            var val = (uint)value;

            // Will NOT compile to instrinsics
            val = (val << shft) | (val >> (16 - shft));
            return (ushort)val;
        }

        /// <summary>
        /// Rotates the specified value right by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROR.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateRight(ushort value, int offset)
        {
            int shft = offset & 15;
            var val = (uint)value;

            // Will NOT compile to instrinsics
            val = (val >> shft) | (val << (16 - shft));
            return (ushort)val;
        }

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, int offset)
        {
            uint val = (value << offset) | (value >> (32 - offset));
            return val;
        }

        /// <summary>
        /// Rotates the specified value right by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROR.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(uint value, int offset)
        {
            uint val = (value >> offset) | (value << (32 - offset));
            return val;
        }

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(ulong value, int offset)
        {
            ulong val = (value << offset) | (value >> (64 - offset));
            return val;
        }

        /// <summary>
        /// Rotates the specified value right by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROR.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateRight(ulong value, int offset)
        {
            ulong val = (value >> offset) | (value << (64 - offset));
            return val;
        }

        #endregion

        #region PopCount

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(byte value)
            => PopCount((uint)value); // Delegate to intrinsic

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(ushort value)
            => PopCount((uint)value); // Delegate to intrinsic

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(uint value)
        {
            const uint c0 = 0x_5555_5555;
            const uint c1 = 0x_3333_3333;
            const uint c2 = 0x_0F0F_0F0F;
            const uint c3 = 0x_0101_0101;

            uint val = value;

            val -= (val >> 1) & c0;
            val = (val & c1) + ((val >> 2) & c1);
            val = (val + (val >> 4)) & c2;
            val *= c3;
            val >>= 24;

            return (int)val;
        }

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(ulong value)
        {
            const ulong c0 = 0x_5555_5555_5555_5555;
            const ulong c1 = 0x_3333_3333_3333_3333;
            const ulong c2 = 0x_0F0F_0F0F_0F0F_0F0F;
            const ulong c3 = 0x_0101_0101_0101_0101;

            ulong val = value;

            val -= (value >> 1) & c0;
            val = (val & c1) + ((val >> 2) & c1);
            val = (val + (val >> 4)) & c2;
            val *= c3;
            val >>= 56;

            return (int)val;
        }

        #endregion

        #region LeadingZeros

        private static readonly byte[] s_deBruijn32 = new byte[32]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        private const uint deBruijn32 = 0x07C4_ACDDu;

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(byte value)
            => LeadingZeros((uint)value) - 24; // Delegate to intrinsic

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(ushort value)
            => LeadingZeros((uint)value) - 16; // Delegate to intrinsic

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(uint value)
        {
            uint val = value;

            //                   1000 0000 0000 0000 0000 0000 0000 0000
            val |= val >> 01; // 1100 0000 0000 0000 0000 0000 0000 0000
            val |= val >> 02; // 1111 0000 0000 0000 0000 0000 0000 0000
            val |= val >> 04; // 1111 1111 0000 0000 0000 0000 0000 0000
            val |= val >> 08; // 1111 1111 1111 1111 0000 0000 0000 0000
            val |= val >> 16; // 1111 1111 1111 1111 1111 1111 1111 1111

            uint ix = (val * deBruijn32) >> 27;
            int zeros = 31 - s_deBruijn32[ix];

            // Log(0) is undefined: Return 32.
            zeros += Evaluate(value == 0, 1);

            return zeros;
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(ulong value)
        {
            ulong val = value;

            //                   1000 0000 0000 0000 0000 0000 0000 0000 0...
            val |= val >> 01; // 1100 0000 0000 0000 0000 0000 0000 0000 0...
            val |= val >> 02; // 1111 0000 0000 0000 0000 0000 0000 0000 0...
            val |= val >> 04; // 1111 1111 0000 0000 0000 0000 0000 0000 0...
            val |= val >> 08; // 1111 1111 1111 1111 0000 0000 0000 0000 0...
            val |= val >> 16; // 1111 1111 1111 1111 1111 1111 1111 1111 0...
            val |= val >> 32; // 1111 1111 1111 1111 1111 1111 1111 1111 1...

            // Instead of using a 64-bit lookup table,
            // we use the existing 32-bit table twice.

            uint mv = (uint)(val >> 32); // High-32
            uint nv = (uint)val; // Low-32

            uint mi = (mv * deBruijn32) >> 27;
            uint ni = (nv * deBruijn32) >> 27;

            int mz = 31 - s_deBruijn32[mi];
            int nz = 31 - s_deBruijn32[ni]; // Use warm cache

            // Log(0) is undefined: Return 32 + 32.
            mz += Evaluate((value >> 32) == 0, 1);
            nz += Evaluate((uint)value == 0, 1);

            // Truth table
            // m   n  m32 actual   m + (n * m32)
            // 32 32  1   32+32   32 + (32 * 1)
            // 32  n  1   32+n    32 + (n * 1)
            // m  32  0   m        m + (32 * 0)
            // m   n  0   m        m + (n * 0)

            nz *= Evaluate(mz == 32, 1); // Only add n if m != 32
            return mz + nz;
        }

        #endregion

        #region LeadingOnes

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(byte value)
            => LeadingZeros((byte)~value);

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(ushort value)
            => LeadingZeros((ushort)~value);

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(uint value)
            => LeadingZeros(~value);

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(ulong value)
            => LeadingZeros(~value);

        #endregion

        #region TrailingZeros

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(byte value)
            => Math.Min(8, TrailingZeros((uint)value)); // Delegate to intrinsic

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(ushort value)
            => Math.Min(16, TrailingZeros((uint)value)); // Delegate to intrinsic

        // Build this table by taking n = 0,1,2,4,...
        // [2^n % 37] = tz(n) manually counted
        // 37 is the closest prime greater than the range's cardinality of 33.
        private static readonly byte[] s_trail32u = new byte[37] // mod 37
        {
            //                2^n  % 37       b=bin(n)                                 z=tz(b)
            32, //              0  [ 0]       0000_0000_0000_0000_0000_0000_0000_0000  32
            00, //              1  [ 1]       0000_0000_0000_0000_0000_0000_0000_0001   0
            01, //              2  [ 2]       0000_0000_0000_0000_0000_0000_0000_0010   1
            26,

            02, //              4  [ 4]       0000_0000_0000_0000_0000_0000_0000_0100   2
            23,
            27,
            32, //  4,294,967,296  [ 7]  0001_0000_0000_0000_0000_0000_0000_0000_0000  32 (n/a) 1ul << 32

            03, //              8  [ 8]       0000_0000_0000_0000_0000_0000_0000_1000   3
            16,
            24,
            30,

            28,
            11, //           2048  [13]       0000_0000_0000_0000_0000_1000_0000_0000  11
            33, //  8,589,934,592  [14]  0010_0000_0000_0000_0000_0000_0000_0000_0000  33 (n/a) 1ul << 33
            13,

            04, //             16  [16]       0000_0000_0000_0000_0000_0000_0001_0000   4
            07, //            128  [17]       0000_0000_0000_0000_0000_0000_1000_0000   7
            17,
            35, // 34,359,738,368  [19]  1000_0000_0000_0000_0000_0000_0000_0000_0000  35 (n/a) 1ul << 35

            25,
            22,
            31,
            15, //           8192  [15]       0000_0000_0000_0000_0010_0000_0000_0000  13

            29,
            10, //           1024  [25]       0000_0000_0000_0000_0000_0100_0000_0000  10
            12, //           4096  [26]       0000_0000_0000_0000_0001_0000_0000_0000  12
            06, //             64  [27]       0000_0000_0000_0000_0000_0000_0100_0000   6

            34, // 17,179,869,184  [28]  0100_0000_0000_0000_0000_0000_0000_0000_0000  34 (n/a) 1ul << 34
            21,
            14,
            09, //            512  [31]       0000_0000_0000_0000_0000_0010_0000_0000   9

            05, //             32  [32]       0000_0000_0000_0000_0000_0000_0010_0000   5
            20,
            08, //            256  [34]       0000_0000_0000_0000_0000_0001_0000_0000   8
            19,

            18  //        262,144  [36]
        };

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(uint value)
        {
            // The expression (n & -n) returns lsb(n).
            // Only possible values are therefore [0,1,2,4,...]
            long lsb = value & -value; // eg 44==0010 1100 -> (44 & -44) -> 4. 4==0100, which is the lsb of 44.

            // We want to map [0,1,2,3,...] to the smallest contiguous range, ideally [0..33] since 33 is the range cardinality.
            // Mod-37 is a simple perfect-hashing scheme over this range, where 37 is chosen as the closest prime greater than 33.
            lsb %= 37; // mod 37

            // Benchmark: Lookup is 2x faster than Switch
            // Method |     Mean |     Error | StdDev    | Scaled |
            //------- | ---------| ----------| ----------| -------|
            // Lookup | 2.920 ns | 0.0893 ns | 0.2632 ns |   1.00 |
            // Switch | 6.548 ns | 0.1301 ns | 0.2855 ns |   2.26 |

            byte cnt = s_trail32u[lsb]; // eg 44 -> 4 -> 2 (44==0010 1100 has 2 trailing zeros)

            // NoOp: Hashing scheme has unused outputs (inputs 4,294,967,296 and higher do not fit a uint)
            Debug.Assert(lsb != 7 && lsb != 14 && lsb != 19 && lsb != 28, $"{value} resulted in unexpected {typeof(uint)} hash {lsb}, with count {cnt}");

            return cnt;
        }

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(ulong value)
        {
            // Instead of using a 64-bit lookup table,
            // we use the existing 32-bit table twice.

            var hv = (uint)(value >> 32); // High-32
            var bv = (uint)value; // Low-32

            long hi = hv & -hv;
            long bi = bv & -bv;

            hi %= 37; // mod 37
            bi %= 37;

            byte h = s_trail32u[hi];
            byte b = s_trail32u[bi]; // Use warm cache

            // Truth table
            // h   b  b32 actual  b + (h * b32 ? 1 : 0)
            // 32 32  1   32+32  32 + (32 * 1)
            // 32  b  0   b       b + (32 * 0)
            // h  32  1   32+h   32 + (h * 1)
            // h   b  0   b       b + (h * 0)

            h = Evaluate(b == 32, h, 0); // Only add h if b==32
            return b + h;
        }

        #endregion

        #region TrailingOnes

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(byte value)
            => TrailingZeros((byte)~value);

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(ushort value)
            => TrailingZeros((ushort)~value);

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(uint value)
            => TrailingZeros(~value);

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(ulong value) 
            => TrailingZeros(~value);

        #endregion

        #region Log2Low

        /// <summary>
        /// Computes the highest power of 2 lower than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2Low(byte value)
        {
            // Perf: Do not use guard clauses; callers must be trusted
            Debug.Assert(value > 0);

            return Log2Low((uint)value); // Delegate to intrinsic
        }

        /// <summary>
        /// Computes the highest power of 2 lower than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2Low(ushort value)
        {
            // Perf: Do not use guard clauses; callers must be trusted
            Debug.Assert(value > 0);

            return Log2Low((uint)value); // Delegate to intrinsic
        }

        /// <summary>
        /// Computes the highest power of 2 lower than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2Low(uint value)
        {
            // Perf: Do not use guard clauses; callers must be trusted
            Debug.Assert(value > 0);

            uint val = value;

            //                   1000 0000 0000 0000 0000 0000 0000 0000
            val |= val >> 01; // 1100 0000 0000 0000 0000 0000 0000 0000
            val |= val >> 02; // 1111 0000 0000 0000 0000 0000 0000 0000
            val |= val >> 04; // 1111 1111 0000 0000 0000 0000 0000 0000
            val |= val >> 08; // 1111 1111 1111 1111 0000 0000 0000 0000
            val |= val >> 16; // 1111 1111 1111 1111 1111 1111 1111 1111

            uint ix = (val * deBruijn32) >> 27;

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
            var val = (uint)value;
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
        internal static int Log2High(byte value)
            => Log2Low(value) + 1;

        /// <summary>
        /// Computes the lowest power of 2 greater than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2High(ushort value)
            => Log2Low(value) + 1;
    
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
        internal static uint Pow2Low(byte value)
        {
            Debug.Assert(value < byte.MaxValue);
            
            return Pow2Low((uint)value); // Delegate to intrinsic
        }

        /// <summary>
        /// Computes the next power of 2 greater than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint Pow2Low(ushort value)
        {
            Debug.Assert(value < ushort.MaxValue);

            return Pow2Low((uint)value); // Delegate to intrinsic
        }

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
            val += Evaluate(value == 0, 1);

            //         77        0100 1101
            val--; //  76        0100 1100 (for exact powers of 2)
            val |= val >> 01; // 0110 1110
            val |= val >> 02; // 0111 1111
            val |= val >> 04; // 0111 1111
            val |= val >> 08; // 0111 1111
            val |= val >> 16; // 0111 1111
            val++; // 128        1000 0000 (for exact powers of 2)

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
            val += Evaluate(value == 0, 1);

            val--;
            val |= val >> 01;
            val |= val >> 02;
            val |= val >> 04;
            val |= val >> 08;
            val |= val >> 16;
            val |= val >> 32;
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
        internal static uint Pow2High(byte value)
            => Pow2Low(value) - 1;

        /// <summary>
        /// Computes the previous power of 2 less than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint Pow2High(ushort value)
            => Pow2Low(value) - 1;

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

        #region IsPowerOf2

        /// <summary>
        /// Returns True if the value is a power of 2, else False.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOf2(byte value)
            => IsPowerOf2((uint)value); // Delegate to intrinsic

        /// <summary>
        /// Returns True if the value is a power of 2, else False.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOf2(ushort value)
            => IsPowerOf2((uint)value); // Delegate to intrinsic

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
        public static int Parity(byte value)
            => Parity((uint)value); // Delegate to intrinsic

        /// <summary>
        /// Returns 1 if the bit count is odd, else 0.
        /// Logically equivalent to PopCount mod 2.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Parity(ushort value)
            => Parity((uint)value); // Delegate to intrinsic

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

        #region Evaluate

        /// <summary>
        /// Converts a bool to a byte value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Evaluate(bool condition, byte trueValue, byte falseValue = 0)
            => (byte)Evaluate(condition, (uint)trueValue, falseValue); // Delegate to intrinsic

        /// <summary>
        /// Converts a bool to a ushort value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Evaluate(bool condition, ushort trueValue, ushort falseValue = 0)
            => (ushort)Evaluate(condition, (uint)trueValue, falseValue); // Delegate to intrinsic

        /// <summary>
        /// Converts a bool to a uint value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Evaluate(bool condition, uint trueValue, uint falseValue = 0)
        {
            uint val = Unsafe.As<bool, byte>(ref condition); // 1|0

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return val;
        }

        /// <summary>
        /// Converts a bool to a ulong value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Evaluate(bool condition, ulong trueValue, ulong falseValue = 0)
        {
            ulong val = Unsafe.As<bool, byte>(ref condition); // 1|0

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return val;
        }

        #endregion

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
            var ins = (uint)(insert << shft); //           0000_0 | 100_0 011_1 | 000

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
            var ins = (uint)(insert << shft);

            uint mask = ~((uint)byte.MaxValue << shft);

            uint val = (value & mask) | ins;
            return val;
        }

        public static ulong InsertByte(ulong value, int bitOffset, byte insert)
        {
            int shft = bitOffset & 63;
            var ins = (ulong)(insert << shft);

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
            var ins = (uint)(insert << shft);

            uint mask = ~((uint)ushort.MaxValue << shft);

            uint val = (value & mask) | ins;
            return val;
        }

        public static ulong InsertUInt16(ulong value, int bitOffset, ushort insert)
        {
            int shft = bitOffset & 63;
            var ins = (ulong)(insert << shft);

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
            var ins = (ulong)(insert << shft);

            ulong mask = ~((ulong)uint.MaxValue << shft);

            ulong val = (value & mask) | ins;
            return val;
        }

        #endregion
    }

#pragma warning restore IDE0007 // Use implicit type
}

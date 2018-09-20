using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
#pragma warning disable IDE0007 // Use implicit type

    /// <summary>
    /// Represents additional blit methods.
    /// </summary>
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

            uint onn = BoolToByte.Unsafe(on); // true ? 1 : 0
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

            uint onn = BoolToByte.Unsafe(on); // true ? 1 : 0
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

            uint onn = BoolToByte.Unsafe(on); // true ? 1 : 0
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

            ulong onn = BoolToByte.Unsafe(on); // true ? 1 : 0
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

            uint onn = BoolToByte.Unsafe(on); // true ? 1 : 0
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

            uint onn = BoolToByte.Unsafe(on); // true ? 1 : 0
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

            uint onn = BoolToByte.Unsafe(on); // true ? 1 : 0
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

            ulong onn = BoolToByte.Unsafe(on); // true ? 1 : 0
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
            => PopCount((uint)value); // May compile to instrinsics

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(ushort value)
            => PopCount((uint)value); // May compile to instrinsics

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

        private const uint c_deBruijn32 = 0x07C4_ACDDu;

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(byte value)
        {
            uint val = value;

            //                   1000 0000
            val |= val >> 01; // 1100 0000
            val |= val >> 02; // 1111 0000
            val |= val >> 04; // 1111 1111

            uint ix = (val * c_deBruijn32) >> 27;
            int zeros = 7 - s_deBruijn32[ix];
            
            // Log(0) is undefined: Return 8.
            zeros += BoolToByte.Unsafe(value == 0);

            return zeros;
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(ushort value)
        {
            uint val = value;

            //                   1000 0000 0000 0000
            val |= val >> 01; // 1100 0000 0000 0000
            val |= val >> 02; // 1111 0000 0000 0000
            val |= val >> 04; // 1111 1111 0000 0000
            val |= val >> 08; // 1111 1111 1111 1111

            uint ix = (val * c_deBruijn32) >> 27;
            int zeros = 15 - s_deBruijn32[ix];

            // Log(0) is undefined: Return 16.
            zeros += BoolToByte.Unsafe(value == 0);

            return zeros;
        }

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

            uint ix = (val * c_deBruijn32) >> 27;
            int zeros = 31 - s_deBruijn32[ix];

            // Log(0) is undefined: Return 32.
            zeros += BoolToByte.Unsafe(value == 0);

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

            uint mi = (mv * c_deBruijn32) >> 27;
            uint ni = (nv * c_deBruijn32) >> 27;

            int mz = 31 - s_deBruijn32[mi];
            int nz = 31 - s_deBruijn32[ni]; // Use warm cache

            // Log(0) is undefined: Return 32 + 32.
            mz += BoolToByte.Unsafe((value >> 32) == 0);
            nz += BoolToByte.Unsafe((uint)value == 0);

            // Truth table
            // m   n  m32 actual   m + (n * m32)
            // 32 32  1   32+32   32 + (32 * 1)
            // 32  n  1   32+n    32 + (n * 1)
            // m  32  0   m        m + (32 * 0)
            // m   n  0   m        m + (n * 0)

            nz *= BoolToByte.Unsafe(mz == 32); // Only add n if m != 32
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

        // Build this table by taking n = 0,1,2,4,...,512
        // [2^n % 11] = tz(n) manually counted
        private static readonly byte[] s_trail08u = new byte[11] // mod 11
        {
            //    2^n  % 11     b=bin(n)   z=tz(b)
            8, //   0  [ 0]     0000_0000  8
            0, //   1  [ 1]     0000_0001  0 
            1, //   2  [ 2]     0000_0010  1
            8, // 256  [ 3]  01_0000_0000  8 (n/a) 1u << 8
               
            2, //   4  [ 4]     0000_0100  2
            4, //  16  [ 5]     0001_0000  4
            9, // 512  [ 6]  10_0000_0000  9 (n/a) 1u << 9
            7, // 128  [ 7]     1000_0000  7
               
            3, //   8  [ 8]     0000_1000  3
            6, //  64  [ 9]     0100_0000  6
            5, //  32  [10]     0010_0000  5
        };

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(byte value)
        {
            // The expression (n & -n) returns lsb(n).
            // Only possible values are therefore [0,1,2,4,...,128]
            int lsb = value & -value; // eg 44==0010 1100 -> (44 & -44) -> 4. 4==0100, which is the lsb of 44.

            // We want to map [0...128] to the smallest contiguous range, ideally [0..9] since 9 is the range cardinality.
            // Mod-11 is a simple perfect-hashing scheme over this range, where 11 is chosen as the closest prime greater than 9.
            lsb %= 11; // mod 11

            // Benchmark: Lookup is 2x faster than Switch
            // Method |     Mean |     Error | StdDev    | Scaled |
            //------- | ---------| ----------| ----------| -------|
            // Lookup | 2.920 ns | 0.0893 ns | 0.2632 ns |   1.00 |
            // Switch | 6.548 ns | 0.1301 ns | 0.2855 ns |   2.26 |

            byte cnt = s_trail08u[lsb]; // eg 44 -> 4 -> 2 (44==0010 1100 has 2 trailing zeros)
            
            // NoOp: Hashing scheme has unused outputs (inputs 256 and higher do not fit a byte)
            Debug.Assert(lsb != 3 && lsb != 6, $"{value} resulted in unexpected {typeof(byte)} hash {lsb}, with count {cnt}");

            return cnt;
        }

        // See algorithm notes in TrailingCount(byte).
        // 19 is the closest prime greater than the range's cardinality of 17.
        private static readonly byte[] s_trail16u = new byte[19]
        {
            //        2^n  % 19     b=bin(n)             z=tz(b)
            16, //      0  [ 0]     0000_0000_0000_0000  16
            00, //      1  [ 1]     0000_0000_0000_0001   0
            01, //      2  [ 2]     0000_0000_0000_0010   1
            13, //   8192  [ 3]     0010_0000_0000_0000  13

            02, //      4  [ 4]     0000_0000_0000_0100   2
            16, //  65536  [ 5]  01_0000_0000_0000_0000  16 (n/a) 1u << 16
            14, //  16384  [ 6]     0100_0000_0000_0000  14
            06, //     64  [ 7]     0000_0000_0100_0000   6

            03, //      8  [ 8]     0000_0000_0000_1000   3
            08, //    256  [ 9]     0000_0001_0000_0000   8
            17, // 131072  [10]  10_0000_0000_0000_0000  17 (n/a) 1u << 17
            12, //   4096  [11]     0001_0000_0000_0000  12

            15, //  32768  [12]     1000_0000_0000_0000  15
            05, //     32  [13]     0000_0000_0010_0000   5
            07, //    128  [14]     0000_0000_1000_0000   7
            11, //   2048  [15]     0000_1000_0000_0000  11

            04, //     16  [16]     0000_0000_0001_0000   4
            10, //   1024  [17]     0000_0100_0000_0000  10
            09  //    512  [18]     0000_0010_0000_0000   9
        };

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(ushort value)
        {
            // See algorithm notes in TrailingZeros(byte)

            int lsb = value & -value;
            lsb %= 19; // mod 19

            byte cnt = s_trail16u[lsb];

            // NoOp: Hashing scheme has unused outputs (inputs 65536 and higher do not fit a ushort)
            Debug.Assert(lsb != 5 && lsb != 10, $"{value} resulted in unexpected {typeof(ushort)} hash {lsb}, with count {cnt}");

            return cnt;
        }

        // See algorithm notes in TrailingCount(byte)
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
            // See algorithm notes in TrailingZeros(byte)

            long lsb = value & -value;
            lsb %= 37; // mod 37

            byte cnt = s_trail32u[lsb];

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

            uint mv = (uint)(value >> 32); // High-32
            uint nv = (uint)value; // Low-32

            long mi = mv & -mv;
            long ni = nv & -nv;

            mi %= 37; // mod 37
            ni %= 37;

            byte mc = s_trail32u[mi];
            byte nc = s_trail32u[ni]; // Use warm cache

            // Truth table
            // m   n  n32 actual  n + (m * n32)
            // 32 32  1   32+32  32 + (32 * 1)
            // 32  n  0   n       n + (32 * 0)
            // m  32  1   32+m   32 + (m * 1)
            // m   n  0   n       n + (m * 0)

            mc *= BoolToByte.Unsafe(nc == 32); // Only add m if n != 32
            return mc + nc;
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

            uint val = value;

            //                   1000 0000
            val |= val >> 01; // 1100 0000
            val |= val >> 02; // 1111 0000
            val |= val >> 04; // 1111 1111

            uint ix = (val * c_deBruijn32) >> 27;

            byte log = s_deBruijn32[ix];
            return log;
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

            uint val = value;

            //                   1000 0000 0000 0000
            val |= val >> 01; // 1100 0000 0000 0000
            val |= val >> 02; // 1111 0000 0000 0000
            val |= val >> 04; // 1111 1111 0000 0000
            val |= val >> 08; // 1111 1111 1111 1111

            uint ix = (val * c_deBruijn32) >> 27;

            byte log = s_deBruijn32[ix];
            return log;
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

            uint ix = (val * c_deBruijn32) >> 27;

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
        // TODO: Perf
        //{
        //    // Perf: Do not use guard clauses; callers must be trusted
        //    Debug.Assert(value > 0);

        //    ulong val = value;

        //    //                   1000 0000 0000 0000 0000 0000 0000 0000 0...
        //    val |= val >> 01; // 1100 0000 0000 0000 0000 0000 0000 0000 0...
        //    val |= val >> 02; // 1111 0000 0000 0000 0000 0000 0000 0000 0...
        //    val |= val >> 04; // 1111 1111 0000 0000 0000 0000 0000 0000 0...
        //    val |= val >> 08; // 1111 1111 1111 1111 0000 0000 0000 0000 0...
        //    val |= val >> 16; // 1111 1111 1111 1111 1111 1111 1111 1111 0...
        //    val |= val >> 32; // 1111 1111 1111 1111 1111 1111 1111 1111 1...

        //    // Instead of using a 64-bit lookup table,
        //    // we use the existing 32-bit table twice.

        //    uint mv = (uint)(val >> 32); // High-32
        //    uint nv = (uint)val; // Low-32

        //    uint mi = (mv * c_deBruijn32) >> 27;
        //    uint ni = (nv * c_deBruijn32) >> 27;

        //    int ml = s_deBruijn32[mi];
        //    int nl = s_deBruijn32[ni]; // Use warm cache

        //    // Log(0) is undefined: Return 32 + 32.
        //    //ml += BoolToByte((value >> 32) == 0);
        //    //nl += BoolToByte((uint)value == 0);

        //    // Truth table
        //    // m   n  m32 actual   m + (n * m32)
        //    // 32 32  1   32+32   32 + (32 * 1)
        //    // 32  n  1   32+n    32 + (n * 1)
        //    // m  32  0   m        m + (32 * 0)
        //    // m   n  0   m        m + (n * 0)

        //    //nl *= BoolToByte(ml == 31); // Only add n if m != 32
        //    return ml + nl;
        //}        

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

            uint val = value;

            // If zero, add 1
            val += BoolToByte.Unsafe(value == 0);

            //         77        0100 1101
            val--; //  76        0100 1100 (for exact powers of 2)
            val |= val >> 01; // 0110 1110
            val |= val >> 02; // 0111 1111
            val |= val >> 04; // 0111 1111
            val++; // 128        1000 0000 (for exact powers of 2)

            return val;
        }

        /// <summary>
        /// Computes the next power of 2 greater than the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint Pow2Low(ushort value)
        {
            Debug.Assert(value < ushort.MaxValue);

            uint val = value;

            // If zero, add 1
            val += BoolToByte.Unsafe(value == 0);

            //         77        0100 1101
            val--; //  76        0100 1100 (for exact powers of 2)
            val |= val >> 01; // 0110 1110
            val |= val >> 02; // 0111 1111
            val |= val >> 04; // 0111 1111
            val |= val >> 08; // 0111 1111
            val++; // 128        1000 0000 (for exact powers of 2)

            return val;
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
            val += BoolToByte.Unsafe(value == 0);

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
            val += BoolToByte.Unsafe(value == 0);

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
            => IsPowerOf2((uint)value);

        /// <summary>
        /// Returns True if the value is a power of 2, else False.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOf2(ushort value)
            => IsPowerOf2((uint)value);

        /// <summary>
        /// Returns True if the value is a power of 2, else False.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOf2(uint value)
            => (value != 0)
            & // && causes branch
            (value & (value - 1)) == 0;

        /// <summary>
        /// Returns True if the value is a power of 2, else False.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOf2(ulong value)
            => (value != 0)
            & // && causes branch
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
        {
            uint val = value;

            //                  1010 0111 (odd)
            val ^= val >> 4; // 1000 1101
            val &= 15; //       0000 1101 (13)

            val = 0b_0110_1001_1001_0110u >> (int)val; // 0011 (3)
            val &= 1; // 0001 (1)

            return (int)val; // 1==odd
        }

        /// <summary>
        /// Returns 1 if the bit count is odd, else 0.
        /// Logically equivalent to PopCount mod 2.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Parity(ushort value)
        {
            uint val = value;

            val ^= val >> 8;
            val ^= val >> 4;
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

        #region Helpers

        [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 1)]
        private struct BoolToByte
        {
            [FieldOffset(0)]
            public bool Bool;

            [FieldOffset(0)]
            public readonly byte Byte;

            /// <summary>
            /// Converts a bool to a byte value without branching
            /// Uses safe code.
            /// </summary>
            /// <param name="on">The value to convert.</param>
            /// <returns>Returns 1 if True, else returns 0.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte Safe(bool on)
                => (new BoolToByte { Bool = on }).Byte;

            /// <summary>
            /// Converts a bool to a byte value without branching
            /// Uses unsafe code.
            /// </summary>
            /// <param name="on">The value to convert.</param>
            /// <returns>Returns 1 if True, else returns 0.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte Unsafe(bool on)
            {
                unsafe
                {
                    return *(byte*)&on;
                }
            }
        }

        #endregion
    }

#pragma warning restore IDE0007 // Use implicit type
}

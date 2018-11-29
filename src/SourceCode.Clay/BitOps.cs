#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Diagnostics;
using System.Runtime.CompilerServices;

// Some of this inspired by the Stanford Bit Widdling Hacks by Sean Eron Anderson:
// http://graphics.stanford.edu/~seander/bithacks.html

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents operations that work on single bits in a mask.
    /// </summary>
    public static partial class BitOps
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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(sbyte value, int bitOffset)
            => ExtractBit(unchecked((byte)value), bitOffset);

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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(short value, int bitOffset)
            => ExtractBit(unchecked((ushort)value), bitOffset);

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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(int value, int bitOffset)
            => ExtractBit(unchecked((uint)value), bitOffset);

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

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(long value, int bitOffset)
            => ExtractBit(unchecked((ulong)value), bitOffset);

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
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteBit(byte value, int bitOffset, bool on)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            uint onn = Any(ref on);
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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte WriteBit(sbyte value, int bitOffset, bool on)
            => unchecked((sbyte)WriteBit((byte)value, bitOffset, on));

        /// <summary>
        /// Writes the specified bit in a mask and returns the new value.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort WriteBit(ushort value, int bitOffset, bool on)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            uint onn = Any(ref on);
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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short WriteBit(short value, int bitOffset, bool on)
            => unchecked((short)WriteBit((ushort)value, bitOffset, on));

        /// <summary>
        /// Writes the specified bit in a mask and returns the new value.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint WriteBit(uint value, int bitOffset, bool on)
        {
            uint mask = 1U << bitOffset;

            uint onn = Any(ref on);
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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBit(int value, int bitOffset, bool on)
            => unchecked((int)WriteBit((uint)value, bitOffset, on));

        /// <summary>
        /// Writes the specified bit in a mask and returns the new value.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// Executes without branching.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong WriteBit(ulong value, int bitOffset, bool on)
        {
            ulong mask = 1UL << bitOffset;

            ulong onn = Any(ref on);
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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long WriteBit(long value, int bitOffset, bool on)
            => unchecked((long)WriteBit((ulong)value, bitOffset, on));

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
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref byte value, int bitOffset, bool on)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            uint onn = Any(ref on);
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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref sbyte value, int bitOffset, bool on)
        {
            int shft = bitOffset & 7;
            int mask = 1 << shft;

            int onn = Any(ref on);
            onn <<= shft;

            int btw = value & mask;
            value = (sbyte)((value & ~mask) | onn);

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
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref ushort value, int bitOffset, bool on)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            uint onn = Any(ref on);
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
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref short value, int bitOffset, bool on)
        {
            int shft = bitOffset & 15;
            int mask = 1 << shft;

            int onn = Any(ref on);
            onn <<= shft;

            int btw = value & mask;
            value = (short)((value & ~mask) | onn);

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
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref uint value, int bitOffset, bool on)
        {
            uint mask = 1U << bitOffset;

            uint onn = Any(ref on);
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
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref int value, int bitOffset, bool on)
        {
            int mask = 1 << bitOffset;

            int onn = Any(ref on);
            onn <<= bitOffset;

            int btw = value & mask;
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
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref ulong value, int bitOffset, bool on)
        {
            ulong mask = 1UL << bitOffset;

            ulong onn = Any(ref on);
            onn <<= bitOffset;

            ulong btw = value & mask;
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
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref long value, int bitOffset, bool on)
        {
            long mask = 1L << bitOffset;

            long onn = Any(ref on);
            onn <<= bitOffset;

            long btw = value & mask;
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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ClearBit(sbyte value, int bitOffset)
            => unchecked((sbyte)ClearBit((byte)value, bitOffset));

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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ClearBit(short value, int bitOffset)
            => unchecked((short)ClearBit((ushort)value, bitOffset));

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
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClearBit(int value, int bitOffset)
            => unchecked((int)ClearBit((uint)value, bitOffset));

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

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ClearBit(long value, int bitOffset)
            => unchecked((long)ClearBit((ulong)value, bitOffset));

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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref sbyte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            int mask = 1 << shft;

            int btr = value & mask;
            value = (sbyte)(value & ~mask);

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
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref short value, int bitOffset)
        {
            int shft = bitOffset & 15;
            int mask = 1 << shft;

            int btr = value & mask;
            value = (short)(value & ~mask);

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
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref int value, int bitOffset)
        {
            int mask = 1 << bitOffset;

            int btr = value & mask;
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

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref long value, int bitOffset)
        {
            long mask = 1L << bitOffset;

            long btr = value & mask;
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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte InsertBit(sbyte value, int bitOffset)
            => unchecked((sbyte)InsertBit((byte)value, bitOffset));

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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short InsertBit(short value, int bitOffset)
            => unchecked((short)InsertBit((ushort)value, bitOffset));

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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int InsertBit(int value, int bitOffset)
            => unchecked((int)InsertBit((uint)value, bitOffset));

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

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long InsertBit(long value, int bitOffset)
            => unchecked((long)InsertBit((ulong)value, bitOffset));

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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref sbyte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            uint mask = 1U << shft;

            uint val = (uint)value;
            uint bts = val & mask;
            value = (sbyte)(val | mask);

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
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref short value, int bitOffset)
        {
            int shft = bitOffset & 15;
            uint mask = 1U << shft;

            uint val = (uint)value;
            uint bts = val & mask;
            value = (short)(val | mask);

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
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref int value, int bitOffset)
        {
            int mask = 1 << bitOffset;

            int bts = value & mask;
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

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref long value, int bitOffset)
        {
            long mask = 1L << bitOffset;

            long bts = value & mask;
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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ComplementBit(sbyte value, int bitOffset)
            => unchecked((sbyte)ComplementBit((byte)value, bitOffset));

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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ComplementBit(short value, int bitOffset)
            => unchecked((short)ComplementBit((ushort)value, bitOffset));

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
        /// Complements the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ComplementBit(int value, int bitOffset)
            => unchecked((int)ComplementBit((uint)value, bitOffset));

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

        /// <summary>
        /// Complements the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ComplementBit(long value, int bitOffset)
            => unchecked((long)ComplementBit((ulong)value, bitOffset));

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
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref sbyte value, int bitOffset)
        {
            int shft = bitOffset & 7;
            int mask = 1 << shft;

            int btc = value & mask;
            value = (sbyte)~(~mask ^ value);

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
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref short value, int bitOffset)
        {
            int shft = bitOffset & 15;
            int mask = 1 << shft;

            int btc = value & mask;
            value = (short)~(~mask ^ value);

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
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref int value, int bitOffset)
        {
            int mask = 1 << bitOffset;

            int btc = value & mask;
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

        /// <summary>
        /// Complements the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTC.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to complement.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ComplementBit(ref long value, int bitOffset)
        {
            long mask = 1L << bitOffset;

            long btc = value & mask;
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
            uint val = value;

            // Will NOT compile to instrinsics
            val = (val << shft) | (val >> (8 - shft));
            return (byte)val;
        }

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte RotateLeft(sbyte value, int offset)
            => unchecked((sbyte)RotateLeft((byte)value, offset));

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
            uint val = value;

            // Will NOT compile to instrinsics
            val = (val >> shft) | (val << (8 - shft));
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
        public static sbyte RotateRight(sbyte value, int offset)
            => unchecked((sbyte)RotateRight((byte)value, offset));

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
            uint val = value;

            // Will NOT compile to instrinsics
            val = (val << shft) | (val >> (16 - shft));
            return (ushort)val;
        }

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short RotateLeft(short value, int offset)
            => unchecked((short)RotateLeft((ushort)value, offset));

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
            uint val = value;

            // Will NOT compile to instrinsics
            val = (val >> shft) | (val << (16 - shft));
            return (ushort)val;
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
        public static short RotateRight(short value, int offset)
            => unchecked((short)RotateRight((ushort)value, offset));

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
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RotateLeft(int value, int offset)
            => unchecked((int)RotateLeft((uint)value, offset));

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
        /// Rotates the specified value right by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROR.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RotateRight(int value, int offset)
            => unchecked((int)RotateRight((uint)value, offset));

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
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RotateLeft(long value, int offset)
            => unchecked((long)RotateLeft((ulong)value, offset));

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

        /// <summary>
        /// Rotates the specified value right by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROR.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RotateRight(long value, int offset)
            => unchecked((long)RotateRight((ulong)value, offset));

        #endregion

        #region PopCount

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(byte value)
            => PopCount((uint)value);

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(sbyte value)
            => PopCount(unchecked((uint)value & 0xFFu)); // Note masking

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(ushort value)
            => PopCount((uint)value);

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(short value)
            => PopCount(unchecked((uint)value & 0xFFFFu)); // Note masking

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
        public static int PopCount(int value)
            => PopCount(unchecked((uint)value));

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

        /// <summary>
        /// Returns the population count (number of bits set) of a mask.
        /// Similar in behavior to the x86 instruction POPCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(long value)
            => PopCount(unchecked((ulong)value));

        #endregion

        #region LeadingZeros

        private static readonly byte[] s_deBruijn32 = new byte[32]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        private const uint DeBruijn32 = 0x07C4_ACDDu;

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
        public static int LeadingZeros(sbyte value)
            => LeadingZeros(unchecked((byte)value));

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
        public static int LeadingZeros(short value)
            => LeadingZeros(unchecked((ushort)value));

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(uint value)
        {
            uint val = value;
            FillTrailing(ref val);

            uint ix = (val * DeBruijn32) >> 27;
            int zeros = 31 - s_deBruijn32[ix];

            // Log(0) is undefined: Return 32.
            zeros += NotAny(value);

            return zeros;
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(int value)
            => LeadingZeros(unchecked((uint)value));

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(ulong value)
        {
            ulong val = value;
            FillTrailing(ref val);

            // Instead of using a 64-bit lookup table,
            // we use the existing 32-bit table twice.

            uint hv = (uint)(val >> 32); // High-32
            uint bv = (uint)val; // Low-32

            uint hi = (hv * DeBruijn32) >> 27;
            uint bi = (bv * DeBruijn32) >> 27;

            uint h = (uint)(31 - s_deBruijn32[hi]);
            uint b = (uint)(31 - s_deBruijn32[bi]); // Use warm cache

            // Log(0) is undefined: Return 32 + 32.
            h += NotAny((uint)(value >> 32));
            b += NotAny((uint)value);

            // Truth table
            // h   b  h32 actual   h + (b * m32 ? 1 : 0)
            // 32 32  1   32+32   32 + (32 * 1)
            // 32  b  1   32+b    32 + (b * 1)
            // h  32  0   h        h + (32 * 0)
            // h   b  0   h        h + (b * 0)

            b = Iff(h == 32, b); // Only add b if h==32
            return (int)(h + b);
        }

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeros(long value)
            => LeadingZeros(unchecked((ulong)value));

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
        public static int LeadingOnes(sbyte value)
            => LeadingOnes(unchecked((byte)value));

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
        public static int LeadingOnes(short value)
            => LeadingOnes(unchecked((ushort)value));

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
        public static int LeadingOnes(int value)
            => LeadingOnes(unchecked((uint)value));

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(ulong value)
            => LeadingZeros(~value);

        /// <summary>
        /// Count the number of leading one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingOnes(long value)
            => LeadingOnes(unchecked((ulong)value));

        #endregion

        #region TrailingZeros

        static BitOps()
        {
            // We want to map [0, 2^0, 2^1, 2^2, ..., 2^32] to the smallest contiguous range, ideally [0..32] since 33 is the range cardinality.
            // Mod-37 is a simple perfect-hashing scheme over this range, where 37 is chosen as the smallest prime greater than 33.
            const int p = 37;

            s_trail32u[0] = 32; // Loop excludes [0]

            long n = 1;
            for (byte i = 1; i < p; i++)
            {
                int m = (int)(n % p); // Hash
                byte z = (byte)(i - 1); // Trailing zeros

                s_trail32u[m] = z;

                n <<= 1; // mul 2
            }
        }

        private static readonly byte[] s_trail32u = new byte[37];

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(byte value)
            => System.Math.Min(8, TrailingZeros((uint)value)); // Delegate to intrinsic

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(sbyte value)
            => TrailingZeros(unchecked((byte)value));

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(ushort value)
            => System.Math.Min(16, TrailingZeros((uint)value)); // Delegate to intrinsic

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(short value)
            => TrailingZeros(unchecked((ushort)value));

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

            lsb %= 37; // mod 37

            // Benchmark: Lookup is 2x faster than Switch
            // Method |     Mean |     Error | StdDev    | Scaled |
            //------- | ---------| ----------| ----------| -------|
            // Lookup | 2.920 ns | 0.0893 ns | 0.2632 ns |   1.00 |
            // Switch | 6.548 ns | 0.1301 ns | 0.2855 ns |   2.26 |

            byte cnt = s_trail32u[lsb]; // eg 44 -> 2 (44==0010 1100 has 2 trailing zeros)

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
        public static int TrailingZeros(int value)
            => TrailingZeros(unchecked((uint)value));

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

            uint hv = (uint)(value >> 32); // High-32
            uint bv = (uint)value; // Low-32

            long hi = hv & -hv;
            long bi = bv & -bv;

            hi %= 37; // mod 37
            bi %= 37;

            uint h = s_trail32u[hi];
            uint b = s_trail32u[bi]; // Use warm cache

            // Truth table
            // h   b  b32 actual  b + (h * b32 ? 1 : 0)
            // 32 32  1   32+32  32 + (32 * 1)
            // 32  b  0   b       b + (32 * 0)
            // h  32  1   32+h   32 + (h * 1)
            // h   b  0   b       b + (h * 0)

            h = Iff(b == 32, h); // Only add h if b==32
            return (int)(b + h);
        }

        /// <summary>
        /// Count the number of trailing zero bits in a mask.
        /// Similar in behavior to the x86 instruction TZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingZeros(long value)
            => TrailingZeros(unchecked((ulong)value));

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
        public static int TrailingOnes(sbyte value)
            => TrailingOnes(unchecked((byte)value));

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
        public static int TrailingOnes(short value)
            => TrailingOnes(unchecked((ushort)value));

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
        public static int TrailingOnes(int value)
            => TrailingOnes(unchecked((uint)value));

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(ulong value)
            => TrailingZeros(~value);

        /// <summary>
        /// Count the number of trailing one bits in a mask.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TrailingOnes(long value)
            => TrailingOnes(unchecked((ulong)value));

        #endregion

        #region AsByte

        /// <summary>
        /// Converts a boolean to a byte value without branching.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(ref bool condition)
            => Unsafe.As<bool, byte>(ref condition);

        /// <summary>
        /// Converts a boolean to a byte value without branching.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(bool condition)
            => Unsafe.As<bool, byte>(ref condition);

        #endregion

        #region Normalize

        /// <summary>
        /// Normalizes a boolean value without branching.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(ref bool condition)
        {
            byte val = Any(ref condition);
            condition = Unsafe.As<byte, bool>(ref val);
        }

        /// <summary>
        /// Normalizes a boolean value without branching.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Normalize(bool condition)
        {
            byte val = Any(ref condition);
            return Unsafe.As<byte, bool>(ref val);
        }

        #endregion

        #region Any

        /// <summary>
        /// Returns 1 if <paramref name="condition"/> is True, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The value to inspect.</param>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Any(ref bool condition)
            => Any((ushort)Unsafe.As<bool, byte>(ref condition));

        /// <summary>
        /// Returns 1 if <paramref name="condition"/> is True, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The value to inspect.</param>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Any(bool condition)
            => Any((ushort)Unsafe.As<bool, byte>(ref condition));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Any(ushort value)
        {
            // Would be great to use intrinsics here instead:
            //   or al, al
            //   cmovnz al, 1
            // cmov isn't a branch and won't stall the pipeline.

            int val = value; // CLR permits 0..255
            val = -val; // Negation will set sign-bit iff non-zero
            val >>= 31; // Send sign-bit to lsb (all other bits will be zero)

            return (byte)val; // 0|1
        }

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Any(short value)
            => Any(unchecked((ushort)value));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Any(uint value)
            // Fold into least significant ushort
            => Any(unchecked((ushort)(value | value >> 16)));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Any(int value)
            // Fold into least significant ushort
            => Any(unchecked((ushort)(value | value >> 16)));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Any(ulong value)
            // Fold into least significant uint
            => Any(unchecked((uint)(value | value >> 32)));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Any(long value)
            // Fold into least significant uint
            => Any(unchecked((uint)(value | value >> 32)));

        #endregion

        #region NotAny

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NotAny(ushort value)
            => (byte)(1 ^ Any(value));

        /*
        {
            // Would be great to use intrinsics here instead:
            //   or al, al
            //   cmovnz al, 1
            // cmov isn't a branch and won't stall the pipeline.

            int val = value;

            val = -val; // Negation will set sign-bit iff non-zero
            val >>= 31; // Send sign-bit to lsb (all other bits will be zero)

            return (byte)(1 ^ val); // 0|1
        }
        */

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NotAny(short value)
            => NotAny(unchecked((ushort)value));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NotAny(uint value)
            // Fold into least significant ushort
            => NotAny(unchecked((ushort)(value | value >> 16)));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NotAny(int value)
            // Fold into least significant ushort
            => NotAny(unchecked((ushort)(value | value >> 16)));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NotAny(ulong value)
            // Fold into least significant uint
            => NotAny(unchecked((uint)(value | value >> 32)));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NotAny(long value)
            // Fold into least significant uint
            => NotAny(unchecked((uint)(value | value >> 32)));

        #endregion

        #region Iff

        /// <summary>
        /// Converts a boolean to a specified integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Iff(bool condition, uint trueValue)
        {
            uint val = Any(ref condition);
            return val * trueValue;
        }

        /// <summary>
        /// Converts a boolean to specified integer values without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Iff(bool condition, uint trueValue, uint falseValue)
        {
            uint val = Any(ref condition);
            return (val * trueValue) + (1 - val) * falseValue;
        }

        /// <summary>
        /// Converts a boolean to a specified integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Iff(bool condition, int trueValue)
            => unchecked((int)Iff(condition, (uint)trueValue));

        /// <summary>
        /// Converts a boolean to specified integer values without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Iff(bool condition, int trueValue, int falseValue)
            => unchecked((int)Iff(condition, (uint)trueValue, (uint)falseValue));

        /// <summary>
        /// Converts a boolean to a specified integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Iff(bool condition, ulong trueValue)
        {
            ulong val = Any(ref condition);
            return val * trueValue;
        }

        /// <summary>
        /// Converts a boolean to specified integer values without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Iff(bool condition, ulong trueValue, ulong falseValue)
        {
            ulong val = Any(ref condition);
            return (val * trueValue) + (1 - val) * falseValue;
        }

        /// <summary>
        /// Converts a boolean to a specified integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Iff(bool condition, long trueValue)
            => unchecked((long)Iff(condition, (ulong)trueValue));

        /// <summary>
        /// Converts a boolean to specified integer values without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Iff(bool condition, long trueValue, long falseValue)
            => unchecked((long)Iff(condition, (ulong)trueValue, (ulong)falseValue));

        #endregion

        #region Helpers

        /// <summary>
        /// Fills the trailing zeros in a mask with ones.
        /// </summary>
        /// <param name="value">The value to mutate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FillTrailing(ref uint value)
        {
            // byte#                         4          3   2  1
            //                       1000 0000  0000 0000  00 00
            value |= value >> 01; // 1100 0000  0000 0000  00 00
            value |= value >> 02; // 1111 0000  0000 0000  00 00
            value |= value >> 04; // 1111 1111  0000 0000  00 00
            value |= value >> 08; // 1111 1111  1111 1111  00 00
            value |= value >> 16; // 1111 1111  1111 1111  FF FF
        }

        /// <summary>
        /// Fills the trailing zeros in a mask with ones.
        /// </summary>
        /// <param name="value">The value to mutate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FillTrailing(ref ulong value)
        {
            // byte#                         8          7   6  5   4  3   2  1
            //                       1000 0000  0000 0000  00 00  00 00  00 00
            value |= value >> 01; // 1100 0000  0000 0000  00 00  00 00  00 00
            value |= value >> 02; // 1111 0000  0000 0000  00 00  00 00  00 00
            value |= value >> 04; // 1111 1111  0000 0000  00 00  00 00  00 00
            value |= value >> 08; // 1111 1111  1111 1111  00 00  00 00  00 00
            value |= value >> 16; // 1111 1111  1111 1111  FF FF  00 00  00 00

            value |= value >> 32; // 1111 1111  1111 1111  FF FF  FF FF  FF FF
        }

        #endregion
    }
}

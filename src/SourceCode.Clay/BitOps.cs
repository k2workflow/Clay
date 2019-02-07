#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;

// Some of this inspired by the Stanford Bit Twiddling Hacks by Sean Eron Anderson:
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

        /*
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
        */

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(sbyte value, int bitOffset)
            => ExtractBit((byte)value, bitOffset);

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
            => ExtractBit((ushort)value, bitOffset);

        /*
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
            => ExtractBit((uint)value, bitOffset);
        */

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
            => ExtractBit((ulong)value, bitOffset);

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

            uint onn = Iff(ref on);
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

            uint onn = Iff(ref on);
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

            uint onn = Iff(ref on);
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

            ulong onn = Iff(ref on);
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

            uint onn = Iff(ref on);
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

            int onn = Iff(ref on);
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

            uint onn = Iff(ref on);
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

            int onn = Iff(ref on);
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

            uint onn = Iff(ref on);
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

            int onn = Iff(ref on);
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

            ulong onn = Iff(ref on);
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

            long onn = Iff(ref on);
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

        /*
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
        */

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

        /*
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
        */

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

        /*
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
        */

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

        /*
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
        */

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

        /*
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
        */

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

        /*
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
        */

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

        /*
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
        */

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

        #endregion

        #region LeadingZeroCount

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeroCount(byte value)
            => LeadingZeroCount((uint)value) - 24;

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeroCount(sbyte value)
            => LeadingZeroCount(unchecked((byte)value));

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeroCount(ushort value)
            => LeadingZeroCount((uint)value) - 16;

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeroCount(short value)
            => LeadingZeroCount(unchecked((ushort)value));

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeroCount(int value)
            => LeadingZeroCount((uint)value);

        /// <summary>
        /// Count the number of leading zero bits in a mask.
        /// Similar in behavior to the x86 instruction LZCNT.
        /// </summary>
        /// <param name="value">The mask.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeroCount(long value)
            => LeadingZeroCount((ulong)value);

        #endregion

        #region TrailingZeroCount

        #endregion

        #region AsByte

        /// <summary>
        /// Casts the underlying <see cref="byte"/> value from a <see cref="bool"/> without normalization.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The value to cast.</param>
        /// <returns>Returns 0 if <paramref name="condition"/> is False, else returns a non-zero number per the remarks.</returns>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(ref bool condition)
            => Unsafe.As<bool, byte>(ref condition);

        /// <summary>
        /// Casts the underlying <see cref="byte"/> value from a <see cref="bool"/> without normalization.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The value to cast.</param>
        /// <returns>Returns 0 if <paramref name="condition"/> is False, else returns a non-zero number per the remarks.</returns>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(bool condition)
            => Unsafe.As<bool, byte>(ref condition);

        #endregion

        #region NonZero

        // Normalize bool's underlying value to 0|1
        // https://github.com/dotnet/roslyn/issues/24652

        // byte b;                 // Non-negative
        // int val = b;            // Widen byte to int so that negation is reliable
        // val = -val;             // Negation will set sign-bit iff non-zero
        // val = (uint)val >> 31;  // Send sign-bit to lsb (all other bits will be thus zero'd)

        // Would be great to use intrinsics here instead:
        //     OR al, al
        //     CMOVNZ al, 1
        // CMOV isn't a branch and won't stall the pipeline.

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVNZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NonZero(ushort value)
            // Negation will set sign-bit iff non-zero
            => unchecked((byte)(((uint)-value) >> 31));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVNZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NonZero(short value)
            => NonZero((ushort)value);

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVNZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NonZero(uint value)
            // Negation will set sign-bit iff non-zero
            => unchecked((byte)(((ulong)-value) >> 63));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVNZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NonZero(int value)
            => NonZero((uint)value);

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVNZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NonZero(ulong value)
            // Fold into uint
            => NonZero((uint)(value | value >> 32));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is non-zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVNZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte NonZero(long value)
            => NonZero((ulong)value);

        #endregion

        #region IsZero

        // XOR is theoretically slightly cheaper than subtraction,
        // due to no carry logic. But both 1 clock cycle regardless.

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte IsZero(ushort value)
            => (byte)(1u ^ NonZero(value));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte IsZero(short value)
            => (byte)(1u ^ NonZero(value));

        /*
        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte IsZero(uint value)
            => (byte)(1u ^ NonZero(value));
        */

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte IsZero(int value)
            => (byte)(1u ^ NonZero(value));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte IsZero(ulong value)
            => (byte)(1u ^ NonZero(value));

        /// <summary>
        /// Returns 1 if <paramref name="value"/> is zero, else returns 0.
        /// Does not incur branching.
        /// Similar in behavior to the x86 instruction CMOVZ.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte IsZero(long value)
            => (byte)(1u ^ NonZero(value));

        #endregion

        #region Iff

        /// <summary>
        /// Normalizes the underlying <see cref="byte"/> value from a <see cref="bool"/> without branching.
        /// Returns 1 if <paramref name="condition"/> is True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to normalize.</param>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Iff(ref bool condition)
            => NonZero((ushort)AsByte(ref condition));

        /// <summary>
        /// Normalizes the underlying <see cref="byte"/> value from a <see cref="bool"/> without branching.
        /// Returns 1 if <paramref name="condition"/> is True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to normalize.</param>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Iff(bool condition)
            => Iff(ref condition);

        /// <summary>
        /// Converts a boolean to a specified integer value without branching.
        /// Returns <paramref name="trueValue"/> if <paramref name="condition"/> is True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Iff(bool condition, uint trueValue)
            => Iff(ref condition) * trueValue;

        /// <summary>
        /// Converts a boolean to specified integer values without branching.
        /// Returns <paramref name="trueValue"/> if <paramref name="condition"/> is True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Iff(bool condition, uint trueValue, uint falseValue)
        {
            uint val = Iff(ref condition);
            return (val * trueValue) + (1u ^ val) * falseValue;
        }

        /// <summary>
        /// Converts a boolean to a specified integer value without branching.
        /// Returns <paramref name="trueValue"/> if <paramref name="condition"/> is True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Iff(bool condition, int trueValue)
            => unchecked((int)Iff(condition, (uint)trueValue));

        /// <summary>
        /// Converts a boolean to specified integer values without branching.
        /// Returns <paramref name="trueValue"/> if <paramref name="condition"/> is True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Iff(bool condition, int trueValue, int falseValue)
            => unchecked((int)Iff(condition, (uint)trueValue, (uint)falseValue));

        /// <summary>
        /// Converts a boolean to a specified integer value without branching.
        /// Returns <paramref name="trueValue"/> if <paramref name="condition"/> is True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Iff(bool condition, ulong trueValue)
            => Iff(ref condition) * trueValue;

        /// <summary>
        /// Converts a boolean to specified integer values without branching.
        /// Returns <paramref name="trueValue"/> if <paramref name="condition"/> is True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Iff(bool condition, ulong trueValue, ulong falseValue)
        {
            ulong val = Iff(ref condition);
            return (val * trueValue) + (1ul ^ val) * falseValue;
        }

        /// <summary>
        /// Converts a boolean to a specified integer value without branching.
        /// Returns <paramref name="trueValue"/> if <paramref name="condition"/> is True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Iff(bool condition, long trueValue)
            => unchecked((long)Iff(condition, (ulong)trueValue));

        /// <summary>
        /// Converts a boolean to specified integer values without branching.
        /// Returns <paramref name="trueValue"/> if <paramref name="condition"/> is True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Iff(bool condition, long trueValue, long falseValue)
            => unchecked((long)Iff(condition, (ulong)trueValue, (ulong)falseValue));

        #endregion

        #region Helpers

        /*
        /// <summary>
        /// Fills the trailing zeros in a mask with ones.
        /// </summary>
        /// <param name="value">The value to mutate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FoldTrailingOnes(ref uint value)
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
        private static void FoldTrailingOnes(ref ulong value)
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
        */

        #endregion
    }
}

#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Diagnostics;
using System.Runtime.CompilerServices;

// See related DotNetCore work by same author:
// https://raw.githubusercontent.com/dotnet/coreclr/master/src/System.Private.CoreLib/shared/System/Numerics/BitOperations.cs

// Some routines inspired by the Stanford Bit Twiddling Hacks by Sean Eron Anderson:
// http://graphics.stanford.edu/~seander/bithacks.html

namespace SourceCode.Clay.Numerics
{
    /// <summary>
    /// Utility methods for intrinsic bit-twiddling operations.
    /// The methods use hardware intrinsics when available on the underlying platform,
    /// otherwise they use optimized software fallbacks.
    /// </summary>
    public static class BitOps
    {
        #region ExtractBit

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(byte value, int bitOffset)
        // For bit-length N, it is conventional to treat N as congruent modulo-N under the shift operation.
        // So for uint, 1 << 33 == 1 << 1, and likewise 1 << -46 == 1 << +18.
        // Note -46 % 32 == -14. But -46 & 31 (0011_1111) == +18.
        // So we use & not %.
        {
            uint mask = 1u << (bitOffset & 7);
            return (value & mask) != 0;
        }

        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(ushort value, int bitOffset)
        {
            uint mask = 1u << (bitOffset & 15);
            return (value & mask) != 0;
        }

        /// <summary>
        /// Reads whether the specified bit in a mask is set.
        /// Similar in behavior to the x86 instruction BT.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to read.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExtractBit(uint value, int bitOffset)
        {
            uint mask = 1u << bitOffset;
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
            ulong mask = 1ul << bitOffset;
            return (value & mask) != 0;
        }

        #endregion

        #region WriteBit

        /// <summary>
        /// Conditionally writes the specified bit in a mask and returns the new value.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte WriteBit(byte value, int bitOffset, bool on)
        {
            uint mask = 1u << (bitOffset & 7);

            if (on)
                return (byte)(value | mask);

            return (byte)(value & ~mask);
        }

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
            uint mask = 1u << (bitOffset & 15);

            if (on)
                return (ushort)(value | mask);

            return (ushort)(value & ~mask);
        }

        /// <summary>
        /// Conditionally writes the specified bit in a mask and returns the new value.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint WriteBit(uint value, int bitOffset, bool on)
        {
            uint mask = 1u << bitOffset;

            if (on)
                return value | mask;

            return value & ~mask;
        }

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
            ulong mask = 1ul << bitOffset;

            if (on)
                return value | mask;

            return value & ~mask;
        }

        /// <summary>
        /// Conditionally writes the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref byte value, int bitOffset, bool on)
        {
            uint mask = 1u << (bitOffset & 7);
            bool btw = (value & mask) != 0;

            if (on)
                value = (byte)(value | mask);
            else
                value = (byte)(value & ~mask);

            return btw;
        }

        /// <summary>
        /// Conditionally writes the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref ushort value, int bitOffset, bool on)
        {
            uint mask = 1u << (bitOffset & 15);
            bool btw = (value & mask) != 0;

            if (on)
                value = (ushort)(value | mask);
            else
                value = (ushort)(value & ~mask);

            return btw;
        }

        /// <summary>
        /// Conditionally writes the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref uint value, int bitOffset, bool on)
        {
            uint mask = 1u << bitOffset;
            bool btw = (value & mask) != 0;

            if (on)
                value |= mask;
            else
                value &= ~mask;

            return btw;
        }

        /// <summary>
        /// Conditionally writes the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instructions BTS and BTR.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <param name="on">True to set the bit to 1, or false to set it to 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteBit(ref ulong value, int bitOffset, bool on)
        {
            ulong mask = 1ul << bitOffset;
            bool btw = (value & mask) != 0;

            if (on)
                value |= mask;
            else
                value &= ~mask;

            return btw;
        }

        #endregion

        #region ClearBit

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ClearBit(byte value, int bitOffset)
        {
            uint mask = 1u << (bitOffset & 7);
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
            uint mask = 1u << (bitOffset & 15);
            return (ushort)(value & ~mask);
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ClearBit(uint value, int bitOffset)
        {
            uint mask = 1u << bitOffset;
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
            ulong mask = 1ul << bitOffset;
            return value & ~mask;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref byte value, int bitOffset)
        {
            uint mask = 1u << (bitOffset & 7);
            bool btr = (value & mask) != 0;

            value = (byte)(value & ~mask);

            return btr;
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
            uint mask = 1u << (bitOffset & 15);
            bool btr = (value & mask) != 0;

            value = (ushort)(value & ~mask);

            return btr;
        }

        /// <summary>
        /// Clears the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTR.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to clear.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ClearBit(ref uint value, int bitOffset)
        {
            uint mask = 1u << bitOffset;
            bool btr = (value & mask) != 0;

            value &= ~mask;

            return btr;
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
            ulong mask = 1ul << bitOffset;
            bool btr = (value & mask) != 0;

            value &= ~mask;

            return btr;
        }

        #endregion

        #region InsertBit

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte InsertBit(byte value, int bitOffset)
        {
            uint mask = 1u << (bitOffset & 7);
            return (byte)(value | mask);
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The mask.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..15] is treated as congruent mod 16.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort InsertBit(ushort value, int bitOffset)
        {
            uint mask = 1u << (bitOffset & 15);
            return (ushort)(value | mask);
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns the new value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint InsertBit(uint value, int bitOffset)
        {
            uint mask = 1u << bitOffset;
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
            ulong mask = 1ul << bitOffset;
            return value | mask;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..7] is treated as congruent mod 8.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref byte value, int bitOffset)
        {
            uint mask = 1u << (bitOffset & 7);
            bool bts = (value & mask) != 0;

            value = (byte)(value | mask);

            return bts;
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
            uint mask = 1u << (bitOffset & 15);
            bool bts = (value & mask) != 0;

            value = (ushort)(value | mask);

            return bts;
        }

        /// <summary>
        /// Sets the specified bit in a mask and returns whether it was originally set.
        /// Similar in behavior to the x86 instruction BTS.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOffset">The ordinal position of the bit to write.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InsertBit(ref uint value, int bitOffset)
        {
            uint mask = 1u << bitOffset;
            bool bts = (value & mask) != 0;

            value |= mask;

            return bts;
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
            ulong mask = 1ul << bitOffset;
            bool bts = (value & mask) != 0;

            value |= mask;

            return bts;
        }

        #endregion

        #region ComplementBit

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
            uint mask = 1u << (bitOffset & 7);
            return (byte)~(~mask ^ value);
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
            uint mask = 1u << (bitOffset & 15);
            return (ushort)~(~mask ^ value);
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
            uint mask = 1u << bitOffset;
            return ~(~mask ^ value);
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
            ulong mask = 1ul << bitOffset;
            return ~(~mask ^ value);
        }

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
            uint mask = 1u << (bitOffset & 7);
            bool btc = (value & mask) != 0;

            value = (byte)~(~mask ^ value);

            return btc;
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
            uint mask = 1u << (bitOffset & 15);
            bool btc = (value & mask) != 0;

            value = (ushort)~(~mask ^ value);

            return btc;
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
            uint mask = 1u << bitOffset;
            bool btc = (value & mask) != 0;

            value = ~(~mask ^ value);

            return btc;
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
            ulong mask = 1ul << bitOffset;
            bool btc = (value & mask) != 0;

            value = ~(~mask ^ value);

            return btc;
        }

        #endregion

        /// <summary>
        /// Casts the underlying <see cref="byte"/> value from a <see cref="bool"/> without normalization.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The value to cast.</param>
        /// <returns>Returns 0 if <paramref name="condition"/> is false, else returns a non-zero number per the remarks.</returns>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(bool condition)
            => Unsafe.As<bool, byte>(ref condition);

        /// <summary>
        /// Casts the underlying <see cref="byte"/> value from a <see cref="bool"/> without normalization.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The value to cast.</param>
        /// <returns>Returns 0 if <paramref name="condition"/> is false, else returns a non-zero number per the remarks.</returns>
        /// <remarks>The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
        /// See https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(ref bool condition)
            => Unsafe.As<bool, byte>(ref condition);

        /// <summary>
        /// Checks a <paramref name="condition"/> and returns <paramref name="trueValue"/> if it is true.
        /// Else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="trueValue">The value to return if the <paramref name="condition"/> is true.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Iff(bool condition, uint trueValue)
            => Iff(ref condition, trueValue);

        /// <summary>
        /// Checks a <paramref name="condition"/> and returns <paramref name="trueValue"/> if it is true.
        /// Else returns 0.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="trueValue">The value to return if the <paramref name="condition"/> is true.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Iff(ref bool condition, uint trueValue)
        {
            uint sel = (uint)-Unsafe.As<bool, byte>(ref condition); // T=FFFFFFFF, F=00000000
            Debug.Assert(sel == 0xFFFF_FFFF || sel == 0); // CLR permits other values for bool

            return trueValue & sel;
        }

        /// <summary>
        /// Checks a <paramref name="condition"/> and returns <paramref name="trueValue"/> if it is true.
        /// Else returns <paramref name="falseValue"/>.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="trueValue">The value to return if the <paramref name="condition"/> is true.</param>
        /// <param name="falseValue">The value to return if the <paramref name="condition"/> is false.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Iff(bool condition, uint trueValue, uint falseValue)
            => Iff(ref condition, trueValue, falseValue);

        /// <summary>
        /// Checks a <paramref name="condition"/> and returns <paramref name="trueValue"/> if it is true.
        /// Else returns <paramref name="falseValue"/>.
        /// Does not incur branching.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="trueValue">The value to return if the <paramref name="condition"/> is true.</param>
        /// <param name="falseValue">The value to return if the <paramref name="condition"/> is false.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Iff(ref bool condition, uint trueValue, uint falseValue)
        {
            // Branchless equivalent of: value == 0 ? x : y
            uint sel = (uint)-Unsafe.As<bool, byte>(ref condition); // T=FFFFFFFF, F=00000000
            Debug.Assert(sel == 0xFFFF_FFFF || sel == 0); // CLR permits other values for bool

            uint tv = trueValue & sel;
            uint fv = /*Bmi1.IsSupported ? Bmi1.AndNot(falseValue, sel) :*/ falseValue & ~sel;
            return tv | fv;
        }
    }
}

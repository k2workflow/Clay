#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents <see cref="bool"/> extensions.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Converts a boolean to a byte value without branching.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(this bool condition)
            => unchecked((byte)((uint)-Unsafe.As<bool, byte>(ref condition) >> 31));

        /// <summary>
        /// Converts a boolean to an integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint If(this bool condition, uint trueValue)
        {
            uint mask = AsByte(condition) - 1u; // 0x00000000 | 0xFFFFFFFF
            return ~mask & trueValue;
        }

        /// <summary>
        /// Converts a boolean to an integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint If(this bool condition, uint trueValue, uint falseValue)
        {
            uint mask = AsByte(condition) - 1u; // 0x00000000 | 0xFFFFFFFF
            return (mask & falseValue) | (~mask & trueValue);
        }

        /// <summary>
        /// Converts a boolean to an integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int If(this bool condition, int trueValue)
            => unchecked((int)If(condition, (uint)trueValue));

        /// <summary>
        /// Converts a boolean to an integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int If(this bool condition, int trueValue, int falseValue)
            => unchecked((int)If(condition, (uint)trueValue, (uint)falseValue));

        /// <summary>
        /// Converts a boolean to an integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong If(this bool condition, ulong trueValue)
        {
            ulong mask = AsByte(condition) - 1ul; // 0x00000000 00000000 | 0xFFFFFFFF FFFFFFFF
            return ~mask & trueValue;
        }

        /// <summary>
        /// Converts a boolean to an integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong If(this bool condition, ulong trueValue, ulong falseValue)
        {
            ulong mask = AsByte(condition) - 1ul; // 0x00000000 00000000 | 0xFFFFFFFF FFFFFFFF
            return (mask & falseValue) | (~mask & trueValue);
        }

        /// <summary>
        /// Converts a boolean to an integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long If(this bool condition, long trueValue)
            => unchecked((long)If(condition, (ulong)trueValue));

        /// <summary>
        /// Converts a boolean to an integer value without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long If(this bool condition, long trueValue, long falseValue)
            => unchecked((long)If(condition, (ulong)trueValue, (ulong)falseValue));
    }
}

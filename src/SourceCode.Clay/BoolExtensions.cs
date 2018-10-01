#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents <see cref="bool"/> extensions.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Converts a boolean to an integer value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Evaluate(this bool condition, uint trueValue, uint falseValue)
        {
            uint val = EvaluateImpl(condition);

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return val;
        }

        /// <summary>
        /// Converts a boolean to an integer value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Evaluate(this bool condition, int trueValue, int falseValue)
        {
            int val = EvaluateImpl(condition);

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return val;
        }

        /// <summary>
        /// Converts a boolean to an integer value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Evaluate(this bool condition, uint trueValue)
            => EvaluateImpl(condition) * trueValue;

        /// <summary>
        /// Converts a boolean to an integer value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Evaluate(this bool condition, int trueValue)
            => EvaluateImpl(condition) * trueValue;

        /// <summary>
        /// Converts a boolean to an integer value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Evaluate(this bool condition, ulong trueValue, ulong falseValue)
        {
            ulong val = EvaluateImpl(condition);

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return val;
        }

        /// <summary>
        /// Converts a boolean to an integer value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Evaluate(this bool condition, long trueValue, long falseValue)
        {
            long val = EvaluateImpl(condition);

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return val;
        }

        /// <summary>
        /// Converts a boolean to an integer value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Evaluate(this bool condition, ulong trueValue)
            => EvaluateImpl(condition) * trueValue;

        /// <summary>
        /// Converts a boolean to an integer value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Evaluate(this bool condition, long trueValue)
            => EvaluateImpl(condition) * trueValue;

        /// <summary>
        /// Converts an integer value to a boolean, without branching.
        /// Returns True if 1, else returns False.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Evaluate(this uint value)
            => EvaluateImpl(value);

        /// <summary>
        /// Converts an integer value to a boolean, without branching.
        /// Returns True if 1, else returns False.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Evaluate(this ulong value)
            => EvaluateImpl(value);

        /// <summary>
        /// Converts an integer value to a boolean, without branching.
        /// Returns True if 1, else returns False.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Evaluate(this int value)
            => EvaluateImpl(unchecked((uint)value));

        /// <summary>
        /// Converts an integer value to a boolean, without branching.
        /// Returns True if 1, else returns False.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Evaluate(this long value)
            => EvaluateImpl(unchecked((ulong)value));

        #region Helpers

        // See benchmarks: Unsafe.As is fastest.
        // Union and unsafe (which cannot be inlined) are faster
        // than idiomatic branching expression.
        // Unsafe.As requires Nuget `System.Runtime.CompilerServices.Unsafe`.

        private static bool EvaluateImpl(uint value)
        {
            uint val = value;

            // byte#                     4         3   2  1
            //                   1000 0000 0000 0000  00 00
            val |= val >> 01; // 1100 0000 0000 0000  00 00
            val |= val >> 02; // 1111 0000 0000 0000  00 00
            val |= val >> 04; // 1111 1111 0000 0000  00 00
            val |= val >> 08; // 1111 1111 1111 1111  00 00
            val |= val >> 16; // 1111 1111 1111 1111  FF FF

            val = val & 1;    // 0000 0000 0000 0000  00 01

            // Ensure the value is 1|0 only
            Debug.Assert(val == 0 || val == 1);

            var b2b = new BoolToByte { Byte = (byte)val };
            return b2b.Bool; // 1|0
        }

        /// <summary>
        /// Converts an integer value to a boolean, without branching.
        /// Returns True if 1, else returns False.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EvaluateImpl(ulong value)
        {
            ulong val = value;

            // byte#                     8         7   6  5   4  3   2  1
            //                   1000 0000 0000 0000  00 00  00 00  00 00
            val |= val >> 01; // 1100 0000 0000 0000  00 00  00 00  00 00
            val |= val >> 02; // 1111 0000 0000 0000  00 00  00 00  00 00
            val |= val >> 04; // 1111 1111 0000 0000  00 00  00 00  00 00
            val |= val >> 08; // 1111 1111 1111 1111  00 00  00 00  00 00
            val |= val >> 16; // 1111 1111 1111 1111  FF FF  00 00  00 00
            val |= val >> 32; // 1111 1111 1111 1111  FF FF  FF FF  FF FF

            val = val & 1;    // 0000 0000 0000 0000  00 00  00 00  00 01

            // Ensure the value is 1|0 only
            Debug.Assert(val == 0 || val == 1);

            var b2b = new BoolToByte { Byte = (byte)val };
            return b2b.Bool; // 1|0
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte EvaluateImpl(this bool condition)
        {
            var b2b = new BoolToByte { Bool = condition };

            // Ensure the value is 1|0 only
            byte val = b2b.Byte;
            Debug.Assert(val == 0 || val == 1);

            return val; // 1|0
        }

        [StructLayout(LayoutKind.Explicit, Size = 1)] // Runtime can choose Pack
        private struct BoolToByte
        {
            [FieldOffset(0)]
            public bool Bool;

            [FieldOffset(0)]
            public byte Byte;
        }

        #endregion
    }
}

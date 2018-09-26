#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents <see cref="bool"/> extensions.
    /// </summary>
    public static class BoolExtensions
    {
        // See benchmarks: Unsafe.As is fastest.
        // Union and unsafe (which cannot be inlined) are faster
        // than idiomatic branching expression.
        // Unsafe.As requires Nuget `System.Runtime.CompilerServices.Unsafe`.

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteImpl(this bool condition)
            => new BoolToByte { Bool = condition }.Byte; // 1|0

        /// <summary>
        /// Converts a bool to a byte value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this bool condition, byte trueValue = 1, byte falseValue = 0)
        {
            uint val = ToByteImpl(condition);
            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (byte)val;
        }

        /// <summary>
        /// Converts a bool to a ushort value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToUInt16(this bool condition, ushort trueValue = 1, ushort falseValue = 0)
        {
            uint val = ToByteImpl(condition);
            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (ushort)val;
        }

        /// <summary>
        /// Converts a bool to a uint value, without branching.
        /// Returns <paramref name="trueValue"/> if True, else returns <paramref name="falseValue"/>.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        /// <param name="trueValue">The value to return if True.</param>
        /// <param name="falseValue">The value to return if False.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(this bool condition, uint trueValue = 1, uint falseValue = 0)
        {
            uint val = ToByteImpl(condition);
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
        public static ulong ToUInt64(this bool condition, ulong trueValue = 1, ulong falseValue = 0)
        {
            ulong val = ToByteImpl(condition);
            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return val;
        }

        [StructLayout(LayoutKind.Explicit, Size = 1)] // Runtime can choose Pack
        private struct BoolToByte
        {
            [FieldOffset(0)]
            public bool Bool;

            [FieldOffset(0)]
            public readonly byte Byte;
        }
    }
}

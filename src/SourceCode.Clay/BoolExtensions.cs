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
        /// <summary>
        /// Converts a bool to a byte value, without branching.
        /// Returns 1 if True, else returns 0.
        /// </summary>
        /// <param name="condition">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this bool condition)
            => new BoolToByte { Bool = condition }.Byte;

        // Union is nearly as fast as unsafe, but unsafe can't be inlined.
        // Unsafe.As requires Nuget `System.Runtime.CompilerServices.Unsafe`.

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

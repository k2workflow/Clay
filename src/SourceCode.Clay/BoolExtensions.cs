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
        /// Converts a bool to a byte value without branching
        /// Uses safe code.
        /// </summary>
        /// <param name="on">The value to convert.</param>
        /// <returns>Returns 1 if True, else returns 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this bool on)
            => (new BoolToByte { Bool = on }).Byte;

        /// <summary>
        /// Converts a bool to a byte value without branching
        /// Uses unsafe code.
        /// </summary>
        /// <param name="on">The value to convert.</param>
        /// <returns>Returns 1 if True, else returns 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByteUnsafe(this bool on)
        {
            unsafe
            {
                return *(byte*)&on;
            }
        }

        [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 1)]
        private struct BoolToByte
        {
            [FieldOffset(0)]
            public bool Bool;

            [FieldOffset(0)]
            public readonly byte Byte;
        }
    }
}

#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents additional blit methods.
    /// </summary>
    public static class Blit
    {
        /// <summary>
        /// Rotates the specified <see cref="byte"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateLeft(byte value, byte bits)
        {
            var b = bits % 8;

            return unchecked((byte)((value << b) | (value >> (8 - b))));
        }

        /// <summary>
        /// Rotates the specified <see cref="byte"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RotateRight(byte value, byte bits)
        {
            var b = bits % 8;

            return unchecked((byte)((value >> b) | (value << (8 - b))));
        }

        /// <summary>
        /// Rotates the specified <see cref="ushort"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateLeft(ushort value, byte bits)
        {
            var b = bits % 16;

            return unchecked((ushort)((value << b) | (value >> (16 - b))));
        }

        /// <summary>
        /// Rotates the specified <see cref="ushort"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort RotateRight(ushort value, byte bits)
        {
            var b = bits % 16;

            return unchecked((ushort)((value >> b) | (value << (16 - b))));
        }

        /// <summary>
        /// Rotates the specified <see cref="uint"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, byte bits)
        {
            var b = bits % 32;

            return unchecked((value << b) | (value >> (32 - b)));
        }

        /// <summary>
        /// Rotates the specified <see cref="uint"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(uint value, byte bits)
        {
            var b = bits % 32;

            return unchecked((value >> b) | (value << (32 - b)));
        }

        /// <summary>
        /// Rotates the specified <see cref="ulong"/> value left by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(ulong value, byte bits)
        {
            var b = bits % 64;

            return unchecked((value << b) | (value >> (64 - b)));
        }

        /// <summary>
        /// Rotates the specified <see cref="ulong"/> value right by the specified number of bits.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="bits">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateRight(ulong value, byte bits)
        {
            var b = bits % 64;

            return unchecked((value >> b) | (value << (64 - b)));
        }
    }
}

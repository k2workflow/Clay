#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents the blittable components of a 32-bit integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 4)]
    [DebuggerDisplay("{Int32,nq}")]
    public struct Blit32
    {
#pragma warning disable S1104 // Fields should not have public accessibility

        /// <summary>
        /// Gets or sets the first <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(0)]
        public byte Byte0;

        /// <summary>
        /// Gets or sets the second <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(1)]
        public byte Byte1;

        /// <summary>
        /// Gets or sets the third <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(2)]
        public byte Byte2;

        /// <summary>
        /// Gets or sets the fourth <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(3)]
        public byte Byte3;

        /// <summary>
        /// Gets or sets the <see cref="int"/> value.
        /// </summary>
        [FieldOffset(0)]
        public int Int32;

        /// <summary>
        /// Gets or sets the <see cref="uint"/> value.
        /// </summary>
        [FieldOffset(0)]
        public uint UInt32;

        /// <summary>
        /// Gets or sets the <see cref="float"/> value.
        /// </summary>
        [FieldOffset(0)]
        public float Single;

        /// <summary>
        /// Gets or sets the low <see cref="Blit16"/> value.
        /// </summary>
        [FieldOffset(0)]
        public Blit16 Blit0;

        /// <summary>
        /// Gets or sets the high <see cref="Blit16"/> value.
        /// </summary>
        [FieldOffset(2)]
        public Blit16 Blit1;

        /// <summary>
        /// The bytes in unsafe form.
        /// </summary>
        [FieldOffset(0)] // 0-3
        public unsafe fixed byte Bytes[4];

#pragma warning restore S1104 // Fields should not have public accessibility

        /// <summary>
        /// Gets the bytes as an array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
            => new[] { Byte0, Byte1, Byte2, Byte3 };
    }
}

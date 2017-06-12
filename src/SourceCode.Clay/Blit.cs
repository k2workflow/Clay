using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents the blittable components of a 8-bit integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 1)]
    [DebuggerDisplay("{Byte,nq}")]
    public struct Blit8
    {
        /// <summary>
        /// Gets or sets the <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(0)]
        public byte Byte;

        /// <summary>
        /// Gets or sets the <see cref="sbyte"/> value.
        /// </summary>
        [FieldOffset(1)]
        public sbyte SByte;
    }

    /// <summary>
    /// Represents the blittable components of a 16-bit integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 2)]
    [DebuggerDisplay("{Int16,nq}")]
    public struct Blit16
    {
        /// <summary>
        /// Gets or sets the <see cref="short"/> value.
        /// </summary>
        [FieldOffset(0)]
        public short Int16;

        /// <summary>
        /// Gets or sets the <see cref="ushort"/> value.
        /// </summary>
        [FieldOffset(0)]
        public ushort UInt16;

        /// <summary>
        /// Gets or sets low <see cref="Blit8"/>.
        /// </summary>
        [FieldOffset(0)]
        public Blit8 Blit0;

        /// <summary>
        /// Gets or sets high <see cref="Blit8"/>.
        /// </summary>
        [FieldOffset(1)]
        public Blit8 Blit1;

        /// <summary>
        /// Gets or sets low <see cref="byte"/>.
        /// </summary>
        [FieldOffset(0)]
        public Byte Byte0;

        /// <summary>
        /// Gets or sets high <see cref="byte"/>.
        /// </summary>
        [FieldOffset(1)]
        public Byte Byte1;
    }

    /// <summary>
    /// Represents the blittable components of a 32-bit integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 4)]
    [DebuggerDisplay("{Int32,nq}")]
    public struct Blit32
    {
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
        /// Gets or sets the first <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(0)]
        public Byte Byte0;

        /// <summary>
        /// Gets or sets the second <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(1)]
        public Byte Byte1;

        /// <summary>
        /// Gets or sets the third <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(2)]
        public Byte Byte2;

        /// <summary>
        /// Gets or sets the fourth <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(3)]
        public Byte Byte3;
    }

    /// <summary>
    /// Represents the blittable components of a 64-bit integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
    [DebuggerDisplay("{Int64,nq}")]
    public struct Blit64
    {
        /// <summary>
        /// Gets or sets the <see cref="long"/> value.
        /// </summary>
        [FieldOffset(0)]
        public long Int64;

        /// <summary>
        /// Gets or sets the <see cref="ulong"/> value.
        /// </summary>
        [FieldOffset(0)]
        public long UInt64;

        /// <summary>
        /// Gets or sets the <see cref="double"/> value.
        /// </summary>
        [FieldOffset(0)]
        public double Double;

        /// <summary>
        /// Gets or sets the low <see cref="Blit32"/> value.
        /// </summary>
        [FieldOffset(0)]
        public Blit32 Blit0;

        /// <summary>
        /// Gets or sets the high <see cref="Blit32"/> value.
        /// </summary>
        [FieldOffset(4)]
        public Blit32 Blit1;

        /// <summary>
        /// Gets or sets the first <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(0)]
        public Byte Byte0;

        /// <summary>
        /// Gets or sets the second <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(1)]
        public Byte Byte1;

        /// <summary>
        /// Gets or sets the third <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(2)]
        public Byte Byte2;

        /// <summary>
        /// Gets or sets the fourth <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(3)]
        public Byte Byte3;

        /// <summary>
        /// Gets or sets the fifth <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(4)]
        public Byte Byte4;

        /// <summary>
        /// Gets or sets the sixth <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(5)]
        public Byte Byte5;

        /// <summary>
        /// Gets or sets the seventh <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(6)]
        public Byte Byte6;

        /// <summary>
        /// Gets or sets the eighth <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(7)]
        public Byte Byte7;
    }
}

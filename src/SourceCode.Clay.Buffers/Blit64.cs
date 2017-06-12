using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents the blittable components of a 64-bit integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
    [DebuggerDisplay("{Int64,nq}")]
    public struct Blit64
    {
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
        /// Gets or sets the fifth <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(4)]
        public byte Byte4;

        /// <summary>
        /// Gets or sets the sixth <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(5)]
        public byte Byte5;

        /// <summary>
        /// Gets or sets the seventh <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(6)]
        public byte Byte6;

        /// <summary>
        /// Gets or sets the eighth <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(7)]
        public byte Byte7;

        /// <summary>
        /// Gets or sets the <see cref="long"/> value.
        /// </summary>
        [FieldOffset(0)]
        public long Int64;

        /// <summary>
        /// Gets or sets the <see cref="ulong"/> value.
        /// </summary>
        [FieldOffset(0)]
        public ulong UInt64;

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

        #region Bytes

        /// <summary>
        /// The bytes in unsafe form.
        /// </summary>
        [FieldOffset(0)] // 0-7
        public unsafe fixed byte Bytes[8];

        /// <summary>
        /// Gets the bytes as an array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
            => new[]
            {
                Byte0, Byte1, Byte2, Byte3,
                Byte4, Byte5, Byte6, Byte7
            };

        #endregion
    }
}

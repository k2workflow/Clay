using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents the blittable components of a 16-bit integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 2)]
    [DebuggerDisplay("{Int16,nq}")]
    public struct Blit16
    {
#pragma warning disable S1104 // Fields should not have public accessibility

        /// <summary>
        /// Gets or sets low <see cref="byte"/>.
        /// </summary>
        [FieldOffset(0)]
        public byte Byte0;

        /// <summary>
        /// Gets or sets high <see cref="byte"/>.
        /// </summary>
        [FieldOffset(1)]
        public byte Byte1;

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
        /// The bytes in unsafe form.
        /// </summary>
        [FieldOffset(0)] // 0-1
        public unsafe fixed byte Bytes[2];

#pragma warning restore S1104 // Fields should not have public accessibility

        /// <summary>
        /// Gets the bytes as an array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
            => new[] { Byte0, Byte1 };
    }
}

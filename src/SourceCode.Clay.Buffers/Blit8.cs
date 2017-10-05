using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    ///   Represents the blittable components of a 8-bit integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 1)]
    [DebuggerDisplay("{Byte,nq}")]
    public struct Blit8
    {
#pragma warning disable S1104 // Fields should not have public accessibility

        /// <summary>
        ///   Gets or sets the <see cref="byte"/> value.
        /// </summary>
        [FieldOffset(0)]
        public byte Byte;

        /// <summary>
        ///   Gets or sets the <see cref="sbyte"/> value.
        /// </summary>
        [FieldOffset(0)]
        public sbyte SByte;

        /// <summary>
        ///   The bytes in unsafe form.
        /// </summary>
        [FieldOffset(0)] // 0
        public unsafe fixed byte Bytes[1];

#pragma warning restore S1104 // Fields should not have public accessibility

        /// <summary>
        ///   Gets the bytes as an array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
            => new[] { Byte };
    }
}

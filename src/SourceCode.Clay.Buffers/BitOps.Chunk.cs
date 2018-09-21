using System.Runtime.CompilerServices;

namespace System
{
#pragma warning disable IDE0007 // Use implicit type

    partial class BitOps // .Chunk
    {
        #region ExtractByte

        public static byte ExtractByte(ushort value, int bitOffset)
        {
            byte shft = Mod(sizeof(ushort), bitOffset, sizeof(byte));

            var val = (uint)(value >> shft);
            return (byte)val;
        }

        public static byte ExtractByte(uint value, int bitOffset)
        {
            byte shft = Mod(sizeof(uint), bitOffset, sizeof(byte));

            uint val = value >> shft;
            return (byte)val;
        }

        public static byte ExtractByte(ulong value, int bitOffset)
        {
            byte shft = Mod(sizeof(ulong), bitOffset, sizeof(byte));

            ulong val = value >> shft;
            return (byte)val;
        }

        #endregion

        #region InsertByte

        // FF00
        private const uint InsertByteMask = (uint)byte.MaxValue << sizeof(byte);

        public static ushort InsertByte(ushort value, int bitOffset, byte insert)
            => (ushort)InsertByteImpl(sizeof(ushort), value, bitOffset, insert);
        
        public static uint InsertByte(uint value, int bitOffset, byte insert)
            => InsertByteImpl(sizeof(uint), value, bitOffset, insert);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint InsertByteImpl(byte valueSize, uint value, int bitOffset, byte insert)
        {
            // eg, offset = 3
            // value = 1100_1100_1001_1101
            // insert =      100 0011 1

            // eg 27->3, int.Max -> 7
            byte shft = Mod(valueSize, bitOffset, sizeof(byte));
            var ins = (uint)(insert << shft); //                 0000_0 | 100_0 011_1 | 000

            //                                                   15  12 | 11  8 7   3 |   0
            uint mask = RotateLeft(InsertByteMask, shft); //     1111_1 | 000_0 000_0 | 111
                                                                
            // value                                             1100_1 | 100_1 001_1 | 101
            uint val = value & mask; //                          1100_1 | 000_0 000_0 | 101
            val |= ins; //                                       1100_1 | 100_0 011_1 | 101

            // value       1100_1 1001 0011 101
            // insert             1000 0111
            return val; // 1100_1 1000 0111 101
        }

        public static ulong InsertByte(ulong value, int bitOffset, byte insert)
        {
            byte shft = Mod(sizeof(ulong), bitOffset, sizeof(byte));
            var ins = (ulong)(insert << shft);

            ulong mask = RotateLeft((ulong)InsertByteMask, shft);

            ulong val = (value & mask) | ins;
            return val;
        }

        #endregion

        #region ExtractUInt16

        public static ushort ExtractUInt16(uint value, int bitOffset)
        {
            byte shft = Mod(sizeof(uint), bitOffset, sizeof(ushort));

            uint val = value >> shft;
            return (ushort)val;
        }

        public static ushort ExtractUInt16(ulong value, int bitOffset)
        {
            byte shft = Mod(sizeof(ulong), bitOffset, sizeof(ushort));

            ulong val = value >> shft;
            return (ushort)val;
        }

        #endregion

        #region InsertUInt16

        // FFFF 0000
        private const uint InsertUInt16Mask = (uint)ushort.MaxValue << sizeof(ushort);

        public static uint InsertUInt16(uint value, int bitOffset, ushort insert)
        {
            byte shft = Mod(sizeof(uint), bitOffset, sizeof(ushort));
            var ins = (uint)(insert << shft);

            uint mask = RotateLeft(InsertUInt16Mask, shft);

            uint val = (value & mask) | ins;
            return val;
        }

        public static ulong InsertUInt16(ulong value, int bitOffset, ushort insert)
        {
            byte shft = Mod(sizeof(ulong), bitOffset, sizeof(ushort));
            var ins = (ulong)(insert << shft);

            ulong mask = RotateLeft((ulong)InsertUInt16Mask, shft);

            ulong val = (value & mask) | ins;
            return val;
        }

        #endregion

        #region ExtractUInt32

        public static uint ExtractUInt32(ulong value, int bitOffset)
        {
            byte shft = Mod(sizeof(ulong), bitOffset, sizeof(uint));

            ulong val = value >> shft;
            return (uint)val;
        }

        #endregion

        #region InsertUInt32

        // FFFF FFFF 0000 0000
        private const ulong InsertUInt32Mask = (ulong)uint.MaxValue << sizeof(uint);

        public static ulong InsertUInt32(ulong value, int bitOffset, uint insert)
        {
            byte shft = Mod(sizeof(ulong), bitOffset, sizeof(uint));
            var ins = (ulong)(insert << shft);

            ulong mask = RotateLeft(InsertUInt32Mask, shft);

            ulong val = (value & mask) | ins;
            return val;
        }

        #endregion

        #region Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte Mod(byte valueSize, int bitOffset, byte size)
            => (byte)(unchecked(bitOffset + size) & (valueSize - 1));

        #endregion
    }

#pragma warning restore IDE0007 // Use implicit type
}

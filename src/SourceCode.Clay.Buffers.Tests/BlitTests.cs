using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class BlitTests
    {
        [Fact(DisplayName = "BlitExtensions RotateLeft Byte")]
        public static void Blit_RotateLeft_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, Blit.RotateLeft(sut, 1));
            Assert.Equal((byte)0b01010101, Blit.RotateLeft(sut, 2));
            Assert.Equal((byte)0b10101010, Blit.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateLeft UShort")]
        public static void Blit_RotateLeft_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, Blit.RotateLeft(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, Blit.RotateLeft(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, Blit.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateLeft UInt")]
        public static void Blit_RotateLeft_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, Blit.RotateLeft(sut, 1));
            Assert.Equal((uint)0b01010101_01010101_01010101_01010101, Blit.RotateLeft(sut, 2));
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, Blit.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateLeft ULong")]
        public static void Blit_RotateLeft_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, Blit.RotateLeft(sut, 1));
            Assert.Equal((ulong)0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, Blit.RotateLeft(sut, 2));
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, Blit.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateRight Byte")]
        public static void Blit_RotateRight_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, Blit.RotateRight(sut, 1));
            Assert.Equal((byte)0b01010101, Blit.RotateRight(sut, 2));
            Assert.Equal((byte)0b10101010, Blit.RotateRight(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateRight UShort")]
        public static void Blit_RotateRight_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, Blit.RotateRight(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, Blit.RotateRight(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, Blit.RotateRight(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateRight UInt")]
        public static void Blit_RotateRight_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, Blit.RotateRight(sut, 1));
            Assert.Equal((uint)0b01010101_01010101_01010101_01010101, Blit.RotateRight(sut, 2));
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, Blit.RotateRight(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateRight ULong")]
        public static void Blit_RotateRight_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, Blit.RotateRight(sut, 1));
            Assert.Equal((ulong)0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, Blit.RotateRight(sut, 2));
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, Blit.RotateRight(sut, 3));
        }
    }
}

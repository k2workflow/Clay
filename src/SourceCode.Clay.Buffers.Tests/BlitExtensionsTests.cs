using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public class BlitExtensionsTests
    {
        [Fact(DisplayName = "BlitExtensions RotateLeft Byte")]
        public void BlitExtensions_RotateLeft_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, BlitExtensions.RotateLeft(sut, 1));
            Assert.Equal((byte)0b01010101, BlitExtensions.RotateLeft(sut, 2));
            Assert.Equal((byte)0b10101010, BlitExtensions.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateLeft UShort")]
        public void BlitExtensions_RotateLeft_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, BlitExtensions.RotateLeft(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, BlitExtensions.RotateLeft(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, BlitExtensions.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateLeft UInt")]
        public void BlitExtensions_RotateLeft_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, BlitExtensions.RotateLeft(sut, 1));
            Assert.Equal((uint)0b01010101_01010101_01010101_01010101, BlitExtensions.RotateLeft(sut, 2));
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, BlitExtensions.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateLeft ULong")]
        public void BlitExtensions_RotateLeft_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, BlitExtensions.RotateLeft(sut, 1));
            Assert.Equal((ulong)0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, BlitExtensions.RotateLeft(sut, 2));
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, BlitExtensions.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateRight Byte")]
        public void BlitExtensions_RotateRight_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, BlitExtensions.RotateRight(sut, 1));
            Assert.Equal((byte)0b01010101, BlitExtensions.RotateRight(sut, 2));
            Assert.Equal((byte)0b10101010, BlitExtensions.RotateRight(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateRight UShort")]
        public void BlitExtensions_RotateRight_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, BlitExtensions.RotateRight(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, BlitExtensions.RotateRight(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, BlitExtensions.RotateRight(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateRight UInt")]
        public void BlitExtensions_RotateRight_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, BlitExtensions.RotateRight(sut, 1));
            Assert.Equal((uint)0b01010101_01010101_01010101_01010101, BlitExtensions.RotateRight(sut, 2));
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, BlitExtensions.RotateRight(sut, 3));
        }

        [Fact(DisplayName = "BlitExtensions RotateRight ULong")]
        public void BlitExtensions_RotateRight_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, BlitExtensions.RotateRight(sut, 1));
            Assert.Equal((ulong)0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, BlitExtensions.RotateRight(sut, 2));
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, BlitExtensions.RotateRight(sut, 3));
        }
    }
}

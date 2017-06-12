using Xunit;

namespace SourceCode.Clay.Tests
{
    public class MathExtensionsTests
    {
        [Fact(DisplayName = "MathExtensions RotateLeft Byte")]
        public void MathExtensions_RotateLeft_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, MathExtensions.RotateLeft(sut, 1));
            Assert.Equal((byte)0b01010101, MathExtensions.RotateLeft(sut, 2));
            Assert.Equal((byte)0b10101010, MathExtensions.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "MathExtensions RotateLeft UShort")]
        public void MathExtensions_RotateLeft_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, MathExtensions.RotateLeft(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, MathExtensions.RotateLeft(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, MathExtensions.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "MathExtensions RotateLeft UInt")]
        public void MathExtensions_RotateLeft_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, MathExtensions.RotateLeft(sut, 1));
            Assert.Equal((uint)0b01010101_01010101_01010101_01010101, MathExtensions.RotateLeft(sut, 2));
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, MathExtensions.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "MathExtensions RotateLeft ULong")]
        public void MathExtensions_RotateLeft_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, MathExtensions.RotateLeft(sut, 1));
            Assert.Equal((ulong)0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, MathExtensions.RotateLeft(sut, 2));
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, MathExtensions.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = "MathExtensions RotateRight Byte")]
        public void MathExtensions_RotateRight_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, MathExtensions.RotateRight(sut, 1));
            Assert.Equal((byte)0b01010101, MathExtensions.RotateRight(sut, 2));
            Assert.Equal((byte)0b10101010, MathExtensions.RotateRight(sut, 3));
        }

        [Fact(DisplayName = "MathExtensions RotateRight UShort")]
        public void MathExtensions_RotateRight_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, MathExtensions.RotateRight(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, MathExtensions.RotateRight(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, MathExtensions.RotateRight(sut, 3));
        }

        [Fact(DisplayName = "MathExtensions RotateRight UInt")]
        public void MathExtensions_RotateRight_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, MathExtensions.RotateRight(sut, 1));
            Assert.Equal((uint)0b01010101_01010101_01010101_01010101, MathExtensions.RotateRight(sut, 2));
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, MathExtensions.RotateRight(sut, 3));
        }

        [Fact(DisplayName = "MathExtensions RotateRight ULong")]
        public void MathExtensions_RotateRight_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, MathExtensions.RotateRight(sut, 1));
            Assert.Equal((ulong)0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, MathExtensions.RotateRight(sut, 2));
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, MathExtensions.RotateRight(sut, 3));
        }
    }
}

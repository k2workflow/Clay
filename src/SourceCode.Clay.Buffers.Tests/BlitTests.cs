#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class BlitTests
    {
        [Fact(DisplayName = nameof(Blit_RotateLeft_Byte))]
        public static void Blit_RotateLeft_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, Blit.RotateLeft(sut, 1));
            Assert.Equal((byte)0b01010101, Blit.RotateLeft(sut, 2));
            Assert.Equal((byte)0b10101010, Blit.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = nameof(Blit_RotateLeft_UShort))]
        public static void Blit_RotateLeft_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, Blit.RotateLeft(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, Blit.RotateLeft(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, Blit.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = nameof(Blit_RotateLeft_UInt))]
        public static void Blit_RotateLeft_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, Blit.RotateLeft(sut, 1));
            Assert.Equal((uint)0b01010101_01010101_01010101_01010101, Blit.RotateLeft(sut, 2));
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, Blit.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = nameof(Blit_RotateLeft_ULong))]
        public static void Blit_RotateLeft_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, Blit.RotateLeft(sut, 1));
            Assert.Equal((ulong)0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, Blit.RotateLeft(sut, 2));
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, Blit.RotateLeft(sut, 3));
        }

        [Fact(DisplayName = nameof(Blit_RotateRight_Byte))]
        public static void Blit_RotateRight_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, Blit.RotateRight(sut, 1));
            Assert.Equal((byte)0b01010101, Blit.RotateRight(sut, 2));
            Assert.Equal((byte)0b10101010, Blit.RotateRight(sut, 3));
        }

        [Fact(DisplayName = nameof(Blit_RotateRight_UShort))]
        public static void Blit_RotateRight_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, Blit.RotateRight(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, Blit.RotateRight(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, Blit.RotateRight(sut, 3));
        }

        [Fact(DisplayName = nameof(Blit_RotateRight_UInt))]
        public static void Blit_RotateRight_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, Blit.RotateRight(sut, 1));
            Assert.Equal((uint)0b01010101_01010101_01010101_01010101, Blit.RotateRight(sut, 2));
            Assert.Equal((uint)0b10101010_10101010_10101010_10101010, Blit.RotateRight(sut, 3));
        }

        [Fact(DisplayName = nameof(Blit_RotateRight_ULong))]
        public static void Blit_RotateRight_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, Blit.RotateRight(sut, 1));
            Assert.Equal((ulong)0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, Blit.RotateRight(sut, 2));
            Assert.Equal((ulong)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010, Blit.RotateRight(sut, 3));
        }

        [Theory(DisplayName = nameof(Blit_FloorLog2_opt5))]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(4, 2)]
        [InlineData(5, 2)]
        public static void Blit_FloorLog2_opt5(uint n, int expected)
        {
            // Test the optimization trick on the lower boundary (1-5)
            var actual = Blit.FloorLog2(n);
            Assert.Equal(expected, (int)actual);
        }

        [Theory(DisplayName = nameof(Blit_FloorLog2_32u))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(1023)]
        [InlineData(1024)]
        [InlineData(1025)]
        [InlineData((uint)sbyte.MaxValue)]
        [InlineData((uint)byte.MaxValue)]
        [InlineData((uint)short.MaxValue)]
        [InlineData((uint)ushort.MaxValue)]
        [InlineData((uint)int.MaxValue - 1)]
        [InlineData((uint)int.MaxValue)] // (1U << 31) - 1
        [InlineData((uint)int.MaxValue + 1)] // 1U << 31
        [InlineData((1U << 31) + 1)]
        [InlineData(uint.MaxValue - 1)]
        [InlineData(uint.MaxValue)]
        public static void Blit_FloorLog2_32u(uint n)
        {
            var log = Blit.FloorLog2(n);

            var lo = Math.Pow(2, log);
            var hi = Math.Pow(2, log + 1);

            Assert.InRange(n, lo, hi);
        }

        [Theory(DisplayName = nameof(Blit_FloorLog2_32))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(1023)]
        [InlineData(1024)]
        [InlineData(1025)]
        [InlineData((int)sbyte.MaxValue)]
        [InlineData((int)byte.MaxValue)]
        [InlineData((int)short.MaxValue)]
        [InlineData((int)ushort.MaxValue)]
        [InlineData((1 << 30) - 1)]
        [InlineData(1 << 30)]
        [InlineData((1 << 30) + 1)]
        [InlineData(int.MaxValue - 1)]
        [InlineData(int.MaxValue)]
        public static void Blit_FloorLog2_32(int n)
        {
            var log = Blit.FloorLog2(n);

            var lo = Math.Pow(2, log);
            var hi = Math.Pow(2, log + 1);

            Assert.InRange(n, lo, hi);
        }

        [Theory(DisplayName = nameof(Blit_FloorLog2_64u))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(1023)]
        [InlineData(1024)]
        [InlineData(1025)]
        [InlineData((ulong)sbyte.MaxValue)]
        [InlineData((ulong)byte.MaxValue)]
        [InlineData((ulong)short.MaxValue)]
        [InlineData((ulong)ushort.MaxValue)]
        [InlineData((ulong)int.MaxValue - 1)]
        [InlineData((ulong)int.MaxValue)]
        [InlineData((ulong)int.MaxValue + 1)]
        [InlineData((ulong)uint.MaxValue - 1)]
        [InlineData((ulong)uint.MaxValue)]
        [InlineData((ulong)uint.MaxValue + 1)]
        [InlineData((1UL << 63) - 1)]
        [InlineData(1UL << 63)]
        [InlineData((1UL << 63) + 1)]
        [InlineData(ulong.MaxValue - 1)]
        [InlineData(ulong.MaxValue)]
        public static void Blit_FloorLog2_64u(ulong n)
        {
            var log = Blit.FloorLog2(n);

            var lo = Math.Pow(2, log);
            var hi = Math.Pow(2, log + 1);

            Assert.InRange(n, lo, hi);
        }

        [Theory(DisplayName = nameof(Blit_FloorLog2_64))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(1023)]
        [InlineData(1024)]
        [InlineData(1025)]
        [InlineData((long)sbyte.MaxValue)]
        [InlineData((long)byte.MaxValue)]
        [InlineData((long)short.MaxValue)]
        [InlineData((long)ushort.MaxValue)]
        [InlineData((long)int.MaxValue - 1)]
        [InlineData((long)int.MaxValue)]
        [InlineData((long)int.MaxValue + 1)]
        [InlineData((long)uint.MaxValue - 1)]
        [InlineData((long)uint.MaxValue)]
        [InlineData((long)uint.MaxValue + 1)]
        [InlineData((1L << 62) - 1)]
        [InlineData(1L << 62)]
        [InlineData((1L << 62) + 1)]
        [InlineData(long.MaxValue - 1)]
        [InlineData(long.MaxValue)]
        public static void Blit_FloorLog2_64(long n)
        {
            var log = Blit.FloorLog2(n);

            var lo = Math.Pow(2, log);
            var hi = Math.Pow(2, log + 1);

            Assert.InRange(n, lo, hi);
        }

        [Fact(DisplayName = nameof(Blit_FloorLog2_Throws))]
        public static void Blit_FloorLog2_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Blit.FloorLog2(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Blit.FloorLog2((uint)0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Blit.FloorLog2((ulong)0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Blit.FloorLog2((long)0));
        }

        [Theory(DisplayName = nameof(Blit_PopCount_32u))]
        [InlineData(0b000, 0)]
        [InlineData(0b001, 1)]
        [InlineData(0b010, 1)]
        [InlineData(0b011, 2)]
        [InlineData(0b100, 1)]
        [InlineData(0b101, 2)]
        [InlineData(0b110, 2)]
        [InlineData(0b111, 3)]
        [InlineData(0b1101, 3)]
        [InlineData(0b1111, 4)]
        [InlineData(0b10111, 4)]
        [InlineData(0b11111, 5)]
        [InlineData(0b110111, 5)]
        [InlineData(0b111111, 6)]
        [InlineData(0b1111110, 6)]
        [InlineData(0b1111111, 7)]
        [InlineData(byte.MaxValue, 8)]
        [InlineData(ushort.MaxValue >> 3, 16 - 3)]
        [InlineData(ushort.MaxValue, 16)]
        [InlineData(uint.MaxValue >> 5, 32 - 5)]
        [InlineData(uint.MaxValue, 32)]
        public static void Blit_PopCount_32u(uint n, int expected)
        {
            var actual = Blit.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(Blit_PopCount_64u))]
        [InlineData(0b000, 0)]
        [InlineData(0b001, 1)]
        [InlineData(0b010, 1)]
        [InlineData(0b011, 2)]
        [InlineData(0b100, 1)]
        [InlineData(0b101, 2)]
        [InlineData(0b110, 2)]
        [InlineData(0b111, 3)]
        [InlineData(0b1101, 3)]
        [InlineData(0b1111, 4)]
        [InlineData(0b10111, 4)]
        [InlineData(0b11111, 5)]
        [InlineData(0b110111, 5)]
        [InlineData(0b111111, 6)]
        [InlineData(0b1111110, 6)]
        [InlineData(0b1111111, 7)]
        [InlineData(byte.MaxValue, 8)]
        [InlineData(ushort.MaxValue >> 3, 16 - 3)]
        [InlineData(ushort.MaxValue, 16)]
        [InlineData(uint.MaxValue >> 5, 32 - 5)]
        [InlineData(uint.MaxValue, 32)]
        [InlineData(ulong.MaxValue >> 7, 64 - 7)]
        [InlineData(ulong.MaxValue, 64)]
        public static void Blit_PopCount_64u(ulong n, int expected)
        {
            var actual = Blit.PopCount(n);
            Assert.Equal(expected, actual);
        }
    }
}

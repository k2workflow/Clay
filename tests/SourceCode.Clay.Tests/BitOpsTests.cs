#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Runtime.CompilerServices;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static partial class BitOpsTests // .Primitive
    {
        #region Samples

        [Fact]
        public static void BitOps_Samples()
        {
            // PopCount: Returns the population count (number of bits set) of a mask.
            Assert.Equal(4, BitOps.PopCount(0x1001_0110u));

            // RotateLeft/Right: Rotates the specified value left/right by the specified number of bits.
            Assert.Equal(0b0100_0001u, BitOps.RotateLeft((byte)0b0010_1000u, 3));
            Assert.Equal(0b1000_0000u, BitOps.RotateRight((byte)0b0010_0000u, 6));

            // Leading/TrailingZeros: Count the number of leading/trailing zero bits in a mask.
            Assert.Equal(2, BitOps.LeadingZeroCount((byte)0b0011_1000));
            Assert.Equal(5, BitOps.TrailingZeroCount(0b1110_0000));

            // ExtractBit: Reads whether the specified bit in a mask is set.
            Assert.True(BitOps.ExtractBit((byte)0b0001_0000, 4));
            Assert.False(BitOps.ExtractBit((byte)0b0001_0000, 7));

            // InsertBit: Sets the specified bit in a mask and returns the new value.
            byte dest = 0b0000_1001;
            Assert.Equal(0b0010_1001, BitOps.InsertBit(dest, 5));

            // InsertBit(ref): Sets the specified bit in a mask and returns whether it was originally set.
            Assert.False(BitOps.InsertBit(ref dest, 5));
            Assert.Equal(0b0010_1001, dest);

            // ClearBit: Clears the specified bit in a mask and returns the new value.
            dest = 0b0000_1001;
            Assert.Equal(0b0000_0001, BitOps.ClearBit(dest, 3));
            // ClearBit(ref): Clears the specified bit in a mask and returns whether it was originally set.
            Assert.True(BitOps.ClearBit(ref dest, 3));
            Assert.Equal(0b0000_0001, dest);

            // ComplementBit: Complements the specified bit in a mask and returns the new value.
            dest = 0b0000_1001;
            Assert.Equal(0b0000_0001, BitOps.ComplementBit(dest, 3));
            // ComplementBit(ref): Complements the specified bit in a mask and returns whether it was originally set.
            Assert.True(BitOps.ComplementBit(ref dest, 3));
            Assert.Equal(0b0000_0001, dest);

            // WriteBit: Writes the specified bit in a mask and returns the new value. Does not branch.
            dest = 0b0000_1001;
            Assert.Equal(0b0000_0001, BitOps.WriteBit(dest, 3, on: false));
            // WriteBit(ref): Writes the specified bit in a mask and returns whether it was originally set. Does not branch.
            Assert.True(BitOps.WriteBit(ref dest, 3, on: false));
            Assert.Equal(0b0000_0001, dest);

            // AsByte: Converts a boolean to a normalized byte value without branching.
            byte n = 3; // Build a bool that has an underlying value of 3
            bool weirdBool = Unsafe.As<byte, bool>(ref n); // Via interop or whatever
            Assert.True(weirdBool);
            Assert.Equal(1, BitOps.Iff(ref weirdBool)); // Normalize

            // Iff: Converts a boolean to a specified integer value without branching.
            Assert.Equal(123, BitOps.Iff(true, 123)); // if then
            Assert.Equal(456, BitOps.Iff(false, 123, 456)); // if then else
        }

        #endregion

        #region ExtractBit

        [Theory]
        [InlineData(0b000, 0, false)]
        [InlineData(0b001, 0, true)]
        [InlineData(0b000, 1, false)]
        [InlineData(0b010, 1, true)]
        [InlineData(1, int.MinValue, true)] // % 8 = 0
        [InlineData(1 << 7, int.MaxValue, true)] // % 8 = 7
        [InlineData(byte.MaxValue, 7, true)]
        [InlineData(byte.MaxValue, 8, true)]
        public static void BitOps_ExtractBit_byte(byte n, int offset, bool expected)
        {
            bool actual = BitOps.ExtractBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset + 8 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset - 8 * 2);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData((sbyte)0b000, 0, false)]
        [InlineData((sbyte)0b001, 0, true)]
        [InlineData((sbyte)0b000, 1, false)]
        [InlineData((sbyte)0b010, 1, true)]
        [InlineData((sbyte)1, int.MinValue, true)] // % 8 = 0
        [InlineData(unchecked((sbyte)(1 << 7)), int.MaxValue, true)] // % 8 = 7
        [InlineData(sbyte.MaxValue, 7, false)]
        [InlineData(sbyte.MaxValue, 8, true)]
        [InlineData(sbyte.MinValue, 7, true)]
        [InlineData(sbyte.MinValue, 8, false)]
        public static void BitOps_ExtractBit_sbyte(sbyte n, int offset, bool expected)
        {
            bool actual = BitOps.ExtractBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset + 8 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset - 8 * 2);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0b000, 0, false)]
        [InlineData(0b001, 0, true)]
        [InlineData(0b000, 1, false)]
        [InlineData(0b010, 1, true)]
        [InlineData(1, int.MinValue, true)] // % 16 = 0
        [InlineData(1 << 15, int.MaxValue, true)] // % 16 = 15
        [InlineData(ushort.MaxValue, 15, true)]
        [InlineData(ushort.MaxValue, 16, true)]
        public static void BitOps_ExtractBit_ushort(ushort n, int offset, bool expected)
        {
            bool actual = BitOps.ExtractBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset + 16 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset - 16 * 2);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData((short)0b000, 0, false)]
        [InlineData((short)0b001, 0, true)]
        [InlineData((short)0b000, 1, false)]
        [InlineData((short)0b010, 1, true)]
        [InlineData((short)1, int.MinValue, true)] // % 16 = 0
        [InlineData(unchecked((short)(1 << 15)), int.MaxValue, true)] // % 16 = 15
        [InlineData(short.MaxValue, 15, false)]
        [InlineData(short.MaxValue, 16, true)]
        [InlineData(short.MinValue, 15, true)]
        [InlineData(short.MinValue, 16, false)]
        public static void BitOps_ExtractBit_short(short n, int offset, bool expected)
        {
            bool actual = BitOps.ExtractBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset + 16 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset - 16 * 2);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0b000u, 0, false)]
        [InlineData(0b001u, 0, true)]
        [InlineData(0b000u, 1, false)]
        [InlineData(0b010u, 1, true)]
        [InlineData(1u, int.MinValue, true)] // % 32 = 0
        [InlineData(1u << 31, int.MaxValue, true)] // % 32 = 31
        [InlineData(uint.MaxValue, 31, true)]
        [InlineData(uint.MaxValue, 32, true)]
        public static void BitOps_ExtractBit_uint(uint n, int offset, bool expected)
        {
            bool actual = BitOps.ExtractBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset + 32 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset - 32 * 2);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0b000, 0, false)]
        [InlineData(0b001, 0, true)]
        [InlineData(0b000, 1, false)]
        [InlineData(0b010, 1, true)]
        [InlineData(1, int.MinValue, true)] // % 32 = 0
        [InlineData(1 << 31, int.MaxValue, true)] // % 32 = 31
        [InlineData(int.MaxValue, 31, false)]
        [InlineData(int.MaxValue, 32, true)]
        [InlineData(int.MinValue, 31, true)]
        [InlineData(int.MinValue, 32, false)]
        public static void BitOps_ExtractBit_int(int n, int offset, bool expected)
        {
            bool actual = BitOps.ExtractBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset + 32 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset - 32 * 2);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0b000ul, 0, false)]
        [InlineData(0b001ul, 0, true)]
        [InlineData(0b000ul, 1, false)]
        [InlineData(0b010ul, 1, true)]
        [InlineData(1ul, int.MinValue, true)] // % 64 = 0
        [InlineData(1ul << 63, int.MaxValue, true)] // % 64 = 63
        [InlineData(ulong.MaxValue, 63, true)]
        [InlineData(ulong.MaxValue, 64, true)]
        public static void BitOps_ExtractBit_ulong(ulong n, int offset, bool expected)
        {
            bool actual = BitOps.ExtractBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset + 64 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset - 64 * 2);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0b000L, 0, false)]
        [InlineData(0b001L, 0, true)]
        [InlineData(0b000L, 1, false)]
        [InlineData(0b010L, 1, true)]
        [InlineData(1L, int.MinValue, true)] // % 64 = 0
        [InlineData(1L << 63, int.MaxValue, true)] // % 64 = 63
        [InlineData(long.MaxValue, 63, false)]
        [InlineData(long.MaxValue, 64, true)]
        [InlineData(long.MinValue, 63, true)]
        [InlineData(long.MinValue, 64, false)]
        public static void BitOps_ExtractBit_long(long n, int offset, bool expected)
        {
            bool actual = BitOps.ExtractBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset + 64 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ExtractBit(n, offset - 64 * 2);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region WriteBit

        [Theory]
        [InlineData(0b000, 0, false, false, 0b000)] // 0
        [InlineData(0b000, 0, true, false, 0b001)]
        [InlineData(0b000, 1, false, false, 0b000)]
        [InlineData(0b000, 1, true, false, 0b010)]
        [InlineData(0b001, 0, false, true, 0b000)] // 1
        [InlineData(0b001, 0, true, true, 0b001)]
        [InlineData(0b001, 1, false, false, 0b001)]
        [InlineData(0b001, 1, true, false, 0b011)]
        [InlineData(0b010, 0, false, false, 0b010)] // 2
        [InlineData(0b010, 0, true, false, 0b011)]
        [InlineData(0b010, 1, false, true, 0b000)]
        [InlineData(0b010, 1, true, true, 0b010)]
        [InlineData(0b011, 0, false, true, 0b010)] // 3
        [InlineData(0b011, 0, true, true, 0b011)]
        [InlineData(0b011, 1, false, true, 0b001)]
        [InlineData(0b011, 1, true, true, 0b011)]
        [InlineData(0b011, 0 - 8 * 2, false, true, 0b010)] // == 1
        [InlineData(0b011, 0 - 8 * 2, true, true, 0b011)]
        [InlineData(0b011, 1 - 8 * 2, false, true, 0b001)]
        [InlineData(0b011, 1 - 8 * 2, true, true, 0b011)]
        [InlineData(1, int.MinValue, false, true, 0)] // % 8 = 0
        //[InlineData(1 << 7, int.MaxValue, false, true, 0)] // % 8 = 7 TODO
        [InlineData(byte.MaxValue, 0, false, true, byte.MaxValue - 1)]
        [InlineData(byte.MaxValue, 0, true, true, byte.MaxValue)]
        [InlineData(byte.MaxValue, 7, false, true, byte.MaxValue >> 1)]
        [InlineData(byte.MaxValue, 7, true, true, byte.MaxValue)]
        [InlineData(byte.MaxValue, 8, false, true, byte.MaxValue - 1)]
        public static void BitOps_WriteBit_byte(byte n, int offset, bool on, bool was, byte expected)
        {
            // Scalar
            byte actual = on ? BitOps.InsertBit(n, offset) : BitOps.ClearBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.WriteBit(n, offset, on);
            Assert.Equal(expected, actual);

            if (actual != n)
            {
                actual = !on ? BitOps.InsertBit(actual, offset) : BitOps.ClearBit(actual, offset);
                Assert.Equal(n, actual);

                actual = BitOps.WriteBit(actual, offset, !on);
                Assert.Equal(n, actual);
            }

            // Ref
            actual = n;
            Assert.Equal(was, on ? BitOps.InsertBit(ref actual, offset) : BitOps.ClearBit(ref actual, offset));
            Assert.Equal(expected, actual);

            actual = n;
            Assert.Equal(was, BitOps.WriteBit(ref actual, offset, on));
            Assert.Equal(expected, actual);

            if (actual != n)
            {
                Assert.Equal(!was, on ? BitOps.ClearBit(ref actual, offset) : BitOps.InsertBit(ref actual, offset));
                Assert.Equal(n, actual);

                Assert.Equal(was, BitOps.WriteBit(ref actual, offset, on));
                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(0b000, 0, false, false, 0b000)] // 0
        [InlineData(0b000, 0, true, false, 0b001)]
        [InlineData(0b000, 1, false, false, 0b000)]
        [InlineData(0b000, 1, true, false, 0b010)]
        [InlineData(0b001, 0, false, true, 0b000)] // 1
        [InlineData(0b001, 0, true, true, 0b001)]
        [InlineData(0b001, 1, false, false, 0b001)]
        [InlineData(0b001, 1, true, false, 0b011)]
        [InlineData(0b010, 0, false, false, 0b010)] // 2
        [InlineData(0b010, 0, true, false, 0b011)]
        [InlineData(0b010, 1, false, true, 0b000)]
        [InlineData(0b010, 1, true, true, 0b010)]
        [InlineData(0b011, 0, false, true, 0b010)] // 3
        [InlineData(0b011, 0, true, true, 0b011)]
        [InlineData(0b011, 1, false, true, 0b001)]
        [InlineData(0b011, 1, true, true, 0b011)]
        [InlineData(0b011, 0 - 16 * 2, false, true, 0b010)] // == 1
        [InlineData(0b011, 0 - 16 * 2, true, true, 0b011)]
        [InlineData(0b011, 1 - 16 * 2, false, true, 0b001)]
        [InlineData(0b011, 1 - 16 * 2, true, true, 0b011)]
        [InlineData(1, int.MinValue, false, true, 0)] // % 16 = 0
        //[InlineData(1 << 15, int.MaxValue, false, true, 0)] // % 16 = 15 TODO
        [InlineData(byte.MaxValue, 0, false, true, byte.MaxValue - 1)]
        [InlineData(byte.MaxValue, 0, true, true, byte.MaxValue)]
        [InlineData(byte.MaxValue, 7, false, true, byte.MaxValue >> 1)]
        [InlineData(byte.MaxValue, 7, true, true, byte.MaxValue)]
        [InlineData(byte.MaxValue, 8, false, false, byte.MaxValue)]
        [InlineData(byte.MaxValue, 8, true, false, byte.MaxValue + (1u << 8))]
        [InlineData(ushort.MaxValue, 0, false, true, ushort.MaxValue - 1)]
        [InlineData(ushort.MaxValue, 0, true, true, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 15, false, true, ushort.MaxValue >> 1)]
        [InlineData(ushort.MaxValue, 15, true, true, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 16, false, true, ushort.MaxValue - 1)]
        public static void BitOps_WriteBit_ushort(ushort n, int offset, bool on, bool was, ushort expected)
        {
            // Scalar
            ushort actual = on ? BitOps.InsertBit(n, offset) : BitOps.ClearBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.WriteBit(n, offset, on);
            Assert.Equal(expected, actual);

            if (actual != n)
            {
                actual = !on ? BitOps.InsertBit(actual, offset) : BitOps.ClearBit(actual, offset);
                Assert.Equal(n, actual);

                actual = BitOps.WriteBit(actual, offset, !on);
                Assert.Equal(n, actual);
            }

            // Ref
            actual = n;
            Assert.Equal(was, on ? BitOps.InsertBit(ref actual, offset) : BitOps.ClearBit(ref actual, offset));
            Assert.Equal(expected, actual);

            actual = n;
            Assert.Equal(was, BitOps.WriteBit(ref actual, offset, on));
            Assert.Equal(expected, actual);

            if (actual != n)
            {
                Assert.Equal(!was, on ? BitOps.ClearBit(ref actual, offset) : BitOps.InsertBit(ref actual, offset));
                Assert.Equal(n, actual);

                Assert.Equal(was, BitOps.WriteBit(ref actual, offset, on));
                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(0b000, 0, false, false, 0b000)] // 0
        [InlineData(0b000, 0, true, false, 0b001)]
        [InlineData(0b000, 1, false, false, 0b000)]
        [InlineData(0b000, 1, true, false, 0b010)]
        [InlineData(0b001, 0, false, true, 0b000)] // 1
        [InlineData(0b001, 0, true, true, 0b001)]
        [InlineData(0b001, 1, false, false, 0b001)]
        [InlineData(0b001, 1, true, false, 0b011)]
        [InlineData(0b010, 0, false, false, 0b010)] // 2
        [InlineData(0b010, 0, true, false, 0b011)]
        [InlineData(0b010, 1, false, true, 0b000)]
        [InlineData(0b010, 1, true, true, 0b010)]
        [InlineData(0b011, 0, false, true, 0b010)] // 3
        [InlineData(0b011, 0, true, true, 0b011)]
        [InlineData(0b011, 1, false, true, 0b001)]
        [InlineData(0b011, 1, true, true, 0b011)]
        [InlineData(0b001, 0 - 32 * 2, false, true, 0b000)] // == 1
        [InlineData(0b001, 0 - 32 * 2, true, true, 0b001)]
        [InlineData(0b001, 1 - 32 * 2, false, false, 0b001)]
        [InlineData(0b001, 1 - 32 * 2, true, false, 0b011)]
        [InlineData(1, int.MinValue, false, true, 0)] // % 32 = 0
        //[InlineData(1 << 31, int.MaxValue, false, true, 0)] // % 32 = 31 TODO
        [InlineData(byte.MaxValue, 0, false, true, byte.MaxValue - 1)]
        [InlineData(byte.MaxValue, 0, true, true, byte.MaxValue)]
        [InlineData(byte.MaxValue, 7, false, true, byte.MaxValue >> 1)]
        [InlineData(byte.MaxValue, 7, true, true, byte.MaxValue)]
        [InlineData(byte.MaxValue, 8, false, false, byte.MaxValue)]
        [InlineData(byte.MaxValue, 8, true, false, byte.MaxValue + (1u << 8))]
        [InlineData(ushort.MaxValue, 0, false, true, ushort.MaxValue - 1)]
        [InlineData(ushort.MaxValue, 0, true, true, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 15, false, true, ushort.MaxValue >> 1)]
        [InlineData(ushort.MaxValue, 15, true, true, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 16, false, false, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 16, true, false, ushort.MaxValue + (1u << 16))]
        [InlineData(uint.MaxValue, 0, false, true, uint.MaxValue - 1)]
        [InlineData(uint.MaxValue, 0, true, true, uint.MaxValue)]
        [InlineData(uint.MaxValue, 31, false, true, uint.MaxValue >> 1)]
        [InlineData(uint.MaxValue, 31, true, true, uint.MaxValue)]
        [InlineData(uint.MaxValue, 32, false, true, uint.MaxValue - 1)]
        public static void BitOps_WriteBit_uint(uint n, int offset, bool on, bool was, uint expected)
        {
            // Scalar
            uint actual = on ? BitOps.InsertBit(n, offset) : BitOps.ClearBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.WriteBit(n, offset, on);
            Assert.Equal(expected, actual);

            if (actual != n)
            {
                actual = !on ? BitOps.InsertBit(actual, offset) : BitOps.ClearBit(actual, offset);
                Assert.Equal(n, actual);

                actual = BitOps.WriteBit(actual, offset, !on);
                Assert.Equal(n, actual);
            }

            // Ref
            actual = n;
            Assert.Equal(was, on ? BitOps.InsertBit(ref actual, offset) : BitOps.ClearBit(ref actual, offset));
            Assert.Equal(expected, actual);

            actual = n;
            Assert.Equal(was, BitOps.WriteBit(ref actual, offset, on));
            Assert.Equal(expected, actual);

            if (actual != n)
            {
                Assert.Equal(!was, on ? BitOps.ClearBit(ref actual, offset) : BitOps.InsertBit(ref actual, offset));
                Assert.Equal(n, actual);

                Assert.Equal(was, BitOps.WriteBit(ref actual, offset, on));
                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(0b000, 0, false, false, 0b000)] // 0
        [InlineData(0b000, 0, true, false, 0b001)]
        [InlineData(0b000, 1, false, false, 0b000)]
        [InlineData(0b000, 1, true, false, 0b010)]
        [InlineData(0b001, 0, false, true, 0b000)] // 1
        [InlineData(0b001, 0, true, true, 0b001)]
        [InlineData(0b001, 1, false, false, 0b001)]
        [InlineData(0b001, 1, true, false, 0b011)]
        [InlineData(0b010, 0, false, false, 0b010)] // 2
        [InlineData(0b010, 0, true, false, 0b011)]
        [InlineData(0b010, 1, false, true, 0b000)]
        [InlineData(0b010, 1, true, true, 0b010)]
        [InlineData(0b011, 0, false, true, 0b010)] // 3
        [InlineData(0b011, 0, true, true, 0b011)]
        [InlineData(0b011, 1, false, true, 0b001)]
        [InlineData(0b011, 1, true, true, 0b011)]
        [InlineData(0b001, 0 - 64 * 2, false, true, 0b000)] // == 1
        [InlineData(0b001, 0 - 64 * 2, true, true, 0b001)]
        [InlineData(0b001, 1 - 64 * 2, false, false, 0b001)]
        [InlineData(0b001, 1 - 64 * 2, true, false, 0b011)]
        [InlineData(1, int.MinValue, false, true, 0)] // % 64 = 0
        //[InlineData(1 << 63, int.MaxValue, false, true, 0)] // % 64 = 63 TODO
        [InlineData(byte.MaxValue, 0, false, true, byte.MaxValue - 1)]
        [InlineData(byte.MaxValue, 0, true, true, byte.MaxValue)]
        [InlineData(byte.MaxValue, 7, false, true, byte.MaxValue >> 1)]
        [InlineData(byte.MaxValue, 7, true, true, byte.MaxValue)]
        [InlineData(byte.MaxValue, 8, false, false, byte.MaxValue)]
        [InlineData(byte.MaxValue, 8, true, false, byte.MaxValue + (1u << 8))]
        [InlineData(ushort.MaxValue, 0, false, true, ushort.MaxValue - 1)]
        [InlineData(ushort.MaxValue, 0, true, true, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 15, false, true, ushort.MaxValue >> 1)]
        [InlineData(ushort.MaxValue, 15, true, true, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 16, false, false, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, 16, true, false, ushort.MaxValue + (1u << 16))]
        [InlineData(uint.MaxValue, 0, false, true, uint.MaxValue - 1)]
        [InlineData(uint.MaxValue, 0, true, true, uint.MaxValue)]
        [InlineData(uint.MaxValue, 31, false, true, uint.MaxValue >> 1)]
        [InlineData(uint.MaxValue, 31, true, true, uint.MaxValue)]
        [InlineData(ulong.MaxValue, 62, false, true, ulong.MaxValue >> 2 | 1ul << 63)]
        [InlineData(ulong.MaxValue, 62, true, true, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, 63, false, true, ulong.MaxValue >> 1)]
        [InlineData(ulong.MaxValue, 63, true, true, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, 64, false, true, ulong.MaxValue - 1)]
        public static void BitOps_WriteBit_ulong(ulong n, int offset, bool on, bool was, ulong expected)
        {
            // Scalar
            ulong actual = on ? BitOps.InsertBit(n, offset) : BitOps.ClearBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.WriteBit(n, offset, on);
            Assert.Equal(expected, actual);

            if (actual != n)
            {
                actual = !on ? BitOps.InsertBit(actual, offset) : BitOps.ClearBit(actual, offset);
                Assert.Equal(n, actual);

                actual = BitOps.WriteBit(actual, offset, !on);
                Assert.Equal(n, actual);
            }

            // Ref
            actual = n;
            Assert.Equal(was, on ? BitOps.InsertBit(ref actual, offset) : BitOps.ClearBit(ref actual, offset));
            Assert.Equal(expected, actual);

            actual = n;
            Assert.Equal(was, BitOps.WriteBit(ref actual, offset, on));
            Assert.Equal(expected, actual);

            if (actual != n)
            {
                Assert.Equal(!was, !on ? BitOps.InsertBit(ref actual, offset) : BitOps.ClearBit(ref actual, offset));
                Assert.Equal(n, actual);

                Assert.Equal(was, BitOps.WriteBit(ref actual, offset, on));
                Assert.Equal(expected, actual);
            }
        }

        #endregion

        #region ComplementBit

        [Theory]
        [InlineData(0b000, 0, 0b001, false)]
        [InlineData(0b001, 0, 0b000, true)]
        [InlineData(0b000, 1, 0b010, false)]
        [InlineData(0b010, 1, 0b000, true)]
        [InlineData(1, int.MinValue, 0, true)] // % 8 = 0
        [InlineData(1 << 7, int.MaxValue, 0, true)] // % 8 = 7
        [InlineData(byte.MaxValue, 0, byte.MaxValue - 1, true)]
        [InlineData(byte.MaxValue, 7, byte.MaxValue >> 1, true)]
        [InlineData(byte.MaxValue, 8, byte.MaxValue - 1, true)]
        public static void BitOps_ComplementBit_byte(byte n, int offset, uint expected, bool was)
        {
            // Scalar
            byte actual = BitOps.ComplementBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ComplementBit(actual, offset);
            Assert.Equal(n, actual);

            actual = BitOps.ComplementBit(actual, offset + 8 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ComplementBit(actual, offset - 8 * 2);
            Assert.Equal(n, actual);

            // Ref
            actual = n;

            Assert.Equal(BitOps.ComplementBit(ref actual, offset), was);
            Assert.Equal(expected, actual);

            Assert.Equal(BitOps.ComplementBit(ref actual, offset), !was);
            Assert.Equal(n, actual);
        }

        [Theory]
        [InlineData(0b000, 0, 0b001, false)]
        [InlineData(0b001, 0, 0b000, true)]
        [InlineData(0b000, 1, 0b010, false)]
        [InlineData(0b010, 1, 0b000, true)]
        [InlineData(1, int.MinValue, 0, true)] // % 16 = 0
        [InlineData(1 << 15, int.MaxValue, 0, true)] // % 16 = 15
        [InlineData(byte.MaxValue, 0, byte.MaxValue - 1, true)]
        [InlineData(byte.MaxValue, 7, byte.MaxValue >> 1, true)]
        [InlineData(byte.MaxValue, 8, byte.MaxValue + (1u << 8), false)]
        [InlineData(ushort.MaxValue, 0, ushort.MaxValue - 1, true)]
        [InlineData(ushort.MaxValue, 15, ushort.MaxValue >> 1, true)]
        [InlineData(ushort.MaxValue, 16, ushort.MaxValue - 1, true)]
        public static void BitOps_ComplementBit_ushort(ushort n, int offset, uint expected, bool was)
        {
            // Scalar
            ushort actual = BitOps.ComplementBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ComplementBit(actual, offset);
            Assert.Equal(n, actual);

            actual = BitOps.ComplementBit(actual, offset + 16 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ComplementBit(actual, offset - 16 * 2);
            Assert.Equal(n, actual);

            // Ref
            actual = n;

            Assert.Equal(BitOps.ComplementBit(ref actual, offset), was);
            Assert.Equal(expected, actual);

            Assert.Equal(BitOps.ComplementBit(ref actual, offset), !was);
            Assert.Equal(n, actual);
        }

        [Theory]
        [InlineData(0b000, 0, 0b001, false)]
        [InlineData(0b001, 0, 0b000, true)]
        [InlineData(0b000, 1, 0b010, false)]
        [InlineData(0b010, 1, 0b000, true)]
        [InlineData(1, int.MinValue, 0, true)] // % 32 = 0
        [InlineData(unchecked((uint)(1 << 31)), int.MaxValue, 0, true)] // % 32 = 31
        [InlineData(byte.MaxValue, 0, byte.MaxValue - 1, true)]
        [InlineData(byte.MaxValue, 7, byte.MaxValue >> 1, true)]
        [InlineData(byte.MaxValue, 8, byte.MaxValue + (1u << 8), false)]
        [InlineData(ushort.MaxValue, 0, ushort.MaxValue - 1, true)]
        [InlineData(ushort.MaxValue, 15, ushort.MaxValue >> 1, true)]
        [InlineData(ushort.MaxValue, 16, ushort.MaxValue + (1u << 16), false)]
        [InlineData(uint.MaxValue, 0, uint.MaxValue - 1, true)]
        [InlineData(uint.MaxValue, 31, uint.MaxValue >> 1, true)]
        [InlineData(uint.MaxValue, 32, uint.MaxValue - 1, true)]
        public static void BitOps_ComplementBit_uint(uint n, int offset, uint expected, bool was)
        {
            // Scalar
            uint actual = BitOps.ComplementBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ComplementBit(actual, offset);
            Assert.Equal(n, actual);

            actual = BitOps.ComplementBit(actual, offset + 32 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ComplementBit(actual, offset - 32 * 2);
            Assert.Equal(n, actual);

            // Ref
            actual = n;

            Assert.Equal(BitOps.ComplementBit(ref actual, offset), was);
            Assert.Equal(expected, actual);

            Assert.Equal(BitOps.ComplementBit(ref actual, offset), !was);
            Assert.Equal(n, actual);
        }

        [Theory]
        [InlineData(0b000, 0, 0b001, false)]
        [InlineData(0b001, 0, 0b000, true)]
        [InlineData(0b000, 1, 0b010, false)]
        [InlineData(0b010, 1, 0b000, true)]
        [InlineData(1, int.MinValue, 0, true)] // % 64 = 0
        //[InlineData(unchecked((ulong)(1 << 63)), int.MaxValue, 0, true)] // % 64 = 63 // TODO
        [InlineData(byte.MaxValue, 0, byte.MaxValue - 1, true)]
        [InlineData(byte.MaxValue, 7, byte.MaxValue >> 1, true)]
        [InlineData(byte.MaxValue, 8, byte.MaxValue + (1ul << 8), false)]
        [InlineData(ushort.MaxValue, 0, ushort.MaxValue - 1, true)]
        [InlineData(ushort.MaxValue, 15, ushort.MaxValue >> 1, true)]
        [InlineData(ushort.MaxValue, 16, ushort.MaxValue + (1ul << 16), false)]
        [InlineData(uint.MaxValue, 0, uint.MaxValue - 1, true)]
        [InlineData(uint.MaxValue, 31, uint.MaxValue >> 1, true)]
        [InlineData(uint.MaxValue, 32, uint.MaxValue + (1ul << 32), false)]
        [InlineData(ulong.MaxValue, 0, ulong.MaxValue - 1, true)]
        [InlineData(ulong.MaxValue, 63, ulong.MaxValue >> 1, true)]
        [InlineData(ulong.MaxValue, 64, ulong.MaxValue - 1, true)]
        public static void BitOps_ComplementBit_ulong(ulong n, int offset, ulong expected, bool was)
        {
            // Scalar
            ulong actual = BitOps.ComplementBit(n, offset);
            Assert.Equal(expected, actual);

            actual = BitOps.ComplementBit(actual, offset);
            Assert.Equal(n, actual);

            actual = BitOps.ComplementBit(actual, offset + 64 * 2);
            Assert.Equal(expected, actual);

            actual = BitOps.ComplementBit(actual, offset - 64 * 2);
            Assert.Equal(n, actual);

            // Ref
            actual = n;

            Assert.Equal(BitOps.ComplementBit(ref actual, offset), was);
            Assert.Equal(expected, actual);

            Assert.Equal(BitOps.ComplementBit(ref actual, offset), !was);
            Assert.Equal(n, actual);
        }

        #endregion

        #region TrailingZeroCount

        [Theory]
        [InlineData(0u, 32)]
        [InlineData(0b1u, 0)]
        [InlineData(0b10u, 1)]
        [InlineData(0b100u, 2)]
        [InlineData(0b1000u, 3)]
        [InlineData(0b10000u, 4)]
        [InlineData(0b100000u, 5)]
        [InlineData(0b1000000u, 6)]
        [InlineData((uint)byte.MaxValue << 24, 24)]
        [InlineData((uint)byte.MaxValue << 22, 22)]
        [InlineData((uint)ushort.MaxValue << 16, 16)]
        [InlineData((uint)ushort.MaxValue << 19, 19)]
        [InlineData(uint.MaxValue << 5, 5)]
        [InlineData(3u << 27, 27)]
        [InlineData(uint.MaxValue, 0)]
        public static void BitOps_TrailingZeroCount_uint(uint n, int expected)
        {
            int actual = BitOps.TrailingZeroCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 32)]
        [InlineData(0b1, 0)]
        [InlineData(0b10, 1)]
        [InlineData(0b100, 2)]
        [InlineData(0b1000, 3)]
        [InlineData(0b10000, 4)]
        [InlineData(0b100000, 5)]
        [InlineData(0b1000000, 6)]
        [InlineData(byte.MaxValue << 24, 24)]
        [InlineData(byte.MaxValue << 22, 22)]
        [InlineData(ushort.MaxValue << 16, 16)]
        [InlineData(ushort.MaxValue << 19, 19)]
        [InlineData(int.MaxValue << 5, 5)]
        [InlineData(3 << 27, 27)]
        [InlineData(int.MaxValue, 0)]
        public static void BitOps_TrailingZeroCount_int(int n, int expected)
        {
            int actual = BitOps.TrailingZeroCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0ul, 64)]
        [InlineData(0b1ul, 0)]
        [InlineData(0b10ul, 1)]
        [InlineData(0b100ul, 2)]
        [InlineData(0b1000ul, 3)]
        [InlineData(0b10000ul, 4)]
        [InlineData(0b100000ul, 5)]
        [InlineData(0b1000000ul, 6)]
        [InlineData((ulong)byte.MaxValue << 40, 40)]
        [InlineData((ulong)byte.MaxValue << 57, 57)]
        [InlineData((ulong)ushort.MaxValue << 31, 31)]
        [InlineData((ulong)ushort.MaxValue << 15, 15)]
        [InlineData(ulong.MaxValue << 5, 5)]
        [InlineData(3ul << 59, 59)]
        [InlineData(5ul << 63, 63)]
        [InlineData(ulong.MaxValue, 0)]
        public static void BitOps_TrailingZeroCount_ulong(ulong n, int expected)
        {
            int actual = BitOps.TrailingZeroCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0L, 64)]
        [InlineData(0b1L, 0)]
        [InlineData(0b10L, 1)]
        [InlineData(0b100L, 2)]
        [InlineData(0b1000L, 3)]
        [InlineData(0b10000L, 4)]
        [InlineData(0b100000L, 5)]
        [InlineData(0b1000000L, 6)]
        [InlineData((long)byte.MaxValue << 40, 40)]
        [InlineData((long)byte.MaxValue << 57, 57)]
        [InlineData((long)ushort.MaxValue << 31, 31)]
        [InlineData((long)ushort.MaxValue << 15, 15)]
        [InlineData(long.MaxValue << 5, 5)]
        [InlineData(3L << 59, 59)]
        [InlineData(5L << 63, 63)]
        [InlineData(long.MaxValue, 0)]
        public static void BitOps_TrailingZeroCount_long(long n, int expected)
        {
            int actual = BitOps.TrailingZeroCount(n);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region LeadingZeroCount

        [Theory]
        [InlineData(0u, 32)]
        [InlineData(0b1u, 31)]
        [InlineData(0b10u, 30)]
        [InlineData(0b100u, 29)]
        [InlineData(0b1000u, 28)]
        [InlineData(0b10000u, 27)]
        [InlineData(0b100000u, 26)]
        [InlineData(0b1000000u, 25)]
        [InlineData(byte.MaxValue << 17, 32 - 8 - 17)]
        [InlineData(byte.MaxValue << 9, 32 - 8 - 9)]
        [InlineData(ushort.MaxValue << 11, 32 - 16 - 11)]
        [InlineData(ushort.MaxValue << 2, 32 - 16 - 2)]
        [InlineData(5 << 7, 32 - 3 - 7)]
        [InlineData(uint.MaxValue, 0)]
        public static void BitOps_LeadingZeroCount_uint(uint n, int expected)
        {
            int actual = BitOps.LeadingZeroCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 32)]
        [InlineData(0b1, 31)]
        [InlineData(0b10, 30)]
        [InlineData(0b100, 29)]
        [InlineData(0b1000, 28)]
        [InlineData(0b10000, 27)]
        [InlineData(0b100000, 26)]
        [InlineData(0b1000000, 25)]
        [InlineData(byte.MaxValue << 17, 32 - 8 - 17)]
        [InlineData(byte.MaxValue << 9, 32 - 8 - 9)]
        [InlineData(ushort.MaxValue << 11, 32 - 16 - 11)]
        [InlineData(ushort.MaxValue << 2, 32 - 16 - 2)]
        [InlineData(5 << 7, 32 - 3 - 7)]
        [InlineData(int.MinValue, 0)]
        [InlineData(int.MaxValue, 1)]
        public static void BitOps_LeadingZeroCount_int(int n, int expected)
        {
            int actual = BitOps.LeadingZeroCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0ul, 64)]
        [InlineData(0b1ul, 63)]
        [InlineData(0b10ul, 62)]
        [InlineData(0b100ul, 61)]
        [InlineData(0b1000ul, 60)]
        [InlineData(0b10000ul, 59)]
        [InlineData(0b100000ul, 58)]
        [InlineData(0b1000000ul, 57)]
        [InlineData((ulong)byte.MaxValue << 41, 64 - 8 - 41)]
        [InlineData((ulong)byte.MaxValue << 53, 64 - 8 - 53)]
        [InlineData((ulong)ushort.MaxValue << 31, 64 - 16 - 31)]
        [InlineData((ulong)ushort.MaxValue << 15, 64 - 16 - 15)]
        [InlineData(ulong.MaxValue >> 5, 5)]
        [InlineData(1ul << 63, 0)]
        [InlineData(1ul << 62, 1)]
        [InlineData(ulong.MaxValue, 0)]
        public static void BitOps_LeadingZeroCount_ulong(ulong n, int expected)
        {
            int actual = BitOps.LeadingZeroCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0L, 64)]
        [InlineData(0b1L, 63)]
        [InlineData(0b10L, 62)]
        [InlineData(0b100L, 61)]
        [InlineData(0b1000L, 60)]
        [InlineData(0b10000L, 59)]
        [InlineData(0b100000L, 58)]
        [InlineData(0b1000000L, 57)]
        [InlineData((long)byte.MaxValue << 41, 64 - 8 - 41)]
        [InlineData((long)byte.MaxValue << 53, 64 - 8 - 53)]
        [InlineData((long)ushort.MaxValue << 31, 64 - 16 - 31)]
        [InlineData((long)ushort.MaxValue << 15, 64 - 16 - 15)]
        [InlineData(1L << 62, 1)]
        [InlineData(long.MinValue, 0)]
        [InlineData(long.MaxValue, 1)]
        public static void BitOps_LeadingZeroCount_long(long n, int expected)
        {
            int actual = BitOps.LeadingZeroCount(n);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Log2

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2 - 1)]
        [InlineData(4, 2)]
        [InlineData(5, 3 - 1)]
        [InlineData(6, 3 - 1)]
        [InlineData(7, 3 - 1)]
        [InlineData(8, 3)]
        [InlineData(9, 4 - 1)]
        [InlineData(byte.MaxValue, 8 - 1)]
        [InlineData(ushort.MaxValue, 16 - 1)]
        [InlineData(uint.MaxValue, 32 - 1)]
        public static void BitOps_Log2_uint(uint n, int expected)
        {
            int actual = BitOps.Log2(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2 - 1)]
        [InlineData(4, 2)]
        [InlineData(5, 3 - 1)]
        [InlineData(6, 3 - 1)]
        [InlineData(7, 3 - 1)]
        [InlineData(8, 3)]
        [InlineData(9, 4 - 1)]
        [InlineData(byte.MaxValue, 8 - 1)]
        [InlineData(ushort.MaxValue, 16 - 1)]
        [InlineData(uint.MaxValue, 32 - 1)]
        [InlineData(ulong.MaxValue, 64 - 1)]
        public static void BitOps_Log2_ulong(ulong n, int expected)
        {
            int actual = BitOps.Log2(n);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region PopCount

        [Theory]
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
        [InlineData(byte.MinValue, 0)] // 0
        [InlineData(byte.MaxValue >> 1, 7)] // 127
        [InlineData(byte.MaxValue - 1, 7)] // 254
        [InlineData(byte.MaxValue, 8)] // 255
        [InlineData(unchecked((byte)sbyte.MinValue), 1)] // 128
        [InlineData(sbyte.MaxValue, 7)] // 127
        public static void BitOps_PopCount_byte(byte n, int expected)
        {
            int actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
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
        [InlineData(byte.MinValue, 0)] // 0
        [InlineData(unchecked((sbyte)byte.MaxValue), 8)] // 255
        [InlineData(sbyte.MinValue, 1)] // -128
        [InlineData(sbyte.MinValue >> 1, 2)] // -64
        [InlineData(sbyte.MaxValue >> 1, 6)] // 63
        [InlineData(sbyte.MaxValue - 1, 6)] // 126
        [InlineData(sbyte.MaxValue, 7)] // 127
        public static void BitOps_PopCount_sbyte(sbyte n, int expected)
        {
            int actual = BitOps.PopCount((byte)n);
            Assert.Equal(expected, actual);
        }

        [Theory]
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
        [InlineData(byte.MinValue, 0)] // 0
        [InlineData(byte.MaxValue, 8)] // 255
        [InlineData(unchecked((ushort)sbyte.MinValue), 16 - 7)] // 65408
        [InlineData(sbyte.MaxValue, 7)] // 127
        [InlineData(ushort.MaxValue >> 3, 16 - 3)] // 8191
        [InlineData(ushort.MaxValue >> 1, 15)] // 32767
        [InlineData(ushort.MaxValue - 1, 15)] // 65534
        [InlineData(ushort.MaxValue, 16)] // 65535
        public static void BitOps_PopCount_ushort(ushort n, int expected)
        {
            int actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
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
        [InlineData(byte.MinValue, 0)] // 0
        [InlineData(byte.MaxValue, 8)] // 255
        [InlineData(sbyte.MinValue, 9)] // -128
        [InlineData(sbyte.MaxValue, 7)] // 127
        [InlineData(short.MinValue, 1)] // -32768
        [InlineData(short.MinValue >> 1, 2)] // -16384
        [InlineData(short.MaxValue >> 1, 14)] // 16383
        [InlineData(short.MaxValue >> 3, 15 - 3)] // 4095
        [InlineData(short.MaxValue - 1, 14)] // 32766
        [InlineData(short.MaxValue, 15)] // 32767
        public static void BitOps_PopCount_short(short n, int expected)
        {
            int actual = BitOps.PopCount((ushort)n);
            Assert.Equal(expected, actual);
        }

        [Theory]
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
        [InlineData(byte.MinValue, 0)] // 0
        [InlineData(byte.MaxValue, 8)] // 255
        [InlineData(unchecked((uint)sbyte.MinValue), 25)] // 4294967168
        [InlineData(sbyte.MaxValue, 7)] // 127
        [InlineData(ushort.MaxValue >> 3, 16 - 3)] // 8191
        [InlineData(ushort.MaxValue, 16)] // 65535
        [InlineData(unchecked((uint)short.MinValue), 32 - 15)] // 4294934528
        [InlineData(short.MaxValue, 15)] // 32767
        [InlineData(unchecked((uint)int.MinValue), 1)] // 2147483648
        [InlineData(unchecked((uint)int.MaxValue), 31)] // 4294967168
        [InlineData(uint.MaxValue >> 5, 32 - 5)] // 134217727
        [InlineData(uint.MaxValue << 11, 32 - 11)] // 4294965248
        [InlineData(uint.MaxValue, 32)] // 4294967295
        public static void BitOps_PopCount_uint(uint n, int expected)
        {
            int actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
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
        [InlineData(byte.MinValue, 0)] // 0
        [InlineData(byte.MaxValue, 8)] // 255
        [InlineData(sbyte.MinValue, 25)] // -128
        [InlineData(sbyte.MaxValue, 7)] // 127
        [InlineData(ushort.MaxValue >> 3, 16 - 3)] // 8191
        [InlineData(ushort.MaxValue, 16)] // 65535
        [InlineData(short.MinValue, 17)] // -32768
        [InlineData(short.MaxValue, 15)] // 32767
        [InlineData(unchecked((int)uint.MaxValue), 32)] // -1
        [InlineData(uint.MaxValue >> 5, 32 - 5)] // 134217727
        [InlineData(unchecked((int)uint.MaxValue << 11), 32 - 11)] // -2048
        [InlineData(int.MaxValue, 31)] // -1
        public static void BitOps_PopCount_int(int n, int expected)
        {
            int actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
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
        [InlineData(byte.MinValue, 0)] // 0
        [InlineData(byte.MaxValue, 8)] // 255
        [InlineData(unchecked((ulong)sbyte.MinValue), 57)] // 18446744073709551488
        [InlineData(sbyte.MaxValue, 7)] // 127
        [InlineData(ushort.MaxValue, 16)] // 65535
        [InlineData(unchecked((ulong)short.MinValue), 49)] // 18446744073709518848
        [InlineData(short.MaxValue, 15)] // 32767
        [InlineData(unchecked((ulong)int.MinValue), 64 - 31)] // 18446744071562067968
        [InlineData(int.MaxValue, 31)] // 2147483647
        [InlineData(ulong.MaxValue >> 9, 64 - 9)] // 36028797018963967
        [InlineData(ulong.MaxValue << 11, 64 - 11)] // 18446744073709549568
        [InlineData(ulong.MaxValue, 64)]
        public static void BitOps_PopCount_ulong(ulong n, int expected)
        {
            int actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory]
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
        [InlineData(byte.MinValue, 0)] // 0
        [InlineData(byte.MaxValue, 8)] // 255
        [InlineData(sbyte.MinValue, 57)] // -128
        [InlineData(sbyte.MaxValue, 7)] // 127
        [InlineData(ushort.MaxValue, 16)] // 65535
        [InlineData(short.MinValue, 49)] // -32768
        [InlineData(short.MaxValue, 15)] // 32767
        [InlineData(int.MinValue, 33)] // -2147483648
        [InlineData(int.MaxValue, 31)] // 2147483647
        [InlineData(unchecked((long)ulong.MaxValue), 64)] // -1
        [InlineData(long.MinValue, 1)]
        [InlineData(long.MaxValue >> 9, 63 - 9)]
        [InlineData(long.MaxValue << 11, 64 - 11)] // -2048
        [InlineData(long.MaxValue, 63)]
        public static void BitOps_PopCount_long(long n, int expected)
        {
            int actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Rotate

        [Fact]
        public static void BitOps_RotateLeft_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, BitOps.RotateLeft(sut, 1));
            Assert.Equal((byte)0b01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal((byte)0b10101010, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateLeft(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact]
        public static void BitOps_RotateLeft_SByte()
        {
            sbyte sut = 0b01010101;
            Assert.Equal(unchecked((sbyte)0b10101010), BitOps.RotateLeft(sut, 1));
            Assert.Equal(unchecked((sbyte)0b01010101), BitOps.RotateLeft(sut, 2));
            Assert.Equal(unchecked((sbyte)0b10101010), BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateLeft(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact]
        public static void BitOps_RotateLeft_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateLeft(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateLeft(sut, int.MaxValue)); // % 16 = 15
        }

        [Fact]
        public static void BitOps_RotateLeft_Short()
        {
            short sut = 0b01010101_01010101;
            Assert.Equal(unchecked((short)0b10101010_10101010), BitOps.RotateLeft(sut, 1));
            Assert.Equal(unchecked((short)0b01010101_01010101), BitOps.RotateLeft(sut, 2));
            Assert.Equal(unchecked((short)0b10101010_10101010), BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateLeft(sut, int.MaxValue)); // % 16 = 15
        }

        [Fact]
        public static void BitOps_RotateLeft_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101u;
            Assert.Equal(0b10101010_10101010_10101010_10101010u, BitOps.RotateLeft(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101u, BitOps.RotateLeft(sut, 2));
            Assert.Equal(0b10101010_10101010_10101010_10101010u, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 32 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 31), BitOps.RotateLeft(sut, int.MaxValue)); // % 32 = 31
        }

        [Fact]
        public static void BitOps_RotateLeft_Int()
        {
            int sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal(unchecked((int)0b10101010_10101010_10101010_10101010), BitOps.RotateLeft(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal(unchecked((int)0b10101010_10101010_10101010_10101010), BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 32 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 31), BitOps.RotateLeft(sut, int.MaxValue)); // % 32 = 31
        }

        [Fact]
        public static void BitOps_RotateLeft_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101ul;
            Assert.Equal(0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010ul, BitOps.RotateLeft(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101ul, BitOps.RotateLeft(sut, 2));
            Assert.Equal(0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010ul, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 64 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 63), BitOps.RotateLeft(sut, int.MaxValue)); // % 64 = 63
        }

        [Fact]
        public static void BitOps_RotateLeft_Long()
        {
            long sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal(unchecked((long)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010), BitOps.RotateLeft(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal(unchecked((long)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010), BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 64 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 63), BitOps.RotateLeft(sut, int.MaxValue)); // % 64 = 63
        }

        [Fact]
        public static void BitOps_RotateRight_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, BitOps.RotateRight(sut, 1));
            Assert.Equal((byte)0b01010101, BitOps.RotateRight(sut, 2));
            Assert.Equal((byte)0b10101010, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateRight(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact]
        public static void BitOps_RotateRight_SByte()
        {
            sbyte sut = 0b01010101;
            Assert.Equal(unchecked((sbyte)0b10101010), BitOps.RotateRight(sut, 1));
            Assert.Equal(unchecked((sbyte)0b01010101), BitOps.RotateRight(sut, 2));
            Assert.Equal(unchecked((sbyte)0b10101010), BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateRight(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact]
        public static void BitOps_RotateRight_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateRight(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, BitOps.RotateRight(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateRight(sut, int.MaxValue)); // % 16 = 15
        }

        [Fact]
        public static void BitOps_RotateRight_Short()
        {
            short sut = 0b01010101_01010101;
            Assert.Equal(unchecked((short)0b10101010_10101010), BitOps.RotateRight(sut, 1));
            Assert.Equal(unchecked((short)0b01010101_01010101), BitOps.RotateRight(sut, 2));
            Assert.Equal(unchecked((short)0b10101010_10101010), BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateRight(sut, int.MaxValue)); // % 16 = 15
        }

        [Fact]
        public static void BitOps_RotateRight_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101u;
            Assert.Equal(0b10101010_10101010_10101010_10101010u, BitOps.RotateRight(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101u, BitOps.RotateRight(sut, 2));
            Assert.Equal(0b10101010_10101010_10101010_10101010u, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 32 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateRight(sut, int.MaxValue)); // % 32 = 15
        }

        [Fact]
        public static void BitOps_RotateRight_Int()
        {
            int sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal(unchecked((int)0b10101010_10101010_10101010_10101010), BitOps.RotateRight(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101, BitOps.RotateRight(sut, 2));
            Assert.Equal(unchecked((int)0b10101010_10101010_10101010_10101010), BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 32 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateRight(sut, int.MaxValue)); // % 32 = 15
        }

        [Fact]
        public static void BitOps_RotateRight_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101ul;
            Assert.Equal(0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010ul, BitOps.RotateRight(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101ul, BitOps.RotateRight(sut, 2));
            Assert.Equal(0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010ul, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 64 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 63), BitOps.RotateRight(sut, int.MaxValue)); // % 64 = 63
        }

        [Fact]
        public static void BitOps_RotateRight_Long()
        {
            long sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal(unchecked((long)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010), BitOps.RotateRight(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, BitOps.RotateRight(sut, 2));
            Assert.Equal(unchecked((long)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010), BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 64 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 63), BitOps.RotateRight(sut, int.MaxValue)); // % 64 = 63
        }

        #endregion

        #region Normalize

        [Fact]
        public static void BitOps_Normalize()
        {
            // The ECMA 335 CLI specification permits a "true" boolean value to be represented by any nonzero value.
            // https://github.com/dotnet/roslyn/issues/24652
            // https://github.com/dotnet/roslyn/blob/master/docs/compilers/Boolean%20Representation.md

            for (int i = 0; i <= byte.MaxValue; i++)
            {
                bool expectedBool = i != 0;
                int expectedByte = expectedBool ? 1 : 0;

                byte n = (byte)i;
                bool weirdBool = Unsafe.As<byte, bool>(ref n);
                Assert.Equal(!expectedBool, !weirdBool);

                // AsByte

                byte byt = BitOps.AsByte(weirdBool);
                Assert.Equal(n, byt);

                byt = BitOps.AsByte(ref weirdBool);
                Assert.Equal(n, byt);

                // Normalize

                byte normalizedByte = BitOps.Iff(weirdBool);
                Assert.Equal(expectedByte, normalizedByte);

                normalizedByte = BitOps.Iff(ref weirdBool);
                Assert.Equal(expectedByte, normalizedByte);
            }
        }

        #endregion

        #region Iff

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void BitOps_Iff_byte(bool condition)
        {
            // Iff (condition, true)

            Assert.Equal(BitOps.Iff(condition, byte.MinValue), condition ? byte.MinValue : 0);
            Assert.Equal(BitOps.Iff(!condition, byte.MinValue), !condition ? byte.MinValue : 0);

            Assert.Equal(BitOps.Iff(condition, byte.MaxValue), condition ? byte.MaxValue : 0);
            Assert.Equal(BitOps.Iff(!condition, byte.MaxValue), !condition ? byte.MaxValue : 0);

            // Iff (condition, true, false)

            Assert.Equal(BitOps.Iff(condition, byte.MinValue, byte.MaxValue), condition ? byte.MinValue : byte.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, byte.MinValue, byte.MaxValue), !condition ? byte.MinValue : byte.MaxValue);

            Assert.Equal(BitOps.Iff(condition, 123, byte.MaxValue), condition ? 123 : byte.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, 123, byte.MaxValue), !condition ? 123 : byte.MaxValue);

            // Any

            Assert.Equal(0, BitOps.NonZero(byte.MinValue));
            Assert.Equal(1, BitOps.NonZero(byte.MinValue + 1));
            Assert.Equal(1, BitOps.NonZero(byte.MaxValue - 1));
            Assert.Equal(1, BitOps.NonZero(byte.MaxValue));

            // NotAny

            Assert.Equal(1, BitOps.IsZero(byte.MinValue));
            Assert.Equal(0, BitOps.IsZero(byte.MinValue + 1));
            Assert.Equal(0, BitOps.IsZero(byte.MaxValue - 1));
            Assert.Equal(0, BitOps.IsZero(byte.MaxValue));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void BitOps_Iff_sbyte(bool condition)
        {
            // Iff (condition, true)

            Assert.Equal(BitOps.Iff(condition, sbyte.MinValue), condition ? sbyte.MinValue : 0);
            Assert.Equal(BitOps.Iff(!condition, sbyte.MinValue), !condition ? sbyte.MinValue : 0);

            Assert.Equal(BitOps.Iff(condition, sbyte.MaxValue), condition ? sbyte.MaxValue : 0);
            Assert.Equal(BitOps.Iff(!condition, sbyte.MaxValue), !condition ? sbyte.MaxValue : 0);

            // Iff (condition, true, false)

            Assert.Equal(BitOps.Iff(condition, sbyte.MinValue, sbyte.MaxValue), condition ? sbyte.MinValue : sbyte.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, sbyte.MinValue, sbyte.MaxValue), !condition ? sbyte.MinValue : sbyte.MaxValue);

            Assert.Equal(BitOps.Iff(condition, -123, sbyte.MaxValue), condition ? -123 : sbyte.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, -123, sbyte.MaxValue), !condition ? -123 : sbyte.MaxValue);

            // Any

            Assert.Equal(1, BitOps.NonZero(sbyte.MinValue));
            Assert.Equal(1, BitOps.NonZero(sbyte.MinValue + 1));
            Assert.Equal(0, BitOps.NonZero((sbyte)0));
            Assert.Equal(1, BitOps.NonZero(sbyte.MaxValue - 1));
            Assert.Equal(1, BitOps.NonZero(sbyte.MaxValue));

            // NotAny

            Assert.Equal(0, BitOps.IsZero(sbyte.MinValue));
            Assert.Equal(0, BitOps.IsZero(sbyte.MinValue + 1));
            Assert.Equal(1, BitOps.IsZero((sbyte)0));
            Assert.Equal(0, BitOps.IsZero(sbyte.MaxValue - 1));
            Assert.Equal(0, BitOps.IsZero(sbyte.MaxValue));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void BitOps_Iff_ushort(bool condition)
        {
            // Iff (condition, true)

            Assert.Equal(BitOps.Iff(condition, ushort.MinValue), condition ? ushort.MinValue : 0);
            Assert.Equal(BitOps.Iff(!condition, ushort.MinValue), !condition ? ushort.MinValue : 0);

            Assert.Equal(BitOps.Iff(condition, ushort.MaxValue), condition ? ushort.MaxValue : 0);
            Assert.Equal(BitOps.Iff(!condition, ushort.MaxValue), !condition ? ushort.MaxValue : 0);

            // Iff (condition, true, false)

            Assert.Equal(BitOps.Iff(condition, ushort.MinValue, ushort.MaxValue), condition ? ushort.MinValue : ushort.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, ushort.MinValue, ushort.MaxValue), !condition ? ushort.MinValue : ushort.MaxValue);

            Assert.Equal(BitOps.Iff(condition, 123, ushort.MaxValue), condition ? 123 : ushort.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, 123, ushort.MaxValue), !condition ? 123 : ushort.MaxValue);

            // Any

            Assert.Equal(0, BitOps.NonZero(ushort.MinValue));
            Assert.Equal(1, BitOps.NonZero(ushort.MinValue + 1));
            Assert.Equal(1, BitOps.NonZero(ushort.MaxValue - 1));
            Assert.Equal(1, BitOps.NonZero(ushort.MaxValue));

            // NotAny

            Assert.Equal(1, BitOps.IsZero(ushort.MinValue));
            Assert.Equal(0, BitOps.IsZero(ushort.MinValue + 1));
            Assert.Equal(0, BitOps.IsZero(ushort.MaxValue - 1));
            Assert.Equal(0, BitOps.IsZero(ushort.MaxValue));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void BitOps_Iff_short(bool condition)
        {
            // Iff (condition, true)

            Assert.Equal(BitOps.Iff(condition, short.MinValue), condition ? short.MinValue : 0);
            Assert.Equal(BitOps.Iff(!condition, short.MinValue), !condition ? short.MinValue : 0);

            Assert.Equal(BitOps.Iff(condition, short.MaxValue), condition ? short.MaxValue : 0);
            Assert.Equal(BitOps.Iff(!condition, short.MaxValue), !condition ? short.MaxValue : 0);

            // Iff (condition, true, false)

            Assert.Equal(BitOps.Iff(condition, short.MinValue, short.MaxValue), condition ? short.MinValue : short.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, short.MinValue, short.MaxValue), !condition ? short.MinValue : short.MaxValue);

            Assert.Equal(BitOps.Iff(condition, -123, short.MaxValue), condition ? -123 : short.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, -123, short.MaxValue), !condition ? -123 : short.MaxValue);

            // Any

            Assert.Equal(1, BitOps.NonZero(short.MinValue));
            Assert.Equal(1, BitOps.NonZero(short.MinValue + 1));
            Assert.Equal(0, BitOps.NonZero((short)0));
            Assert.Equal(1, BitOps.NonZero(short.MaxValue - 1));
            Assert.Equal(1, BitOps.NonZero(short.MaxValue));

            // NotAny

            Assert.Equal(0, BitOps.IsZero(short.MinValue));
            Assert.Equal(0, BitOps.IsZero(short.MinValue + 1));
            Assert.Equal(1, BitOps.IsZero((short)0));
            Assert.Equal(0, BitOps.IsZero(short.MaxValue - 1));
            Assert.Equal(0, BitOps.IsZero(short.MaxValue));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void BitOps_Iff_uint(bool condition)
        {
            // Iff (condition, true)

            Assert.Equal(BitOps.Iff(condition, uint.MinValue), condition ? uint.MinValue : 0);
            Assert.Equal(BitOps.Iff(!condition, uint.MinValue), !condition ? uint.MinValue : 0);

            Assert.Equal(BitOps.Iff(condition, uint.MaxValue), condition ? uint.MaxValue : 0);
            Assert.Equal(BitOps.Iff(!condition, uint.MaxValue), !condition ? uint.MaxValue : 0);

            // Iff (condition, true, false)

            Assert.Equal(BitOps.Iff(condition, uint.MinValue, uint.MaxValue), condition ? uint.MinValue : uint.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, uint.MinValue, uint.MaxValue), !condition ? uint.MinValue : uint.MaxValue);

            Assert.Equal(BitOps.Iff(condition, 123, uint.MaxValue), condition ? 123 : uint.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, 123, uint.MaxValue), !condition ? 123 : uint.MaxValue);

            // Any

            Assert.Equal(0, BitOps.NonZero(uint.MinValue));
            Assert.Equal(1, BitOps.NonZero(uint.MinValue + 1));
            Assert.Equal(1, BitOps.NonZero(uint.MaxValue - 1));
            Assert.Equal(1, BitOps.NonZero(uint.MaxValue));

            // NotAny

            Assert.Equal(1, BitOps.IsZero(uint.MinValue));
            Assert.Equal(0, BitOps.IsZero(uint.MinValue + 1));
            Assert.Equal(0, BitOps.IsZero(uint.MaxValue - 1));
            Assert.Equal(0, BitOps.IsZero(uint.MaxValue));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void BitOps_Iff_int(bool condition)
        {
            // Iff (condition, true)

            Assert.Equal(BitOps.Iff(condition, int.MinValue), condition ? int.MinValue : 0);
            Assert.Equal(BitOps.Iff(!condition, int.MinValue), !condition ? int.MinValue : 0);

            Assert.Equal(BitOps.Iff(condition, int.MaxValue), condition ? int.MaxValue : 0);
            Assert.Equal(BitOps.Iff(!condition, int.MaxValue), !condition ? int.MaxValue : 0);

            // Iff (condition, true, false)

            Assert.Equal(BitOps.Iff(condition, int.MinValue, int.MaxValue), condition ? int.MinValue : int.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, int.MinValue, int.MaxValue), !condition ? int.MinValue : int.MaxValue);

            Assert.Equal(BitOps.Iff(condition, -123, int.MaxValue), condition ? -123 : int.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, -123, int.MaxValue), !condition ? -123 : int.MaxValue);

            // Any

            Assert.Equal(1, BitOps.NonZero(int.MinValue));
            Assert.Equal(1, BitOps.NonZero(int.MinValue + 1));
            Assert.Equal(0, BitOps.NonZero(0));
            Assert.Equal(1, BitOps.NonZero(int.MaxValue - 1));
            Assert.Equal(1, BitOps.NonZero(int.MaxValue));

            // NotAny

            Assert.Equal(0, BitOps.IsZero(int.MinValue));
            Assert.Equal(0, BitOps.IsZero(int.MinValue + 1));
            Assert.Equal(1, BitOps.IsZero(0));
            Assert.Equal(0, BitOps.IsZero(int.MaxValue - 1));
            Assert.Equal(0, BitOps.IsZero(int.MaxValue));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void BitOps_Iff_ulong(bool condition)
        {
            // Iff (condition, true)

            Assert.Equal(BitOps.Iff(condition, ulong.MinValue), condition ? ulong.MinValue : 0);
            Assert.Equal(BitOps.Iff(!condition, ulong.MinValue), !condition ? ulong.MinValue : 0);

            Assert.Equal(BitOps.Iff(condition, ulong.MaxValue), condition ? ulong.MaxValue : 0);
            Assert.Equal(BitOps.Iff(!condition, ulong.MaxValue), !condition ? ulong.MaxValue : 0);

            // Iff (condition, true, false)

            Assert.Equal(BitOps.Iff(condition, ulong.MinValue, ulong.MaxValue), condition ? ulong.MinValue : ulong.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, ulong.MinValue, ulong.MaxValue), !condition ? ulong.MinValue : ulong.MaxValue);

            Assert.Equal(BitOps.Iff(condition, 123, ulong.MaxValue), condition ? 123 : ulong.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, 123, ulong.MaxValue), !condition ? 123 : ulong.MaxValue);

            // Any

            Assert.Equal(0, BitOps.NonZero(ulong.MinValue));
            Assert.Equal(1, BitOps.NonZero(ulong.MinValue + 1));
            Assert.Equal(1, BitOps.NonZero(ulong.MaxValue - 1));
            Assert.Equal(1, BitOps.NonZero(ulong.MaxValue));

            // NotAny

            Assert.Equal(1, BitOps.IsZero(ulong.MinValue));
            Assert.Equal(0, BitOps.IsZero(ulong.MinValue + 1));
            Assert.Equal(0, BitOps.IsZero(ulong.MaxValue - 1));
            Assert.Equal(0, BitOps.IsZero(ulong.MaxValue));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void BitOps_Iff_long(bool condition)
        {
            // Iff (condition, true)

            Assert.Equal(BitOps.Iff(condition, long.MinValue), condition ? long.MinValue : 0);
            Assert.Equal(BitOps.Iff(!condition, long.MinValue), !condition ? long.MinValue : 0);

            Assert.Equal(BitOps.Iff(condition, long.MaxValue), condition ? long.MaxValue : 0);
            Assert.Equal(BitOps.Iff(!condition, long.MaxValue), !condition ? long.MaxValue : 0);

            // Iff (condition, true, false)

            Assert.Equal(BitOps.Iff(condition, long.MinValue, long.MaxValue), condition ? long.MinValue : long.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, long.MinValue, long.MaxValue), !condition ? long.MinValue : long.MaxValue);

            Assert.Equal(BitOps.Iff(condition, -123, long.MaxValue), condition ? -123 : long.MaxValue);
            Assert.Equal(BitOps.Iff(!condition, -123, long.MaxValue), !condition ? -123 : long.MaxValue);

            // Any

            Assert.Equal(1, BitOps.NonZero(long.MinValue));
            Assert.Equal(1, BitOps.NonZero(long.MinValue + 1));
            Assert.Equal(0, BitOps.NonZero(0L));
            Assert.Equal(1, BitOps.NonZero(long.MaxValue - 1));
            Assert.Equal(1, BitOps.NonZero(long.MaxValue));

            // NotAny

            Assert.Equal(0, BitOps.IsZero(long.MinValue));
            Assert.Equal(0, BitOps.IsZero(long.MinValue + 1));
            Assert.Equal(1, BitOps.IsZero(0L));
            Assert.Equal(0, BitOps.IsZero(long.MaxValue - 1));
            Assert.Equal(0, BitOps.IsZero(long.MaxValue));
        }

        #endregion

        #region Helpers

        private static byte Reverse(byte value)
        {
            byte result = 0;

            for (int i = 0; i < 8; i++)
            {
                if ((value & (1u << i)) != 0)
                    result |= (byte)(1u << (7 - i));
            }

            return result;
        }

        private static sbyte Reverse(sbyte value)
            => unchecked((sbyte)Reverse((byte)value));

        private static ushort Reverse(ushort value)
        {
            ushort result = 0;

            for (int i = 0; i < 16; i++)
            {
                if ((value & (1u << i)) != 0)
                    result |= (ushort)(1u << (15 - i));
            }

            return result;
        }

        private static short Reverse(short value)
            => unchecked((short)Reverse((ushort)value));

        private static uint Reverse(uint value)
        {
            uint result = 0u;

            for (int i = 0; i < 32; i++)
            {
                if ((value & (1u << i)) != 0)
                    result |= 1u << (31 - i);
            }

            return result;
        }

        private static int Reverse(int value)
            => unchecked((int)Reverse((uint)value));

        private static ulong Reverse(ulong value)
        {
            ulong result = 0ul;

            for (int i = 0; i < 64; i++)
            {
                if ((value & (1ul << i)) != 0)
                    result |= 1ul << (63 - i);
            }

            return result;
        }

        private static long Reverse(long value)
            => unchecked((long)Reverse((ulong)value));

        #endregion
    }
}

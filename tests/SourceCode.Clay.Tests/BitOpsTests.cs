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

        [Fact(DisplayName = nameof(BitOps_Samples))]
        public static void BitOps_Samples()
        {
            // PopCount: Returns the population count (number of bits set) of a mask.
            Assert.Equal(4u, BitOps.PopCount(0x1001_0110u));

            // RotateLeft/Right: Rotates the specified value left/right by the specified number of bits.
            Assert.Equal(0b0100_0001u, BitOps.RotateLeft((byte)0b0010_1000u, 3));
            Assert.Equal(0b1000_0000u, BitOps.RotateRight((byte)0b0010_0000u, 6));

            // Leading/TrailingZeros: Count the number of leading/trailing zero bits in a mask.
            Assert.Equal(2u, BitOps.LeadingZeroCount((byte)0b0011_1000));
            Assert.Equal(5u, BitOps.TrailingZeroCount(0b1110_0000));

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

        [Theory(DisplayName = nameof(BitOps_ExtractBit_byte))]
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

        [Theory(DisplayName = nameof(BitOps_ExtractBit_sbyte))]
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

        [Theory(DisplayName = nameof(BitOps_ExtractBit_ushort))]
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

        [Theory(DisplayName = nameof(BitOps_ExtractBit_short))]
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

        [Theory(DisplayName = nameof(BitOps_ExtractBit_uint))]
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

        [Theory(DisplayName = nameof(BitOps_ExtractBit_int))]
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

        [Theory(DisplayName = nameof(BitOps_ExtractBit_ulong))]
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

        [Theory(DisplayName = nameof(BitOps_ExtractBit_long))]
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

        [Theory(DisplayName = nameof(BitOps_WriteBit_byte))]
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

        [Theory(DisplayName = nameof(BitOps_WriteBit_ushort))]
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

        [Theory(DisplayName = nameof(BitOps_WriteBit_uint))]
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

        [Theory(DisplayName = nameof(BitOps_WriteBit_ulong))]
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

        [Theory(DisplayName = nameof(BitOps_ComplementBit_byte))]
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

        [Theory(DisplayName = nameof(BitOps_ComplementBit_ushort))]
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

        [Theory(DisplayName = nameof(BitOps_ComplementBit_uint))]
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

        [Theory(DisplayName = nameof(BitOps_ComplementBit_ulong))]
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

        #region Rotate

        [Fact(DisplayName = nameof(BitOps_RotateLeft_Byte))]
        public static void BitOps_RotateLeft_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, BitOps.RotateLeft(sut, 1));
            Assert.Equal((byte)0b01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal((byte)0b10101010, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateLeft(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact(DisplayName = nameof(BitOps_RotateLeft_SByte))]
        public static void BitOps_RotateLeft_SByte()
        {
            sbyte sut = 0b01010101;
            Assert.Equal(unchecked((sbyte)0b10101010), BitOps.RotateLeft(sut, 1));
            Assert.Equal(unchecked((sbyte)0b01010101), BitOps.RotateLeft(sut, 2));
            Assert.Equal(unchecked((sbyte)0b10101010), BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateLeft(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact(DisplayName = nameof(BitOps_RotateLeft_UShort))]
        public static void BitOps_RotateLeft_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateLeft(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateLeft(sut, int.MaxValue)); // % 16 = 15
        }

        [Fact(DisplayName = nameof(BitOps_RotateLeft_Short))]
        public static void BitOps_RotateLeft_Short()
        {
            short sut = 0b01010101_01010101;
            Assert.Equal(unchecked((short)0b10101010_10101010), BitOps.RotateLeft(sut, 1));
            Assert.Equal(unchecked((short)0b01010101_01010101), BitOps.RotateLeft(sut, 2));
            Assert.Equal(unchecked((short)0b10101010_10101010), BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateLeft(sut, int.MaxValue)); // % 16 = 15
        }

        [Fact(DisplayName = nameof(BitOps_RotateLeft_UInt))]
        public static void BitOps_RotateLeft_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101u;
            Assert.Equal(0b10101010_10101010_10101010_10101010u, BitOps.RotateLeft(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101u, BitOps.RotateLeft(sut, 2));
            Assert.Equal(0b10101010_10101010_10101010_10101010u, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 32 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 31), BitOps.RotateLeft(sut, int.MaxValue)); // % 32 = 31
        }

        [Fact(DisplayName = nameof(BitOps_RotateLeft_Int))]
        public static void BitOps_RotateLeft_Int()
        {
            int sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal(unchecked((int)0b10101010_10101010_10101010_10101010), BitOps.RotateLeft(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal(unchecked((int)0b10101010_10101010_10101010_10101010), BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 32 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 31), BitOps.RotateLeft(sut, int.MaxValue)); // % 32 = 31
        }

        [Fact(DisplayName = nameof(BitOps_RotateLeft_ULong))]
        public static void BitOps_RotateLeft_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101ul;
            Assert.Equal(0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010ul, BitOps.RotateLeft(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101ul, BitOps.RotateLeft(sut, 2));
            Assert.Equal(0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010ul, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 64 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 63), BitOps.RotateLeft(sut, int.MaxValue)); // % 64 = 63
        }

        [Fact(DisplayName = nameof(BitOps_RotateLeft_Long))]
        public static void BitOps_RotateLeft_Long()
        {
            long sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101;
            Assert.Equal(unchecked((long)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010), BitOps.RotateLeft(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal(unchecked((long)0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010), BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 64 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 63), BitOps.RotateLeft(sut, int.MaxValue)); // % 64 = 63
        }

        [Fact(DisplayName = nameof(BitOps_RotateRight_Byte))]
        public static void BitOps_RotateRight_Byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, BitOps.RotateRight(sut, 1));
            Assert.Equal((byte)0b01010101, BitOps.RotateRight(sut, 2));
            Assert.Equal((byte)0b10101010, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateRight(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact(DisplayName = nameof(BitOps_RotateRight_SByte))]
        public static void BitOps_RotateRight_SByte()
        {
            sbyte sut = 0b01010101;
            Assert.Equal(unchecked((sbyte)0b10101010), BitOps.RotateRight(sut, 1));
            Assert.Equal(unchecked((sbyte)0b01010101), BitOps.RotateRight(sut, 2));
            Assert.Equal(unchecked((sbyte)0b10101010), BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateRight(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact(DisplayName = nameof(BitOps_RotateRight_UShort))]
        public static void BitOps_RotateRight_UShort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateRight(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, BitOps.RotateRight(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateRight(sut, int.MaxValue)); // % 16 = 15
        }

        [Fact(DisplayName = nameof(BitOps_RotateRight_Short))]
        public static void BitOps_RotateRight_Short()
        {
            short sut = 0b01010101_01010101;
            Assert.Equal(unchecked((short)0b10101010_10101010), BitOps.RotateRight(sut, 1));
            Assert.Equal(unchecked((short)0b01010101_01010101), BitOps.RotateRight(sut, 2));
            Assert.Equal(unchecked((short)0b10101010_10101010), BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateRight(sut, int.MaxValue)); // % 16 = 15
        }

        [Fact(DisplayName = nameof(BitOps_RotateRight_UInt))]
        public static void BitOps_RotateRight_UInt()
        {
            uint sut = 0b01010101_01010101_01010101_01010101u;
            Assert.Equal(0b10101010_10101010_10101010_10101010u, BitOps.RotateRight(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101u, BitOps.RotateRight(sut, 2));
            Assert.Equal(0b10101010_10101010_10101010_10101010u, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 32 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateRight(sut, int.MaxValue)); // % 32 = 15
        }

        [Fact(DisplayName = nameof(BitOps_RotateRight_Int))]
        public static void BitOps_RotateRight_Int()
        {
            int sut = 0b01010101_01010101_01010101_01010101;
            Assert.Equal(unchecked((int)0b10101010_10101010_10101010_10101010), BitOps.RotateRight(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101, BitOps.RotateRight(sut, 2));
            Assert.Equal(unchecked((int)0b10101010_10101010_10101010_10101010), BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 32 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateRight(sut, int.MaxValue)); // % 32 = 15
        }

        [Fact(DisplayName = nameof(BitOps_RotateRight_ULong))]
        public static void BitOps_RotateRight_ULong()
        {
            ulong sut = 0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101ul;
            Assert.Equal(0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010ul, BitOps.RotateRight(sut, 1));
            Assert.Equal(0b01010101_01010101_01010101_01010101_01010101_01010101_01010101_01010101ul, BitOps.RotateRight(sut, 2));
            Assert.Equal(0b10101010_10101010_10101010_10101010_10101010_10101010_10101010_10101010ul, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 64 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 63), BitOps.RotateRight(sut, int.MaxValue)); // % 64 = 63
        }

        [Fact(DisplayName = nameof(BitOps_RotateRight_Long))]
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

        #region PopCount

        [Theory(DisplayName = nameof(BitOps_PopCount_byte))]
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
        public static void BitOps_PopCount_byte(byte n, uint expected)
        {
            uint actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_PopCount_sbyte))]
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
        public static void BitOps_PopCount_sbyte(sbyte n, uint expected)
        {
            uint actual = BitOps.PopCount((byte)n);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_PopCount_ushort))]
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
        public static void BitOps_PopCount_ushort(ushort n, uint expected)
        {
            uint actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_PopCount_short))]
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
        public static void BitOps_PopCount_short(short n, uint expected)
        {
            uint actual = BitOps.PopCount((ushort)n);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_PopCount_uint))]
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
        public static void BitOps_PopCount_uint(uint n, uint expected)
        {
            uint actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_PopCount_int))]
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
        public static void BitOps_PopCount_int(int n, uint expected)
        {
            uint actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_PopCount_ulong))]
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
        public static void BitOps_PopCount_ulong(ulong n, uint expected)
        {
            uint actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_PopCount_long))]
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
        public static void BitOps_PopCount_long(long n, uint expected)
        {
            uint actual = BitOps.PopCount(n);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Log2

        [Theory(DisplayName = nameof(BitOps_Log2_uint))]
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
        public static void BitOps_Log2_uint(uint n, uint expected)
        {
            uint actual = BitOps.Log2(n);
            Assert.Equal(expected, actual);
        }

        //[Theory(DisplayName = nameof(BitOps_Log2_int))]
        //public static void BitOps_Log2_int(int n, int expected)
        //{
        //    int actual = BitOps.Log2(n);
        //    Assert.Equal(expected, actual);
        //}

        [Theory(DisplayName = nameof(BitOps_Log2_ulong))]
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
        public static void BitOps_Log2_ulong(ulong n, uint expected)
        {
            uint actual = BitOps.Log2(n);
            Assert.Equal(expected, actual);
        }

        //[Theory(DisplayName = nameof(BitOps_Log2_long))]
        //public static void BitOps_Log2_long(long n, int expected)
        //{
        //    int actual = BitOps.Log2(n);
        //    Assert.Equal(expected, actual);
        //}

        #endregion

        #region LeadTrail

        [Theory(DisplayName = nameof(BitOps_LeadTrail_byte))]
        [InlineData((byte)0b000u, 8)]
        [InlineData((byte)0b001u, 7)]
        [InlineData((byte)0b010u, 6)]
        [InlineData((byte)0b011u, 6)]
        [InlineData((byte)0b100u, 5)]
        [InlineData((byte)0b101u, 5)]
        [InlineData((byte)0b110u, 5)]
        [InlineData((byte)0b111u, 5)]
        [InlineData((byte)0b1101u, 4)]
        [InlineData((byte)0b1111u, 4)]
        [InlineData((byte)0b10111u, 3)]
        [InlineData((byte)0b11111u, 3)]
        [InlineData((byte)0b110111u, 2)]
        [InlineData((byte)0b111011u, 2)]
        [InlineData((byte)0b1111010u, 1)]
        [InlineData((byte)0b1111101u, 1)]
        [InlineData(byte.MaxValue, 0)]
        [InlineData((byte)0b_0001_0110u, 3)]
        public static void BitOps_LeadTrail_byte(byte n, uint expected)
        {
            byte m = n;

            // LeadingZeros
            uint actual = BitOps.LeadingZeroCount(m);
            Assert.Equal(expected, actual);

            // TrailingZeros
            m = Reverse(n);
            Assert.Equal(n, Reverse(m));

            actual = BitOps.TrailingZeroCount(m);
            if (actual == 32u) actual = 8u; // 32 vs 8 bit
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_LeadTrail_sbyte))]
        [InlineData((sbyte)0b000u, 8)]
        [InlineData((sbyte)0b001u, 7)]
        [InlineData((sbyte)0b010u, 6)]
        [InlineData((sbyte)0b011u, 6)]
        [InlineData((sbyte)0b100u, 5)]
        [InlineData((sbyte)0b101u, 5)]
        [InlineData((sbyte)0b110u, 5)]
        [InlineData((sbyte)0b111u, 5)]
        [InlineData((sbyte)0b1101u, 4)]
        [InlineData((sbyte)0b1111u, 4)]
        [InlineData((sbyte)0b10111u, 3)]
        [InlineData((sbyte)0b11111u, 3)]
        [InlineData((sbyte)0b110111u, 2)]
        [InlineData((sbyte)0b111011u, 2)]
        [InlineData((sbyte)0b1111010u, 1)]
        [InlineData((sbyte)0b1111101u, 1)]
        [InlineData(sbyte.MinValue, 0)]
        [InlineData(sbyte.MaxValue, 1)]
        [InlineData((sbyte)0b_0001_0110u, 3)]
        public static void BitOps_LeadTrail_sbyte(sbyte n, uint expected)
        {
            sbyte m = n;

            // LeadingZeros
            uint actual = BitOps.LeadingZeroCount(m);
            Assert.Equal(expected, actual);

            // TrailingZeros
            m = Reverse(n);
            Assert.Equal(n, Reverse(m));

            actual = BitOps.TrailingZeroCount(m);
            if (actual == 32u) actual = 8u; // 32 vs 8 bit
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_LeadTrail_ushort))]
        [InlineData((ushort)0b000u, 16)]
        [InlineData((ushort)0b001u, 15)]
        [InlineData((ushort)0b010u, 14)]
        [InlineData((ushort)0b011u, 14)]
        [InlineData((ushort)0b100u, 13)]
        [InlineData((ushort)0b101u, 13)]
        [InlineData((ushort)0b110u, 13)]
        [InlineData((ushort)0b111u, 13)]
        [InlineData((ushort)0b1101u, 12)]
        [InlineData((ushort)0b1111u, 12)]
        [InlineData((ushort)0b10111u, 11)]
        [InlineData((ushort)0b11111u, 11)]
        [InlineData((ushort)0b110111u, 10)]
        [InlineData((ushort)0b111011u, 10)]
        [InlineData((ushort)0b1111010u, 9)]
        [InlineData((ushort)0b1111101u, 9)]
        [InlineData(ushort.MaxValue, 0)]
        [InlineData((ushort)0b_0001_0110u, 11)]
        public static void BitOps_LeadTrail_ushort(ushort n, uint expected)
        {
            ushort m = n;

            // LeadingZeros
            uint actual = BitOps.LeadingZeroCount(m);
            Assert.Equal(expected, actual);

            // TrailingZeros
            m = Reverse(n);
            Assert.Equal(n, Reverse(m));

            actual = BitOps.TrailingZeroCount(m);
            if (actual == 32u) actual = 16u; // 32 vs 16 bit
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_LeadTrail_short))]
        [InlineData((short)0b000u, 16)]
        [InlineData((short)0b001u, 15)]
        [InlineData((short)0b010u, 14)]
        [InlineData((short)0b011u, 14)]
        [InlineData((short)0b100u, 13)]
        [InlineData((short)0b101u, 13)]
        [InlineData((short)0b110u, 13)]
        [InlineData((short)0b111u, 13)]
        [InlineData((short)0b1101u, 12)]
        [InlineData((short)0b1111u, 12)]
        [InlineData((short)0b10111u, 11)]
        [InlineData((short)0b11111u, 11)]
        [InlineData((short)0b110111u, 10)]
        [InlineData((short)0b111011u, 10)]
        [InlineData((short)0b1111010u, 9)]
        [InlineData((short)0b1111101u, 9)]
        [InlineData(short.MaxValue, 1)]
        [InlineData((short)0b_0001_0110u, 11)]
        public static void BitOps_LeadTrail_short(short n, uint expected)
        {
            short m = n;

            // LeadingZeros
            uint actual = BitOps.LeadingZeroCount(m);
            Assert.Equal(expected, actual);

            // TrailingZeros
            m = Reverse(n);
            Assert.Equal(n, Reverse(m));

            actual = BitOps.TrailingZeroCount(m);
            if (actual == 32u) actual = 16u; // 32 vs 16 bit
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_LeadTrail_uint))]
        [InlineData(0b000u, 32)]
        [InlineData(0b001u, 31)]
        [InlineData(0b010u, 30)]
        [InlineData(0b011u, 30)]
        [InlineData(0b100u, 29)]
        [InlineData(0b101u, 29)]
        [InlineData(0b110u, 29)]
        [InlineData(0b111u, 29)]
        [InlineData(0b1101u, 28)]
        [InlineData(0b1111u, 28)]
        [InlineData(0b10111u, 27)]
        [InlineData(0b11111u, 27)]
        [InlineData(0b110111u, 26)]
        [InlineData(0b111011u, 26)]
        [InlineData(0b1111010u, 25)]
        [InlineData(0b1111101u, 25)]
        [InlineData((uint)byte.MaxValue, 32 - 8)]
        [InlineData((uint)(ushort.MaxValue >> 3), 32 - 16 + 3)]
        [InlineData((uint)ushort.MaxValue, 32 - 16)]
        [InlineData((uint.MaxValue >> 5), 5)]
        [InlineData(1u << 27, 32 - 1 - 27)]
        [InlineData(uint.MaxValue, 0)]
        [InlineData(0b_0001_0111_1111_1111_1111_1111_1111_1110u, 3)]
        public static void BitOps_LeadTrail_uint(uint n, uint expected)
        {
            uint m = n;

            // LeadingZeros
            uint actual = BitOps.LeadingZeroCount(m);
            Assert.Equal(expected, actual);

            m = Reverse(n);
            // TrailingZeros
            Assert.Equal(n, Reverse(m));

            actual = BitOps.TrailingZeroCount(m);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_LeadTrail_int))]
        [InlineData(0b000, 32)]
        [InlineData(0b001, 31)]
        [InlineData(0b010, 30)]
        [InlineData(0b011, 30)]
        [InlineData(0b100, 29)]
        [InlineData(0b101, 29)]
        [InlineData(0b110, 29)]
        [InlineData(0b111, 29)]
        [InlineData(0b1101, 28)]
        [InlineData(0b1111, 28)]
        [InlineData(0b10111, 27)]
        [InlineData(0b11111, 27)]
        [InlineData(0b110111, 26)]
        [InlineData(0b111011, 26)]
        [InlineData(0b1111010, 25)]
        [InlineData(0b1111101, 25)]
        [InlineData((int)byte.MaxValue, 32 - 8)]
        [InlineData((int)(ushort.MaxValue >> 3), 32 - 16 + 3)]
        [InlineData((int)ushort.MaxValue, 32 - 16)]
        [InlineData(int.MaxValue >> 5, 6)]
        [InlineData(1 << 27, 32 - 1 - 27)]
        [InlineData(int.MaxValue, 1)]
        [InlineData(0b_0001_0111_1111_1111_1111_1111_1111_1110, 3)]
        public static void BitOps_LeadTrail_int(int n, uint expected)
        {
            int m = n;

            // LeadingZeros
            uint actual = BitOps.LeadingZeroCount(m);
            Assert.Equal(expected, actual);

            // TrailingZeros
            m = Reverse(n);
            Assert.Equal(n, Reverse(m));

            actual = BitOps.TrailingZeroCount(m);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_LeadTrail_ulong))]
        [InlineData(0b000ul, 64)]
        [InlineData(0b001ul, 63)]
        [InlineData(0b010ul, 62)]
        [InlineData(0b011ul, 62)]
        [InlineData(0b100ul, 61)]
        [InlineData(0b101ul, 61)]
        [InlineData(0b110ul, 61)]
        [InlineData(0b111ul, 61)]
        [InlineData(0b1101ul, 60)]
        [InlineData(0b1111ul, 60)]
        [InlineData(0b10111ul, 59)]
        [InlineData(0b11111ul, 59)]
        [InlineData(0b110111ul, 58)]
        [InlineData(0b111011ul, 58)]
        [InlineData(0b1111010ul, 57)]
        [InlineData(0b1111101ul, 57)]
        [InlineData((ulong)byte.MaxValue, 64 - 8)]
        [InlineData((ulong)(ushort.MaxValue >> 3), 64 - 16 + 3)]
        [InlineData((ulong)ushort.MaxValue, 64 - 16)]
        [InlineData((ulong)(uint.MaxValue >> 5), 32 + 5)]
        [InlineData((ulong)uint.MaxValue, 32)]
        [InlineData(ulong.MaxValue >> 9, 9)]
        [InlineData(1ul << 57, 64 - 1 - 57)]
        [InlineData(ulong.MaxValue, 0)]
        [InlineData(0b_0001_0111_1111_1111_1111_1111_1111_1110ul, 32 + 3)]
        public static void BitOps_LeadTrail_ulong(ulong n, uint expected)
        {
            ulong m = n;

            // LeadingZeros
            uint actual = BitOps.LeadingZeroCount(m);
            Assert.Equal(expected, actual);

            m = Reverse(n);
            // TrailingZeros
            Assert.Equal(n, Reverse(m));

            actual = BitOps.TrailingZeroCount(m);
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = nameof(BitOps_LeadTrail_long))]
        [InlineData(0b000L, 64)]
        [InlineData(0b001L, 63)]
        [InlineData(0b010L, 62)]
        [InlineData(0b011L, 62)]
        [InlineData(0b100L, 61)]
        [InlineData(0b101L, 61)]
        [InlineData(0b110L, 61)]
        [InlineData(0b111L, 61)]
        [InlineData(0b1101L, 60)]
        [InlineData(0b1111L, 60)]
        [InlineData(0b10111L, 59)]
        [InlineData(0b11111L, 59)]
        [InlineData(0b110111L, 58)]
        [InlineData(0b111011L, 58)]
        [InlineData(0b1111010L, 57)]
        [InlineData(0b1111101L, 57)]
        [InlineData((long)byte.MaxValue, 64 - 8)]
        [InlineData((long)(ushort.MaxValue >> 3), 64 - 16 + 3)]
        [InlineData((long)ushort.MaxValue, 64 - 16)]
        [InlineData((long)(uint.MaxValue >> 5), 32 + 5)]
        [InlineData((long)uint.MaxValue, 32)]
        [InlineData(long.MaxValue >> 9, 10)]
        [InlineData(1L << 57, 64 - 1 - 57)]
        [InlineData(long.MaxValue, 1)]
        [InlineData(0b_0001_0111_1111_1111_1111_1111_1111_1110L, 32 + 3)]
        public static void BitOps_LeadTrail_long(long n, uint expected)
        {
            long m = n;

            // LeadingZeros
            uint actual = BitOps.LeadingZeroCount(m);
            Assert.Equal(expected, actual);

            m = Reverse(n);
            // TrailingZeros
            Assert.Equal(n, Reverse(m));

            actual = BitOps.TrailingZeroCount(m);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Normalize

        [Fact(DisplayName = nameof(BitOps_Normalize))]
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

        [Theory(DisplayName = nameof(BitOps_Iff_byte))]
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

        [Theory(DisplayName = nameof(BitOps_Iff_sbyte))]
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

        [Theory(DisplayName = nameof(BitOps_Iff_ushort))]
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

        [Theory(DisplayName = nameof(BitOps_Iff_short))]
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

        [Theory(DisplayName = nameof(BitOps_Iff_ushort))]
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

        [Theory(DisplayName = nameof(BitOps_Iff_int))]
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

        [Theory(DisplayName = nameof(BitOps_Iff_ulong))]
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

        [Theory(DisplayName = nameof(BitOps_Iff_long))]
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
                    result |= (1u << (31 - i));
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
                    result |= (1ul << (63 - i));
            }

            return result;
        }

        private static long Reverse(long value)
            => unchecked((long)Reverse((ulong)value));

        #endregion
    }
}

#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

namespace SourceCode.Clay.Tests
{
    partial class BitOpsTests
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
            Assert.Equal(2, BitOps.LeadingZeroCount(0b0011_1000));
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

        private static ReadOnlySpan<byte> s_Log10Ceiling32 => new byte[33]
        {
            11, 10, 10, 09, 09, 09, 08, 08,
            08, 07, 07, 07, 07, 06, 06, 06,
            05, 05, 05, 04, 04, 04, 04, 03,
            03, 03, 02, 02, 02, 01, 01, 01,
            02
        };

        private static readonly uint[] s_Pow10Ceiling32 = new uint[12]
        {
            0, 1, 10, 100, 1000, 10000,
            100000, 1000000, 10000000, 100000000, 1000000000, 0
        };

        [Fact]
        public static void Foo()
        {
            for (uint value = 0; value <= uint.MaxValue; value++)
            {
                uint log10ceil = Unsafe.AddByteOffset(
                    ref MemoryMarshal.GetReference(s_Log10Ceiling32),
                    (IntPtr)BitOps.LeadingZeroCount(value));

                uint pow10;
                unsafe
                {
                    fixed (uint* u = s_Pow10Ceiling32)
                    {
                        pow10 = u[log10ceil];
                    }
                }

                //exp10ceil = s_Exp10Ceiling32[log10ceil];
                int d = (int)(value - pow10 >> 31);
                var result = log10ceil - d;
            }
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

        #region Rotate

        [Fact]
        public static void BitOps_RotateLeft_byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, BitOps.RotateLeft(sut, 1));
            Assert.Equal((byte)0b01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal((byte)0b10101010, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateLeft(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact]
        public static void BitOps_RotateLeft_ushort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateLeft(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, BitOps.RotateLeft(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateLeft(sut, 3));
            Assert.Equal(sut, BitOps.RotateLeft(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateLeft(sut, int.MaxValue)); // % 16 = 15
        }

        [Fact]
        public static void BitOps_RotateRight_byte()
        {
            byte sut = 0b01010101;
            Assert.Equal((byte)0b10101010, BitOps.RotateRight(sut, 1));
            Assert.Equal((byte)0b01010101, BitOps.RotateRight(sut, 2));
            Assert.Equal((byte)0b10101010, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 8 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 7), BitOps.RotateRight(sut, int.MaxValue)); // % 8 = 7
        }

        [Fact]
        public static void BitOps_RotateRight_ushort()
        {
            ushort sut = 0b01010101_01010101;
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateRight(sut, 1));
            Assert.Equal((ushort)0b01010101_01010101, BitOps.RotateRight(sut, 2));
            Assert.Equal((ushort)0b10101010_10101010, BitOps.RotateRight(sut, 3));
            Assert.Equal(sut, BitOps.RotateRight(sut, int.MinValue)); // % 16 = 0
            Assert.Equal(BitOps.RotateLeft(sut, 15), BitOps.RotateRight(sut, int.MaxValue)); // % 16 = 15
        }

        #endregion

        #region AsByte

        [Fact]
        public static void BitOps_AsByte()
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
            }
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

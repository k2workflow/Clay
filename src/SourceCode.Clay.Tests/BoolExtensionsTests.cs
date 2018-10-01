#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class BoolExtensionsTests
    {
#pragma warning disable IDE0044 // Add readonly modifier
        // Prevent folding by using non-readonly non-constant
        private static bool @true = true;
        private static bool @false = false;
#pragma warning restore IDE0044 // Add readonly modifier

        [Fact(DisplayName = nameof(BoolToByte))]
        public static void BoolToByte()
        {
            const byte t0 = 0;
            const byte t1 = 1;
            const byte t2 = 2;
            const byte t3 = 3;
            const byte max = byte.MaxValue;
            const byte min = byte.MinValue;

            Assert.Equal(t1, @true.Evaluate(t1));
            Assert.Equal(t0, @false.Evaluate(t1));

            Assert.Equal(t2, @true.Evaluate(t2));
            Assert.Equal(t3, @false.Evaluate(t1, t3));

            Assert.Equal(t2, @true.Evaluate(t2, t3));
            Assert.Equal(t3, @false.Evaluate(t2, t3));

            Assert.Equal(max, @true.Evaluate(max, min));
            Assert.Equal(max, @false.Evaluate(min, max));
        }

        [Fact(DisplayName = nameof(ByteToBool))]
        public static void ByteToBool()
        {
            for (uint i = 0; i <= byte.MaxValue; i++)
            {
                Assert.Equal(i != 0, i.Evaluate());
            }
        }

        [Fact(DisplayName = nameof(SByteToBool))]
        public static void SByteToBool()
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                Assert.Equal(i != 0, i.Evaluate());
            }
        }

        [Fact(DisplayName = nameof(BoolToUInt16))]
        public static void BoolToUInt16()
        {
            const ushort t0 = 0;
            const ushort t1 = 1;
            const ushort t2 = 2;
            const ushort t3 = 3;
            const ushort max = ushort.MaxValue;
            const ushort min = ushort.MinValue;

            Assert.Equal(t1, @true.Evaluate(t1));
            Assert.Equal(t0, @false.Evaluate(t1));

            Assert.Equal(t2, @true.Evaluate(t2));
            Assert.Equal(t3, @false.Evaluate(t1, t3));

            Assert.Equal(t2, @true.Evaluate(t2, t3));
            Assert.Equal(t3, @false.Evaluate(t2, t3));

            Assert.Equal(max, @true.Evaluate(max, min));
            Assert.Equal(max, @false.Evaluate(min, max));
        }

        [Fact(DisplayName = nameof(UInt16ToBool))]
        public static void UInt16ToBool()
        {
            for (uint i = 0; i <= ushort.MaxValue; i++)
            {
                Assert.Equal(i != 0, i.Evaluate());
            }
        }

        [Fact(DisplayName = nameof(Int16ToBool))]
        public static void Int16ToBool()
        {
            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                Assert.Equal(i != 0, i.Evaluate());
            }
        }

        [Fact(DisplayName = nameof(BoolToUInt32))]
        public static void BoolToUInt32()
        {
            const uint t0 = 0;
            const uint t1 = 1;
            const uint t2 = 2;
            const uint t3 = 3;
            const uint max = uint.MaxValue;
            const uint min = uint.MinValue;

            Assert.Equal(t1, @true.Evaluate(t1));
            Assert.Equal(t0, @false.Evaluate(t1));

            Assert.Equal(t2, @true.Evaluate(t2));
            Assert.Equal(t3, @false.Evaluate(t1, t3));

            Assert.Equal(t2, @true.Evaluate(t2, t3));
            Assert.Equal(t3, @false.Evaluate(t2, t3));

            Assert.Equal(max, @true.Evaluate(max, min));
            Assert.Equal(max, @false.Evaluate(min, max));
        }

        [Theory(DisplayName = nameof(UInt32ToBool))]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(byte.MaxValue - 1)]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MaxValue + 1)]
        [InlineData(ushort.MaxValue - 1)]
        [InlineData(ushort.MaxValue)]
        [InlineData(ushort.MaxValue + 1)]
        [InlineData(uint.MaxValue - 1)]
        [InlineData(uint.MaxValue)]
        public static void UInt32ToBool(uint value)
        {
            Assert.Equal(value != 0, value.Evaluate());
        }

        [Theory(DisplayName = nameof(Int32ToBool))]
        [InlineData(int.MinValue)]
        [InlineData(int.MinValue + 1)]
        [InlineData(short.MinValue - 1)]
        [InlineData(short.MinValue)]
        [InlineData(short.MinValue + 1)]
        [InlineData(sbyte.MinValue - 1)]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MinValue + 1)]
        [InlineData(-2)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(sbyte.MaxValue - 1)]
        [InlineData(sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue + 1)]
        [InlineData(short.MaxValue - 1)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MaxValue + 1)]
        [InlineData(int.MaxValue - 1)]
        [InlineData(int.MaxValue)]
        public static void Int32ToBool(int value)
        {
            Assert.Equal(value != 0, value.Evaluate());
        }

        [Fact(DisplayName = nameof(BoolToUInt64))]
        public static void BoolToUInt64()
        {
            const ulong t0 = 0;
            const ulong t1 = 1;
            const ulong t2 = 2;
            const ulong t3 = 3;
            const ulong max = ulong.MaxValue;
            const ulong min = ulong.MinValue;

            Assert.Equal(t1, @true.Evaluate(t1));
            Assert.Equal(t0, @false.Evaluate(t1));

            Assert.Equal(t2, @true.Evaluate(t2));
            Assert.Equal(t3, @false.Evaluate(t1, t3));

            Assert.Equal(t2, @true.Evaluate(t2, t3));
            Assert.Equal(t3, @false.Evaluate(t2, t3));

            Assert.Equal(max, @true.Evaluate(max, min));
            Assert.Equal(max, @false.Evaluate(min, max));
        }

        [Theory(DisplayName = nameof(UInt64ToBool))]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(byte.MaxValue - 1)]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MaxValue + 1)]
        [InlineData(ushort.MaxValue - 1)]
        [InlineData(ushort.MaxValue)]
        [InlineData(ushort.MaxValue + 1)]
        [InlineData(uint.MaxValue - 1)]
        [InlineData(uint.MaxValue)]
        [InlineData((ulong)uint.MaxValue + 1)]
        [InlineData(ulong.MaxValue - 1)]
        [InlineData(ulong.MaxValue)]
        public static void UInt64ToBool(ulong value)
        {
            Assert.Equal(value != 0, value.Evaluate());
        }

        [Theory(DisplayName = nameof(Int64ToBool))]
        [InlineData(long.MinValue)]
        [InlineData(long.MinValue + 1)]
        [InlineData((long)int.MinValue - 1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MinValue + 1)]
        [InlineData(short.MinValue - 1)]
        [InlineData(short.MinValue)]
        [InlineData(short.MinValue + 1)]
        [InlineData(sbyte.MinValue - 1)]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MinValue + 1)]
        [InlineData(-2)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(sbyte.MaxValue - 1)]
        [InlineData(sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue + 1)]
        [InlineData(short.MaxValue - 1)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MaxValue + 1)]
        [InlineData(int.MaxValue - 1)]
        [InlineData(int.MaxValue)]
        [InlineData((long)int.MaxValue + 1)]
        [InlineData(long.MaxValue - 1)]
        [InlineData(long.MaxValue)]
        public static void Int64ToBool(long value)
        {
            Assert.Equal(value != 0, value.Evaluate());
        }
    }
}
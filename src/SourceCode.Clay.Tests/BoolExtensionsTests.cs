#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class BoolExtensionsTests
    {
        private static readonly bool @true = true;
        private static readonly bool @false = false;

        [Fact(DisplayName = nameof(BoolToByte))]
        public static void BoolToByte()
        {
            Assert.Equal(1, @true.ToByte());
            Assert.Equal(0, @false.ToByte());

            Assert.Equal(2, @true.ToByte(2));
            Assert.Equal(3, @false.ToByte(falseValue:3));

            Assert.Equal(2, @true.ToByte(2, 3));
            Assert.Equal(3, @false.ToByte(2, 3));

            Assert.Equal(byte.MaxValue, @true.ToByte(byte.MaxValue, byte.MinValue));
            Assert.Equal(byte.MaxValue, @false.ToByte(byte.MinValue, byte.MaxValue));
        }

        [Fact(DisplayName = nameof(BoolToUInt16))]
        public static void BoolToUInt16()
        {
            Assert.Equal(1, @true.ToUInt16());
            Assert.Equal(0, @false.ToUInt16());

            Assert.Equal(2, @true.ToUInt16(2));
            Assert.Equal(3, @false.ToUInt16(falseValue: 3));

            Assert.Equal(2, @true.ToUInt16(2, 3));
            Assert.Equal(3, @false.ToUInt16(2, 3));

            Assert.Equal(ushort.MaxValue, @true.ToUInt16(ushort.MaxValue, ushort.MinValue));
            Assert.Equal(ushort.MaxValue, @false.ToUInt16(ushort.MinValue, ushort.MaxValue));
        }

        [Fact(DisplayName = nameof(BoolToUInt32))]
        public static void BoolToUInt32()
        {
            Assert.Equal(1u, @true.ToUInt32());
            Assert.Equal(0u, @false.ToUInt32());

            Assert.Equal(2u, @true.ToUInt32(2));
            Assert.Equal(3u, @false.ToUInt32(falseValue: 3));

            Assert.Equal(2u, @true.ToUInt32(2, 3));
            Assert.Equal(3u, @false.ToUInt32(2, 3));

            Assert.Equal(uint.MaxValue, @true.ToUInt32(uint.MaxValue, uint.MinValue));
            Assert.Equal(uint.MaxValue, @false.ToUInt32(uint.MinValue, uint.MaxValue));
        }

        [Fact(DisplayName = nameof(BoolToUInt64))]
        public static void BoolToUInt64()
        {
            Assert.Equal(1ul, @true.ToUInt64());
            Assert.Equal(0ul, @false.ToUInt64());

            Assert.Equal(2ul, @true.ToUInt64(2));
            Assert.Equal(3ul, @false.ToUInt64(falseValue: 3));

            Assert.Equal(2ul, @true.ToUInt64(2, 3));
            Assert.Equal(3ul, @false.ToUInt64(2, 3));

            Assert.Equal(ulong.MaxValue, @true.ToUInt64(ulong.MaxValue, ulong.MinValue));
            Assert.Equal(ulong.MaxValue, @false.ToUInt64(ulong.MinValue, ulong.MaxValue));
        }
    }
}
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
    }
}
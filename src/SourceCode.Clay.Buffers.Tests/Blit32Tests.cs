#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class Blit32Tests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_blitting_UInt32_to_Int32))]
        public static void When_blitting_UInt32_to_Int32()
        {
            var tests = new uint[] { uint.MinValue, uint.MinValue + 1, 0, 100, uint.MaxValue - 1, uint.MaxValue, BitConverter.ToUInt32(new byte[] { 1, 2, 4, 8 }, 0) };

            foreach (var test in tests)
            {
                var expected = unchecked((int)test);

                var actual = new Blit32 { UInt32 = test };
                Assert.Equal(expected, actual.Int32);

                var bytes = actual.GetBytes();
                Assert.Equal(actual.Byte0, bytes[0]);
                Assert.Equal(actual.Byte1, bytes[1]);
                Assert.Equal(actual.Byte2, bytes[2]);
                Assert.Equal(actual.Byte3, bytes[3]);

                Assert.Equal(actual.Byte0, actual.Blit0.Byte0);
                Assert.Equal(actual.Byte1, actual.Blit0.Byte1);
                Assert.Equal(actual.Byte2, actual.Blit1.Byte0);
                Assert.Equal(actual.Byte3, actual.Blit1.Byte1);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_blitting_Int32_to_UInt32))]
        public static void When_blitting_Int32_to_UInt32()
        {
            var tests = new[] { int.MinValue, int.MinValue + 1, 0, 100, int.MaxValue - 1, int.MaxValue, BitConverter.ToInt32(new byte[] { 1, 2, 4, 8 }, 0) };

            foreach (var test in tests)
            {
                var expected = unchecked((uint)test);

                var actual = new Blit32 { Int32 = test };
                Assert.Equal(expected, actual.UInt32);

                var bytes = actual.GetBytes();
                Assert.Equal(actual.Byte0, bytes[0]);
                Assert.Equal(actual.Byte1, bytes[1]);
                Assert.Equal(actual.Byte2, bytes[2]);
                Assert.Equal(actual.Byte3, bytes[3]);

                Assert.Equal(actual.Byte0, actual.Blit0.Byte0);
                Assert.Equal(actual.Byte1, actual.Blit0.Byte1);
                Assert.Equal(actual.Byte2, actual.Blit1.Byte0);
                Assert.Equal(actual.Byte3, actual.Blit1.Byte1);
            }
        }

        #endregion
    }
}

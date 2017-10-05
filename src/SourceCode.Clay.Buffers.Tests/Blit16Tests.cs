#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class Blit16Tests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_blitting_UInt16_to_Int16))]
        public static void When_blitting_UInt16_to_Int16()
        {
            var tests = new ushort[] { ushort.MinValue, ushort.MinValue + 1, 0, 100, ushort.MaxValue - 1, ushort.MaxValue, BitConverter.ToUInt16(new byte[] { 1, 2 }, 0) };

            foreach (var test in tests)
            {
                var expected = unchecked((short)test);

                var actual = new Blit16 { UInt16 = test };
                Assert.Equal(expected, actual.Int16);

                var bytes = actual.GetBytes();
                Assert.Equal(actual.Byte0, bytes[0]);
                Assert.Equal(actual.Byte1, bytes[1]);

                Assert.Equal(actual.Byte0, actual.Blit0.Byte);
                Assert.Equal(actual.Byte1, actual.Blit1.Byte);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_blitting_Int16_to_UInt16))]
        public static void When_blitting_Int16_to_UInt16()
        {
            var tests = new short[] { short.MinValue, short.MinValue + 1, 0, 100, short.MaxValue - 1, short.MaxValue, BitConverter.ToInt16(new byte[] { 1, 2 }, 0) };

            foreach (var test in tests)
            {
                var expected = unchecked((ushort)test);

                var actual = new Blit16 { Int16 = test };
                Assert.Equal(expected, actual.UInt16);

                var bytes = actual.GetBytes();
                Assert.Equal(actual.Byte0, bytes[0]);
                Assert.Equal(actual.Byte1, bytes[1]);

                Assert.Equal(actual.Byte0, actual.Blit0.Byte);
                Assert.Equal(actual.Byte1, actual.Blit1.Byte);
            }
        }

        #endregion
    }
}

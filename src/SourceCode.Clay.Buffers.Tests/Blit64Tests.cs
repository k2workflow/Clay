using System;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class Blit64Tests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_blitting_UInt64_to_Int64))]
        public static void When_blitting_UInt64_to_Int64()
        {
            var tests = new ulong[] { ulong.MinValue, ulong.MinValue + 1, 0, 100, ulong.MaxValue - 1, ulong.MaxValue, BitConverter.ToUInt64(new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 }, 0) };

            foreach (var test in tests)
            {
                var expected = unchecked((long)test);

                var actual = new Blit64 { UInt64 = test };
                Assert.Equal(expected, actual.Int64);

                var bytes = actual.GetBytes();
                Assert.Equal(actual.Byte0, bytes[0]);
                Assert.Equal(actual.Byte1, bytes[1]);
                Assert.Equal(actual.Byte2, bytes[2]);
                Assert.Equal(actual.Byte3, bytes[3]);
                Assert.Equal(actual.Byte4, bytes[4]);
                Assert.Equal(actual.Byte5, bytes[5]);
                Assert.Equal(actual.Byte6, bytes[6]);
                Assert.Equal(actual.Byte7, bytes[7]);

                Assert.Equal(actual.Byte0, actual.Blit0.Byte0);
                Assert.Equal(actual.Byte1, actual.Blit0.Byte1);
                Assert.Equal(actual.Byte2, actual.Blit0.Byte2);
                Assert.Equal(actual.Byte3, actual.Blit0.Byte3);

                Assert.Equal(actual.Byte4, actual.Blit1.Byte0);
                Assert.Equal(actual.Byte5, actual.Blit1.Byte1);
                Assert.Equal(actual.Byte6, actual.Blit1.Byte2);
                Assert.Equal(actual.Byte7, actual.Blit1.Byte3);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_blitting_Int64_to_UInt64))]
        public static void When_blitting_Int64_to_UInt64()
        {
            var tests = new[] { long.MinValue, long.MinValue + 1, 0, 100, long.MaxValue - 1, long.MaxValue, BitConverter.ToInt64(new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 }, 0) };

            foreach (var test in tests)
            {
                var expected = unchecked((ulong)test);

                var actual = new Blit64 { Int64 = test };
                Assert.Equal(expected, actual.UInt64);

                var bytes = actual.GetBytes();
                Assert.Equal(actual.Byte0, bytes[0]);
                Assert.Equal(actual.Byte1, bytes[1]);
                Assert.Equal(actual.Byte2, bytes[2]);
                Assert.Equal(actual.Byte3, bytes[3]);
                Assert.Equal(actual.Byte4, bytes[4]);
                Assert.Equal(actual.Byte5, bytes[5]);
                Assert.Equal(actual.Byte6, bytes[6]);
                Assert.Equal(actual.Byte7, bytes[7]);

                Assert.Equal(actual.Byte0, actual.Blit0.Byte0);
                Assert.Equal(actual.Byte1, actual.Blit0.Byte1);
                Assert.Equal(actual.Byte2, actual.Blit0.Byte2);
                Assert.Equal(actual.Byte3, actual.Blit0.Byte3);

                Assert.Equal(actual.Byte4, actual.Blit1.Byte0);
                Assert.Equal(actual.Byte5, actual.Blit1.Byte1);
                Assert.Equal(actual.Byte6, actual.Blit1.Byte2);
                Assert.Equal(actual.Byte7, actual.Blit1.Byte3);
            }
        }
    }
}

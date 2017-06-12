using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class Blit16Tests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "Blit16: UInt16 to Int16")]
        public static void When_blitting_UInt16_to_Int16()
        {
            var tests = new ushort[] { ushort.MinValue, ushort.MinValue + 1, 0, 100, ushort.MaxValue - 1, ushort.MaxValue };

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
        [Fact(DisplayName = "Blit16: Int16 to UInt16")]
        public static void When_blitting_Int16_to_UInt16()
        {
            var tests = new short[] { short.MinValue, short.MinValue + 1, 0, 100, short.MaxValue - 1, short.MaxValue };

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
    }
}

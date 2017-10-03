using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class Blit8Tests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_blitting_byte_to_sbyte))]
        public static void When_blitting_byte_to_sbyte()
        {
            var tests = new byte[] { byte.MinValue, byte.MinValue + 1, 0, 100, byte.MaxValue - 1, byte.MaxValue };

            foreach (var test in tests)
            {
                var expected = unchecked((sbyte)test);

                var actual = new Blit8 { Byte = test };
                Assert.Equal(expected, actual.SByte);

                var bytes = actual.GetBytes();
                Assert.Equal(expected, (sbyte)bytes[0]);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_blitting_sbyte_to_byte))]
        public static void When_blitting_sbyte_to_byte()
        {
            var tests = new sbyte[] { sbyte.MinValue, sbyte.MinValue + 1, 0, 100, sbyte.MaxValue - 1, sbyte.MaxValue };

            foreach (var test in tests)
            {
                var expected = unchecked((byte)test);

                var actual = new Blit8 { SByte = test };
                Assert.Equal(expected, actual.Byte);

                var bytes = actual.GetBytes();
                Assert.Equal(expected, bytes[0]);
            }
        }
    }
}

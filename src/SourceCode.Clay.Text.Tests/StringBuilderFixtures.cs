using System.Text;
using Xunit;

namespace SourceCode.Clay.Text.Tests
{
    public static class StringBuilderFixtures
    {
        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_format_line_1()
        {
            var actual = new StringBuilder("A").AppendFormatLine("-{0}-", 123);
            const string expected = "A-123-\r\n";

            Assert.Equal(expected.Length, actual.Length);
            Assert.Equal(expected, actual.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_format_line_2()
        {
            var actual = new StringBuilder("A").AppendFormatLine("-{0}-{1}-", 123, false);
            const string expected = "A-123-False-\r\n";

            Assert.Equal(expected.Length, actual.Length);
            Assert.Equal(expected, actual.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_format_line_3()
        {
            var actual = new StringBuilder("A").AppendFormatLine("-{0}-{1}-{2}-", 123, false, "X");
            const string expected = "A-123-False-X-\r\n";

            Assert.Equal(expected.Length, actual.Length);
            Assert.Equal(expected, actual.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_format_line_N()
        {
            var actual = new StringBuilder("A").AppendFormatLine("-{0}-{1}-{2}-{3}-", 123, false, "X", 'z');
            const string expected = "A-123-False-X-z-\r\n";

            Assert.Equal(expected.Length, actual.Length);
            Assert.Equal(expected, actual.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_builder_tiny()
        {
            var actual = new StringBuilder("A").AppendBuilder(new StringBuilder("B"));
            const string expected = "AB";

            Assert.Equal(expected.Length, actual.Length);
            Assert.Equal(expected, actual.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_builder_medium()
        {
            var x = new string('A', 256);
            var y = new string('B', 256);
            var actual = new StringBuilder(x).AppendBuilder(new StringBuilder(y));

            Assert.Equal(x.Length + y.Length, actual.Length);
            Assert.Equal(x + y, actual.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_builder_large()
        {
            var x = new string('A', 1024);
            var y = new string('B', 1024);
            var actual = new StringBuilder(x).AppendBuilder(new StringBuilder(y));

            Assert.Equal(x.Length + y.Length, actual.Length);
            Assert.Equal(x + y, actual.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_builder_null()
        {
            var actual = new StringBuilder("A").AppendBuilder(null);
            const string expected = "A";

            Assert.Equal(expected.Length, actual.Length);
            Assert.Equal(expected, actual.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_builder_line()
        {
            var actual = new StringBuilder("A").AppendBuilderLine(new StringBuilder("B"));
            const string expected = "AB\r\n";

            Assert.Equal(expected.Length, actual.Length);
            Assert.Equal(expected, actual.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_append_builder_line_null()
        {
            var actual = new StringBuilder("A").AppendBuilderLine(null);
            const string expected = "A\r\n";

            Assert.Equal(expected.Length, actual.Length);
            Assert.Equal(expected, actual.ToString());
        }
    }
}

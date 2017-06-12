using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class StringExtensionsTests
    {
        private const string LongStr = @"From Wikipedia: Astley was born on 6 February 1966 in Newton-le-Willows in Lancashire, the fourth child of his family. His parents divorced when he was five, and Astley was brought up by his father.[9] His musical career started when he was ten, singing in the local church choir.[10] During his schooldays, Astley formed and played the drums in a number of local bands, where he met guitarist David Morris.[2][11] After leaving school at sixteen, Astley was employed during the day as a driver in his father's market-gardening business and played drums on the Northern club circuit at night in bands such as Give Way – specialising in covering Beatles and Shadows songs – and FBI, which won several local talent competitions.[10]";

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "StringExtensions Left")]
        public static void When_left_string()
        {
            // Null
            var actual = ((string)null).Left(-1);
            Assert.Null(actual);

            actual = ((string)null).Left(0);
            Assert.Null(actual);

            actual = ((string)null).Left(1);
            Assert.Null(actual);

            // Empty
            actual = string.Empty.Left(-1);
            Assert.Equal(string.Empty, actual);

            actual = string.Empty.Left(0);
            Assert.Equal(string.Empty, actual);

            actual = string.Empty.Left(1);
            Assert.Equal(string.Empty, actual);

            // A
            actual = "A".Left(-1);
            Assert.Equal(string.Empty, actual);

            actual = "A".Left(0);
            Assert.Equal(string.Empty, actual);

            actual = "A".Left(1);
            Assert.Equal("A", actual);

            actual = "A".Left(2);
            Assert.Equal("A", actual);

            // AB
            actual = "AB".Left(-1);
            Assert.Equal(string.Empty, actual);

            actual = "AB".Left(0);
            Assert.Equal(string.Empty, actual);

            actual = "AB".Left(1);
            Assert.Equal("A", actual);

            // AAA...
            for (var len = 0; len <= 10; len++)
            {
                string s = null;
                for (var i = -1; i < 10; i++)
                {
                    var ln = System.Math.Max(0, System.Math.Min(i - 1, len));
                    var expected = string.IsNullOrEmpty(s) ? s : s.Substring(0, ln);

                    actual = s.Left(len);
                    Assert.Equal(expected, actual);

                    s = (s ?? string.Empty).PadRight(i <= 0 ? 0 : i, 'a');
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "StringExtensions Right")]
        public static void When_right_string()
        {
            // Null
            var actual = ((string)null).Right(-1);
            Assert.Null(actual);

            actual = ((string)null).Right(0);
            Assert.Null(actual);

            actual = ((string)null).Right(1);
            Assert.Null(actual);

            // Empty
            actual = string.Empty.Right(-1);
            Assert.Equal(string.Empty, actual);

            actual = string.Empty.Right(0);
            Assert.Equal(string.Empty, actual);

            actual = string.Empty.Right(1);
            Assert.Equal(string.Empty, actual);

            // A
            actual = "A".Right(-1);
            Assert.Equal(string.Empty, actual);

            actual = "A".Right(0);
            Assert.Equal(string.Empty, actual);

            actual = "A".Right(1);
            Assert.Equal("A", actual);

            actual = "A".Right(2);
            Assert.Equal("A", actual);

            // AB
            actual = "AB".Right(-1);
            Assert.Equal(string.Empty, actual);

            actual = "AB".Right(0);
            Assert.Equal(string.Empty, actual);

            actual = "AB".Right(1);
            Assert.Equal("B", actual);

            // AAA...
            for (var len = 0; len <= 10; len++)
            {
                string s = null;
                for (var i = -1; i < 10; i++)
                {
                    var ln = System.Math.Max(0, System.Math.Min(i - 1, len));
                    var expected = string.IsNullOrEmpty(s) ? s : s.Substring(0, ln);

                    actual = s.Right(len);
                    Assert.Equal(expected, actual);

                    s = (s ?? string.Empty).PadRight(i <= 0 ? 0 : i, 'a');
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "StringExtensions Elide Null")]
        public static void When_elide_string_null()
        {
            for (var totalWidth = -1; totalWidth < 10; totalWidth++)
            {
                var actual = ((string)null).Elide(totalWidth);

                Assert.Null(actual);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "StringExtensions Elide Empty")]
        public static void When_elide_string_empty()
        {
            for (var totalWidth = -1; totalWidth < 10; totalWidth++)
            {
                var actual = string.Empty.Elide(totalWidth);

                Assert.Equal(string.Empty, actual);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "StringExtensions Elide 4-")]
        public static void When_elide_string_lte_4()
        {
            var tests = new[] { "A", "AB", "ABC", "ABCD" };

            foreach (var test in tests)
            {
                for (var totalWidth = -1; totalWidth < 10; totalWidth++)
                {
                    var expected = test;
                    var actual = test.Elide(totalWidth);

                    Assert.Equal(expected.Length, actual.Length);
                    Assert.Equal(expected, actual);
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "StringExtensions Elide 5+")]
        public static void When_elide_string_gte_5()
        {
            var tests = new[] { "ABCDE", "ABCDEF", "ABCDEFG", "ABCDEFGHIJKLMNOP", LongStr };

            foreach (var test in tests)
            {
                for (var totalWidth = -1; totalWidth <= 4; totalWidth++)
                {
                    var expected = test;
                    var actual = test.Elide(totalWidth);

                    Assert.Equal(expected.Length, actual.Length);
                    Assert.Equal(expected, actual);
                }

                for (var totalWidth = 5; totalWidth < test.Length + 10; totalWidth++)
                {
                    var expected = test.Length <= totalWidth ? test : test.Left(totalWidth - 3) + @"...";
                    var actual = test.Elide(totalWidth);

                    Assert.Equal(expected.Length, actual.Length);
                    Assert.Equal(expected, actual);
                }
            }
        }
    }
}

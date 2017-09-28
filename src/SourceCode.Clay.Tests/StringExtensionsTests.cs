using System;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class StringExtensionsTests
    {
        private const string LongStr = @"From Wikipedia: Astley was born on 6 February 1966 in Newton-le-Willows in Lancashire, the fourth child of his family. His parents divorced when he was five, and Astley was brought up by his father.[9] His musical career started when he was ten, singing in the local church choir.[10] During his schooldays, Astley formed and played the drums in a number of local bands, where he met guitarist David Morris.[2][11] After leaving school at sixteen, Astley was employed during the day as a driver in his father's market-gardening business and played drums on the Northern club circuit at night in bands such as Give Way – specialising in covering Beatles and Shadows songs – and FBI, which won several local talent competitions.[10]";
        private const string surrogateChar = "\uD869\uDE01";

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

        // Narrow-1
        [InlineData("A", -1, 1)]
        [InlineData("A", 0, 1)]
        [InlineData("A", 1, 1)]
        [InlineData("A", 2, 1)]
        // Wide-1
        [InlineData(surrogateChar, -1, 2)]
        [InlineData(surrogateChar, 0, 2)]
        [InlineData(surrogateChar, 1, 2)]
        [InlineData(surrogateChar, 2, 2)]
        [InlineData(surrogateChar, 3, 2)]
        // Narrow-2
        [InlineData("AB", -1, 2)]
        [InlineData("AB", 0, 2)]
        [InlineData("AB", 1, 2)]
        [InlineData("AB", 2, 2)]
        [InlineData("AB", 3, 2)]
        // Wide-2
        [InlineData(surrogateChar + surrogateChar, -1, 4)]
        [InlineData(surrogateChar + surrogateChar, 0, 4)]
        [InlineData(surrogateChar + surrogateChar, 1, 4)]
        [InlineData(surrogateChar + surrogateChar, 2, 4)]
        [InlineData(surrogateChar + surrogateChar, 3, 3)]
        [InlineData(surrogateChar + surrogateChar, 4, 4)]
        [InlineData(surrogateChar + surrogateChar, 5, 4)]
        // Narrow-3
        [InlineData("ABC", -1, 3)]
        [InlineData("ABC", 0, 3)]
        [InlineData("ABC", 1, 3)]
        [InlineData("ABC", 2, 3)]
        [InlineData("ABC", 3, 3)]
        [InlineData("ABC", 4, 3)]
        // Wide-3
        [InlineData(surrogateChar + surrogateChar + surrogateChar, -1, 6)]
        [InlineData(surrogateChar + surrogateChar + surrogateChar, 0, 6)]
        [InlineData(surrogateChar + surrogateChar + surrogateChar, 1, 6)]
        [InlineData(surrogateChar + surrogateChar + surrogateChar, 2, 6)]
        [InlineData(surrogateChar + surrogateChar + surrogateChar, 3, 3)]
        [InlineData(surrogateChar + surrogateChar + surrogateChar, 4, 3)]
        [InlineData(surrogateChar + surrogateChar + surrogateChar, 5, 5)]
        [InlineData(surrogateChar + surrogateChar + surrogateChar, 6, 6)]
        [InlineData(surrogateChar + surrogateChar + surrogateChar, 7, 6)]
        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(When_elide_string_boundary))]
        public static void When_elide_string_boundary(string str, int totalWidth, int expected)
        {
            var actual = str.Elide(totalWidth);

            Assert.Equal(expected, actual.Length);
        }

        [InlineData(null, null, true)]
        [InlineData(null, "", false)]
        [InlineData(null, "a", false)]
        [InlineData("a", "a", true)]
        [InlineData(LongStr, LongStr, true)]
        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(When_String_EqualsOrdinal))]
        public static void When_String_EqualsOrdinal(string x, string y, bool expected)
        {
            // Forward
            var actual = x.EqualsOrdinal(y);
            Assert.Equal(expected, actual);

            var exp = StringComparer.Ordinal.Equals(x, y);
            Assert.Equal(exp, actual);

            // Reverse
            actual = y.EqualsOrdinal(x);
            Assert.Equal(expected, actual);

            exp = StringComparer.Ordinal.Equals(y, x);
            Assert.Equal(exp, actual);
        }
    }
}

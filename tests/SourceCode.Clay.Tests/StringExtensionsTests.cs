#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class StringExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact]
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

            // ABC
            actual = "ABC".Left(-1);
            Assert.Equal(string.Empty, actual);

            actual = "ABC".Left(0);
            Assert.Equal(string.Empty, actual);

            actual = "ABC".Left(1);
            Assert.Equal("A", actual);

            actual = "ABC".Left(2);
            Assert.Equal("AB", actual);

            // AAA...
            for (var len = 0; len <= 10; len++)
            {
                string s = null;
                for (var i = -1; i < 10; i++)
                {
                    var ln = Math.Max(0, Math.Min(i - 1, len));
                    var expected = string.IsNullOrEmpty(s) ? s : s.Substring(0, ln);

                    actual = s.Left(len);
                    Assert.Equal(expected, actual);

                    s = (s ?? string.Empty).PadRight(i <= 0 ? 0 : i, 'a');
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
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

            // ABC
            actual = "ABC".Right(-1);
            Assert.Equal(string.Empty, actual);

            actual = "ABC".Right(0);
            Assert.Equal(string.Empty, actual);

            actual = "ABC".Right(1);
            Assert.Equal("C", actual);

            actual = "ABC".Right(2);
            Assert.Equal("BC", actual);

            // AAA...
            for (var len = 0; len <= 10; len++)
            {
                string s = null;
                for (var i = -1; i < 10; i++)
                {
                    var ln = Math.Max(0, Math.Min(i - 1, len));
                    var expected = string.IsNullOrEmpty(s) ? s : s.Substring(0, ln);

                    actual = s.Right(len);
                    Assert.Equal(expected, actual);

                    s = (s ?? string.Empty).PadRight(i <= 0 ? 0 : i, 'a');
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_elide_string_null()
        {
            for (var totalWidth = -1; totalWidth < 10; totalWidth++)
            {
                var actual = ((string)null).Elide(totalWidth);

                Assert.Null(actual);
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_elide_string_empty()
        {
            for (var totalWidth = -1; totalWidth < 10; totalWidth++)
            {
                var actual = string.Empty.Elide(totalWidth);

                Assert.Equal(string.Empty, actual);
            }
        }

        [Trait("Type", "Unit")]
        [Theory]
        // Narrow-1
        [InlineData("A", -1, 1)]
        [InlineData("A", 0, 1)]
        [InlineData("A", 1, 1)]
        [InlineData("A", 2, 1)]
        // Wide-1
        [InlineData(TestConstants.SurrogatePair, -1, 2)]
        [InlineData(TestConstants.SurrogatePair, 0, 2)]
        [InlineData(TestConstants.SurrogatePair, 1, 2)]
        [InlineData(TestConstants.SurrogatePair, 2, 2)]
        [InlineData(TestConstants.SurrogatePair, 3, 2)]
        // Narrow-2
        [InlineData("AB", -1, 2)]
        [InlineData("AB", 0, 2)]
        [InlineData("AB", 1, 2)]
        [InlineData("AB", 2, 2)]
        [InlineData("AB", 3, 2)]
        // Wide-2
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair, -1, 4)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair, 0, 4)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair, 1, 4)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair, 2, 4)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair, 3, 3)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair, 4, 4)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair, 5, 4)]
        // Narrow-3 TestVectors.
        [InlineData("ABC", -1, 3)]
        [InlineData("ABC", 0, 3)]
        [InlineData("ABC", 1, 3)]
        [InlineData("ABC", 2, 3)]
        [InlineData("ABC", 3, 3)]
        [InlineData("ABC", 4, 3)]
        // Wide-3
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair + TestConstants.SurrogatePair, -1, 6)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair + TestConstants.SurrogatePair, 0, 6)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair + TestConstants.SurrogatePair, 1, 6)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair + TestConstants.SurrogatePair, 2, 6)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair + TestConstants.SurrogatePair, 3, 3)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair + TestConstants.SurrogatePair, 4, 3)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair + TestConstants.SurrogatePair, 5, 5)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair + TestConstants.SurrogatePair, 6, 6)]
        [InlineData(TestConstants.SurrogatePair + TestConstants.SurrogatePair + TestConstants.SurrogatePair, 7, 6)]
        public static void When_elide_string_boundary(string str, int totalWidth, int expected)
        {
            var actual = str.Elide(totalWidth);

            Assert.Equal(expected, actual.Length);
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, "", false)]
        [InlineData(null, "a", false)]
        [InlineData("a", "a", true)]
        [InlineData(TestConstants.LongStr, TestConstants.LongStr, true)]
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

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "", null)]
        [InlineData("", null, "")]
        [InlineData("", "", "")]
        [InlineData(null, "a", null)]
        [InlineData("", "a", "")]
        [InlineData("a", null, "a")]
        [InlineData("a", "", "a")]
        [InlineData("a", "a", "")]
        [InlineData("aa", "a", "a")]
        [InlineData("a", "aa", "a")]
        [InlineData("ab", "a", "b")]
        [InlineData("ba", "a", "ba")]
        [InlineData("aab", "a", "ab")]
        public static void When_String_RemoveStart(string str, string prefix, string expected)
        {
            var actual = str.RemoveStart(prefix);
            Assert.Equal(expected, actual);

            if (prefix != null)
            {
                actual = str.RemoveStart(prefix.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase);
                Assert.Equal(expected, actual);
            }
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "", null)]
        [InlineData("", null, "")]
        [InlineData("", "", "")]
        [InlineData(null, "a", null)]
        [InlineData("", "a", "")]
        [InlineData("a", null, "a")]
        [InlineData("a", "", "a")]
        [InlineData("a", "a", "")]
        [InlineData("aa", "a", "a")]
        [InlineData("a", "aa", "a")]
        [InlineData("ab", "a", "ab")]
        [InlineData("ba", "a", "b")]
        [InlineData("abb", "b", "ab")]
        public static void When_String_RemoveEnd(string str, string suffix, string expected)
        {
            var actual = str.RemoveEnd(suffix);
            Assert.Equal(expected, actual);

            if (suffix != null)
            {
                actual = str.RemoveEnd(suffix.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase);
                Assert.Equal(expected, actual);
            }
        }
    }
}

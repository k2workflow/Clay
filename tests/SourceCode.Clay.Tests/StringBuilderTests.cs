#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Text;
using Xunit;

namespace SourceCode.Clay.Text.Tests
{
    public static class StringBuilderTests
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
    }
}

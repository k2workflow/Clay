#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class MemoryExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_index_of_span))]
        public static void When_index_of_span()
        {
            Assert.Equal(-1, ReadOnlySpan<char>.Empty.IndexOf('a', 0));
            Assert.Equal(-1, "A".AsSpan().IndexOf('B', 0));
            Assert.Equal(-1, "A".AsSpan().IndexOf('A', 1));

            Assert.Equal(0, "A".AsSpan().IndexOf('A', 0));
            Assert.Equal(1, "BA".AsSpan().IndexOf('A', 0));
            Assert.Equal(1, "BA".AsSpan().IndexOf('A', 1));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_left_memory))]
        public static void When_left_memory()
        {
            // Empty
            ReadOnlyMemory<char> actual = string.Empty.AsMemory().Left(-1);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = string.Empty.AsMemory().Left(0);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = string.Empty.AsMemory().Left(1);
            Assert.Equal(string.Empty, new string(actual.Span));

            // A
            actual = "A".AsMemory().Left(-1);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "A".AsMemory().Left(0);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "A".AsMemory().Left(1);
            Assert.Equal("A", new string(actual.Span));

            actual = "A".AsMemory().Left(2);
            Assert.Equal("A", new string(actual.Span));

            // AB
            actual = "AB".AsMemory().Left(-1);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "AB".AsMemory().Left(0);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "AB".AsMemory().Left(1);
            Assert.Equal("A", new string(actual.Span));

            // ABC
            actual = "ABC".AsMemory().Left(-1);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "ABC".AsMemory().Left(0);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "ABC".AsMemory().Left(1);
            Assert.Equal("A", new string(actual.Span));

            actual = "ABC".AsMemory().Left(2);
            Assert.Equal("AB", new string(actual.Span));

            // AAA...
            for (int len = 0; len <= 10; len++)
            {
                string s = string.Empty;
                for (int i = -1; i < 10; i++)
                {
                    int ln = Math.Max(0, Math.Min(i - 1, len));
                    string expected = string.IsNullOrEmpty(s) ? s : s.Substring(0, ln);

                    actual = s.AsMemory().Left(len);
                    Assert.Equal(expected, new string(actual.Span));

                    s = (s ?? string.Empty).PadRight(i <= 0 ? 0 : i, 'a');
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_right_memory))]
        public static void When_right_memory()
        {
            // Empty
            ReadOnlyMemory<char> actual = string.Empty.AsMemory().Right(-1);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = string.Empty.AsMemory().Right(0);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = string.Empty.AsMemory().Right(1);
            Assert.Equal(string.Empty, new string(actual.Span));

            // A
            actual = "A".AsMemory().Right(-1);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "A".AsMemory().Right(0);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "A".AsMemory().Right(1);
            Assert.Equal("A", new string(actual.Span));

            actual = "A".AsMemory().Right(2);
            Assert.Equal("A", new string(actual.Span));

            // AB
            actual = "AB".AsMemory().Right(-1);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "AB".AsMemory().Right(0);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "AB".AsMemory().Right(1);
            Assert.Equal("B", new string(actual.Span));

            // ABC
            actual = "ABC".AsMemory().Right(-1);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "ABC".AsMemory().Right(0);
            Assert.Equal(string.Empty, new string(actual.Span));

            actual = "ABC".AsMemory().Right(1);
            Assert.Equal("C", new string(actual.Span));

            actual = "ABC".AsMemory().Right(2);
            Assert.Equal("BC", new string(actual.Span));

            // AAA...
            for (int len = 0; len <= 10; len++)
            {
                string s = string.Empty;
                for (int i = -1; i < 10; i++)
                {
                    int ln = Math.Max(0, Math.Min(i - 1, len));
                    string expected = string.IsNullOrEmpty(s) ? s : s.Substring(0, ln);

                    actual = s.AsMemory().Right(len);
                    Assert.Equal(expected, new string(actual.Span));

                    s = (s ?? string.Empty).PadRight(i <= 0 ? 0 : i, 'a');
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_left_span))]
        public static void When_left_span()
        {
            // Empty
            ReadOnlySpan<char> actual = string.Empty.AsSpan().Left(-1);
            Assert.Equal(string.Empty, new string(actual));

            actual = string.Empty.AsSpan().Left(0);
            Assert.Equal(string.Empty, new string(actual));

            actual = string.Empty.AsSpan().Left(1);
            Assert.Equal(string.Empty, new string(actual));

            // A
            actual = "A".AsSpan().Left(-1);
            Assert.Equal(string.Empty, new string(actual));

            actual = "A".AsSpan().Left(0);
            Assert.Equal(string.Empty, new string(actual));

            actual = "A".AsSpan().Left(1);
            Assert.Equal("A", new string(actual));

            actual = "A".AsSpan().Left(2);
            Assert.Equal("A", new string(actual));

            // AB
            actual = "AB".AsSpan().Left(-1);
            Assert.Equal(string.Empty, new string(actual));

            actual = "AB".AsSpan().Left(0);
            Assert.Equal(string.Empty, new string(actual));

            actual = "AB".AsSpan().Left(1);
            Assert.Equal("A", new string(actual));

            // ABC
            actual = "ABC".AsSpan().Left(-1);
            Assert.Equal(string.Empty, new string(actual));

            actual = "ABC".AsSpan().Left(0);
            Assert.Equal(string.Empty, new string(actual));

            actual = "ABC".AsSpan().Left(1);
            Assert.Equal("A", new string(actual));

            actual = "ABC".AsSpan().Left(2);
            Assert.Equal("AB", new string(actual));

            // AAA...
            for (int len = 0; len <= 10; len++)
            {
                string s = string.Empty;
                for (int i = -1; i < 10; i++)
                {
                    int ln = Math.Max(0, Math.Min(i - 1, len));
                    string expected = string.IsNullOrEmpty(s) ? s : s.Substring(0, ln);

                    actual = s.AsSpan().Left(len);
                    Assert.Equal(expected, new string(actual));

                    s = (s ?? string.Empty).PadRight(i <= 0 ? 0 : i, 'a');
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_right_span))]
        public static void When_right_span()
        {
            // Empty
            ReadOnlySpan<char> actual = string.Empty.AsSpan().Right(-1);
            Assert.Equal(string.Empty, new string(actual));

            actual = string.Empty.AsSpan().Right(0);
            Assert.Equal(string.Empty, new string(actual));

            actual = string.Empty.AsSpan().Right(1);
            Assert.Equal(string.Empty, new string(actual));

            // A
            actual = "A".AsSpan().Right(-1);
            Assert.Equal(string.Empty, new string(actual));

            actual = "A".AsSpan().Right(0);
            Assert.Equal(string.Empty, new string(actual));

            actual = "A".AsSpan().Right(1);
            Assert.Equal("A", new string(actual));

            actual = "A".AsSpan().Right(2);
            Assert.Equal("A", new string(actual));

            // AB
            actual = "AB".AsSpan().Right(-1);
            Assert.Equal(string.Empty, new string(actual));

            actual = "AB".AsSpan().Right(0);
            Assert.Equal(string.Empty, new string(actual));

            actual = "AB".AsSpan().Right(1);
            Assert.Equal("B", new string(actual));

            // ABC
            actual = "ABC".AsSpan().Right(-1);
            Assert.Equal(string.Empty, new string(actual));

            actual = "ABC".AsSpan().Right(0);
            Assert.Equal(string.Empty, new string(actual));

            actual = "ABC".AsSpan().Right(1);
            Assert.Equal("C", new string(actual));

            actual = "ABC".AsSpan().Right(2);
            Assert.Equal("BC", new string(actual));

            // AAA...
            for (int len = 0; len <= 10; len++)
            {
                string s = string.Empty;
                for (int i = -1; i < 10; i++)
                {
                    int ln = Math.Max(0, Math.Min(i - 1, len));
                    string expected = string.IsNullOrEmpty(s) ? s : s.Substring(0, ln);

                    actual = s.AsSpan().Right(len);
                    Assert.Equal(expected, new string(actual));

                    s = (s ?? string.Empty).PadRight(i <= 0 ? 0 : i, 'a');
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_both_null))]
        public static void MemoryEquals_both_null()
        {
            var equal = ReadOnlyMemory<string>.Empty.MemoryEquals(Memory<string>.Empty, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_both_empty))]
        public static void MemoryEquals_both_empty()
        {
            var list1 = new ReadOnlyMemory<string>(Array.Empty<string>());
            var list2 = new ReadOnlyMemory<string>(new string[0]);

            var equal = list1.MemoryEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_both_one))]
        public static void MemoryEquals_both_one()
        {
            var list1 = new ReadOnlyMemory<string>(new string[] { "hi" });
            var list2 = new ReadOnlyMemory<string>(new string[] { "HI" });
            var list3 = new ReadOnlyMemory<string>(new string[] { "bye" });

            var equal = list1.MemoryEquals(list2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = list1.MemoryEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = list1.MemoryEquals(list3, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_one_null))]
        public static void MemoryEquals_one_null()
        {
            var list = new ReadOnlyMemory<string>(TestData.List);
            var equal = list.MemoryEquals(ReadOnlyMemory<string>.Empty, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_different_count))]
        public static void MemoryEquals_different_count()
        {
            var list = new ReadOnlyMemory<string>(TestData.List);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                TestData.List[0],
                TestData.List[1],
                TestData.List[2]
            });

            var equal = list.MemoryEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_IsEqual))]
        public static void MemoryEquals_IsEqual()
        {
            var list = new ReadOnlyMemory<string>(TestData.List);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                TestData.List[0],
                TestData.List[1],
                TestData.List[2],
                TestData.List[3]
            });

            var equal = list.MemoryEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_NotEqual))]
        public static void MemoryEquals_NotEqual()
        {
            var list = new ReadOnlyMemory<string>(TestData.List);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                TestData.List[0],
                TestData.List[1],
                "a",
                TestData.List[3]
            });

            var equal = list.MemoryEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_duplicates))]
        public static void MemoryEquals_duplicates()
        {
            var list = new ReadOnlyMemory<string>(TestData.List);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                TestData.List[2],
                TestData.List[1],
                TestData.List[2],
                TestData.List[0]
            });

            var equal = list.MemoryEquals(list2);
            Assert.False(equal);
        }
    }
}

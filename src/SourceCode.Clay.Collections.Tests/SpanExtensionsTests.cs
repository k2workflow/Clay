using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class SpanExtensionsTests
    {
        private static readonly ReadOnlySpan<string> _null = new ReadOnlySpan<string>();

        private static readonly ReadOnlySpan<string> _list = new[]
        {
            "foo",
            "bar",
            "baz",
            "nin"
        }.AsReadOnlySpan();

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_both_null))]
        public static void SpanEquals_both_null()
        {
            var equal = _null.SpanEquals(Span<string>.Empty, StringComparer.Ordinal, false);
            Assert.True(equal);

            equal = _null.SpanEquals(Span<string>.Empty, StringComparer.Ordinal, true);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_both_empty))]
        public static void SpanEquals_both_empty()
        {
            var list1 = Array.Empty<string>().AsReadOnlySpan();
            var list2 = new string[0].AsReadOnlySpan();

            var equal = list1.SpanEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);

            equal = list1.SpanEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_both_one))]
        public static void SpanEquals_both_one()
        {
            var list1 = new string[] { "hi" }.AsReadOnlySpan();
            var list2 = new string[] { "HI" }.AsReadOnlySpan();
            var list3 = new string[] { "bye" }.AsReadOnlySpan();

            var equal = list1.SpanEquals(list2, StringComparer.OrdinalIgnoreCase, false);
            Assert.True(equal);

            equal = list1.SpanEquals(list2, StringComparer.OrdinalIgnoreCase, true);
            Assert.True(equal);

            equal = list1.SpanEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);

            equal = list1.SpanEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = list1.SpanEquals(list3, StringComparer.Ordinal, false);
            Assert.False(equal);

            equal = list1.SpanEquals(list3, StringComparer.Ordinal, true);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_one_null))]
        public static void SpanEquals_one_null()
        {
            var equal = _list.SpanEquals(_null, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_different_count))]
        public static void SpanEquals_different_count()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                _list[2]
            }.AsReadOnlySpan();

            var equal = _list.SpanEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_IsEqual_sequential_true))]
        public static void SpanEquals_IsEqual_sequential_true()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                _list[2],
                _list[3]
            }.AsReadOnlySpan();

            var equal = _list.SpanEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = _list.SpanEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_IsEqual_sequential_false))]
        public static void SpanEquals_IsEqual_sequential_false()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                _list[3],
                _list[0]
            }.AsReadOnlySpan();

            var equal = _list.SpanEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = _list.SpanEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_NotEqual_sequential_true))]
        public static void SpanEquals_NotEqual_sequential_true()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                "a",
                _list[3]
            }.AsReadOnlySpan();

            var equal = _list.SpanEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = _list.SpanEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_NotEqual_sequential_false))]
        public static void SpanEquals_NotEqual_sequential_false()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                "a",
                _list[0]
            }.AsReadOnlySpan();

            var equal = _list.SpanEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = _list.SpanEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_duplicates))]
        public static void SpanEquals_duplicates()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                _list[2],
                _list[0]
            }.AsReadOnlySpan();

            var equal = _list.SpanEquals(list2, true);
            Assert.False(equal);

            equal = _list.SpanEquals(list2, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_Extract_IsEqual_sequential_true))]
        public static void SpanEquals_Extract_IsEqual_sequential_true()
        {
            var listA = new[]
            {
                new KeyValuePair<int,string>(1, _list[0]),
                new KeyValuePair<int,string>(1, _list[1]),
                new KeyValuePair<int,string>(1, _list[2]),
                new KeyValuePair<int,string>(1, _list[3])
            }.AsReadOnlySpan();

            var listB = new[]
            {
                new KeyValuePair<int,string>(2, _list[0]),
                new KeyValuePair<int,string>(2, _list[1]),
                new KeyValuePair<int,string>(2, _list[2]),
                new KeyValuePair<int,string>(2, _list[3])
            }.AsReadOnlySpan();

            var equal = listA.SpanEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = listA.SpanEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SpanEquals_Extract_NotEqual_sequential_true))]
        public static void SpanEquals_Extract_NotEqual_sequential_true()
        {
            var listA = new[]
            {
                new KeyValuePair<int,string>(1, _list[0]),
                new KeyValuePair<int,string>(1, _list[1]),
                new KeyValuePair<int,string>(1, _list[2]),
                new KeyValuePair<int,string>(1, _list[3])
            }.AsReadOnlySpan();

            var listB = new[]
            {
                new KeyValuePair<int,string>(2, _list[0]),
                new KeyValuePair<int,string>(2, _list[1]),
                new KeyValuePair<int,string>(2, "a"),
                new KeyValuePair<int,string>(2, _list[3])
            }.AsReadOnlySpan();

            var equal = listA.SpanEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = listA.SpanEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.False(equal);
        }
    }
}

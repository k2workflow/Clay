using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class MemoryExtensionsTests
    {
        private static readonly ReadOnlyMemory<string> _null = new ReadOnlyMemory<string>();

        private static readonly string[] _list = new[]
        {
            "foo",
            "bar",
            "baz",
            "nin"
        };

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_both_null))]
        public static void MemoryEquals_both_null()
        {
            var equal = _null.MemoryEquals(Memory<string>.Empty, StringComparer.Ordinal, false);
            Assert.True(equal);

            equal = _null.MemoryEquals(Memory<string>.Empty, StringComparer.Ordinal, true);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_both_empty))]
        public static void MemoryEquals_both_empty()
        {
            var list1 = new ReadOnlyMemory<string>(Array.Empty<string>());
            var list2 = new ReadOnlyMemory<string>(new string[0]);

            var equal = list1.MemoryEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);

            equal = list1.MemoryEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_both_one))]
        public static void MemoryEquals_both_one()
        {
            var list1 = new ReadOnlyMemory<string>(new string[] { "hi" });
            var list2 = new ReadOnlyMemory<string>(new string[] { "HI" });
            var list3 = new ReadOnlyMemory<string>(new string[] { "bye" });

            var equal = list1.MemoryEquals(list2, StringComparer.OrdinalIgnoreCase, false);
            Assert.True(equal);

            equal = list1.MemoryEquals(list2, StringComparer.OrdinalIgnoreCase, true);
            Assert.True(equal);

            equal = list1.MemoryEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);

            equal = list1.MemoryEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = list1.MemoryEquals(list3, StringComparer.Ordinal, false);
            Assert.False(equal);

            equal = list1.MemoryEquals(list3, StringComparer.Ordinal, true);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_one_null))]
        public static void MemoryEquals_one_null()
        {
            var list = new ReadOnlyMemory<string>(_list);
            var equal = list.MemoryEquals(_null, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_different_count))]
        public static void MemoryEquals_different_count()
        {
            var list = new ReadOnlyMemory<string>(_list);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                _list[0],
                _list[1],
                _list[2]
            });

            var equal = list.MemoryEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_IsEqual_sequential_true))]
        public static void MemoryEquals_IsEqual_sequential_true()
        {
            var list = new ReadOnlyMemory<string>(_list);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                _list[0],
                _list[1],
                _list[2],
                _list[3]
            });

            var equal = list.MemoryEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = list.MemoryEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_IsEqual_sequential_false))]
        public static void MemoryEquals_IsEqual_sequential_false()
        {
            var list = new ReadOnlyMemory<string>(_list);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                _list[2],
                _list[1],
                _list[3],
                _list[0]
            });

            var equal = list.MemoryEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = list.MemoryEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_NotEqual_sequential_true))]
        public static void MemoryEquals_NotEqual_sequential_true()
        {
            var list = new ReadOnlyMemory<string>(_list);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                _list[0],
                _list[1],
                "a",
                _list[3]
            });

            var equal = list.MemoryEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = list.MemoryEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_NotEqual_sequential_false))]
        public static void MemoryEquals_NotEqual_sequential_false()
        {
            var list = new ReadOnlyMemory<string>(_list);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                _list[2],
                _list[1],
                "a",
                _list[0]
            });

            var equal = list.MemoryEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = list.MemoryEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_duplicates))]
        public static void MemoryEquals_duplicates()
        {
            var list = new ReadOnlyMemory<string>(_list);
            var list2 = new ReadOnlyMemory<string>(new[]
            {
                _list[2],
                _list[1],
                _list[2],
                _list[0]
            });

            var equal = list.MemoryEquals(list2, true);
            Assert.False(equal);

            equal = list.MemoryEquals(list2, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_Extract_IsEqual_sequential_true))]
        public static void MemoryEquals_Extract_IsEqual_sequential_true()
        {
            var listA = new ReadOnlyMemory<KeyValuePair<int, string>>(new[]
            {
                new KeyValuePair<int,string>(1, _list[0]),
                new KeyValuePair<int,string>(1, _list[1]),
                new KeyValuePair<int,string>(1, _list[2]),
                new KeyValuePair<int,string>(1, _list[3])
            });

            var listB = new ReadOnlyMemory<KeyValuePair<int, string>>(new[]
            {
                new KeyValuePair<int,string>(2, _list[0]),
                new KeyValuePair<int,string>(2, _list[1]),
                new KeyValuePair<int,string>(2, _list[2]),
                new KeyValuePair<int,string>(2, _list[3])
            });

            var equal = listA.MemoryEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = listA.MemoryEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryEquals_Extract_NotEqual_sequential_true))]
        public static void MemoryEquals_Extract_NotEqual_sequential_true()
        {
            var listA = new ReadOnlyMemory<KeyValuePair<int, string>>(new[]
            {
                new KeyValuePair<int,string>(1, _list[0]),
                new KeyValuePair<int,string>(1, _list[1]),
                new KeyValuePair<int,string>(1, _list[2]),
                new KeyValuePair<int,string>(1, _list[3])
            });

            var listB = new ReadOnlyMemory<KeyValuePair<int, string>>(new[]
            {
                new KeyValuePair<int,string>(2, _list[0]),
                new KeyValuePair<int,string>(2, _list[1]),
                new KeyValuePair<int,string>(2, "a"),
                new KeyValuePair<int,string>(2, _list[3])
            });

            var equal = listA.MemoryEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = listA.MemoryEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.False(equal);
        }
    }
}

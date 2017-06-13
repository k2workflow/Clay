using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class ListExtensionsFixtures
    {
        private static readonly string[] _list =
        {
            "foo",
            "bar",
            "baz",
            "nin"
        };

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals null, null")]
        public static void Use_FastEquals_both_null()
        {
            var equal = ((string[])null).FastEquals((string[])null, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals list, null")]
        public static void Use_FastEquals_one_null()
        {
            var equal = _list.FastEquals((string[])null, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals N, M")]
        public static void Use_FastEquals_different_count()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                _list[2]
            };

            var equal = _list.FastEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals IsEqual colocated true")]
        public static void Use_FastEquals_IsEqual_colocated_true()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                _list[2],
                _list[3]
            };

            var equal = _list.FastEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = _list.FastEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals IsEqual colocated false")]
        public static void Use_FastEquals_IsEqual_colocated_false()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                _list[3],
                _list[0]
            };

            var equal = _list.FastEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = _list.FastEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals NotEqual colocated true")]
        public static void Use_FastEquals_NotEqual_colocated_true()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                "a",
                _list[3]
            };

            var equal = _list.FastEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = _list.FastEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals NotEqual colocated false")]
        public static void Use_FastEquals_NotEqual_colocated_false()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                "a",
                _list[0]
            };

            var equal = _list.FastEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = _list.FastEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals duplicates")]
        public static void Use_FastEquals_duplicates()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                _list[2],
                _list[0]
            };

            var equal = _list.FastEquals(list2, true);
            Assert.False(equal);

            equal = _list.FastEquals(list2, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals IsEqual colocated transform")]
        public static void Use_FastEquals_Transform_IsEqual_colocated_true()
        {
            var listA = new[]
            {
                new KeyValuePair<int,string>(1, _list[0]),
                new KeyValuePair<int,string>(1, _list[1]),
                new KeyValuePair<int,string>(1, _list[2]),
                new KeyValuePair<int,string>(1, _list[3])
            };

            var listB = new[]
           {
                new KeyValuePair<int,string>(2, _list[0]),
                new KeyValuePair<int,string>(2, _list[1]),
                new KeyValuePair<int,string>(2, _list[2]),
                new KeyValuePair<int,string>(2, _list[3])
            };

            var equal = listA.FastEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = listA.FastEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions FastEquals NotEqual colocated transform")]
        public static void Use_FastEquals_Transform_NotEqual_colocated_true()
        {
            var listA = new[]
            {
                new KeyValuePair<int,string>(1, _list[0]),
                new KeyValuePair<int,string>(1, _list[1]),
                new KeyValuePair<int,string>(1, _list[2]),
                new KeyValuePair<int,string>(1, _list[3])
            };

            var listB = new[]
           {
                new KeyValuePair<int,string>(2, _list[0]),
                new KeyValuePair<int,string>(2, _list[1]),
                new KeyValuePair<int,string>(2, "a"),
                new KeyValuePair<int,string>(2, _list[3])
            };

            var equal = listA.FastEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = listA.FastEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.False(equal);
        }
    }
}

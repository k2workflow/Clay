using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class ListExtensionsTests
    {
        private static readonly string[] _list =
        {
            "foo",
            "bar",
            "baz",
            "nin"
        };

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals null, null")]
        public static void Use_NullableEquals_both_null()
        {
            var equal = ((string[])null).NullableEquals(null, StringComparer.Ordinal, false);
            Assert.True(equal);

            equal = ((string[])null).NullableEquals(null, StringComparer.Ordinal, true);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals 0, 0")]
        public static void Use_NullableEquals_both_empty()
        {
            var list1 = Array.Empty<string>();
            var list2 = new string[0];

            var equal = list1.NullableEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);

            equal = list1.NullableEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals 1, 1")]
        public static void Use_NullableEquals_both_one()
        {
            var list1 = new string[] { "hi" };
            var list2 = new string[] { "HI" };
            var list3 = new string[] { "bye" };

            var equal = list1.NullableEquals(list2, StringComparer.OrdinalIgnoreCase, false);
            Assert.True(equal);

            equal = list1.NullableEquals(list2, StringComparer.OrdinalIgnoreCase, true);
            Assert.True(equal);

            equal = list1.NullableEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);

            equal = list1.NullableEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = list1.NullableEquals(list3, StringComparer.Ordinal, false);
            Assert.False(equal);

            equal = list1.NullableEquals(list3, StringComparer.Ordinal, true);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals list, null")]
        public static void Use_NullableEquals_one_null()
        {
            var equal = _list.NullableEquals((string[])null, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals N, M")]
        public static void Use_NullableEquals_different_count()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                _list[2]
            };

            var equal = _list.NullableEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals IsEqual colocated true")]
        public static void Use_NullableEquals_IsEqual_colocated_true()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                _list[2],
                _list[3]
            };

            var equal = _list.NullableEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = _list.NullableEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals IsEqual colocated false")]
        public static void Use_NullableEquals_IsEqual_colocated_false()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                _list[3],
                _list[0]
            };

            var equal = _list.NullableEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = _list.NullableEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals NotEqual colocated true")]
        public static void Use_NullableEquals_NotEqual_colocated_true()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                "a",
                _list[3]
            };

            var equal = _list.NullableEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = _list.NullableEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals NotEqual colocated false")]
        public static void Use_NullableEquals_NotEqual_colocated_false()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                "a",
                _list[0]
            };

            var equal = _list.NullableEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = _list.NullableEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals duplicates")]
        public static void Use_NullableEquals_duplicates()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                _list[2],
                _list[0]
            };

            var equal = _list.NullableEquals(list2, true);
            Assert.False(equal);

            equal = _list.NullableEquals(list2, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals IsEqual colocated transform")]
        public static void Use_NullableEquals_Transform_IsEqual_colocated_true()
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

            var equal = listA.NullableEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = listA.NullableEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ListExtensions NullableEquals NotEqual colocated transform")]
        public static void Use_NullableEquals_Transform_NotEqual_colocated_true()
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

            var equal = listA.NullableEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = listA.NullableEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.False(equal);
        }
    }
}

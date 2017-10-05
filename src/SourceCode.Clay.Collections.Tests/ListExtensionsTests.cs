#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class ListExtensionsTests
    {
        #region Fields

        private static readonly string[] _null = null;

        private static readonly string[] _list =
        {
            "foo",
            "bar",
            "baz",
            "nin"
        };

        #endregion

        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_both_null))]
        public static void ListEquals_both_null()
        {
            var equal = _null.ListEquals(null, StringComparer.Ordinal, false);
            Assert.True(equal);

            equal = _null.ListEquals(null, StringComparer.Ordinal, true);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_both_empty))]
        public static void ListEquals_both_empty()
        {
            var list1 = Array.Empty<string>();
            var list2 = new string[0];

            var equal = list1.ListEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);

            equal = list1.ListEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_both_one))]
        public static void ListEquals_both_one()
        {
            var list1 = new string[] { "hi" };
            var list2 = new string[] { "HI" };
            var list3 = new string[] { "bye" };

            var equal = list1.ListEquals(list2, StringComparer.OrdinalIgnoreCase, false);
            Assert.True(equal);

            equal = list1.ListEquals(list2, StringComparer.OrdinalIgnoreCase, true);
            Assert.True(equal);

            equal = list1.ListEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);

            equal = list1.ListEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = list1.ListEquals(list3, StringComparer.Ordinal, false);
            Assert.False(equal);

            equal = list1.ListEquals(list3, StringComparer.Ordinal, true);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_one_null))]
        public static void ListEquals_one_null()
        {
            var equal = _list.ListEquals((string[])null, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_different_count))]
        public static void ListEquals_different_count()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                _list[2]
            };

            var equal = _list.ListEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_IsEqual_sequential_true))]
        public static void ListEquals_IsEqual_sequential_true()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                _list[2],
                _list[3]
            };

            var equal = _list.ListEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = _list.ListEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_IsEqual_sequential_false))]
        public static void ListEquals_IsEqual_sequential_false()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                _list[3],
                _list[0]
            };

            var equal = _list.ListEquals(list2, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = _list.ListEquals(list2, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_NotEqual_sequential_true))]
        public static void ListEquals_NotEqual_sequential_true()
        {
            var list2 = new[]
            {
                _list[0],
                _list[1],
                "a",
                _list[3]
            };

            var equal = _list.ListEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = _list.ListEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_NotEqual_sequential_false))]
        public static void ListEquals_NotEqual_sequential_false()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                "a",
                _list[0]
            };

            var equal = _list.ListEquals(list2, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = _list.ListEquals(list2, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_duplicates))]
        public static void ListEquals_duplicates()
        {
            var list2 = new[]
            {
                _list[2],
                _list[1],
                _list[2],
                _list[0]
            };

            var equal = _list.ListEquals(list2, true);
            Assert.False(equal);

            equal = _list.ListEquals(list2, false);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_Extract_IsEqual_sequential_true))]
        public static void ListEquals_Extract_IsEqual_sequential_true()
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

            var equal = listA.ListEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.True(equal);

            equal = listA.ListEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_Extract_NotEqual_sequential_true))]
        public static void ListEquals_Extract_NotEqual_sequential_true()
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

            var equal = listA.ListEquals(listB, n => n.Value, StringComparer.Ordinal, true);
            Assert.False(equal);

            equal = listA.ListEquals(listB, n => n.Value, StringComparer.Ordinal, false);
            Assert.False(equal);
        }

        #endregion
    }
}

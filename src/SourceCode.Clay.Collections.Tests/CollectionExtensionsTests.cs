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
    public static class CollectionExtensionsTests
    {
        #region Constants

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
            var equal = ((ICollection<string>)_null).NullableCollectionEquals(null, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)_null).NullableCollectionEquals(null, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_both_empty))]
        public static void ListEquals_both_empty()
        {
            var list1 = Array.Empty<string>();
            var list2 = new string[0];

            var equal = ((ICollection<string>)list1).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)list1).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_both_one))]
        public static void ListEquals_both_one()
        {
            var list1 = new string[] { "hi" };
            var list2 = new string[] { "HI" };
            var list3 = new string[] { "bye" };

            var equal = ((ICollection<string>)list1).NullableCollectionEquals(list2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)list1).NullableCollectionEquals(list2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = ((ICollection<string>)list1).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)list1).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((ICollection<string>)list1).NullableCollectionEquals(list3, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)list1).NullableCollectionEquals(list3, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_one_null))]
        public static void ListEquals_one_null()
        {
            var equal = ((ICollection<string>)_list).NullableCollectionEquals(null, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)_list).NullableCollectionEquals(null, StringComparer.Ordinal);
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

            var equal = ((ICollection<string>)_list).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)_list).NullableCollectionEquals(list2, StringComparer.Ordinal);
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

            var equal = ((ICollection<string>)_list).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)_list).NullableCollectionEquals(list2, StringComparer.Ordinal);
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

            var equal = ((ICollection<string>)_list).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)_list).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ListEquals_IsEqual_sequential_false_duplicates))]
        public static void ListEquals_IsEqual_sequential_false_duplicates()
        {
            var list1 = new[]
            {
                _list[2],
                _list[0], // Duplicate
                _list[1],
                _list[3],
                _list[0]
            };

            var list2 = new[]
            {
                _list[2],
                _list[1],
                _list[3],
                _list[0], // Duplicate
                _list[0]
            };

            var equal = ((ICollection<string>)list1).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)list1).NullableSetEquals(list2, StringComparer.Ordinal);
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

            var equal = ((ICollection<string>)_list).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)_list).NullableCollectionEquals(list2, StringComparer.Ordinal);
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

            var equal = ((ICollection<string>)_list).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)_list).NullableCollectionEquals(list2, StringComparer.Ordinal);
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

            var equal = ((ICollection<string>)_list).NullableCollectionEquals(list2);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)_list).NullableCollectionEquals(list2);
            Assert.False(equal);
        }

        #endregion
    }
}

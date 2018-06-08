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
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(CollectionEquals_both_null))]
        public static void CollectionEquals_both_null()
        {
            var equal = ((ICollection<string>)TestData.Null).NullableCollectionEquals(null, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)TestData.Null).NullableCollectionEquals(null, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(CollectionEquals_both_empty))]
        public static void CollectionEquals_both_empty()
        {
            var list1 = Array.Empty<string>();
            var list2 = new string[0];

            var equal = ((ICollection<string>)list1).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)list1).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(CollectionEquals_both_one))]
        public static void CollectionEquals_both_one()
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
        [Fact(DisplayName = nameof(CollectionEquals_one_null))]
        public static void CollectionEquals_one_null()
        {
            var equal = ((ICollection<string>)TestData.List).NullableCollectionEquals(null, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)TestData.List).NullableCollectionEquals(null, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(CollectionEquals_different_count))]
        public static void CollectionEquals_different_count()
        {
            var list2 = new[]
            {
                TestData.List[0],
                TestData.List[1],
                TestData.List[2]
            };

            var equal = ((ICollection<string>)TestData.List).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)TestData.List).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(CollectionEquals_is_equal))]
        public static void CollectionEquals_is_equal()
        {
            var list2 = new[]
            {
                TestData.List[0],
                TestData.List[1],
                TestData.List[2],
                TestData.List[3]
            };

            var equal = ((ICollection<string>)TestData.List).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)TestData.List).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(CollectionEquals_not_equal_1))]
        public static void CollectionEquals_not_equal_1()
        {
            var list2 = new[]
            {
                TestData.List[0],
                TestData.List[1],
                "a",
                TestData.List[3]
            };

            var equal = ((ICollection<string>)TestData.List).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)TestData.List).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(CollectionEquals_not_equal_2))]
        public static void CollectionEquals_not_equal_2()
        {
            var list2 = new[]
            {
                TestData.List[2],
                TestData.List[1],
                "a",
                TestData.List[0]
            };

            var equal = ((IList<string>)TestData.List).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyList<string>)TestData.List).NullableCollectionEquals(list2, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(CollectionEquals_duplicates))]
        public static void CollectionEquals_duplicates()
        {
            var list2 = new[]
            {
                TestData.List[2],
                TestData.List[1],
                TestData.List[2],
                TestData.List[0]
            };

            var equal = ((ICollection<string>)TestData.List).NullableCollectionEquals(list2);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)TestData.List).NullableCollectionEquals(list2);
            Assert.False(equal);
        }
    }
}

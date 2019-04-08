#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using SourceCode.Clay.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class ListExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact]
        public static void ListEquals_both_null()
        {
            var equal = ((IList<string>)TestData.Null).NullableSequenceEqual(null, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyList<string>)TestData.Null).NullableSequenceEqual(null, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void ListEquals_both_empty()
        {
            var list1 = Array.Empty<string>();
            var list2 = new string[0];

            var equal = ((IList<string>)list1).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyList<string>)list1).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void ListEquals_both_one()
        {
            var list1 = new string[] { "hi" };
            var list2 = new string[] { "HI" };
            var list3 = new string[] { "bye" };

            var equal = ((IList<string>)list1).NullableSequenceEqual(list2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = ((IReadOnlyList<string>)list1).NullableSequenceEqual(list2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = ((IList<string>)list1).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyList<string>)list1).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IList<string>)list1).NullableSequenceEqual(list3, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyList<string>)list1).NullableSequenceEqual(list3, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void ListEquals_one_null()
        {
            var equal = ((IList<string>)TestData.List).NullableSequenceEqual(null, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyList<string>)TestData.List).NullableSequenceEqual(null, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void ListEquals_different_count()
        {
            var list2 = new[]
            {
                TestData.List[0],
                TestData.List[1],
                TestData.List[2]
            };

            var equal = ((IList<string>)TestData.List).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyList<string>)TestData.List).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void ListEquals_is_equal()
        {
            var list2 = new[]
            {
                TestData.List[0],
                TestData.List[1],
                TestData.List[2],
                TestData.List[3]
            };

            var equal = ((IList<string>)TestData.List).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyList<string>)TestData.List).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void ListEquals_not_equal_1()
        {
            var list2 = new[]
            {
                TestData.List[0],
                TestData.List[1],
                "a",
                TestData.List[3]
            };

            var equal = ((IList<string>)TestData.List).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyList<string>)TestData.List).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void ListEquals_not_equal_2()
        {
            var list2 = new[]
            {
                TestData.List[2],
                TestData.List[1],
                "a",
                TestData.List[0]
            };

            var equal = ((IList<string>)TestData.List).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyList<string>)TestData.List).NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void ListEquals_duplicates()
        {
            var list2 = new[]
            {
                TestData.List[2],
                TestData.List[1],
                TestData.List[2],
                TestData.List[0]
            };

            var equal = ((IList<string>)TestData.List).NullableSequenceEqual(list2);
            Assert.False(equal);

            equal = ((IReadOnlyList<string>)TestData.List).NullableSequenceEqual(list2);
            Assert.False(equal);
        }
    }
}

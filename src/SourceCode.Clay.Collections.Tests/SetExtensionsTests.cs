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
    public static class SetExtensionsTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_both_null))]
        public static void SetEquals_both_null()
        {
            var equal = ((HashSet<string>)null).NullableSetEquals(null);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_both_empty))]
        public static void SetEquals_both_empty()
        {
            var set1 = new HashSet<string>();
            var set2 = new HashSet<string>();

            var equal = set1.NullableSetEquals(set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_both_one))]
        public static void SetEquals_both_one()
        {
            var set1 = new HashSet<string> { "hi" };
            var set2 = new HashSet<string> { "HI" };
            var set3 = new HashSet<string> { "bye" };

            var equal = set1.NullableSetEquals(set2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = set1.NullableSetEquals(set3, StringComparer.OrdinalIgnoreCase);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_one_null))]
        public static void SetEquals_one_null()
        {
            var equal = TestData.Set.NullableSetEquals(null);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_different_count))]
        public static void SetEquals_different_count()
        {
            var set2 = new HashSet<string>(TestData.Set);
            set2.Remove("foo");

            var equal = TestData.Set.NullableSetEquals(set2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_IsEqual))]
        public static void SetEquals_IsEqual()
        {
            var set2 = new HashSet<string>(TestData.Set);

            var equal = TestData.Set.NullableSetEquals(set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_NotEqual))]
        public static void SetEquals_NotEqual()
        {
            var set2 = new HashSet<string>(TestData.Set)
            {
                "xyz"
            };

            var equal = TestData.Set.NullableSetEquals(set2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_IsEqual_sequential_false))]
        public static void SetEquals_IsEqual_sequential_false()
        {
            var list2 = new[]
            {
                TestData.List[2],
                TestData.List[1],
                TestData.List[3],
                TestData.List[0]
            };

            var equal = ((ICollection<string>)TestData.List).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)TestData.List).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IList<string>)TestData.List).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyList<string>)TestData.List).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_IsEqual_sequential_false_duplicates))]
        public static void SetEquals_IsEqual_sequential_false_duplicates()
        {
            var list1 = new[]
            {
                TestData.List[2],
                TestData.List[0], // Duplicate
                TestData.List[1],
                TestData.List[3],
                TestData.List[0]
            };

            var list2 = new[]
            {
                TestData.List[2],
                TestData.List[1],
                TestData.List[3],
                TestData.List[0], // Duplicate
                TestData.List[0]
            };

            var equal = ((ICollection<string>)list1).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)list1).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IList<string>)list1).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyList<string>)list1).NullableSetEquals(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        #endregion
    }
}

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
    public static class SetExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_both_null))]
        public static void SetEquals_both_null()
        {
            var equal = ((HashSet<string>)null).NullableSetEqual(null);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_both_empty))]
        public static void SetEquals_both_empty()
        {
            var set1 = new HashSet<string>();
            var set2 = new HashSet<string>();

            var equal = set1.NullableSetEqual(set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_both_one))]
        public static void SetEquals_both_one()
        {
            var set1 = new HashSet<string> { "hi" };
            var set2 = new HashSet<string> { "HI" };
            var set3 = new HashSet<string> { "bye" };

            var equal = set1.NullableSetEqual(set2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = set1.NullableSetEqual(set3, StringComparer.OrdinalIgnoreCase);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_one_null))]
        public static void SetEquals_one_null()
        {
            var equal = TestData.Set.NullableSetEqual(null);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_different_count))]
        public static void SetEquals_different_count()
        {
            var set2 = new HashSet<string>(TestData.Set);
            set2.Remove("foo");

            var equal = TestData.Set.NullableSetEqual(set2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_is_equal))]
        public static void SetEquals_is_equal()
        {
            var set2 = new HashSet<string>(TestData.Set);

            var equal = TestData.Set.NullableSetEqual(set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_not_equal))]
        public static void SetEquals_not_equal()
        {
            var set2 = new HashSet<string>(TestData.Set)
            {
                "xyz"
            };

            var equal = TestData.Set.NullableSetEqual(set2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_not_equal_1))]
        public static void SetEquals_not_equal_1()
        {
            var set = new[]
            {
                TestData.List[2],
                TestData.List[1],
                "awk",
                TestData.List[0]
            };

            var equal = ((ICollection<string>)TestData.List).NullableSetEqual(set, StringComparer.Ordinal);
            Assert.False(equal);

            equal = ((IReadOnlyCollection<string>)TestData.List).NullableSetEqual(set, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_is_equal_1))]
        public static void SetEquals_is_equal_1()
        {
            var list2 = new[]
            {
                TestData.List[2],
                TestData.List[1],
                TestData.List[3],
                TestData.List[0]
            };

            var equal = ((ICollection<string>)TestData.List).NullableSetEqual(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)TestData.List).NullableSetEqual(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IList<string>)TestData.List).NullableSetEqual(list2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyList<string>)TestData.List).NullableSetEqual(list2, StringComparer.Ordinal);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_is_equal_duplicates))]
        public static void SetEquals_is_equal_duplicates()
        {
            var equal = ((ICollection<string>)TestData.Dupe1).NullableSetEqual(TestData.Dupe2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyCollection<string>)TestData.Dupe1).NullableSetEqual(TestData.Dupe2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IList<string>)TestData.Dupe1).NullableSetEqual(TestData.Dupe2, StringComparer.Ordinal);
            Assert.True(equal);

            equal = ((IReadOnlyList<string>)TestData.Dupe1).NullableSetEqual(TestData.Dupe2, StringComparer.Ordinal);
            Assert.True(equal);
        }
    }
}

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
    public static class EnumerableExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_both_null()
        {
            var equal = ((HashSet<string>)null).NullableSequenceEqual(null);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_both_empty()
        {
            var set1 = new HashSet<string>();
            var set2 = new HashSet<string>();

            var equal = set1.NullableSequenceEqual(set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_both_one()
        {
            var set1 = new HashSet<string> { "hi" };
            var set2 = new HashSet<string> { "HI" };
            var set3 = new HashSet<string> { "bye" };

            var equal = set1.NullableSequenceEqual(set2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = set1.NullableSequenceEqual(set3, StringComparer.OrdinalIgnoreCase);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_one_null()
        {
            var equal = TestData.Set.NullableSequenceEqual(null);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_different_count()
        {
            var set2 = new HashSet<string>(TestData.Set);
            set2.Remove("foo");

            var equal = TestData.Set.NullableSequenceEqual(set2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_is_equal()
        {
            var set2 = new HashSet<string>(TestData.Set);

            var equal = TestData.Set.NullableSequenceEqual(set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_not_equal()
        {
            var set2 = new HashSet<string>(TestData.Set)
            {
                "xyz"
            };

            var equal = TestData.Set.NullableSequenceEqual(set2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_not_equal_0()
        {
            var set = new[]
            {
                TestData.   List[2],
                TestData.   List[1],
                "awk",
                TestData.  List[0]
            };

            var equal = TestData.List.NullableSequenceEqual(set, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_not_equal_1()
        {
            var list2 = new[]
            {
                TestData.List[2],
                TestData.List[1],
                TestData.List[3],
                TestData.List[0]
            };

            var equal = TestData.List.NullableSequenceEqual(list2, StringComparer.Ordinal);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void SequenceEquals_not_equal_duplicates()
        {
            var equal = TestData.Dupe1.NullableSequenceEqual(TestData.Dupe2, StringComparer.Ordinal);
            Assert.False(equal);
        }
    }
}

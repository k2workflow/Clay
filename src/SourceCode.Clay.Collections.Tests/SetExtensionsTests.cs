using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class SetExtensionsTests
    {
        private static readonly HashSet<string> _set = new HashSet<string>
        {
            "foo",
            "bar",
            "baz",
            "nin"
        };

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SetExtensions NullableEquals null, null")]
        public static void Use_NullableEquals_both_null()
        {
            var equal = SetExtensions.SetEquals(((HashSet<string>)null), null);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SetExtensions NullableEquals 0, 0")]
        public static void Use_NullableEquals_both_empty()
        {
            var set1 = new HashSet<string>();
            var set2 = new HashSet<string>();

            var equal = SetExtensions.SetEquals(set1, set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SetExtensions NullableEquals 1, 1")]
        public static void Use_NullableEquals_both_one()
        {
            var set1 = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "hi" };
            var set2 = new HashSet<string> { "HI" };
            var set3 = new HashSet<string> { "bye" };

            var equal = SetExtensions.SetEquals(set1, set2);
            Assert.True(equal);

            equal = SetExtensions.SetEquals(set1, set3);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SetExtensions NullableEquals list, null")]
        public static void Use_NullableEquals_one_null()
        {
            var equal = SetExtensions.SetEquals(_set, null);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SetExtensions NullableEquals N, M")]
        public static void Use_NullableEquals_different_count()
        {
            var set2 = new HashSet<string>(_set);
            set2.Remove("foo");

            var equal = SetExtensions.SetEquals(_set, set2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SetExtensions NullableEquals IsEqual")]
        public static void Use_NullableEquals_IsEqual()
        {
            var set2 = new HashSet<string>(_set);

            var equal = SetExtensions.SetEquals(_set, set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SetExtensions NullableEquals NotEqual")]
        public static void Use_NullableEquals_NotEqual()
        {
            var set2 = new HashSet<string>(_set);
            set2.Add("xyz");

            var equal = SetExtensions.SetEquals(_set, set2);
            Assert.False(equal);
        }
    }
}

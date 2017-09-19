using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class DictionaryExtensionsFixtures
    {
        private static readonly Dictionary<string, string> _dict = new Dictionary<string, string>()
        {
            ["foo"] = "foo1",
            ["bar"] = "bar1",
            ["baz"] = "baz1",
            ["nin"] = "nin1"
        };

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions NullableEquals null, null")]
        public static void Use_NullableEquals_both_null()
        {
            var equal = ((Dictionary<string, string>)null).NullableEquals(null);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions NullableEquals 0, 0")]
        public static void Use_NullableEquals_both_empty()
        {
            var dict1 = new Dictionary<string, string>();
            var dict2 = new Dictionary<string, string>();

            var equal = dict1.NullableEquals(dict2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions NullableEquals 1, 1")]
        public static void Use_NullableEquals_both_one()
        {
            var dict1 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { ["hi"] = "hi1" };
            var dict2 = new Dictionary<string, string> { ["HI"] = "HI1" };
            var dict3 = new Dictionary<string, string> { ["bye"] = "bye1" };

            var equal = dict1.NullableEquals(dict2);
            Assert.False(equal);

            equal = dict1.NullableEquals(dict2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = dict1.NullableEquals(dict3);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions NullableEquals list, null")]
        public static void Use_NullableEquals_one_null()
        {
            var equal = _dict.NullableEquals(null);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions NullableEquals N, M")]
        public static void Use_NullableEquals_different_count()
        {
            var dict2 = new Dictionary<string, string>(_dict);
            dict2.Remove("foo");

            var equal = _dict.NullableEquals(dict2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions NullableEquals IsEqual")]
        public static void Use_NullableEquals_IsEqual()
        {
            var dict2 = new Dictionary<string, string>(_dict);

            var equal = _dict.NullableEquals(dict2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions NullableEquals NotEqual")]
        public static void Use_NullableEquals_NotEqual()
        {
            var dict2 = new Dictionary<string, string>(_dict)
            {
                ["xyz"] = "xyz1"
            };

            var equal = _dict.NullableEquals(dict2);
            Assert.False(equal);
        }
    }
}

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
    public static class DictionaryExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DictionaryEquals_both_null))]
        public static void DictionaryEquals_both_null()
        {
            var equal = ((IDictionary<string, string>)null).NullableDictionaryEqual(null);
            Assert.True(equal);

            equal = ((IReadOnlyDictionary<string, string>)null).NullableDictionaryEqual(null);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DictionaryEquals_both_empty))]
        public static void DictionaryEquals_both_empty()
        {
            var dict1 = new Dictionary<string, string>();
            var dict2 = new Dictionary<string, string>();

            var equal = dict1.NullableDictionaryEqual(dict2);
            Assert.True(equal);

            equal = ((IReadOnlyDictionary<string, string>)dict1).NullableDictionaryEqual(dict2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DictionaryEquals_both_one))]
        public static void DictionaryEquals_both_one()
        {
            var dict1 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { ["hi"] = "hi1" };
            var dict2 = new Dictionary<string, string> { ["HI"] = "HI1" };
            var dict3 = new Dictionary<string, string> { ["bye"] = "bye1" };

            var equal = dict1.NullableDictionaryEqual(dict2);
            Assert.False(equal);

            equal = ((IReadOnlyDictionary<string, string>)dict1).NullableDictionaryEqual(dict2);
            Assert.False(equal);

            equal = dict1.NullableDictionaryEqual(dict2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = ((IReadOnlyDictionary<string, string>)dict1).NullableDictionaryEqual(dict2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = dict1.NullableDictionaryEqual(dict3);
            Assert.False(equal);

            equal = ((IReadOnlyDictionary<string, string>)dict1).NullableDictionaryEqual(dict3);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DictionaryEquals_one_null))]
        public static void DictionaryEquals_one_null()
        {
            var equal = TestData.Dict.NullableDictionaryEqual(null);
            Assert.False(equal);

            equal = ((IReadOnlyDictionary<string, string>)TestData.Dict).NullableDictionaryEqual(null);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DictionaryEquals_different_count))]
        public static void DictionaryEquals_different_count()
        {
            var dict2 = new Dictionary<string, string>(TestData.Dict);
            dict2.Remove("foo");

            var equal = TestData.Dict.NullableDictionaryEqual(dict2);
            Assert.False(equal);

            equal = ((IReadOnlyDictionary<string, string>)TestData.Dict).NullableDictionaryEqual(dict2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DictionaryEquals_IsEqual))]
        public static void DictionaryEquals_IsEqual()
        {
            var dict2 = new Dictionary<string, string>(TestData.Dict);

            var equal = TestData.Dict.NullableDictionaryEqual(dict2);
            Assert.True(equal);

            equal = ((IReadOnlyDictionary<string, string>)TestData.Dict).NullableDictionaryEqual(dict2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DictionaryEquals_NotEqual))]
        public static void DictionaryEquals_NotEqual()
        {
            var dict2 = new Dictionary<string, string>(TestData.Dict)
            {
                ["xyz"] = "xyz1"
            };

            var equal = TestData.Dict.NullableDictionaryEqual(dict2);
            Assert.False(equal);

            equal = ((IReadOnlyDictionary<string, string>)TestData.Dict).NullableDictionaryEqual(dict2);
            Assert.False(equal);
        }
    }
}

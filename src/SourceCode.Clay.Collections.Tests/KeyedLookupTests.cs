using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class KeyedLookupTests
    {
        [Fact(DisplayName = nameof(Use_KeyedLookup_KeyExtractor))]
        public static void Use_KeyedLookup_KeyExtractor()
        {
            var items = new[]
            {
                new KeyValuePair<int, AttributeTargets>(10, AttributeTargets.Class),
                new KeyValuePair<int, AttributeTargets>(20, AttributeTargets.Constructor),
                new KeyValuePair<int, AttributeTargets>(30, AttributeTargets.Delegate)
            };

            var lookup = KeyedCollectionFactory.Create<int, KeyValuePair<int, AttributeTargets>>(items, t => t.Key / 10);

            Assert.Throws<KeyNotFoundException>(() => lookup[4]);

            Assert.Equal(AttributeTargets.Class, lookup[1].Value);
            Assert.Equal(AttributeTargets.Constructor, lookup[2].Value);
            Assert.Equal(AttributeTargets.Delegate, lookup[3].Value);
        }

        [Fact(DisplayName = nameof(Use_KeyedLookup_KeyExtractor_OrdinalIgnoreCase))]
        public static void Use_KeyedLookup_KeyExtractor_OrdinalIgnoreCase()
        {
            var items = new[]
            {
                new KeyValuePair<string, AttributeTargets>("a", AttributeTargets.Class),
                new KeyValuePair<string, AttributeTargets>("b", AttributeTargets.Constructor),
                new KeyValuePair<string, AttributeTargets>("c", AttributeTargets.Delegate)
            };

            var lookup = KeyedCollectionFactory.Create<string, KeyValuePair<string, AttributeTargets>>(items, t => t.Key, StringComparer.OrdinalIgnoreCase);

            Assert.Throws<KeyNotFoundException>(() => lookup["d"]);

            Assert.Equal(AttributeTargets.Class, lookup["a"].Value);
            Assert.Equal(AttributeTargets.Class, lookup["A"].Value);
            Assert.Equal(AttributeTargets.Constructor, lookup["b"].Value);
            Assert.Equal(AttributeTargets.Constructor, lookup["B"].Value);
            Assert.Equal(AttributeTargets.Delegate, lookup["c"].Value);
            Assert.Equal(AttributeTargets.Delegate, lookup["C"].Value);
        }

        [Fact(DisplayName = nameof(Use_KeyedLookup_KeyExtractor_Ordinal))]
        public static void Use_KeyedLookup_KeyExtractor_Ordinal()
        {
            var items = new[]
            {
                new KeyValuePair<string, AttributeTargets>("a", AttributeTargets.Class),
                new KeyValuePair<string, AttributeTargets>("b", AttributeTargets.Constructor),
                new KeyValuePair<string, AttributeTargets>("c", AttributeTargets.Delegate)
            };

            var lookup = KeyedCollectionFactory.Create<string, KeyValuePair<string, AttributeTargets>>(items, t => t.Key, StringComparer.Ordinal);

            Assert.Throws<KeyNotFoundException>(() => lookup["d"]);

            Assert.Equal(AttributeTargets.Class, lookup["a"].Value);
            Assert.Throws<KeyNotFoundException>(() => lookup["A"]);
            Assert.Equal(AttributeTargets.Constructor, lookup["b"].Value);
            Assert.Throws<KeyNotFoundException>(() => lookup["B"]);
            Assert.Equal(AttributeTargets.Delegate, lookup["c"].Value);
            Assert.Throws<KeyNotFoundException>(() => lookup["C"]);
        }
    }
}

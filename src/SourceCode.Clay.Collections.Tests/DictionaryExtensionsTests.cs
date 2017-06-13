using SourceCode.Clay.Collections.Generic;
using System;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class DictionaryExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions BuildOrdinalSwitchExpression IgnoreCase")]
        public static void Use_BuildOrdinalSwitchExpression_IgnoreCase()
        {
            var list = new[]
            {
                "Foo",
                "Bar",
                "Baz"
            };

            var sut = ListExtensions.BuildOrdinalSwitchExpression(list, x => x, true);

            Assert.Equal(-1, sut("fob"));
            Assert.Equal(0, sut("foo"));
            Assert.Equal(1, sut("bar"));
            Assert.Equal(2, sut("baz"));
            Assert.Equal(0, sut("Foo"));
            Assert.Equal(1, sut("Bar"));
            Assert.Equal(2, sut("Baz"));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions BuildOrdinalSwitchExpression StrictCase")]
        public static void Use_BuildOrdinalSwitchExpression_StrictCase()
        {
            var list = new[]
            {
                "Foo",
                "Bar",
                "Baz"
            };

            var sut = ListExtensions.BuildOrdinalSwitchExpression(list, x => x, false);

            Assert.Equal(-1, sut("fob"));
            Assert.Equal(-1, sut("foo"));
            Assert.Equal(-1, sut("bar"));
            Assert.Equal(-1, sut("baz"));
            Assert.Equal(0, sut("Foo"));
            Assert.Equal(1, sut("Bar"));
            Assert.Equal(2, sut("Baz"));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DictionaryExtensions BuildOrdinalSwitchExpression KeyExtractor ValueExtractor")]
        public static void Use_BuildSwitchExpression()
        {
            var list = new[]
            {
                Tuple.Create("foo", AttributeTargets.Class),
                Tuple.Create("bar", AttributeTargets.Constructor),
                Tuple.Create("baz", AttributeTargets.Delegate)
            };

            var sut = ListExtensions.BuildSwitchExpression(list, x => x.Item1, x => x.Item2);

            Assert.Equal((AttributeTargets)0, sut("fob"));
            Assert.Equal(AttributeTargets.Class, sut("foo"));
            Assert.Equal(AttributeTargets.Constructor, sut("bar"));
            Assert.Equal(AttributeTargets.Delegate, sut("baz"));
            Assert.Equal((AttributeTargets)0, sut("Foo"));
            Assert.Equal((AttributeTargets)0, sut("Bar"));
            Assert.Equal((AttributeTargets)0, sut("Baz"));
        }
    }
}

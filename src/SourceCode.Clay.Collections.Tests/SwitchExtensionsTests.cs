using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class SwitchExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch Ordinal")]
        public static void Use_ToDynamicSwitch_Ordinal()
        {
            var dict = new Dictionary<string, AttributeTargets>(StringComparer.Ordinal)
            {
                ["foo"] = AttributeTargets.Class,
                ["bar"] = AttributeTargets.Constructor,
                ["baz"] = AttributeTargets.Delegate
            };

            var @switch = dict.ToDynamicSwitch(false);

            Assert.Equal(dict.Count, @switch.Count);
            Assert.Equal(dict.ContainsKey("FOO"), @switch.ContainsKey("FOO"));
            Assert.Equal(dict.ContainsKey("foo"), @switch.ContainsKey("foo"));
            Assert.Equal(dict.ContainsKey("fob"), @switch.ContainsKey("fob"));
            Assert.Equal(dict["foo"], @switch["foo"]);
            Assert.Equal(dict["bar"], @switch["bar"]);
            Assert.Equal(dict["baz"], @switch["baz"]);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch OrdinalIgnoreCase")]
        public static void Use_ToDynamicSwitch_OrdinalIgnoreCase()
        {
            var dict = new Dictionary<string, AttributeTargets>(StringComparer.OrdinalIgnoreCase)
            {
                ["foo"] = AttributeTargets.Class,
                ["bar"] = AttributeTargets.Constructor,
                ["baz"] = AttributeTargets.Delegate
            };

            var @switch = dict.ToDynamicSwitch(true);

            Assert.Equal(dict.Count, @switch.Count);
            Assert.Equal(dict.ContainsKey("FOO"), @switch.ContainsKey("FOO"));
            Assert.Equal(dict.ContainsKey("foo"), @switch.ContainsKey("foo"));
            Assert.Equal(dict.ContainsKey("fob"), @switch.ContainsKey("fob"));
            Assert.Equal(dict["foo"], @switch["foo"]);
            Assert.Equal(dict["bar"], @switch["bar"]);
            Assert.Equal(dict["baz"], @switch["baz"]);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch Bool")]
        public static void Use_ToDynamicSwitch_Bool()
        {
            var dict = new Dictionary<bool, int>()
            {
                [false] = 0,
                [true] = 1
            };

            var @switch = dict.ToDynamicSwitch();

            Assert.Equal(dict.Count, @switch.Count);
            Assert.Equal(dict.ContainsKey(false), @switch.ContainsKey(false));
            Assert.Equal(dict.ContainsKey(true), @switch.ContainsKey(true));
            Assert.Equal(dict[false], @switch[false]);
            Assert.Equal(dict[true], @switch[true]);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch Int32")]
        public static void Use_ToDynamicSwitch_Int32()
        {
            var dict = new Dictionary<int, string>()
            {
                [-1] = "neg",
                [0] = "zero",
                [1] = "one",
                [3] = "three"
            };

            var @switch = dict.ToDynamicSwitch();

            Assert.Equal(dict.Count, @switch.Count);
            Assert.Equal(dict.ContainsKey(-1), @switch.ContainsKey(-1));
            Assert.Equal(dict.ContainsKey(0), @switch.ContainsKey(0));
            Assert.Equal(dict.ContainsKey(1), @switch.ContainsKey(1));
            Assert.Equal(dict.ContainsKey(2), @switch.ContainsKey(2));
            Assert.Equal(dict.ContainsKey(3), @switch.ContainsKey(3));
            Assert.Equal(dict[-1], @switch[-1]);
            Assert.Equal(dict[0], @switch[0]);
            Assert.Equal(dict[1], @switch[1]);
            Assert.Equal(dict[3], @switch[3]);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SwitchExtensions BuildOrdinalSwitchExpression IgnoreCase")]
        public static void Use_BuildOrdinalSwitchExpression_IgnoreCase()
        {
            var list = new[]
            {
                "Foo",
                "Bar",
                "Baz"
            };

            var @switch = list.BuildOrdinalSwitchExpression(x => x, true);

            Assert.Equal(-1, @switch("fob"));
            Assert.Equal(0, @switch("foo"));
            Assert.Equal(1, @switch("bar"));
            Assert.Equal(2, @switch("baz"));
            Assert.Equal(0, @switch("Foo"));
            Assert.Equal(1, @switch("Bar"));
            Assert.Equal(2, @switch("Baz"));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SwitchExtensions BuildOrdinalSwitchExpression StrictCase")]
        public static void Use_BuildOrdinalSwitchExpression_StrictCase()
        {
            var list = new[]
            {
                "Foo",
                "Bar",
                "Baz"
            };

            var @switch = list.BuildOrdinalSwitchExpression(x => x, false);

            Assert.Equal(-1, @switch("fob"));
            Assert.Equal(-1, @switch("foo"));
            Assert.Equal(-1, @switch("bar"));
            Assert.Equal(-1, @switch("baz"));
            Assert.Equal(0, @switch("Foo"));
            Assert.Equal(1, @switch("Bar"));
            Assert.Equal(2, @switch("Baz"));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SwitchExtensions BuildOrdinalSwitchExpression KeyExtractor ValueExtractor")]
        public static void Use_BuildSwitchExpression()
        {
            var list = new[]
            {
                Tuple.Create("foo", AttributeTargets.Class),
                Tuple.Create("bar", AttributeTargets.Constructor),
                Tuple.Create("baz", AttributeTargets.Delegate)
            };

            var @switch = list.BuildSwitchExpression(x => x.Item1, x => x.Item2);

            Assert.Equal((AttributeTargets)0, @switch("fob"));
            Assert.Equal(AttributeTargets.Class, @switch("foo"));
            Assert.Equal(AttributeTargets.Constructor, @switch("bar"));
            Assert.Equal(AttributeTargets.Delegate, @switch("baz"));
            Assert.Equal((AttributeTargets)0, @switch("Foo"));
            Assert.Equal((AttributeTargets)0, @switch("Bar"));
            Assert.Equal((AttributeTargets)0, @switch("Baz"));
        }
    }
}

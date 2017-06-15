﻿using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Collections.Tests
{
    public static class SwitchExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch string StrictCase")]
        public static void Use_ToDynamicSwitch_String_StrictCase()
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
        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch String IgnoreCase")]
        public static void Use_ToDynamicSwitch_String_IgnoreCase()
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
        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch KeyExtractor IgnoreCase")]
        public static void Use_SwitchExtensions_ToDynamicSwitch_KeyExtractor_IgnoreCase()
        {
            var list = new[]
            {
                "Foo",
                "Bar",
                "Baz"
            };

            var @switch = list.ToDynamicSwitch(x => x, true);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["fob"]);

            Assert.Equal("Foo", @switch["Foo"]);
            Assert.Equal("Bar", @switch["Bar"]);
            Assert.Equal("Baz", @switch["Baz"]);

            Assert.Equal("Foo", @switch["foo"]);
            Assert.Equal("Bar", @switch["bar"]);
            Assert.Equal("Baz", @switch["baz"]);

            Assert.Equal("Foo", @switch["FOO"]);
            Assert.Equal("Bar", @switch["BAR"]);
            Assert.Equal("Baz", @switch["BAZ"]);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch KeyExtractor StrictCase")]
        public static void Use_SwitchExtensions_ToDynamicSwitch_KeyExtractor_StrictCase()
        {
            var list = new[]
            {
                "Foo",
                "Bar",
                "Baz"
            };

            var @switch = list.ToDynamicSwitch(x => x, false);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["fob"]);

            Assert.Equal("Foo", @switch["Foo"]);
            Assert.Equal("Bar", @switch["Bar"]);
            Assert.Equal("Baz", @switch["Baz"]);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["foo"]);
            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["bar"]);
            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["baz"]);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["FOO"]);
            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["BAR"]);
            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["BAZ"]);
        }

        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch KeyValueExtractor IgnoreCase")]
        public static void Use_SwitchExtensions_ToDynamicSwitch_KeyValueExtractor_IgnoreCase()
        {
            var list = new[]
            {
                Tuple.Create("foo", AttributeTargets.Class),
                Tuple.Create("bar", AttributeTargets.Constructor),
                Tuple.Create("baz", AttributeTargets.Delegate)
            };

            var @switch = list.ToDynamicSwitch(x => x.Item1, x => x.Item2, true);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["fob"]);

            Assert.Equal(AttributeTargets.Class, @switch["Foo"]);
            Assert.Equal(AttributeTargets.Constructor, @switch["Bar"]);
            Assert.Equal(AttributeTargets.Delegate, @switch["Baz"]);

            Assert.Equal(AttributeTargets.Class, @switch["foo"]);
            Assert.Equal(AttributeTargets.Constructor, @switch["bar"]);
            Assert.Equal(AttributeTargets.Delegate, @switch["baz"]);

            Assert.Equal(AttributeTargets.Class, @switch["FOO"]);
            Assert.Equal(AttributeTargets.Constructor, @switch["BAR"]);
            Assert.Equal(AttributeTargets.Delegate, @switch["BAZ"]);
        }

        [Fact(DisplayName = "SwitchExtensions ToDynamicSwitch KeyValueExtractor StrictCase")]
        public static void Use_SwitchExtensions_ToDynamicSwitch_KeyValueExtractor_StrictCase()
        {
            var list = new[]
            {
                Tuple.Create("foo", AttributeTargets.Class),
                Tuple.Create("bar", AttributeTargets.Constructor),
                Tuple.Create("baz", AttributeTargets.Delegate)
            };

            var @switch = list.ToDynamicSwitch(x => x.Item1, x => x.Item2, false);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["fob"]);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["Foo"]);
            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["Bar"]);
            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["Baz"]);

            Assert.Equal(AttributeTargets.Class, @switch["foo"]);
            Assert.Equal(AttributeTargets.Constructor, @switch["bar"]);
            Assert.Equal(AttributeTargets.Delegate, @switch["baz"]);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["FOO"]);
            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["BAR"]);
            Assert.Throws<ArgumentOutOfRangeException>(() => @switch["BAZ"]);
        }
    }
}

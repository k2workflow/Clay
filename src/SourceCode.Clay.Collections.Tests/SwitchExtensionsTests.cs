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
    public static class SwitchExtensionsTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ToDynamicSwitch_Bool))]
        public static void ToDynamicSwitch_Bool()
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
        [Fact(DisplayName = nameof(ToDynamicSwitch_Int32))]
        public static void ToDynamicSwitch_Int32()
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
        [Fact(DisplayName = nameof(ToDynamicSwitch_KeyExtractor))]
        public static void ToDynamicSwitch_KeyExtractor()
        {
            var list = new[]
            {
                10,
                20,
                30
            };

            var @switch = list.ToDynamicSwitch(x => x / 10);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch[4]);

            Assert.Equal(10, @switch[1]);
            Assert.Equal(20, @switch[2]);
            Assert.Equal(30, @switch[3]);
        }

        [Fact(DisplayName = nameof(ToDynamicSwitch_KeyValueExtractor))]
        public static void ToDynamicSwitch_KeyValueExtractor()
        {
            var list = new[]
            {
                Tuple.Create(10, AttributeTargets.Class),
                Tuple.Create(20, AttributeTargets.Constructor),
                Tuple.Create(30, AttributeTargets.Delegate)
            };

            var @switch = list.ToDynamicSwitch(x => x.Item1 / 10, x => x.Item2);

            Assert.Throws<ArgumentOutOfRangeException>(() => @switch[4]);

            Assert.Equal(AttributeTargets.Class, @switch[1]);
            Assert.Equal(AttributeTargets.Constructor, @switch[2]);
            Assert.Equal(AttributeTargets.Delegate, @switch[3]);
        }

        #endregion
    }
}

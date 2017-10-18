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
    public static class SetExtensionsTests
    {
        #region Constants

        private static readonly HashSet<string> _set = new HashSet<string>
        {
            "foo",
            "bar",
            "baz",
            "nin"
        };

        #endregion

        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_both_null))]
        public static void SetEquals_both_null()
        {
            var equal = ((HashSet<string>)null).NullableSetEquals(null);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_both_empty))]
        public static void SetEquals_both_empty()
        {
            var set1 = new HashSet<string>();
            var set2 = new HashSet<string>();

            var equal = set1.NullableSetEquals(set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_both_one))]
        public static void SetEquals_both_one()
        {
            var set1 = new HashSet<string> { "hi" };
            var set2 = new HashSet<string> { "HI" };
            var set3 = new HashSet<string> { "bye" };

            var equal = set1.NullableSetEquals(set2, StringComparer.OrdinalIgnoreCase);
            Assert.True(equal);

            equal = set1.NullableSetEquals(set3, StringComparer.OrdinalIgnoreCase);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_one_null))]
        public static void SetEquals_one_null()
        {
            var equal = _set.NullableSetEquals(null);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_different_count))]
        public static void SetEquals_different_count()
        {
            var set2 = new HashSet<string>(_set);
            set2.Remove("foo");

            var equal = _set.NullableSetEquals(set2);
            Assert.False(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_IsEqual))]
        public static void SetEquals_IsEqual()
        {
            var set2 = new HashSet<string>(_set);

            var equal = _set.NullableSetEquals(set2);
            Assert.True(equal);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(SetEquals_NotEqual))]
        public static void SetEquals_NotEqual()
        {
            var set2 = new HashSet<string>(_set)
            {
                "xyz"
            };

            var equal = _set.NullableSetEquals(set2);
            Assert.False(equal);
        }

        #endregion
    }
}

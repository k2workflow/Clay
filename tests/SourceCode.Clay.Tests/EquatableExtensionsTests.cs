#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class EquatableExtensionsTests
    {
        private const string _a = "a";

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, _a, false)]
        [InlineData(_a, null, false)]
        [InlineData(_a, _a, true)]
        [InlineData("", "", true)]
        [InlineData(_a, "c", false)]
        [InlineData("c", "c", true)]
        public static void When_nullable_equals_string(string x, string y, bool expected)
        {
            Assert.Equal(expected, x.NullableEquals(y));
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, false)]
        public static void When_nullable_equals_int(int? x, int? y, bool expected)
        {
            Assert.Equal(expected, x.NullableEquals(y));
        }
    }
}

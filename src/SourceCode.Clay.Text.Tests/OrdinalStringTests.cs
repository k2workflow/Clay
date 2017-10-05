#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Text.Tests
{
    public static class OrdinalStringTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalStringTests NotEqual null")]
        public static void When_not_equal_null()
        {
            OrdinalString actual = null;
            const string expected = "ABCD";

            Assert.NotEqual(expected, actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalStringTests NotEqual value")]
        public static void When_not_equal()
        {
            OrdinalString actual = "abcd";
            const string expected = "ABCDE";

            Assert.NotEqual(expected, actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalStringTests NotEqual case")]
        public static void When_not_equal_case()
        {
            OrdinalString actual = "abcd";
            const string expected = "ABCD";

            Assert.NotEqual(expected, actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalStringTests Equal")]
        public static void When_equal()
        {
            OrdinalString actual = "abcd";
            const string expected = "abcd";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Text.Tests
{
    public static class OrdinalIgnoreCaseStringTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalIgnoreCaseString NotEqual null")]
        public static void When_not_equal_null()
        {
            OrdinalIgnoreCaseString actual = null;
            const string expected = "ABCD";

            Assert.NotEqual(expected, actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalIgnoreCaseString NotEqual value")]
        public static void When_not_equal()
        {
            OrdinalIgnoreCaseString actual = "abcd";
            const string expected = "ABCDE";

            Assert.NotEqual(expected, actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalIgnoreCaseString Equal case")]
        public static void When_equal()
        {
            OrdinalIgnoreCaseString actual = "abcd";
            const string expected = "ABCD";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

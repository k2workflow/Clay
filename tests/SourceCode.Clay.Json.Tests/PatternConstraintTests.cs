#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class PatternConstraintTests
    {
        [Trait("Type", "Unit")]
        [Theory]
        [InlineData("a", true)]
        [InlineData("A", true)]
        [InlineData("b", false)]
        [InlineData(null, true)]
        public static void Test_Simple_Optional_PatternConstraint(string value, bool valid)
        {
            // (-∞, ∞)
            var range = new PatternConstraint("a", false);

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData("a", true)]
        [InlineData("A", true)]
        [InlineData("b", false)]
        [InlineData(null, false)]
        public static void Test_Simple_Required_PatternConstraint(string value, bool valid)
        {
            // (-∞, ∞)
            var range = new PatternConstraint("a", true);

            Assert.True(range.IsValid(value) == valid);
        }
    }
}

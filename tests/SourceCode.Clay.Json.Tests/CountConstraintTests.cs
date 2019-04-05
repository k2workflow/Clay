#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class CountConstraintTests
    {
        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-1, false)] // < 0
        [InlineData(0, true)]
        [InlineData(1, true)]
        public static void Test_Empty_CountConstraint(long value, bool valid)
        {
            // (0, ∞)
            var range = new CountConstraint(null, null);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        public static void Test_InclusiveValue_Infinity_CountConstraint(long value, bool valid)
        {
            // [0, ∞)
            var range = new CountConstraint(0, null);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(10, true)]
        [InlineData(11, true)]
        [InlineData(12, false)]
        public static void Test_Infinity_InclusiveValue_CountConstraint(long value, bool valid)
        {
            // (-∞, 11]
            var range = new CountConstraint(null, 11);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-11, false)]
        [InlineData(3, true)]
        [InlineData(10, true)]
        [InlineData(11, false)]
        public static void Test_InclusiveValue_CountConstraint(long value, bool valid)
        {
            // [0, 10]
            var range = new CountConstraint(3, 10);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-11, false)]
        [InlineData(3, false)]
        [InlineData(10, true)]
        [InlineData(11, false)]
        public static void Test_Exact_CountConstraint(long value, bool valid)
        {
            // [10, 10]
            var range = new CountConstraint(10, 10);
            var exact = CountConstraint.Exact(10);

            Assert.Equal(range, exact);
            Assert.True(exact.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }
    }
}

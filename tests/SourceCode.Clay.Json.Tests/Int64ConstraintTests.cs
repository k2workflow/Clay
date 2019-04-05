#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class Int64ConstraintTests
    {
        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-15, true)]
        [InlineData(-10, true)] // Inclusive
        [InlineData(6, true)]
        [InlineData(null, true)]
        public static void Test_Empty_Int64Constraint(long? value, bool valid)
        {
            // (-∞, ∞)
            var range = new Int64Constraint(null, null, RangeOptions.Exclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-15, false)]
        [InlineData(-10, true)] // Inclusive
        [InlineData(6, true)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_Infinity_Int64Constraint(long? value, bool valid)
        {
            // [5, ∞)
            var range = new Int64Constraint(-10, null, RangeOptions.MinimumInclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-15, false)]
        [InlineData(-10, false)] // Exclusive
        [InlineData(6, true)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_Infinity_Int64Constraint(long? value, bool valid)
        {
            // (5, ∞)
            var range = new Int64Constraint(-10, default, RangeOptions.Exclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-15, true)]
        [InlineData(10, true)] // Inclusive
        [InlineData(15, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_InclusiveValue_Int64Constraint(long? value, bool valid)
        {
            // (-∞, 10.1]
            var range = new Int64Constraint(null, 10, RangeOptions.MaximumInclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-15, true)]
        [InlineData(10, false)] // Exclusive
        [InlineData(15, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_ExclusiveValue_Int64Constraint(long? value, bool valid)
        {
            // (-∞, 10.1)
            var range = new Int64Constraint(null, 10, RangeOptions.Exclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-15, false)]
        [InlineData(-10, true)] // Inclusive
        [InlineData(-5, true)]
        [InlineData(5, true)]
        [InlineData(10, true)] // Inclusive
        [InlineData(15, false)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_Int64Constraint(long? value, bool valid)
        {
            // [-10.1, 10.1]
            var range = new Int64Constraint(-10, 10, RangeOptions.Inclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(-15, false)]
        [InlineData(-10, false)] // Exclusive
        [InlineData(-5, true)]
        [InlineData(5, true)]
        [InlineData(10, false)] // Exclusive
        [InlineData(15, false)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_Int64Constraint(long? value, bool valid)
        {
            // (-10.1, 10.1)
            var range = new Int64Constraint(-10, 10, RangeOptions.Exclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }
    }
}

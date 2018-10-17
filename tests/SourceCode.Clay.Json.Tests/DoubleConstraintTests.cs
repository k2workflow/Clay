#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class DoubleConstraintTests
    {
        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Empty_DoubleConstraint))]
        [InlineData(0.49, true)]
        [InlineData(0.5, true)] // Inclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void Test_Empty_DoubleConstraint(double? value, bool valid)
        {
            // (-∞, ∞)
            var range = new DoubleConstraint(default, default, RangeOptions.Exclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_InclusiveValue_Infinity_DoubleConstraint))]
        [InlineData(0.49, false)]
        [InlineData(0.5, true)] // Inclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_Infinity_DoubleConstraint(double? value, bool valid)
        {
            // [0.5, ∞)
            var range = new DoubleConstraint(0.5, default, RangeOptions.MinimumInclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_ExclusiveValue_Infinity_DoubleConstraint))]
        [InlineData(0.49, false)]
        [InlineData(0.5, false)] // Exclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_Infinity_DoubleConstraint(double? value, bool valid)
        {
            // (0.5, ∞)
            var range = new DoubleConstraint(0.5, default, RangeOptions.Exclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_InclusiveValue_DoubleConstraint))]
        [InlineData(10, true)]
        [InlineData(10.1, true)] // Inclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_InclusiveValue_DoubleConstraint(double? value, bool valid)
        {
            // (-∞, 10.1]
            var range = new DoubleConstraint(default, 10.1, RangeOptions.MaximumInclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_ExclusiveValue_DoubleConstraint))]
        [InlineData(10, true)]
        [InlineData(10.1, false)] // Exclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_ExclusiveValue_DoubleConstraint(double? value, bool valid)
        {
            // (-∞, 10.1)
            var range = new DoubleConstraint(default, 10.1, RangeOptions.Exclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_InclusiveValue_DoubleConstraint))]
        [InlineData(-10.2, false)]
        [InlineData(-10.1, true)] // Inclusive
        [InlineData(-10, true)]
        [InlineData(10, true)]
        [InlineData(10.1, true)] // Inclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_DoubleConstraint(double? value, bool valid)
        {
            // [-10.1, 10.1]
            var range = new DoubleConstraint(-10.1, 10.1, RangeOptions.Inclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_ExclusiveValue_DoubleConstraint))]
        [InlineData(-10.2, false)]
        [InlineData(-10.1, false)] // Exclusive
        [InlineData(-10, true)]
        [InlineData(10, true)]
        [InlineData(10.1, false)] // Exclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_DoubleConstraint(double? value, bool valid)
        {
            // (-10.1, 10.1)
            var range = new DoubleConstraint(-10.1, 10.1, RangeOptions.Exclusive);

            Assert.True(range.IsValid(value) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }
    }
}

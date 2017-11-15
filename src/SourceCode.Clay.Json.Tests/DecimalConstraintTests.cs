#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class DecimalConstraintTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Empty_DecimalConstraint))]
        [InlineData(0.49, true)]
        [InlineData(0.5, true)] // Inclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void Test_Empty_DecimalConstraint(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (-∞, ∞)
            var range = new DecimalConstraint(default, default, RangeOptions.Exclusive);

            Assert.True(range.IsValid(dec) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_InclusiveValue_Infinity_DecimalConstraint))]
        [InlineData(0.49, false)]
        [InlineData(0.5, true)] // Inclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_Infinity_DecimalConstraint(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // [0.5, ∞)
            var range = new DecimalConstraint(0.5m, default, RangeOptions.MinimumInclusive);

            Assert.True(range.IsValid(dec) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_ExclusiveValue_Infinity_DecimalConstraint))]
        [InlineData(0.49, false)]
        [InlineData(0.5, false)] // Exclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_Infinity_DecimalConstraint(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (0.5, ∞)
            var range = new DecimalConstraint(0.5m, default, RangeOptions.Exclusive);

            Assert.True(range.IsValid(dec) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_InclusiveValue_DecimalConstraint))]
        [InlineData(10, true)]
        [InlineData(10.1, true)] // Inclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_InclusiveValue_DecimalConstraint(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (-∞, 10.1]
            var range = new DecimalConstraint(default, 10.1m, RangeOptions.MaximumInclusive);

            Assert.True(range.IsValid(dec) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_ExclusiveValue_DecimalConstraint))]
        [InlineData(10, true)]
        [InlineData(10.1, false)] // Exclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_ExclusiveValue_DecimalConstraint(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (-∞, 10.1)
            var range = new DecimalConstraint(default, 10.1m, RangeOptions.Exclusive);

            Assert.True(range.IsValid(dec) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_InclusiveValue_DecimalConstraint))]
        [InlineData(-10.2, false)]
        [InlineData(-10.1, true)] // Inclusive
        [InlineData(-10, true)]
        [InlineData(10, true)]
        [InlineData(10.1, true)] // Inclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_DecimalConstraint(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // [-10.1, 10.1]
            var range = new DecimalConstraint(-10.1m, 10.1m, RangeOptions.Inclusive);

            Assert.True(range.IsValid(dec) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_ExclusiveValue_DecimalConstraint))]
        [InlineData(-10.2, false)]
        [InlineData(-10.1, false)] // Exclusive
        [InlineData(-10, true)]
        [InlineData(10, true)]
        [InlineData(10.1, false)] // Exclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_DecimalConstraint(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (-10.1, 10.1)
            var range = new DecimalConstraint(-10.1m, 10.1m, RangeOptions.Exclusive);

            Assert.True(range.IsValid(dec) == valid);
            Assert.NotEqual(0, range.GetHashCode());
        }

        #endregion
    }
}

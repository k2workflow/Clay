#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class NumberValidatorTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(OpenApi_Test_Empty_DoubleValidator))]
        [InlineData(0.49, true)]
        [InlineData(0.5, true)] // Inclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void OpenApi_Test_Empty_DoubleValidator(double? value, bool valid)
        {
            // (-∞, ∞)
            var range = new NumberValidator(default, default, false);

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(OpenApi_Test_InclusiveValue_Infinity_DoubleValidator))]
        [InlineData(0.49, false)]
        [InlineData(0.5, true)] // Inclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void OpenApi_Test_InclusiveValue_Infinity_DoubleValidator(double? value, bool valid)
        {
            // [0.5, ∞)
            var range = new NumberValidator(0.5, default, false, true, false, null); // maxExclusive is redundant

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(OpenApi_Test_ExclusiveValue_Infinity_DoubleValidator))]
        [InlineData(0.49, false)]
        [InlineData(0.5, false)] // Exclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void OpenApi_Test_ExclusiveValue_Infinity_DoubleValidator(double? value, bool valid)
        {
            // (0.5, ∞)
            var range = new NumberValidator(0.5, default, true, true, false, null); // maxExclusive is redundant

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(OpenApi_Test_Infinity_InclusiveValue_DoubleValidator))]
        [InlineData(10, true)]
        [InlineData(10.1, true)] // Inclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void OpenApi_Test_Infinity_InclusiveValue_DoubleValidator(double? value, bool valid)
        {
            // (-∞, 10.1]
            var range = new NumberValidator(default, 10.1, true, false, false, null); // minExclusive is redundant

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(OpenApi_Test_Infinity_ExclusiveValue_DoubleValidator))]
        [InlineData(10, true)]
        [InlineData(10.1, false)] // Exclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void OpenApi_Test_Infinity_ExclusiveValue_DoubleValidator(double? value, bool valid)
        {
            // (-∞, 10.1)
            var range = new NumberValidator(default, 10.1, true, true, false, null); // minExclusive is redundant

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(OpenApi_Test_Infinity_InclusiveValue_DoubleValidator))]
        [InlineData(-10.2, false)]
        [InlineData(-10.1, true)] // Inclusive
        [InlineData(-10, true)]
        [InlineData(10, true)]
        [InlineData(10.1, true)] // Inclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void OpenApi_Test_InclusiveValue_DoubleValidator(double? value, bool valid)
        {
            // (-10.1, 10.1)
            var range = new NumberValidator(-10.1, 10.1, false, false, false, null);

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(OpenApi_Test_ExclusiveValue_DoubleValidator))]
        [InlineData(-10.2, false)]
        [InlineData(-10.1, false)] // Exclusive
        [InlineData(-10, true)]
        [InlineData(10, true)]
        [InlineData(10.1, false)] // Exclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void OpenApi_Test_ExclusiveValue_DoubleValidator(double? value, bool valid)
        {
            // [-10.1, 10.1]
            var range = new NumberValidator(-10.1, 10.1, true, true, false, null);

            Assert.True(range.IsValid(value) == valid);
        }

        #endregion
    }
}

#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class LongValidatorTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Empty_LongValidator))]
        [InlineData(-15, true)]
        [InlineData(-10, true)] // Inclusive
        [InlineData(6, true)]
        [InlineData(null, true)]
        public static void Test_Empty_LongValidator(long? value, bool valid)
        {
            // (-∞, ∞)
            var range = new Int64Validator(null, null, false);

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_InclusiveValue_Infinity_LongValidator))]
        [InlineData(-15, false)]
        [InlineData(-10, true)] // Inclusive
        [InlineData(6, true)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_Infinity_LongValidator(long? value, bool valid)
        {
            // [5, ∞)
            var range = new Int64Validator(-10, null, false, true, false, null); // maxExclusive is redundant

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_ExclusiveValue_Infinity_LongValidator))]
        [InlineData(-15, false)]
        [InlineData(-10, false)] // Exclusive
        [InlineData(6, true)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_Infinity_LongValidator(long? value, bool valid)
        {
            // (5, ∞)
            var range = new Int64Validator(-10, null, true, true, false, null); // maxExclusive is redundant

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_InclusiveValue_LongValidator))]
        [InlineData(-15, true)]
        [InlineData(10, true)] // Inclusive
        [InlineData(15, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_InclusiveValue_LongValidator(long? value, bool valid)
        {
            // (-∞, 10.1]
            var range = new Int64Validator(null, 10, true, false, false, null); // minExclusive is redundant

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_ExclusiveValue_LongValidator))]
        [InlineData(-15, true)]
        [InlineData(10, false)] // Exclusive
        [InlineData(15, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_ExclusiveValue_LongValidator(long? value, bool valid)
        {
            // (-∞, 10.1)
            var range = new Int64Validator(null, 10, true, true, false, null); // minExclusive is redundant

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_InclusiveValue_LongValidator))]
        [InlineData(-15, false)]
        [InlineData(-10, true)] // Inclusive
        [InlineData(-5, true)]
        [InlineData(5, true)]
        [InlineData(10, true)] // Inclusive
        [InlineData(15, false)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_LongValidator(long? value, bool valid)
        {
            // (-10.1, 10.1)
            var range = new Int64Validator(-10, 10, false, false, false, null);

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_ExclusiveValue_LongValidator))]
        [InlineData(-15, false)]
        [InlineData(-10, false)] // Exclusive
        [InlineData(-5, true)]
        [InlineData(5, true)]
        [InlineData(10, false)] // Exclusive
        [InlineData(15, false)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_LongValidator(long? value, bool valid)
        {
            // [-10.1, 10.1]
            var range = new Int64Validator(-10, 10, true, true, false, null);

            Assert.True(range.IsValid(value) == valid);
        }

        #endregion
    }
}

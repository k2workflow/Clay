using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class DecimalValidatorTests
    {
        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Empty_DecimalValidator))]
        [InlineData(0.49, true)]
        [InlineData(0.5, true)] // Inclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void Test_Empty_DecimalValidator(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (-∞, ∞)
            var range = new DecimalValidator(default, default, false);

            Assert.True(range.IsValid(dec) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_InclusiveValue_Infinity_DecimalValidator))]
        [InlineData(0.49, false)]
        [InlineData(0.5, true)] // Inclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_Infinity_DecimalValidator(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // [0.5, ∞)
            var range = new DecimalValidator(0.5m, default, false, true, false, null); // maxExclusive is redundant

            Assert.True(range.IsValid(dec) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_ExclusiveValue_Infinity_DecimalValidator))]
        [InlineData(0.49, false)]
        [InlineData(0.5, false)] // Exclusive
        [InlineData(0.51, true)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_Infinity_DecimalValidator(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (0.5, ∞)
            var range = new DecimalValidator(0.5m, default, true, true, false, null); // maxExclusive is redundant

            Assert.True(range.IsValid(dec) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_InclusiveValue_DecimalValidator))]
        [InlineData(10, true)]
        [InlineData(10.1, true)] // Inclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_InclusiveValue_DecimalValidator(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (-∞, 10.1]
            var range = new DecimalValidator(default, 10.1m, true, false, false, null); // minExclusive is redundant

            Assert.True(range.IsValid(dec) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_ExclusiveValue_DecimalValidator))]
        [InlineData(10, true)]
        [InlineData(10.1, false)] // Exclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_Infinity_ExclusiveValue_DecimalValidator(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (-∞, 10.1)
            var range = new DecimalValidator(default, 10.1m, true, true, false, null); // minExclusive is redundant

            Assert.True(range.IsValid(dec) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_InclusiveValue_DecimalValidator))]
        [InlineData(-10.2, false)]
        [InlineData(-10.1, true)] // Inclusive
        [InlineData(-10, true)]
        [InlineData(10, true)]
        [InlineData(10.1, true)] // Inclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_InclusiveValue_DecimalValidator(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // (-10.1, 10.1)
            var range = new DecimalValidator(-10.1m, 10.1m, false, false, false, null);

            Assert.True(range.IsValid(dec) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_ExclusiveValue_DecimalValidator))]
        [InlineData(-10.2, false)]
        [InlineData(-10.1, false)] // Exclusive
        [InlineData(-10, true)]
        [InlineData(10, true)]
        [InlineData(10.1, false)] // Exclusive
        [InlineData(10.2, false)]
        [InlineData(null, true)]
        public static void Test_ExclusiveValue_DecimalValidator(double? value, bool valid)
        {
            // InlineData does not like decimal literals (eg 10.0m)
            var dec = (decimal?)value;

            // [-10.1, 10.1]
            var range = new DecimalValidator(-10.1m, 10.1m, true, true, false, null);

            Assert.True(range.IsValid(dec) == valid);
        }
    }
}

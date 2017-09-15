using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class LengthValidatorTests
    {
        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Empty_LengthValidator))]
        [InlineData(-1, true)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        public static void Test_Empty_LengthValidator(long value, bool valid)
        {
            // (-∞, ∞)
            var range = new LengthValidator(null, null);

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_InclusiveValue_Infinity_LengthValidator))]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        public static void Test_InclusiveValue_Infinity_LengthValidator(long value, bool valid)
        {
            // [0, ∞)
            var range = new LengthValidator(0, null);

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_InclusiveValue_LengthValidator))]
        [InlineData(10, true)]
        [InlineData(11, true)]
        [InlineData(12, false)]
        public static void Test_Infinity_InclusiveValue_LengthValidator(long value, bool valid)
        {
            // (-∞, 11]
            var range = new LengthValidator(null, 11);

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Infinity_InclusiveValue_LengthValidator))]
        [InlineData(-11, false)]
        [InlineData(-10, true)]
        [InlineData(10, true)]
        [InlineData(11, false)]
        public static void Test_InclusiveValue_LengthValidator(long value, bool valid)
        {
            // [-10, 10]
            var range = new LengthValidator(-10, 10);

            Assert.True(range.IsValid(value) == valid);
        }
    }
}

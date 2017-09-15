using SourceCode.Clay.Json.Validation;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class PatternValidatorTests
    {
        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Simple_Optional_PatternValidator))]
        [InlineData("a", true)]
        [InlineData("A", true)]
        [InlineData("b", false)]
        [InlineData(null, true)]
        public static void Test_Simple_Optional_PatternValidator(string value, bool valid)
        {
            // (-∞, ∞)
            var range = new PatternValidator("a", false);

            Assert.True(range.IsValid(value) == valid);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(Test_Simple_Required_PatternValidator))]
        [InlineData("a", true)]
        [InlineData("A", true)]
        [InlineData("b", false)]
        [InlineData(null, false)]
        public static void Test_Simple_Required_PatternValidator(string value, bool valid)
        {
            // (-∞, ∞)
            var range = new PatternValidator("a", true);

            Assert.True(range.IsValid(value) == valid);
        }
    }
}

using Xunit;

namespace SourceCode.Clay.Text.Tests
{
    public static class OrdinalIgnoreCaseStringTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalIgnoreCaseString NotEqual null")]
        public static void When_not_equal_null()
        {
            OrdinalIgnoreCaseString actual = null;
            const string expected = "ABCD";

            Assert.NotEqual(expected, actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalIgnoreCaseString NotEqual value")]
        public static void When_not_equal()
        {
            OrdinalIgnoreCaseString actual = "abcd";
            const string expected = "ABCDE";

            Assert.NotEqual(expected, actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "OrdinalIgnoreCaseString Equal case")]
        public static void When_equal()
        {
            OrdinalIgnoreCaseString actual = "abcd";
            const string expected = "ABCD";

            Assert.Equal(expected, actual);
        }
    }
}

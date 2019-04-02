using SourceCode.Clay.Correlation.Internal;
using Xunit;

namespace SourceCode.Clay.Correlation.Tests
{
    public static class LambdaAccessorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        public static void LambdaAccessor_returns_input(string expected)
        {
            // Arrange
            var accessor = new LambdaAccessor(() => expected);

            // Act
            var actual = accessor.CorrelationId;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}

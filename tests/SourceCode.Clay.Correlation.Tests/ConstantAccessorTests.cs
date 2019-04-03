using SourceCode.Clay.Correlation.Internal;
using Xunit;

namespace SourceCode.Clay.Correlation.Tests
{
    public static class ConstantAccessorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        public static void ConstantAccessor_returns_string_input(string expected)
        {
            // Arrange
            var accessor = new ConstantAccessor(expected);

            // Act
            var actual = accessor.CorrelationId;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}

using System;
using System.Globalization;
using SourceCode.Clay.Correlation.Internal;
using Xunit;

namespace SourceCode.Clay.Correlation.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class LambdaAccessorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        public static void LambdaAccessor_returns_string_input(string expected)
        {
            // Arrange
            var accessor = new LambdaAccessor<string>(() => expected);

            // Act
            var actual = accessor.CorrelationId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void LambdaAccessor_executes_null_lambda()
        {
            // Arrange
            var accessor = new LambdaAccessor<string>(() => null);

            // Act
            var actual = accessor.CorrelationId;

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public static void LambdaAccessor_executes_static_lambda()
        {
            // Arrange
            var accessor = new LambdaAccessor<string>(MyLambda);

            // Act
            var actual = accessor.CorrelationId;

            // Assert
            Assert.Equal("123", actual);

            string MyLambda()
            {
                return "123";
            }
        }

        [Fact]
        public static void LambdaAccessor_executes_dynamic_lambda()
        {
            // Arrange
            var accessor = new LambdaAccessor<string>(MyLambda);

            // Act
            var actual = accessor.CorrelationId;

            // Assert
            Assert.True(Guid.TryParse(actual, out _));

            string MyLambda()
            {
                return Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
            }
        }

        [Fact]
        public static void LambdaAccessor_throws_null_lambda()
        {
            // Act
            void act() => new LambdaAccessor<string>(null);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}

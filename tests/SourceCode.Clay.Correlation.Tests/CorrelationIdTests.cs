using System;
using System.Globalization;
using Xunit;

namespace SourceCode.Clay.Correlation.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class CorrelationIdTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        public static void CorrelationId_From_string_returns_value(string expected)
        {
            // Arrange
            ICorrelationIdAccessor accessor = CorrelationId.From(expected);

            // Act
            var actual = accessor.CorrelationId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void CorrelationId_From_guid_returns_value()
        {
            // Arrange
            var expected = Guid.NewGuid();
            ICorrelationIdAccessor accessor = CorrelationId.From(expected);

            // Act
            var actual = Guid.Parse(accessor.CorrelationId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void CorrelationId_From_lambda_returns_value()
        {
            // Act
            ICorrelationIdAccessor accessor = CorrelationId.From(() => Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));

            // Assert
            Assert.True(Guid.TryParse(accessor.CorrelationId, out _));
        }

        //[Fact]
        //public static void EnsureRequestContext_WithARequestContext_CorrelationIdIsSet()
        //{
        //    // Act
        //    string correlationId = CorrelationId.GenerateValue();

        //    // Assert
        //    Assert.True(!string.IsNullOrWhiteSpace(correlationId));
        //}

        //[Fact]
        //public static void EnsureRequestContext_WithARequestContextContainingACorrelationId_CorrelationIdIsSet()
        //{
        //    // Arrange
        //    string correlationId = "MyCustomValue";

        //    // Act
        //    string outputCorrelationId = CorrelationId.EnsureValue(correlationId);

        //    // Assert
        //    outputCorrelationId.Should().NotBeNullOrWhiteSpace();
        //    outputCorrelationId.Should().Be(correlationId);
        //}

        //[Fact]
        //public static void GetCorrelationId_should_handle_null()
        //{
        //    // Arrange
        //    HttpResponseMessage response = null;

        //    // Act
        //    var correlationId = response.GetCorrelationId();

        //    // Assert
        //    correlationId.Should().BeNull();
        //}

        //[Fact]
        //public static void GetCorrelationId_should_handle_no_headers()
        //{
        //    // Arrange
        //    var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

        //    // Act
        //    var correlationId = response.GetCorrelationId();

        //    // Assert
        //    correlationId.Should().BeNull();
        //}

        //[Fact]
        //public static void GetCorrelationId_should_handle_missing_headers()
        //{
        //    // Arrange
        //    var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        //    response.Headers.Location = new Uri("a/b/c", UriKind.Relative);

        //    // Act
        //    var correlationId = response.GetCorrelationId();

        //    // Assert
        //    correlationId.Should().BeNull();
        //}
    }
}

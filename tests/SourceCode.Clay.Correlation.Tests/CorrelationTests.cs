namespace SourceCode.Clay.Correlation.Tests
{
    public static class CorrelationHelpersTests
    {
        /*
        [Fact]
        public static void EnsureRequestContext_WithARequestContext_CorrelationIdIsSet()
        {
            // Act
            string correlationId = CorrelationId.GenerateValue();

            // Assert
            Assert.True(!string.IsNullOrWhiteSpace(correlationId));
        }

        [Fact]
        public static void EnsureRequestContext_WithARequestContextContainingACorrelationId_CorrelationIdIsSet()
        {
            // Arrange
            string correlationId = "MyCustomValue";

            // Act
            //string outputCorrelationId = CorrelationId.EnsureValue(correlationId);

            //// Assert
            //outputCorrelationId.Should().NotBeNullOrWhiteSpace();
            //outputCorrelationId.Should().Be(correlationId);
        }

        [Fact]
        public static void GetCorrelationId_should_handle_null()
        {
            // Arrange
            HttpResponseMessage response = null;

            // Act
            var correlationId = response.GetCorrelationId();

            // Assert
            correlationId.Should().BeNull();
        }

        [Fact]
        public static void GetCorrelationId_should_handle_no_headers()
        {
            // Arrange
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            // Act
            var correlationId = response.GetCorrelationId();

            // Assert
            correlationId.Should().BeNull();
        }

        [Fact]
        public static void GetCorrelationId_should_handle_missing_headers()
        {
            // Arrange
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Headers.Location = new Uri("a/b/c", UriKind.Relative);

            // Act
            var correlationId = response.GetCorrelationId();

            // Assert
            correlationId.Should().BeNull();
        }
        */
    }
}

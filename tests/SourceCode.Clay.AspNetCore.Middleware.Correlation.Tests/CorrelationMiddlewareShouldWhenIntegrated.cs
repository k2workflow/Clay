using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    public class CorrelationMiddlewareShouldWhenIntegrated
        : IClassFixture<TestWebApplicationFactory<TestWebApplicationStartup>>
    {
        private readonly TestWebApplicationFactory<TestWebApplicationStartup> _factory;

        public CorrelationMiddlewareShouldWhenIntegrated(TestWebApplicationFactory<TestWebApplicationStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnAGeneratedCorrelationIdInResponseHeadersIfNoneIsSetInRequestHeaders()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(new Uri("http://localhost/"))
                .ConfigureAwait(false);

            // Assert
            response.Headers.Should().ContainHeader("X-Correlation-ID");
            response.Headers.GetValues("X-Correlation-ID").Should().HaveCount(1, "the X-Correlation-ID header should contain one value");
            var returnedCorrelationId = response.Headers.GetValues("X-Correlation-ID").First();
            Guid.TryParse(returnedCorrelationId, out Guid _).Should().BeTrue("the correlation id value should be parsable as a Guid");
        }

        [Fact]
        public async Task ReturnTheSameCorrelationIdInResponseHeadersIfOneIsSetInRequestHeaders()
        {
            // Arrange
            var expectedCorrelationId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            HttpClient client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Correlation-ID", expectedCorrelationId);

            // Act
            HttpResponseMessage response = await client.GetAsync(new Uri("http://localhost/"))
                .ConfigureAwait(false);

            // Assert
            response.Headers.Should().ContainHeader("X-Correlation-ID", expectedCorrelationId);
        }
    }
}

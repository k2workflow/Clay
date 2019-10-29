using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks;
using Xunit;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    public class CorrelationMiddlewareShould
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CallNextMiddlewareWithHttpContextTraceIdentifierSet(bool withLogger)
        {
            // Arrange
            var generatedId = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);

            HttpContext context = new DefaultHttpContext();
            var responseFeature = new MockHttpResponseFeature();
            context.Features.Set<IHttpResponseFeature>(responseFeature);
            var next = new MockMiddleware(async (_) => await responseFeature.CompleteAsync().ConfigureAwait(false));

            var accessor = new CorrelationContextAccessor();
            ILogger<CorrelationMiddleware> logger = withLogger ? new MockLogger() : null;
            IOptions<CorrelationOptions> options = Options.Create(new CorrelationOptions()
            {
                Headers =
                {
                    [0] = new CorrelationHeader(Constants.XCorrelationID, correlationIdGenerator: _ => generatedId)
                }
            });

            var middleware = new CorrelationMiddleware(next, accessor, logger, options);

            // Act
            await middleware.Invoke(context).ConfigureAwait(false);

            // Assert
            next.WasCalled.Should().Be(true, "the next middleware should be called");
            next.WithTraceIdentifier.Should().Be(generatedId, "the TraceIdentifier should have been set to the generared correlation id");
            context.Response.Headers.Should().Contain(Constants.XCorrelationID, generatedId);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GenerateANewCorrelationIdIfTheRequestDoesNotContainOne(bool withLogger)
        {
            // Arrange
            var generatedId = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);

            HttpContext context = new DefaultHttpContext();
            var responseFeature = new MockHttpResponseFeature();
            context.Features.Set<IHttpResponseFeature>(responseFeature);
            var next = new MockMiddleware(async (_) => await responseFeature.CompleteAsync());

            var accessor = new CorrelationContextAccessor();
            ILogger<CorrelationMiddleware> logger = withLogger ? new MockLogger() : null;
            IOptions<CorrelationOptions> options = Options.Create(new CorrelationOptions()
            {
                Headers =
                {
                    [0] = new CorrelationHeader(Constants.XCorrelationID, correlationIdGenerator: _ => generatedId)
                }
            });

            var middleware = new CorrelationMiddleware(next, accessor, logger, options);

            // Invoke middleware
            middleware.Invoke(context);

            // Assert
            next.WasCalled.Should().Be(true, "the next middleware should be called");
            next.WithTraceIdentifier.Should().Be(generatedId, "the TraceIdentifier should have been set");
            accessor.CorrelationContext.Should().NotBeNull("the middleware should initialize the CorrelationContext");
            accessor.CorrelationContext.CorrelationId.Should().Be(generatedId, "the middleware should initialize the CorrelationContext.CorrelatioId correctly with the generated value form the options provider function.");
            accessor.CorrelationContext.Headers.ContainsKey(Constants.XCorrelationID).Should().BeTrue();
            accessor.CorrelationContext.Headers[Constants.XCorrelationID].Should().Equal(new[] { generatedId }, "the middleware should initialize the CorrelationContext.Header correctly with the header from the options provided");
            context.Response.Headers.Should().Contain(Constants.XCorrelationID, generatedId);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UseTheExistingCorrelationIdIfTheRequestContainsOne(bool withLogger)
        {
            // Arrange
            var incomingCorrelationId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            HttpContext context = new DefaultHttpContext();
            var responseFeature = new MockHttpResponseFeature();
            context.Features.Set<IHttpResponseFeature>(responseFeature);

            // Add incoming correlation id
            context.Request.Headers.Add(Constants.XCorrelationID, incomingCorrelationId);

            var next = new MockMiddleware(async (_) => await responseFeature.CompleteAsync());

            var accessor = new CorrelationContextAccessor();
            ILogger<CorrelationMiddleware> logger = withLogger ? new MockLogger() : null;
            IOptions<CorrelationOptions> options = Options.Create(new CorrelationOptions()
            {
                Headers =
                {
                    [0] = new CorrelationHeader(Constants.XCorrelationID, correlationIdGenerator:
                        _ => throw new InvalidOperationException("Should not be called because a request correlation id was supplied"))
                }
            });

            var middleware = new CorrelationMiddleware(next, accessor, logger, options);

            // Invoke middleware
            middleware.Invoke(context);

            // Assert
            next.WasCalled.Should().Be(true, "the next middleware should be called");
            next.WithTraceIdentifier.Should().Be(incomingCorrelationId, "the TraceIdentifier should have been set");
            accessor.CorrelationContext.Should().NotBeNull("the middleware should initialize the CorrelationContext");
            accessor.CorrelationContext.CorrelationId.Should().Be(incomingCorrelationId, "the middleware should initialize the CorrelationContext.CorrelatioId correctly with the supplied value from the request");
            accessor.CorrelationContext.Headers.ContainsKey(Constants.XCorrelationID).Should().BeTrue();
            accessor.CorrelationContext.Headers[Constants.XCorrelationID].Should().Equal(new[] { incomingCorrelationId }, "the middleware should initialize the CorrelationContext.Header correctly with the header from the options provided");
            context.Response.Headers.Should().Contain(Constants.XCorrelationID, incomingCorrelationId);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UseTheExistingCorrelationIdsIfTheRequestContainsThem(bool withLogger)
        {
            // Arrange
            var incomingCorrelationId1 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var incomingCorrelationId2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            HttpContext context = new DefaultHttpContext();
            var responseFeature = new MockHttpResponseFeature();
            context.Features.Set<IHttpResponseFeature>(responseFeature);

            // Add incoming correlation id
            context.Request.Headers.Add(Constants.XCorrelationID, incomingCorrelationId1);
            context.Request.Headers.Add("tracestate", incomingCorrelationId2);

            var next = new MockMiddleware(async (_) => await responseFeature.CompleteAsync());

            var accessor = new CorrelationContextAccessor();
            Func<HttpContext, StringValues> generator = _ => throw new InvalidOperationException("Should not be called because a request correlation id was supplied");

            ILogger<CorrelationMiddleware> logger = withLogger ? new MockLogger() : null;
            IOptions<CorrelationOptions> options = Options.Create(new CorrelationOptions());

            options.Value.Headers.Clear();
            options.Value.Headers.Add(new CorrelationHeader(Constants.XCorrelationID, correlationIdGenerator: generator));
            options.Value.Headers.Add(new CorrelationHeader("tracestate", correlationIdGenerator: generator));

            var middleware = new CorrelationMiddleware(next, accessor, logger, options);

            // Invoke middleware
            middleware.Invoke(context);

            // Assert
            next.WasCalled.Should().Be(true, "the next middleware should be called");
            next.WithTraceIdentifier.Should().Be(incomingCorrelationId1, "the TraceIdentifier should have been set");
            accessor.CorrelationContext.Should().NotBeNull("the middleware should initialize the CorrelationContext");
            accessor.CorrelationContext.CorrelationId.Should().Be(incomingCorrelationId1, "the middleware should initialize the CorrelationContext.CorrelatioId correctly with the supplied value from the request");
            accessor.CorrelationContext.Headers.ContainsKey(Constants.XCorrelationID).Should().BeTrue();
            accessor.CorrelationContext.Headers[Constants.XCorrelationID].Should().Equal(new[] { incomingCorrelationId1 }, "the middleware should initialize the CorrelationContext.Header correctly with the header from the options provided");
            accessor.CorrelationContext.Headers.ContainsKey("tracestate").Should().BeTrue();
            accessor.CorrelationContext.Headers["tracestate"].Should().Equal(new[] { incomingCorrelationId2 }, "the middleware should initialize the CorrelationContext.Header correctly with the header from the options provided");
            context.Response.Headers.Should().Contain(Constants.XCorrelationID, incomingCorrelationId1);
            context.Response.Headers.Should().Contain("tracestate", incomingCorrelationId2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void NotReturnTheCorrelationIdHeaderIfConfiguredSo(bool withLogger)
        {
            // Arrange
            var incomingCorrelationId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            HttpContext context = new DefaultHttpContext();
            var responseFeature = new MockHttpResponseFeature();
            context.Features.Set<IHttpResponseFeature>(responseFeature);

            // Add incoming correlation id
            context.Request.Headers.Add(Constants.XCorrelationID, incomingCorrelationId);

            var next = new MockMiddleware(async (_) => await responseFeature.CompleteAsync());

            var accessor = new CorrelationContextAccessor();
            ILogger<CorrelationMiddleware> logger = withLogger ? new MockLogger() : null;
            IOptions<CorrelationOptions> options = Options.Create(new CorrelationOptions()
            {
                Headers =
                {
                    [0] = new CorrelationHeader(Constants.XCorrelationID, includeInResponse: false)
                }
            });

            var middleware = new CorrelationMiddleware(next, accessor, logger, options);

            // Invoke middleware
            middleware.Invoke(context);

            // Assert
            next.WasCalled.Should().Be(true, "the next middleware should be called");
            next.WithTraceIdentifier.Should().Be(incomingCorrelationId, "the TraceIdentifier should have been set");
            accessor.CorrelationContext.Should().NotBeNull("the middleware should initialize the CorrelationContext");
            accessor.CorrelationContext.Headers.ContainsKey(Constants.XCorrelationID).Should().BeTrue();
            accessor.CorrelationContext.Headers[Constants.XCorrelationID].Should().Equal(new[] { incomingCorrelationId }, "the middleware should initialize the CorrelationContext.Header correctly with the header from the options provided");
            context.Response.Headers.Should().NotContainKey(Constants.XCorrelationID, "because the middleware was configured to not return the correlation id in the response.");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UseTheExistingTraceIdentifierAsCorrelationIdIfConfiguredSo(bool withLogger)
        {
            // Arrange
            var traceIdentifier = "abc" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);

            HttpContext context = new DefaultHttpContext();
            var responseFeature = new MockHttpResponseFeature();
            context.Features.Set<IHttpResponseFeature>(responseFeature);
            var next = new MockMiddleware(async (_) => await responseFeature.CompleteAsync());

            // Set the trace identifier to use
            context.TraceIdentifier = traceIdentifier;

            var accessor = new CorrelationContextAccessor();
            ILogger<CorrelationMiddleware> logger = withLogger ? new MockLogger() : null;
            IOptions<CorrelationOptions> options = Options.Create(new CorrelationOptions()
            {
                Headers =
                {
                    [0] = new CorrelationHeader(Constants.XCorrelationID, useTraceIdentifier: true, correlationIdGenerator:
                        _ => throw new InvalidOperationException("Should not be called because a request correlation id was supplied"))
                }
            });

            var middleware = new CorrelationMiddleware(next, accessor, logger, options);

            // Invoke middleware
            middleware.Invoke(context);

            // Assert
            next.WasCalled.Should().Be(true, "the next middleware should be called");
            next.WithTraceIdentifier.Should().Be(traceIdentifier, "the TraceIdentifier should still be the same");
            accessor.CorrelationContext.Should().NotBeNull("the middleware should initialize the CorrelationContext");
            accessor.CorrelationContext.CorrelationId.Should().Be(traceIdentifier, "the middleware should initialize the CorrelationContext.CorrelatioId correctly with the trace identifier from the HttpContext");
            accessor.CorrelationContext.Headers.ContainsKey(Constants.XCorrelationID).Should().BeTrue();
            accessor.CorrelationContext.Headers[Constants.XCorrelationID].Should().Equal(new[] { traceIdentifier }, "the middleware should initialize the CorrelationContext.Header correctly with the header from the options provided");
            context.Response.Headers.Should().Contain(Constants.XCorrelationID, traceIdentifier);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void NotUpdateExistingTraceIdentifierWithTheCorrelationIdIfConfiguredSo(bool withLogger)
        {
            // Arrange
            var traceIdentifier = "abc" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var generatedId = "def" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);

            HttpContext context = new DefaultHttpContext();
            var responseFeature = new MockHttpResponseFeature();
            context.Features.Set<IHttpResponseFeature>(responseFeature);
            var next = new MockMiddleware(async (_) => await responseFeature.CompleteAsync());

            // Set the trace identifier to use
            context.TraceIdentifier = traceIdentifier;

            var accessor = new CorrelationContextAccessor();
            ILogger<CorrelationMiddleware> logger = withLogger ? new MockLogger() : null;
            IOptions<CorrelationOptions> options = Options.Create(new CorrelationOptions()
            {
                UpdateTraceIdentifier = false,
                Headers =
                {
                    [0] = new CorrelationHeader(Constants.XCorrelationID, correlationIdGenerator: _ => generatedId)
                }
            });

            var middleware = new CorrelationMiddleware(next, accessor, logger, options);

            // Invoke middleware
            middleware.Invoke(context);

            // Assert
            next.WasCalled.Should().Be(true, "the next middleware should be called");
            next.WithTraceIdentifier.Should().Be(traceIdentifier, "the TraceIdentifier should still be the same");
            accessor.CorrelationContext.Should().NotBeNull("the middleware should initialize the CorrelationContext");
            accessor.CorrelationContext.CorrelationId.Should().Be(generatedId, "the middleware should initialize the CorrelationContext.CorrelatioId correctly with the the generated id (not the trace identifier)");
            accessor.CorrelationContext.Headers.ContainsKey(Constants.XCorrelationID).Should().BeTrue();
            accessor.CorrelationContext.Headers[Constants.XCorrelationID].Should().Equal(new[] { generatedId }, "the middleware should initialize the CorrelationContext.Header correctly with the header from the options provided");
            context.Response.Headers.Should().Contain(Constants.XCorrelationID, generatedId, "the generated id should be used (not the trace identifier)");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LogWarningIfAnotherMiddlewareAddsACorrelationIdIfLoggerIsSupplied(bool withLogger)
        {
            // Arrange
            var generatedId = "generatedFromOptions" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var otherId = "customGeneratedByOtherMiddleware" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);

            HttpContext context = new DefaultHttpContext();
            var responseFeature = new MockHttpResponseFeature();
            context.Features.Set<IHttpResponseFeature>(responseFeature);

            var last = new MockMiddleware(async (_) => await responseFeature.CompleteAsync());
            var next = new MockMiddleware((HttpContext thisContext) =>
            {
                thisContext.Response.Headers.Add(Constants.XCorrelationID, otherId);
            }, last);

            var accessor = new CorrelationContextAccessor();
            MockLogger logger = withLogger ? new MockLogger() : null;
            var options = new CorrelationOptions()
            {
                Headers =
                {
                    [0] = new CorrelationHeader(Constants.XCorrelationID, correlationIdGenerator: _ => generatedId)
                }
            };

            var middleware = new CorrelationMiddleware(next, accessor, logger, options);

            // Invoke middleware
            middleware.Invoke(context);

            // Assert
            next.WasCalled.Should().Be(true, "the next middleware should be called");
            next.WithTraceIdentifier.Should().Be(generatedId, "the TraceIdentifier should have been set");
            accessor.CorrelationContext.Should().NotBeNull("the middleware should initialize the CorrelationContext");
            accessor.CorrelationContext.CorrelationId.Should().Be(generatedId, "the middleware should initialize the CorrelationContext.CorrelatioId correctly with the generated value form the options provider function.");
            accessor.CorrelationContext.Headers.ContainsKey(Constants.XCorrelationID).Should().BeTrue();
            context.Response.Headers.Should().Contain(Constants.XCorrelationID, otherId, "the middleware should not override a custom correlation id if added by other middleware");
            // TBD: Should both be added?
            if (withLogger)
            {
                var expectedMessage = string.Format(CultureInfo.InvariantCulture, Properties.Resources.CorrelationIdAlreadyAddedToResponse, Constants.XCorrelationID, otherId, generatedId);
                logger.Messages.Any(element =>
                    element.LogLevel == LogLevel.Warning &&
                    element.Formatter.Invoke(element.State, element.Exception).IndexOf(expectedMessage, StringComparison.OrdinalIgnoreCase) > -1
                ).Should().BeTrue($"a warning message '{expectedMessage}' should have been logged containing the expected message");
            }
        }

        [Fact]
        public void ThrowIfNullNextContextWasSuppliedToInvoke()
        {
            // Arrange
            var next = new MockMiddleware();
            var accessor = new MockCorrelationContextAccessor();
            var logger = new MockLogger();
            var options = new CorrelationOptions();
            var middleware = new CorrelationMiddleware(next, accessor, logger, Options.Create(options));

            // Act
            Action invoker = () => middleware.Invoke(null);

            // Assert
            invoker.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("context");
        }

        [Fact]
        public void ThrowIfNullNextDelegateWasSuppliedToConstructor()
        {
            // Arrange
            var next = (RequestDelegate)null;
            var accessor = new MockCorrelationContextAccessor();
            var logger = new MockLogger();
            var options = new CorrelationOptions();

            // Act
            Func<CorrelationMiddleware> factory = () => new CorrelationMiddleware(next, accessor, logger, Options.Create(options));

            // Assert
            factory.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("next");
        }

        [Fact]
        public void NotThrowIfNullAccessorWasSuppliedToConstructor()
        {
            // Arrange
            var next = new MockMiddleware();
            var accessor = (ICorrelationContextAccessor)null;
            var logger = new MockLogger();
            var options = new CorrelationOptions();

            // Act
            Func<CorrelationMiddleware> factory = () => new CorrelationMiddleware(next, accessor, logger, Options.Create(options));

            // Assert
            factory.Should().NotThrow();
        }

        [Fact]
        public void NotThrowIfNullOptionsWasSuppliedToConstructor()
        {
            // Arrange
            var next = new MockMiddleware();
            var accessor = new MockCorrelationContextAccessor();
            var logger = new MockLogger();
            var options = (CorrelationOptions)null;

            // Act
            Func<CorrelationMiddleware> factory = () => new CorrelationMiddleware(next, accessor, logger, options);

            // Assert
            factory.Should().NotThrow();
        }

        [Fact]
        public void NotThrowIfNullIOptionsWasSuppliedToConstructor()
        {
            // Arrange
            var next = new MockMiddleware();
            var accessor = new MockCorrelationContextAccessor();
            var logger = new MockLogger();
            var options = (IOptions<CorrelationOptions>)null;

            // Act
            Func<CorrelationMiddleware> factory = () => new CorrelationMiddleware(next, accessor, logger, options);

            // Assert
            factory.Should().NotThrow();
        }
    }
}

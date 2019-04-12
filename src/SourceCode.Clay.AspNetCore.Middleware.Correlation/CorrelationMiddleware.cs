using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using static SourceCode.Clay.AspNetCore.Middleware.Correlation.Properties.Resources;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    /// <summary>
    /// Represents the correlationid handling middleware for the application.
    /// </summary>
    public sealed class CorrelationMiddleware
    {
        private readonly ICorrelationContextAccessor _accessor;
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationMiddleware> _logger;
        private readonly CorrelationOptions _options;

        /// <summary>
        /// Creates a new instance of the CorrelationIdMiddleware.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="accessor">The current <see cref="ICorrelationContextAccessor"/> (optional).</param>
        /// <param name="logger">The current <see cref="ILogger{T}"/> (optional).</param>
        /// <param name="options">The configuration options.</param>
        public CorrelationMiddleware(RequestDelegate next, ICorrelationContextAccessor accessor = default, ILogger<CorrelationMiddleware> logger = default, IOptions<CorrelationOptions> options = default)
            : this(next, accessor, logger, options?.Value)
        {
        }

        /// <summary>
        /// Creates a new instance of the CorrelationIdMiddleware.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="accessor">The current <see cref="ICorrelationContextAccessor"/> (optional).</param>
        /// <param name="logger">The current <see cref="ILogger{T}"/> (optional).</param>
        /// <param name="options">The configuration options.</param>
        public CorrelationMiddleware(RequestDelegate next, ICorrelationContextAccessor accessor = default, ILogger<CorrelationMiddleware> logger = default, CorrelationOptions options = default)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _accessor = accessor;
            _logger = logger;
            _options = options ?? new CorrelationOptions();
            if (_options.CorrelationIdGenerator == null)
                throw new ArgumentNullException($"{nameof(options)}.{nameof(options.CorrelationIdGenerator)}");
        }

        internal CorrelationOptions Options { get { return _options; } }

        /// <summary>
        /// Processes a request to synchronise TraceIdentifier and Correlation ID headers.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        public Task Invoke(HttpContext context)
        {
            StringValues correlationId = GetCurrentCorrelationId(context);

            if (StringValues.IsNullOrEmpty(correlationId))
            {
                correlationId = _options.CorrelationIdGenerator(context);
                _logger?.LogDebug(NoExistingRequestCorrelationIDFound, correlationId);
            }
            else
            {
                _logger?.LogDebug(ExistingRequestCorrelationIDFound, correlationId);
            }

            // Create correlation context from the correlationId and header
            if (_accessor != null)
            {
                _accessor.CorrelationContext = new CorrelationContext(correlationId, _options.Header);
            }

            if (_options.UpdateTraceIdentifier && context.TraceIdentifier != correlationId)
            {
                _logger?.LogDebug(UpdatingHttpContextTraceIdentifier, context.TraceIdentifier, correlationId);
                context.TraceIdentifier = correlationId;
            }

            if (_options.IncludeInResponse)
            {
                // Add delegate to add correlation header just before the response headers are sent to the client.
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(_options.Header))
                    {
                        _logger?.LogDebug(AddCorrelationIdToResponse, correlationId);
                        context.Response.Headers.Add(_options.Header, correlationId);
                    }
                    else
                    {
                        _logger?.LogWarning(CorrelationIdAlreadyAddedToResponse, _options.Header, context.Response.Headers[_options.Header], correlationId);
                    }

                    return Task.CompletedTask;
                });
            }

            return _next(context);
        }

        private StringValues GetCurrentCorrelationId(HttpContext context)
        {
            var headerExists = context.Request.Headers.TryGetValue(_options.Header, out StringValues headerValue);
            if (headerExists && !StringValues.IsNullOrEmpty(headerValue))
                return headerValue;

            if (_options.UseTraceIdentifier && !string.IsNullOrEmpty(context.TraceIdentifier))
                return context.TraceIdentifier;

            return default;
        }
    }
}

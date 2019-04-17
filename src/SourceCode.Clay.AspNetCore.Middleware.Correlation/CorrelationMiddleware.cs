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

        internal CorrelationOptions Options { get; }

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
            Options = options ?? new CorrelationOptions();
            if (Options.CorrelationIdGenerator == null)
                throw new ArgumentNullException($"{nameof(options)}.{nameof(options.CorrelationIdGenerator)}");
        }

        /// <summary>
        /// Processes a request to synchronise TraceIdentifier and Correlation ID headers.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        public Task Invoke(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            StringValues correlationId = GetCurrentCorrelationId(context);

            if (StringValues.IsNullOrEmpty(correlationId))
            {
                correlationId = Options.CorrelationIdGenerator(context);
                _logger?.LogDebug(NoExistingRequestCorrelationIDFound, correlationId);
            }
            else
            {
                _logger?.LogDebug(ExistingRequestCorrelationIDFound, correlationId);
            }

            // Create correlation context from the correlationId and header
            if (_accessor != null)
            {
                _accessor.CorrelationContext = new CorrelationContext(correlationId, Options.Header);
            }

            if (Options.UpdateTraceIdentifier && context.TraceIdentifier != correlationId)
            {
                _logger?.LogDebug(UpdatingHttpContextTraceIdentifier, context.TraceIdentifier, correlationId);
                context.TraceIdentifier = correlationId;
            }

            if (Options.IncludeInResponse)
            {
                // Add delegate to add correlation header just before the response headers are sent to the client.
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(Options.Header))
                    {
                        _logger?.LogDebug(AddCorrelationIdToResponse, correlationId);
                        context.Response.Headers.Add(Options.Header, correlationId);
                    }
                    else
                    {
                        _logger?.LogWarning(CorrelationIdAlreadyAddedToResponse, Options.Header, context.Response.Headers[Options.Header], correlationId);
                    }

                    return Task.CompletedTask;
                });
            }

            return _next(context);
        }

        private StringValues GetCurrentCorrelationId(HttpContext context)
        {
            var headerExists = context.Request.Headers.TryGetValue(Options.Header, out StringValues headerValue);
            if (headerExists && !StringValues.IsNullOrEmpty(headerValue))
                return headerValue;

            if (Options.UseTraceIdentifier && !string.IsNullOrEmpty(context.TraceIdentifier))
                return context.TraceIdentifier;

            return default;
        }
    }
}

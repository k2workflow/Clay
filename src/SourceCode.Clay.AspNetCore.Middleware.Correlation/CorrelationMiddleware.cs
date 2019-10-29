using System;
using System.Collections.Generic;
using System.Globalization;
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
        }

        /// <summary>
        /// Processes a request to synchronise TraceIdentifier and Correlation ID headers.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        public Task Invoke(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            IReadOnlyDictionary<CorrelationHeader, StringValues> headers = GetCurrentCorrelationId(context, out StringValues correlationId);

            if (StringValues.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
                _logger?.LogDebug(NoExistingRequestCorrelationIDFound, correlationId);
            }
            else
            {
                _logger?.LogDebug(ExistingRequestCorrelationIDFound, correlationId);
            }

            // Create correlation context from the correlationId and header
            if (_accessor != null)
            {
                _accessor.CorrelationContext = new CorrelationContext(correlationId, headers);
            }

            if (Options.UpdateTraceIdentifier && context.TraceIdentifier != correlationId)
            {
                _logger?.LogDebug(UpdatingHttpContextTraceIdentifier, context.TraceIdentifier, correlationId);
                context.TraceIdentifier = correlationId;
            }

            // Add delegate to add correlation header just before the response headers are sent to the client.
            context.Response.OnStarting(() =>
            {
                foreach ((CorrelationHeader header, StringValues value) in headers)
                {
                    if (!header.IncludeInResponse) continue;

                    if (!context.Response.Headers.ContainsKey(header.Name))
                    {
                        _logger?.LogDebug(AddCorrelationIdToResponse, value);
                        context.Response.Headers.Add(header.Name, value);
                    }
                    else
                    {
                        _logger?.LogWarning(CorrelationIdAlreadyAddedToResponse, header.Name, context.Response.Headers[header.Name], value);
                    }
                }
                return Task.CompletedTask;
            });

            return _next(context);
        }

        private IReadOnlyDictionary<CorrelationHeader, StringValues> GetCurrentCorrelationId(HttpContext context, out StringValues correlationId)
        {
            correlationId = StringValues.Empty;

            var result = new Dictionary<CorrelationHeader, StringValues>();
            for (var i = 0; i < Options.Headers.Count; i++)
            {
                CorrelationHeader header = Options.Headers[i];
                var headerExists = context.Request.Headers.TryGetValue(header.Name, out StringValues headerValue);
                if (headerExists && !StringValues.IsNullOrEmpty(headerValue))
                {
                    if (correlationId.Count == 0) correlationId = headerValue;
                    result.Add(header, headerValue);
                }
                else if (header.UseTraceIdentifier && !string.IsNullOrEmpty(context.TraceIdentifier))
                {
                    headerValue = context.TraceIdentifier;
                    if (correlationId.Count == 0) correlationId = headerValue;
                    result.Add(header, headerValue);
                }
                else if (header.CorrelationIdGenerator != null)
                {
                    headerValue = header.CorrelationIdGenerator(context);
                    if (correlationId.Count == 0) correlationId = headerValue;
                    result.Add(header, headerValue);
                }
            }

            return result;
        }
    }
}

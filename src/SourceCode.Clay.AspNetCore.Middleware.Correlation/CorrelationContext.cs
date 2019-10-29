using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using static SourceCode.Clay.AspNetCore.Middleware.Correlation.Properties.Resources;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    /// <summary>
    /// Provides access to per request correlation properties.
    /// </summary>
    public class CorrelationContext
    {
        /// <summary>
        /// Gets a list of the correlation ID headers that were set in the request.
        /// </summary>
        public IReadOnlyDictionary<CorrelationHeader, StringValues> Headers { get; }

        /// <summary>
        /// The Correlation ID which is applicable to the current request.
        /// </summary>
        /// <remarks>
        /// This is the value of the first header that was found.
        /// </remarks>
        public string CorrelationId { get; }

        internal CorrelationContext(string correlationId, IReadOnlyDictionary<CorrelationHeader, StringValues> headers)
        {
            if (string.IsNullOrWhiteSpace(correlationId))
                throw new ArgumentException(ArgumentNullOrWhitespace, nameof(correlationId));

            CorrelationId = correlationId;
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }
    }
}

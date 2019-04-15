using System;
using static SourceCode.Clay.AspNetCore.Middleware.Correlation.Properties.Resources;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    /// <summary>
    /// Provides access to per request correlation properties.
    /// </summary>
    public class CorrelationContext
    {
        internal CorrelationContext(string correlationId, string header)
        {
            if (string.IsNullOrWhiteSpace(correlationId))
                throw new ArgumentException(ArgumentNullOrWhitespace, nameof(correlationId));

            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException(ArgumentNullOrWhitespace, nameof(header));

            CorrelationId = correlationId;
            Header = header;
        }

        /// <summary>
        /// The Correlation ID which is applicable to the current request.
        /// </summary>
        public string CorrelationId { get; }

        /// <summary>
        /// The name of the header from which the Correlation ID is read/written.
        /// </summary>
        public string Header { get; }
    }
}

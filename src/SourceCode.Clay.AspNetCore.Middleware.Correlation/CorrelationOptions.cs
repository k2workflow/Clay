using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    /// <summary>
    /// Represents the options for the <see cref="CorrelationMiddleware"/>.
    /// </summary>
    public class CorrelationOptions
    {
        /// <summary>
        /// The default http header to use.
        /// </summary>
        public const string DefaultHeader = "X-Correlation-ID";
        
        /// <summary>
        /// Gets or sets whether the <see cref="HttpContext.TraceIdentifier"/> of the context should be updated (defaults to true).
        /// </summary>
        public bool UpdateTraceIdentifier { get; set; } = true;

        /// <summary>
        /// Gets or sets the list of headers.
        /// </summary>
        public IList<CorrelationHeader> Headers { get; }
                /// <summary>
        /// Creates a new instance of the <see cref="CorrelationOptions"/> type.
        /// </summary>
        public CorrelationOptions()
        {
            Headers = new List<CorrelationHeader>()
            {
                new CorrelationHeader(DefaultHeader)
            };
        }
    }
}

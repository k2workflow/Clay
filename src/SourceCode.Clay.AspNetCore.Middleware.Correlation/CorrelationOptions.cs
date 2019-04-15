using System;
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
        internal const string DefaultHeader = "X-Correlation-ID";

        /// <summary>
        /// Gets or sets whether the <see cref="HttpContext.TraceIdentifier"/> of the context should be used if not correlation id
        /// is present in request (defaults to false).
        /// If <c>false</c>, <see cref="CorrelationIdGenerator"/> is invoked to generate the correlation id.
        /// </summary>
        public bool UseTraceIdentifier { get; internal set; } = false;

        /// <summary>
        /// Gets or sets whether the <see cref="HttpContext.TraceIdentifier"/> of the context should be updated (defaults to true).
        /// </summary>
        public bool UpdateTraceIdentifier { get; internal set; } = true;

        /// <summary>
        /// Gets or sets whether the http response is updated to include the correlation id (defaults to true).
        /// </summary>
        public bool IncludeInResponse { get; internal set; } = true;

        /// <summary>
        /// Gets or sets the name of the header to use for correlation ids.
        /// </summary>
        public string Header { get; internal set; } = DefaultHeader;

        /// <summary>
        /// Get function used to generate new correlation ids.
        /// </summary>
        /// <remarks>
        /// <para>The default generator function generates a random <see cref="Guid"/> string.</para>
        /// <para>The string is generated using <see cref="Guid.ToString(string, IFormatProvider)"/> with
        /// format <c>"D"</c> and <see cref="CultureInfo.InvariantCulture"/> as arguments.</para>
        /// <para>Example: <c>12341234-1234-1234-1234-123412341234</c></para>
        /// </remarks>
        public Func<HttpContext, StringValues> CorrelationIdGenerator { get; set; } = (HttpContext _)
            => Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
    }
}

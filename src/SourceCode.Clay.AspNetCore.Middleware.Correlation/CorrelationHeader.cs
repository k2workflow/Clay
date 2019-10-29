using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    /// <summary>
    /// Represents a correlation ID header.
    /// </summary>
    public readonly struct CorrelationHeader : IEquatable<CorrelationHeader>
    {
        private static readonly Func<HttpContext, StringValues> s_defaultCorrelationIdGenerator = _
            => Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

        /// <summary>
        /// The name of the correlation ID header.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the header should be included in responses.
        /// </summary>
        public bool IncludeInResponse { get; }

        /// <summary>
        /// Gets or sets whether the <see cref="HttpContext.TraceIdentifier"/> of the context should be used if no correlation ID
        /// is present in request (defaults to false).
        /// If <c>false</c>, <see cref="CorrelationIdGenerator"/> is invoked to generate the correlation id.
        /// </summary>
        public bool UseTraceIdentifier { get; }

        /// <summary>
        /// Get function used to generate new correlation ids.
        /// </summary>
        /// <remarks>
        /// <para>The default generator function generates a random <see cref="Guid"/> string.</para>
        /// <para>The string is generated using <see cref="Guid.ToString(string, IFormatProvider)"/> with
        /// format <c>"D"</c> and <see cref="CultureInfo.InvariantCulture"/> as arguments.</para>
        /// <para>Example: <c>12341234-1234-1234-1234-123412341234</c></para>
        /// </remarks>
        public Func<HttpContext, StringValues> CorrelationIdGenerator { get; }

        /// <summary>
        /// Creates a new <see cref="CorrelationHeader"/> value.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="includeInResponse">A value indicating whether to include the header in responses, defaults to <c>true</c>.</param>
        /// <param name="useTraceIdentifier">A value indicating whether to copy the <see cref="HttpContext.TraceIdentifier"/> if the header is not present.</param>
        public CorrelationHeader(string name, bool includeInResponse = true, bool useTraceIdentifier = false, Func<HttpContext, StringValues> correlationIdGenerator = null)
        {
            Name = name;
            IncludeInResponse = includeInResponse;
            UseTraceIdentifier = useTraceIdentifier;
            CorrelationIdGenerator = correlationIdGenerator ?? s_defaultCorrelationIdGenerator;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is CorrelationHeader header && Equals(header);

        /// <inheritdoc />
        public bool Equals(CorrelationHeader other) => Name == other.Name;

        /// <inheritdoc />
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);

        /// <inheritdoc />
        public static bool operator ==(CorrelationHeader left, CorrelationHeader right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(CorrelationHeader left, CorrelationHeader right) => !(left == right);

        /// <inheritdoc />
        public static implicit operator CorrelationHeader(string name) => new CorrelationHeader(name);
    }
}

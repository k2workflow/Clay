using System;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    /// <summary>
    /// Represents a correlation ID header.
    /// </summary>
    public readonly struct CorrelationHeader : IEquatable<CorrelationHeader>
    {
        /// <summary>
        /// The name of the correlation ID header.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the header should be included in responses.
        /// </summary>
        public bool IncludeInResponse { get; }

        /// <summary>
        /// Creates a new <see cref="CorrelationHeader"/> value.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="includeInResponse">A value indicating whether to include the header in responses, defaults to <c>true</c>.</param>
        public CorrelationHeader(string name, bool includeInResponse = true)
        {
            Name = name;
            IncludeInResponse = includeInResponse;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is CorrelationHeader header && Equals(header);

        /// <inheritdoc />
        public bool Equals(CorrelationHeader other)
        {
            return Name == other.Name &&
                   IncludeInResponse == other.IncludeInResponse;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = -406407647;
            hashCode = hashCode * -1521134295 + StringComparer.Ordinal.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + IncludeInResponse.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc />
        public static bool operator ==(CorrelationHeader left, CorrelationHeader right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(CorrelationHeader left, CorrelationHeader right) => !(left == right);
    }
}

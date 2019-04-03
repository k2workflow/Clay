using System;
using System.Diagnostics;
using System.Globalization;
using SourceCode.Clay.Correlation.Internal;

namespace SourceCode.Clay.Correlation
{
    /// <summary>
    /// Factory methods for setting the correlation identifier.
    /// </summary>
    public static class CorrelationId
    {
        /// <summary>
        /// Creates a <see cref="ICorrelationIdAccessor"/> instance using a constant value.
        /// </summary>
        /// <param name="correlationId">The constant <see cref="string"/> value to use as the correlation identifier.</param>
        [DebuggerStepThrough]
        public static ICorrelationIdAccessor From(string correlationId)
            => new ConstantAccessor(correlationId);

        /// <summary>
        /// Creates a <see cref="ICorrelationIdAccessor"/> instance using a constant value.
        /// </summary>
        /// <param name="correlationId">The constant <see cref="Guid"/> value to use as the correlation identifier.</param>
        [DebuggerStepThrough]
        public static ICorrelationIdAccessor From(Guid correlationId)
            => new ConstantAccessor(correlationId.ToString("D", CultureInfo.InvariantCulture));

        /// <summary>
        /// Creates a <see cref="ICorrelationIdAccessor"/> instance using a function.
        /// </summary>
        /// <param name="getter">The function that returns the correlationId.</param>
        [DebuggerStepThrough]
        public static ICorrelationIdAccessor From(Func<string> getter)
            => new LambdaAccessor(getter);

        /// <summary>
        /// Ensures the specified correlation identifier value is not null or whitespace.
        /// Creates a random <see cref="Guid"/>-based value if it is.
        /// </summary>
        /// <param name="correlationId">The correlation identifier to check.</param>
        //public static string EnsureValue(string correlationId)
        //    => string.IsNullOrWhiteSpace(correlationId) ? GenerateValue() : correlationId;

        /// <summary>
        /// Creates a <see cref="ICorrelationIdAccessor"/> instance using a randomly generated <see cref="Guid"/>.
        /// </summary>
        //[DebuggerStepThrough]
        //public static ICorrelationIdAccessor FromGenerated()
        //    => new ConstantAccessor(GenerateValue());
    }
}

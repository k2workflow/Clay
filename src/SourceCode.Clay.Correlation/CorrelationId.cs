using System;
using System.Diagnostics;
using System.Globalization;
using SourceCode.Clay.Correlation.Internal;

namespace SourceCode.Clay.Correlation
{
    /// <summary>
    /// Factory methods for creating a <see cref="ICorrelationIdAccessor"/> instance.
    /// </summary>
    public static class CorrelationId
    {
        /// <summary>
        /// Creates a <see cref="ICorrelationIdAccessor"/> instance using a constant <see cref="string"/> value.
        /// </summary>
        /// <param name="correlationId">The constant <see cref="string"/> value to use as the correlation identifier.</param>
        [DebuggerStepThrough]
        public static ICorrelationIdAccessor From(string correlationId)
            => new ConstantAccessor(correlationId);

        /// <summary>
        /// Creates a <see cref="ICorrelationIdAccessor"/> instance using a constant <see cref="Guid"/> value.
        /// The value will be converted to a string using the "D" format specifier ("00000000-0000-0000-0000-000000000000").
        /// </summary>
        /// <param name="correlationId">The constant <see cref="Guid"/> value to use as the correlation identifier.</param>
        [DebuggerStepThrough]
        public static ICorrelationIdAccessor From(Guid correlationId)
            => new ConstantAccessor(correlationId.ToString("D", CultureInfo.InvariantCulture));

        /// <summary>
        /// Creates a <see cref="ICorrelationIdAccessor"/> instance using an expression that is
        /// re-evaluated each time it is called.
        /// </summary>
        /// <param name="getter">The function that returns the correlationId.</param>
        [DebuggerStepThrough]
        public static ICorrelationIdAccessor From(Func<string> getter)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            return new LambdaAccessor(getter);
        }
    }
}

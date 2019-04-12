using System;
using System.Diagnostics;
using SourceCode.Clay.Correlation.Internal;

namespace SourceCode.Clay.Correlation
{
    /// <summary>
    /// Factory methods for creating a <see cref="ICorrelationIdAccessor{T}"/> instance.
    /// </summary>
    public static class CorrelationId
    {
        /// <summary>
        /// Creates a <see cref="ICorrelationIdAccessor{T}"/> instance using a constant <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="correlationId">The constant <typeparamref name="T"/> value to use as the correlation identifier.</param>
        [DebuggerStepThrough]
        public static ICorrelationIdAccessor<T> From<T>(T correlationId)
            => new ConstantAccessor<T>(correlationId);

        /// <summary>
        /// Creates a <see cref="ICorrelationIdAccessor{T}"/> instance using an expression that is
        /// re-evaluated each time it is called.
        /// </summary>
        /// <param name="getter">The function that returns the correlation identifier.</param>
        [DebuggerStepThrough]
        public static ICorrelationIdAccessor<T> From<T>(Func<T> getter)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            return new LambdaAccessor<T>(getter);
        }
    }
}

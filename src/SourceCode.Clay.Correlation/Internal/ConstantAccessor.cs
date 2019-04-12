using System.Diagnostics;

namespace SourceCode.Clay.Correlation.Internal
{
    /// <summary>
    /// Represents a <see cref="ICorrelationIdAccessor{T}"/> instance that returns a constant value.
    /// </summary>
    [DebuggerStepThrough]
    internal sealed class ConstantAccessor<T> : ICorrelationIdAccessor<T>
    {
        public T CorrelationId { get; }

        public ConstantAccessor(T value)
            => CorrelationId = value;
    }
}

using System.Diagnostics;

namespace SourceCode.Clay.Correlation.Internal
{
    /// <summary>
    /// Represents a <see cref="ICorrelationIdAccessor"/> instance that returns a constant value.
    /// </summary>
    [DebuggerStepThrough]
    internal sealed class ConstantAccessor : ICorrelationIdAccessor
    {
        public string CorrelationId { get; }

        public ConstantAccessor(string value)
            => CorrelationId = value;
    }
}

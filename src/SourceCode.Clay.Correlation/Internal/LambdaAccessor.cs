using System;
using System.Diagnostics;

namespace SourceCode.Clay.Correlation.Internal
{
    /// <summary>
    /// Represents a <see cref="ICorrelationIdAccessor"/> instance that returns a lambda-based value.
    /// </summary>
    [DebuggerStepThrough]
    internal sealed class LambdaAccessor<T> : ICorrelationIdAccessor<T>
    {
        private readonly Func<T> _getter;

        public T CorrelationId => _getter();

        public LambdaAccessor(Func<T> getter)
        {
            Debug.Assert(getter != null);
            _getter = getter;
        }
    }
}

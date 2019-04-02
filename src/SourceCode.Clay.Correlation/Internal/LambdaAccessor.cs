using System;
using System.Diagnostics;

namespace SourceCode.Clay.Correlation.Internal
{
    /// <summary>
    /// Represents a <see cref="ICorrelationIdAccessor"/> instance that returns a lambda-based value.
    /// </summary>
    [DebuggerStepThrough]
    internal sealed class LambdaAccessor : ICorrelationIdAccessor
    {
        private readonly Func<string> _getter;

        public string CorrelationId => _getter();

        public LambdaAccessor(Func<string> getter)
        {
            Debug.Assert(getter != null);
            _getter = getter;
        }
    }
}

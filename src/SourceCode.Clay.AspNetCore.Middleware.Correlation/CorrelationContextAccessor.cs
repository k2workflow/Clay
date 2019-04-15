using System.Threading;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    /// <inheritdoc />
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        private static readonly AsyncLocal<CorrelationContext> s_correlationContext = new AsyncLocal<CorrelationContext>();

        /// <inheritdoc />
        public CorrelationContext CorrelationContext
        {
            get => s_correlationContext.Value;
            set => s_correlationContext.Value = value;
        }
    }
}

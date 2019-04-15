using SourceCode.Clay.AspNetCore.Middleware.Correlation;

namespace MvcCorrelationCustomSample.Services
{
    public class ScopedService
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public ScopedService(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public string GetCorrelationId => _correlationContextAccessor.CorrelationContext.CorrelationId;
    }
}

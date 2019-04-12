using SourceCode.Clay.AspNetCore.Middleware.Correlation;

namespace MvcCorrelationCustomSample.Services
{
    public class TransientService
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public TransientService(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public string GetCorrelationId => _correlationContextAccessor.CorrelationContext.CorrelationId;
    }
}

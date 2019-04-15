using SourceCode.Clay.AspNetCore.Middleware.Correlation;

namespace MvcCorrelationCustomSample.Services
{
    public class SingletonService
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public SingletonService(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public string GetCorrelationId => _correlationContextAccessor.CorrelationContext.CorrelationId;
    }
}

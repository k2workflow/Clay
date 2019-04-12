using SourceCode.Clay.AspNetCore.Middleware.Correlation;

namespace MvcCorrelationCustomSample
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

    public class TransientService
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public TransientService(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public string GetCorrelationId => _correlationContextAccessor.CorrelationContext.CorrelationId;
    }

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

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockCorrelationContextAccessor : ICorrelationContextAccessor
    {
        public CorrelationContext CorrelationContext { get; set; }
    }
}

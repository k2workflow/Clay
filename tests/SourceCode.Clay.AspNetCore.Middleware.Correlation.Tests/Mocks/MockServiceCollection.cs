using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockServiceCollection : Collection<ServiceDescriptor>, IServiceCollection
    {
    }
}

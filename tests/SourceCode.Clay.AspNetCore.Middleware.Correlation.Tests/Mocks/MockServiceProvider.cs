using System;
using System.Collections.Generic;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockServiceProvider : IServiceProvider
    {
        public IDictionary<Type, object> ServiceInstances { get; } = new Dictionary<Type, object>();

        public object GetService(Type serviceType)
        {
            ServiceInstances.TryGetValue(serviceType, out object serviceInstance);
            return serviceInstance;
        }
    }
}

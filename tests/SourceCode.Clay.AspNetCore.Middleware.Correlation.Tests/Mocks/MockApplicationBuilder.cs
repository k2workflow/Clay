using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockApplicationBuilder : IApplicationBuilder
    {
        public IServiceProvider ApplicationServices { get; set; }
        public IFeatureCollection ServerFeatures { get; }
        public IDictionary<string, object> Properties { get; }

        public RequestDelegate Build()
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder New()
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            if (OnUseCalled != null)
            {
                OnUseCalled.Invoke(middleware);
                return this;
            }
            else
                throw new NotImplementedException();
        }

        public Action<Func<RequestDelegate, RequestDelegate>> OnUseCalled { get; set; }

        internal static MockApplicationBuilder MockUpWithRequiredServices(out MockCorrelationContextAccessor accessor, out MockLogger logger, out MockServiceProvider provider)
        {
            var app = new MockApplicationBuilder();
            logger = new MockLogger();
            accessor = new MockCorrelationContextAccessor();
            provider = new MockServiceProvider();
            provider.ServiceInstances[typeof(ILogger<CorrelationMiddleware>)] = logger;
            provider.ServiceInstances[typeof(ICorrelationContextAccessor)] = accessor;
            app.ApplicationServices = provider;

            return app;
        }
    }
}

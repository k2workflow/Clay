using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockMiddleware
    {
        private Action<HttpContext> _onInvoke;
        private RequestDelegate _next;

        public MockMiddleware(Action<HttpContext> onInvoke = default, RequestDelegate next = default)
        {
            _onInvoke = onInvoke;
            _next = next;
        }

        public bool WasCalled { get; private set; }
        public string WithTraceIdentifier { get; private set; }

        public Task Invoke(HttpContext context)
        {
            WasCalled = true;
            WithTraceIdentifier = context?.TraceIdentifier;

            _onInvoke?.Invoke(context);

            if (_next != default)
                return _next(context);
            else
                return Task.CompletedTask;
        }

        public static implicit operator RequestDelegate(MockMiddleware value)
        {
            return value.ToRequestDelegate();
        }

        public RequestDelegate ToRequestDelegate()
        {
            return new RequestDelegate(this.Invoke);
        }
    }
}

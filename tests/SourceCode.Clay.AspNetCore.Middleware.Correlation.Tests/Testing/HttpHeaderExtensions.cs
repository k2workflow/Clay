using System.Diagnostics;
using System.Net.Http.Headers;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    [DebuggerNonUserCode]
    public static class HttpHeadersExtensions
    {
        public static HttpHeadersAssertions Should(this HttpHeaders instance)
        {
            return new HttpHeadersAssertions(instance);
        }
    }
}

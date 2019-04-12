using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockHttpHeadersDictionary : Dictionary<string, StringValues>, IHeaderDictionary
    {
        public long? ContentLength { get; set; }
    }
}

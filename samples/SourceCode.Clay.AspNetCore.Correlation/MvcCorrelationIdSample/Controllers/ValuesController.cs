using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MvcCorrelationCustomSample.Services;
using SourceCode.Clay.AspNetCore.Middleware.Correlation;

namespace MvcCorrelationCustomSample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ICorrelationContextAccessor _accessor;
        private readonly ScopedService _scoped;
        private readonly TransientService _transient;
        private readonly SingletonService _singleton;

        public ValuesController(ScopedService scoped, TransientService transient, SingletonService singleton, ICorrelationContextAccessor accessor)
        {
            _accessor = accessor;
            _scoped = scoped;
            _transient = transient;
            _singleton = singleton;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // Note that all of these will have the same value.
            return new[]
            {
                $"accessor={_accessor.CorrelationContext.CorrelationId}",
                $"transient={_transient.GetCorrelationId}",
                $"scoped={_scoped.GetCorrelationId}",
                $"singleton={_singleton.GetCorrelationId}",
                $"traceIdentifier={HttpContext.TraceIdentifier}"
            };
        }
    }
}

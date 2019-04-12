
## Introduction

This is a Correlation Id handling middleware for ASP.NET Core based in part on the work of [https://github.com/stevejgordon/CorrelationId](Steve J. Gordon).

## Installation

Install the package into your ASP.NET Core web project.

## Minimum Configuration

Add the middleware in the Startup.Configure method of your application.

```C#
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // ...

            // Enable incoming, generation when missing and outgoing correlation ids.
            app.UseCorrelationId();

            // ..
        }
```

With this configuration the middleware will:
1. Look for a X-Correlation-ID header in the request anduse that
2. Otherwise generate a X-Correlation-ID (using a random Guid string)
2. Update the HttpContext.TraceIdentifier to use the existing or generated X-Correlation-ID
3. Update the response to include the X-Correlation-ID with the original or generated value.


## Basic Configuration

Add the required services in Startup.ConfigureServices of your application.

```C#
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationId();
        }
```

And add the middleware in the Startup.Configure method of your application.

```C#
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCorrelationId();
        }
```

You can configure additional options using the [CorrelationOptions](CorrelationOptions.cs) class.

```C#

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ICorrelationContextAccessor _accessor;

        public ValuesController(ICorrelationContextAccessor accessor)
        {
            _accessor = accessor;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            // Note that both of these will have the same value.
            return new []
            {
                $"accessor={_accessor.CorrelationContext.CorrelationId}",
                $"traceIdentifier={HttpContext.TraceIdentifier}"
            };
        }
    }
```

## Additional Configuration

You can configure additional options using the [ICorrelationContextAccessor](ICorrelationContextAccessor.cs) class.

For example:

```C#
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // ...

            app.UseCorrelationId(options => {
                // Add a custom generator function (default function generates Guids).
                options.CorrelationIdGenerator = (HttpContext context) =>
                {
                    return new StringValues(new string[]
                    {
                        context.Request.Path,
                        DateTime.Now.Ticks.ToString()
                    });
                };
            });


            // ..
        }
```


## TODO:

- Add more tests
- Add support for multiple correlation ids


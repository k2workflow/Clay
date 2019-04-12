using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    public class TestWebApplicationStartup
    {
#pragma warning disable CA1822 // Mark members as static - Called by runtime
#pragma warning disable CA1801 // Remove unused parameter - Part of interface
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCorrelationId();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCorrelationId();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello world");
            });
        }
#pragma warning restore CA1801 // Remove unused parameter
#pragma warning restore CA1822 // Mark members as static
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SourceCode.Clay.AspNetCore.Middleware.Correlation;

namespace MvcCorrelationMinimalSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCorrelationId();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello world!")
                    .ConfigureAwait(false);
            });
        }
    }
}

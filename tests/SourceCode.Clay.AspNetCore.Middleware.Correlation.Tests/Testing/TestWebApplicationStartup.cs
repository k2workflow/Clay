using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    public class TestWebApplicationStartup
    {
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
    }
}

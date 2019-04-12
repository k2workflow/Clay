using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SourceCode.Clay.AspNetCore.Middleware.Correlation;

namespace MvcCorrelationCustomSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Enable the dependency injection of a ICorrelationContextAccessor instance into
            // controllers and services.
            services.AddCorrelationId();

            // Add services that gets the correlation context injected into there constructor.
            services
                .AddScoped<ScopedService>()
                .AddTransient<TransientService>()
                .AddSingleton<SingletonService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable incoming, generation when missing and outgoing correlation ids.
            app.UseCorrelationId();

            app.UseMvc();
        }
    }
}

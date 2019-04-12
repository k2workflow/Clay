using System;
using Microsoft.Extensions.DependencyInjection;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    /// <summary>
    /// Holds <see cref="CorrelationMiddleware"/> related extension methods.
    /// </summary>
    public static class CollelationServicesCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="CorrelationMiddleware"/> to ASP.NET pipeline.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/>.</param>
        /// <returns>The original <paramref name="services"/> for chaining purposes.</returns>
        public static IServiceCollection AddCorrelationId(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();

            return services;
        }
    }
}

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    /// <summary>
    /// Holds <see cref="CorrelationMiddleware"/> related extension methods.
    /// </summary>
    public static class CorrelationApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="CorrelationMiddleware"/> to ASP.NET pipeline.
        /// </summary>
        /// <param name="app">The current <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The original <paramref name="app"/> for chaining purposes.</returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return UseMiddleware(app, new CorrelationOptions());
        }

        /// <summary>
        /// Adds <see cref="CorrelationMiddleware"/> to ASP.NET pipeline.
        /// </summary>
        /// <param name="app">The current <see cref="IApplicationBuilder"/>.</param>
        /// <param name="options">The <see cref="CorrelationOptions"/> that control the middleware's behaviour.</param>
        /// <returns>The original <paramref name="app"/> for chaining purposes.</returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, CorrelationOptions options)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return UseMiddleware(app, options);
        }

        /// <summary>
        /// Adds <see cref="CorrelationMiddleware"/> to ASP.NET pipeline.
        /// </summary>
        /// <param name="app">The current <see cref="IApplicationBuilder"/>.</param>
        /// <param name="configure">An <see cref="System.Action{T}"/> used to configure the <see cref="CorrelationOptions"/>.</param>
        /// <returns>The original <paramref name="app"/> targeted for chaining purposes.</returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, Action<CorrelationOptions> configure)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            var options = new CorrelationOptions();
            configure.Invoke(options);

            return UseMiddleware(app, options);
        }

        private static IApplicationBuilder UseMiddleware(IApplicationBuilder app, CorrelationOptions options)
        {
            return app.UseMiddleware<CorrelationMiddleware>(Options.Create(options));
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Template.Services.Web.Api
{
    /// <summary>
    /// Partial startup class to logger configuration.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Logger service collection configuration.
        /// </summary>
        /// <param name="services">Service collection to be configured.</param>
        private void ConfigureLogger(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
        }

        /// <summary>
        /// Logger application builder configuration.
        /// </summary>
        /// <param name="app">Application builder to be configured.</param>
        /// <param name="loggerFactory">Logger factory to be configured.</param>
        private void ConfigureLogger(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ////app.UseApplicationInsightsRequestTelemetry();
            ////app.UseApplicationInsightsExceptionTelemetry();
        }
    }
}

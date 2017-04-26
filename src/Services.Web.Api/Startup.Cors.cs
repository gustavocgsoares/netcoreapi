using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Services.Web.Api
{
    /// <summary>
    /// Partial startup class to cors policy configuration.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Default name of cors policy.
        /// </summary>
        private const string CorsPolicy = "CorsPolicy";

        /// <summary>
        /// Cors service collection configuration.
        /// </summary>
        /// <param name="services">Service collection to be configured.</param>
        private void ConfigureCors(IServiceCollection services)
        {
            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
            });
        }

        /// <summary>
        /// Cors policy application builder configuration.
        /// </summary>
        /// <param name="app">Application builder to be configured.</param>
        private void ConfigureCors(IApplicationBuilder app)
        {
            // Global policy, if assigned here (it could be defined indvidually for each controller)
            app.UseCors(CorsPolicy);
        }
    }
}

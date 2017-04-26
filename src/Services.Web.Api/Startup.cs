using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Template.Application.Contexts.Corporate;
using Template.Application.Contexts.Support;
using Template.Application.Interfaces.Corporate;
using Template.Application.Interfaces.Support;
using Template.Data.Repositories.Corporate;
using Template.Data.Repositories.Support;
using Template.Data.SqlServer.Helpers;
using Template.Data.SqlServer.Models;

namespace Template.Services.Web.Api
{
    /// <summary>
    /// Partial startup class to main configuration.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Hosting environment.
        /// </summary>
        private IHostingEnvironment hostingEnv;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">Hosting environment to specific configuration, if exists.</param>
        public Startup(IHostingEnvironment env)
        {
            hostingEnv = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration root.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Main service collection configuration.
        /// </summary>
        /// <param name="services">Service collection to be configured.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmployeeApp, EmployeeApp>();
            services.AddTransient<ISuggestionApp, SuggestionApp>();
            services.AddTransient<IUserApp, UserApp>();

            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<ISuggestionRepository, SuggestionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.Configure<CrossCutting.Configurations.Security>(Configuration.GetSection("Security"));

            services.Configure<CrossCutting.Configurations.Data>(Configuration.GetSection("Data"));
            services.AddDbContext<TemplateDbContext>(options => options.UseSqlServer(Configuration.GetValue<string>("Data:SqlServer:ConnectionString")));
            services.AddSingleton<IQueryableUnitOfWork, UnitOfWork>();

            ConfigureLogger(services);
            ConfigureCors(services);
            ConfigureSwagger(services);
            ConfigureApi(services);
        }

        /// <summary>
        /// Main application builder configuration.
        /// </summary>
        /// <param name="app">Application builder to be configured.</param>
        /// <param name="env">Hosting environment to specific configuration, if exists.</param>
        /// <param name="loggerFactory">Logger factory to be configured.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();

            ConfigureLogger(app, loggerFactory);
            ConfigureCors(app);
            ////ConfigureAuth(app);
            ConfigureSwagger(app);
            ConfigureApi(app);
        }
    }
}

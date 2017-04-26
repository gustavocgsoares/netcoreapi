using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Template.Services.Web.Api.Attributes;

namespace Template.Services.Web.Api
{
    /// <summary>
    /// Partial startup class to API configuration.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// API service collection configuration.
        /// </summary>
        /// <param name="services">Service collection to be configured.</param>
        private void ConfigureApi(IServiceCollection services)
        {
            services.AddRouting(opt => opt.LowercaseUrls = true);

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(ExceptionHandlingAttribute));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());

                    options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                });

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(new DateTime(2016, 7, 1));
            });
        }

        /// <summary>
        /// API application builder configuration.
        /// </summary>
        /// <param name="app">Application builder to be configured.</param>
        private void ConfigureApi(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "api/{version}/{controller}/{id?}");
            });
        }
    }
}

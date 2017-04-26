using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Template.Services.Web.Api.Swagger.OperationFilters;

namespace Template.Services.Web.Api
{
    /// <summary>
    /// Partial startup class to docs configuration.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Docs service collection configuration.
        /// </summary>
        /// <param name="services">Service collection to be configured.</param>
        private void ConfigureSwagger(IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", GetSwaggerInfo());

                c.DescribeAllEnumsAsStrings();
                c.DocInclusionPredicate(ResolveDocInclusionPredicate);
                c.CustomSchemaIds(s => s.Name.Replace("Model", string.Empty));
                c.OperationFilter<OmitActionParametersOperationFilter>();
                c.OperationFilter<ExamplesOperationFilter>();

                 // Set the comments path for the swagger json and ui.
                 var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                var xmlPath = Path.Combine(basePath, "Services.Web.Api.xml");
                c.IncludeXmlComments(xmlPath);

                var modelXmlPath = Path.Combine(basePath, "Application.Model.xml");
                c.IncludeXmlComments(modelXmlPath);
            });

            if (hostingEnv.IsDevelopment())
            {
                ////services.ConfigureSwaggerGen(c =>
                ////{
                ////    c.IncludeXmlComments(GetXmlCommentsPath(PlatformServices.Default.Application));
                ////});
            }
        }

        /// <summary>
        /// Docs application builder configuration.
        /// </summary>
        /// <param name="app">Application builder to be configured.</param>
        private void ConfigureSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api-docs/{documentName}/apis.json";
                options.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";
                options.SwaggerEndpoint("/api-docs/v1/apis.json", "Template APIs - v1");
                options.EnabledValidator();
                options.BooleanValues(new object[] { 0, 1 });
                options.DocExpansion("none");
                options.SupportedSubmitMethods(new[] { "get", "post", "put", "delete" });
                options.ShowRequestHeaders();
                options.ShowJsonEditor();
                ////options.InjectStylesheet("/swagger-ui/custom.css");
                // Provide client ID, client ID, realm and application name
                ////options.ConfigureOAuth2("swagger-ui", "swagger-ui-secret", "swagger-ui-realm", "Swagger UI");
            });
        }

        /// <summary>
        /// Get swagger info.
        /// </summary>
        /// <returns>Swagger info.</returns>
        private Info GetSwaggerInfo()
        {
            return new Info
            {
                Title = "Template APIs - v1",
                Version = "v1",
                Description = "Our APIs are yours",
                TermsOfService = "Knock yourself out",
                Contact = new Contact
                {
                    Name = "4 Think Team",
                    Email = "we.are.developer@domain.com.br"
                },
                License = new License
                {
                    Name = "Use under LICX",
                    Url = "http://url.com"
                }
            };
        }

        /// <summary>
        /// Resolving api version constraint.
        /// </summary>
        /// <param name="docName">docName parameter.</param>
        /// <param name="apiDesc">Api description.</param>
        /// <returns>Return if version exists.</returns>
        private bool ResolveDocInclusionPredicate(string docName, ApiDescription apiDesc)
        {
            var values = apiDesc.RelativePath
                        .Split('/')
                        .Select(v => v.Replace("v{version}", docName));

            apiDesc.RelativePath = string.Join("/", values);

            var versionParameter = apiDesc.ParameterDescriptions
                .SingleOrDefault(p => p.Name == "version");

            if (versionParameter != null)
            {
                apiDesc.ParameterDescriptions.Remove(versionParameter);
            }

            var versions = apiDesc.ControllerAttributes()
                .OfType<ApiVersionAttribute>()
                .SelectMany(attr => attr.Versions);

            return versions.Any(v =>
            {
                return $"v{v.ToString()}" == docName;
            });
        }

        /// <summary>
        /// Authorization filter.
        /// </summary>
        private class AuthResponsesOperationFilter : IOperationFilter
        {
            /// <summary>
            /// Configure possible response for API with authorize attribute.
            /// </summary>
            /// <param name="operation">Operation to be configured.</param>
            /// <param name="context">Context configuration.</param>
            public void Apply(Operation operation, OperationFilterContext context)
            {
                var authAttributes = context.ApiDescription
                    .ControllerAttributes()
                    .Union(context.ApiDescription.ActionAttributes())
                    .OfType<AuthorizeAttribute>();

                if (authAttributes.Any())
                {
                    operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                }
            }
        }
    }
}

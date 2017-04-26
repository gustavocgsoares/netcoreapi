using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Template.Services.Web.Api.Swagger.Attibutes;

namespace Template.Services.Web.Api.Swagger.OperationFilters
{
    /// <summary>
    /// Examples operation filter.
    /// </summary>
    public class ExamplesOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Aplly examples.
        /// </summary>
        /// <param name="operation">See <see cref="Operation"/>.</param>
        /// <param name="context">See <see cref="OperationFilterContext"/>.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            PrepareSwaggerRequest(operation, context);
            PrepareSwaggerResponse(operation, context);
        }

        /// <summary>
        /// Prepare swagger request.
        /// </summary>
        /// <param name="operation">See <see cref="Operation"/>.</param>
        /// <param name="context">See <see cref="OperationFilterContext"/>.</param>
        private static void PrepareSwaggerRequest(Operation operation, OperationFilterContext context)
        {
            var responseAttributes = context.ApiDescription
                .ActionAttributes()
                .OfType<SwaggerRequestExamplesAttribute>();

            var actionDescriptor = (ControllerActionDescriptor)context.ApiDescription.ActionDescriptor;
            var action = actionDescriptor.ControllerTypeInfo.GetMethod(actionDescriptor.ActionName);
            var parameters = action.GetParameters();

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var attr = parameter.GetCustomAttribute<SwaggerRequestExamplesAttribute>();

                if (attr != null)
                {
                    var request = operation.Parameters.FirstOrDefault(p => p.Name == parameter.Name);
                    var list = Enumerable.ToList(operation.Parameters);
                    var requestWithExample = ParameterWithExample.Convert(request);

                    if (attr.Instance != null)
                    {
                        requestWithExample.Xexamples = FormatAsJson(attr.Instance);
                    }
                    else if (!string.IsNullOrEmpty(attr.Example))
                    {
                        requestWithExample.Xexamples = new Dictionary<string, string>()
                            {
                                {
                                    "application/json", attr.Example
                                }
                            };
                    }

                    operation.Parameters[i] = requestWithExample;
                }
            }
        }

        /// <summary>
        /// Prepare swagger response.
        /// </summary>
        /// <param name="operation">See <see cref="Operation"/>.</param>
        /// <param name="context">See <see cref="OperationFilterContext"/>.</param>
        private static void PrepareSwaggerResponse(Operation operation, OperationFilterContext context)
        {
            var responseAttributes = context.ApiDescription
                .ActionAttributes()
                .OfType<SwaggerResponseExamplesAttribute>();

            foreach (var attr in responseAttributes)
            {
                var schema = context.SchemaRegistry.GetOrRegister(attr.ResponseType);

                var response = operation.Responses
                    .FirstOrDefault(x => x.Value.Schema.Type == schema.Type && x.Value.Schema.Ref == schema.Ref).Value;

                if (response != null && attr.MockNotFound == false)
                {
                    response.Examples = FormatAsJson(attr.Instance);
                }
            }
        }

        /// <summary>
        /// Format as json.
        /// </summary>
        /// <param name="obj">Object to be formated.</param>
        /// <returns>Object formated.</returns>
        private static object FormatAsJson(object obj)
        {
            var examples = new Dictionary<string, object>()
                {
                    {
                        "application/json", obj
                    }
                };

            return ConvertToCamelCase(examples);
        }

        /// <summary>
        /// Convert examples to camel case.
        /// </summary>
        /// <param name="examples">Examples to be converted.</param>
        /// <returns>Examples converted.</returns>
        private static object ConvertToCamelCase(Dictionary<string, object> examples)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var jsonString = JsonConvert.SerializeObject(examples, settings);

            return JsonConvert.DeserializeObject(jsonString);
        }

        /// <summary>
        /// Parameter with example.
        /// </summary>
        private class ParameterWithExample : IParameter
        {
            public string Description { get; set; }

            public Dictionary<string, object> Extensions { get; }

            public string In { get; set; }

            public string Name { get; set; }

            public bool Required { get; set; }

            [JsonProperty("x-examples")]
            public object Xexamples { get; set; }

            public static ParameterWithExample Convert(IParameter parameter)
            {
                var parameterWithExample = new ParameterWithExample();
                var fields = parameter.GetType().GetFields();

                foreach (var field in fields)
                {
                    parameterWithExample
                        .GetType()
                        .GetField(field.Name)
                        .SetValue(parameterWithExample, field.GetValue(parameter));
                }

                return parameterWithExample;
            }
        }
    }
}

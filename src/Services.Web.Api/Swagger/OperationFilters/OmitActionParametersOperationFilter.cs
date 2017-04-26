using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Template.Services.Web.Api.Swagger.Attibutes;

namespace Template.Services.Web.Api.Swagger.OperationFilters
{
    /// <summary>
    /// Omit action parameters.
    /// </summary>
    public class OmitActionParametersOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply filter.
        /// </summary>
        /// <param name="operation">See <see cref="Operation"/>.</param>
        /// <param name="context">See <see cref="OperationFilterContext"/>.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ApiDescription.ActionDescriptor;

            var action = actionDescriptor.ControllerTypeInfo.GetMethod(actionDescriptor.ActionName);
            var parameters = action.GetParameters();

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var attr = parameter.GetCustomAttribute<SwaggerIgnoreParameterAttribute>();

                if (attr != null)
                {
                    var list = Enumerable.ToList(operation.Parameters);
                    list.RemoveAll(p => p.Name == parameter.Name);
                    operation.Parameters = list;
                }
            }
        }
    }
}

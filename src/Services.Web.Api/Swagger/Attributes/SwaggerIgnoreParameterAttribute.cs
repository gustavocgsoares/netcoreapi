using System;

namespace Template.Services.Web.Api.Swagger.Attibutes
{
    /// <summary>
    /// Ignore parameter attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SwaggerIgnoreParameterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerIgnoreParameterAttribute"/> class.
        /// </summary>
        public SwaggerIgnoreParameterAttribute()
        {
        }
    }
}
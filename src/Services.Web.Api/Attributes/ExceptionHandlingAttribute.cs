using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Template.Application.Model.Contexts.Base;
using Template.CrossCutting.Configurations;
using Template.CrossCutting.Exceptions.Base;
using Template.CrossCutting.ExtensionMethods;

namespace Template.Services.Web.Api.Attributes
{
    /// <summary>
    /// Exception handling.
    /// </summary>
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        #region Fields | Members

        /// <summary>
        /// Security configuration.
        /// </summary>
        private readonly IOptions<Security> config;

        /// <summary>
        /// Hosting environment.
        /// </summary>
        private readonly IHostingEnvironment hostingEnvironment;

        /// <summary>
        /// Model metadata provider.
        /// </summary>
        private readonly IModelMetadataProvider modelMetadataProvider;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingAttribute"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">See <see cref="IHostingEnvironment"/>.</param>
        /// <param name="modelMetadataProvider">See <see cref="IModelMetadataProvider"/>.</param>
        /// <param name="config">See <see cref="IOptions{TOptions}"/> of type <see cref="Security"/>.</param>
        public ExceptionHandlingAttribute(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider,
            IOptions<Security> config)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.modelMetadataProvider = modelMetadataProvider;
            this.config = config;
        }
        #endregion

        #region ExceptionFilterAttribute overrides

        /// <summary>
        /// On exception overrides.
        /// </summary>
        /// <param name="context">See <see cref="ExceptionContext"/>.</param>
        public override void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;

            if (exception is InvalidParameterException)
            {
                context.Result = GetInvalidParameterExceptionResult(exception);
            }
            else if (exception is BusinessRuleException)
            {
                context.Result = GetBusinessRuleExceptionResult(exception);
            }
            else if (exception is BusinessConflictException)
            {
                context.Result = new ConflictResult();
            }
            else if (exception is DataNotFoundException)
            {
                context.Result = new NotFoundResult();
            }
            else if (exception is NotImplementedException)
            {
                context.Result = new StatusCodeResult(HttpStatusCode.NotImplemented.To<int>());
            }
            else
            {
                context.Result = GetUnpredictableExceptionResult(context);
            }
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Get <see cref="InvalidParameterException"/> result.
        /// </summary>
        /// <param name="exception">Context exception.</param>
        /// <returns><see cref="InvalidParameterException"/> result.</returns>
        private IActionResult GetInvalidParameterExceptionResult(Exception exception)
        {
            var ex = (InvalidParameterException)exception;

            var obj = new BadRequestModel
            {
                Code = ex.Code,
                Message = ex.Message,
                Validations = ex.Validations
            };

            return new BadRequestObjectResult(obj);
        }

        /// <summary>
        /// Get <see cref="BusinessRuleException"/> result.
        /// </summary>
        /// <param name="exception">Context exception.</param>
        /// <returns><see cref="BusinessRuleException"/> result.</returns>
        private IActionResult GetBusinessRuleExceptionResult(Exception exception)
        {
            var ex = (BusinessRuleException)exception;

            var obj = new BadRequestModel
            {
                Code = ex.Code,
                Message = ex.Message
            };

            return new BadRequestObjectResult(obj);
        }

        /// <summary>
        /// Get <see cref="UnpredictableException"/> result.
        /// </summary>
        /// <param name="context">Context exception.</param>
        /// <returns><see cref="UnpredictableException"/> result.</returns>
        private IActionResult GetUnpredictableExceptionResult(ExceptionContext context)
        {
            Exception exception = context.Exception;

            if (hostingEnvironment.IsDevelopment())
            {
                return new ExceptionResult(exception, true);
            }

            var detailedError = context.HttpContext.Request.Headers
                .FirstOrDefault(h => h.Key.Equals("X-Detailed-Error", StringComparison.CurrentCultureIgnoreCase))
                .Value.FirstOrDefault();

            var valueToCheck = DateTime.Now.ToString("yyyyMMdd").Encrypt(config.Value.CryptoKey);
            var showStack = detailedError.HasValue()
                         && detailedError.Equals(valueToCheck, StringComparison.CurrentCultureIgnoreCase);

            var ex = new UnpredictableException(exception);
            return new ExceptionResult(ex, showStack);
        }
        #endregion
    }
}

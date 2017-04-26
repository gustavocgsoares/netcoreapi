using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace Template.Services.Web.Api.Controllers
{
    /// <summary>
    /// Base api controller.
    /// </summary>
    ////[Authorize]
    [Controller]
    public abstract class BaseApiController : ApiController
    {
        #region Fields | Members

        /// <summary>
        /// API date format (YYYY-MM-DD).
        /// </summary>
        public const string ApiDateFormat = ":datetime:regex(\\d{4}-\\d{2}-\\d{2})";

        /// <summary>
        /// API date time format (YYYY-MM-DDTHH-mm | YYYY-MM-DDTHH-mm-SS).
        /// </summary>
        public const string ApiDateTimeFormat = ":datetime:regex(\\d{4}-\\d{2}-\\d{2}T(\\d{2}:\\d{2}:\\d{2}|\\d{2}:\\d{2})";
        #endregion

        #region ApiController Overrides
        ////public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        ////{
        ////    controllerContext.Request.SetCultureInfo();
        ////    ////TransacaoSingleton.Instance.ParceiroIp = controllerContext.Request.GetClientIpAddress();

        ////    return base.ExecuteAsync(controllerContext, cancellationToken);
        ////}
        #endregion
    }
}

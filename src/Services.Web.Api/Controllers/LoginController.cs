using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Template.Application.Model.Contexts.Base;
using Template.Application.Model.Contexts.V1.Security;
using Template.Application.Model.Enums.V1.Corporate;
using Template.CrossCutting.Exceptions.Base;
using Template.CrossCutting.ExtensionMethods;
using Template.CrossCutting.Resources.Validations;

namespace Template.Services.Web.Api.Controllers
{
    /// <summary>
    /// Login APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/login")]
    public class LoginController : BaseApiController
    {
        #region Fields | Members

        /// <summary>
        /// See <see cref="IUrlHelperFactory"/>.
        /// </summary>
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="urlHelperFactory">See <see cref="IUrlHelperFactory"/>.</param>
        public LoginController(
            IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
        }
        #endregion

        #region Services

        /// <summary>
        /// Login to an existing user test.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="login">User login like email, phone number.</param>
        /// <param name="password">User password.</param>
        /// <response code="200">Authenticated user. \o/</response>
        /// <response code="400">Invalid parameter values.</response>
        /// <response code="403">Blocked login.</response>
        /// <response code="404">Login not found or not exists.</response>
        /// <response code="500">Oops! Can't get your authentication right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{login}/{password}", Name = "Login")]
        [ProducesResponseType(typeof(LoginModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 403)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Login(string login, string password)
        {
            if (login == "blocked@domain.com")
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            else if (login == "notfound@domain.com")
            {
                return NotFound();
            }

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);

            var result = new LoginModel
            {
                Links = new List<Link>
                {
                    UsersController.GetUserByIdLink(urlHelper, "rwer3453erw")
                }
            };

            await Task.Run(() => result);

            return Ok(result);
        }

        /// <summary>
        /// Login by social network like facebook, google plus, among others.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="socialNetwork">Social network.</param>
        /// <param name="externalCode">External code received by social network.</param>
        /// <response code="200">Authenticated user. \o/</response>
        /// <response code="400">Invalid parameter values.</response>
        /// <response code="403">Blocked login.</response>
        /// <response code="404">Login not found or not exists.</response>
        /// <response code="500">Oops! Can't get your authentication right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{socialNetwork}/{externalCode}/social_network", Name = "LoginWithSocialNetwork")]
        [ProducesResponseType(typeof(LoginModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 403)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> LoginWithSocialNetwork(SocialNetwork socialNetwork, string externalCode)
        {
            if (externalCode == "blocked")
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            else if (externalCode == "notfound")
            {
                return NotFound();
            }

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);

            var result = new LoginModel
            {
                Links = new List<Link>
                {
                    UsersController.GetUserByIdLink(urlHelper, "rwer3453erw")
                }
            };

            await Task.Run(() => result);
            return Ok(result);
        }

        /// <summary>
        /// Recover password.
        /// </summary>
        /// <param name="login">User login like email, phone number.</param>
        /// <response code="204">Email sent for password recovery. \o/</response>
        /// <response code="400">Invalid parameter values.</response>
        /// <response code="403">Blocked login.</response>
        /// <response code="404">Login not found or not exists.</response>
        /// <response code="500">Oops! Can't get your password recovery right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{login}", Name = "RecoverPassword")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 403)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> RecoverPassword(string login)
        {
            if (login == "blocked@domain.com")
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            else if (login == "notfound@domain.com")
            {
                return NotFound();
            }

            await Task.Run(() => login);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Change password by user logged in.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="pass">Current password.</param>
        /// <param name="newPass">New password.</param>
        /// <param name="newPassConfirm">New password confirmation. Must be the same value as the parameter <paramref name="newPass"/>.</param>
        /// <response code="204">Password changed. \o/</response>
        /// <response code="400">Invalid parameter values.</response>
        /// <response code="404">User not found or not exists.</response>
        /// <response code="500">Oops! Can't get your change password right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPut]
        [Route("{userId}/{pass}/{newPass}/{newPassConfirm}", Name = "ChangePassword")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> ChangePassword(
            string userId,
            string pass,
            string newPass,
            string newPassConfirm)
        {
            if (newPass != newPassConfirm)
            {
                return BadRequest();
            }
            else if (userId == "notfound")
            {
                return NotFound();
            }

            await Task.Run(() => userId);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Change password by email received from RecoverPassword API.
        /// </summary>
        /// <param name="newPass">New password.</param>
        /// <param name="newPassConfirm">New password confirmation. Must be the same value as the parameter <paramref name="newPass"/>.</param>
        /// <param name="token">Token in the password recovery link.</param>
        /// <response code="204">Password changed. \o/</response>
        /// <response code="400">Invalid parameter values.</response>
        /// <response code="403">Blocked login.</response>
        /// <response code="404">Token not found, not exists or disabled.</response>
        /// <response code="500">Oops! Can't get your change password right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPut]
        [Route("{newPass}/{newPassConfirm}/{token}", Name = "ChangePasswordByToken")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 403)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> ChangePasswordByToken(
            string newPass,
            string newPassConfirm,
            string token)
        {
            newPass.IsNullOrEmpty().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNullOrEmpty, "newPass"));

            if (newPass != newPassConfirm)
            {
                return BadRequest();
            }
            else if (token == "notfound")
            {
                return NotFound();
            }

            await Task.Run(() => token);

            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion
    }
}

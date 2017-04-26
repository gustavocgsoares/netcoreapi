using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Template.Application.Interfaces.Corporate;
using Template.Application.Model.Contexts.Base;
using Template.Application.Model.Contexts.V1.Corporate;
using Template.Application.Model.Enums.Base;
using Template.CrossCutting.Exceptions.Base;
using Template.CrossCutting.ExtensionMethods;
using Template.CrossCutting.Resources.Validations;

namespace Template.Services.Web.Api.Controllers
{
    /// <summary>
    /// Users APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class UsersController : BaseApiController
    {
        #region Fields | Members

        /// <summary>
        /// User application flow.
        /// </summary>
        private readonly IUserApp userApp;

        /// <summary>
        /// See <see cref="IUrlHelperFactory"/>.
        /// </summary>
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userApp">User application flow.</param>
        /// <param name="urlHelperFactory">See <see cref="IUrlHelperFactory"/>.</param>
        public UsersController(
            IUserApp userApp,
            IUrlHelperFactory urlHelperFactory)
        {
            this.userApp = userApp;
            this.urlHelperFactory = urlHelperFactory;
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Get link to GetUserById API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link GetUserByIdLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var userLink = urlHelper
                .Link("GetUserById", new { id = id });

            return new Link
            {
                Href = userLink,
                Method = Method.GET,
                Relation = self ? "self" : "user_by_id"
            };
        }

        /// <summary>
        /// Get link to UpdateUser API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link UpdateUserLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var userLink = urlHelper
                .Link("UpdateUser", new { id = id });

            return new Link
            {
                Href = userLink,
                Method = Method.PUT,
                Relation = self ? "self" : "update_user"
            };
        }

        /// <summary>
        /// Get link to DeleteUser API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <returns>API link.</returns>
        public static Link DeleteUserLink(
            IUrlHelper urlHelper,
            string id)
        {
            var userLink = urlHelper
                .Link("DeleteUser", new { id = id });

            return new Link
            {
                Href = userLink,
                Method = Method.DELETE,
                Relation = "delete_user"
            };
        }
        #endregion

        #region Services

        /// <summary>
        /// Get all users in a paged list.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="offset">Where to start returning records from the entire set of results. If you don't include this parameter, the default is to start at record number 0.</param>
        /// <param name="limit">How many records you want to return all at once. If you don't include this parameter, the limit is 100 records by default.</param>
        /// <response code="200">Users found. \o/</response>
        /// <response code="400">Invalid values.</response>
        /// <response code="500">Oops! Can't get your users right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("", Name = "GetAllUsers")]
        [ProducesResponseType(typeof(UsersModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetAll(int offset = 0, int limit = 100)
        {
            long totalCount = 0;
            var totalPages = 0;
            var routeName = "GetAllUsers";

            var pagedList = await userApp.GetAllAsync(offset, limit);

            if (pagedList.IsNotNull() && pagedList.Items.IsNotNull() && pagedList.Items.Count > 0)
            {
                totalCount = pagedList.Total;
                totalPages = (int)Math.Ceiling((double)totalCount / limit);
            }

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);

            var prevLink = offset > 0 ? urlHelper.Link(routeName, new { offset = limit > offset ? 0 : offset - limit, limit = limit }) : string.Empty;
            var nextLink = offset < totalCount - limit ? urlHelper.Link(routeName, new { offset = offset + limit, limit = limit }) : string.Empty;

            var firstLink = offset > 0 ? urlHelper.Link(routeName, new { offset = 0, limit = limit }) : string.Empty;
            var lastLink = offset < totalCount - limit ? urlHelper.Link(routeName, new { offset = totalCount - limit, limit = limit }) : string.Empty;

            var links = new List<Link>();

            if (prevLink.HasValue())
            {
                links.Add(new Link { Href = prevLink, Method = Method.GET, Relation = "previous" });
            }

            if (nextLink.HasValue())
            {
                links.Add(new Link { Href = nextLink, Method = Method.GET, Relation = "next" });
            }

            if (firstLink.HasValue())
            {
                links.Add(new Link { Href = firstLink, Method = Method.GET, Relation = "first" });
            }

            if (lastLink.HasValue())
            {
                links.Add(new Link { Href = lastLink, Method = Method.GET, Relation = "last" });
            }

            var result = new UsersModel
            {
                TotalCount = pagedList.Total,
                TotalPages = totalPages,
                Links = links,
                Items = pagedList.Items.Select(e => UserModel.ToModel(e)).ToArray()
            };

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific user by unique id.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="id">User id.</param>
        /// <response code="200">User found. \o/</response>
        /// <response code="400">User has missing/invalid values.</response>
        /// <response code="404">User not found or not exists.</response>
        /// <response code="500">Oops! Can't get your user right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{id}", Name = "GetUserById")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetById(string id)
        {
            var entity = await userApp.GetAsync(id);

            if (entity.IsNull())
            {
                return NotFound();
            }

            var model = UserModel.ToModel(entity);

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetUserByIdLink(urlHelper, model.Id, true);
            var deleteLink = DeleteUserLink(urlHelper, model.Id);
            var updateLink = UpdateUserLink(urlHelper, model.Id);

            model.Links = new List<Link>
                {
                    getByIdlink,
                    updateLink,
                    deleteLink
                };

            return Ok(model);
        }

        /// <summary>
        /// Create an user.
        /// </summary>
        /// <param name="user">User to be created.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="201">User created. \o/</response>
        /// <response code="400">User has missing/invalid values.</response>
        /// <response code="409">User has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't create your user right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPost]
        [Route("", Name = "CreateUser")]
        [ProducesResponseType(typeof(UserModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Create([FromBody]UserModel user)
        {
            user.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "user"));

            var entity = await userApp.SaveAsync(user.ToDomain());
            var result = UserModel.ToModel(entity);

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetUserByIdLink(urlHelper, result.Id);
            var deleteLink = DeleteUserLink(urlHelper, result.Id);
            var updateLink = UpdateUserLink(urlHelper, result.Id);

            result.Links = new List<Link>
                {
                    getByIdlink,
                    updateLink,
                    deleteLink
                };

            return new CreatedResult(getByIdlink.Href, result);
        }

        /// <summary>
        /// Update an user.
        /// </summary>
        /// <param name="id">User id to be updated.</param>
        /// <param name="user">User to be updated.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">User updated. \o/</response>
        /// <response code="400">User has missing/invalid values.</response>
        /// <response code="404">User not found.</response>
        /// <response code="409">User has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't update your user right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPut]
        [Route("{id}", Name = "UpdateUser")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Update(string id, [FromBody]UserModel user)
        {
            user.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "User"));

            user.Id = id;

            var entity = await userApp.SaveAsync(user.ToDomain());
            var result = UserModel.ToModel(entity);

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetUserByIdLink(urlHelper, result.Id);
            var updateLink = UpdateUserLink(urlHelper, result.Id, true);
            var deleteLink = DeleteUserLink(urlHelper, result.Id);

            result.Links = new List<Link>
                {
                    getByIdlink,
                    updateLink,
                    deleteLink
                };

            return Ok(result);
        }

        /// <summary>
        /// Delete an user.
        /// </summary>
        /// <param name="id">User id to be deleted.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="204">User deleted. o.O</response>
        /// <response code="400">User has missing/invalid values.</response>
        /// <response code="500">Oops! Can't create your user right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteUser")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Delete(string id)
        {
            await userApp.DeleteAsync(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion
    }
}

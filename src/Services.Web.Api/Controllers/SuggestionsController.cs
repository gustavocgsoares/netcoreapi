using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Template.Application.Interfaces.Support;
using Template.Application.Model.Contexts.Base;
using Template.Application.Model.Contexts.V1.Support;
using Template.Application.Model.Enums.Base;
using Template.CrossCutting.Exceptions;
using Template.CrossCutting.Exceptions.Base;
using Template.CrossCutting.ExtensionMethods;
using Template.CrossCutting.Resources.Validations;
using Template.Services.Web.Api.Swagger.Attibutes;

namespace Template.Services.Web.Api.Controllers
{
    /// <summary>
    /// Suggestions APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class SuggestionsController : BaseApiController
    {
        #region Fields | Members

        /// <summary>
        /// Suggestion application flow.
        /// </summary>
        private readonly ISuggestionApp suggestionApp;

        /// <summary>
        /// See <see cref="IUrlHelperFactory"/>.
        /// </summary>
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestionsController"/> class.
        /// </summary>
        /// <param name="suggestionApp">Suggestion application flow.</param>
        /// <param name="urlHelperFactory">See <see cref="IUrlHelperFactory"/>.</param>
        public SuggestionsController(
            ISuggestionApp suggestionApp,
            IUrlHelperFactory urlHelperFactory)
        {
            this.suggestionApp = suggestionApp;
            this.urlHelperFactory = urlHelperFactory;
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Get link to GetSuggestionById API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link GetSuggestionByIdLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var suggestionLink = urlHelper
                .Link("GetSuggestionById", new { id = id });

            return new Link
            {
                Href = suggestionLink,
                Method = Method.GET,
                Relation = self ? "self" : "suggestion_by_id"
            };
        }

        /// <summary>
        /// Get link to UpdateSuggestion API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">Suggestion id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link UpdateSuggestionLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var suggestionLink = urlHelper
                .Link("UpdateSuggestion", new { id = id });

            return new Link
            {
                Href = suggestionLink,
                Method = Method.PUT,
                Relation = self ? "self" : "update_suggestion"
            };
        }

        /// <summary>
        /// Get link to DeleteSuggestion API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">Suggestion id.</param>
        /// <returns>API link.</returns>
        public static Link DeleteSuggestionLink(
            IUrlHelper urlHelper,
            string id)
        {
            var suggestionLink = urlHelper
                .Link("DeleteSuggestion", new { id = id });

            return new Link
            {
                Href = suggestionLink,
                Method = Method.DELETE,
                Relation = "delete_suggestion"
            };
        }
        #endregion

        #region Services

        /// <summary>
        /// Get all suggestions in a paged list.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="offset">Where to start returning records from the entire set of results. If you don't include this parameter, the default is to start at record number 0.</param>
        /// <param name="limit">How many records you want to return all at once. If you don't include this parameter, the limit is 100 records by default.</param>
        /// <response code="200">Suggestions found. \o/</response>
        /// <response code="400">Invalid values.</response>
        /// <response code="500">Oops! Can't get your suggestions right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("", Name = "GetAllSuggestions")]
        [ProducesResponseType(typeof(SuggestionsModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetAll(int offset = 0, int limit = 100)
        {
            long totalCount = 0;
            var totalPages = 0;
            var routeName = "GetAllSuggestions";

            var pagedList = await suggestionApp.GetAllAsync(offset, limit);

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

            var result = new SuggestionsModel
            {
                TotalCount = pagedList.Total,
                TotalPages = totalPages,
                Links = links,
                Items = pagedList.Items.Select(e => SuggestionModel.ToModel(e)).ToArray()
            };

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific suggestion by unique id.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="id">Suggestion id.</param>
        /// <response code="200">Suggestion found. \o/</response>
        /// <response code="400">Suggestion has missing/invalid values.</response>
        /// <response code="404">Suggestion not found or not exists.</response>
        /// <response code="500">Oops! Can't get your suggestion right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{id}", Name = "GetSuggestionById")]
        [ProducesResponseType(typeof(SuggestionModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        [SwaggerResponseExamples(typeof(SuggestionModel), "Examples/Response/Suggestion.json")]
        [SwaggerResponseExamples(typeof(BadRequestModel), "Examples/Response/BadRequest.json")]
        public async Task<IActionResult> GetById(string id)
        {
            var entity = await suggestionApp.GetAsync(id);

            if (entity.IsNull())
            {
                return NotFound();
            }

            var model = SuggestionModel.ToModel(entity);

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetSuggestionByIdLink(urlHelper, model.Id, true);
            var deleteLink = DeleteSuggestionLink(urlHelper, model.Id);
            var updateLink = UpdateSuggestionLink(urlHelper, model.Id);

            model.Links = new List<Link>
                {
                    getByIdlink,
                    updateLink,
                    deleteLink
                };

            return Ok(model);
        }

        /// <summary>
        /// Create a suggestion.
        /// </summary>
        /// <param name="suggestion">Suggestion to be created.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="201">Suggestion created. \o/</response>
        /// <response code="400">Suggestion has missing/invalid values.</response>
        /// <response code="409">Suggestion has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't create your suggestion right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPost]
        [Route("", Name = "CreateSuggestion")]
        [ProducesResponseType(typeof(SuggestionModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Create([FromBody]SuggestionModel suggestion)
        {
            suggestion.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "suggestion"));

            var entity = await suggestionApp.SaveAsync(suggestion.ToDomain());
            var result = SuggestionModel.ToModel(entity);

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetSuggestionByIdLink(urlHelper, result.Id);
            var deleteLink = DeleteSuggestionLink(urlHelper, result.Id);
            var updateLink = UpdateSuggestionLink(urlHelper, result.Id);

            result.Links = new List<Link>
                {
                    getByIdlink,
                    updateLink,
                    deleteLink
                };

            return new CreatedResult(getByIdlink.Href, result);
        }

        /// <summary>
        /// Update a suggestion.
        /// </summary>
        /// <param name="id">Suggestion id to be updated.</param>
        /// <param name="suggestion">Suggestion to be updated.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Suggestion updated. \o/</response>
        /// <response code="400">Suggestion has missing/invalid values.</response>
        /// <response code="404">Suggestion not found.</response>
        /// <response code="409">Suggestion has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't update your suggestion right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPut]
        [Route("{id}", Name = "UpdateSuggestion")]
        [ProducesResponseType(typeof(SuggestionModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Update(string id, [FromBody]SuggestionModel suggestion)
        {
            suggestion.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "Suggestion"));

            suggestion.Id = id;

            var entity = await suggestionApp.SaveAsync(suggestion.ToDomain());
            var result = SuggestionModel.ToModel(entity);

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetSuggestionByIdLink(urlHelper, result.Id);
            var updateLink = UpdateSuggestionLink(urlHelper, result.Id, true);
            var deleteLink = DeleteSuggestionLink(urlHelper, result.Id);

            result.Links = new List<Link>
                {
                    getByIdlink,
                    updateLink,
                    deleteLink
                };

            return Ok(result);
        }

        /// <summary>
        /// Delete a suggestion.
        /// </summary>
        /// <param name="id">Suggestion id to be deleted.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="204">Suggestion deleted. o.O</response>
        /// <response code="400">Suggestion has missing/invalid values.</response>
        /// <response code="500">Oops! Can't create your suggestion right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteSuggestion")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Delete(string id)
        {
            await suggestionApp.DeleteAsync(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion
    }
}

using System.Net.Http;
using System.Threading.Tasks;
using Template.Application.Model.Contexts.V1.Corporate;
using Template.Services.Web.Api.Tests.Base;
using Template.Services.Web.Api.Tests.Controllers.Base;

namespace Template.Services.Web.Api.Tests.Controllers.UsersController.DeleteApi
{
    public class Configuration : BaseConfiguration
    {
        private ClassFixture fixture;

        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
            this.fixture = fixture;
        }

        public async Task<HttpResponseMessage> WhenRequestingTheDeleteUserApiAsync(string userId = "")
        {
            return await RequestingApiAsync(new HttpMethod("DELETE"), $"/v1/users/{userId}");
        }

        #region Givens
        public async Task<string> GivenAValidUserId()
        {
            var createApiConfig = new CreateApi.Configuration(fixture);
            var user = createApiConfig.GivenAValidUserModel();
            user.Email = "deleteapi@domain.com";

            var response = await createApiConfig.WhenRequestingTheCreateUserApiAsync(user);
            var result = await createApiConfig.WhenGetContentAsModelAsync<UserModel>(response);

            return result.Id;
        }

        public string GivenANotExistsUserId()
        {
            return "notfound";
        }

        public string GivenAnEmptyUserId()
        {
            return string.Empty;
        }
        #endregion
    }
}

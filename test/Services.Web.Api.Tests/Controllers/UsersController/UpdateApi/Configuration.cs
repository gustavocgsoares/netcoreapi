using System;
using System.Net.Http;
using System.Threading.Tasks;
using Template.Application.Model.Contexts.V1.Corporate;
using Template.Application.Model.Enums.V1.Corporate;
using Template.Services.Web.Api.Tests.Base;
using Template.Services.Web.Api.Tests.Controllers.Base;

namespace Template.Services.Web.Api.Tests.Controllers.UsersController.UpdateApi
{
    public class Configuration : BaseConfiguration
    {
        private ClassFixture fixture;

        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
            this.fixture = fixture;
        }

        public async Task<HttpResponseMessage> WhenRequestingTheUpdateUserApiAsync(
            string userId = "",
            UserModel user = null)
        {
            return await RequestingApiAsync(new HttpMethod("PUT"), $"/v1/users/{userId}", user);
        }

        #region Givens
        public async Task<string> GivenAValidUserId()
        {
            var createApiConfig = new CreateApi.Configuration(fixture);
            var user = createApiConfig.GivenAValidUserModel();
            user.Email = "updateapi@domain.com";

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

        public UserModel GivenAValidUserModel()
        {
            return new UserModel
            {
                FirstName = "Jane",
                LastName = "Smith Doe",
                Email = "jsd@domain.com",
                BirthDate = new DateTime(1988, 07, 03),
                Gender = Gender.Female,
                ProfileImage = "../img/profiles/234rewr23422.png"
            };
        }

        public UserModel GivenAnEmptyUserModel()
        {
            return new UserModel();
        }

        public UserModel GivenANullUserModel()
        {
            return null;
        }
        #endregion
    }
}

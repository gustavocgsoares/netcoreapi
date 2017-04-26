using System;
using System.Net.Http;
using System.Threading.Tasks;
using Template.Application.Model.Contexts.V1.Corporate;
using Template.Application.Model.Enums.V1.Corporate;
using Template.Services.Web.Api.Tests.Base;
using Template.Services.Web.Api.Tests.Controllers.Base;

namespace Template.Services.Web.Api.Tests.Controllers.UsersController.CreateApi
{
    public class Configuration : BaseConfiguration
    {
        private ClassFixture fixture;

        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
            this.fixture = fixture;
        }

        public async Task<HttpResponseMessage> WhenRequestingTheCreateUserApiAsync(UserModel user = null)
        {
            return await RequestingApiAsync(new HttpMethod("POST"), $"/v1/users", user);
        }

        #region Givens
        public UserModel GivenAValidUserModel()
        {
            return new UserModel
            {
                FirstName = "Jane",
                LastName = "Smith Doe",
                Email = "new@domain.com",
                BirthDate = new DateTime(1988, 07, 03),
                Gender = Gender.Female,
                ProfileImage = "../img/profiles/234rewr23422.png"
            };
        }

        public UserModel GivenAnUserModelWithConflictingEmail()
        {
            return new UserModel
            {
                FirstName = "Jane",
                LastName = "Smith Doe",
                Email = "conflict@domain.com",
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

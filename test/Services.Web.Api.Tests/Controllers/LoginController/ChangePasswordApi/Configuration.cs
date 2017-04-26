using System.Net.Http;
using System.Threading.Tasks;
using Template.Services.Web.Api.Tests.Base;
using Template.Services.Web.Api.Tests.Controllers.Base;

namespace Template.Services.Web.Api.Tests.Controllers.LoginController.ChangePasswordApi
{
    public class Configuration : BaseConfiguration
    {
        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
        }

        public async Task<HttpResponseMessage> WhenRequestingTheChangePasswordApiAsync(
            string userId = "",
            string pass = "",
            string newPass = "",
            string newPassConfirm = "")
        {
            return await RequestingApiAsync(new HttpMethod("PUT"), $"/v1/login/{userId}/{pass}/{newPass}/{newPassConfirm}");
        }

        #region Givens
        public string GivenAValidUserId()
        {
            return "wer23423";
        }

        public string GivenANotExistsUserId()
        {
            return "notfound";
        }

        public string GivenAnEmptyUserId()
        {
            return string.Empty;
        }

        public string GivenAValidPassword()
        {
            return "erw435435";
        }

        public string GivenAnEmptyPassword()
        {
            return string.Empty;
        }

        public string GivenAValidNewPassword()
        {
            return "435ewWwe3";
        }

        public string GivenAnEmptyNewPassword()
        {
            return string.Empty;
        }

        public string GivenAnEqualsNewPasswordConfirm()
        {
            return GivenAValidNewPassword();
        }

        public string GivenADifferentNewPasswordConfirm()
        {
            return "123";
        }

        public string GivenAnEmptyNewPasswordConfirm()
        {
            return string.Empty;
        }
        #endregion
    }
}

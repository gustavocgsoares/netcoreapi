using System.Net.Http;
using System.Threading.Tasks;
using Template.Services.Web.Api.Tests.Base;
using Template.Services.Web.Api.Tests.Controllers.Base;

namespace Template.Services.Web.Api.Tests.Controllers.LoginController.LoginApi
{
    public class Configuration : BaseConfiguration
    {
        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
        }

        public async Task<HttpResponseMessage> WhenRequestingTheLoginApiAsync(string email = "", string password = "")
        {
            return await RequestingApiAsync(new HttpMethod("GET"), $"/v1/login/{email}/{password}");
        }

        #region Givens
        public string GivenAValidEmail()
        {
            return "email@domain.com.br";
        }

        public string GivenAnInvalidEmail()
        {
            return "domain.com.br";
        }

        public string GivenAnEmptyEmail()
        {
            return string.Empty;
        }

        public string GivenAValidPassword()
        {
            return "erw435435ewWwe3##e$";
        }

        public string GivenAnEmptyPassword()
        {
            return string.Empty;
        }
        #endregion
    }
}

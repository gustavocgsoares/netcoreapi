using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Template.Application.Model.Contexts.V1.Corporate;
using Template.Application.Model.Enums.Base;
using Template.Services.Web.Api.Tests.Base;
using Xunit;

namespace Template.Services.Web.Api.Tests.Controllers.UsersController.CreateApi
{
    [Collection("TestContext")]
    public class CreateApiTests : Configuration
    {
        public CreateApiTests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Trait("CI", "")]
        [Trait("Api", "User Create")]
        [Trait("Controller", "Users")]
        [Fact]
        public async Task Should_return_user_created_with_right_data_sent()
        {
            ////Given
            var user = GivenAValidUserModel();

            ////When
            var response = await WhenRequestingTheCreateUserApiAsync(user);
            var result = await WhenGetContentAsModelAsync<UserModel>(response);

            ////Then
            var lnkDefault = "http://localhost/v1/users/";

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Should().NotBeNull();
            result.Links.Should().NotBeNullOrEmpty();
            result.Links.Should().HaveCount(3);

            response.Headers.Location.Should().NotBeNull();
            response.Headers.Location.ToString().Should().Be($"{lnkDefault}{result.Id}");

            var lnkUserById = result.Links.FirstOrDefault(lnk => lnk.Relation == "user_by_id");
            lnkUserById.Should().NotBeNull();
            lnkUserById.Href.Should().StartWith($"{lnkDefault}{result.Id}");
            lnkUserById.Method.Should().Be(Method.GET);

            var lnkUserDelete = result.Links.FirstOrDefault(lnk => lnk.Relation == "delete_user");
            lnkUserDelete.Should().NotBeNull();
            lnkUserDelete.Href.Should().StartWith($"{lnkDefault}{result.Id}");
            lnkUserDelete.Method.Should().Be(Method.DELETE);

            var lnkUserUpdate = result.Links.FirstOrDefault(lnk => lnk.Relation == "update_user");
            lnkUserUpdate.Should().NotBeNull();
            lnkUserUpdate.Href.Should().StartWith($"{lnkDefault}{result.Id}");
            lnkUserUpdate.Method.Should().Be(Method.PUT);
        }

        [Trait("CI", "")]
        [Trait("Api", "User Create")]
        [Trait("Controller", "Users")]
        [Fact]
        public async Task Should_return_error_when_user_email_already_exists()
        {
            ////Given
            var user = GivenAnUserModelWithConflictingEmail();

            ////When
            var response = await WhenRequestingTheCreateUserApiAsync(user);

            ////Then
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
    }
}

using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Template.Application.Model.Contexts.V1.Corporate;
using Template.Application.Model.Enums.Base;
using Template.Services.Web.Api.Tests.Base;
using Xunit;

namespace Template.Services.Web.Api.Tests.Controllers.UsersController.GetByIdApi
{
    [Collection("TestContext")]
    public class GetByIdApiTests : Configuration
    {
        public GetByIdApiTests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Trait("CI", "")]
        [Trait("Api", "User Get by Id")]
        [Trait("Controller", "Users")]
        [Fact]
        public async Task Should_return_the_user_data_found()
        {
            ////Given
            var userId = await GivenAValidUserId();

            ////When
            var response = await WhenRequestingTheGetUserByIdApiAsync(userId);
            var result = await WhenGetContentAsModelAsync<UserModel>(response);

            ////Then
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
            result.Links.Should().NotBeNullOrEmpty();
            result.Links.Should().HaveCount(3);

            var lnkDefault = "http://localhost/v1/users/";

            var lnkUserById = result.Links.FirstOrDefault(lnk => lnk.Relation == "self");
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
    }
}

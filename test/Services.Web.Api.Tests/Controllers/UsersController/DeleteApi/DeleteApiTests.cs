using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Template.Services.Web.Api.Tests.Base;
using Xunit;

namespace Template.Services.Web.Api.Tests.Controllers.UsersController.DeleteApi
{
    [Collection("TestContext")]
    public class DeleteApiTests : Configuration
    {
        public DeleteApiTests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Trait("CI", "")]
        [Trait("Api", "User Delete")]
        [Trait("Controller", "Users")]
        [Fact]
        public async Task Should_return_success_with_right_data_sent()
        {
            ////Given
            var userId = await GivenAValidUserId();

            ////When
            var response = await WhenRequestingTheDeleteUserApiAsync(userId);

            ////Then
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}

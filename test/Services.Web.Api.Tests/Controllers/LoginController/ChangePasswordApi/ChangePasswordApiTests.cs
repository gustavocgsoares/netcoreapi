using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Template.Services.Web.Api.Tests.Base;
using Xunit;

namespace Template.Services.Web.Api.Tests.Controllers.LoginController.ChangePasswordApi
{
    [Collection("TestContext")]
    public class ChangePasswordApiTests : Configuration
    {
        public ChangePasswordApiTests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Trait("CI", "")]
        [Trait("Api", "Change Password")]
        [Trait("Controller", "Login")]
        [Fact]
        public async Task Should_return_success_with_right_data_sent()
        {
            ////Given
            var userId = GivenAValidUserId();
            var pass = GivenAValidPassword();
            var newPass = GivenAValidNewPassword();
            var newPassConfirm = GivenAnEqualsNewPasswordConfirm();

            ////When
            var response = await WhenRequestingTheChangePasswordApiAsync(userId, pass, newPass, newPassConfirm);

            ////Then
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Trait("CI", "")]
        [Trait("Api", "Change Password")]
        [Trait("Controller", "Login")]
        [Fact]
        public async Task Should_return_error_when_user_not_exists()
        {
            ////Given
            var userId = GivenANotExistsUserId();
            var pass = GivenAValidPassword();
            var newPass = GivenAValidNewPassword();
            var newPassConfirm = GivenAnEqualsNewPasswordConfirm();

            ////When
            var response = await WhenRequestingTheChangePasswordApiAsync(userId, pass, newPass, newPassConfirm);

            ////Then
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Trait("CI", "")]
        [Trait("Api", "Change Password")]
        [Trait("Controller", "Login")]
        [Fact]
        public async Task Should_return_error_when_new_pass_is_different_from_the_confirmation()
        {
            ////Given
            var userId = GivenANotExistsUserId();
            var pass = GivenAValidPassword();
            var newPass = GivenAValidNewPassword();
            var newPassConfirm = GivenADifferentNewPasswordConfirm();

            ////When
            var response = await WhenRequestingTheChangePasswordApiAsync(userId, pass, newPass, newPassConfirm);

            ////Then
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}

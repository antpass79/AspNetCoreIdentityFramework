using Globe.Identity.Models;
using Globe.Identity.Security;
using Globe.Identity.Services;
using Globe.Tests.Mocks;
using Xunit;

namespace Globe.Identity.Tests
{
    public class LoginServiceTests
    {
        [Fact]
        async public void LoginWithNotExistsCredentials()
        {
            var userManager = FakeUserManager.Mock();
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = TestConstants.USER_NAME_user3,
                Password = TestConstants.PASSWORD_user3ETemailDOTcom
            });

            Assert.False(loginResult.Successful);
            Assert.Equal(TestConstants.RETURN_VALUE_Invalid_credentials, loginResult.Error);
        }

        [Fact]
        async public void LoginWithInvalidCredentials()
        {
            var userManager = FakeUserManager.Mock();
            FakeUserManager.FillWithMockUsers(userManager);
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = TestConstants.USER_NAME_user2,
                Password = TestConstants.PASSWORD_user2ETpasswordNotValid
            });

            Assert.False(loginResult.Successful);
            Assert.Equal(TestConstants.RETURN_VALUE_Invalid_credentials, loginResult.Error);
        }

        [Fact]
        async public void LoginWithValidCredentials()
        {
            var userManager = FakeUserManager.Mock();
            FakeUserManager.FillWithMockUsers(userManager);
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            var loginResult = await loginService.LoginAsync(new Credentials
            {
                UserName = TestConstants.USER_NAME_user2,
                Password = TestConstants.PASSWORD_user2ETpassword
            });

            Assert.True(loginResult.Successful);
            Assert.NotNull(loginResult.Token);
            Assert.NotEmpty(loginResult.Token);
        }

        [Fact]
        async public void LogoutWithNotExistsUser()
        {
            var userManager = FakeUserManager.Mock();
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            await loginService.LogoutAsync();

            Assert.True(false);
        }

        [Fact]
        async public void LogoutALoggedUser()
        {
            var userManager = FakeUserManager.Mock();
            FakeUserManager.FillWithMockUsers(userManager);
            var signInManager = FakeSignInManager.Mock(userManager);
            var jwtIssuerOptions = new MockJwtAuthenticationOptions().Mock();
            var jwtTokenEncoder = new JwtTokenEncoder<GlobeUser>(userManager, jwtIssuerOptions);

            var loginService = new LoginService(userManager, signInManager, jwtIssuerOptions, jwtTokenEncoder);
            await loginService.LoginAsync(new Credentials
            {
                UserName = TestConstants.USER_NAME_user2,
                Password = TestConstants.PASSWORD_user2ETpasswordNotValid
            });

            var loggedUser = await userManager.FindByNameAsync(TestConstants.USER_NAME_user2);

            await loginService.LogoutAsync();

            var notLoggedUser = await userManager.FindByNameAsync(TestConstants.USER_NAME_user2);

            Assert.True(loggedUser != null);
            Assert.True(false);
        }
    }
}
